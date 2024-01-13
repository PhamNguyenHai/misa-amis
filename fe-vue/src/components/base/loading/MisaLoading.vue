<template lang="">
  <div class="loader" v-if="loadingStatus"></div>
</template>
<script>
export default {
  name: "MisaLoading",
  props: {
    // prop từ EmployeePage -> tín hiệu để bật/tắt loading
    loadingStatus: { type: Boolean, default: true },
  },
};
</script>
<style lang="css">
@import "./loading.css";
</style>
