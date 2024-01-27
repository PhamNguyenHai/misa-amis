<template lang="">
  <misa-import-form
    :importObjectResources="tableImportObjColumns"
    :importRecordsData="importTableData"
    :importSteps="importSteps"
    @notifyHideExcelImportForm="handleHideExcelImportForm"
    @notifyClickedDowloadTemplateFile="handleDowloadEmployeeTemplateFile"
    @notifyPostExcelImportFile="handleChangedExcelImportFile"
  />
</template>
<script>
import MisaImportForm from "@/components/base/import-form/MisaImportForm.vue";
import employeeService from "@/js/services/EmployeeService";
import employeeImportResources from "@/js/helpers/employeeImportResources";

export default {
  name: "EmployeeImportForm",
  components: {
    MisaImportForm,
  },

  data() {
    return {
      // Object cho các cột để hiển thị kết quả sau khi đọc file
      tableImportObjColumns: {},

      importTableData: [],

      importSteps: [
        {
          stepIndex: 1,
          stepName: "Chọn tệp nguồn",
        },
        {
          stepIndex: 2,
          stepName: "Kiểm tra dữ liệu",
        },
        {
          stepIndex: 3,
          stepName: "Kết quả nhập khẩu",
        },
      ],
    };
  },

  created() {
    try {
      this.tableImportObjColumns = employeeImportResources;
    } catch (err) {
      console.error(err);
    }
  },

  methods: {
    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện dowload file template
     */
    async handleDowloadEmployeeTemplateFile() {
      try {
        const res = await this.dowloadTemplateFile();
        console.log(res);
        if (res.success) {
          // Xuất file mẫu
          this.dowloadExcelFile(res.data, "Danh sach nhan vien.xlsx");
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện xử lý khi chang file gửi lên để import
     */
    async handleChangedExcelImportFile(file) {
      try {
        console.log(file);
        const formData = new FormData();
        formData.append("importFile", file);
        console.log(formData);
        const res = await employeeService.importExcelFile(formData);
        if (res.success) {
          this.importTableData = res.data;
        }
      } catch (err) {
        console.error(err);
      }
    },
  },
};
</script>
