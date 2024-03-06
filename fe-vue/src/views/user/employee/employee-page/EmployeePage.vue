<template lang="">
  <div class="main-content-header">
    <h3 class="main-content-heading">
      User {{ $_MisaResources.appText.employeePageText.pageTitle }}
    </h3>
    <misa-button
      :buttonName="$_MisaResources.buttons.addEmployee.name"
      :buttonTooltips="$_MisaResources.buttons.addEmployee.tooltip"
      buttonClass="add-button primary-button"
      @click.stop="onClickAddEmployee"
    />
  </div>
  <div class="main-content-center">
    <div class="main-content-functions">
      <div class="content-left-functions" v-if="isAtLeastOneRowSelected">
        <span>
          {{ $_MisaResources.appText.selectedCount }}
          <b class="selected-row-number">
            {{ selectedEmployeeIds.length }}
          </b>
        </span>
        <span class="unselectd-all" @click.stop="handleUnselecAll">{{
          $_MisaResources.buttons.unselectedAll
        }}</span>
        <misa-button
          :buttonName="$_MisaResources.buttons.delete"
          buttonClass="delete-button primary-button"
          @click.stop="onClickDeleteMany"
        />
      </div>

      <div class="content-right-functions">
        <misa-text-field
          textFieldClass="searching-function size-l"
          :placeholder="$_MisaResources.appText.findInforTitle"
          v-model="filterParams.searchString"
        />
        <misa-button
          @click.stop="onClickRefreshData"
          buttonClass="refresh-function button-icon-only"
          :buttonTooltips="$_MisaResources.appText.refreshTooltip"
        />
        <div class="excel-area">
          <misa-button
            buttonClass="excel-function button-icon-only"
            :buttonTooltips="$_MisaResources.appText.excelWorkingTooltip"
            @click.stop="handleToggleExcelWorking"
          />
          <ul class="excel-option" v-if="isShowExcelWorking">
            <li class="excel-item" @click.stop="handleShowExcelImportForm">
              {{ $_MisaResources.appText.excelImportTitle.import }}
            </li>
            <li class="excel-item" @click.stop="handleExportAll">
              {{ $_MisaResources.appText.excelExportTitle.exportAll }}
            </li>
            <li
              class="excel-item"
              @click.stop="handleExportWithFilterCondition"
            >
              {{
                $_MisaResources.appText.excelExportTitle
                  .exportWithFilerCondition
              }}
            </li>
          </ul>
        </div>
      </div>
    </div>

    <misa-table
      ref="tableRef"
      class="table-main"
      :tableData="tableData"
      :tableColumns="tableColumns"
      :rowFunctions="rowFunctions"
      @notifyWorkWithRecord="handleWorkWithRow"
      @notifyTableCheckboxChanged="handleChangedTableCheckbox"
      @notifySortColumnClicked="handleSortColumn"
      @notifyFilterColumnParamsChanged="handleFilterColumn"
    />
  </div>

  <div class="main-content-footer">
    <span class="record-display-number"
      >{{ $_MisaResources.appText.totalCount }}
      <b class="records-number">{{ totalRecords }}</b>
      {{ $_MisaResources.appText.records }}
    </span>

    <div class="paging-navigation-area">
      <select class="record-number-chooser" v-model="filterParams.pageSize">
        <option :value="10">
          10 {{ $_MisaResources.appText.recordsPerPage }}
        </option>
        <option :value="20">
          20 {{ $_MisaResources.appText.recordsPerPage }}
        </option>
        <option :value="60">
          60 {{ $_MisaResources.appText.recordsPerPage }}
        </option>
        <option :value="80">
          80 {{ $_MisaResources.appText.recordsPerPage }}
        </option>
      </select>

      <misa-paging-navigation
        v-model:currentPage="filterParams.currentPage"
        :pageSize="filterParams.pageSize"
        :totalPage="totalPage"
      />
    </div>
  </div>

  <employee-form
    v-if="form.isShow"
    :formDataType="form.formMode"
    :editEmployeeId="form.employeeId"
    @notifySubmittedForm="handleSubmittedForm"
    @notifyHideForm="handleHideForm"
  />

  <misa-import-form
    v-if="isShowExcelImportForm"
    @notifyHideExcelImportForm="handleHideExcelImportForm"
    @notifyImportSuccess="filterData"
  />

  <misa-dialog
    v-if="dialog.isShow"
    :dialogType="dialog.type"
    :numberOfButton="dialog.numberOfButton"
    @notifyCloseDialog="handleCloseDialog"
    @notifyDialogResponded="handleDialogResponded"
  >
    <template v-slot:dialog-content>
      <p class="dialog__message">{{ dialog.text }}</p>
    </template>
  </misa-dialog>
