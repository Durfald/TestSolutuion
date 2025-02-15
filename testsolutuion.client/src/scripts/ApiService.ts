import { useStore } from 'vuex';

export default class ApiService {
  baseUrl: string;

  store = useStore();
  constructor(baseUrl: string|null = null) {
    this.baseUrl = baseUrl || 'https://localhost:7019/'; // Укажите URL вашего ASP.NET API
  }

  async request(endpoint: string, options: any = {},neededdata=true) {
    const url = `${this.baseUrl}${endpoint}`;
    const response = await fetch(url, {
      headers: {
        'Content-Type': 'application/json',
        ...options.headers,
      },
      ...options,
    });

    if (!response.ok) {
      console.log(`HTTP error! status: ${response.status}`);
    }
    if (neededdata) {
      return response.json();
    }
    return response.ok;
  }

  // Пример авторизации
   async login(credentials = {}) {

    var response = await this.request('Auth/Login', {
      method: 'POST',
      body: JSON.stringify(credentials),
      credentials: 'include'
    });
     if (response) {
       this.store.dispatch('setUsername', credentials.Username);
       this.store.dispatch('setRole', response.role);
       this.store.dispatch('setId', response.customerId);
       this.store.dispatch('setJWToken', response.token);
     }
     return response;
  }

  async getAllProducts() {
    const response = await this.request('Product/GetAllProducts');
    return response;
  }

  async getCategories() {
    const response = await this.request('Product/GetCategories');
    return response;
  }

  async PostOrder(Order: any) {
    try {
      const response = await this.request('Order/CreateOrder', {
        method: 'POST',
        body: JSON.stringify(Order),
        headers: { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + this.store.getters.getJWToken }
      },false);
      return response;
    } catch (error) {
      return false;
    }
    
  }

  async GetOrderHistory() {
    const response = await this.request('Order/GetHistory/' + this.store.state.id, {
      method: 'GET',
      headers: { 'Authorization': 'Bearer ' + this.store.getters.getJWToken }
    });
    return response;
  }

  async DeleteOrder(id: string) {
    try {
      const response = await this.request('Order/DeleteOrder/' + id, {
        method: 'DELETE',
        headers: { 'Authorization': 'Bearer ' + this.store.getters.getJWToken }
      }, false);
      return response;
    } catch (error) {
      return false;
    }
  }

  async GetAllCustomers() {
    try {

      const response = await this.request('Customer/GetAllCustomers', {
        method: 'GET',
        headers: { 'Authorization': 'Bearer ' + this.store.getters.getJWToken }
      });
      return response;
    } catch (error) {
      return false;
    }
  }

  async GetAllUsers() {
    try {

      const response = await this.request('User/GetAllUsers', {
        method: 'GET',
        headers: { 'Authorization': 'Bearer ' + this.store.getters.getJWToken }
      });
      return response;
    } catch (error) {
      return false;
    }
  }

  async DeleteUser(userid: string) {
    try {
      const response = await this.request('User/DeleteUser/' + userid, {
        method: 'DELETE',
        headers: { 'Authorization': 'Bearer ' + this.store.getters.getJWToken }
      },false);
      return response;
    } catch (error) {
      return false;
    }
  }

  async DeleteCustomer(customerid: string) {
    try {
      const response = await this.request('Customer/DeleteCustomer/' + customerid, {
        method: 'DELETE',
        headers: { 'Authorization': 'Bearer ' + this.store.getters.getJWToken }
      },false);
    } catch (error) {
      return false;
    }
  }

  async UpdateUser(userid: string, data: any) {
    try {
      const response = await this.request('User/UpdateUser/' + userid, {
        method: 'PUT',
        body: JSON.stringify(data),
        headers: { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + this.store.getters.getJWToken }
      },false);
      return response;
    } catch (error) {
      return false;
    }
  }

  async UpdateCustomer(customerid: string, data: any) {
    try {
      const response = await this.request('Customer/UpdateCustomer/' + customerid, {
        method: 'PUT',
        body: JSON.stringify(data),
        headers: { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + this.store.getters.getJWToken }
      }, false);
    } catch (error) {
      return false;
    }
  }

  async CreateUser(user: any) {
    try {
      const response = await this.request('User/CreateUser', {
        method: 'POST',
        body: JSON.stringify(user),
        headers: { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + this.store.getters.getJWToken }
      }, false);
      return response;
    } catch (error) {
      return false;
    }
  }

  async UpdateProduct(productid: string, product: any) {
    try {
      const response = await this.request('Product/UpdateProduct/' + productid, {
        method: 'PUT',
        body: JSON.stringify(product),
        headers: { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + this.store.getters.getJWToken }
      }, false);
      return response;
    } catch (error) {
      return false;
    }
  }

  async DeleteProduct(productid: string) {
    try {
      const response = await this.request('Product/DeleteProduct/' + productid, {
        method: 'DELETE',
        headers: { 'Authorization': 'Bearer ' + this.store.getters.getJWToken }
      }, false);
      return response;
    } catch (error) {
      return false;
    }
  }

  async CreateProduct(product: any) {
    try {
      const response = await this.request('Product/CreateProduct', {
        method: 'POST',
        body: JSON.stringify(product),
        headers: { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + this.store.getters.getJWToken }
      }, false);
      return response;
    } catch (error) {
      return false;
    }
  }

  logout() {
    this.store.dispatch('deleteUsername');
    this.store.dispatch('deleteRole');
    this.store.dispatch('deleteJWToken');
    this.store.dispatch('deleteId');
  }
}
