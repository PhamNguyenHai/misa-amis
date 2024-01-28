<template lang="">
  <h2 class="employee-statistical">Thống kê nhân viên</h2>
  <div class="charts-wrapper">
    <misa-chart
      :chartTitle="genderStatistical.chartTitle"
      :chartSize="genderStatistical.chartSize"
      :chartType="genderStatistical.chartType"
      :dataForChart="genderStatistical.dataForChart"
    />

    <misa-chart
      :chartTitle="yearOldsStatistical.chartTitle"
      :chartSize="yearOldsStatistical.chartSize"
      :chartType="yearOldsStatistical.chartType"
      :dataForChart="yearOldsStatistical.dataForChart"
    />

    <misa-chart
      :chartTitle="departmentStatistical.chartTitle"
      :chartSize="departmentStatistical.chartSize"
      :chartType="departmentStatistical.chartType"
      :isrotate="departmentStatistical.isrotate"
      :dataForChart="departmentStatistical.dataForChart"
    />
  </div>
</template>
<script>
import employeeService from "@/js/services/employee-service";
import { convertGender } from "@/js/helpers/convert-data";
export default {
  name: "EmployeeStatistical",

  data() {
    return {
      // Dữ liệu thống kê theo giới tính
      genderStatistical: {
        chartTitle:
          this.$_MisaResources.appText.employeePageText.statisticalTitle
            .statisticalByGender,
        dataForChart: {
          labels: [],
          data: [],
        },
        chartType: "doughnut",
        chartSize: { width: 300, height: 300 },
      },

      // Dữ liệu thống kê theo độ tuổi
      yearOldsStatistical: {
        chartTitle:
          this.$_MisaResources.appText.employeePageText.statisticalTitle
            .statisticalByYearOlds,
        dataForChart: {
          labels: [],
          data: [],
        },
        chartType: "bar",
        chartSize: { width: 300, height: 300 },
      },

      // Dữ liệu thống kê theo phòng ban
      departmentStatistical: {
        chartTitle:
          this.$_MisaResources.appText.employeePageText.statisticalTitle
            .statisticalByDepartment,
        dataForChart: {
          labels: [],
          data: [],
        },
        chartType: "bar",
        chartSize: { width: 480, height: 300 },
        isrotate: true,
      },
    };
  },

  created() {
    try {
      this.statisticalByGender();
      this.statisticalByYearOlds();
      this.statisticalByDepartment();
    } catch (error) {
      console.error(error);
    }
  },

  methods: {
    /**
     * /**
     * Author: PNNHai
     * Date:
     * @param {*} propertyKey thuộc tính muốn thống kê
     * Description: Hàm thực hiện thống kê dữ liệu theo thuộc tính
     */
    async statisticalByKey(propertyKey) {
      try {
        this.$store.state.isLoading = true;
        const res = await employeeService.statisticalByProperty(propertyKey);
        if (res?.success) {
          setTimeout(() => {
            this.$store.state.isLoading = false;
          }, 1000);
        } else {
          this.$store.state.isLoading = false;
        }
        return res;
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện thống kê dữ liệu theo giới tính
     */
    async statisticalByGender() {
      try {
        const res = await this.statisticalByKey("gender");
        if (res.success) {
          const statisticalData = res.data;
          const data = {};

          data.labels = statisticalData.map((item) =>
            convertGender(item.fieldDataName)
          );
          data.data = statisticalData.map((item) => item.count);

          this.genderStatistical.dataForChart = data;
        }
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện thống kê dữ liệu theo độ tuổi
     */
    async statisticalByYearOlds() {
      try {
        const res = await this.statisticalByKey("dateOfBirth");
        if (res.success) {
          const statisticalData = res.data;
          const data = {};

          data.labels = statisticalData.map((item) => item.fieldDataName);
          data.data = statisticalData.map((item) => item.count);

          this.yearOldsStatistical.dataForChart = data;
        }
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện thống kê dữ liệu theo phòng ban
     */
    async statisticalByDepartment() {
      try {
        const res = await this.statisticalByKey("departmentId");
        if (res.success) {
          const statisticalData = res.data;
          const data = {};

          data.labels = statisticalData.map((item) => item.fieldDataName);
          data.data = statisticalData.map((item) => item.count);

          this.departmentStatistical.dataForChart = data;
        }
      } catch (error) {
        console.error(error);
      }
    },
  },
};
</script>
<style lang="css">
@import "./employee-statistical";
</style>
