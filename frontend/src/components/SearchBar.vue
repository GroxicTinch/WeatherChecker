<template>
  <div ref="wrapper" class="flex w-full max-w-4xl relative">
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
        🔍
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
      v-if="showDropdown && suggestions.length > 0"
      class="absolute left-0 top-full z-10 w-full text-black bg-white border rounded shadow-lg max-h-64 overflow-y-auto"
    >
      <li
        v-for="(city, index) in suggestions"
        :key="city.latitude + '^' + city.longitude"
        @click="selectCity(city)"
        class="p-2 hover:bg-blue-100 cursor-pointer"
      >
        {{ city.name }}, {{ city.admin1 ? city.admin1 + ', ' : '' }}{{ city.country }}
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
    }
  ),
  methods: {
    async fetchSuggestions() {
      if (!this.cityQuery) {
        this.suggestions = [];
        return;
      }
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
      this.$emit("search", city.latitude, city.longitude, $`${city.name}, ${ city.admin1 ? city.admin1 + ', ' : '' }${ city.country }`);
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