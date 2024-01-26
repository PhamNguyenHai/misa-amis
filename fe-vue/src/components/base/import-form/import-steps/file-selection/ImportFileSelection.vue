<template lang="">
  <div class="import-current-step-content">
    <div>Chọn dữ liệu đã chuẩn bị dữ liệu để nhập khẩu vào phần mềm</div>
    <div class="file-selection">
      <div class="file-selected-name" :title="selectedFileName">
        {{ selectedFileName }}
      </div>
      <misa-button
        buttonClass="normal-button"
        buttonName="Chọn"
        @click="selectFile"
      />
      <input type="file" ref="inputFileRef" @change="handleFileChange" hidden />
    </div>
    <div>
      Chưa có tệp mẫu để chuẩn bị dữ liệu? Tải tệp excel mẫu mà phần mềm cung
      cấp để chuẩn bị dữ liệu nhập khẩu
      <span
        class="dowload-template-hyperlink"
        @click="handleClickDowloadTemplateFile"
        >tại đây</span
      >
    </div>
  </div>
</template>
<script>
export default {
  name: "ImportFileSelection",
  emits: ["notifyChangeFile", "notifyClickedDowloadTemplateFile"],
  data() {
    return {
      selectedFile: null,
      selectedFileName: "Chưa có tệp nào được chọn !",
    };
  },

  methods: {
    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện xử lý chọn file
     */
    selectFile() {
      try {
        this.$refs.inputFileRef.click();
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {*} event : event
     * Description: Hàm thực hiện thông báo khi form thay đổi
     */
    handleFileChange(event) {
      try {
        const file = event.target.files[0];
        if (file) {
          this.selectedFile = file;
          this.selectedFileName = file.name;
        } else {
          this.selectedFile = null;
          this.selectedFileName = "Chưa có tệp nào được chọn !";
        }
        this.$emit("notifyChangeFile", file);
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện thông báo dowload file mẫu
     */
    handleClickDowloadTemplateFile() {
      this.$emit("notifyClickedDowloadTemplateFile");
    },
  },
};
</script>
<style lang="css" scoped>
@import "./file-selection.css";
</style>
