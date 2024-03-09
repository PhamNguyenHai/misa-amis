<template lang="">
  <div class="change-password-area">
    <h2 class="change-password-heading">Thay đổi mật khẩu</h2>
    <span class="change-password-notify"
      >Nhập mật khẩu có tối thiểu 6 ký tự bao gồm số, chữ hoa và chữ
      thường</span
    >
    <div class="change-password-form">
      <misa-password-field
        ref="oldPassword"
        :label="$_MisaResources.formText.changePasswordForm.oldPassword.title"
        :title="$_MisaResources.formText.changePasswordForm.oldPassword.tooltip"
        :fieldClass="[
          { invalid: errorMessages.oldPassword },
          'change-password-field',
          'required',
        ]"
        :inputTitle="errorMessages.oldPassword"
        textFieldId="old-password"
        passwordFieldClass="size-xl"
        v-model="formData.oldPassword"
        @notifyInputPassword="
          validateField(
            'oldPassword',
            $_MisaResources.formText.changePasswordForm.oldPassword.title
          )
        "
        @notifyBlurPasswordInput="
          validateField(
            'oldPassword',
            $_MisaResources.formText.changePasswordForm.oldPassword.title
          )
        "
      />

      <misa-password-field
        ref="newPassword"
        :label="$_MisaResources.formText.changePasswordForm.newPassword.title"
        :title="$_MisaResources.formText.changePasswordForm.newPassword.tooltip"
        :fieldClass="[
          { invalid: errorMessages.newPassword },
          'change-password-field',
          'required',
        ]"
        :inputTitle="errorMessages.newPassword"
        textFieldId="new-password"
        passwordFieldClass="size-xl"
        v-model="formData.newPassword"
        @notifyInputPassword="
          validateField(
            'newPassword',
            $_MisaResources.formText.changePasswordForm.newPassword.title
          )
        "
        @notifyBlurPasswordInput="
          validateField(
            'newPassword',
            $_MisaResources.formText.changePasswordForm.newPassword.title
          )
        "
      />

      <misa-password-field
        ref="repeatedNewPassword"
        :label="
          $_MisaResources.formText.changePasswordForm.repeatedNewPassword.title
        "
        :title="
          $_MisaResources.formText.changePasswordForm.repeatedNewPassword
            .tooltip
        "
        :fieldClass="[
          { invalid: errorMessages.repeatedNewPassword },
          'change-password-field',
          'required',
        ]"
        :inputTitle="errorMessages.repeatedNewPassword"
        textFieldId="repeated-new-password"
        passwordFieldClass="size-xl"
        v-model="formData.repeatedNewPassword"
        @notifyInputPassword="
          validateField(
            'repeatedNewPassword',
            $_MisaResources.formText.changePasswordForm.repeatedNewPassword
              .title
          )
        "
        @notifyBlurPasswordInput="
          validateField(
            'repeatedNewPassword',
            $_MisaResources.formText.changePasswordForm.repeatedNewPassword
              .title
          )
        "
      />

      <div class="change-password-footer">
        <misa-button
          :buttonName="$_MisaResources.buttons.cancel"
          buttonClass="button normal-button"
        />
        <misa-button
          :buttonName="$_MisaResources.buttons.save.name"
          :buttonTooltips="$_MisaResources.buttons.save.tooltip"
          buttonClass="button primary-button"
          @click.stop="onClickCangePasswordButton"
        />
      </div>
    </div>
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
<script>
import getFieldInvalidError from "@/js/helpers/validate";
import userService from "@/js/services/user-service";
export default {
  name: "MisaChangePasswordPage",

  data() {
    return {
      itemListDialogContent: [],

      confirmAction: null,

      formData: {
        oldPassword: null,
        newPassword: null,
        repeatedNewPassword: null,
      },

      validateFormRules: {
        oldPassword: ["required", "maxLength_50"],
        newPassword: [
          "required",
          "minLength_6",
          "strongPassword",
          "maxLength_50",
        ],
        repeatedNewPassword: ["required", "maxLength_50"],
      },

      errorMessages: {
        oldPassword: "",
        newPassword: "",
        repeatedNewPassword: "",
      },

      dialog: {
        isShow: false,
        type: null,
        text: null,
        numberOfButton: null,
      },
    };
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
            this.$_MisaResources.formText.changePasswordForm[field].title;
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

        // Nếu mật khẩu và mật khẩu nhập lại không khớp
        if (
          this.formData.newPassword &&
          this.formData.repeatedNewPassword &&
          this.formData.newPassword !== this.formData.repeatedNewPassword
        ) {
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

    async changePassword(userId, changePasswordInfor) {
      try {
        this.$store.state.isLoading = true;
        const res = await userService.changePassword(
          userId,
          changePasswordInfor
        );
        if (res?.success) {
          return res.data;
        }
        return null;
      } catch (error) {
        console.error(error);
      } finally {
        this.$store.state.isLoading = false;
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
        console.error(error);
      }
    },

    onClickCangePasswordButton() {
      try {
        // Validate và submit form
        if (this.validateForm() === true) {
          // Action confirm thực hiện đc gán là xác nhận lưu
          this.confirmAction = this.$_MisaEnums.CONFIRM_ACTION.SAVE;

          this.dialog.isShow = true;
          this.dialog.type = "warning";
          this.dialog.numberOfButton =
            this.$_MisaEnums.DIALOG_TYPE_BUTTON.TWO_BUTTONS;
          this.dialog.text =
            "Bạn có chắc chắn muốn đổi mật khẩu mới này không ?";
        }
      } catch (error) {
        console.error(error);
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
     * @param {*} responseStatus : Trạng thái phản hồi của dialog ( NO(0) / YES(1) )
     * Description: Hàm xử lý khi nhận tín hiệu dialog đã được phản hồi
     * +/ Nếu dialog phản hồi với trường hợp xác nhận lưu -> chuyển sang thực hiện lưu
     */
    async handleDialogResponded(responseStatus) {
      try {
        if (this.confirmAction === this.$_MisaEnums.CONFIRM_ACTION.SAVE) {
          await this.handleConfirmSaveNewPassword(responseStatus);
        }

        // Tắt dialog đi
        this.handleCloseDialog();
        // Sau khi thực hiện xong thì reset trạng thái của confirm action
        this.confirmAction = null;
      } catch (err) {
        console.error(err);
      }
    },

    async handleConfirmSaveNewPassword(response) {
      try {
        // Với trường hợp response = yes
        if (response === this.$_MisaEnums.DIALOG_RESPONSE.YES) {
          const changePasswordInfor = {
            currentPassword: this.formData.oldPassword,
            changePassword: this.formData.newPassword,
          };

          const changePasswordStatus = await this.changePassword(
            this.$store.state.loginStatus.userId,
            changePasswordInfor
          );

          if (changePasswordStatus) {
            this.$store.commit("addToast", {
              type: "success",
              message: "Đổi mật khẩu thành công",
            });
            // Reset form
            this.resetFieldData();
            this.resetTextFieldError();
          }
        }
      } catch (err) {
        console.error(err);
      }
    },
  },
};
</script>
<style lang="css">
@import "./change-password.css";
</style>
