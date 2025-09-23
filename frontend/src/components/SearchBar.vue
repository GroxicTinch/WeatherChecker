<template>
  <div class="flex w-full max-w-md">
    <input
      type="text"
      v-model="cityQuery"
      placeholder="Enter city name..."
      @keyup.enter="search"
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
  </div>
</template>

<script>
export default {
  data: () => ({ cityQuery: "" }),
  methods: {
    search() {
      this.$emit("search", this.cityQuery)
    },
    clear() {
        this.cityQuery = ""
    }
  }
}
</script>