<template lang="">
  <div class="import-current-step-content">
    <div class="records-valid-result">
      <span class="records-result valid-records-result">21/23 dòng hợp lệ</span>
      <span class="records-result invalid-records-result"
        >2/23 dòng không hợp lệ</span
      >
    </div>
    <div class="tb"></div>
    <span class="dowload-invalid-records"
      >Tải về tập tin chứa những dòng dữ liệu không hợp lệ
      <span class="dowload-invalid-hyperlink">tại đây</span></span
    >
  </div>
</template>
<script>
export default {
  name: "ImportDataChecking",
};
</script>
<style lang="css" scoped>
@import "./data-checking.css";
</style>
