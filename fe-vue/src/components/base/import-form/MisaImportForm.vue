<template lang="">
  <div class="form-import-overlay">
    <div class="form-import">
      <div class="form-import-header">
        <h3 class="form-import-heading">Nhập khẩu nhân viên</h3>
        <div
          class="form-import-header-close"
          @click.stop="handleHideImportForm"
        ></div>
      </div>

      <div class="form-import-content">
        <h4 class="form-import-current-step">{{ currentStepDisplay }}</h4>
        <div class="form-import-main">
          <import-progress
            :importSteps="importSteps"
            :currentStepIndex="currentStep"
          />

          <div class="form-import-current-content">
            <import-file-selection
              v-if="currentStep === 1"
              @notifyChangeFile="handleChangeImportFile"
              @notifyClickedDowloadTemplateFile="handleDowloadTemplateFile"
            />
            <import-data-checking
              v-else-if="currentStep === 2"
              :importObjectResources="importObjectResources"
              :importRecordsData="importRecordsData"
              :validRecordsCount="validRecordsCount"
              :invalidRecordsCount="invalidRecordsCount"
            />
            <import-result
              v-else-if="currentStep === 3"
              :validRecordsCount="validRecordsCount"
              :invalidRecordsCount="invalidRecordsCount"
            />
          </div>
        </div>
        <div class="form-import-footer">
          <div class="form-import-footer-left">
            <misa-button
              buttonName="Giúp"
              buttonClass="normal-button"
              iconClass="helper-import-icon"
            />
          </div>
          <div class="form-import-footer-right">
            <misa-button
              buttonName="Quay lại"
              buttonClass="normal-button"
              iconClass="previous-step-icon"
              iconDirection="left"
              :isDisable="currentStep === 1 ? true : false"
              v-if="currentStep === 1 || currentStep === 2"
              @click.stop="handleClickPreviousStep"
            />
            <misa-button
              buttonName="Tiếp theo"
              buttonClass="normal-button right-direction-button-icon"
              iconClass="next-step-icon"
              v-if="currentStep === 1"
              @click.stop="handleClickNextStep"
            />
            <misa-button
              buttonName="Thực hiện"
              buttonClass="normal-button"
              iconClass="confirm-step-icon"
              v-else-if="currentStep === 2"
              @click.stop="handleClickConfirmImport"
            />
            <misa-button
              buttonName="Đóng"
              buttonClass="normal-button"
              iconClass="cancel-icon"
              @click="handleHideImportForm"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import ImportProgress from "./import-progress/ImportProgress.vue";
import ImportFileSelection from "./import-steps/file-selection/ImportFileSelection.vue";
import ImportDataChecking from "./import-steps/data-checking/ImportDataChecking.vue";
import ImportResult from "./import-steps/import-result/ImportResult.vue";
export default {
  name: "MisaImportForm",
  components: {
    ImportProgress,
    ImportFileSelection,
    ImportDataChecking,
    ImportResult,
  },

  emits: [
    "notifyHideExcelImportForm",
    "notifyClickedDowloadTemplateFile",
    "notifyPostExcelImportFile",
  ],

  props: {
    // Resources để hiển thị kết quả sau khi import file
    importObjectResources: { type: Object, required: true },

    // Dữ liệu được hiển thị sau khi check các bản ghi sau khi đọc file
    importRecordsData: { type: Array, required: true },
  },

  data() {
    return {
      currentStep: 1,
      importSteps: [
        {
          stepIndex: 1,
          stepName: "Chọn tệp nguồn",
        },
        {
          stepIndex: 2,
          stepName: "Kiểm tra dữ liệu",
        },
        {
          stepIndex: 3,
          stepName: "Kết quả nhập khẩu",
        },
      ],
      importFile: null,
      validRecordsCount: 0,
      invalidRecordsCount: 0,
    };
  },

  computed: {
    /**
     * Author: PNNHai
     * Date:
     * Description: Tính toán hiển thị bước hiện tại để hiển thị
     */
    currentStepDisplay() {
      try {
        if (this.importSteps && this.importSteps.length > 0) {
          const currentStep = this.importSteps.find(
            (step) => step.stepIndex === this.currentStep
          );
          if (currentStep) {
            return `Bước ${currentStep.stepIndex}: ${currentStep.stepName}`;
          }
        }
        return null;
      } catch (err) {
        console.error(err);
        return null;
      }
    },
  },

  methods: {
    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện thông báo cho component cha ẩn form import đi
     */
    handleHideImportForm() {
      try {
        this.$emit("notifyHideExcelImportForm");
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện xử lý khi file thay đổi
     */
    handleChangeImportFile(file) {
      try {
        this.importFile = file;
      } catch (err) {
        console.error(err);
      }
    },

    handleDowloadTemplateFile() {
      this.$emit("notifyClickedDowloadTemplateFile");
    },

    handleClickPreviousStep() {
      if (this.currentStep === 2) {
        this.currentStep--;
      }
    },

    handleClickNextStep() {
      if (this.currentStep === 1) {
        this.currentStep++;
        this.$emit("notifyPostExcelImportFile", this.importFile);
      }
    },

    handleClickConfirmImport() {
      this.currentStep++;
    },
  },
};
</script>
<style lang="css" scoped>
@import "./import-form.css";
</style>
