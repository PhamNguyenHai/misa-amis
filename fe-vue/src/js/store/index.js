import Vuex from "vuex";
import { findArrayIndexByAttribute, generateGuid } from "@/js/common/common.js";

const storeData = {
  state: {
    // Trạng thái đăng nhập người dùng
    loginStatus: {
      accessToken: null,
      userId: null,
      fullName: null,
      userRole: null,
      email: null,
    },

    // state loading
    isLoading: false,

    // state cho sidebar có phải mini không
    isSidebarMini: false,

    // toasts
    toast: [],

    dialogNotify: {
      isShow: false,
      type: "warning",
      text: "",
    },
  },

  mutations: {
    /**
     * Author: PNNHai
     * Date:
     * @param {*} state
     * @param {*} user : Thông tin đăng nhập người dùng
     * Description: Hàm thực hiện cập nhật trạng thái đăng nhập
     */
    updateLoginStatus(
      state,
      { accessToken, userId, fullName, userRole, email }
    ) {
      try {
        state.loginStatus.accessToken = accessToken;
        state.loginStatus.userId = userId;
        state.loginStatus.fullName = fullName;
        state.loginStatus.userRole = userRole;
        state.loginStatus.email = email;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {*} state
     * Description: Hàm thực hiện đăng xuất tài khoản khỏi hệ thống
     */
    resetLoginStatus(state) {
      try {
        state.loginStatus.accessToken = null;
        state.loginStatus.userId = null;
        state.loginStatus.fullName = null;
        state.loginStatus.userRole = null;
        state.loginStatus.email = null;
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {*} state : state của store
     * @param {*} param1 : object chứa type: loại toast và message
     * Description: Hàm thực hiện thêm mới toast
     */
    addToast(state, { type, message }) {
      try {
        const id = generateGuid();
        const newToast = {
          id: id,
          type: type,
          message: message,
        };

        state.toast.push(newToast);

        setTimeout(() => {
          const toastIndex = findArrayIndexByAttribute(state.toast, "id", id);
          if (toastIndex !== -1) {
            state.toast.splice(toastIndex, 1);
          }
        }, 5000); // Ẩn Toast sau 5 giây
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {*} state : state của store
     * @param {*} toastId : Id của toast cần tắt
     * Description: Hàm thực hiện tắt toast thông qua id
     */
    closeToast(state, toastId) {
      try {
        const toastIndex = findArrayIndexByAttribute(
          state.toast,
          "id",
          toastId
        );
        if (toastIndex !== -1) {
          state.toast.splice(toastIndex, 1);
        }
      } catch (err) {
        console.error(err);
      }
    },
  },

  actions: {
    logout({ commit }) {
      localStorage.removeItem("accessToken");
      commit("resetLoginStatus");
    },
  },
};

const store = new Vuex.Store(storeData);
export default store;
