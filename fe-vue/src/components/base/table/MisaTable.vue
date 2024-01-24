<template lang="">
  <div class="table-container" id="main-table-container">
    <div class="main-content-table" @scroll.stop="onScrollTable">
      <table>
        <thead>
          <tr>
            <th>
              <misa-checkbox-field
                @change.stop="onChangeTableCheckbox()"
                v-model="isSelectedAll"
              />
            </th>
            <th
              v-for="(header, index) in tableColumns.resources"
              :key="index"
              :title="header?.tooltips"
              :style="{
                width: header.width + 'px',
                'min-width': header.width - 30 + 'px',
                'max-width': header.width + 50 + 'px',
                'text-align': header.align,
              }"
              @click.stop="onClickColumnSort(header.columnKey)"
            >
              {{ header.title }}
              <div class="filter-data">
                <div
                  class="filter-data-icon"
                  :class="{
                    'filter-data--active': isFilterColumnAccessed(
                      header.columnKey
                    ),
                  }"
                  :title="$_MisaResources.filterPopup.tooltipForFilterIcon"
                  @click.stop="handleShowFilterColumnPopup(header.columnKey)"
                ></div>
                <div
                  class="filter-column-area"
                  title=""
                  v-if="header.columnKey === filterPopupColumnName"
                  @click.stop
                >
                  <div class="filter-condition-area">
                    <div class="filter-condition-heading">
                      {{ $_MisaResources.filterPopup.filterPopupHeading }}
                    </div>

                    <!-- Vùng chọn giá trị để lọc ứng với type của các column -->
                    <!-- Ứng với cùng chọn text -->
                    <div
                      class="filter-condition-selection"
                      v-if="
                        header.filterType ===
                        $_MisaEnums.FILTER_COLUMN_TYPE.TEXT_TYPE
                      "
                    >
                      <misa-combobox
                        :fieldClass="['combobox-l', 'filter-selector']"
                        :dataResources="filterResources.textFilter"
                        keyDisplayName="filterTypeName"
                        keyValue="filterTypeValue"
                        v-model="filterColumns[index].filterType"
                      />
                      <misa-text-field
                        fieldClass="filter-value-text"
                        textFieldClass="size-l text-field"
                        :placeholder="$_MisaResources.filterPopup.inputValue"
                        v-model="filterColumns[index].filterString"
                      />
                    </div>

                    <!-- Ứng với vùng chọn thể so sánh -->
                    <div
                      class="filter-condition-selection"
                      v-else-if="
                        header.filterType ===
                        $_MisaEnums.FILTER_COLUMN_TYPE.COMPARABLE_TYPE
                      "
                    >
                      <misa-combobox
                        :fieldClass="['combobox-l', 'filter-selector']"
                        :dataResources="filterResources.comparisionFilter"
                        keyDisplayName="filterTypeName"
                        keyValue="filterTypeValue"
                        v-model="filterColumns[index].filterType"
                      />
                      <!-- Với trường nào có type = date -->
                      <misa-date-field
                        v-if="header.columnType === 'date'"
                        fieldClass="filter-value-text"
                        dateInputClass="size-l"
                        :placeholder="$_MisaResources.filterPopup.inputValue"
                        v-model="filterColumns[index].filterString"
                      />
                    </div>

                    <!-- Ứng với vùng chọn lựa chọn-->
                    <div class="filter-condition-selection" v-else>
                      <misa-combobox
                        :fieldClass="['combobox-l', 'filter-selector']"
                        :dataResources="filterResources.selectionFilter"
                        keyDisplayName="filterTypeName"
                        keyValue="filterTypeValue"
                        v-model="filterColumns[index].filterType"
                      />
                      <misa-many-combobox
                        :fieldClass="['combobox-l', 'filter-selector']"
                        :dataResources="header?.filterOption"
                        keyDisplayName="keyDisplayName"
                        keyValue="keyValue"
                        v-model="filterColumns[index].filterString"
                      />
                    </div>
                  </div>

                  <div class="filter-footer">
                    <misa-button
                      :buttonName="
                        $_MisaResources.filterPopup.filterCancelButton
                      "
                      buttonClass="button normal-button"
                      @click.stop="
                        handleChangedCurrentFilterColumnStatus(
                          $_MisaEnums.FILTER_POPUP_SELECTION.CANCEL,
                          filterColumns[index]
                        )
                      "
                    />
                    <misa-button
                      :buttonName="
                        $_MisaResources.filterPopup.filterApplyButton
                      "
                      buttonClass="button primary-button"
                      @click.stop="
                        handleChangedCurrentFilterColumnStatus(
                          $_MisaEnums.FILTER_POPUP_SELECTION.FILTER_APPLY,
                          filterColumns[index]
                        )
                      "
                    />
                  </div>
                </div>
              </div>
              <div
                class="heading-icon"
                :class="{
                  'arrow-z-a-sort': sortParams.isSortDesc,
                  'arrow-a-z-sort': !sortParams.isSortDesc,
                }"
                v-if="sortParams.sortColumn == header.columnKey"
              ></div>
            </th>
            <th>{{ $_MisaResources.tableFunctions.title }}</th>
          </tr>
        </thead>
        <tbody v-if="rowsData.length > 0">
          <tr
            v-for="tableRow in rowsData"
            :key="tableRow[tableColumns.objectId]"
            @dblclick.stop="
              handleNotifyWithRowFunction(
                $_MisaEnums.ROW_MODE.EDIT,
                tableRow[tableColumns.objectId]
              )
            "
          >
            <td>
              <misa-checkbox-field
                @change.stop="
                  onChangeTableCheckbox(tableRow[tableColumns.objectId])
                "
                v-model="selectedRowIds"
                :value="tableRow[tableColumns.objectId]"
              />
            </td>
            <td
              v-for="(column, columnIndex) in tableColumns.resources"
              :key="columnIndex"
              :style="{
                width: column.width + 'px',
                'min-width': column.width - 30 + 'px',
                'max-width': column.width + 50 + 'px',
                'text-align': column.align,
              }"
              :title="
                column.formatType === undefined
                  ? tableRow[column.columnKey]
                  : formatData(column.formatType, tableRow[column.columnKey])
              "
            >
              {{
                column.formatType === undefined
                  ? tableRow[column.columnKey]
                  : formatData(column.formatType, tableRow[column.columnKey])
              }}
            </td>
            <td>
              <div class="functions-area">
                <span
                  class="function-name"
                  @click.stop="
                    handleNotifyWithRowFunction(
                      $_MisaEnums.ROW_MODE.EDIT,
                      tableRow[tableColumns.objectId]
                    )
                  "
                  >{{ $_MisaResources.tableFunctions.edit }}</span
                >
                <div
                  :class="{
                    'drop-icon': true,
                    'drop-icon-active':
                      functionalRowId === tableRow[tableColumns.objectId],
                  }"
                  @click.stop="
                    onClickDropFunctionIcon(
                      tableRow[tableColumns.objectId],
                      $event
                    )
                  "
                ></div>
              </div>
            </td>
          </tr>
        </tbody>
        <div class="empty-data" v-else>
          <img
            src="../../../assets/img/empty_data_icon.png"
            :alt="$_MisaResources.appText.emptyDataImage"
          />
        </div>
      </table>
      <teleport to="#main-content">
        <div
          class="drop-list-function"
          v-if="functionalRowId"
          :style="styleToolEdit"
        >
          <div
            class="function-item"
            @click.stop="
              handleNotifyWithRowFunction(
                $_MisaEnums.ROW_MODE.DUPLICATE,
                functionalRowId
              )
            "
          >
            {{ $_MisaResources.tableFunctions.duplicate }}
          </div>
          <div
            class="function-item"
            @click.stop="
              handleNotifyWithRowFunction(
                $_MisaEnums.ROW_MODE.DELETE,
                functionalRowId
              )
            "
          >
            {{ $_MisaResources.tableFunctions.delete }}
          </div>
          <div class="function-item">
            {{ $_MisaResources.tableFunctions.disable }}
          </div>
        </div>
      </teleport>
    </div>
  </div>
