<template>
  <div class="h-full w-full p-6 bg-stone-900 flex flex-col items-center rounded-lg">
    <h1 class="text-white text-3xl font-bold">Weather Checker</h1>
    <SearchBar class="mt-4" @search="fetchWeather" />
    <WeatherCard class="mt-4" :weather="weather" :loading="loading" @save="saveLocation" />
    <SavedLocations class="mt-4" :locations="locations" @select="fetchWeather" @delete="removeLocation"/>
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
    saveLocation(latitude, longitude, cityName) {
      const exists = this.locations.some(loc =>
        loc.latitude === latitude && loc.longitude === longitude
      );
      const cityObject = { latitude, longitude, cityName };
      if (!exists) {
        this.locations.push(cityObject)
        localStorage.setItem('savedLocations', JSON.stringify(this.locations))
      }
    },
    removeLocation(latitude, longitude) {
      this.locations = this.locations.filter(
        loc => loc.latitude !== latitude || loc.longitude !== longitude
      );
      localStorage.setItem('savedLocations', JSON.stringify(this.locations));
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