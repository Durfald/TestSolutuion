<template>
  <div class="main-start-page">
    <div class="accordion-filter">
      <Accordion class="accordion" :multiple="true">
        <AccordionTab header="Цена">
          <div class="price-filter">
            <input type="number"
                   v-model="minPrice"
                   :placeholder="`от ${minProductPrice}`"
                   class="price-input"
                   @input="validatePriceInput($event, 'min')"/>
            <input type="text"
                   v-model="number"
                   :placeholder="`до ${maxProductPrice}`"
                   class="price-input"
                   @input="validatePriceInput($event, 'max')"/>
          </div>
        </AccordionTab>
        <AccordionTab style="border-radius: 8px" header="Категория">
          <div style="padding-bottom: 10px;">
            <div class="ui-input-search">
              <input class="ui-input-search-input" v-model="categoryInput" placeholder="Поиск"/>
            </div>
            <div class="ui-ckeckbox-group" v-for="category in filteredCategories">
              <label class="ui-checkbox">
                <input type="checkbox" v-model="category.selected" :binary="true"/>
                <span>{{category.name}}<span style="color:dimgray;"> ({{category.count}})</span></span>
              </label>
            </div>
          </div>
        </AccordionTab>
      </Accordion>
    </div>
    <div>
      <div class="product-list">
        <div class="product-card"
             v-for="product in paginatedProducts">
          <h3>{{ product.name }}</h3>
          <p>{{ product.category }}</p>
          <p>{{ product.price }} руб.</p>
          <button class="btn-accent" @click="addToCart(product)">Добавить в корзину</button>
        </div>
      </div>
      <div v-if="totalPages !== 1" class="pagination">
        <button class="btn" @click="prevPage" :disabled="currentPage === 1">Предыдущая</button>
        <span>Страница {{ currentPage }} из {{ totalPages }}</span>
        <button class="btn" @click="nextPage" :disabled="currentPage === totalPages">Следующая</button>
      </div>
    </div>
  </div>
 
</template>


<script lang="ts" setup>
  import { ref, computed, watch, onMounted, onUnmounted  } from 'vue';
  import { useStore } from 'vuex'; // Используем Vuex для поиска
  import ApiService from '@scripts/ApiService.ts';
  import { useNotification } from '@/components/Notification.vue';
  import Accordion from 'primevue/accordion';
  import AccordionTab from 'primevue/accordiontab';
  import checkbox from 'primevue/checkbox';
  const { showNotification } = useNotification();
  const srv: ApiService = new ApiService();
  const store = useStore();

  interface Filter {
    minPrice: number | null;
    maxPrice: number | null;
  }

  const filter: Filter = {
    minPrice: null,
    maxPrice: null,
  };

  const minPrice = ref<number | null>(null);
  const maxPrice = ref<number | null>(null);

  const loadedCategories = ref([]);

  const categoryInput = ref<string | null>(null);
  const filteredCategories = computed(() => {
    if (!categoryInput.value) {
      return loadedCategories.value; // Если поле ввода пустое, показываем все категории
    }
    var q = loadedCategories.value.filter(category =>
      category.name?.toLowerCase().includes(categoryInput.value?.toLowerCase().trim() || '')
    );
    return q;
  });
  const minProductPrice = computed(() => {
    return Math.min(...products.value.map(product => product.price));
  });

  const maxProductPrice = computed(() => {
    return Math.max(...products.value.map(product => product.price));
  });

  const validatePriceInput = (event, type) => {
    let value = event.target.value.replace(/\D/g, '');
    if (type === 'min') {
      minPrice.value = value ? parseInt(value) : null;
    } else {
      maxPrice.value = value ? parseInt(value) : null;
    }
  };


  const products = ref([]);
  const currentPage = ref(1);
  const itemsPerPage = 15;
  let intervalId = null;
  // Получаем поисковый запрос из Vuex
  const searchQuery = computed(() => store.getters.getsearchQuery);

  // Вычисляем общее количество страниц (включая фильтрацию)
  const totalPages = computed(() => {
    const filtered = filteredProducts.value;
    return filtered.length > 0 ? Math.ceil(filtered.length / itemsPerPage) : 1;
  });

  // Фильтруем товары по запросу из Vuex
  const filteredProducts = computed(() => {
    let filtered = products.value;

    if (searchQuery.value) {
      const query = searchQuery.value.toLowerCase();
      filtered = filtered.filter(product =>
        product.name.toLowerCase().includes(query) ||
        product.category.toLowerCase().includes(query)
      );
    }

    // Фильтрация по цене
    if (minPrice.value !== null) {
      filtered = filtered.filter(product => product.price >= minPrice.value);
    }

    if (maxPrice.value !== null) {
      filtered = filtered.filter(product => product.price <= maxPrice.value);
    }

    const selectedcategories = loadedCategories.value.filter(item => item.selected);
    if (selectedcategories && selectedcategories.length > 0) {
      filtered = filtered.filter(product =>
        selectedcategories.some(category => category.name === product.category)
      );
    }

    return filtered;
  });

  // Пагинируем отфильтрованные товары
  const paginatedProducts = computed(() => {
    const start = (currentPage.value - 1) * itemsPerPage;
    const end = start + itemsPerPage;
    return filteredProducts.value.slice(start, end);
  });

  // Добавление товара в корзину (для примера)
  const addToCart = (product) => {
    const cartItem = {
      id: product.id,  // Уникальный идентификатор товара
      name: product.name,  // Имя товара
      price: product.price,  // Цена товара
      category: product.category,  // Категория товара
      quantity: 1  // Количество товара в корзине, по умолчанию 1
    };
    const _product = store.getters.getCart.find(x=>x.id==product.id);
    if(_product && _product.quantity >=20){
      showNotification({ type: 'warning', message:'больше 20 товаров одного вида нельзя',time: 4000 });
      return;
    }
    store.dispatch('addToCart', cartItem);
    showNotification({ type: 'success', message: product.name+' добавлен в корзину',time: 2000 });
    console.log(store.getters.getCart);
  };

  // Переход на предыдущую страницу
  const prevPage = () => {
    if (currentPage.value > 1) {
      currentPage.value--;
    }
  };

  // Переход на следующую страницу
  const nextPage = () => {
    if (currentPage.value < totalPages.value) {
      currentPage.value++;
    }
  };

  // Загружаем продукты с сервера
  const loadProductsAsync = async () => {
    products.value = await srv.getAllProducts();
  };

  const loadCategoriesAsync = async () => {
    loadedCategories.value = await srv.getCategories();
  };

  // Загружаем данные при монтировании компонента
  onMounted(async () => {
    loadProductsAsync(); // Делаем первый запрос сразу
    loadCategoriesAsync();
    intervalId = setInterval(() => {// Повторяем каждые 20 секунд

    loadCategoriesAsync();
    loadProductsAsync(); 
  }, 1000 * 20);
  });
  onUnmounted(() => {
  clearInterval(intervalId);
});
  // Следим за изменениями в поисковом запросе и сбрасываем страницу на 1, если изменяется запрос
  watch([searchQuery,minPrice,maxPrice], () => {
    currentPage.value = 1;
  });
