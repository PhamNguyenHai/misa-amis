import baseAxios from "./base-axios";

class BaseService {
  constructor(baseUrl) {
    this.baseUrl = baseUrl;
    this.baseApiService = baseAxios;
  }

  endpoint(url) {
    try {
      return this.baseUrl + url;
    } catch (err) {
      console.error(err);
    }
  }

  /**
   * Author: PNNHai
   * Date:
   * @returns danh sách dữ liệu
   * Description: method thực hiện lấy hết dữ liệu
   */
  async get() {
    try {
      const res = await this.baseApiService.get(this.baseUrl);
      return res;
    } catch (err) {
      console.error(err);
    }
  }

  /**
   * Author: PNNHai
   * Date:
   * @param {*} dataAdd : dữ liệu muốn thêm
   * @returns kết quả thêm mới
   * Description: method thực hiện thêm mới dữ liệu
   */
  async post(dataAdd) {
    try {
      const res = await this.baseApiService.post(this.baseUrl, dataAdd);
      return res;
    } catch (err) {
      console.error(err);
    }
  }

  /**
   * Author: PNNHai
   * Date:
   * @param {*} id : id của đối tượng muốn update
   * @param {*} dataUpdate : dữ liệu update
   * @returns
   * Description: method update dữ liệu
   */
  async put(id, dataUpdate) {
    try {
      const res = await this.baseApiService.put(
        this.baseUrl + `/${id}`,
        dataUpdate
      );
      return res;
    } catch (err) {
      console.error(err);
    }
  }

  /**
   * Author: PNNHai
   * Date:
   * @param {*} id : id muốn xóa
   * @returns dữ liệu trạng thái xóa
   * Description: method thực hiện xóa đối tượng
   */
  async delete(id) {
    try {
      const res = await this.baseApiService.delete(this.baseUrl + `/${id}`);
      return res;
    } catch (err) {
      console.error(err);
    }
  }

  /**
   * Author: PNNHai
   * Date:
   * @param {*} ids : danh sách id cần xóa
   * @returns trạng thái xóa
   * Description: method thực hiện xóa nhiều đối tượng
   */
  async deleteMany(ids) {
    try {
      const res = await this.baseApiService.delete(this.baseUrl, { data: ids });
      return res;
    } catch (err) {
      console.error(err);
    }
  }

  /**
   * Author: PNNHai
   * Date:
   * @param {*} workingTable : bảng muốn lấy file template
   * @returns : dữ liệu file trả về
   * Description: method thực hiện get file template
   */
  async dowloadTemplateFile(workingTable) {
    try {
      const res = await this.baseApiService.get(
        this.endpoint(`/Dowload-File-Template?workingTable=${workingTable}`),
        { responseType: "blob" } // Yêu cầu phản hồi dưới dạng Blob
      );
      return res;
    } catch (err) {
      console.error(err);
    }
  }
}
export default BaseService;
