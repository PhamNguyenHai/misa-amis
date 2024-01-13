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
