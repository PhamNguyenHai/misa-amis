import Vuex from "vuex";
import { findArrayIndexByAttribute, generateGuid } from "@/js/common/common.js";

const storeData = {
  state: {
    // state loading
    isLoading: true,

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
};

const store = new Vuex.Store(storeData);
export default store;
