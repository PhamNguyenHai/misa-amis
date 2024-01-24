import BaseService from "./BaseService";

class EmployeeService extends BaseService {
  constructor() {
    super("/employees");
  }

  async getById(id) {
    const res = await this.baseApiService.get(this.endpoint(`/${id}`));
    return res;
  }

  async getNewCode() {
    const res = await this.baseApiService.get(this.endpoint("/NewCode"));
    return res;
  }

  async filter({
    currentPage,
    pageSize,
    searchString,
    sortColumn,
    isSortDesc,
    filterColumns,
  }) {
    try {
      const res = await this.baseApiService.post(
        this.endpoint("/FilterPaging"),
        {
          currentPage,
          pageSize,
          searchString,
          sortColumn,
          isSortDesc,
          filterColumns,
        }
      );
      return res;
    } catch (err) {
      console.error(err);
    }
  }

  async exportExcel(exportData) {
    try {
      const res = await this.baseApiService.post(
        this.endpoint("/Export"),
        exportData,
        { responseType: "blob" } // Yêu cầu phản hồi dưới dạng Blob
      );
      return res;
    } catch (err) {
      console.error(err);
    }
  }
}

const employeeService = new EmployeeService();

export default employeeService;
