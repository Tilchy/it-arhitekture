<template>
	<div>Name: tracking-frontend</div>
	<button @click="getAllTrackingRecords" class="button">
		Get All Tracking Records
	</button>
	<br />
	<button @click="getTrackingRecordById" class="button">
		Get Tracking Record by ID
	</button>
	<br />
	<button @click="createTrackingRecord" class="button">
		Create Tracking Record
	</button>
	<br />
	<button @click="updateTrackingRecord" class="button">
		Update Tracking Record
	</button>
	<br />
	<button @click="deleteTrackingRecord" class="button">
		Delete Tracking Record
	</button>
</template>

<style scoped>
.button {
	background-color: #a855f7;
	color: #ffffff;
	font-weight: bold;
	padding: 8px 16px;
	border-radius: 4px;
	margin-top: 16px;
	transition: background-color 0.3s ease-in-out;
}

.button:hover {
	background-color: #6b21a8;
}
</style>

<script setup>
import { ref } from "vue";
import axios from "axios";

const host = "http://localhost:9040";

const getAllTrackingRecords = async () => {
	try {
		const response = await axios.get(`${host}/api/tracking/list/Car`);
		console.log(response.data);
	} catch (error) {
		console.error(error);
	}
};

const getTrackingRecordById = async () => {
	const id = 3;
	try {
		const response = await axios.get(`${host}/api/tracking/${id}`);
		console.log(response.data);
	} catch (error) {
		console.error(error);
	}
};

const createTrackingRecord = async () => {
	try {
		const response = await axios.post(`${host}/api/tracking`, {
			name: "New Tracker",
			description: "Description of the tracker",
			category: "Car",
			status: "New",
			priority: 1,
		});
		console.log(response.data);
	} catch (error) {
		console.error(error);
	}
};

const updateTrackingRecord = async () => {
	const id = 5; // Example ID, replace or modify as needed
	try {
		const response = await axios.put(`${host}/api/tracking`, {
			id: "1",
			name: "Updated Tracker",
			description: "Description of the tracker",
			category: "Car",
			status: "Updated",
			priority: 2,
		});
		console.log(response.data);
	} catch (error) {
		console.error(error);
	}
};

const deleteTrackingRecord = async () => {
	try {
		await axios.delete(`${host}/api/tracking`, {
			id: "3",
		});
		console.log(`Record with ID ${id} deleted successfully.`);
	} catch (error) {
		console.error(error);
	}
};
</script>
