<template lang="">
  <div class="checkbox-field" :class="fieldClass">
    <input
      type="checkbox"
      :id="checkboxId"
      :class="checkboxClass"
      :value="value"
      v-model="inputValue"
      @change="onCheckboxChange"
    />
    <label :for="checkboxId" :title="title" v-if="label">{{ label }}</label>
  </div>
</template>
<script>
export default {
  name: "MisaCheckboxField",

  props: {
    //    PROPS CHO VIỆC TÁI SỬ DỤNG COMPONENT
    // Label cho checkbox field
    label: { type: String, default: "" },

    // tooltips label
    title: { type: String, default: "" },

    // class cho toàn bộ
    fieldClass: { required: false },

    // Id của checkbox input
    checkboxId: { type: String },

    // Class cho checkbox
    checkboxClass: { type: String, default: "" },

    // Giá trị của checkbox button
    value: { required: false },

    // Giá trị đã chọn, được binding từ parent
    modelValue: { required: false },
  },

  data() {
    return {
      inputValue: this.value, // Dữ liệu đang được edit trong component
    };
  },

  created() {
    try {
      // gán prop modelValue cho inputValue để tương tác dữ liệu
      this.inputValue = this.modelValue;
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
     * Description : Xử lý khi checkbox bị thay đổi check/không check
     * Truyền emit ra ngoài : - Emit để báo hiệu update giá trị modelValue đối với việc binding 2 chiều
     *                        - Emit báo hiệu checkbox đã change và sẽ đẩy theo Id của nhân viên tương
     *                          với dòng đó cùng với tình trạng check của checkbox (true/false)
     */
    onCheckboxChange() {
      try {
        // EMIT CHO BINDING 2 CHIỀU CỦA COMPONENT ĐỂ UPDATE LẠI ModelValue
        this.$emit("update:modelValue", this.inputValue);
      } catch (err) {
        console.error(err);
      }
    },
  },
};
</script>
<style lang="css">
@import "./checkbox-field.css";
</style>
