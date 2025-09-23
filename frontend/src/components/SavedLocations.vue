<template>
  <div>
    <h3 class="text-xl font-bold">Saved Locations</h3>
    <div v-if="locations.length != 0">
      <ul>
        <li v-for="loc in locations" :key="loc">
          <button @click="$emit('select', loc)">{{ loc }}</button><button @click="$emit('delete', loc)" class="ml-2 text-red-500">🗑</button>
        </li>
      </ul>
    </div>
    <div v-else>
      <p>No saved locations</p>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    locations: Array
  }
}
</script>