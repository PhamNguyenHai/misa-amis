<template>
  <div class="chart-area">
    <h3 class="chart-title" v-if="chartTitle">{{ chartTitle }}</h3>
    <canvas
      ref="chartCanvas"
      :style="{
        width: chartSize.width + 'px',
        height: chartSize.height + 'px',
      }"
    ></canvas>
  </div>
</template>

<script>
import Chart from "chart.js/auto";

export default {
  name: "MisaChart",
  props: {
    chartTitle: { type: String, required: false },
    dataForChart: { type: Object, required: true },
    chartType: { type: String, required: true },
    chartSize: { type: Object, default: () => ({ width: 300, height: 300 }) },
    isrotate: { type: Boolean, default: false },
  },
  watch: {
    dataForChart: function () {
      this.renderChart();
    },
  },

  methods: {
    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện render biểu đồ
     */
    renderChart() {
      const chartData = {
        labels: this.dataForChart.labels,
        datasets: [
          {
            data: this.dataForChart.data,
          },
        ],
      };

      const chartOptions = {
        indexAxis: this.isrotate === true ? "y" : "x",
        responsive: true,
        plugins: {
          legend: {
            display: false,
          },
        },
      };

      new Chart(this.$refs.chartCanvas, {
        type: this.chartType,
        data: chartData,
        options: chartOptions,
      });
    },
  },
};
</script>

<style lang="css" scoped>
@import "./chart.css";
</style>
