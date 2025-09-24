<!-- Usage:
  import { showAlert } from '../utils/alert.js';
  
  showAlert("Your message here", [
    "Option 1",
    {"Option 2": "#ff0000"}, // Use hex code to set button color
    "Option 3"
  ]).then((choice) => {
    console.log("User selected option index:", choice);
    // If showInput was true, choice will be an object: { choice: index, input: value }
  });
-->

<script setup>
import { ref, onMounted } from 'vue';
import { setAlertModalRef } from '../utils/alert.js';

const isVisible = ref(false);
const promptText = ref('');
const options = ref([]);
const showInput = ref(false);
const inputValue = ref('');
const inputRef = ref(null);
let resolveFn = null;

function handleClick(index) {
  isVisible.value = false;
  if (resolveFn) {
    if (showInput.value) {
      resolveFn({ choice: index, input: inputValue.value });
    } else {
      resolveFn(index);
    }
  }
}

function handleEnter() {
  if (options.value.length === 1) {
    handleClick(0);
  }
}

onMounted(() => {
  setAlertModalRef({
    get promptText() { return promptText.value; },
    set promptText(val) { promptText.value = val; },
    get options() { return options.value; },
    set options(val) { options.value = val; },
    get isVisible() { return isVisible.value; },
    set isVisible(val) { isVisible.value = val; },
    get showInput() { return showInput.value; },
    set showInput(val) { showInput.value = val; },
    get inputValue() { return inputValue.value; },
    set inputValue(val) { inputValue.value = val; },
    get inputRef() { return inputRef.value; },
    resolveFn,
    set resolveFn(val) { resolveFn = val; }
  });
});
</script>

<template>
  <div v-show="isVisible" class="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
    <div class="bg-gray-800 text-white p-6 rounded-lg shadow-lg max-w-md w-full">
      <div class="mb-4">{{ promptText }}</div>

      <input
        v-if="showInput"
        ref="inputRef"
        v-model="inputValue"
        type="text"
        class="w-full p-2 mb-4 rounded text-black bg-white"
        placeholder="Type here..."
        @keyup.enter="handleEnter"
      />

      <div class="flex justify-end gap-2">
        <button
          v-for="(opt, i) in options"
          :key="i"
          :style="typeof opt === 'object' ? { backgroundColor: Object.values(opt)[0] } : {}"
          class="px-4 py-2 rounded hover:opacity-90"
          @click="handleClick(i)"
        >
          {{ typeof opt === 'string' ? opt : Object.keys(opt)[0] }}
        </button>
      </div>
    </div>
  </div>
</template>