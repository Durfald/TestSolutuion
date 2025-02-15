<template>
  <div class="cart-card" style="height:fit-content;display:block;margin-bottom:10px;">
    <button class="btn-accent" @click="openPopup()">Добавить товар</button>
  </div>

  <div class="product-list">
    <div v-for="product in products" :key="product.id" class="product-card">
      <label>
        Название:
        <input v-model="product.name" />
      </label>
      <label>
        Категория:
        <div class="custom-select">
          <input type="text" v-model="product.category" @focus="product.showDropdown = true" @input="filterCategories(product)" @blur="hideDropdown(product)" />
          <ul v-if="product.showDropdown" class="dropdown-list">
            <li v-for="category in product.filteredCategories" :key="category.name" @click="selectCategory(product, category.name)">
              {{ category.name }}
            </li>
          </ul>
        </div>
      </label>
      <label>
        Цена:
        <input type="number" min="1" v-model="product.price" @input="validatePrice(product)" />
      </label>
      <div style="display:flex;gap:1rem;justify-content:space-evenly;">
        <button class="btn" @click="Update(product)">Сохранить</button>
        <button class="btn" @click="Delete(product)">Удалить</button>
      </div>
    </div>
  </div>

  <Popup ref="popup">
    <div class="user-card" style="box-shadow:none;margin:10px;">
      <label>
        Название:
        <input v-model="tempProduct.name" />
      </label>
      <label>
        Цена:
        <input type="number" min="1" @input="validatePrice(tempProduct)" v-model="tempProduct.price" />
      </label>
      <label>
        Категория:
        <div class="custom-select">
          <input type="text" v-model="tempProduct.category" @focus="tempProduct.showDropdown = true" @input="filterCategories(tempProduct)" @blur="hideDropdown(tempProduct)" />
          <ul v-if="tempProduct.showDropdown" class="dropdown-list">
            <li v-for="category in tempProduct.filteredCategories" :key="category.name" @click="selectCategory(tempProduct, category.name)">
              {{ category.name }}
            </li>
          </ul>
        </div>
      </label>
      <button class="btn" @click="addProduct()">Создать</button>
    </div>
  </Popup>
</template>

