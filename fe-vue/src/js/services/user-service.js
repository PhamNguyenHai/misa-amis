import BaseService from "./base-service";

class UserService extends BaseService {
  constructor() {
    super("/users");
  }

  /**
   * Author: PNNHai
   * Date:
   * @param {*} username
   * @param {*} password
   * @returns : Thông tin đăng nhập thành công (token, username, userid, userrole)
   * Description: Hàm thực hiện đăng nhập vào hệ thống
   */
  async login(emailOrPhoneNumber, password) {
    const res = await this.baseApiService.post(this.endpoint("/Login"), {
      emailOrPhoneNumber,
      password,
    });
    return res;
  }

  /**
   * Author: PNNHai
   * Date:
   * @returns
   * Description: Hàm thực hiện đăng xuất khỏi hệ thống
   */
  async logout() {
    const res = await this.baseApiService.post(this.endpoint("/Logout"));
    return res;
  }

  /**
   * Author: PNNHai
   * Date:
   * @returns Access token mới
   * Description: Hàm thực hiện làm mới refresh token
   */
  async refresh() {
    const res = await this.baseApiService.post(this.endpoint("/Refresh-Token"));
    return res;
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
}

const userService = new UserService();

export default userService;
