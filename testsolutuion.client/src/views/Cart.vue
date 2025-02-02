<template>
  <h2 style="margin:0px 0px 0px 15px;">Корзина</h2>
  <div class="cart-list">
    <div class="cart-card" v-for="data in cart">
      <div class="title">
        <p>{{data.name}}</p>
      </div>
      <div class="delete">
        <button @click="deleteProduct(data)"><i class="fa-solid fa-trash-can"></i></button>
      </div>
      <div class="count-buttons">
        <div class="counter">
          <button @click="decreaseItem(data)">-</button>
          <input type="number" min="1" max="20" v-model="data.quantity" @input="validateQuantity(data)" />
          <button @click="increaseItem(data)">+</button>
        </div>
      </div>
      <div class="price">
        <span>{{ getProductPrice(data) }} ₽</span>
      </div>
    </div>
    <div class="total-price" v-if="totalprice !== 0">
      <strong>Общая сумма: {{ totalprice }} ₽</strong>
    </div>
    <div v-if="totalprice === 0" class="cart-card" style="display:flex;justify-content:center;padding:60px">
      <p>Пока пусто</p>
    </div>
  </div>
</template>

<script lang="ts" setup>
  import { ref, computed } from 'vue';
  import { useStore } from 'vuex';
  import { useNotification } from '@/components/Notification.vue';

 const { showNotification } = useNotification();
  const store = useStore();
  const cart = computed(() => store.getters.getCart);
  const totalprice = computed(() => store.getters.getTotalPrice);

const getProductPrice = (product) => {
  return product.price * product.quantity;
};

  const deleteProduct = (product) =>{
    store.dispatch('removeFromCart', product.id);
    showNotification({ type: 'info', message: product.name + ' удален из корзины', time: 3000 });
  };
  const decreaseItem = (product) =>{
    if(product.quantity==1){
      deleteProduct(product);
    } else{
      store.dispatch('decreaseProduct',product);
    }

 };


 const validateQuantity = (product) => {
  if (product.quantity < 1) product.quantity = 1;
  if (product.quantity > 20) product.quantity = 20;
};


 const increaseItem = (product) =>{
   store.dispatch('addToCart',product);
 };
</script>
<style scoped>

  .counter {
    display: flex;
    align-items: center;
    gap: 5px;
  }

    .counter button {
      width: 30px;
      font-size: 20px;
      border: none;
      color: white;
      color: black;
      background-color:transparent;
      border-radius: 8px;
      cursor: pointer;
      transition: 0.2s;
    }

    .counter button:active {
      background: #afafaf;
    }

    .counter input {
      text-align: center;
      vertical-align: top;
      border: none;
      font-size: 16px;
      outline: none;
      width: 21px;
    }

    .counter input::-webkit-outer-spin-button,
    .counter input::-webkit-inner-spin-button {
      -webkit-appearance: none;
      margin: 0;
    }

  .count-buttons{
      grid-area: quantity;
      height: 25px;
      border: 1px solid #afafaf;
      border-radius: 8px;
      width: fit-content;
  }
  .cart-list{
    display: flex;
    flex-direction: column;
    gap: 1rem;
  }

  .cart-card {
    padding: 18px 15px;
    border: 1px solid #ddd;
    border-radius: 12px;
    box-shadow: 0 1px 2px #0000001a;
    background-color: white;
    display: grid;
    column-gap: 12px;
    grid-template-columns: auto minmax(0%,1fr) auto;
    grid-template-areas:
        "rem title delete"
        "rem quantity price";
  }

  .title {
      grid-area: title;
      font-size: 20px;
  }

  .delete{
      grid-area: delete;
  }

  .delete button {
        padding: 7px 8px;
      border: unset;
      background-color: white;
      border-radius: 6px;
  }

  .delete button:hover {
        background-color: #cecece;
        border: none;
        border-radius: 6px;
  }

  .total-price {
    margin-top: 15px;
    font-size: 18px;
  }

  .price{
      grid-area: price;
  }
</style>
