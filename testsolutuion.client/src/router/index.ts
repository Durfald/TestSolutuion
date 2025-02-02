import { createRouter, createWebHistory } from 'vue-router';
import login from '@/views/Login.vue';
import StartPage from '@/views/StartPage.vue';
import CartPage from '@/views/Cart.vue';
const routes = [
  { path: '/login', component: login },
  { path: '/', component: StartPage },
  { path: '/cart', component: CartPage },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
