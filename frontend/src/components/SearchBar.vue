<template>
  <div class="w-full flex flex-col items-center">
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
        <div v-show="suggestions.length <= 0 && !searching">
          <!-- Using v-show instead of v-if/v-else Because it is something that changes often -->
          <li class="p-2 text-gray-500">Type to find your city</li>
          <hr class="border-gray-200 mx-2"/>
          <span v-show="recentSearches.length > 0" class="pl-6 text-gray-500 text-xs">Recent Searches:</span>
          <li
            v-for="(search, index) in recentSearches"
            :key="search"
            class="flex justify-between hover:bg-blue-100"
          >
            <div 
              @click="this.cityQuery = search; onInput()"
              class="p-2 cursor-pointer flex-1"
            >
              {{ search }}
            </div>
            <button
              class="px-2 text-red-500 hover:bg-red-100 cursor-pointer"
              @click="removeFromRecentSearches(index)"
            >
              ❌
            </button>
          </li>
        </div>
        <li v-show="searching"class="p-2 text-gray-500">Searching.....</li>
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
    <div class="w-full max-w-4xl text-white/60 text-right text-sm">
      <a href="https://www.openstreetmap.org/copyright" target="_blank" rel="noopener noreferrer" class="hover:text-white hover:underline">Location data © OpenStreetMap contributors</a>
    </div>
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
      recentSearches: [],
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
    addToRecentSearches(query) {
      if (query && !this.recentSearches.includes(query)) {
        this.recentSearches.push(this.cityQuery);
        if (this.recentSearches.length > 5) {
          this.recentSearches.shift();
        }
        localStorage.setItem('recentSearches', JSON.stringify(this.recentSearches));
      }
    },
    removeFromRecentSearches(index) {
      this.recentSearches.splice(index, 1);
      localStorage.setItem('recentSearches', JSON.stringify(this.recentSearches));
      setTimeout(() => {
        this.showDropdown = true;
      }, 0);
    },
    selectCity(city) {
      this.addToRecentSearches(this.cityQuery);
      this.showDropdown = false;
      this.$emit("search", city.lat, city.lon, city.displayName);
    },
    search() {
      if (this.suggestions.length === 0) return;
      this.selectCity(this.suggestions[0]);
    },
    clear() {
      this.cityQuery = "";
      this.suggestions = [];
    },
    handleClickOutside(event) {
      if (this.$refs.wrapper && !this.$refs.wrapper.contains(event.target)) {
        this.showDropdown = false;
      }
    }
  },
  created() {
    this.debouncedFetch = debounce(this.fetchSuggestions, 500);
    this.recentSearches = JSON.parse(localStorage.getItem('recentSearches'));
    if (!this.recentSearches) {
      this.recentSearches = [];
    }
  },
  mounted() {
    document.addEventListener("click", this.handleClickOutside);
  },
  beforeUnmount() {
    document.removeEventListener("click", this.handleClickOutside);
  },
}
</script>