<template lang="">
  <div class="import-current-step-content">
    <div>
      {{ $_MisaResources.appText.excelImportTitle.importText.prepareFile }}
    </div>
    <div class="file-selection">
      <div class="file-selected-name" :title="selectedFileName">
        {{ selectedFileName }}
      </div>
      <misa-button
        buttonClass="normal-button"
        :buttonName="$_MisaResources.appText.excelImportTitle.dowload.select"
        @click="selectFile"
      />
      <input type="file" ref="inputFileRef" @change="handleFileChange" hidden />
    </div>
    <div>
      {{ $_MisaResources.appText.excelImportTitle.dowload.templateFile }}
      <span
        class="dowload-template-hyperlink"
        @click="handleClickDowloadTemplateFile"
        >{{ $_MisaResources.appText.excelImportTitle.dowload.here }}</span
      >
    </div>
  </div>
</template>
<script>
export default {
  name: "ImportFileSelection",
  emits: ["notifyChangeFile", "notifyClickedDowloadTemplateFile"],

  props: {
    postedFile: { required: false },
  },

  data() {
    return {
      selectedFile: null,
      selectedFileName:
        this.$_MisaResources.appText.excelImportTitle.importText.noFileSelected,
    };
  },

  watch: {
    /**
     * Author: PNNHai
     * Date:
     * Description: Thực hiện gán lại file khi đã có file chọn trước
     */
    postedFile: function () {
      try {
        this.selectedFile = this.postedFile;
        if (this.selectedFile) {
          this.selectedFileName = this.selectedFile?.name;
        }
      } catch (err) {
        console.error(err);
      }
    },
  },

  /**
   * Author: PNNHai
   * Date:
   * Description: Thực hiện gán lại file khi đã có file chọn trước
   */
  created() {
    try {
      this.selectedFile = this.postedFile;
      if (this.selectedFile) {
        this.selectedFileName = this.selectedFile?.name;
      }
    } catch (err) {
      console.error(err);
    }
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
          this.selectedFileName =
            this.$_MisaResources.appText.excelImportTitle.importText.noFileSelected;
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
      try {
        this.$emit("notifyClickedDowloadTemplateFile");
      } catch (err) {
        console.error(err);
      }
    },
  },
};
</script>
<style lang="css" scoped>
@import "./file-selection.css";
</style>
