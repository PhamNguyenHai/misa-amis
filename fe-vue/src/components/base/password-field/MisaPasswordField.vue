<template lang="">
  <div :class="fieldClass">
    <label :for="passwordFieldId" :title="title" v-if="label">{{
      label
    }}</label>
    <div class="password-field" :class="passwordFieldClass">
      <input
        ref="passwordFieldInput"
        :type="passwordType"
        :id="passwordFieldId"
        :title="inputTitle"
        class="password-input"
        :placeholder="placeholder"
        v-model="passwordValue"
        @input="onInputPassword"
        @blur="onBlurPasswordInput"
      />
      <span
        class="toggle-password-icon"
        :class="{
          'hide-pass': isHiddenPassword,
          'show-pass': !isHiddenPassword,
        }"
        @click.stop="handleTogglePassword"
      ></span>
    </div>
  </div>
</template>
<script>
export default {
  name: "MisaPasswordField",

  props: {
    // Label cho text field
    label: { type: String, default: "" },

    // tooltips label
    title: { type: String, default: "" },

    // tooltips cho input
    inputTitle: { type: String, default: "" },

    // Id của input
    passwordFieldId: { type: String },

    // Class cho input
    passwordFieldClass: { type: String, default: "" },

    // class cho toàn bộ field đó
    fieldClass: { required: false },

    // placeholder cho input
    placeholder: { type: String, default: "" },

    // value cuar model dùng để binding 2 chiều cho component
    modelValue: { type: String, default: "" },
  },

  emits: [
    "update:modelValue",
    "notifyInputPassword",
    "notifyBlurPasswordInput",
  ],

  data() {
    return {
      passwordValue: "", // Dữ liệu đang được edit trong component
      // date ẩn/hiện password
      isHiddenPassword: true,
    };
  },

  created() {
    try {
      this.passwordValue = this.modelValue; // gán prop modelValue cho passwordValue để tương tác dữ liệu
    } catch (err) {
      console.error(err);
    }
  },

  computed: {
    /**
     * Author : PNNHai
     * Date :
     * Description : Thực hiện tính toán hiển thị hoặc không hiển thị mật khẩu
     */
    passwordType() {
      try {
        if (this.isHiddenPassword === true) return "password";
        return "text";
      } catch {
        return "password";
      }
    },
  },

  watch: {
    // Theo dõi sự thay đổi của prop value của component đó nếu có thay đổi thì gán vào biến passwordValue
    modelValue: function () {
      try {
        this.passwordValue = this.modelValue;
      } catch (err) {
        console.error(err);
      }
    },
  },

  methods: {
    /**
     * Hàm thực hiện focus vào ô input password
     */
    focus() {
      try {
        this.$refs.passwordFieldInput.focus();
      } catch (err) {
        console.error(err);
      }
    },
    /**
     * Author : PNNHai
     * Date :
     * Description : Emit một sự kiện 'input' khi có sự thay đổi trong input password
     * khi input có thay đổi thì truyền emit update lại modelValue bằng giá trị passwordValue
     * Đồng thời có phát ra một emit để truyền thông tin nhằm bắt sự kiện input cho bên ngoài
     */
    onInputPassword() {
      try {
        this.$emit("update:modelValue", this.passwordValue);
        this.$emit("notifyInputPassword");
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date :
     * Description : Emit một sự kiện notifyBlurPasswordInput khi blur ở input
     */
    onBlurPasswordInput() {
      try {
        this.$emit("notifyBlurPasswordInput");
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date :
     * Description : Hàm thực hiện xử lý ẩn hiện password
     */
    handleTogglePassword() {
      try {
        this.isHiddenPassword = !this.isHiddenPassword;
      } catch (err) {
        console.error(err);
      }
    },
  },
};
</script>
<style lang="css">
@import "./password-field.css";
</style>
