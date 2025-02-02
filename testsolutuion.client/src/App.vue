<template>
  <div class="app-container">
    <Header />
    <main class="content">
      <router-view />
    </main>
  </div>
  <NotificationComponent />
</template>

<script>
  import Header from "./components/Header.vue";
  import NotificationComponent from '@/components/Notification.vue';

  export default {
    components: {
      Header,
    NotificationComponent
    },
  };
</script>

<style>
  /* Устанавливаем контейнер на всю ширину и высоту страницы */
  html,
  body,
  #app {
    margin: 0;
    padding: 0;
    width: 100%;
    height: 100%;
    font-family: Arial, sans-serif;
  }

  .app-container {
    display: flex;
    flex-direction: column;
    width: 100%;
    height: 100%;
  }

  /* Стили для основного контента */
  .content {
    flex-grow: 1; /* Контент занимает оставшееся пространство */
    background-color: #e5e5e5; /* Цвет фона контента */
    padding: 1rem;
  }
</style>
