<template lang="">
  <div class="import-result">
    <h3 class="import-result-title">
      {{
        $_MisaResources.appText.excelImportTitle.importResult.importResultTitle
      }}
    </h3>
    <div class="import-result-main">
      <div class="import-dowload-result">
        {{ $_MisaResources.appText.excelImportTitle.dowload.importResult }}

        <span class="dowload-result-hyperlink">{{
          $_MisaResources.appText.excelImportTitle.dowload.here
        }}</span>
      </div>
      <div class="import-records-result">
        <div class="import-valid-records-count">
          +
          {{
            $_MisaResources.appText.excelImportTitle.importResult
              .validRecordsNumber
          }}: {{ validRecordsCount }}
        </div>
        <div class="import-invalid-records-count">
          +
          {{
            $_MisaResources.appText.excelImportTitle.importResult
              .invalidRecordsNumber
          }}: {{ invalidRecordsCount }}
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
