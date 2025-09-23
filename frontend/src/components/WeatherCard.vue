<template>
  <div
    v-if="weather"
    class="max-w-4xl w-full text-black bg-white/90 backdrop-blur-sm rounded-lg shadow-md p-6 mb-6 flex flex-col items-center"
  >
    <h2 class="text-2xl font-bold mb-2 text-center">CityName</h2>
    <h3 class="text-xl font-bold mb-1 text-center">Current:</h3>

    <div class="flex items-center mb-3">
      <img
        v-if="this.weather.icon"
        :src="`http://openweathermap.org/img/wn/${this.weather.latitude}@2x.png`"
        alt="Weather Icon"
        class="w-16 h-16 mr-4"
      />
      <p class="text-3xl font-semibold">{{ Math.round(this.weather.latitude) }}°C</p>
    </div>

    <p class="capitalize mb-4 text-gray-700 text-center">
      {{ this.weather.latitude }}
    </p>

    <button
      @click="$emit('save', this.weather.latitude)"
      class="bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600 transition"
    >
      Save Location
    </button>
  </div>

  <div v-else-if="loading" class="text-black font-bold max-w-4xl w-full bg-white/80 rounded-lg shadow-md p-6 mb-6 text-center">
    Loading...
  </div>

  <div v-else class="max-w-4xl w-full bg-white/80 rounded-lg shadow-md p-6 mb-6 text-center text-gray-500">
    Search a city to see the weather
  </div>
</template>

<script>
export default {
  props: {
    weather: Object,
    loading: {
      type: Boolean,
      default: false
    }
  },
  emits: ['save']
}
</script>