<script lang="ts" setup>
  import { ref, onMounted } from 'vue';
  import { useStore } from 'vuex';
  import Popup from '@/components/Popup.vue';
  import ApiService from '@scripts/ApiService.ts';
  import { useNotification } from '@/components/Notification.vue';

  const { showNotification } = useNotification();
  const srv: ApiService = new ApiService();
  const store = useStore();
  const popup = ref(null);
  const products = ref([]);
  const loadedCategories = ref([]);
  const tempProduct = ref({ name: "", price: 0, category: "", showDropdown: false, Code: generateCode() });
  function generateCode(): string {
    const getRandomDigit = () => Math.floor(Math.random() * 10); // Число 0-9
    const getRandomLetter = () => String.fromCharCode(65 + Math.floor(Math.random() * 26)); // Буква A-Z

    return `${getRandomDigit()}${getRandomDigit()}-${getRandomDigit()}${getRandomDigit()}${getRandomDigit()}${getRandomDigit()}-${getRandomLetter()}${getRandomLetter()}${getRandomDigit()}${getRandomDigit()}`;
  }
  async function addProduct() {
    try {
      // Создаем пользователя
      const createdUser = await srv.CreateProduct(tempProduct.value);


      if (createdUser) {
        showNotification({ type: 'success', message: 'Пользователь добавлен', time: 4000 });


        // Очищаем временные переменные
        tempProduct.value = { name: "", price: 0, category: "", showDropdown: false };

        await loadProductsAsync();
        await loadCategoriesAsync();
        // Закрываем Popup
        popup.value.close();

      } else {
        showNotification({ type: 'warning', message: 'Ошибка при добавлении', time: 4000 });
      }
    } catch (error) {
      console.error("Ошибка при добавлении пользователя:", error);
      showNotification({ type: 'warning', message: 'Произошла ошибка', time: 4000 });
    }
  }

  function openPopup() {
    popup.value.open();
  }

  const loadProductsAsync = async () => {
    products.value = (await srv.getAllProducts()).map(product => ({
      ...product,
      showDropdown: false,
      filteredCategories: []
    }));
  };

  const loadCategoriesAsync = async () => {
    loadedCategories.value = await srv.getCategories();
  };

  function filterCategories(product) {
    if (!product.category) {
      product.filteredCategories = loadedCategories.value;
    } else {
      product.filteredCategories = loadedCategories.value.filter(category =>
        category.name.toLowerCase().includes(product.category.toLowerCase())
      );
    }
  }

  function selectCategory(product, categoryName) {
    product.category = categoryName;
    product.showDropdown = false;
  }

  function hideDropdown(product) {
    setTimeout(() => {
      product.showDropdown = false;
    }, 200);
  }

  async function Update(product) {
    try {
      const success = await srv.UpdateProduct(product.id, product);
      if (success) {
        showNotification({ type: 'success', message: 'Продукт обновлён!', time: 4000 });
      } else {
        showNotification({ type: 'error', message: 'Ошибка обновления продукта!', time: 4000 });
      }
    } catch (error) {
      showNotification({ type: 'error', message: 'Ошибка обновления продукта!', time: 4000 });
      console.error(error);
    }
  }

  async function Delete(product) {
    try {
      const success = await srv.DeleteProduct(product.id);
      if (success) {
        products.value = products.value.filter(p => p.id !== product.id);
        showNotification({ type: 'success', message: 'Продукт удалён!', time: 4000 });
      } else {
        showNotification({ type: 'error', message: 'Ошибка удаления продукта!', time: 4000 });
      }
    } catch (error) {
      showNotification({ type: 'error', message: 'Ошибка удаления продукта!', time: 4000 });
      console.error(error);
    }
  }

  const validatePrice = (product: any) => {
    if (product.price === null || product.price === '') {
      product.price = 0; // Если поле пустое, устанавливаем скидку в 0
    }
    if (product.price < 0) {
      product.price = 0;
    }
  };

  onMounted(async () => {
    await loadProductsAsync();
    await loadCategoriesAsync();
  });
</script>

<style>
  .custom-select {
    position: relative;
    width: 100%;
  }

  .custom-select input {
    width: 100%;
    padding: 8px;
    border: 1px solid #ccc;
    border-radius: 4px;
    background-color: #fff;
    font-size: 15px;
  }

  .dropdown-list {
    position: absolute;
    width: 100%;
    background: white;
    border: 1px solid #ccc;
    border-radius: 4px;
    max-height: 150px;
    overflow-y: auto;
    list-style: none;
    padding: 0;
    margin: 0;
    z-index: 10;
  }

  .dropdown-list li {
    padding: 8px;
    cursor: pointer;
  }

  .dropdown-list li:hover {
    background-color: #f0f0f0;
  }

  .product-list {
    display: flex;
    flex-wrap: wrap;
    gap: 20px;
  }

  .product-card {
    width: fit-content;
    padding: 10px;
    border: 1px solid #ddd;
    display: flex;
    justify-content: space-around;
    flex-direction: column;
    border-radius: 8px;
    background-color: white;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    text-align: center;
    font-size: 20px;
    gap: 1rem;
  }

  .product-card input {
    outline: none;
    border: none;
    border-bottom: 1px solid gray;
    padding: 3px 10px;
    text-align: center;
    width: -webkit-fill-available;
    margin: 0px 10px;
    font-size: 15px;
  }

  .product-card label {
    display: flex;
    justify-content: space-between;
  }

  select {
    width: 100%;
    padding: 8px;
    border: 1px solid #ccc;
    border-radius: 4px;
    background-color: #fff;
    margin: 0px 10px;
    font-size: 15px;
  }
</style>
