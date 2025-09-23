<template>
  <div class="h-full p-6 bg-stone-950 flex flex-col items-center rounded-lg">
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

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export default {
  components: { SearchBar, WeatherCard, SavedLocations, AlertModal },
  data: () => ({
    weather: null,
    loading: false,
    locations: []
  }),
  methods: {
    async fetchWeather(city) {
      if (!city) return
      this.loading = true
      try {
        const res = await fetch(`${API_BASE_URL}/weather/${city}`)
        if (res.ok) {
          this.weather = await res.json()
          alert("huh");
        } else {
          this.weather = null
          showAlert("The API returned an unexpected response.", [
            {"Ok" : "#c66228ff"}
          ])
        }
      }
      catch {
        showAlert("There was an issue waiting for the API to respond.", [
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