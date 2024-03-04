import axios from "axios";
import store from "../store";
import MisaResources from "../helpers/resources";
import userService from "./user-service";

const baseAxios = axios.create({
  baseURL: "https://localhost:7085/api/v1",
  headers: {
    Accept: "application/json",
    "Content-Type": "application/json",
  },
  withCredentials: true, // gửi cookie, session lên server
  credentials: "include",
});

// Thêm intercepter để gắn token vào yêu cầu API
baseAxios.interceptors.request.use((config) => {
  const token = localStorage.getItem("accessToken");
  if (token) {
    config.headers["Authorization"] = `Bearer ${token}`;
  }
  return config;
});

let isRefresh = false;
// Thêm một intercepter để xử lý lỗi trên toàn bộ ứng dụng
baseAxios.interceptors.response.use(
  (response) => {
    // Trả về dữ liệu từ phản hồi
    return {
      success: true,
      statusCode: response.status,
      data: response.data,
    };
  },
  async (error) => {
    // Xử lý lỗi response dựa trên HTTP status code
    if (error.response) {
      const status = error.response.status;
      if (status === 400) {
        // alert("Dữ liệu không hợp lệ. " + error.response.data.UserMessage);
        store.state.dialogNotify.isShow = true;
        store.state.dialogNotify.text = error.response.data.UserMessage;
      } else if (status === 401) {
        const originalConfig = error.config;
        if (originalConfig.url !== "users/login" && !isRefresh) {
          isRefresh = true;
          try {
            const res = await userService.refresh();
            if (res?.success) {
              const newToken = res.data.accessToken;
              error.config.headers["Authorization"] = `Bearer ${newToken}`;
              localStorage.setItem("accessToken", newToken);
              isRefresh = false;
              return baseAxios(error.config);
            }
          } catch (err) {
            store.dispatch("logout");
            return Promise.reject(err);
          }
        } else {
          store.state.dialogNotify.isShow = true;
          store.state.dialogNotify.text =
            "Có lỗi xác thực. Vui lòng đăng nhập lại.";
        }
      } else if (status === 403) {
        // alert("Không tìm thấy tài nguyên. " + error.response.data.UserMessage);
        store.state.dialogNotify.isShow = true;
        store.state.dialogNotify.text =
          "Bạn không có quyền sử dụng tính năng này";
      } else if (status === 404) {
        // alert("Không tìm thấy tài nguyên. " + error.response.data.UserMessage);
        store.state.dialogNotify.isShow = true;
        store.state.dialogNotify.text = error.response.data.UserMessage;
      } else {
        // alert("Có lỗi xảy ra, vui lòng liên hệ Misa để được giúp đỡ.");
        store.state.dialogNotify.isShow = true;
        store.state.dialogNotify.text = error.response.data.UserMessage;
      }
    } else {
      // Xử lý lỗi khi không kết nối được đến server
      // alert("Có lỗi xảy ra, vui lòng liên hệ Misa để được giúp đỡ");
      store.state.dialogNotify.isShow = true;
      store.state.dialogNotify.text = MisaResources.errorHandle.serveError;
    }
    return Promise.reject(error);
  }
);

export default baseAxios;
