<template lang="">
  <div :class="fieldClass">
    <label :for="textFieldId" :title="title" v-if="label">{{ label }}</label>
    <input
      ref="textFieldInput"
      type="text"
      :id="textFieldId"
      :title="inputTitle"
      class="text-field"
      :class="textFieldClass"
      :placeholder="placeholder"
      v-model="inputValue"
      @input="onInputText"
      @blur="onBlurInput"
    />
  </div>
</template>
<script>
export default {
  name: "MisaTextField",

  props: {
    // Label cho text field
    label: { type: String, default: "" },

    // tooltips label
    title: { type: String, default: "" },

    // tooltips cho input
    inputTitle: { type: String, default: "" },

    // Id của input
    textFieldId: { type: String },

    // Class cho input
    textFieldClass: { type: String, default: "" },

    // class cho toàn bộ field đó
    fieldClass: { required: false },

    // placeholder cho input
    placeholder: { type: String, default: "" },

    // value cuar model dùng để binding 2 chiều cho component
    modelValue: { type: String, default: "" },
  },

  data() {
    return {
      inputValue: "", // Dữ liệu đang được edit trong component
    };
  },

  created() {
    try {
      this.inputValue = this.modelValue; // gán prop modelValue cho inputValue để tương tác dữ liệu
    } catch (err) {
      console.error(err);
    }
  },

  watch: {
    // Theo dõi sự thay đổi của prop value của component đó nếu có thay đổi thì gán vào biến inputValue
    modelValue: function () {
      try {
        this.inputValue = this.modelValue;
      } catch (err) {
        console.error(err);
      }
    },
  },

  methods: {
    /**
     * Hàm thực hiện focus vào ô input
     */
    focus() {
      try {
        this.$refs.textFieldInput.focus();
      } catch (err) {
        console.error(err);
      }
    },
    /**
     * Author : PNNHai
     * Date :
     * Description : Emit một sự kiện 'input' khi có sự thay đổi trong input
     * khi input có thay đổi thì truyền emit update lại modelValue bằng giá trị inputValue
     * Đồng thời có phát ra một emit để truyền thông tin nhằm bắt sự kiện input cho bên ngoài
     */
    onInputText() {
      try {
        this.$emit("update:modelValue", this.inputValue);
        this.$emit("notifyInputText");
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date :
     * Description : Emit một sự kiện notifyBlurInput khi blur ở input
     */
    onBlurInput() {
      try {
        this.$emit("notifyBlurInput");
      } catch (err) {
        console.error(err);
      }
    },
  },
};
</script>
<style lang="css" scoped>
@import "./text-field.css";
</style>