</template>
<script>
import EmployeeForm from "../employee-form/EmployeeForm.vue";
import employeeResources from "@/js/helpers/employee-resources";
import employeeService from "@/js/services/employee-service";
import { convertCamelCaseToPascelCase } from "@/js/common/common.js";

export default {
  name: "EmployeePage",
  components: {
    EmployeeForm,
  },

  data() {
    return {
      // Column các cột của table. Đc gán vào từ employeeTableResources
      tableColumns: {},

      // Dữ liệu của bảng sẽ được đọc từ API về và gán vào
      tableData: [],

      // Danh sách dòng trong bảng được chọn
      selectedEmployeeIds: [],

      form: {
        isShow: false,
        formMode: null,
        employeeId: "",
      },

      dialog: {
        isShow: false,
        type: null,
        text: null,
        numberOfButton: null,
      },

      employeeIdToDelete: null,

      filterParams: {
        currentPage: 1,
        pageSize: 10,
        searchString: "",
        sortColumn: "",
        isSortDesc: null,
        filterColumns: [],
      },

      // Tổng số bản ghi đang hiển thị trên bảng
      totalRecords: 0,

      totalPage: 0,

      confirmAction: null,

      isShowExcelWorking: false,

      excelExportData: {
        // Loại xuất excel (0: all, 1: các dòng đã chọn, 2: các điều kiện lọc)
        exportType: null,
        currentPage: -1,
        columns: [],
        ids: [],
        searchString: null,
        filterColumns: [],
      },

      isShowExcelImportForm: false,

      rowFunctions: [
        {
          rowMode: this.$_MisaEnums.ROW_MODE.EDIT,
          functionName: this.$_MisaResources.tableFunctions.edit,
        },
        {
          rowMode: this.$_MisaEnums.ROW_MODE.DUPLICATE,
          functionName: this.$_MisaResources.tableFunctions.duplicate,
        },
        {
          rowMode: this.$_MisaEnums.ROW_MODE.DELETE,
          functionName: this.$_MisaResources.tableFunctions.delete,
        },
      ],
    };
  },

  created() {
    try {
      // Sử dụng employeeTableResources để gán các heading để hiển thị và key để gọi api
      // Bên trong $_EmployeeTableResources có : resources có title để hiển thị và columnKey là trường để gọi lên api
      // Đồng thời $_EmployeeTableResources có : objectKey là id để khi sử dụng v-for sẽ lấy id này để làm key
      this.tableColumns = employeeResources;

      // Load toàn bộ employee từ API về
      // this.loadAllData();

      // Load employee kết hợp phân trang từ API về
      this.filterData();
    } catch (err) {
      console.error(err);
    }
  },

  mounted() {
    // Gán sự kiện keydown
    window.addEventListener("keydown", this.handleShortHandKeyDown);
    window.addEventListener("click", this.handleHideExcelWorking);
  },

  unmounted() {
    window.removeEventListener("keydown", this.handleShortHandKeyDown);
    window.removeEventListener("click", this.handleHideExcelWorking);
  },

  computed: {
    /**
     * Author : PNNHai
     * Date :
     * Description : Kiểm tra có ít nhất một bản ghi được chọn không
     * Nếu có thì hiển thị thao tác hàng loạt còn không thì bị ẩn
     */
    isAtLeastOneRowSelected() {
      return this.selectedEmployeeIds.length >= 1;
    },
  },

  watch: {
    /**
     * Author: PNNHai
     * Date:
     * Description: Theo dõi filterParams nếu có thay đổi thì gọi lại hàm filterData()
     */
    filterParams: {
      handler: function () {
        try {
          this.filterData();
          // Khi thực hiện chuyển trang
          this.handleUnselecAll();
        } catch (err) {
          console.error(err);
        }
      },
      deep: true,
    },
  },

  methods: {
    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện bắt sự kiện nhanh với key
     * +/ insert -> mở form thêm
     */
    handleShortHandKeyDown() {
      try {
        event.stopPropagation();
        // Chỉ bắt sự kiện keydown nhanh với form khi không có dialog và form hiển thị
        if (
          !this.dialog.isShow &&
          !this.$store.state.dialogNotify.isShow &&
          !this.form.isShow
        ) {
          // Khi nhấn phím insert -> show form thêm mới
          if (event.key === "Insert") {
            event.preventDefault();
            this.onClickAddEmployee();
          }
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date :
     * Description : Hàm để get danh sách nhân viên từ api về
     */
    async loadAllData() {
      try {
        this.$store.state.isLoading = true;

        // Gọi lên api lấy data
        const res = await employeeService.get();
        if (res?.success) {
          this.tableData = res.data;

          // Gán số lượng hiển thị
          this.totalRecords = this.tableData.length;
        }
      } catch (error) {
        console.error(error);
      } finally {
        this.$store.state.isLoading = false;
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm để filter nhân viên. Kết hợp phân trang và tìm kiếm
     */
    async filterData() {
      try {
        this.$store.state.isLoading = true;
        // Gọi lên api lấy data
        const res = await employeeService.filter(this.filterParams);
        // Nếu dữ liệu trả về thành công thì loading thêm 1s
        if (res?.success) {
          setTimeout(() => {
            // Gán số lượng hiển thị
            this.tableData = res.data.data;
            this.totalRecords = res.data.totalRecords;
            this.totalPage = res.data.totalPage;
            this.$store.state.isLoading = false;
          }, 1000); // Thời gian chờ là 5 giây
        }
        // Nếu lỗi thì dừng loading luôn
        else {
          this.$store.state.isLoading = false;
        }
      } catch (error) {
        console.error(error);
        this.$store.state.isLoading = false;
      }
    },

    /**
     * Author: PNHai
     * Date:
     * @param {String(Guid)} deleteId : Id thực hiện xóa
     * Description: Hàm xóa nhân viên theo Id
     */
    async deleteEmployee(deleteId) {
      try {
        this.$store.state.isLoading = true;
        const res = await employeeService.delete(deleteId);
        return res;
      } catch (error) {
        console.error(error);
      } finally {
        this.$store.state.isLoading = false;
      }
    },

    /**
     * Author: PNHai
     * Date:
     * @param {Array[String(Guid)]} ids : Array id thực hiện xóa nhiều
     * Description: Hàm xóa nhiều nhân viên theo list id
     */
    async deleteEmployees(ids) {
      try {
        this.$store.state.isLoading = true;
        const res = await employeeService.deleteMany(ids);
        return res;
      } catch (error) {
        console.error(error);
      } finally {
        this.$store.state.isLoading = false;
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {*} exportData Dữ liệu để export
     * Descruption: Hàm để gọi api export dữ liệu ra file excel
     */
    async exportToExcelFile(exportData) {
      try {
        this.$store.state.isLoading = true;
        const res = await employeeService.exportExcel(exportData);
        return res;
      } catch (err) {
        console.error(err);
      } finally {
        this.$store.state.isLoading = false;
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {*} sortParams : Giá trị sort được nhận về thông qua emit từ table
     * Description: Hàm thực hiện xử lý khi giá trị sort ở các cột trong table thay đổi
     */
    handleSortColumn(sortParams) {
      try {
        this.filterParams.sortColumn = sortParams.sortColumn;
        this.filterParams.isSortDesc = sortParams.isSortDesc;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {*} filterColumnParams : Giá trị filter của các cột trong table được nhận về từ emit
     * Description: Hàm thực hiện xử lý filter dữ liệu trong table khi các filter params thay đổi
     */
    handleFilterColumn(filterColumnParams) {
      try {
        this.filterParams.filterColumns = filterColumnParams;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date :
     * Description : Hàm để refresher lại dữ liệu
     */
    async onClickRefreshData() {
      try {
        // await this.loadAllData();
        await this.filterData();
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date :
     * @param {String(Guid)} editEmployeeId : Id của nhân viên đang muốn tương tác
     * Description : Hàm để hiện hiển thị form, sẽ ứng với các trường hợp edit/ add/ duplicate
     */
    handleShowForm(editEmployeeId) {
      try {
        this.form.employeeId = editEmployeeId;
        this.form.isShow = true;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm xử lý khi click vào nút thêm mới nhân viên.
     * Thực hiện gán formMode = ADD + Show form tham số truyền vào là "" => ko có id
     */
    onClickAddEmployee() {
      try {
        this.form.formMode = this.$_MisaEnums.FORM_MODE.ADD;
        this.handleShowForm(null);
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date:
     * @param {Number} rowFunctionType : loại chức năng muốn thực hiện
     * +/ rowFunctionType = EDIT => gán formMode = EDIT + showForm với id là rowId
     * +/ rowFunctionType = DUPLICATE => gán formMode = DUPLICATE + showForm
     * +/ rowFunctionType = DELETE => thực hiện xóa với id là id của dòng đang tương tác
     *
     * @param {String(Guid)} rowId : Id của dòng đang thực hiện
     *
     * Description: Hàm để xử lý làm việc với bản ghi (edit, duplicate, delete)
     */
    async handleWorkWithRow(rowFunctionType, rowId) {
      try {
        // Trường hợp cập nhật bản ghi
        switch (rowFunctionType) {
          case this.$_MisaEnums.ROW_MODE.EDIT:
            this.form.formMode = this.$_MisaEnums.FORM_MODE.EDIT;
            this.handleShowForm(rowId);
            break;
          case this.$_MisaEnums.ROW_MODE.DUPLICATE: {
            this.form.formMode = this.$_MisaEnums.FORM_MODE.DUPLICATE;
            this.handleShowForm(rowId);
            break;
          }
          case this.$_MisaEnums.ROW_MODE.DELETE:
            this.employeeIdToDelete = rowId;
            this.confirmAction = this.$_MisaEnums.CONFIRM_ACTION.DELETE;

            this.dialog.isShow = true;
            this.dialog.type = "warning";
            this.dialog.numberOfButton =
              this.$_MisaEnums.DIALOG_TYPE_BUTTON.TWO_BUTTONS;
            this.dialog.text =
              this.$_MisaResources.appText.employeePageText.confirmTitle.ConfirmToDelete;

            break;
          default:
            break;
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện hiển thị thông báo xác nhận xóa nhiều
     */
    onClickDeleteMany() {
      try {
        this.confirmAction = this.$_MisaEnums.CONFIRM_ACTION.DELETE_MANY;

        this.dialog.isShow = true;
        this.dialog.type = "warning";
        this.dialog.numberOfButton =
          this.$_MisaEnums.DIALOG_TYPE_BUTTON.TWO_BUTTONS;
        this.dialog.text =
          this.$_MisaResources.appText.employeePageText.confirmTitle.ConfirmToDeleteMany;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date :
     * Description : Hàm để ẩn form khi nhận emit từ component EmployeeForm khi click vào nút tắt
     */
    handleHideForm() {
      try {
        this.form.isShow = false;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện bỏ chọn tất cả các checkbox ở table
     */
    handleUnselecAll() {
      try {
        if (this.isAtLeastOneRowSelected) {
          // Gọi ref của table
          const tableComponent = this.$refs.tableRef;
          // Gọi phương thực bỏ chọn tất cả row của table
          tableComponent.handleUnselecAllRow();
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author : PNNHai
     * Date :
     * @param {Array[String(Guid)]} rowIdSelecteds : arr các id của phần tử đang được checked trong bảng
     * Description :Hàm xử lý khi nhận tín hiệu từ component table khi checkbox trong table thay đổi
     */
    handleChangedTableCheckbox(rowIdSelecteds) {
      try {
        this.selectedEmployeeIds = rowIdSelecteds;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {Number} submitMode : type truyền sang
     * submitMode = SAVE => Load lại bảng
     * submitForm = SAVE_ADD -> Load lại bảng + ẩn form đi
     * Description: Hàm xử lý khi tín hiệu submitted từ form truyền sang
     */
    async handleSubmittedForm(submitMode) {
      try {
        await this.filterData();
        if (submitMode === this.$_MisaEnums.FORM_SUBMIT_MODE.SAVE) {
          this.form.isShow = false;
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm ẩn dialog
     */
    handleCloseDialog() {
      try {
        this.dialog.isShow = false;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {*} responseStatus : Trạng thái phản hồi của dialog ( NO(0) / YES(1) )
     * Description: Hàm xử lý khi nhận tín hiệu dialog đã được phản hồi
     * +/ Nếu dialog phản hồi với trường hợp xóa nhiều -> chuyển sang thực hiện với xóa nhiều
     * +/ Nếu dialog phản hồi với trường hợp xóa 1 -> chuyển sang thực hiện với xóa 1
     */
    async handleDialogResponded(responseStatus) {
      try {
        if (this.confirmAction === this.$_MisaEnums.CONFIRM_ACTION.DELETE) {
          await this.handleConfirmDelete(responseStatus);
        } else if (
          this.confirmAction === this.$_MisaEnums.CONFIRM_ACTION.DELETE_MANY
        ) {
          await this.handleConfirmDeleteMany(responseStatus);
        }

        this.handleCloseDialog();
        // Sau khi thực hiện xong thì reset trạng thái của confirm action
        this.confirmAction = null;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {*} response : Trạng thái phản hồi của dialog ( NO(0) / YES(1) )
     * Description: Hàm xử lý xác nhận xóa 1
     * Nếu response = YES -> Xóa + load lại dl + addToast
     */
    async handleConfirmDelete(response) {
      try {
        // Với trường hợp response = yes
        if (response === this.$_MisaEnums.DIALOG_RESPONSE.YES) {
          const res = await this.deleteEmployee(this.employeeIdToDelete);
          if (res?.success) {
            this.$store.commit("addToast", {
              type: "success",
              message:
                this.$_MisaResources.appText.employeePageText.successAction
                  .deleteSucess,
            });

            // load lại table
            this.filterData();
            // Sau khi xóa xong thì reset lại danh sách id
            this.handleUnselecAll();
          }
          // dù xóa được hay không đều reset id to delete = null
          this.employeeIdToDelete = null;
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {*} response : Trạng thái phản hồi của dialog ( NO(0) / YES(1) )
     * Description: Hàm xử lý xác nhận xóa nhiều
     * Nếu response = YES -> Xóa nhiều + load lại dl + addToast
     */
    async handleConfirmDeleteMany(response) {
      try {
        // Với trường hợp response = yes
        if (response === this.$_MisaEnums.DIALOG_RESPONSE.YES) {
          const res = await this.deleteEmployees(this.selectedEmployeeIds);

          if (res?.success) {
            this.$store.commit("addToast", {
              type: "success",
              message:
                this.$_MisaResources.appText.employeePageText.successAction
                  .deleteManySucess,
            });

            // load lại table
            this.filterData();
            // Sau khi xóa xong thì reset lại danh sách id
            this.handleUnselecAll();
          }
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện toggle các option làm việc với excel khi bấm vào button excel
     */
    async handleToggleExcelWorking() {
      try {
        this.isShowExcelWorking = !this.isShowExcelWorking;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện ẩn các option làm việc với excel
     */
    async handleHideExcelWorking() {
      try {
        this.isShowExcelWorking = false;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {*} byteData : Mảng byte dữ liệu data
     * @param {*} fileName : tên file muốn xuất
     * Description: Hàm thực hiện xuất file excel
     */
    dowloadExcelFile(byteData, fileName) {
      const blob = new Blob([byteData], {
        type: "application/vnd.ms-excel",
      });
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement("a");
      link.href = url;
      link.setAttribute("download", fileName);
      link.style.display = "none";
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      window.URL.revokeObjectURL(url);
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện export tất cả bản ghi ra file excel
     */
    async handleExportAll() {
      try {
        this.excelExportData.exportType =
          this.$_MisaEnums.EXPORT_TYPE.EXPORT_ALL;
        this.excelExportData.columns = employeeResources.resources.map(
          (column) => ({
            title: column.title,
            columnKey: convertCamelCaseToPascelCase(column.columnKey),
            width: column.width,
            formatType: column.formatType,
            align:
              column.align === "left" ? 0 : column.align === "center" ? 1 : 2,
          })
        );

        const res = await this.exportToExcelFile(this.excelExportData);
        if (res?.success) {
          // Xuất file
          this.dowloadExcelFile(res.data, "DanhSachNhanVien.xlsx");

          this.$store.commit("addToast", {
            type: "success",
            message:
              this.$_MisaResources.appText.employeePageText.successAction
                .exportAllSuccess,
          });
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện export các bản ghi thỏa với điều kiện lọc ra file excel
     */
    async handleExportWithFilterCondition() {
      try {
        this.excelExportData.exportType =
          this.$_MisaEnums.EXPORT_TYPE.EXPORT_WITH_FILTER_CONDITION;
        this.excelExportData.columns = employeeResources.resources.map(
          (column) => ({
            title: column.title,
            columnKey: convertCamelCaseToPascelCase(column.columnKey),
            width: column.width,
            formatType: column.formatType,
            align:
              column.align === "left" ? 0 : column.align === "center" ? 1 : 2,
          })
        );

        this.excelExportData.searchString = this.filterParams.searchString;
        this.excelExportData.filterColumns = this.filterParams.filterColumns;

        const res = await this.exportToExcelFile(this.excelExportData);
        if (res?.success) {
          // Xuất file
          this.dowloadExcelFile(res.data, "DanhSachNhanVien.xlsx");

          this.$store.commit("addToast", {
            type: "success",
            message:
              this.$_MisaResources.appText.employeePageText.successAction
                .exportWithFilterConditionSuccess,
          });
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện show form import file excel
     */
    handleShowExcelImportForm() {
      try {
        this.isShowExcelImportForm = true;
        this.isShowExcelWorking = false;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện ẩn form import excel
     *
     */
    handleHideExcelImportForm() {
      try {
        this.isShowExcelImportForm = false;
      } catch (err) {
        console.error(err);
      }
    },
  },
};
</script>
<style lang="css" scoped>
@import "./employee-page.css";
</style>
