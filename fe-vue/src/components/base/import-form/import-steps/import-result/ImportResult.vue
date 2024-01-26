<template lang="">
  <div class="import-result">
    <h3 class="import-result-title">Kết quả nhập khẩu</h3>
    <div class="import-result-main">
      <div class="import-dowload-result">
        Tải về tập tin chứa kết quả nhập khẩu
        <span class="dowload-result-hyperlink">tại đây</span>
      </div>
      <div class="import-records-result">
        <div class="import-valid-records-count">
          + Số dòng nhập khẩu thành công: 2
        </div>
        <div class="import-invalid-records-count">
          + Số dòng nhập khẩu thành công: 10
        </div>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: "ImportResult",
  props: {
    // Số lượng bản ghi hợp lệ
    validRecordsCount: { type: Number, required: true },

    // Số lượng bản ghi không hợp lệ
    invalidRecordsCount: { type: Number, required: true },
  },
};
</script>
<style lang="css" scoped>
@import "./impor-result.css";
</style>
