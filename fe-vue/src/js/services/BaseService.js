import baseAxios from "./baseAxios";

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

  async get() {
    try {
      const res = await this.baseApiService.get(this.baseUrl);
      return res;
    } catch (err) {
      console.error(err);
    }
  }

  async post(dataAdd) {
    try {
      const res = await this.baseApiService.post(this.baseUrl, dataAdd);
      return res;
    } catch (err) {
      console.error(err);
    }
  }

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

  async delete(id) {
    try {
      const res = await this.baseApiService.delete(this.baseUrl + `/${id}`);
      return res;
    } catch (err) {
      console.error(err);
    }
  }

  async deleteMany(ids) {
    try {
      const res = await this.baseApiService.delete(this.baseUrl, { data: ids });
      return res;
    } catch (err) {
      console.error(err);
    }
  }
}
export default BaseService;
