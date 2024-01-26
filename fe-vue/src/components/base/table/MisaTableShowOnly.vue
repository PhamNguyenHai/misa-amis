<template lang="">
  <div class="show-only-table">
    <table v-if="tableColumns?.resources?.length > 0 && tableData?.length > 0">
      <thead>
        <tr>
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
          >
            {{ header.title }}
          </th>
        </tr>
      </thead>
      <tbody v-if="rowsData.length > 0">
        <tr v-for="tableRow in rowsData" :key="tableRow[tableColumns.objectId]">
          <td
            v-for="(column, columnIndex) in tableColumns.resources"
            :key="columnIndex"
            :style="{
              width: column.width + 'px',
              'min-width': column.width - 30 + 'px',
              'max-width': column.width + 50 + 'px',
              'text-align': column.align,
            }"
          >
            <template v-if="Array.isArray(tableRow[column.columnKey])">
              <ul v-if="tableRow[column.columnKey].length > 0">
                <li
                  v-for="(item, index) in tableRow[column.columnKey]"
                  class="item-error"
                  :key="index"
                  :title="item"
                >
                  {{ item }}
                </li>
              </ul>
              <span v-else class="item-valid">Hợp lệ </span>
            </template>

            <template v-else>
              {{ tableRow[column.columnKey] }}
            </template>
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
  </div>
</template>
<script>
export default {
  name: "MisaTableShowOnly",
  props: {
    // các heading cho table
    // Component sử dụng : EmployeePage
    tableColumns: { type: Object },

    // data cho table
    // Component sử dụng : EmployeePage
    tableData: { type: Array },
  },

  data() {
    return {
      // Biến để hứng dữ liệu từ prop tableData truyền vào
      rowsData: [],
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
};
</script>
<style lang="css" scoped>
@import "./table-show-only.css";
</style>
