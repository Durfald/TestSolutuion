<template>
	<div class="login-container">
		<div class="login-card">

			<h2 class="title">Вход</h2>
			<form @submit.prevent="handleLogin">

				<div class="input-group">
					<label for="username">Имя пользователя</label>
					<input id="username" type="text"
								 v-model="username"/>
				</div>

				<div class="input-group">
					<label for="password">Пароль</label>
					<input id="password" type="password" 
								 v-model="password"/>
				</div>

				<button type="submit" class="login-btn">Войти</button>
			</form>
		</div>
	</div>
</template>

<script lang="ts" setup>
	import { defineComponent, ref } from 'vue';
	import { useRouter } from 'vue-router';
	import api from '@scripts/ApiService.ts';
	const srv = new api();
  const username = ref('');
  const password = ref('');
  const router = useRouter(); 
  
  const handleLogin = async () => {
  	console.log('Logging in with', { username: username.value, password: password.value });
  	try {

			srv.login({
				Username: username.value,
				Password: password.value,
			});
  
  		router.push({ path: '/' });
  	} catch (error) {
  		console.error('Login failed:', error);
  	}
  };
	
</script>

<style scoped>
	/* Общие стили для страницы входа */
	.login-container {
		display: flex;
		justify-content: center;
		align-items: center;
		height: 100vh;
	}

	.login-card {
		background-color: #ffffff;
		padding: 30px;
		border-radius: 10px;
		box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
		width: 300px;
		border: 2px solid #14213d; /* Цвет компонента */
	}

	.title {
		color: #000000; /* Цвет текста */
		text-align: center;
		font-size: 24px;
		margin-bottom: 20px;
	}

	.input-group {
		margin-bottom: 15px;
	}

	label {
		color: #000000; /* Цвет текста */
		font-size: 14px;
		margin-bottom: 5px;
		display: block;
	}

	input {
		width: 100%;
		padding: 10px;
		font-size: 14px;
		border: 1px solid #e5e5e5; /* Цвет фона элемента */
		border-radius: 5px;
		color: #000000; /* Цвет текста */
		box-sizing: border-box;
	}

		input:focus {
			outline: none;
			border-color: #fca311; /* Цвет акцента */
		}

	button.login-btn {
		width: 100%;
		padding: 12px;
		background-color: #fca311; /* Цвет акцента */
		border: none;
		border-radius: 5px;
		color: #ffffff;
		font-size: 16px;
		cursor: pointer;
	}

		button.login-btn:hover {
			background-color: #e58e0d; /* Более темный оттенок акцентного цвета */
		}

		button.login-btn:active {
			background-color: #f78e10; /* Еще более темный оттенок */
		}
</style>
