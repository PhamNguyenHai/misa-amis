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
          <misa-button buttonName="Giúp" buttonClass="normal-button" />
          <misa-button buttonName="Đóng" buttonClass="normal-button" />
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

  emits: ["notifyHideExcelImportForm"],

  props: {
    // Resources để hiển thị kết quả sau khi import file
    importObjectResources: { type: Object, required: true },

    // Dữ liệu được hiển thị sau khi check các bản ghi sau khi đọc file
    importRecordsData: { type: Array, required: true },
  },

  data() {
    return {
      currentStep: 2,
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
        console.log(file);
      } catch (err) {
        console.error(err);
      }
    },
  },
};
</script>
<style lang="css" scoped>
@import "./import-form.css";
</style>
