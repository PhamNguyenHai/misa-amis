<template lang="">
  <div class="form-overlay">
    <form class="form">
      <div class="form-header">
        <div class="form-header-left">
          <h3 class="form-title">
            {{ formTitle }}
          </h3>

          <misa-checkbox-field
            :label="$_MisaResources.formText.employeeForm.isCustomer"
            checkboxId="customer"
            checkboxClass=""
            v-model="formData.isCustomer"
          />

          <misa-checkbox-field
            :label="$_MisaResources.formText.employeeForm.isProvider"
            checkboxId="provider"
            checkboxClass=""
            v-model="formData.isProvider"
          />
        </div>
        <div class="form-header-right">
          <div class="form-support-icon"></div>
          <div
            class="form-close-icon"
            @click.stop="onClickHideFormButton"
          ></div>
        </div>
      </div>

      <div class="form-main">
        <misa-text-field
          ref="employeeCode"
          :label="$_MisaResources.formText.employeeForm.employeeCode.title"
          :title="$_MisaResources.formText.employeeForm.employeeCode.tooltip"
          :fieldClass="[
            { invalid: errorMessages.employeeCode },
            'form-field-area',
            'required',
          ]"
          :inputTitle="errorMessages.employeeCode"
          textFieldId="employeeCode"
          textFieldClass="size-s"
          v-model="formData.employeeCode"
          @notifyInputText="
            validateField(
              'employeeCode',
              $_MisaResources.formText.employeeForm.employeeCode.title
            )
          "
          @notifyBlurInput="
            validateField(
              'employeeCode',
              $_MisaResources.formText.employeeForm.employeeCode.title
            )
          "
        />

        <misa-text-field
          ref="employeeName"
          :label="$_MisaResources.formText.employeeForm.employeeName.title"
          :title="$_MisaResources.formText.employeeForm.employeeName.tooltip"
          :fieldClass="[
            { invalid: errorMessages.employeeName },
            'form-field-area',
            'required',
          ]"
          :inputTitle="errorMessages.employeeName"
          textFieldId="employeeName"
          textFieldClass="size-l"
          v-model="formData.employeeName"
          @notifyInputText="
            validateField(
              'employeeName',
              $_MisaResources.formText.employeeForm.employeeName.title
            )
          "
          @notifyBlurInput="
            validateField(
              'employeeName',
              $_MisaResources.formText.employeeForm.employeeName.title
            )
          "
        />

        <misa-date-field
          ref="dateOfBirth"
          :label="$_MisaResources.formText.employeeForm.dateOfBirth.title"
          :title="$_MisaResources.formText.employeeForm.dateOfBirth.tooltip"
          :fieldClass="[
            { invalid: errorMessages.dateOfBirth },
            'form-field-area',
            'margin-left-16',
          ]"
          :inputTitle="errorMessages.dateOfBirth"
          dateInputId="dateOfBirth"
          dateInputClass="size-s"
          v-model="formData.dateOfBirth"
          @notifyChangeDate="
            validateField(
              'dateOfBirth',
              $_MisaResources.formText.employeeForm.dateOfBirth.title
            )
          "
        />

        <div class="form-field-area gender-field-area">
          <label>
            {{ $_MisaResources.formText.employeeForm.gender.genderTitle }}
          </label>

          <div class="gender-group">
            <misa-radio-field
              :label="$_MisaResources.formText.employeeForm.gender.male"
              fieldClass="gender-field"
              radioId="male"
              radioFieldGroup="gender"
              radioClass=""
              :value="$_MisaEnums.GENDER.MALE"
              v-model="formData.gender"
            />

            <misa-radio-field
              :label="$_MisaResources.formText.employeeForm.gender.female"
              fieldClass="gender-field"
              radioId="female"
              radioFieldGroup="gender"
              radioClass=""
              :value="$_MisaEnums.GENDER.FEMALE"
              v-model="formData.gender"
            />

            <misa-radio-field
              :label="$_MisaResources.formText.employeeForm.gender.other"
              fieldClass="gender-field"
              radioId="orther"
              radioFieldGroup="gender"
              radioClass=""
              :value="$_MisaEnums.GENDER.ORTHER"
              v-model="formData.gender"
            />
          </div>
        </div>

        <misa-combobox
          ref="departmentId"
          :label="$_MisaResources.formText.employeeForm.departmentId.title"
          :title="$_MisaResources.formText.employeeForm.departmentId.tooltip"
          :fieldClass="[
            { invalid: errorMessages.departmentId },
            'form-field-area',
            'required',
            'combobox-xl',
          ]"
          :inputTitle="errorMessages.departmentId"
          comboboxId="department"
          :dataResources="departments"
          keyDisplayName="departmentName"
          keyValue="departmentId"
          v-model="formData.departmentId"
          @notifyComboboxSearchTextBlur="
            validateField(
              'departmentId',
              $_MisaResources.formText.employeeForm.departmentId.title
            )
          "
          @notifyItemSelected="
            validateField(
              'departmentId',
              $_MisaResources.formText.employeeForm.departmentId.title
            )
          "
        />

        <misa-text-field
          ref="identityNumber"
          :label="$_MisaResources.formText.employeeForm.identityNumber.title"
          :title="$_MisaResources.formText.employeeForm.identityNumber.tooltip"
          :fieldClass="[
            { invalid: errorMessages.identityNumber },
            'form-field-area',
            'margin-left-16',
          ]"
          textFieldId="identityNumber"
          textFieldClass="size-l"
          :inputTitle="errorMessages.identityNumber"
          v-model="formData.identityNumber"
          @notifyInputText="
            validateField(
              'identityNumber',
              $_MisaResources.formText.employeeForm.identityNumber.title
            )
          "
          @notifyBlurInput="
            validateField(
              'identityNumber',
              $_MisaResources.formText.employeeForm.identityNumber.title
            )
          "
        />

        <misa-date-field
          ref="identityIssuedDate"
          :label="
            $_MisaResources.formText.employeeForm.identityIssuedDate.title
          "
          :title="
            $_MisaResources.formText.employeeForm.identityIssuedDate.tooltip
          "
          :fieldClass="[
            { invalid: errorMessages.identityIssuedDate },
            'form-field-area',
          ]"
          dateInputId="identityIssuedDate"
          dateInputClass="size-s"
          :inputTitle="errorMessages.identityIssuedDate"
          v-model="formData.identityIssuedDate"
          @notifyChangeDate="
            validateField(
              'identityIssuedDate',
              $_MisaResources.formText.employeeForm.identityIssuedDate.title
            )
          "
        />

        <misa-text-field
          ref="positionName"
          :label="$_MisaResources.formText.employeeForm.positionName.title"
          :title="$_MisaResources.formText.employeeForm.positionName.tooltip"
          :fieldClass="[
            { invalid: errorMessages.positionName },
            'form-field-area',
          ]"
          textFieldId="positionName"
          textFieldClass="size-xl"
          :inputTitle="errorMessages.positionName"
          v-model="formData.positionName"
          @notifyInputText="
            validateField(
              'positionName',
              $_MisaResources.formText.employeeForm.positionName.title
            )
          "
          @notifyBlurInput="
            validateField(
              'positionName',
              $_MisaResources.formText.employeeForm.positionName.title
            )
          "
        />

        <misa-text-field
          ref="identityIssuedPlace"
          :label="
            $_MisaResources.formText.employeeForm.identityIssuedPlace.title
          "
          :title="
            $_MisaResources.formText.employeeForm.identityIssuedPlace.tooltip
          "
          :fieldClass="[
            { invalid: errorMessages.identityIssuedPlace },
            'form-field-area',
            'margin-left-16',
          ]"
          textFieldId="identityIssuedPlace"
          textFieldClass="size-xl"
          :inputTitle="errorMessages.identityIssuedPlace"
          v-model="formData.identityIssuedPlace"
          @notifyInputText="
            validateField(
              'identityIssuedPlace',
              $_MisaResources.formText.employeeForm.identityIssuedPlace.title
            )
          "
          @notifyBlurInput="
            validateField(
              'identityIssuedPlace',
              $_MisaResources.formText.employeeForm.identityIssuedPlace.title
            )
          "
        />

        <misa-text-field
          ref="address"
          :label="$_MisaResources.formText.employeeForm.address.title"
          :title="$_MisaResources.formText.employeeForm.address.tooltip"
          :fieldClass="[
            { invalid: errorMessages.address },
            'form-field-area',
            'margin-top-20',
          ]"
          textFieldId="address"
          textFieldClass="size-xxl"
          :inputTitle="errorMessages.address"
          v-model="formData.address"
          @notifyInputText="
            validateField(
              'address',
              $_MisaResources.formText.employeeForm.address.title
            )
          "
          @notifyBlurInput="
            validateField(
              'address',
              $_MisaResources.formText.employeeForm.address.title
            )
          "
        />

        <misa-text-field
          ref="phoneNumber"
          :label="$_MisaResources.formText.employeeForm.phoneNumber.title"
          :title="$_MisaResources.formText.employeeForm.phoneNumber.tooltip"
          :fieldClass="[
            { invalid: errorMessages.phoneNumber },
            'form-field-area',
          ]"
          textFieldId="phoneNumber"
          textFieldClass="size-m"
          :inputTitle="errorMessages.phoneNumber"
          v-model="formData.phoneNumber"
          @notifyInputText="
            validateField(
              'phoneNumber',
              $_MisaResources.formText.employeeForm.phoneNumber.title
            )
          "
          @notifyBlurInput="
            validateField(
              'phoneNumber',
              $_MisaResources.formText.employeeForm.phoneNumber.title
            )
          "
        />

        <misa-text-field
          ref="landlineNumber"
          :label="$_MisaResources.formText.employeeForm.landlineNumber.title"
          :title="$_MisaResources.formText.employeeForm.landlineNumber.tooltip"
          :fieldClass="[
            { invalid: errorMessages.landlineNumber },
            'form-field-area',
          ]"
          textFieldId="landlineNumber"
          textFieldClass="size-m"
          :inputTitle="errorMessages.landlineNumber"
          v-model="formData.landlineNumber"
          @notifyInputText="
            validateField(
              'landlineNumber',
              $_MisaResources.formText.employeeForm.landlineNumber.title
            )
          "
          @notifyBlurInput="
            validateField(
              'landlineNumber',
              $_MisaResources.formText.employeeForm.landlineNumber.title
            )
          "
        />

        <misa-text-field
          ref="email"
          :label="$_MisaResources.formText.employeeForm.email.title"
          :title="$_MisaResources.formText.employeeForm.email.tooltip"
          :fieldClass="[{ invalid: errorMessages.email }, 'form-field-area']"
          textFieldId="email"
          textFieldClass="size-m"
          :inputTitle="errorMessages.email"
          v-model="formData.email"
          @notifyInputText="
            validateField(
              'email',
              $_MisaResources.formText.employeeForm.email.title
            )
          "
          @notifyBlurInput="
            validateField(
              'email',
              $_MisaResources.formText.employeeForm.email.title
            )
          "
        />

        <misa-text-field
          ref="bankNumber"
          :label="$_MisaResources.formText.employeeForm.bankNumber.title"
          :title="$_MisaResources.formText.employeeForm.bankNumber.tooltip"
          :fieldClass="[
            { invalid: errorMessages.bankNumber },
            'form-field-area',
          ]"
          textFieldId="bankNumber"
          textFieldClass="size-m"
          :inputTitle="errorMessages.bankNumber"
          v-model="formData.bankNumber"
          @notifyInputText="
            validateField(
              'bankNumber',
              $_MisaResources.formText.employeeForm.bankNumber.title
            )
          "
          @notifyBlurInput="
            validateField(
              'bankNumber',
              $_MisaResources.formText.employeeForm.bankNumber.title
            )
          "
        />

        <misa-text-field
          ref="bankName"
          :label="$_MisaResources.formText.employeeForm.bankName.title"
          :title="$_MisaResources.formText.employeeForm.bankName.tooltip"
          :fieldClass="[{ invalid: errorMessages.bankName }, 'form-field-area']"
          textFieldId="bankName"
          textFieldClass="size-m"
          :inputTitle="errorMessages.bankName"
          v-model="formData.bankName"
          @notifyInputText="
            validateField(
              'bankName',
              $_MisaResources.formText.employeeForm.bankName.title
            )
          "
          @notifyBlurInput="
            validateField(
              'bankName',
              $_MisaResources.formText.employeeForm.bankName.title
            )
          "
        />

        <misa-text-field
          ref="bankBranch"
          :label="$_MisaResources.formText.employeeForm.bankBranch.title"
          :title="$_MisaResources.formText.employeeForm.bankBranch.tooltip"
          :fieldClass="[
            { invalid: errorMessages.bankBranch },
            'form-field-area',
          ]"
          textFieldId="bankBranch"
          textFieldClass="size-m"
          :inputTitle="errorMessages.bankBranch"
          v-model="formData.bankBranch"
          @notifyInputText="
            validateField(
              'bankBranch',
              $_MisaResources.formText.employeeForm.bankBranch.title
            )
          "
          @notifyBlurInput="
            validateField(
              'bankBranch',
              $_MisaResources.formText.employeeForm.bankBranch.title
            )
          "
        />
      </div>

      <div class="form-footer">
        <div class="form-footer--left">
          <misa-button
            :buttonName="$_MisaResources.buttons.cancel"
            buttonClass="button normal-button"
            @click.stop="onClickHideFormButton"
          />
        </div>
        <div class="form-footer--right">
          <misa-button
            :buttonName="$_MisaResources.buttons.save.name"
            :buttonTooltips="$_MisaResources.buttons.save.tooltip"
            buttonClass="button normal-button"
            @click.stop="onClickSaveButton"
          />
          <misa-button
            :buttonName="$_MisaResources.buttons.saveAndAdd.name"
            :buttonTooltips="$_MisaResources.buttons.saveAndAdd.tooltip"
            buttonClass="button primary-button"
            @click.stop="onClickSaveAndAddButton"
          />
        </div>
      </div>
    </form>
  </div>

  <misa-dialog
    v-if="dialog.isShow"
    :dialogType="dialog.type"
    :numberOfButton="dialog.numberOfButton"
    @notifyCloseDialog="handleCloseDialog"
    @notifyDialogResponded="handleDialogResponded"
  >
    <template v-slot:dialog-content>
      <!-- Nếu chọn chức năng show list -->
      <ul class="dialog__list-message" v-if="itemListDialogContent.length > 0">
        <li v-for="(item, index) in itemListDialogContent" :key="index">
          {{ item }}
        </li>
      </ul>
      <p class="dialog__message" v-else>{{ dialog.text }}</p>
    </template>
  </misa-dialog>
