<template>
  <div class="notification" v-show="notifications.length">
    <div v-for="(notification, index) in notifications" :key="index" class="notification-item" :style="{ color: notification.color }">
      <i :class="[notification.icon, { 'fa-bounce': notification.bounce }]"></i>
      <span>{{ notification.message }}</span>
    </div>
  </div>
</template>

<script>
  import { reactive, readonly } from 'vue';

  const state = reactive({ notifications: [] });

  export function useNotification() {
    function showNotification(data) {
      let color = '';
      switch (data.type) {
        case 'success':
          data.icon = 'fa-solid fa-circle-check';
          color = 'green';
          break;
        case 'error':
          data.icon = 'fa-solid fa-circle-exclamation';
          color = 'red';
          console.log(data.error_message || 'Сообщения нет по приколу');
          break;
        case 'warning':
          data.icon = 'fa-solid fa-triangle-exclamation';
          color = 'orange';
          break;
        case 'info':
          data.icon = 'fa-solid fa-circle-info';
          color = 'blue';
          break;
        default:
          data.icon = 'fa-solid fa-poo';
          color = 'black';
          break;
      }

      // Добавляем новое уведомление
      const notification = reactive({
        message: data.message,
        icon: data.icon,
        color: color,
        bounce: true // свойство для анимации
      });

      state.notifications.unshift(notification);

      // Убираем класс 'fa-bounce' через 1 секунду
      setTimeout(() => {
        notification.bounce = false; // изменяем реактивное свойство
      }, 1000);

      // Удаление уведомления через 3 секунды
      setTimeout(() => {
        state.notifications.shift();
      }, data.time);
    }

    return {
      notifications: readonly(state.notifications),
      showNotification
    };
  }

  export default {
    setup() {
      return { notifications: state.notifications };
    }
  };
</script>

<style>
  .notification {
    position: fixed;
    bottom: 20px;
    right: 20px;
    padding: 10px;
    z-index: 3;
    display: flex;
    flex-direction: column;
    overflow: auto;
    max-height: 50%;
    align-items: flex-end;
  }

    .notification::-webkit-scrollbar {
      width: 0px;
      height: 0px;
    }

    .notification .notification-item {
      margin: 5px 0;
      align-items: center;
      display: flex;
      border: 1px solid #ccc;
      padding: 10px;
      border-radius: 6px;
      font-size: 17px;
      background: #ffffff;
      width: fit-content;
    }

  .notification-item span {
    margin-left: 7px;
  }
</style>
