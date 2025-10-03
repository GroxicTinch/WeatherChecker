<template>
  <div class="overflow-y-auto max-h-96 max-w-4xl w-full bg-white/80 text-black rounded-lg shadow-md p-6 mb-6">
    <h3 class="text-xl font-bold">Saved Locations</h3>
    <div v-if="locations.length != 0">
      <ul>
        <li v-for="loc in locations" :key="loc" class="flex justify-between rounded my-2 p-4 bg-stone-100 hover:bg-white flex parent group">
          <button
            class="flex-1 cursor-pointer text-left"
            @click="$emit('select', {latitude: loc.latitude,longitude: loc.longitude,cityName: loc.cityName, customName: loc.customName || ''})"
          >
            {{ loc.customName ? loc.customName : loc.cityName }}
          </button>
          <button
            class="ml-2 text-red-500 cursor-pointer opacity-100"
            @click="$emit('changeName', {latitude: loc.latitude,longitude: loc.longitude,cityName: loc.cityName, customName: loc.customName || ''})"
          >
            ✏️
          </button>
          <button
            class="ml-2 text-red-500 cursor-pointer opacity-100"
            @click="$emit('delete', {latitude: loc.latitude,longitude: loc.longitude})"
          >
            ❌
          </button>
        </li>
      </ul>
    </div>
    <div v-else>
      <p>No saved locations</p>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    locations: Array
  }
}
</script>