import { createStore } from 'vuex';
interface CartItem {
  id: string;
  name: string;
  price: number;
  category: string;
  quantity: number;
}

interface State {
  role: string | null;
  JWToken: string | null;
  cart: CartItem[];
  selectedCategories: string[];
  username: string | null;
  searchQuery: string;
  id: string | null;
}

const store = createStore<State>({
  state: {
    JWToken: localStorage.getItem('JWToken') || null,
    role: localStorage.getItem('Role') || null, // Загружаем роль из localStorage
    cart: sessionStorage.getItem('Cart') ? JSON.parse(sessionStorage.getItem('Cart') as string) : [],
    selectedCategories: [],
    username: localStorage.getItem('Username') || null,
    searchQuery: '',
    id: localStorage.getItem('Id') || null
  },
  mutations: {
    setId(state: State, id: string) {
      state.id = id;
      localStorage.setItem('Id', id);
    },
    deleteId(state: State) {
      state.id = null;
      localStorage.removeItem('Id');
    },
    // Мутация для изменения роли
    setRole(state: State, role: string) {
      state.role = role;
      localStorage.setItem('Role', role); // Сохраняем роль в localStorage
    },
    deleteeRole(state: State) {
      state.role = null;
      localStorage.removeItem('Role');
    },
    setJWToken(state: State, JWToken: string) {
      state.JWToken = JWToken;
      localStorage.setItem('JWToken', JWToken);
    },
    deleteJWToken(state: State) {
      state.JWToken = null;
      localStorage.removeItem('JWToken');
    },
    addToCart(state: State, product: CartItem) {
      const existingProduct = state.cart.find(item => item.id === product.id);
      if (existingProduct) {
        if (existingProduct.quantity < 20) {
          existingProduct.quantity += 1; // Увеличиваем количество
        }
      } else {
        state.cart.push(product); // Добавляем новый товар
      }
      sessionStorage.setItem('Cart', JSON.stringify(state.cart));
    },
    removeFromCart(state: State, productId: string) {
      state.cart = state.cart.filter(item => item.id !== productId);
      sessionStorage.setItem('Cart', JSON.stringify(state.cart));
    },
    decreaseItem(state: State, product: CartItem) {
      const existingProduct = state.cart.find(item => item.id === product.id);
      if (existingProduct && existingProduct.quantity > 1) {
        existingProduct.quantity -= 1; // Уменьшаем количество

        sessionStorage.setItem('Cart', JSON.stringify(state.cart));
      }
    },
    clearCart(state: State) {
      state.cart = [];
      sessionStorage.removeItem('Cart');
    },
    setSelectedCategory(state: State, category: string | null) {
      if (category) {
        state.selectedCategories.push(category);
      }
    },
    SetUsername(state: State, username: string) {
      state.username = username;
      localStorage.setItem('Username', username);
    },
    DeleteUsername(state: State) {
      state.username = null;
      localStorage.removeItem('Username');
    },
    setSearchQuery(state, query) {
      state.searchQuery = query;
    },
  },
  actions: {
    updateSearchQuery({ commit }, query:string) {
      commit('setSearchQuery', query);
    },
    // Действие для установки роли
    setRole({ commit }, role: string) {
      commit('setRole', role);
    },
    deleteRole({ commit }) {
      commit('deleteeRole');
    },
    setId({ commit }, id: string) {
      commit('setId', id);
    },
    deleteId({ commit }) {
      commit('deleteId');
    },
    setJWToken({ commit }, JWToken: string) {
      commit('setJWToken', JWToken);
    },
    deleteJWToken({ commit }) {
      commit('deleteJWToken');
    },
    addToCart({ commit }, product: CartItem) {
      commit('addToCart', product);
    },
    removeFromCart({ commit }, productId: number) {
      commit('removeFromCart', productId);
    },
    decreaseProduct({ commit }, product: CartItem) {
      commit('decreaseItem', product);
    },
    clearCart({ commit }) {
      commit('clearCart');
    },
    setSelectedCategory({ commit }, category: string | null) {
      commit('setSelectedCategory', category);
    },
    setUsername({ commit }, username: string) {
      commit('SetUsername', username);
    },
    deleteUsername({ commit }) {
      commit('DeleteUsername');
    },
  },
  getters: {
    // Геттер для получения роли
    getRole(state: State) {
      return state.role;
    },
    getJWToken(state: State) {
      return state.JWToken;
    },
    getCart(state: State) {
      return state.cart; // Получение всех товаров в корзине
    },
    getTotalPrice(state: State) {
      return state.cart.reduce((total, item) => total + item.price * item.quantity, 0);
    },
    getTotalItems(state: State) {
      return state.cart.reduce((total, item) => total + item.quantity, 0);
    },
    getCartByCategory(state: State) {
      if (state.selectedCategories) {
        return state.cart.filter(item => state.selectedCategories.includes(item.category));
      }
      return state.cart; // Если категория не выбрана, возвращаем все товары
    },
    getUsername(state: State) {
      return state.username;
    },
    getsearchQuery(state: State) {
      return state.searchQuery;
    },
    getId(state: State) {
      return state.id;
    }
  },
});

export default store;
