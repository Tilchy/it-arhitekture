<template>
	<div>Name: maintenance-frontend</div>
	<button @click="getAllMaintenanceRecords" class="button">
		Get All Maintenance Records
	</button>
	<br />
	<button @click="getMaintenanceRecordById" class="button">
		Get Maintenance Record by ID
	</button>
	<br />
	<button @click="createMaintenanceRecord" class="button">
		Create Maintenance Record
	</button>
	<br />
	<button @click="updateMaintenanceRecord" class="button">
		Update Maintenance Record
	</button>
	<br />
	<button @click="deleteMaintenanceRecord" class="button">
		Delete Maintenance Record
	</button>
</template>

<style scoped>
.button {
	background-color: #3b82f6;
	color: #ffffff;
	font-weight: bold;
	padding: 8px 16px;
	border-radius: 4px;
	margin-top: 16px;
	transition: background-color 0.3s ease-in-out;
}

.button:hover {
	background-color: #1d4ed8;
}
</style>

<script setup>
import { ref } from "vue";
import axios from "axios";

const host = "http://localhost:9040";

const getAllMaintenanceRecords = async () => {
	try {
		const response = await axios.get(`${host}/maintenance`);
		console.log(response.data);
	} catch (error) {
		console.error(error);
	}
};

const getMaintenanceRecordById = async () => {
	const id = 3; // Example ID, replace or modify as needed
	try {
		const response = await axios.get(`${host}/maintenance/${id}`);
		console.log(response.data);
	} catch (error) {
		console.error(error);
	}
};

const createMaintenanceRecord = async () => {
	try {
		const response = await axios.post(`${host}/maintenance`, {
			name: "New Maintenance Task",
			description: "Description of the task",
			location: "Location A",
			duration: "2 hours",
			status: "Pending",
			type: "Type A",
			priority: "High",
		});
		console.log(response.data);
	} catch (error) {
		console.error(error);
	}
};

const updateMaintenanceRecord = async () => {
	const id = 1; // Example ID, replace or modify as needed
	try {
		const response = await axios.put(`${host}/maintenance/${id}`, {
			name: "Updated Maintenance Task",
			description: "Updated description",
			location: "Location B",
			duration: "3 hours",
			status: "Completed",
			type: "Type B",
			priority: "Medium",
		});
		console.log(response.data);
	} catch (error) {
		console.error(error);
	}
};

const deleteMaintenanceRecord = async () => {
	const id = 2; // Example ID, replace or modify as needed
	try {
		await axios.delete(`${host}/maintenance/${id}`);
		console.log(`Record with ID ${id} deleted successfully.`);
	} catch (error) {
		console.error(error);
	}
};
</script>
