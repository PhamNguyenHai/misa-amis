<template lang="">
  <base-form :formWidth="535" :formMinHeight="490">
    <template v-slot:form-header-content>
      <div class="form-header">
        <div class="form-header-left">
          <h3 class="form-title">
            {{ formTitle }}
          </h3>
        </div>
        <div class="form-header-right">
          <div class="form-support-icon"></div>
          <div
            class="form-close-icon"
            @click.stop="onClickHideFormButton"
          ></div>
        </div>
      </div>
    </template>

    <template v-slot:form-main-content>
      <misa-text-field
        ref="fullName"
        :label="$_MisaResources.formText.userForm.fullName.title"
        :fieldClass="[
          { invalid: errorMessages.fullName },
          'form-field-area',
          'required',
        ]"
        :inputTitle="errorMessages.fullName"
        textFieldId="fullName"
        textFieldClass="size-xl"
        v-model="formData.fullName"
        @notifyInputText="
          validateField(
            'fullName',
            $_MisaResources.formText.userForm.fullName.title
          )
        "
        @notifyBlurInput="
          validateField(
            'fullName',
            $_MisaResources.formText.userForm.fullName.title
          )
        "
      />

      <misa-combobox
        ref="role"
        :label="$_MisaResources.formText.userForm.role.title"
        :fieldClass="[
          { invalid: errorMessages.role },
          'form-field-area',
          'required',
          'combobox-xl',
        ]"
        :inputTitle="errorMessages.role"
        comboboxId="role"
        :dataResources="userRole"
        keyDisplayName="name"
        keyValue="value"
        v-model="formData.role"
        @notifyComboboxSearchTextBlur="
          validateField('role', $_MisaResources.formText.userForm.role.title)
        "
        @notifyItemSelected="
          validateField('role', $_MisaResources.formText.userForm.role.title)
        "
      />

      <misa-text-field
        ref="email"
        :label="$_MisaResources.formText.userForm.email.title"
        :fieldClass="[
          { invalid: errorMessages.email },
          'form-field-area',
          'required',
        ]"
        :inputTitle="errorMessages.email"
        textFieldId="email"
        textFieldClass="size-xl"
        v-model="formData.email"
        @notifyInputText="
          validateField('email', $_MisaResources.formText.userForm.email.title)
        "
        @notifyBlurInput="
          validateField('email', $_MisaResources.formText.userForm.email.title)
        "
      />

      <misa-text-field
        ref="phoneNumber"
        :label="$_MisaResources.formText.userForm.phoneNumber.title"
        :fieldClass="[
          { invalid: errorMessages.phoneNumber },
          'form-field-area',
          'required',
        ]"
        :inputTitle="errorMessages.phoneNumber"
        textFieldId="phoneNumber"
        textFieldClass="size-xl"
        v-model="formData.phoneNumber"
        @notifyInputText="
          validateField(
            'phoneNumber',
            $_MisaResources.formText.userForm.phoneNumber.title
          )
        "
        @notifyBlurInput="
          validateField(
            'phoneNumber',
            $_MisaResources.formText.userForm.phoneNumber.title
          )
        "
      />

      <misa-password-field
        v-if="formMode === $_MisaEnums.FORM_MODE.ADD"
        ref="password"
        :label="$_MisaResources.formText.userForm.password.title"
        :fieldClass="[
          { invalid: errorMessages.password },
          'form-field-area',
          'required',
        ]"
        :inputTitle="errorMessages.password"
        passwordFieldId="password"
        passwordFieldClass="size-xl"
        v-model="formData.password"
        @notifyInputPassword="
          validateField(
            'password',
            $_MisaResources.formText.userForm.password.title
          )
        "
        @notifyBlurPasswordInput="
          validateField(
            'password',
            $_MisaResources.formText.userForm.password.title
          )
        "
      />

      <misa-password-field
        v-if="formMode === $_MisaEnums.FORM_MODE.ADD"
        ref="repeatedPassword"
        :label="$_MisaResources.formText.userForm.repeatedPassword.title"
        :fieldClass="[
          { invalid: errorMessages.repeatedPassword },
          'form-field-area',
          'required',
        ]"
        :inputTitle="errorMessages.repeatedPassword"
        passwordFieldId="repeatedPassword"
        passwordFieldClass="size-xl"
        v-model="formData.repeatedPassword"
        @notifyInputPassword="
          validateField(
            'repeatedPassword',
            $_MisaResources.formText.userForm.repeatedPassword.title
          )
        "
        @notifyBlurPasswordInput="
          validateField(
            'repeatedPassword',
            $_MisaResources.formText.userForm.repeatedPassword.title
          )
        "
      />
    </template>

    <template v-slot:form-footer-content>
      <div class="form-footer--left">
        <misa-button
          buttonId="user-form-cancel"
          :buttonName="$_MisaResources.buttons.cancel"
          buttonClass="button normal-button"
          @click.stop="onClickHideFormButton"
        />
      </div>
      <div class="form-footer--right">
        <misa-button
          buttonId="user-form-save"
          :buttonName="$_MisaResources.buttons.save.name"
          :buttonTooltips="$_MisaResources.buttons.save.tooltip"
          buttonClass="button normal-button"
          @click.stop="onClickSaveButton"
        />
        <misa-button
          buttonId="user-form-save-add"
          :buttonName="$_MisaResources.buttons.saveAndAdd.name"
          :buttonTooltips="$_MisaResources.buttons.saveAndAdd.tooltip"
          buttonClass="button primary-button"
          @click.stop="onClickSaveAndAddButton"
        />
      </div>
    </template>
  </base-form>

  <misa-dialog
    v-if="dialog.isShow"
    :dialogType="dialog.type"
    :dialogText="dialog.text"
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
@import "./user-form.css";
</style>

