import BaseService from "./base-service";

class EmployeeService extends BaseService {
  constructor() {
    super("/employees");
  }

  /**
   * Author: PNNHai
   * Date:
   * @param {*} id : id cần lấy data
   * @returns Kết quả trả về từ api
   * Description: method thực hiện lấy dữ liệu bằng id từ api về
   */
  async getById(id) {
    try {
      const res = await this.baseApiService.get(this.endpoint(`/${id}`));
      return res;
    } catch (err) {
      console.error(err);
    }
  }

  /**
   * Author: PNNHai
   * Date:
   * @returns kết quả mã code mới từ api
   * Description: method thực hiện lấy mã mới từ api
   */
  async getNewCode() {
    try {
      const res = await this.baseApiService.get(this.endpoint("/NewCode"));
      return res;
    } catch (err) {
      console.error(err);
    }
  }

  /**
   * Author: PNNHai
   * Date:
   * @param {*} param: param truyền vào để lấy data paging
   * @returns : method thực hiện lấy dữ liệu kết hợp phân trang, sắp xếp tìm kiếm và lọc dữ liệu
   * Description: method thực hiện lấy dữ liệu kết hợp phân trang, tìm kiếm, sắp xếp và lọc dữ liệu
   */
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

  /**
   * Author: PNNHai
   * Date:
   * @param {*} exportData : dữ liệu để thực hiện xuất file excel
   * @returns dữ liệu xuất file excel
   * Description: method thực hiện xuất file excel
   */
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

  /**
   * Author: PNNHai
   * Date:
   * @param {*} file : file truyền vào để import
   * @returns trả về kết quả sau khi đọc file
   * Description: method thực hiện import file excel
   */
  async importExcelFile(file) {
    try {
      const res = await this.baseApiService.post(
        this.endpoint("/Import"),
        file,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );
      return res;
    } catch (err) {
      console.error(err);
    }
  }

  /**
   * Author: PNNHai
   * Date:
   * @param {*} workingImportTable : bảng thực hiện import
   * @param {*} selectType : loại lựa chọn (0: không insert; 1: có import)
   * @returns trạng thái import
   */
  async confirmImport(workingImportTable, selectType) {
    try {
      const res = await this.baseApiService.post(
        this.endpoint(
          `/Confirm-Import-Excel?workingTable=${workingImportTable}&confirmType=${selectType}`
        )
      );
      return res;
    } catch (err) {
      console.error(err);
    }
  }

  /**
   * Author: PNNHai
   * Date:
   * @param {*} propertyKey : Key của thuộc tính cần thực hiện thống kê
   * Description: Hàm thực hiện thống kê dữ liệu thông qua key thuộc tính
   */
  async statisticalByProperty(propertyKey) {
    try {
      const res = await this.baseApiService.get(
        this.endpoint(`/Statistical?propertyKey=${propertyKey}`)
      );
      return res;
    } catch (err) {
      console.error(err);
    }
  }
}

const employeeService = new EmployeeService();

export default employeeService;