</template>

<style lang="css" scoped>
@import "./employee-form.css";
</style>

<script>
import getFieldInvalidError from "@/js/helpers/validate.js";
import employeeService from "@/js/services/EmployeeService";
import departmentService from "@/js/services/DepartmentService";

export default {
  name: "EmployeeForm",
  props: {
    //Id của nhân viên cần sửa nếu ko có thì là xóa
    editEmployeeId: { type: String, default: null },

    // type của form (update/insert)
    formDataType: { type: Number, required: true },
  },

  emits: ["notifyHideForm", "notifySubmittedForm"],

  data() {
    return {
      itemListDialogContent: [],
      formData: {
        employeeCode: null,
        isCustomer: null,
        isProvider: null,
        employeeName: null,
        dateOfBirth: null,
        gender: null,
        departmentId: null,
        positionName: null,
        identityNumber: null,
        identityIssuedDate: null,
        identityIssuedPlace: null,
        address: null,
        phoneNumber: null,
        landlineNumber: null,
        email: null,
        bankNumber: null,
        bankName: null,
        bankBranch: null,
      },

      validateFormRules: {
        employeeCode: ["required", "maxLength_20"],
        employeeName: ["required", "maxLength_100"],
        dateOfBirth: ["noMoreThanCurrentDate"],
        departmentId: ["required"],
        positionName: ["maxLength_255"],
        identityNumber: ["identityNumber", "maxLength_25"],
        identityIssuedDate: ["noMoreThanCurrentDate"],
        identityIssuedPlace: ["maxLength_255"],
        address: ["maxLength_255"],
        phoneNumber: ["phoneNumber", "maxLength_50"],
        landlineNumber: ["phoneNumber", "maxLength_50"],
        email: ["email", "maxLength_100"],
        bankNumber: ["numberOnly", "maxLength_50"],
        bankName: ["maxLength_255"],
        bankBranch: ["maxLength_255"],
      },

      errorMessages: {
        employeeCode: "",
        employeeName: "",
        dateOfBirth: "",
        departmentId: "",
        positionName: "",
        identityNumber: "",
        identityIssuedDate: "",
        identityIssuedPlace: "",
        address: "",
        phoneNumber: "",
        landlineNumber: "",
        email: "",
        bankNumber: "",
        bankName: "",
        bankBranch: "",
      },

      departments: [],

      formMode: null,

      copyFormData: {},

      dialog: {
        isShow: false,
        type: null,
        text: null,
        numberOfButton: null,
      },
    };
  },

  /**
   * Author : PNNHai
   * Date:
   * Description: Trong created sẽ
   */
  async created() {
    try {
      // Gán sự kiện keydown
      window.addEventListener("keydown", this.handleShortHandKeyDown);

      // Gán formMode = props formDataType
      this.formMode = this.formDataType;
      // get departments
      this.departments = await this.getDepartments();
      // binding dữ liệu lên form ứng với form mode
      this.bindingDataToForm(this.formMode);
    } catch (err) {
      console.error(err);
    }
  },

  unmounted() {
    window.removeEventListener("keydown", this.handleShortHandKeyDown);
  },

  computed: {
    // Hàm để thực hiện hiển thị form title theo formMode
    formTitle() {
      // Đối với trường hợp thêm mới và nhân bản
      if (
        this.formMode === this.$_MisaEnums.FORM_MODE.ADD ||
        this.formMode === this.$_MisaEnums.FORM_MODE.DUPLICATE
      ) {
        return this.$_MisaResources.formText.employeeForm.addFormTitle;
      }
      // Đối với trường hợp cập nhật
      else {
        return this.$_MisaResources.formText.employeeForm.editFormTitle;
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: So sánh giá trị của biến ban đầu và biến copy có khác nhau không
     */
    isFormChanged() {
      // So sánh giá trị hiện tại với giá trị ban đầu
      // Chuyển về JSON để so sánh
      return (
        JSON.stringify(this.formData) !== JSON.stringify(this.copyFormData)
      );
    },
  },

  methods: {
    // ======================   2 HÀM DƯỚI ĐỂ VALIDATE FIELD & FORM ============================
    /**
     * Author : PNNHai
     * Date:
     * @param {String} fieldName : tên trường thực hiện validate
     * Description: Hàm để validate cho từng field trong form
     */
    validateField(fieldName, fieldLabel) {
      try {
        // reset error trước khi validate
        this.errorMessages[fieldName] = "";
        // Lấy ra toàn bộ validate rules của field ở obj validateFormRules khi truyền field name
        const fieldRules = this.validateFormRules[fieldName];

        // Nếu validate rules được đăng kí tại obj validateFormRules và tồn tại thì mới kiểm tra
        if (fieldRules) {
          // Với trường hợp trong validate rules ko có required thì chỉ validate
          // các trường còn lại khi field đó có giá trị
          if (!fieldRules.includes("required")) {
            // Kiểm tra xem nếu field đó có giá trị thì mới validate các trường còn lại
            // Nếu có giá trị thì thực hiện từng rule functions ứng với các rules có trong field đó
            if (this.formData[fieldName]) {
              // Thực hiện gán errorMessage của field tương ứng
              this.errorMessages[fieldName] = getFieldInvalidError(
                fieldRules,
                this.formData[fieldName],
                fieldLabel
              );
            }
            // Với trường hợp trong validate rules có required thì sẽ validate tất cả rules
          } else {
            // Thực hiện gán errorMessage của field tương ứng
            this.errorMessages[fieldName] = getFieldInvalidError(
              fieldRules,
              this.formData[fieldName],
              fieldLabel
            );
          }
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date:
     * Description: Hàm để validate toàn bộ form (kết hợp hiển thị danger bao ngoài field và dialog error)
     */
    validateForm() {
      try {
        this.itemListDialogContent = [];
        // Kiểm tra lỗi trước khi submit
        // Validate với các field có đăng kí validate ở obj validateFormRules
        let isOk = true;
        for (let field in this.errorMessages) {
          // Lấy ra label ứng với từng trường để hiển thị trong error
          const fieldLabel =
            this.$_MisaResources.formText.employeeForm[field].title;
          // validate field tương ứng
          this.validateField(field, fieldLabel);

          // Kiểm tra nếu errorMessages ứng với trường đó có nội dung thì isOk = false
          const errorText = this.errorMessages[field];
          if (errorText !== "") {
            // Nếu trường có lỗi -> push vào mảng để hiển thị lên dialog
            this.itemListDialogContent.push(errorText);
            isOk = false;
          }
        }

        // Nếu có error ở các trường -> hiển thị dialog error
        if (this.itemListDialogContent.length > 0) {
          this.dialog.isShow = true;
          this.dialog.type = "danger";
          this.dialog.numberOfButton =
            this.$_MisaEnums.DIALOG_TYPE_BUTTON.ONE_BUTTON;
        }
        return isOk;
      } catch (err) {
        console.error(err);
      }
    },
    // ==================================================================================================================

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện bắt sự kiện nhanh với key
     * +/ esc -> tắt form
     * +/ control + s -> lưu
     * +/ control + shift + s -> lưu và add
     */
    handleShortHandKeyDown() {
      try {
        event.stopPropagation();
        // Chỉ bắt sự kiện keydown nhanh với form khi không có dialog hiển thị
        if (!this.dialog.isShow && !this.$store.state.dialogNotify.isShow) {
          // Khi nhấn phím esc -> tắt form
          if (event.key === "Escape") {
            event.preventDefault();
            this.onClickHideFormButton();
          }
          // control + s -> lưu thông tin
          else if (event.ctrlKey && event.key === "s") {
            event.preventDefault();
            this.onClickSaveButton();
          }
          // control + shift + s -> lưu và add thông tin
          else if (event.shiftKey && event.ctrlKey && event.code === "KeyS") {
            event.preventDefault();
            this.onClickSaveAndAddButton();
          }
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date:
     * Description: Hàm để load toàn bộ departments để đẩy vào prop cho combobox
     */
    async getDepartments() {
      try {
        const res = await departmentService.get();
        if (res.success) {
          return res.data;
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date:
     * Description: Hàm để lấy mã nhân viên mới từ api về
     */
    async getNewEmployeeCode() {
      try {
        const res = await employeeService.getNewCode();
        if (res.success) {
          return res.data;
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date:
     * Description: Hàm để lấy mã nhân viên mới từ api về
     */
    async getEmployeeById() {
      try {
        const res = await employeeService.getById(this.editEmployeeId);
        if (res.success) {
          return res.data;
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm để ẩn dialog
     */
    handleCloseDialog() {
      try {
        // ẩn dialog đi
        this.dialog.isShow = false;

        // Nếu có trường nào có lỗi thì focus vào field đó
        const firstFieldErrorKey = Object.keys(this.errorMessages).find(
          (key) => this.errorMessages[key] !== ""
        );
        if (firstFieldErrorKey) {
          this.$refs[firstFieldErrorKey].focus();
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm để ẩn form
     */
    handleNotifyCloseForm() {
      try {
        this.$emit("notifyHideForm");
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date :
     * Description : Hàm để xử lý tắt form khi click vào nút tắt
     * +/ Nếu form chưa có gì thay đổi => tắt luôn
     * +/ Nếu form đã thay đổi : Hiển thị confirm cất dữ liệu hay không ?
     *    + Hủy : Tắt dialog và tiếp tục tác vụ
     *    + Không : không cất dl
     *    + Có : Cất dl
     */
    onClickHideFormButton() {
      try {
        const formChanged = this.isFormChanged;
        if (formChanged) {
          // Khi tắt popup đi thì bỏ hết item đang có trong list dialog
          // -> đồng thời cũng có tác dụng ẩn dialog validate
          this.itemListDialogContent = [];

          // Hiển thị dialog cảnh báo form thay đổi
          this.dialog.isShow = true;
          this.dialog.type = "confirm";
          this.dialog.numberOfButton =
            this.$_MisaEnums.DIALOG_TYPE_BUTTON.THREE_BUTTONS;
          this.dialog.text =
            this.$_MisaResources.appText.employeePageText.confirmTitle.ConfirmToSaveWhileDataChanged;
        } else {
          this.handleNotifyCloseForm();
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {Enum(Number)} responseStatus : Kết quả phản hồi của người dùng (NO(0) / YES(1) )
     * Description: Hàm để xử lý khi nhận được thông báo từ dialog đã được phản hồi
     */
    async handleDialogResponded(responseStatus) {
      try {
        // Close dialog
        this.handleCloseDialog();
        await this.handleConfirmCloseChangedForm(responseStatus);
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {Enum(Number)} response : Kết quả người dùng từ dialog (NO(0) / YES(1) )
     * Description: Hàm để xử lý khi người dùng xác nhận cất dữ liệu hay không khi tắt form lúc form đã thay đổi dữ liệu
     * +/ Nếu người dùng chọn NO -> Thông báo tắt form ra ngoài
     * +/ Nếu người dùng chọn có -> bắt vào sự kiện onClickSaveButton để thực hiện
     */
    async handleConfirmCloseChangedForm(response) {
      try {
        // Nếu response = NO -> Tắt form
        if (response === this.$_MisaEnums.DIALOG_RESPONSE.NO) {
          this.handleNotifyCloseForm();
        } else {
          await this.onClickSaveButton();
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date :
     * Description : Hàm xóa các error của các trường. Để reset form
     */
    resetTextFieldError() {
      try {
        // Bỏ toàn bộ error có trong dialog
        this.itemListDialogContent = [];
        for (let keyAttr in this.errorMessages) {
          this.errorMessages[keyAttr] = "";
        }
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Author : PNNHai
     * Date :
     * Description : Hàm xóa dữ liệu của các trường. Để reset form
     */
    resetFieldData() {
      try {
        for (let keyAttr in this.formData) {
          this.formData[keyAttr] = null;
        }

        // set gender mặc định là trường đầu tiên (nam)
        this.formData.gender = this.$_MisaEnums.GENDER.MALE;
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Author : PNNHai
     * Date :
     * @param {Enum(Number)} formMode : chế độ hiện tại của form (ADD (0) /EDIT (1) / DUPLICATE(2) )
     * Description: Hàm để binding data lên form với từng trường hợp form
     * +/ Với form ADD : Lấy Code mới và truyền lên
     * +/ Với form EDIT: Lấy thông tin của nhân viên và truyền lên
     * +/ Với form DUPLICATE: Lấy thông tin của nhân viên truyền lên và thay thế mã bằng mã mới
     * (DUPLICATE: bản chất là ADD)
     *
     * -- SAU KHI BINDING XONG THÌ LẤY GÁN GIÁ TRỊ CHO BIẾN COPY ĐỂ THEO DÕI CÓ THAY ĐỔI KHÔNG
     */
    async bindingDataToForm(formMode) {
      try {
        this.resetFieldData();
        this.resetTextFieldError();
        // Focus vào ô đầu tiên
        this.$refs.employeeCode.focus();

        // Đối với trường hợp edit => binding dữ liệu lên form
        if (formMode === this.$_MisaEnums.FORM_MODE.EDIT) {
          const employee = await this.getEmployeeById();
          if (employee) {
            this.formData = employee;
          }
        }
        // Đối với trường hợp add => binding mã mới lên
        else if (formMode === this.$_MisaEnums.FORM_MODE.ADD) {
          const newEmployeeCode = await this.getNewEmployeeCode();
          if (newEmployeeCode) {
            this.formData.employeeCode = newEmployeeCode;
          }
        }
        // Đối với trường hợp duplicate => binding dữ liệu lên và thay mã = mã mới
        else {
          const newEmployeeCode = await this.getNewEmployeeCode();
          const employee = await this.getEmployeeById();
          if (newEmployeeCode && employee) {
            this.formData = employee;
            this.formData.employeeCode = newEmployeeCode;
          }
        }

        // Coppy formData ban đầu ra một object mới để kiểm tra có thay đổi không
        this.copyFormData = { ...this.formData };
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {Object} employee : Nhân viên muốn thêm mới
     * Description: Hàm thêm mới nhân viên gọi API
     */
    async addNewEmployee(employee) {
      try {
        this.$store.state.isLoading = true;
        const res = await employeeService.post(employee);

        return res;
      } catch (error) {
        console.error(error);
      } finally {
        this.$store.state.isLoading = false;
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {String(Guid)} id : Id của nhân viên muốn update
     * @param {*} employee : Thông tin nhân viên để update
     * Description: Hàm cập nhật thông tin nhân viên gọi API
     */
    async updateEmployee(id, employee) {
      try {
        this.$store.state.isLoading = true;
        const res = await employeeService.put(id, employee);

        return res;
      } catch (error) {
        console.error(error);
      } finally {
        this.$store.state.isLoading = false;
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Submit form với add/update/duplicate(bản chất là add)
     */
    async submitForm() {
      try {
        // Với trường hợp form thêm mới / nhân bản
        let res = {};
        if (
          this.formMode === this.$_MisaEnums.FORM_MODE.ADD ||
          this.formMode === this.$_MisaEnums.FORM_MODE.DUPLICATE
        ) {
          res = await this.addNewEmployee(this.formData);
        }
        // Với trường hợp form update
        else {
          const formChanged = this.isFormChanged;
          if (!formChanged) {
            // Khi tắt popup đi thì bỏ hết item đang có trong list dialog
            // -> đồng thời cũng có tác dụng ẩn dialog validate
            this.itemListDialogContent = [];

            // Hiển thị dialog cảnh báo form thay đổi
            this.dialog.isShow = true;
            this.dialog.type = "warning";
            this.dialog.numberOfButton =
              this.$_MisaEnums.DIALOG_TYPE_BUTTON.ONE_BUTTON;
            this.dialog.text =
              this.$_MisaResources.appText.employeePageText.notifyTitle.notifyNothingChange;
          } else {
            res = await this.updateEmployee(this.editEmployeeId, this.formData);
          }
        }

        return res;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date :
     * Description : Hàm xử lý khi click vào nút lưu
     * Sẽ thông báo ra bên ngoài Sẽ thông báo ra bên ngoài để refres lại bảng và ẩn form đi
     */
    async onClickSaveButton() {
      try {
        // Validate và submit form

        // Nếu validate ok => submit form
        if (this.validateForm() === true) {
          // Gọi hàm submit form
          const res = await this.submitForm();
          if (res?.success) {
            // Thông báo ra bên ngoài EmployeePage đã submit thành công để load lại bảng
            // Với save => ẩn form
            this.$emit(
              "notifySubmittedForm",
              this.$_MisaEnums.FORM_SUBMIT_MODE.SAVE
            );

            // Thông báo lưu thông tin thành công
            // Kiểm tra res.success thành công = true -> add Toast
            this.$store.commit("addToast", {
              type: "success",
              message:
                this.$_MisaResources.appText.employeePageText.successAction
                  .saveDataSuccess,
            });
          }
        }
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Author : PNNHai
     * Date :
     * Description : Hàm xử lý khi click vào nút lưu và thêm mới
     * Sẽ thông báo ra bên ngoài để refres lại bảng nhưng không ẩn form mà tự động tăng mã lên
     */
    async onClickSaveAndAddButton() {
      try {
        // Validate và submit form
        if (this.validateForm() === true) {
          // Gọi hàm submit form
          const res = await this.submitForm();

          if (res?.success) {
            // Thông báo ra bên ngoài EmployeePage đã submit thành công để load lại bảng
            // Với save => ẩn form
            // Với save_add => reset thông tin -> tiếp tục thêm được
            this.$emit(
              "notifySubmittedForm",
              this.$_MisaEnums.FORM_SUBMIT_MODE.SAVE_ADD
            );

            // Thông báo lưu thông tin thành công
            // Kiểm tra res.success thành công = true -> add Toast
            this.$store.commit("addToast", {
              type: "success",
              message:
                this.$_MisaResources.appText.employeePageText.successAction
                  .saveDataSuccess,
            });

            // Sau khi click nút lưu và thêm => chuyển mode -> add (nếu mode hiện tại đg là add => ko đổi)
            // nhưng nếu mode đg là update => add
            this.formMode = this.$_MisaEnums.FORM_MODE.ADD;
            this.bindingDataToForm(this.formMode);
          }
        }
      } catch (error) {
        console.error(error);
      }
    },
  },
};
</script>
