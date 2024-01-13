<template lang="">
  <div :class="fieldClass" ref="manyComboboxArea">
    <label :for="comboboxId" :title="title" v-if="label">{{ label }}</label>
    <div class="combobox-many" ref="combobox">
      <div
        class="selected-results"
        v-if="selectedItemValues.length > 0 && !isSelectedAll"
      >
        <div
          class="selected-option"
          v-for="(item, index) in selectedNameItems"
          :key="item"
        >
          <div class="selected-title" :title="item">
            {{ item }}
          </div>
          <div
            class="selected-remove"
            @click.stop="removeSelectedItemAndNotify(index)"
          ></div>
        </div>
        <div
          class="selected-more"
          v-if="selectedItemValues.length > numberOfFullDisplayItem"
          :title="anotherSelectedName"
        >
          <div class="selected-more-text">
            +<span class="selected-more-number">{{
              selectedItemValues.length - numberOfFullDisplayItem
            }}</span>
          </div>
        </div>
      </div>
      <div class="selected-results" v-if="isSelectedAll">
        <div class="selected-option">
          <div class="selected-title" title="Tất cả">Tất cả giá trị</div>
          <div
            class="selected-remove"
            @click.stop="removeSelectedItemAndNotify(-1)"
          ></div>
        </div>
      </div>
      <input
        ref="inputSearch"
        :id="comboboxId"
        class="combobox-many-input"
        :title="inputTitle"
        type="text"
        @blur.stop="handleBlurSearchText"
        @input.stop="onInputSearchData"
        @keydown.stop="handleItemSelectByKeyboard"
      />
      <button
        class="combobox-many-button"
        type="button"
        @click.stop="onClickToggleListData"
      ></button>
      <ul
        class="combobox-many-list-items"
        ref="itemsDisplayList"
        v-if="isShowListData"
      >
        <li
          class="combobox-many-item"
          :class="{
            'combobox-many-item--selected': selectedItemValues === -1,
            'combobox-many-item-hover': indexItemHoverByKey === -1,
          }"
        >
          <misa-checkbox-field
            label="Tất cả giá trị"
            checkboxId="combobox-many-checked-all"
            @change.stop="handleSelectItem('all')"
            v-model="isSelectedAll"
          />
        </li>
        <li
          class="combobox-many-item"
          v-for="(item, index) in optionData"
          :key="index"
          :class="{
            'combobox-many-item--selected': selectedItemValues === item,
            'combobox-many-item-hover': index === indexItemHoverByKey,
          }"
        >
          <misa-checkbox-field
            :label="item[keyDisplayName]"
            :checkboxId="'combobox-many-item-' + index"
            :value="item[keyValue]"
            @change.stop="handleSelectItem('item')"
            v-model="selectedItemValues"
          />
        </li>
      </ul>
    </div>
  </div>
