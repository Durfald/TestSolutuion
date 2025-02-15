<template>
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
</template>

<script lang="ts" setup>

</script>

<style>

</style>