</template>
<script>
import { convertCamelCaseToPascelCase } from "@/js/common/common";
import { convertDateForFE, convertGender } from "@/js/helpers/convert-data.js";
import { filterColumnResources } from "@/js/helpers/filterColumnResources";

export default {
  name: "MisaTable",

  props: {
    // các heading cho table
    // Component sử dụng : EmployeePage
    tableColumns: { type: Object },

    // data cho table
    // Component sử dụng : EmployeePage
    tableData: { type: Array },
  },

  emits: [
    "notifyTableCheckboxChanged",
    "notifyWorkWithRecord",
    "notifySortColumnClicked",
    "notifyFilterColumnParamsChanged",
  ],

  data() {
    return {
      // Biến để hứng dữ liệu từ prop tableData truyền vào
      rowsData: [],

      // Biến để lưu trữ danh sách id đã chọn
      selectedRowIds: [],

      // Hiển thị pop-up để filter hay không ?
      // Tên cột hiển thị filter popup
      filterPopupColumnName: null,

      // Biến đánh dấu là tất cả các dòng checked hay không
      isSelectedAll: false,

      // Dòng đang được chọn hiển thị danh sách các chức năng
      functionalRowId: null,

      // params để phục vụ cho việc sort data trong bảng
      sortParams: {
        sortColumn: "", // Cột thực hiện sort
        isSortDesc: false, // Loại sort (null: none | tăng dần (false) |giảm dần (true))
      },

      // các column cần lọc dữ liệu
      filterColumns: [],

      // Data đang có hiện tại trong filter column tương ứng khi được chọn
      dataInFilterColumnPopupForChecking: {},

      //-----------------------------
      resizing: false,
      resizingIndex: null,
      initialMouseX: 0,
      initialColumnWidth: 0,

      // filter resources
      filterResources: filterColumnResources,

      styleToolEdit: {
        left: 9,
        top: 0,
      },
    };
  },

  watch: {
    // Update tableData từ props truyền vào và thêm
    tableData: {
      handler: function () {
        try {
          if (this.tableData) {
            this.rowsData = this.tableData.map((rowData) => ({
              ...rowData,
            }));
          }
        } catch (err) {
          console.error(err);
        }
      },
      immediate: true,
    },
  },

  computed: {
    /**
     * Author : PNNHai
     * Date :
     * Description : Hàm để ktra xem tất cả các dòng đã checked hết hay không
     */
    isAllRowsSelected() {
      return this.rowsData.length === this.selectedRowIds.length;
    },

    /**
     * Author : PNNHai
     * Date :
     * Description : Hàm để gán giá trị cho vị trí của functions trong table row
     */
    getPositionFunction() {
      return {
        left: this.styleToolEdit.left + "px",
        top: this.styleToolEdit.top + "px",
      };
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Thực hiện hứng dữ liệu từ prop tableColumns vào
     * và thêm trường filterValue để thực hiện filter với từng trường
     */
    // tableColumns() {
    //   return {
    //     objectId: this.tableColumns.objectId,
    //     resources: this.tableColumns.resources.map((column) => {
    //       return { ...column, isAccessFilter: false, filterString: null };
    //     }),
    //   };
    //   // console.log(this.tableColumns);
    // },
  },

  created() {
    // Thực hiện tạo filterColumnParams cho các cột
    this.createFilterColumnParams();
  },

  // Gán các sự kiện cho việc nhấn ra các vùng ngoài ở document thì ẩn list function ở row và ẩn filter popup
  mounted() {
    this.columnsResizable();
    window.addEventListener("click", this.closeMenuListFunction);
    window.addEventListener("click", this.handleHideFilterColumnPopup);
  },

  unmounted() {
    window.removeEventListener("click", this.closeMenuListFunction);
    window.removeEventListener("click", this.handleHideFilterColumnPopup);
  },

  methods: {
    /**
     * Author: PNNHai
     * Date
     * @param {*} columnName : Tên cột muốn tìm kiếm
     * Description: Hàm thực hiện tìm kiếm phần tử trong mảng thông qua tên cột
     */
    findItemByColumnName(columnName) {
      try {
        // Kiểm tra xem columnName có tồn tại không
        const result = this.filterColumns.find(
          (column) => column.columnName === columnName
        );
        return result;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date :
     * @param {String} columnName : column thực hiện kiểm tra có đang thực hiện filter
     * Description: Hàm thực hiện kiểm tra xem column có được thực hiện filter hay chưa
     */
    isFilterColumnAccessed(columnName) {
      try {
        // Kiểm tra xem columnName có tồn tại không
        const columnFilter = this.findItemByColumnName(columnName);
        // Nếu có tồn tại thực hiện kiểm tra xem đã access chưa nếu access rồi thì trả về true
        if (columnFilter) {
          if (columnFilter.isAccessFilter) {
            return true;
          }
        }
        return false;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date :
     * Description : Hàm để thực hiện tạo filter cho từng cột
     * Lấy ra key của column đó
     * Tạo thêm các key là filterType, filterString và trường kiểm tra xem người dùng có kích hoạt bộ lọc đó không
     * Nếu có thì thực hiện lọc, nếu không thì bỏ filterType và filterString đi
     */
    createFilterColumnParams() {
      try {
        this.filterColumns = this.tableColumns.resources.map((column) => {
          return {
            columnName: column.columnKey,
            filterType: null,
            filterString: null,
            isAccessFilter: false,
          };
        });
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date :
     * Description : Hàm để format date khi hiển thị lên bảng với những trường cần format
     */
    formatData(formatType, value) {
      try {
        if (formatType === this.$_MisaEnums.FORMAT_TYPE.DATE_FOR_FE) {
          return convertDateForFE(value);
        } else if (formatType === this.$_MisaEnums.FORMAT_TYPE.GENDER) {
          return convertGender(value);
        }
        return value;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date :
     * Description : Hàm để bỏ hiển thị danh sách các chức năng của dòng
     */
    closeMenuListFunction() {
      try {
        event.stopPropagation();
        this.functionalRowId = null;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date :
     * Description : Hàm để xử lý ẩn/hiện danh sách các chức năng chọn của các dòng
     */
    onClickDropFunctionIcon(rowId) {
      try {
        // Nếu dòng đang mở drop list function lại được click, ẩn nó.
        if (this.functionalRowId === rowId) {
          this.functionalRowId = null;
        } else {
          // Nếu không, mở nó và xác định vị trí của nó.
          this.functionalRowId = rowId;
          const rectItem = event.target.getBoundingClientRect();
          const left = rectItem.x;
          const top = rectItem.y;

          // gán vị trí cho list function ứng với mỗi dòng
          this.styleToolEdit.left = left - 102 + "px";
          this.styleToolEdit.top = top + 20 + "px";
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm bắt các sự kiện liên quan đến scroll trên table
     */
    onScrollTable() {
      try {
        this.closeMenuListFunction();
        this.handleHideFilterColumnPopup();
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date :
     * Description : Hàm để truyền emit ra ngoài khi tương tác với dòng dữ liệu
     * Truyền id của dòng ra và type (edit, duplicate, delete) ứng với dòng và lựa chọn đó
     */
    handleNotifyWithRowFunction(type, rowId) {
      try {
        this.$emit("notifyWorkWithRecord", type, rowId);

        // bỏ hiển thị list function chọn dòng tương ứng (nếu là xóa hoặc duplicate)
        // -- gán luôn functionalRowId = null đỡ phải kiểm tra xem có phải là delete hoặc duplicate
        this.functionalRowId = null;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {*} rowId : Id của dòng thực hiện change checkbox
     * @param {*} checkedStatus : trạng thái của checkbox sau sự kiện (mới) (true/false)
     * Description: Hàm thực hiện thêm hoặc bỏ id của dòng thực hiện
     * Nếu trong mảng selectedRowIds đã tồn tại id của row hiện tại -> Nếu trạng thái check = false -> bỏ id đó đi khỏi mảng
     * Nếu trong mảng selectedRowIds chưa có id của dòng row hiện tại -> Nếu trạng thái check = true -> thêm id vào mảng
     */
    changeTableBodyRowCheckbox(rowId, checkedStatus) {
      try {
        // Kiểm tra sự tồn tại của id trong mảng selectedRowIds
        const indexOfRowId = this.selectedRowIds.findIndex(
          (id) => id === rowId
        );

        // Nếu chưa tồn tại (index != -1) -> trạng thái checked = true -> cho thêm
        if (indexOfRowId === -1) {
          if (checkedStatus) {
            this.selectedRowIds.push(rowId);
          }
        }
        // Với trường hợp đã tồn tại trong danh sách -> trạng thái checked = false -> bỏ
        else {
          if (!checkedStatus) {
            this.selectedRowIds.splice(indexOfRowId, 1);
          }
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {*} selectedStatus : Trạng thái chọn của checkbox (true/false)
     * Description: Hàm thay đổi trạng thái của tất cả các checkbox đang có trong table
     * Cụ thể là trả về mảng các id (tất cả nếu trạng thái true / rỗng nếu false)
     */
    changeAllCheckboxSelectedStatus(selectedStatus) {
      try {
        if (selectedStatus === true) {
          // Nếu trạng thái chọn là true -> trả về mảng tất cả các id ô bên dưới (checked tất cả các ô bên dưới)
          return this.rowsData.map((row) => row[this.tableColumns.objectId]);
        }
        // Nếu trạng thái chọn là false -> trả về mảng rỗng (unchecked các ô dưới)
        return [];
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date :
     * Description : Hàm bắt sự kiện changed của các checkbox ở table (gồm cả checkbox ở header)
     * Và lấy ra mảng id của các dòng được chọn truyền qua emit ra ngoài
     */
    onChangeTableCheckbox(rowId) {
      try {
        // Lấy trạng thái checked của event.target
        const isChecked = event.target.checked;
        // Trường hợp check vào header sẽ không có rowId
        if (!rowId) {
          // Lấy trạng thái checked/unchecked của header checkbox
          this.isSelectedAll = isChecked;
          // Set mảng id đã chọn = trạng thái trả về (mảng đầy đủ id (true) / mảng rỗng (false))
          this.selectedRowIds = this.changeAllCheckboxSelectedStatus(isChecked);
        }
        // Với trường hợp các checkbox ở dòng dưới của bảng
        else {
          // Xử lý việc thêm/bỏ id của dòng changed khỏi mảng selectedRowIds
          this.changeTableBodyRowCheckbox(rowId, isChecked);
          // Kiểm tra xem tất cả có được checked hết không thì tự động thay đổi trạng thái ở header
          this.isSelectedAll = this.isAllRowsSelected;
        }

        // Thực hiện notify cho component cha thay đổi của selectedRowIds
        this.handleNotifyRowSelected();
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện notify thông báo cho component cha
     * và gửi kèm theo danh sách các ids đã được chọn
     */
    handleNotifyRowSelected() {
      try {
        // LẤY RA MẢNG CÁC RowIds và truyền qua emit sang EmployeePage
        console.log(this.selectedRowIds);
        this.$emit("notifyTableCheckboxChanged", this.selectedRowIds);
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện bỏ chọn tất cả các dòng trong table
     */
    handleUnselecAllRow() {
      try {
        // Xét biến chọn tất cả = false -> bỏ chọn
        this.isSelectedAll = false;

        // Change các checbox bên dưới với trạng thái bỏ (false)
        this.selectedRowIds = this.changeAllCheckboxSelectedStatus(
          this.isSelectedAll
        );

        // notify thông báo cho component cha checkbox changed và gửi kèm selectedRowIds
        this.handleNotifyRowSelected();
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {*} sortColumn Cột click để sort
     * Description: Hàm thông báo cho component cha là vừa click vào tiêu đề (gửi kèm theo tên cột vừa click sort)
     */
    onClickColumnSort(sortColumn) {
      try {
        // Nếu tên trường cần sort có tồn tại
        if (sortColumn) {
          // Với trường hợp click vào ô khác ô sort -> chuyển sang sort ở ô mới
          if (this.sortParams.sortColumn !== sortColumn) {
            // đổi cột thực hiện sort thành tên cột vừa chọn và mặc định chuyển isSortDesc = false với lần đầu
            this.sortParams.sortColumn = sortColumn;
            this.sortParams.isSortDesc = false;
          }
          // Với trường hợp click vào đúng ô đang thực hiện sort
          // -> chuyển thành true (lần click 2) / null - bỏ chọn (lần click 3)
          else {
            // Với lần click thứ 2 vào đúng ô
            if (!this.sortParams.isSortDesc) {
              this.sortParams.isSortDesc = true;
            }
            // Với trường hợp click lần 3 -> bỏ sort
            else {
              this.sortParams.sortColumn = "";
              this.sortParams.isSortDesc = null;
            }
          }
        }

        // Truyền emit ra ngoài component cha theo params là sortParams
        this.$emit("notifySortColumnClicked", this.sortParams);
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Thực hiện điều chỉnh độ rộng của các cột
     * @author: PNNHai
     */
    columnsResizable() {
      try {
        var thElm;
        var startOffset;

        Array.prototype.forEach.call(
          document.querySelectorAll("table thead th"),
          function (th) {
            th.style.position = "relative";

            var grip = document.createElement("div");
            grip.innerHTML = "&nbsp;";
            grip.style.top = 0;
            grip.style.right = 0;
            grip.style.bottom = 0;
            grip.style.width = "4px";
            grip.style.position = "absolute";
            grip.style.cursor = "col-resize";
            grip.addEventListener("mousedown", function (e) {
              thElm = th;
              startOffset = th.offsetWidth - e.pageX;
            });

            th.appendChild(grip);
          }
        );

        document.addEventListener("mousedown", function (e) {
          if (thElm) {
            thElm.style.width = startOffset + e.pageX + "px";
            thElm.style.minWidth = startOffset + e.pageX + "px";
          }
        });

        document.addEventListener("mouseup", function () {
          thElm = undefined;
        });
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Auhor: PNNHai
     * Date:
     * Description: Hàm thực hiện ẩn pop up đi (dùng hàm này cho sự kiện scroll hoặc ấn ra ngoài)
     * Khi ẩn đi thì kiểm tra xem data đang có trong filter popup này có thay đổi không (có thay đổi mà ko access ko ?)
     * Nếu có thay đổi thì gán lại ban đầu chứ ko giữ cái mới thay đổi mà không access
     */
    handleHideFilterColumnPopup() {
      try {
        event.stopPropagation();
        // Không tắt khi có dialog hiển thị
        if (!this.$store.state.dialogNotify.isShow) {
          if (
            this.filterPopupColumnName &&
            this.dataInFilterColumnPopupForChecking
          ) {
            // Tìm phần tử ứng với tên cột đang được hiển thị filter popup
            const filterColumnData = this.filterColumns.find(
              (column) => column.columnName === this.filterPopupColumnName
            );

            // Kiểm tra xem trc khi tắt đi thì data đang có trong filter popup đó có
            // thay đổi không (có khác với dataInFilterColumnPopupForChecking không )
            // Nếu có khác thì chứng tỏ data trong đó đã thay đổi và chưa access lại nên phải gán lại data trc khi thay đổi
            if (
              JSON.stringify(filterColumnData) !==
              JSON.stringify(this.dataInFilterColumnPopupForChecking)
            ) {
              // Gán lại các phần tử vào lại phần tử nếu có thay đổi
              (filterColumnData.filterType =
                this.dataInFilterColumnPopupForChecking.filterType),
                (filterColumnData.filterString =
                  this.dataInFilterColumnPopupForChecking.filterString),
                (filterColumnData.isAccessFilter =
                  this.dataInFilterColumnPopupForChecking.isAccessFilter);
            }
            // ẩn popup đi
            this.filterPopupColumnName = null;
            // bỏ biến lưu trữ tạm đi sau khi ẩn (thực hiện xong việc check thay đổi)
            this.dataInFilterColumnPopupForChecking = {};

            // console.log(filterColumnData);
            // console.log(this.dataInFilterColumnPopupForChecking);
            // console.log(this.filterColumns);
          }
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện copy data của popup đang show
     * để thực hiện sao lưu khi giá trị nhập vào có thay đổi nhưng không access filter
     */
    handleCoppyCurrentDataInFilterColumnPopupForChecking() {
      try {
        if (this.filterPopupColumnName) {
          const filterColumnData = this.findItemByColumnName(
            this.filterPopupColumnName
          );
          if (filterColumnData) {
            // Rải phần tử vào object mới tránh bị tham chiếu
            this.dataInFilterColumnPopupForChecking = { ...filterColumnData };
          }
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {string} columnName : Tên cột thực hiện filter
     * Description: Hàm thực hiện hiển thị/ẩn filter pop-up ứng với column chọn
     */
    handleShowFilterColumnPopup(columnName) {
      try {
        if (columnName) {
          // Với trường hợp pop-up chưa hiển thị -> hiển thị ứng với cột đó
          if (!this.filterPopupColumnName) {
            this.filterPopupColumnName = columnName;
            this.handleCoppyCurrentDataInFilterColumnPopupForChecking();
          }
          // Với trường hợp pop-up đang hiển thị
          else {
            // Với trường hợp chọn sang cột khác
            if (this.filterPopupColumnName !== columnName) {
              // Thực hiện check sự thay đổi của data trc khi đổi sang cột mới
              this.handleHideFilterColumnPopup();
              this.filterPopupColumnName = columnName;
              // Sau khi chuyển sang cột mới thì lưu vào biến tạm để lưu check xem có sự thay đổi không
              this.handleCoppyCurrentDataInFilterColumnPopupForChecking();
            }
            // Với trường hợp chọn đúng vào cột đấy -> ẩn filter popup
            else {
              this.handleHideFilterColumnPopup();
            }
          }
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {enum} status : trạng thái lựa chọn popup (Hủy / Áp dụng) đối với filter popup
     * @param {*} filterColumn: Cột thực hiện lọc
     */
    handleChangedCurrentFilterColumnStatus(status, filterColumn) {
      try {
        if (this.filterPopupColumnName) {
          // Với trường hợp apply filter
          if (status === this.$_MisaEnums.FILTER_POPUP_SELECTION.FILTER_APPLY) {
            // Khi thực hiện filter mà các giá trị chưa có -> hiển thị dialog cảnh báo
            if (
              filterColumn.filterType === null ||
              filterColumn.filterString === null ||
              filterColumn.filterString === "" ||
              filterColumn.filterString?.length === 0
            ) {
              this.$store.state.dialogNotify.isShow = true;
              this.$store.state.dialogNotify.text =
                this.$_MisaResources.filterPopupErrorMessage.accessFilterWithoutValue;
            }
            // Khi thực hiện filter với các trường đã nhập đủ
            else {
              // Gán trường access = true
              filterColumn.isAccessFilter = true;
              // notyfy cho component cha giá trị để lọc
              this.handleNotifyChangedFilterColumnParams();

              // Ẩn popup đi(không dùng hàm hide vì đây chỉ ẩn thủ công vì đã thay đổi data và access lại)
              this.filterPopupColumnName = null;
            }
          }
          // với trường hợp bỏ lọc
          else {
            // Đối với trường chưa access lọc thì khi bấm bỏ sẽ hiển thị cảnh báo
            if (filterColumn.isAccessFilter === false) {
              this.$store.state.dialogNotify.isShow = true;
              this.$store.state.dialogNotify.text =
                this.$_MisaResources.filterPopupErrorMessage.removeAFilterColumnWasNotAccessed;
            }
            // Nếu với trường đã access thì reset lại tất cả các giá trị và notify cho component cha
            else {
              // Bỏ các giá trị đi và đánh dấu lại access = false
              filterColumn.filterType = null;
              filterColumn.filterString = null;
              filterColumn.isAccessFilter = false;
              this.handleNotifyChangedFilterColumnParams();

              // Ẩn popup đi(không dùng hàm hide vì đây chỉ ẩn thủ công vì đã thay đổi data và access lại)
              this.filterPopupColumnName = null;
            }
          }
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện thông báo cho component cha về sự thay đổi của giá trị filter với các cột
     */
    handleNotifyChangedFilterColumnParams() {
      const filterColumnParams = [];

      // Thực hiện lấy ra các column được access: columnName (kiểu PacalCase), filterType (loại lọc), filterString (giá trị lọc)
      this.filterColumns.forEach((column) => {
        if (column.isAccessFilter) {
          const data = {
            // Với trường department thì lọc theo Id chứ không phải lọc theo Name
            columnName:
              column.columnName === "departmentName"
                ? "DepartmentId"
                : convertCamelCaseToPascelCase(column.columnName),
            filterType: column.filterType,
            filterString: column.filterString.toString(),
          };
          filterColumnParams.push(data);
        }
      });

      console.log(filterColumnParams);
      this.$emit("notifyFilterColumnParamsChanged", filterColumnParams);
    },
  },
};
</script>
<style lang="css" scoped>
@import "./table.css";
</style>
