import axios from "axios";
import store from "../store";

const baseAxios = axios.create({
  baseURL: "https://localhost:7085/api/v1",
  // headers: {
  //   Accept: "application/json",
  //   "Content-Type": "application/json",
  // },
  // withCredentials: true, // gửi cookie, session lên server
});

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
  (error) => {
    // Xử lý lỗi response dựa trên HTTP status code
    if (error.response) {
      const status = error.response.status;
      if (status === 400) {
        // alert("Dữ liệu không hợp lệ. " + error.response.data.UserMessage);
        store.state.dialogNotify.isShow = true;
        store.state.dialogNotify.text = error.response.data.UserMessage;
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
      store.state.dialogNotify.text =
        "Có lỗi xảy ra, vui lòng liên hệ Misa để được giúp đỡ";
    }
    return Promise.reject(error);
  }
);

export default baseAxios;
