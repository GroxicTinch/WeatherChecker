export default {
  mounted(el) {
    el.addEventListener("wheel", (e) => {
      if (window.innerWidth < 640) return;
      if (e.deltaY !== 0) {
        e.preventDefault();
        el.scrollLeft += e.deltaY;
      }
    });
  },
  unmounted(el) {
    el.removeEventListener("wheel", () => {});
  }
};