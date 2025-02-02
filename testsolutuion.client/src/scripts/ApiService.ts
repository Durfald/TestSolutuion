import { useStore } from 'vuex';

export default class ApiService {
  baseUrl: string;

  store = useStore();
  constructor(baseUrl: string|null = null) {
    this.baseUrl = baseUrl || 'https://localhost:7019/'; // Укажите URL вашего ASP.NET API
  }

  async request(endpoint: string, options: any = {}) {
    const url = `${this.baseUrl}${endpoint}`;
    const response = await fetch(url, {
      headers: {
        'Content-Type': 'application/json',
        ...options.headers,
      },
      ...options,
    });

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    return response.json();
  }

  // Пример авторизации
   async login(credentials = {}) {

    var response = await this.request('Auth/Login', {
      method: 'POST',
      body: JSON.stringify(credentials),
      credentials: 'include'
    });
     this.store.dispatch('setUsername', credentials.Username);
     this.store.dispatch('setRole', response.role);
     this.store.dispatch('setJWToken', response.token);
  }

  async getAllProducts() {
    const response = await this.request('Product/GetAllProducts');
    return response;
  }

  async getCategories() {
    const response = await this.request('Product/GetCategories');
    return response;
  }

  logout() {
    this.store.dispatch('deleteUsername');
    this.store.dispatch('deleteRole');
    this.store.dispatch('deleteJWToken');
  }
}
