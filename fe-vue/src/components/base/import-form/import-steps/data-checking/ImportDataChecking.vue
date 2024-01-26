<template lang="">
  <div class="import-current-step-content">
    <div class="records-valid-result">
      <span class="records-result valid-records-result"
        >{{ validRecordsCount }} dòng hợp lệ</span
      >
      <span class="records-result invalid-records-result"
        >{{ invalidRecordsCount }} dòng không hợp lệ</span
      >
    </div>
    <misa-table-show-only
      class="import-table"
      :tableColumns="importObjectResources"
      :tableData="importRecordsData"
    />
    <span class="dowload-invalid-records"
      >Tải về tập tin chứa những dòng dữ liệu không hợp lệ
      <span class="dowload-invalid-hyperlink">tại đây</span></span
    >
  </div>
</template>
<script>
import MisaTableShowOnly from "@/components/base/table/MisaTableShowOnly.vue";
export default {
  name: "ImportDataChecking",
  components: {
    MisaTableShowOnly,
  },

  props: {
    // Số lượng bản ghi hợp lệ
    validRecordsCount: { type: Number, required: true },

    // Số lượng bản ghi không hợp lệ
    invalidRecordsCount: { type: Number, required: true },

    // Resources để hiển thị kết quả sau khi import file
    importObjectResources: { type: Object, required: true },

    // Dữ liệu được hiển thị sau khi check các bản ghi sau khi đọc file
    importRecordsData: { type: Array, required: true },
  },
};
</script>
<style lang="css" scoped>
@import "./data-checking.css";
</style>
