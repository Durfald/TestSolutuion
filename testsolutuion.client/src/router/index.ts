import { createRouter, createWebHistory } from 'vue-router';
import login from '@/views/Login.vue';
import StartPage from '@/views/StartPage.vue';
import CartPage from '@/views/Cart.vue';
import HistoryPage from '@/views/OrderHistory.vue';
import UsersPage from '@/views/UserList.vue';
import ProductsPage from '@/views/ProductList.vue';
import HistoryListPage from '@/views/HistoryList.vue';

const routes = [
  { path: '/login', component: login },
  { path: '/', component: StartPage },
  { path: '/cart', component: CartPage },
  { path: '/history', component: HistoryPage },
  { path: '/users', component: UsersPage },
  { path: '/products', component: ProductsPage },
  { path: '/historylist', component: HistoryListPage },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
