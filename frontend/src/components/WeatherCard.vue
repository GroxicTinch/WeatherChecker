<template>
  <div
    v-if="weather"
    class="max-w-2xl w-full text-black bg-white/90 backdrop-blur-sm rounded-lg shadow-md p-6 mb-6 flex flex-col items-center"
  >
    <h2 class="text-2xl font-bold mb-2 text-center">{{ this.weather.cityName }}</h2>

    <h3 class="text-xl font-bold mb-1">Today:</h3>
    <div class="w-full flex justify-around">
      <div id="today-temps">
        <div class="flex items-center">
          <!-- <img class="weather-icon h-6 w-6 mx-1 object-fit" src="">
        <span class="font-semibold mx-1 mr-4">
          ${weatherToday.code.description} [TODO] Add WMO icon
        </span> -->
        <span class="font-semibold text-lg mx-1">
          {{ this.weather.current.temperature_2m }}°C
        </span>
        <div class="flex flex-col mx-1 text-m">
          <span class="border-b border-gray-600">
            {{ this.weather.daily.temperature_2m_min[0].toFixed(1) }}°C min
          </span>
          <span class="">
            {{ this.weather.daily.temperature_2m_max[0].toFixed(1) }}°C max
          </span>
        </div>
        </div>
      </div>
      <div id="today-rainWind">
        💧 Precipitation: {{ this.weather.current.precipitation }}%
      </div>
    </div>

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