</template>
<script>
import { removeVietnameseSigns } from "@/js/common/common";
export default {
  name: "MisaManyCombobox",

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

  emits: ["notifyComboboxSearchTextBlur", "update:modelValue"],

  data() {
    return {
      // biến để đánh dấu show/hide list ở combobox
      isShowListData: false,

      // data để chứa toàn bộ dữ liệu list option của combobox gọi từ api về
      optionData: [],

      // item value đang được chọn hiện tại trong combobox
      selectedItemValues: [],

      // chuỗi để filter data hiển thị
      filterString: "",

      // index đang được hover bằng bàn phím
      indexItemHoverByKey: -2,

      // Biến để thực hiện có selected tất cả hay không
      isSelectedAll: false,

      // Số lượng phần tử có thể hiển thị đầy đủ khi chọn
      numberOfFullDisplayItem: 0,
    };
  },

  computed: {
    /**
     * Author: PNNHai
     * Date:
     * Description: Hiển thị name của các item đang được chọn
     */
    selectedNameItems() {
      try {
        // Cắt ra các phần tử có thể hiển thị được lên option
        const selectedItems = this.selectedItemValues.slice(
          0,
          this.numberOfFullDisplayItem
        );

        // Trả về tên các giá trị đó
        return selectedItems.map((itemValue) => {
          const selectedItem = this.findItemByAttributeKey(
            this.dataResources,
            this.keyValue,
            itemValue
          );
          return selectedItem[this.keyDisplayName];
        });
      } catch (err) {
        console.error(err);
        return [];
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Thực hiện ghép các phần tử  không được hiển thị lại để hiển thị lên tooltips
     */
    anotherSelectedName() {
      try {
        if (this.selectedItemValues.length > 1) {
          let anotherSelectedNames = this.selectedItemValues.map((item) => {
            const selectedItem = this.findItemByAttributeKey(
              this.dataResources,
              this.keyValue,
              item
            );
            return selectedItem[this.keyDisplayName];
          });

          anotherSelectedNames.splice(0, this.numberOfFullDisplayItem);
          return anotherSelectedNames.join(", ");
        }
        return "";
      } catch (err) {
        console.error(err);
        return null;
      }
    },
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
          // Khi modelValue có giá trị thì mới thực hiện
          if (this.modelValue) {
            if (this.dataResources && this.dataResources.length > 0) {
              // Chỉ thực hiện gán optionData khi chưa có giá trị (khi mới thiết lập)
              if (!this.optionData || this.optionData.length <= 0) {
                // Gán biến danh sách các item hiển thị trong combobox bằng giá trị của prop truyền vào
                this.optionData = this.dataResources;
              }

              const isSelectedItemValuesContainAll =
                this.isListItemsContainAllListKey(
                  this.dataResources,
                  this.modelValue,
                  this.keyValue
                );

              // Nếu tất cả item đc chọn -> hiển thị checked nút selected all
              if (isSelectedItemValuesContainAll) {
                this.isSelectedAll = true;
              }
              // Nếu chưa chọn tất cả item -> hiển thị tất cả những cái được chọn
              else {
                this.selectedItemValues = [];
                this.modelValue.forEach((value) => {
                  const itemInModelValue = this.findItemByAttributeKey(
                    this.dataResources,
                    this.keyValue,
                    value
                  );
                  if (itemInModelValue) {
                    this.selectedItemValues.push(
                      itemInModelValue[this.keyValue]
                    );
                  }
                });
              }
            }
          }
          // Khi modelValue = null | rỗng thì thực hiện gán lại optionData = dataResources
          // Các phần tử đã chọn -> []
          // Chọn tất cả -> false
          else {
            this.optionData = this.dataResources;
            this.selectedItemValues = [];
            this.isSelectedAll = false;
          }
        } catch (err) {
          console.error(err);
        }
      },
      immediate: true,
    },
  },

  mounted() {
    this.numberOfFullDisplayItem =
      this.caclNumberOfDisplayFullSelectedItemName();
    window.addEventListener("click", this.hideComboboxListData);
  },

  unmounted() {
    window.removeEventListener("click", this.hideComboboxListData);
  },

  methods: {
    /**
     * Author: PNNHai
     * Date:
     * Description: Thực hiện tính toán số lượng phần tử có thể hiển thị được lên combobox phù hợp
     * với chiều rộng của nó
     */
    caclNumberOfDisplayFullSelectedItemName() {
      try {
        const itemWidth = 110; // Độ rộng của mỗi selected-option
        const searchMinWidth = 160; // Độ rộng tối thiểu của search text
        const comboboxAreaWidth = this.$refs.manyComboboxArea.offsetWidth; // Độ rộng của combobox area
        const maxDisplayItemCount = parseInt(
          (comboboxAreaWidth - searchMinWidth) / itemWidth
        ); // Số lượng selected-option tối đa có thể hiển thị
        return maxDisplayItemCount;
      } catch (err) {
        console.error(err);
        return 0;
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện focus vào input
     */
    focus() {
      this.$refs.inputSearch.focus();
    },

    isListItemsContainAllListKey(listItems, listKey, key) {
      try {
        if (
          listItems &&
          listKey &&
          listItems.length > 0 &&
          listKey.length > 0
        ) {
          for (const item of listItems) {
            const isContainItemKey = listKey.indexOf(item[key]);
            if (isContainItemKey === -1) {
              return false;
            }
          }
          return true;
        }
        return false;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện notify blur search text cho component cha
     */
    handleBlurSearchText() {
      try {
        this.$emit("notifyComboboxSearchTextBlur");
        // Khi blur ra thì bỏ hover bằng key đi
        this.indexItemHoverByKey = -2;
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
     * Description: Hàm thực hiện xóa search string
     */
    handleRemoveSearchString() {
      try {
        // Chuỗi tìm kiếm input
        this.$refs.inputSearch.value = "";
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
          this.indexItemHoverByKey = -2;
          // Tự động xóa search string trc khi tắt
          this.handleRemoveSearchString();
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
    handleUpdateModelValue(updateValue) {
      try {
        this.$emit("update:modelValue", updateValue);
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện xử lý với selected all khi chọn
     */
    handleCheckSelectedAllValueAndNotify() {
      try {
        // Nếu checked selected all
        if (this.isSelectedAll) {
          // Khi đã thay đổi ở 'chọn tất cả' -> bỏ hết selectedItemValues dù isSelectedAll = true | false
          this.selectedItemValues = [];
          const allItems = this.dataResources.map(
            (item) => item[this.keyValue]
          );
          this.handleUpdateModelValue(allItems);
        }
        // Khi bỏ checked all -> update với giá trị bình thường
        else {
          this.handleUpdateModelValue(this.selectedItemValues);
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date:
     * @param {*} selectionType : Kiểu chọn (all | item): ứng với việc chọn tất cả hay item
     * Description: Hàm thực hiện xử lý khi một item thay đổi trạng thái checked (bao gồm cả item 'chọn tất cả')
     * Với 'chọn tất cả': selectionType = all. Khi thay đổi trạng thái update ra ngoài luôn
     * Với các item khác: selectionType = item. Khi thay đổi trạng thái thì update selectedItemValues
     */
    handleSelectItem(selectionType) {
      try {
        // Với trường hợp changed chọn tất cả
        if (selectionType === "all") {
          // Lấy trạng thái selectedAll và kiểm tra xử lý và update giá trị ra ngoài
          this.handleCheckSelectedAllValueAndNotify();
        }
        // Với trường hợp không phải chọn ứng với các item
        // Kiểm tra xem sau khi checked item này xong tất cả item có đc checked hết không
        // Nếu có -> bỏ hết checked item và chỉ checked vào chọn tất cả
        // Nếu ko thì tiếp tục update giá trị trong selectedValues
        // **** LƯU Ý: CHỈ THỰC HIỆN CHỌN ĐC KHI CHƯA SELECTED ALL
        else if (selectionType === "item") {
          // Nếu chưa chọn tất cả item rồi thì cho chọn item đó
          if (!this.isSelectedAll) {
            // Kiểm tra xem selectedItemValues có chứa hết tất cả key không (hay ns cách khác tất cả item có đc chọn ko)
            const isSelectedItemValuesContainAll =
              this.isListItemsContainAllListKey(
                this.dataResources,
                this.selectedItemValues,
                this.keyValue
              );

            // Nếu chọn xong thành chọn tất cả -> bỏ hết checked item dưới + chọn item 'chọn tất cả' + update value
            if (isSelectedItemValuesContainAll) {
              this.isSelectedAll = true;
              // Lấy trạng thái selectedAll và kiểm tra xử lý và update giá trị ra ngoài
              this.handleCheckSelectedAllValueAndNotify();
            }
            // Nếu chưa chọn tất cả item -> vẫn cho chọn item
            else {
              this.handleUpdateModelValue(this.selectedItemValues);
            }
          }
          // Nếu đã chọn tất cả item thì không cho chọn thằng nào nữa (selectedItemValues = rỗng)
          else {
            this.selectedItemValues = [];
          }
        }

        console.log(this.selectedItemValues);
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date:
     * Description: Hàm để xử lý khi chọn một item trong list tại combobox.
     * Cụ thể sẽ thay đổi biến selectedItemValues trên mỗi khi chọn gán lại
     * Khi đã chọn được phần tử thì list sẽ bị ẩn đi
     */

    /**
     * Author : PNNHai
     * Date:
     * @param {*} index: index của phần tử muốn xóa (nếu = -1)
     * Description: Hàm thực hiện xóa phần tử khỏi mảng đã chọn khi bấm nút x và update giá trị
     * LƯU Ý : CÓ BAO GỒM CẢ "XÓA TẤT CẢ" (NẾU index TRUYỀN VÀO = -1)
     */
    removeSelectedItemAndNotify(index) {
      try {
        if (index === -1) {
          this.isSelectedAll = false;
        } else {
          this.selectedItemValues.splice(index, 1);
        }
        this.handleUpdateModelValue(this.selectedItemValues);
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
          this.isShowListData = true;
          this.optionData = this.dataFilter();
        }
        // this.$emit("notifyComboboxSearchText");
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện toggle phần tử với mảng
     * @param {*} array : mảng thực hiện
     * @param {*} itemIndex : chỉ số của phần tử
     */
    toggleArrayElement(array, itemIndex) {
      try {
        const selectedItemInArrayIndex = array.findIndex(
          (item) => item === this.optionData[itemIndex][this.keyValue]
        );

        // Nếu không tìm được (chưa có ptu đó) -> thêm
        if (selectedItemInArrayIndex === -1) {
          array.push(this.optionData[itemIndex][this.keyValue]); // Nếu không tồn tại, thêm phần tử vào mảng
        }
        // Nếu có tìm thấy phần tử đó (đã tồn tại ) -> xóa
        else {
          array.splice(selectedItemInArrayIndex, 1); // Nếu tồn tại, xóa phần tử khỏi mảng
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
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
              // Với trường hợp phần tử đang hover bằng bàn phím vào có chỉ số = -2 | -1
              // -> Chuyển hover xuống cuối
              if (
                this.indexItemHoverByKey === -1 ||
                this.indexItemHoverByKey === -2
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
                this.indexItemHoverByKey = -1;
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
                this.indexItemHoverByKey > -2 &&
                this.indexItemHoverByKey < this.optionData.length
              ) {
                // Với trường hợp chọn tất cả (index = -1)
                if (this.indexItemHoverByKey === -1) {
                  this.isSelectedAll = !this.isSelectedAll;
                  this.handleSelectItem("all");
                }
                // Với các item khác
                else {
                  if (!this.isSelectedAll) {
                    // chọn phần tử ứng với index của phần tử đang hover
                    this.toggleArrayElement(
                      this.selectedItemValues,
                      this.indexItemHoverByKey
                    );

                    this.handleSelectItem("item");
                  }
                }
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
        const itemTop =
          this.indexItemHoverByKey <= 0
            ? 0
            : this.indexItemHoverByKey * listItemHeight;

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
        const listItemHeight = 40; // Chiều cao của mỗi phần tử trong danh sách
        const container = this.$refs.itemsDisplayList; // Lấy ra element list chứa các item
        const containerHeight = container.offsetHeight; // Lấy chiều cao của element list chứa các item
        const currentItemPosition =
          this.indexItemHoverByKey <= 0
            ? 0
            : (this.indexItemHoverByKey + 2) * listItemHeight; // Lấy ra vị trí của item hiện tại đang hover bằng bàn phím
        // + 2 bởi vì có thêm 'chọn tất cả' nên khi indexHover = 1 đã có 2 cái trên là 0 và -1

        // Kiểm tra xem vị trí của item hiện tại có thể chứa thêm 1 item với độ cao của list items không (>= không ?)
        // Nếu không hiển thị được thì scroll xuống độ cao của 1 item và (+10 bởi mặc định list items bị hiển thị dở dang item cuối 10px)
        if (currentItemPosition >= containerHeight - listItemHeight) {
          // Scroll xuống và hiển thị thêm 1 phần tử nếu có thể
          container.scrollTop =
            currentItemPosition - containerHeight + listItemHeight + 10;
        }
      } catch (err) {
        console.error(err);
      }
    },
  },
};
</script>
<style lang="css" scoped>
@import "./many-combobox.css";
</style>
