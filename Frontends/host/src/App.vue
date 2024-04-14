<template>
	<div class="mt-10 text-base mx-auto max-w-6xl">
		<div>Name: host</div>
		<MaintenanceCrud />
		<br />
		<br />
		<TrackingCrud />
		<br />
		<br />
		<AssetsCrud />
	</div>
</template>

<script setup>
import MaintenanceCrud from "maintenance_frontend/MaintenanceCrud";
import TrackingCrud from "tracking_frontend/TrackingCrud";
import AssetsCrud from "assets_frontend/AssetsCrud";
</script>
