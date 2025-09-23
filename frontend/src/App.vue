<template>
  <div class="h-full w-full p-6 bg-stone-950 flex flex-col items-center rounded-lg">
    <h1 class="text-3xl font-bold">Weather Checker</h1>
    <SearchBar class="mt-4" @search="fetchWeather" />
    <WeatherCard class="mt-4" :weather="weather" :loading="loading" @save="saveLocation" />
    <SavedLocations class="mt-4" :locations="locations" @select="fetchWeather" />
    <AlertModal/>
  </div>
</template>

<script>
import SearchBar from './components/SearchBar.vue'
import WeatherCard from './components/WeatherCard.vue'
import SavedLocations from './components/SavedLocations.vue'

import AlertModal from "./components/AlertModal.vue";
import { showAlert } from './utils/alert.js';

import { API_BASE_URL } from './utils/constants.js';

export default {
  components: { SearchBar, WeatherCard, SavedLocations, AlertModal },
  data: () => ({
    weather: null,
    loading: false,
    locations: []
  }),
  methods: {
    async fetchWeather(latitude, longitude, cityName) {
      if (!latitude || !longitude) return
      this.loading = true
      try {
        const res = await fetch(`${API_BASE_URL}/weather/get/${latitude}/${longitude}`);
        if (res.ok) {
          const jsonResult = await res.json();
          
          jsonResult.fetchedAt = new Date().toISOString();
          jsonResult.cityName = cityName;
          
          console.log(jsonResult);
          this.weather = jsonResult;
        } else {
          this.weather = null
          showAlert(`The API returned an unexpected response.\n${res}`, [
            {"Ok" : "#c66228ff"}
          ])
        }
      }
      catch(e) {
        showAlert(`There was an issue waiting for the API to respond.\n${e}`, [
            {"Ok" : "#c66228ff"}
          ])
      }
      finally {
        this.loading = false
      }
    },
    saveLocation(city) {
      if (!this.locations.includes(city)) {
        this.locations.push(city)
        localStorage.setItem('savedLocations', JSON.stringify(this.locations))
      }
    },
    loadSavedLocations() {
      const saved = localStorage.getItem('savedLocations')
      if (saved) {
        this.locations = JSON.parse(saved)
      }
    }
  },
  mounted() {
    this.loadSavedLocations()
  }
}
</script>