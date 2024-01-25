<template lang="">
  <div class="form-import-overlay">
    <div class="form-import">
      <div class="form-import-header">
        <h3 class="form-import-heading">Nhập khẩu khách hàng</h3>
        <div class="form-import-header-close"></div>
      </div>

      <div class="form-import-content">
        <h4 class="form-import-current-step">Bước 1: Chọn tệp nguồn</h4>
        <div class="form-import-main">
          <ul class="form-import-progress">
            <li class="import-progress-step progress--current-step">
              1. Chọn tệp nguồn
            </li>
            <li class="import-progress-step">2. Kiểm tra dữ liệu</li>
            <li class="import-progress-step">3. Kết quả nhập khẩu</li>
          </ul>

          <div class="form-import-current-content">
            <!-- <import-data-checking /> -->
            <import-file-selection />
          </div>
        </div>
        <div class="form-import-footer">
          <misa-button buttonName="Giúp" buttonClass="normal-button" />
          <misa-button buttonName="Đóng" buttonClass="normal-button" />
        </div>
      </div>
    </div>
  </div>
</template>
<script>
// import ImportDataChecking from "./import-steps/data-checking/ImportDataChecking.vue";
import ImportFileSelection from "./import-steps/file-selection/ImportFileSelection.vue";
// import ImportResult from "./import-steps/import-result/ImportResult.vue";
export default {
  name: "MisaImportForm",
  components: {
    ImportFileSelection,
    // ImportDataChecking,
    // ImportResult,
  },
};
</script>
<style lang="css" scoped>
@import "./import-form.css";
</style>
