<template lang="">
  <div :class="fieldClass">
    <label :for="comboboxId" :title="title" v-if="label">{{ label }}</label>
    <div class="combobox">
      <input
        ref="inputSearch"
        type="text"
        class="combobox-input"
        :id="comboboxId"
      />
      <button
        class="combobox-button"
        type="button"
        @click.stop="onClickToggleListData"
      ></button>
      <ul class="combobox-list-items" v-if="isShowListData">
        <li
          class="combobox-item"
          v-for="(item, index) in optionData"
          :key="index"
          @click.stop="onSelectItem(item)"
          :class="{ 'combobox-item--selected': itemSelected === item }"
        >
          {{ item[keyDisplayName] }}
        </li>
      </ul>
    </div>
  </div>
</template>
<script>
export default {
  name: "MisaDropDownList",

  props: {
    // Label cho drop down list field
    label: { type: String, required: false, default: "" },

    // tooltips label
    title: { type: String, required: false, default: "" },

    // Label cho combobox field
    fieldClass: { type: String, required: false, default: "" },

    // Id cho combobox (của input giúp cho label dùng for để bấm vào được)
    dropdo: { type: String, required: false, default: "" },

    // Nạp mảng các item để hiển thị trong combobox
    // Nhận từ EmployeeForm sang
    dataResources: { type: Array, required: true },

    // Nạp vào key để từ key đó lấy ra trường hiển thị tên lên combobox
    // Nhận từ EmployeeForm sang
    keyDisplayName: { type: String, required: true },

    // Nạp vào key để từ key đó lấy ra trường để lấy ra value của combobox
    // Nhận từ EmployeeForm sang
    keyValue: { type: String, required: true },

    // Giá trị được binding từ parent v-model
    modelValue: { required: false },
  },

  data() {
    return {
      // biến để đánh dấu show/hide list ở combobox
      isShowListData: false,

      // data để chứa toàn bộ dữ liệu list option của combobox gọi từ api về
      optionData: [],

      // item đang được chọn hiện tại trong combobox
      itemSelected: {},
    };
  },

  created() {
    try {
      // Gán biến danh sách các item hiển thị trong combobox bằng giá trị của prop truyền vào
      this.optionData = this.dataResources;
    } catch (err) {
      console.error(err);
    }
  },

  watch: {
    // Khi prop chứa danh sách item có thay đổi thì gán lại vào biến nội tại
    dataResources: function () {
      try {
        // Gán biến danh sách các item hiển thị trong combobox bằng giá trị của prop truyền vào
        this.optionData = this.dataResources;

        // Hiển thị phần từ đang được chọn khi binding từ ngoài vào
        this.itemSelected = this.optionData.find(
          (item) => item[this.keyValue] === this.modelValue
        );

        if (this.itemSelected)
          this.$refs.inputSearch.value = this.itemSelected[this.keyDisplayName];
      } catch (err) {
        console.error(err);
      }
    },
  },

  methods: {
    /**
     * Author : PNNHai
     * Date:
     * Description: Hàm để show/hide sự ẩn hiện của list combobox
     */
    onClickToggleListData() {
      try {
        this.isShowListData = !this.isShowListData;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date:
     * Description: Hàm để xử lý khi chọn một item trong list tại combobox.
     * Cụ thể sẽ thay đổi biến itemSelected trên mỗi khi chọn gán lại
     */
    onSelectItem(item) {
      try {
        this.itemSelected = item;
        this.isShowListData = false;
        if (this.itemSelected)
          this.$refs.inputSearch.value =
            this.itemSelected?.[this.keyDisplayName];
        this.$emit("update:modelValue", this.itemSelected[this.keyValue]);
      } catch (err) {
        console.error(err);
      }
    },
  },
};
</script>
<style lang="css" scoped>
@import "./dropdown-list.css";
</style>
