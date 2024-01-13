<template lang="">
  <div :class="fieldClass">
    <input
      type="radio"
      :id="radioId"
      :class="radioClass"
      :name="radioFieldGroup"
      :value="value"
      v-model="inputValue"
      @change="onRadioChange"
    />
    <label :for="radioId" :title="title" v-if="label">{{ label }}</label>
  </div>
</template>
<script>
export default {
  name: "MisaRadioField",

  props: {
    // Label cho radio field
    label: { type: String, default: "" },

    // tooltips label
    title: { type: String, default: "" },

    // Id của radio input
    radioId: { type: String },

    // Class cho input
    radioClass: { type: String, default: "" },

    // class cho toàn bộ field đó
    fieldClass: { required: false },

    // radio field group
    radioFieldGroup: { type: String, default: "" },

    // Giá trị của radio button
    value: { required: false },

    // Giá trị đã chọn, được binding từ parent
    modelValue: { require: false },
  },

  data() {
    return {
      inputValue: this.value, // Dữ liệu đang được edit trong component
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
     * Author : PNNHai
     * Date :
     * Description : Emit một sự kiện 'change' cho radio button
     */
    onRadioChange() {
      try {
        this.$emit("update:modelValue", this.inputValue);
      } catch (err) {
        console.error(err);
      }
    },
  },
};
</script>
<style lang="css">
@import "./radio-field.css";
</style>
