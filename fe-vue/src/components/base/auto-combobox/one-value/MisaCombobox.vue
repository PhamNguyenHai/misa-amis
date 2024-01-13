<template lang="">
  <div :class="fieldClass">
    <label :for="comboboxId" :title="title" v-if="label">{{ label }}</label>
    <div class="combobox" ref="combobox">
      <input
        autocomplete="off"
        ref="inputSearch"
        :id="comboboxId"
        class="combobox-input"
        :title="inputTitle"
        type="text"
        @blur.stop="handleBlurSearchText"
        @input.stop="onInputSearchData"
        @keydown.stop="handleItemSelectByKeyboard"
      />
      <button
        class="combobox-button"
        type="button"
        @click.stop="onClickToggleListData"
      ></button>
      <ul
        class="combobox-list-items"
        ref="itemsDisplayList"
        v-if="isShowListData"
      >
        <li
          class="combobox-item"
          v-for="(item, index) in optionData"
          :key="index"
          @click.stop="handleSelectItem(item)"
          :class="{
            'combobox-item--selected': itemSelected === item,
            'combobox-item-hover': index === indexItemHoverByKey,
          }"
        >
          {{ item[keyDisplayName] }}
        </li>
      </ul>
    </div>
  </div>
</template>
<script>
import { removeVietnameseSigns } from "@/js/common/common";
export default {
  name: "MisaCombobox",

  props: {
    // Label cho combobox field
    label: { type: String, required: false, default: "" },

    // tooltips label
    title: { type: String, required: false, default: "" },

    // tooltips cho input
    inputTitle: { type: String, default: "" },

    // Label cho combobox field
    fieldClass: { required: false },

    // Id cho combobox (của input giúp cho label dùng for để bấm vào được)
    comboboxId: { type: String, required: false, default: "" },

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
      itemSelected: null,

      // chuỗi để filter data hiển thị
      filterString: "",

      indexItemHoverByKey: -1,
    };
  },

  // created() {
  //   // Gán biến danh sách các item hiển thị trong combobox bằng giá trị của prop truyền vào
  //   this.optionData = this.dataResources;
  // },

  watch: {
    // Theo dõi nếu v-model của biến đầu vào thay đổi -> binding giá trị chọn = giá trị truyền vào
    modelValue: {
      handler: function () {
        try {
          if (this.dataResources && this.dataResources.length > 0) {
            // Gán biến danh sách các item hiển thị trong combobox bằng giá trị của prop truyền vào
            this.optionData = this.dataResources;

            // Hiển thị phần từ đang được chọn khi binding từ ngoài vào
            // Thực hiện gán itemSelected = phần tử được tìm kiếm theo modelValue
            this.itemSelected = this.findItemByAttributeKey(
              this.optionData,
              this.keyValue,
              this.modelValue
            );

            // Nếu tồn tại itemSelected -> gán inputSearch = displayName của itemSelected
            if (this.itemSelected !== null && this.itemSelected !== undefined) {
              this.$refs.inputSearch.value =
                this.itemSelected[this.keyDisplayName];
            } else {
              this.$refs.inputSearch.value = "";
            }
          }
        } catch (err) {
          console.error(err);
        }
      },
    },
  },

  mounted() {
    window.addEventListener("click", this.hideComboboxListData);

    if (this.dataResources && this.dataResources.length > 0) {
      // Gán biến danh sách các item hiển thị trong combobox bằng giá trị của prop truyền vào
      this.optionData = this.dataResources;

      // Hiển thị phần từ đang được chọn khi binding từ ngoài vào
      // Thực hiện gán itemSelected = phần tử được tìm kiếm theo modelValue
      this.itemSelected = this.findItemByAttributeKey(
        this.optionData,
        this.keyValue,
        this.modelValue
      );

      // Nếu tồn tại itemSelected -> gán inputSearch = displayName của itemSelected
      if (this.itemSelected !== null && this.itemSelected !== undefined) {
        this.$refs.inputSearch.value = this.itemSelected[this.keyDisplayName];
      } else {
        this.$refs.inputSearch.value = "";
      }
    }
  },

  unmounted() {
    window.removeEventListener("click", this.hideComboboxListData);
  },

  methods: {
    focus() {
      this.$refs.inputSearch.focus();
    },
    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện notify blur search text cho component cha
     */
    handleBlurSearchText() {
      try {
        this.$emit("notifyComboboxSearchTextBlur");
      } catch (err) {
        console.error(err);
      }
    },
    /**
     * Author: PNNHai
     * Date:
     * @param {*} items : items muốn thực hiện lọc
     * @param {*} searchString : chuỗi tìm kiếm
     * Description: Hàm thực hiện filter dữ liệu theo searchString
     * -> trả về kq phù hợp (không quan tâm vietnamese và case)
     */
    filterSuitableItem(items, searchString) {
      try {
        // Chuỗi truyền vào được bỏ âm tiếng việt và chuyển sang lowercase
        const nonCaseAndVietnameseSignsSearchString =
          removeVietnameseSigns(searchString).toLocaleLowerCase();

        // Thực hiện filter các item includes với chuỗi
        const suitableItems = items.filter((item) => {
          // displayName của item được bỏ vietnamese và chuyển thành lowercase
          const nonVietnameseCaseItem = removeVietnameseSigns(
            item[this.keyDisplayName]
          ).toLowerCase();
          return nonVietnameseCaseItem.includes(
            nonCaseAndVietnameseSignsSearchString
          );
        });

        // Trả về kết quả phù hợp
        return suitableItems;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {Array} items : Array items muốn tìm kiếm
     * @param {*} attributeKey : key của object cần tìm kiếm
     * @param {*} searchValue : giá trị muốn tìm kiếm
     * Description: Hàm thực hiện tìm kiếm phần tử theo key và giá trị
     */
    findItemByAttributeKey(items, attributeKey, searchValue) {
      try {
        const selectedItem = items.find(
          (item) => item[attributeKey] === searchValue
        );
        return selectedItem;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date:
     * Description: Hàm để lọc dữ liệu theo chuỗi truyền vào
     */
    dataFilter() {
      try {
        if (this.dataResources && this.dataResources.length > 0) {
          // Chuỗi tìm kiếm
          const searchString = this.$refs.inputSearch.value;

          // filter các kết quả phù hợp với chuỗi truyền vào (bỏ kí tự vietnam và không quan tâm đến hoa thường)
          const resultData = this.filterSuitableItem(
            this.dataResources,
            searchString
          );

          return resultData;
        }

        // Nếu không có dataResources ban đầu -> trả về []
        return [];
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện xóa search string nếu như kết quả nhập vào không thỏa mãn
     */
    handleRemoveInvalidSearchString() {
      try {
        // Chuỗi tìm kiếm input
        this.$refs.inputSearch.value = "";
        // Gán itemSelected = rỗng và truyền thông báo update value ra ngoài
        this.itemSelected = null;
        this.handleUpdateModelValue();
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện ẩn list data của combobox
     */
    hideComboboxListData() {
      try {
        this.isShowListData = false;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date:
     * Description: Hàm để show/hide sự ẩn hiện của list combobox
     */
    onClickToggleListData() {
      try {
        // Với trường hợp list items đang show -> khi ấn vào ẩn đi
        if (this.isShowListData) {
          this.isShowListData = false;
          // Gán phần tử đang hover bằng bàn phím = -1 để lần sau khi bật lên sẽ về mặc định
          this.indexItemHoverByKey = -1;
          if (!this.itemSelected) this.handleRemoveInvalidSearchString();
        }
        // Với trường hợp list items đang ẩn -> khi ấn vào show lên
        else {
          this.isShowListData = true;
          // Khi list được hiển thị thì inputSearch phải được focus để bắt các sự kiện keyboard
          this.$refs.inputSearch.focus();
          // Gán optionData = dataResources để dù khi có phần tử được chọn rồi thì vẫn load cả danh sách
          this.optionData = this.dataResources;
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện update modelValue
     */
    handleUpdateModelValue() {
      try {
        // Với trường hợp itemSelected tồn tại -> lấy giá trị chuyển sang
        if (this.itemSelected) {
          this.$emit("update:modelValue", this.itemSelected[this.keyValue]);
        }
        // Với trường hợp itemSelected không có giá trị thì update null
        else {
          this.$emit("update:modelValue", null);
        }
        this.$emit("notifyItemSelected");
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date:
     * Description: Hàm để xử lý khi chọn một item trong list tại combobox.
     * Cụ thể sẽ thay đổi biến itemSelected trên mỗi khi chọn gán lại
     * Khi đã chọn được phần tử thì list sẽ bị ẩn đi
     */
    handleSelectItem(item) {
      try {
        if (item) {
          // Ẩn list đi
          this.isShowListData = false;

          // Gán itemSelected = item muốn gán
          this.itemSelected = item;

          // -> gán giá trị inputSearch = displayName của itemSelected
          this.$refs.inputSearch.value = this.itemSelected[this.keyDisplayName];

          this.handleUpdateModelValue();

          // Khi đã gán xong giá trị thì gán phần tử bàn phím đang hover indexItemHoverByKey = -1
          // để khi đã chọn xong lần sau khi bấm các nút sẽ trở về ban đầu
          this.indexItemHoverByKey = -1;
          // Khi đã chọn được phần tử thì blur ra khỏi input search
          this.$refs.inputSearch.blur();
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date:
     * Description: Hàm để xử lý khi nhập vào input để lọc ra giá trị hiển thị ở list data trong combobox
     */
    onInputSearchData() {
      try {
        if (this.dataResources.length > 0) {
          // Khi nhập vao vàoinput -> bỏ phần tử đã selected đi
          // Show list lên và hiển thị kết quả phù hợp với input
          this.itemSelected = null;
          this.handleUpdateModelValue();
          this.isShowListData = true;
          this.optionData = this.dataFilter();
        }
        // this.$emit("notifyComboboxSearchText");
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date:
     * Hàm thực hiện chọn phần tử bằng bàn phím
     * Nút arrowDown, arrowUp, và enter để chọn phần tử
     *
     */
    handleItemSelectByKeyboard() {
      try {
        if (this.isShowListData) {
          const keyboardCode = event.keyCode;
          switch (keyboardCode) {
            case this.$_MisaEnums.SHORT_KEY.ARROW_UP: {
              // Với trường hợp phần tử đang hover bằng bàn phím vào có chỉ số = 0 | -1
              // -> Chuyển hover xuống cuối
              if (
                this.indexItemHoverByKey === 0 ||
                this.indexItemHoverByKey === -1
              ) {
                this.indexItemHoverByKey = this.optionData.length - 1;
                this.scrollListToVisibleBottom(); // Cuộn để hiển thị phần tử cuối cùng
              }
              // Với các trường hợp khác -- index của phần tử đã chọn đi
              else {
                this.indexItemHoverByKey--;
                this.scrollListToVisibleTop(); // Cuộn để hiển thị phần tử ở trên
              }

              // break mặc định của switch case
              break;
            }
            case this.$_MisaEnums.SHORT_KEY.ARROW_DOWN: {
              // Khi đang hover bàn phím vào phần tử cuối -> chuyển lên đầu
              if (this.indexItemHoverByKey === this.optionData.length - 1) {
                this.indexItemHoverByKey = 0;
                this.scrollListToVisibleTop(); // Cuộn để hiển thị phần tử đầu tiên
              }
              // Với các trường hợp khác -> ++ index của phần tử lên
              else {
                this.indexItemHoverByKey++;
                this.scrollListToVisibleBottom(); // Cuộn để hiển thị phần tử ở dưới
              }
              break;
            }
            // Khi enter
            case this.$_MisaEnums.SHORT_KEY.ENTER: {
              // Với các phần tử thỏa mã giá trị mới cho enter
              if (
                this.indexItemHoverByKey > -1 &&
                this.indexItemHoverByKey < this.optionData.length
              ) {
                // chọn phần tử ứng với index của phần tử đang hover
                this.handleSelectItem(
                  this.optionData[this.indexItemHoverByKey]
                );
              }
              break;
            }
          }
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date:
     * Description: Hàm thực hiện hiển thị danh sách trong combobox khi bấm phím arrowUp để hiển thị đầy đủ
     */
    scrollListToVisibleTop() {
      try {
        const listItemHeight = 40; // Chiều cao của mỗi phần tử trong danh sách
        const container = this.$refs.itemsDisplayList; // Lấy ra element list chứa các item
        const itemTop = (this.indexItemHoverByKey - 1) * listItemHeight;

        // Kiểm tra xem phần tử hiện tại có <= vị trí của scroll không
        // Nếu có thì scroll lên
        // Nếu không thì để nguyên
        if (itemTop <= container.scrollTop) {
          container.scrollTop = itemTop;
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date:
     * Description: Hàm thực hiện hiển thị danh sách trong combobox khi bấm phím arrowDown để hiển thị đầy đủ
     */
    scrollListToVisibleBottom() {
      try {
        const listItemHeight = 48; // Chiều cao của mỗi phần tử trong danh sách
        const container = this.$refs.itemsDisplayList; // Lấy ra element list chứa các item
        const containerHeight = container.offsetHeight; // Lấy chiều cao của element chứa các item
        const currentItemPosition = this.indexItemHoverByKey * listItemHeight; // Lấy ra vị trí của item hiện tại đang hover bằng bàn phím

        // Kiểm tra nếu vị trí của phần tử hiện tại đang đứng có thể hiển thị được thêm 1 item nữa ko
        // Nếu có thì hiển thị
        // Nếu không thì scroll xuống
        if (currentItemPosition >= containerHeight - listItemHeight) {
          container.scrollTop += listItemHeight;
        }
      } catch (err) {
        console.error(err);
      }
    },
  },
};
</script>
<style lang="css" scoped>
@import "./auto-combobox.css";
</style>
