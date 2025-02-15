<template>
  <header class="header">
    <div style="display:flex;gap:15px;">
      <router-link v-if="role !== 'Manager'" to="/">
        <button class="header-btn-accent">Каталог</button>
      </router-link>
      <router-link v-if="role === 'Manager'" to="/Users">
        <button class="header-btn">Пользователи</button>
      </router-link>
      <router-link v-if="role === 'Manager'" to="/Products">
        <button class="header-btn">Товары</button>
      </router-link>
      <router-link v-if="role === 'Manager'" to="/Historylist">
        <button class="header-btn">Заказы</button>
      </router-link>
    </div>
    <!-- Центральная часть -->
    <div class="header-center">
      <input v-model="searchQuery" @input="onSearch"  type="text" class="search-input" placeholder="Поиск..." />
    </div>

    <!-- Правая часть -->
    <div class="header-right">
      

      <router-link to="/History" v-if="role !== 'Manager' && username">
        <button class="header-btn">История заказов</button>
      </router-link>
      <router-link to="/Cart" v-if="role !== 'Manager' && username">
        <button class="header-btn">Корзина</button>
      </router-link>

      <div class="div user-info" v-if="username">
        <p class="username">{{username}}</p>
        <router-link to="/Login">
          <button class="header-btn-accent" @click="logout">Выйти</button>
        </router-link>
      </div>
      <router-link v-else to="/Login">
        <button class="header-btn-accent">Войти</button>
      </router-link>
    </div>
  </header>
</template>

<script lang="ts" setup>
  import router from './router';
  import { ref, computed } from 'vue'
  import { useStore } from 'vuex';
  import ApiService from '@scripts/ApiService.ts';

  const srv = new ApiService();
  const store = useStore();

  const username = computed(()=>store.getters.getUsername);
  const role = computed(() => store.getters.getRole);

  const logout = () => {
    srv.logout(); // Вызываем action для выхода
   
  };
  const searchQuery = ref('');
  const onSearch = () => {
    store.dispatch('updateSearchQuery', searchQuery.value);
   
  };
</script>

<style scoped>
  /* Стили для Header */
  .header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    background-color: #ffffff; /* Цвет фона */
    color: #000000; /* Цвет текста */
    padding: 10px 20px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  }

  /* Левая часть */
  .header-right {
    display: flex;
    align-items: center;
    gap: 10px;
  }

  /* Общие стили для кнопок */
  .header-btn,
  .header-btn-accent {
    width: auto;
    padding: 10px 20px;
    font-size: 14px;
    border-radius: 5px;
    cursor: pointer;
    border: none;
    box-sizing: border-box;
    font-weight: bold;
    text-align: center;
    transition: background-color 0.3s ease;
  }

  /* Кнопки слева */
  .header-btn {
    background-color: #ffffff; /* Цвет фона */
    outline: 2px solid #14213d; /* Цвет компонента */
    outline-offset: -2px;
    color: #14213d; /* Цвет текста */
  }

    .header-btn:hover {
      background-color: #fca311; /* Цвет акцента */
      color: #ffffff; /* Цвет текста */
    }

  /* Кнопка справа */
  .header-btn-accent {
    background-color: #fca311; /* Цвет акцента */
    color: #ffffff; /* Цвет текста */
  }

    .header-btn-accent:hover {
      background-color: #e58e0d; /* Более темный оттенок акцентного цвета */
    }

  /* Центральная часть */
  .header-center {
    flex-grow: 1;
    max-width: 400px;
    margin: 0 20px;
  }

  .search-input {
    width: 100%;
    padding: 10px;
    font-size: 14px;
    border: 1px solid #e5e5e5; /* Цвет фона элемента */
    border-radius: 5px;
    background-color: #e5e5e5; /* Цвет фона элемента */
    color: #000000; /* Цвет текста */
  }
  .username {
    font-size: 16px;
    margin-right: 10px;
  }

  .user-info{
    display: flex;
    align-items: center;
  }
    .search-input:focus {
      outline: none;
      border-color: #fca311; /* Цвет акцента */
    }
</style>
