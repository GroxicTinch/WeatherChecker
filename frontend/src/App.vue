<template>
  <div class="h-full w-full p-6 bg-stone-900 flex flex-col items-center rounded-lg">
    <h1 class="text-white text-3xl font-bold">Weather Checker</h1>
    <SearchBar class="mt-4" @search="fetchWeather" />
    <WeatherCard class="mt-4" :weather="weather" :loading="loading" @save="saveLocation" />
    <SavedLocations class="mt-4" :locations="locations" @select="fetchWeather" @changeName="changeLocationName" @delete="removeLocation"/>
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
    async fetchWeather(cityObject) {
      if (!cityObject || !cityObject.latitude || !cityObject.longitude) return
      this.loading = true
      try {
        const res = await fetch(`${API_BASE_URL}/weather/get/${cityObject.latitude}/${cityObject.longitude}`);
        if (res.ok) {
          const jsonResult = await res.json();

          jsonResult.fetchedAt = new Date().toISOString();
          jsonResult.cityName = cityObject.cityName;
          jsonResult.customName = cityObject.customName;

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
    saveLocation(cityObject) {
      if (!cityObject || !cityObject.latitude || !cityObject.longitude) return
      const exists = this.locations.some(loc =>
        loc.latitude === cityObject.latitude && loc.longitude === cityObject.longitude
      );
      if (!exists) {
        this.locations.push(cityObject)
        localStorage.setItem('savedLocations', JSON.stringify(this.locations))
      }
    },
    changeLocationName(cityObject) {
      if (!cityObject || !cityObject.latitude || !cityObject.longitude) return
      const index = this.locations.findIndex(loc =>
        loc.latitude == cityObject.latitude && loc.longitude == cityObject.longitude
      );
      if (index !== -1) {
        const alertButtons = [
          {
            "Submit": "#00af37ff"
          },
          {
            "Remove Custom Name": "#950000ff"
          },
          {
            "Cancel": "#c66228ff"
          }
        ];
        showAlert("Enter the new saved location name:", alertButtons, true, cityObject.customName || cityObject.cityName)
        .then(result => {
          if(result.choice === 0) { // Submit
            this.locations[index].customName = result.input;
            localStorage.setItem('savedLocations', JSON.stringify(this.locations));
          } else if(result.choice === 1) { // Remove Custom Name
            delete this.locations[index].customName;
            localStorage.setItem('savedLocations', JSON.stringify(this.locations));
          }
        });
      } else {
        console.error("Location not found for renaming");
      }
    },
    removeLocation(cityObject) {
      if (!cityObject || !cityObject.latitude || !cityObject.longitude) return
      this.locations = this.locations.filter(
        loc => loc.latitude !== cityObject.latitude || loc.longitude !== cityObject.longitude
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