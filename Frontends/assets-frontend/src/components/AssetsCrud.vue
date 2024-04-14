<template>
	<div>Name: assets-frontend</div>
	<button @click="getAllAssetRecords" class="button">
		Get All Asset Records
	</button>
	<br />
	<button @click="getAssetRecordById" class="button">
		Get Asset Record by ID
	</button>
	<br />
	<button @click="createAssetRecord" class="button">
		Create Asset Record
	</button>
	<br />
	<button @click="updateAssetRecord" class="button">
		Update Asset Record
	</button>
	<br />
	<button @click="deleteAssetRecord" class="button">
		Delete Asset Record
	</button>
</template>

<style scoped>
.button {
	background-color: #48bb78;
	color: #ffffff;
	font-weight: bold;
	padding: 8px 16px;
	border-radius: 4px;
	margin-top: 16px;
	transition: background-color 0.3s ease-in-out;
}

.button:hover {
	background-color: #2f855a;
}
</style>

<script setup>
import { ref } from "vue";
import axios from "axios";

const host = "http://localhost:9040";

const getAllAssetRecords = async () => {
	try {
		const response = await axios.get(`${host}/api/assets`);
		console.log(response.data);
	} catch (error) {
		console.error(error);
	}
};

const getAssetRecordById = async () => {
	const id = 3; // Example ID, replace or modify as needed
	try {
		const response = await axios.get(`${host}/api/assets/${id}`);
		console.log(response.data);
	} catch (error) {
		console.error(error);
	}
};

const createAssetRecord = async () => {
	try {
		const response = await axios.post(`${host}/api/assets`, {
			name: "New Asset",
			type: "Description of the asset",
			status: "New",
			latitude: 50.0,
			longitude: 15.0,
		});
		console.log(response.data);
	} catch (error) {
		console.error(error);
	}
};

const updateAssetRecord = async () => {
	const id = 5; // Example ID, replace or modify as needed
	try {
		const response = await axios.put(`${host}/api/assets/${id}`, {
			name: "Updated Asset",
			type: "Description of the updated asset",
			status: "Updated",
			latitude: 50.0,
			longitude: 15.0,
		});
		console.log(response.data);
	} catch (error) {
		console.error(error);
	}
};

const deleteAssetRecord = async () => {
	const id = 2; // Example ID, replace or modify as needed
	try {
		await axios.delete(`${host}/api/assets/${id}`);
		console.log(`Record with ID ${id} deleted successfully.`);
	} catch (error) {
		console.error(error);
	}
};
</script>
