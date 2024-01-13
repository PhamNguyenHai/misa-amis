<template lang="">
  <div
    class="main-content"
    :class="{ 'max-main-content': $store.state.isSidebarMini }"
  >
    <router-view></router-view>
  </div>
</template>
<script>
export default {
  name: "TheMain",
};
</script>
<style lang="css" scoped>
@import "./content.css";
</style>
