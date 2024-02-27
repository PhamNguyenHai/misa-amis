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
}

const userService = new UserService();

export default userService;
