<template lang="">
  <div class="form-import-overlay">
    <div class="form-import">
      <div class="form-import-header">
        <h3 class="form-import-heading">
          {{
            $_MisaResources.appText.excelImportTitle.importText
              .employeeImportTitle
          }}
        </h3>
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
              :postedFile="importFile"
              @notifyChangeFile="handleChangeImportFile"
              @notifyClickedDowloadTemplateFile="
                handleDowloadEmployeeTemplateFile
              "
            />
            <import-data-checking
              v-else-if="currentStep === 2"
              :importObjectResources="importObjectResources"
              :importRecordsData="importTableData"
              :validRecordsCount="validRecordsCount"
              :invalidRecordsCount="invalidRecordsCount"
              :totalImportCount="totalImportRecordsCount"
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
              :buttonName="
                $_MisaResources.appText.excelImportTitle.importText.buttons
                  .helper
              "
              buttonClass="normal-button"
              iconClass="helper-import-icon"
            />
          </div>
          <div class="form-import-footer-right">
            <misa-button
              :buttonName="
                $_MisaResources.appText.excelImportTitle.importText.buttons
                  .previous
              "
              buttonClass="normal-button"
              iconClass="previous-step-icon"
              iconDirection="left"
              :isDisable="currentStep === 1 ? true : false"
              v-if="currentStep === 1 || currentStep === 2"
              @click.stop="handleClickPreviousStep"
            />
            <misa-button
              :buttonName="
                $_MisaResources.appText.excelImportTitle.importText.buttons.next
              "
              buttonClass="normal-button right-direction-button-icon"
              iconClass="next-step-icon"
              v-if="currentStep === 1"
              @click.stop="handleClickNextStep"
            />
            <misa-button
              :buttonName="
                $_MisaResources.appText.excelImportTitle.importText.buttons
                  .execute
              "
              buttonClass="normal-button"
              iconClass="confirm-step-icon"
              v-else-if="currentStep === 2"
              @click.stop="handleClickConfirmImport"
            />
            <misa-button
              :buttonName="
                $_MisaResources.appText.excelImportTitle.importText.buttons
                  .cancel
              "
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

import employeeImportResources from "@/js/helpers/employee-import-resources.js";
import employeeService from "@/js/services/employee-service";

export default {
  name: "MisaImportForm",
  components: {
    ImportProgress,
    ImportFileSelection,
    ImportDataChecking,
    ImportResult,
  },

  emits: ["notifyHideExcelImportForm", "notifyImportSuccess"],

  data() {
    return {
      importSteps: [
        {
          stepIndex: 1,
          stepName:
            this.$_MisaResources.appText.excelImportTitle.importEmployeeSteps
              .chooseSourceFile,
        },
        {
          stepIndex: 2,
          stepName:
            this.$_MisaResources.appText.excelImportTitle.importEmployeeSteps
              .checkingData,
        },
        {
          stepIndex: 3,
          stepName:
            this.$_MisaResources.appText.excelImportTitle.importEmployeeSteps
              .importResult,
        },
      ],
      currentStep: 1,

      importFile: null,

      validRecordsCount: 0,

      invalidRecordsCount: 0,

      totalImportRecordsCount: 0,

      // Object cho các cột để hiển thị kết quả sau khi đọc file
      importObjectResources: {},

      importTableData: [],
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
            return `${this.$_MisaResources.appText.excelImportTitle.importEmployeeSteps.step} ${currentStep.stepIndex}: ${currentStep.stepName}`;
          }
        }
        return null;
      } catch (err) {
        console.error(err);
        return null;
      }
    },
  },

  /**
   * Author: PNNHai
   * Date:
   * Description: Gán resources
   */
  created() {
    try {
      this.importObjectResources = employeeImportResources;
    } catch (err) {
      console.error(err);
    }
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

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện dowload file template cho employee
     */
    async dowloadTemplateFile() {
      try {
        this.$store.state.isLoading = true;
        const res = await employeeService.dowloadTemplateFile("employee");
        return res;
      } catch (err) {
        console.error(err);
      } finally {
        this.$store.state.isLoading = false;
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện dowload file template
     */
    async handleDowloadEmployeeTemplateFile() {
      try {
        const res = await this.dowloadTemplateFile();
        if (res.success) {
          // Xuất file mẫu
          this.dowloadExcelFile(res.data, "Danh sach nhan vien.xlsx");

          this.$store.commit("addToast", {
            type: "success",
            message:
              this.$_MisaResources.appText.employeePageText.successAction
                .dowloadTemplateExcelImportFileSuccess,
          });
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {*} byteData : Mảng byte dữ liệu data
     * @param {*} fileName : tên file muốn xuất
     * Description: Hàm thực hiện xuất file excel
     */
    dowloadExcelFile(byteData, fileName) {
      try {
        const blob = new Blob([byteData], {
          type: "application/vnd.ms-excel",
        });
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("download", fileName);
        link.style.display = "none";
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        window.URL.revokeObjectURL(url);
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện xử lý khi file gửi lên để import
     */
    async handlePostExcelImportFile() {
      try {
        this.$store.state.isLoading = true;
        const formData = new FormData();
        formData.append("importFile", this.importFile);
        const userId = this.$store.state.loginStatus.userId;
        const res = await employeeService.importExcelFile(userId, formData);
        return res;
      } catch (err) {
        console.error(err);
      } finally {
        this.$store.state.isLoading = false;
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Thực hiện xử lý khi click trở về
     */
    handleClickPreviousStep() {
      try {
        if (this.currentStep === 2) {
          this.currentStep--;
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Thực hiện xử lý khi nhấn next file (đọc file và hiển thị lỗi)
     */
    async handleClickNextStep() {
      try {
        if (this.currentStep === 1) {
          const res = await this.handlePostExcelImportFile();
          if (res?.success) {
            this.currentStep++;
            this.importTableData = res.data;
            if (res.data?.length > 0) {
              this.totalImportRecordsCount = res.data.length;
              this.validRecordsCount = res.data.filter(
                (obj) => obj.errors.length === 0
              ).length;

              this.invalidRecordsCount =
                this.totalImportRecordsCount - this.validRecordsCount;
            }
          }
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Thực hiện xử lý khi click xác nhận import
     */
    async handleClickConfirmImport() {
      try {
        if (this.validRecordsCount > 0) {
          this.$store.state.isLoading = true;
          const userId = this.$store.state.loginStatus.userId;
          const res = await employeeService.confirmImport(
            userId,
            this.$_MisaEnums.COMFIRM_TYPE.AGREE
          );
          if (res?.success) {
            this.currentStep++;

            // thông báo nhập khẩu thành công
            this.$store.commit("addToast", {
              type: "success",
              message:
                this.$_MisaResources.appText.employeePageText.successAction
                  .importSuccess,
            });

            this.$emit("notifyImportSuccess");
          }
        } else {
          this.$store.state.dialogNotify.isShow = true;
          this.$store.state.dialogNotify.text =
            this.$_MisaResources.errorHandle.noValidRecordsToImport;
        }
      } catch (err) {
        console.error(err);
      } finally {
        this.$store.state.isLoading = false;
      }
    },
  },
};
</script>
<style lang="css" scoped>
@import "./import-form.css";
</style>
