<template lang="">
  <div class="login-area">
    <div class="login-form">
      <div class="login-form-heading"></div>
      <div class="login-form-body">
        <div class="login-form-main">
          <div class="login-field" :class="{ invalid: errorMessages.username }">
            <misa-text-field
              textFieldClass="width-100-percent"
              placeholder="Số điện thoại / email"
              v-model="formData.username"
              @notifyInputText="
                validateField('username', 'Số điện thoại / email')
              "
              @notifyBlurInput="
                validateField('username', 'Số điện thoại / email')
              "
            />
            <span class="field-not-empty login-field-error">{{
              errorMessages.username
            }}</span>
          </div>

          <div class="login-field" :class="{ invalid: errorMessages.password }">
            <misa-password-field
              passwordFieldClass="width-100-percent"
              placeholder="Mật khẩu"
              v-model="formData.password"
              @notifyInputPassword="validateField('password', 'Mật khẩu')"
              @notifyBlurPasswordInput="validateField('password', 'Mật khẩu')"
            />
            <span class="field-not-empty login-field-error">{{
              errorMessages.password
            }}</span>
          </div>
        </div>
        <span class="login-password-forget">Quên mật khẩu ?</span>
        <misa-button
          class="login-submit-button"
          buttonName="Đăng nhập"
          buttonClass="button primary-button width-100-percent"
          @click.stop="onClickLoginButton"
        ></misa-button>
      </div>
      <div class="login-form-footer">
        <div class="login-other-header">
          <span class="login-line-throw"></span>
          <div class="login-other-heading">Hoặc đăng nhập với</div>
          <span class="login-line-throw"></span>
        </div>
        <div class="login-other-main">
          <div class="google-login-icon"></div>
          <div class="apple-login-icon"></div>
          <div class="microsoft-login-icon"></div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import userService from "@/js/services/user-service";
import getFieldInvalidError from "@/js/helpers/validate.js";

export default {
  name: "MisaLogin",

  data() {
    return {
      formData: {
        username: null,
        password: null,
      },

      validateFormRules: {
        username: ["required", "maxLength_100"],
        password: ["required", "maxLength_50"],
      },

      errorMessages: {
        username: "",
        password: "",
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
        // Kiểm tra lỗi trước khi submit
        // Validate với các field có đăng kí validate ở obj validateFormRules
        let isOk = true;
        for (let field in this.errorMessages) {
          // Lấy ra label ứng với từng trường để hiển thị trong error
          const fieldLabel =
            this.$_MisaResources.formText.loginForm[field].title;
          // validate field tương ứng
          this.validateField(field, fieldLabel);

          // Kiểm tra nếu errorMessages ứng với trường đó có nội dung thì isOk = false
          const errorText = this.errorMessages[field];
          if (errorText !== "") {
            isOk = false;
          }
        }
        return isOk;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date :
     * Description : Hàm thực hiện login vào hệ thống
     */
    async handleLogin() {
      try {
        this.$store.state.isLoading = true;
        const res = await userService.login(
          this.formData.username,
          this.formData.password
        );

        if (res?.success) {
          this.$store.commit("addToast", {
            type: "success",
            message: "Đăng nhập thành công !",
          });

          const accountInfor = res.data;
          if (accountInfor) {
            // set accessToken vào localStorage
            localStorage.setItem("accessToken", accountInfor.accessToken);

            // Lấy thông tin chi tiết người dùng
            const respon = await userService.getById(accountInfor.userId);
            if (respon?.success) {
              const user = respon.data;
              this.$store.commit("updateLoginStatus", {
                accessToken: accountInfor.accessToken,
                userId: user.userId,
                fullName: user.fullName,
                userRole: user.role,
                email: user.email,
              });
            }

            if (accountInfor.userRole === this.$_MisaEnums.LOGIN_ROLE.ADMIN) {
              // Chuyển sang trang admin
              this.$router.push("/admin");
            } else {
              // Chuyển sang trang user
              this.$router.push("/user");
            }
          }
        }
      } catch (err) {
        console.error(err);
      } finally {
        this.$store.state.isLoading = false;
      }
    },

    /**
     * Author : PNNHai
     * Date :
     * Description : Hàm thực hiện xử lý khi bấm nút đăng nhập
     */
    onClickLoginButton() {
      try {
        const isLogined = this.validateForm();
        if (isLogined) {
          this.handleLogin();
        }
      } catch (err) {
        console.error(err);
      }
    },
  },
};
</script>
<style lang="css">
@import "./login.css";
</style>
