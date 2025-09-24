<template>
  <div ref="wrapper" :class="{'gradient-border': searching}" class="flex w-full max-w-4xl relative rounded-lg border-4 border-transparent">
    <input
      type="text"
      v-model="cityQuery"
      placeholder="Enter city name..."
      @keyup.enter="onEnter"
      @focus="showDropdown = true"
      @input="onInput"
      class="flex-1 p-4 rounded-s-lg text-black text-lg bg-stone-200 focus:outline-none"
    />
    <div class="flex items-center">
      <button
        @click="search"
        class="w-16 p-4 text-black text-lg bg-stone-200 hover:bg-stone-300 transition cursor-pointer"
      >
        <div v-if="!searching">🔍</div>
        <div v-if="searching">🔃</div>
      </button>
    </div>
    <div class="flex items-center">
      <button
        @click="clear"
        class="w-16 p-4 rounded-e-lg text-black text-lg bg-stone-200 hover:bg-stone-300 transition cursor-pointer"
      >
        X
      </button>
    </div>
    <ul
      v-if="showDropdown"
      class="absolute left-0 top-full z-10 w-full text-black bg-white border rounded shadow-lg max-h-64 overflow-y-auto"
    >
      <div v-if="suggestions.length <= 0">
        <li v-if="!searching"class="p-2 text-gray-500">Start Typing to find your city</li>
        <li v-if="searching"class="p-2 text-gray-500">Searching.....</li>
      </div>
      <li
        v-for="(city, index) in suggestions"
        :key="city.lat + '^' + city.lon"
        @click="selectCity(city)"
        class="p-2 hover:bg-blue-100 cursor-pointer"
      >
        {{ city.displayName }}
      </li>
    </ul>
  </div>
</template>

<script>
import debounce from "lodash/debounce";
import { API_BASE_URL } from '../utils/constants.js';

export default {
  data: () => (
    {
      cityQuery: "",
      suggestions: [],
      showDropdown: false,
      searching: false,
    }
  ),
  methods: {
    async fetchSuggestions() {
      if (!this.cityQuery) {
        this.suggestions = [];
        return;
      }
      this.searching = true;
      try {
        const encodedCity = encodeURIComponent(this.cityQuery);
        
        const res = await fetch(`${API_BASE_URL}/weather/search/${encodedCity}`);
        
        if (res.ok) {
          const jsonResult = await res.json();          
          this.suggestions = jsonResult;
        } else if (res.status === 404) {
          // no cities found
          this.suggestions = [];
        } else {
          console.error("Unexpected response", res);
          this.suggestions = [];
        }
      } catch (err) {
        console.error(err);
        this.suggestions = [];
      }
      this.searching = false;
    },
    async onEnter() {
      await this.fetchSuggestions();

      if (this.suggestions.length > 0) {
        this.selectCity(this.suggestions[0]);
      } else {
        this.showDropdown = true;
      }
    },
    async onInput() {
      this.debouncedFetch();
      this.showDropdown = true;
    },
    selectCity(city) {
      this.showDropdown = false;
      this.$emit("search", city.lat, city.lon, city.displayName);
    },
    search() {
      if (this.suggestions.length === 0) return;
      this.selectCity(this.suggestions[0]);
    },
    clear() {
        this.cityQuery = "";
    },
    handleClickOutside(event) {
      if (this.$refs.wrapper && !this.$refs.wrapper.contains(event.target)) {
        this.showDropdown = false;
      }
    }
  },
  created() {
    this.debouncedFetch = debounce(this.fetchSuggestions, 500);
  },
  mounted() {
    document.addEventListener("click", this.handleClickOutside);
  },
  beforeUnmount() {
    document.removeEventListener("click", this.handleClickOutside);
  },
}
</script>