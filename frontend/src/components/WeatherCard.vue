<template>
  <div
    v-if="weather && !loading"
    class="max-w-5xl w-full text-black bg-white/90 backdrop-blur-sm rounded-lg shadow-md p-6 mb-6 flex flex-col items-center border-4 border-transparent"
  >
    <div class="parent group">
      <h2 class="text-2xl font-bold mb-2 text-center">{{ this.weather.cityName }}</h2>
      <span class="tooltip absolute hidden bg-white group-hover:block border-1 border-stone-200 p-2 rounded text-sm max-w-xs truncate z-10 pointer-events-none">
        Lat: {{ this.weather.latitude }}, Lon: {{ this.weather.longitude }}
      </span>
    </div>
    <h3 class="text-xl font-bold mb-1">Today:</h3>
    <div class="max-w-2xl w-full flex max-sm:flex-col justify-around bg-stone-100 p-4 rounded-lg mb-4">
      <div id="today-temps" class="flex max-sm:justify-around text-center items-center">
        <div class="flex items-center">
          <div>
            <img class="weather-icon size-20 mx-1 object-fit" :src="currentWeatherCode.icon" alt="Weather Icon"/>
            <span>
              {{ currentWeatherCode.description }}
            </span>
          </div>
          <span class="font-semibold text-lg mx-4 parent group relative">
            {{ this.weather.current.temperature_2m }}°C
            <span class="tooltip absolute hidden bg-white group-hover:block border-1 border-stone-200 p-2 rounded text-sm max-w-xs truncate z-10 pointer-events-none">
              Feels like {{ this.weather.current.apparent_temperature }}°C
            </span>
          </span>
          <div class="flex flex-col mx-1 text-m">
            <span class="border-b border-gray-600">
              {{ this.weather.daily.temperature_2m_min[0].toFixed(0) }}°C min
            </span>
            <span class="">
              {{ this.weather.daily.temperature_2m_max[0].toFixed(0) }}°C max
            </span>
          </div>
        </div>
      </div>
      <div id="today-rainWind" class="flex flex max-sm:justify-around max-sm:mt-4 text-stone-600">
        💧 Precipitation: {{ this.weather.current.precipitation }}%<br/>
        🔆 UV Max: {{ this.weather.daily.uv_index_max[0].toFixed(1) }}<br/>
        💨 Wind Gusts: {{ this.weather.current.wind_gusts_10m.toFixed(1) }}km/h<br/>
        🍃 Wind Speed: {{ this.weather.current.wind_speed_10m.toFixed(1) }}km/h<br/>
      </div>
    </div>
    <div
      v-scroll-horizontal
      class="w-full max-sm:w-[200px] flex max-sm:flex-col justify-around bg-stone-100 p-4 rounded-lg mb-4 overflow-x-auto"
    >
      <div v-for="(day, dayIndex) in dailyWeather"
        :key="dayIndex"
        class="flex flex-col items-center mx-2 max-sm:mx-auto sm:min-w-[110px]"
      >
        <hr v-if="dayIndex > 0" class="sm:hidden border-t border-1 border-gray-300 w-full my-2">
        <span class="font-semibold text-lg">
          {{day.date}}
        </span>
        <span class="group relative parent">
          <span>
            {{ day.maxTemp?.toFixed(0) }}°
          </span>
          <span class="text-black/60">
            {{ day.minTemp?.toFixed(0) }}°
          </span>
          <span class="tooltip absolute hidden bg-white group-hover:block border-1 border-stone-200 p-2 rounded text-sm max-w-xs truncate z-10 pointer-events-none">
            Feels like<br/>
            {{ day.maxApparentTemp?.toFixed(0) }}°C Max<br/>
            {{ day.minApparentTemp?.toFixed(0) }}°C Min
          </span>
        </span>
        <div class="flex flex-col text-center items-center mx-auto font-semibold my-2">
          <img class="weather-icon size-15 mx-1 object-fit" :src="day.code.icon" alt="Weather Icon"/>
          <span>
            {{ day.code.description }}
          </span>
        </div>
        <div class="flex justify-between w-full">
          <span>💧{{ day.precip }}%</span>
          <span>🔆{{ day.uv.toFixed(1) }}</span>
        </div>
        <div class="flex justify-between w-full">
          <span>💨{{ this.weather.current.wind_gusts_10m.toFixed(1) }}</span>
          <span>🍃{{ this.weather.current.wind_speed_10m.toFixed(1) }}</span>
        </div>
      </div>
    </div>

    <button
      @click="$emit('save', this.weather.latitude, this.weather.longitude, this.weather.cityName)"
      class="bg-green-700 text-white px-4 py-2 rounded hover:bg-green-600 transition cursor-pointer"
    >
      Save Location
    </button>
    <div class="absolute bottom-8 right-8 text-black/60 text-right text-sm hover:text-black hover:underline">
      <a href="https://open-meteo.com" target="_blank" rel="noopener noreferrer">Weather data available by Open-Meteo API</a>
    </div>
  </div>

  <div
    v-else
    :class="{'gradient-border': loading}"
    class="max-w-5xl w-full bg-white/90 backdrop-blur-sm rounded-lg shadow-md p-6 mb-6 flex flex-col items-center text-gray-500 border-4 border-transparent"
  >
    <span v-if="loading">Loading...</span>
    <span v-else>Search a city to see the weather</span>
    <div class="flex h-[545px] text-9xl items-center text-black/30">
     😶‍🌫️
    </div>
  </div>
</template>

<script>
import scrollHorizontal from "../directives/scrollHorizontal.js";
import { wmoCode } from '../utils/wmoCodes.js';

export default {
  directives: {
    scrollHorizontal
  },
  props: {
    weather: Object,
    loading: {
      type: Boolean,
      default: false
    }
  },
  emits: ['save'],
  computed: {
    currentWeatherCode() {
      return wmoCode(this.weather?.current?.weather_code);
    },
    dailyWeather() {
      if (!this.weather?.daily) return [];
      
      const daily = this.weather.daily;
      const length = daily.time.length || 0;

      return Array.from({ length }, (_, index) => ({
        dayIndex: index,
        date: new Date(daily.time?.[index] * 1000).toLocaleDateString('en-US', { weekday: 'short'}),
        code: wmoCode(daily.weather_code?.[index]) ?? null,
        maxTemp: daily.temperature_2m_max?.[index] ?? null,
        minTemp: daily.temperature_2m_min?.[index] ?? null,
        maxApparentTemp: daily.apparent_temperature_max?.[index] ?? null,
        minApparentTemp: daily.apparent_temperature_min?.[index] ?? null,
        precip: daily.precipitation_probability_max?.[index] ?? null,
        uv: daily.uv_index_max?.[index] ?? null,
        gusts: daily.wind_gusts_10m_max?.[index] ?? null,
        windSpeed: daily.wind_speed_10m_max?.[index] ?? null
      }));
    }
  }
}
</script>