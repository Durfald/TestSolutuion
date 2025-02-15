<template>
  <div class="cart-card" style="height:fit-content;display:block;margin-bottom:10px;">
    <button class="btn-accent" @click="openPopup()">Добавить пользователя</button>
  </div>
  <div class="user-list">
    <div v-for="userData in combinedData" :key="userData.user.id" class="user-card">
      <label>
        Имя:
        <input v-model="userData.user.username" />
      </label>
      <label>
        Роль:
        <select v-model="userData.user.role">
          <option v-for="role in roles" :key="role" :value="role">{{ role }}</option>
        </select>
      </label>
      <label v-if="userData.customer">
        Скидка:
        <input type="number" min="0" max="99" v-model="userData.customer.discount" @input="validateDiscount(userData.customer)" />
      </label>
      <label>
        Адрес:
        <input v-if="userData.customer" v-model="userData.customer.address" />
      </label>
      <div style="display:flex;gap:1rem;justify-content:space-evenly;">
        <button class="btn" @click="Update(userData)">Сохранить</button>
        <button class="btn" @click="Delete(userData)">Удалить</button>
      </div>
    </div>
  </div>

  <Popup ref="popup">
    <div class="user-card" style="box-shadow:none;margin:10px;">
      <label>
        Имя:
        <input v-model="tempUser.username" />
      </label>
      <label>
        Пароль:
        <input v-model="tempUser.password" />
      </label>
      <label>
        Роль:
        <select v-model="tempUser.role">
          <option v-for="role in roles" :key="role" :value="role">{{ role }}</option>
        </select>
      </label>
      <button class="btn" @click="addUser()">Создать</button>
    </div>
  </Popup>
</template>

<script lang="ts" setup>
  import { ref, computed, onMounted} from 'vue';
  import { useStore } from 'vuex'; // Используем Vuex для поиска
  import Popup from '@/components/Popup.vue';
  import ApiService from '@scripts/ApiService.ts';
  import { useNotification } from '@/components/Notification.vue';
  const { showNotification } = useNotification();
  const srv: ApiService = new ApiService();
  const store = useStore();
  var customers = ref([]);
  const popup = ref(null);
  var users = ref([]);
  const roles = ref(["Manager", "User"]);
  const selectedRole = ref<string>('');
  const combinedData = ref([]);

  const tempUser = ref({ username: "", role: "User",password: "" });

  function openPopup() {
    popup.value.open();
  }

  async function addUser() {
    try {
      // Создаем пользователя
      const createdUser = await srv.CreateUser(tempUser.value);
      

      if (createdUser ) {
        showNotification({ type: 'success', message: 'Пользователь добавлен', time: 4000 });


        // Очищаем временные переменные
        tempUser.value = { username: "", role: "User" };

        await loadData();
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

  async function Update(userData) {
    try {

      var user_res = await srv.UpdateUser(userData.user.id, userData.user);
      var customer_res = await srv.UpdateCustomer(userData.customer.id, userData.customer);
      if (user_res == false || customer_res == false) {
        showNotification({ type: 'warning', message: 'Произошла ошибка', time: 4000 });
      } else {
        showNotification({ type: 'success', message: 'Пользователь обновлен', time: 4000 });
      }
    } catch (error) {
      showNotification({ type: 'warning', message: 'Произошла ошибка', time: 4000 });
    }
  }

  async function Delete(userData) {
    try {
      var user_res = await srv.DeleteUser(userData.user.id);
      var customer_res = await srv.DeleteCustomer(userData.customer.id);
      if (user_res == false || customer_res == false) {
        showNotification({ type: 'warning', message: 'Произошла ошибка', time: 4000 });
      } else {
        showNotification({ type: 'success', message: 'Пользователь удален', time: 4000 });
        combinedData.value = combinedData.value.filter(item => item.user.id !== userData.user.id);
      }
    } catch (error) {
      showNotification({ type: 'warning', message: 'Произошла ошибка', time: 4000 });
    }

  }

  const loadData = async () => {
    try {
      // Загружаем данные о пользователях и клиентах одновременно
      const [usersData, customersData] = await Promise.all([
        srv.GetAllUsers(),
        srv.GetAllCustomers(),
      ]);

      // Присваиваем данные в соответствующие переменные
      users.value = usersData;
      customers.value = customersData;

      // После загрузки данных объединяем их
      combineData();
    } catch (error) {
      console.error('Ошибка при загрузке данных:', error);
    }
  };

  const combineData = () => {
    const currentUserId = localStorage.getItem("Id"); // Получаем ID текущего пользователя
    combinedData.value = users.value
      .filter(user => user.id !== currentUserId) // Исключаем текущего пользователя
      .map(user => {
        // Ищем customer по id
        const customer = customers.value.find(customer => customer.id === user.id);

        if (customer && customer.discount === null) {
          customer.discount = 0;
        }

        return { user, customer }; // Возвращаем новый объект с user и customer
      });
  };

  const validateDiscount = (customer: any) => {
    if (customer.discount === null || customer.discount === '') {
      customer.discount = 0; // Если поле пустое, устанавливаем скидку в 0
    }
    if (customer.discount < 0) {
      customer.discount = 0;
    }
    if (customer.discount > 99) {
      customer.discount = 99;
    }
  };

  onMounted(async () => {
    loadData();
  });
</script>

<style>
  .user-list {
    display: flex;
    flex-wrap: wrap;
    gap: 20px;
  }

  .user-card {
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
    gap: 1rem;
    font-size: 20px;
  }

    .user-card input {
      outline: none;
      border: none;
      border-bottom: 1px solid gray;
      padding: 3px 10px;
      text-align: center;
      width: -webkit-fill-available;
      margin: 0px 10px;
      font-size: 15px;
    }

    .user-card label {
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