</script>

<style scoped>

  .ui-checkbox {
    font-size: 14px;
    padding: 6px 0px 6px 15px;
    transition: .3s;
    cursor: pointer;
    display: block;
  }

  .ui-ckeckbox-group{
      max-height: 280px;
      border-radius: 6px;
      margin: 1px;
      overflow-y: auto;
      overflow-x: hidden;
      transition: .5s;
  }

    .ui-ckeckbox-group:hover {
      background-color: rgba(252, 163, 17,.5)
    }

    .ui-input-search {
      margin: 5px 15px 8px;
      background: white;
      border-radius: 8px;
      font-size: 15px;
      height: 40px;
      line-height: 40px;
      position: relative;
      transition: background .3s;
    }

  .ui-input-search-input {
    background: transparent;
    border: unset;
    border-radius: 8px;
    height: 34px;
    outline: 1px solid gray;
    padding: 0px 40px 0px 12px;
    transition: background .3s;
    width: 100%;
  }

  .main-start-page{
      display: flex;
      flex-direction: row;
      gap: 20px;
  }

  .accordion-filter {
    width: 13rem;
    min-width: 10rem;
    border-radius: 8px;
  }

  :deep(.p-accordion-header:hover) {
    background-color: #fca311 !important;
    transition: background-color 0.3s ease;
  }

  :deep(.p-accordion-header:hover .p-accordion-header-text) {
    color: white !important;
    transition: color 0.3s ease;
  }

  .price-filter {
    display: flex;
    flex-direction: row;
    gap: 10px;
    padding: 10px;
  }

  .price-input {
    width: 100%;
    padding: 5px;
    border: 1px solid #ccc;
    border-radius: 4px;
    -moz-appearance: textfield;
    outline: none;
    height: 34px;
  }

  .price-input::-webkit-outer-spin-button,
  .price-input::-webkit-inner-spin-button {
    -webkit-appearance: none;
    margin: 0;
  }

  .product-list {
    display: flex;
    flex-wrap: wrap;
    gap: 20px;
  }

  .product-card {
    width: 200px;
    padding: 10px;
    border: 1px solid #ddd;
    display: flex;
    justify-content: space-around;
    flex-direction: column;
    border-radius: 8px;
    background-color: white;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    text-align: center;
  }

    .product-card:hover {
      border: 1px solid #fca311;
    }

    .product-card h3 {
      font-size: 18px;
      margin: 0;
      overflow-wrap: break-word;
    }

    .product-card p {
      font-size: 14px;
      color: #555;
    }

  .pagination {
    text-align: center;
    margin-top: 20px;
  }

    .pagination button {
      padding: 5px 15px;
      margin: 0 10px;
    }
</style>