<script>
import getFieldInvalidError from "@/js/helpers/validate.js";
import userService from "@/js/services/user-service";
import BaseForm from "@/components/base/base-form/BaseForm.vue";

export default {
  name: "UserManagementForm",
  props: {
    //Id của người dùng cần sửa nếu ko có thì là thêm
    editUserId: { type: String, default: null },

    // type của form (update/insert)
    formDataType: { type: Number, required: true },
  },

  components: {
    BaseForm,
  },

  emits: ["notifyHideForm", "notifySubmittedForm"],

  data() {
    return {
      itemListDialogContent: [],

      formData: {
        fullName: null,
        role: 0,
        email: null,
        phoneNumber: null,
        password: null,
        repeatedPassword: null,
      },

      validateFormRules: {
        fullName: ["required", "maxLength_100"],
        role: ["required"],
        email: ["required", "email", "maxLength_100"],
        phoneNumber: ["required", "phoneNumber", "maxLength_50"],
        password: ["required", "minLength_6", "strongPassword", "maxLength_50"],
        repeatedPassword: ["required", "maxLength_50"],
      },

      errorMessages: {
        fullName: null,
        role: null,
        email: null,
        phoneNumber: null,
        password: null,
        repeatedPassword: null,
      },

      userRole: [
        {
          name: "Quản trị viên",
          value: 0,
        },
        {
          name: "Người dùng",
          value: 1,
        },
      ],

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
  created() {
    // Gán sự kiện keydown
    window.addEventListener("keydown", this.handleShortHandKeyDown);

    // Gán formMode = props formDataType
    this.formMode = this.formDataType;
  },

  mounted() {
    this.bindingDataToForm(this.formMode);
  },

  computed: {
    // Hàm để thực hiện hiển thị form title theo formMode
    formTitle() {
      // Đối với trường hợp thêm mới và nhân bản
      if (this.formMode === this.$_MisaEnums.FORM_MODE.ADD) {
        return "Thêm mới người dùng";
      }
      // Đối với trường hợp cập nhật
      else {
        return "Cập nhật thông tin người dùng";
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
          // Nếu field khác password và repeatedPassword thì mới validate
          if (
            this.formMode === this.$_MisaEnums.FORM_MODE.EDIT &&
            (field === "password" || field === "repeatedPassword")
          ) {
            continue;
          } else {
            // Lấy ra label ứng với từng trường để hiển thị trong error
            const fieldLabel =
              this.$_MisaResources.formText.userForm[field].title;
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
        }

        // Nếu mật khẩu và mật khẩu nhập lại không khớp
        if (this.formData.password !== this.formData.repeatedPassword) {
          this.itemListDialogContent.push(
            "Mật khẩu nhập lại và mật khẩu không khớp."
          );
          isOk = false;
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
      this.$emit("notifyHideForm");
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
        await this.handleConfirmCloseChangedForm(responseStatus);

        // Close dialog
        this.handleCloseDialog();
      } catch (error) {
        console.log(error);
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
      } catch (error) {
        console.log(error);
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
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Author : PNNHai
     * Date:
     * Description: Hàm để lấy người dùng bằng id
     */
    async getUserById() {
      try {
        this.$store.state.isLoading = true;
        const res = await userService.getById(this.editUserId);
        if (res?.success) {
          return res.data;
        }
      } catch (error) {
        console.log(error);
      } finally {
        this.$store.state.isLoading = false;
      }
    },

    /**
     * Author : PNNHai
     * Date :
     * @param {Enum(Number)} formMode : chế độ hiện tại của form (ADD (0) /EDIT (1)
     * Description: Hàm để binding data lên form với từng trường hợp form
     * +/ Với form ADD : Lấy Code mới và truyền lên
     * +/ Với form EDIT: Lấy thông tin của nhân viên và truyền lên
     *
     * -- SAU KHI BINDING XONG THÌ LẤY GÁN GIÁ TRỊ CHO BIẾN COPY ĐỂ THEO DÕI CÓ THAY ĐỔI KHÔNG
     */
    async bindingDataToForm(formMode) {
      try {
        this.resetFieldData();
        this.resetTextFieldError();
        // Focus vào ô đầu tiên
        this.$refs.fullName.focus();

        // Đối với trường hợp edit => binding dữ liệu lên form
        if (formMode === this.$_MisaEnums.FORM_MODE.EDIT) {
          const user = await this.getUserById();
          if (user) {
            this.formData = user;
          }
        }
        // Với trường hợp thêm thì chỉ hiển thị form trống
        // Coppy formData ban đầu ra một object mới để kiểm tra có thay đổi không
        this.copyFormData = { ...this.formData };
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {Object} user : user muốn thêm mới
     * Description: Hàm thêm mới user gọi API
     */
    async addNewUser(user) {
      try {
        this.$store.state.isLoading = true;
        const res = await userService.post(user);
        return res;
      } catch (error) {
        console.log(error);
      } finally {
        this.$store.state.isLoading = false;
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {String(Guid)} id : Id của người dùng muốn update
     * @param {*} user : Thông tin người dùng để update
     * Description: Hàm cập nhật thông tin người dùng gọi API
     */
    async updateUser(id, user) {
      try {
        this.$store.state.isLoading = true;
        const res = await userService.put(id, user);
        return res;
      } catch (error) {
        console.log(error);
      } finally {
        this.$store.state.isLoading = false;
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Submit form với add/update
     */
    async submitForm() {
      try {
        // Với trường hợp form thêm mới / nhân bản
        let res = {};
        if (this.formMode === this.$_MisaEnums.FORM_MODE.ADD) {
          res = await this.addNewUser(this.formData);
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
            this.dialog.text = "Form không có gì thay đổi. Không thể cất";
          } else {
            // Vơi update thì ko cho update password và repeatedPass
            const userToUpdate = {
              role: this.formData.role,
              fullName: this.formData.fullName,
              email: this.formData.email,
              phoneNumber: this.formData.phoneNumber,
            };

            res = await this.updateUser(this.editUserId, userToUpdate);
          }
        }

        // Thông báo lưu thông tin thành công
        // Kiểm tra res.success thành công = true -> add Toast
        if (res?.success) {
          this.$store.commit("addToast", {
            type: "success",
            message: "Lưu thông tin thành công.",
          });
          return true;
        }
        return false;
      } catch (err) {
        console.log(err);
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
          const isSuccess = await this.submitForm();

          if (isSuccess) {
            // Thông báo ra bên ngoài đã submit thành công để load lại bảng
            // Với save => ẩn form
            this.$emit(
              "notifySubmittedForm",
              this.$_MisaEnums.FORM_SUBMIT_MODE.SAVE
            );
          }
        }
      } catch (error) {
        console.log(error);
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
          const isSuccess = await this.submitForm();
          if (isSuccess) {
            // Thông báo ra bên ngoài EmployeePage đã submit thành công để load lại bảng
            // Với save => ẩn form
            // Với save_add => reset thông tin -> tiếp tục thêm được
            this.$emit(
              "notifySubmittedForm",
              this.$_MisaEnums.FORM_SUBMIT_MODE.SAVE_ADD
            );

            // Sau khi click nút lưu và thêm => chuyển mode -> add (nếu mode hiện tại đg là add => ko đổi)
            // nhưng nếu mode đg là update => add
            this.formMode = this.$_MisaEnums.FORM_MODE.ADD;
            this.bindingDataToForm(this.formMode);
            console.log(this.formData);
          }
        }
      } catch (error) {
        console.log(error);
      }
    },
  },
};
</script>
