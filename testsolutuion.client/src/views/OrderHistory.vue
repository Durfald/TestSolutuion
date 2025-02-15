<template>
  <div>
    <select v-model="selectedStatus" @change="filterOrders" class="status-filter">
      <option value="all">Все</option>
      <option value="Новый">Новый</option>
      <option value="Выполняется">Выполняется</option>
      <option value="Выполнен">Выполнен</option>
    </select>

    <div class="order-list">
      <div class="order-card" v-for="order in filteredOrders" :key="order.id">
        <div class="order-header">
          <div style="display:flex;flex-direction:column;gap:20px">
            <div>
              <div class="order-status" :class="getStatusClass(order.status)">
                {{ order.status }}
              </div>
              <button class="btn-dlt btn" v-if="order.status === 'Новый'" @click="DeleteOrder(order.id)" style="position:absolute;right:0;top:0;">
                Отменить
              </button>
            </div>
            <span>Заказ {{ order.orderNumber }} <span style="color: gray;">от {{ formatDate(order.orderDate) }}</span></span>
          </div>
        </div>
        <div class="order-body">
          <div v-for="element in order.orderElements" :key="element.id" class="order-summary">
            <span>{{ element.productName }} </span>
            <span style="text-align:end;"> {{ element.quantity }}</span>
          </div>
        </div>
        <div class="order-footer">
          <span>{{ order.price }} ₽</span>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
  import { ref, computed, onMounted } from 'vue';
  import ApiService from '@scripts/ApiService.ts';
  import { useNotification } from '@/components/Notification.vue';
  const { showNotification } = useNotification();
  const srv: ApiService = new ApiService();
  const OrderHistory = ref([]);
  const selectedStatus = ref("all");

  const loadHistory = async () => {
    OrderHistory.value = await srv.GetOrderHistory();
  };

  onMounted(loadHistory);

  const filteredOrders = computed(() => {
    if (selectedStatus.value === "all") return OrderHistory.value;
    return OrderHistory.value.filter(order => order.status === selectedStatus.value);
  });

  function formatDate(dateString: string) {
    const date = new Date(dateString);
    return date.toLocaleDateString('ru-RU', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
    });
  }

  async function DeleteOrder(orderid: string) {
    var req = await srv.DeleteOrder(orderid);
    if (req) {
      showNotification({ type: 'success', message: 'Ваш заказ успешно удален', time: 6000 });
      OrderHistory.value = OrderHistory.value.filter(order => order.id !== orderid);
    } else {
      showNotification({ type: 'warning', message: 'Ошибка при удалении заказа', time: 6000 });
    }
  }

  function getStatusClass(status: string) {
    switch (status) {
      case 'Выполнен':
        return 'status-completed';
      case 'Выполняется':
        return 'status-in-progress';
      default:
        return 'status-default';
    }
  }
</script>

<style scoped>
  .status-filter {
    margin-bottom: 1rem;
    padding: 8px;
    border-radius: 5px;
    border: 1px solid #ddd;
    width:fit-content;
  }

  .order-list {
    column-count: 3;
    width: fit-content;
  }

  .order-card {
    padding: 18px 15px;
    border: 1px solid #ddd;
    border-radius: 12px;
    box-shadow: 0 1px 2px #0000001a;
    background-color: white;
    width: 350px;
    height: fit-content;
    margin-bottom: 1rem;
    display: inline-block;
  }

  .order-header {
    display: flex;
    gap: 20px;
    align-items: center;
    position: relative;
  }

  .order-status {
    padding: 5px 10px;
    border-radius: 8px;
    font-weight: bold;
    text-align: center;
    width: fit-content;
  }

  .status-completed {
    background-color: green;
    color: white;
  }

  .status-in-progress {
    background-color: #ffa218;
    color: black;
  }

  .status-default {
    background-color: rgb(242, 237, 255);
    color: black;
  }

  .order-body {
    padding: 16px 20px 20px;
    display: flex;
    gap: 10px;
    flex-direction: column;
    column-gap: 20px;
  }

  .order-footer {
    text-align: end;
    font-size: 20px;
  }

  .order-summary {
    display: flex;
    border-bottom: 1px solid rgba(0, 0, 0, 0.2);
    justify-content: space-between;
  }
</style>
