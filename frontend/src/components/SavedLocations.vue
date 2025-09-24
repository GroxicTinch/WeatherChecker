<template>
  <div class="overflow-y-auto max-h-96 max-w-4xl w-full bg-white/80 text-black rounded-lg shadow-md p-6 mb-6">
    <h3 class="text-xl font-bold">Saved Locations</h3>
    <div v-if="locations.length != 0">
      <ul>
        <li v-for="loc in locations" :key="loc" class="flex justify-between rounded my-2 p-4 bg-stone-100 hover:bg-white flex parent group">
          <button
            class="cursor-pointer"
            @click="$emit('select', loc.latitude, loc.longitude, loc.cityName)"
          >
            {{ loc.cityName }}
          </button>
          <button
            class="ml-2 text-red-500 cursor-pointer parent group-hover:opacity-100 opacity-0"
            @click="$emit('delete', loc.latitude, loc.longitude)"
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