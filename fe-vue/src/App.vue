<template>
  <template v-if="!isLoginPage">
    <the-header @notifyLogout="handleLogout" />
    <the-sidebar />
    <the-main />
  </template>
  <router-view v-else></router-view>
  <misa-loading v-if="$store.state.isLoading" />

  <!-- Toast -->
  <div class="toast-wrapper">
    <misa-toast
      v-for="toast in $store.state.toast"
      :key="toast.id"
      :toastType="toast.type"
      @notyfyCloseToast="closeToast(toast.id)"
    >
      {{ toast.message }}
    </misa-toast>
  </div>

  <!-- Dialog -->
  <misa-dialog
    v-if="$store.state.dialogNotify.isShow"
    :dialogType="$store.state.dialogNotify.type"
    :numberOfButton="$_MisaEnums.DIALOG_TYPE_BUTTON.ONE_BUTTON"
    @notifyCloseDialog="handleCloseDialog"
  >
    <template v-slot:dialog-content>
      <p class="dialog__message">{{ $store.state.dialogNotify.text }}</p>
    </template>
  </misa-dialog>
</template>

<script>
import TheHeader from "@/components/layouts/TheHeader/TheHeader.vue";
import TheSidebar from "@/components/layouts/TheSidebar/TheSidebar.vue";
import TheMain from "@/components/layouts/TheMain/TheMain.vue";
import MisaLoading from "./components/base/loading/MisaLoading.vue";
import MisaToast from "@/components/base/toast/MisaToast.vue";
import userService from "./js/services/user-service";
import { decodeJwtToken } from "./js/helpers/jwt-helper";

export default {
  name: "App",
  components: {
    TheHeader,
    TheSidebar,
    TheMain,
    MisaLoading,
    MisaToast,
  },

  mounted() {
    this.getCurrentUser();
  },

  computed: {
    /**
     * Author: PNNHai
     * Date:
     * Description: Thực hiện kiểm tra xem có phải đang là trang login không
     */
    isLoginPage() {
      return this.$route.path === "/login";
    },
  },

  methods: {
    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện lấy ra người dùng hiện tại (nếu có trong local storage)
     */
    getCurrentUser() {
      try {
        const accessToken = localStorage.getItem("accessToken");
        if (accessToken) {
          const jwtData = decodeJwtToken(accessToken);
          this.$store.commit("updateLoginStatus", {
            accessToken,
            userId: jwtData.userId,
            fullName: jwtData.fullName,
            userRole: parseInt(jwtData.role),
            email: jwtData.email,
          });
        }
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện đăng xuất khỏi hệ thống
     */
    async handleLogout() {
      try {
        this.$store.state.isLoading = true;
        const res = await userService.logout();
        if (res?.success) {
          this.$store.dispatch("logout");

          this.isShowUserOption = false;
          this.$router.push("/");
        }
      } catch (err) {
        console.error(err);
      } finally {
        this.$store.state.isLoading = false;
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * @param {*} id : id của toast cần tắt
     * Hàm tắt toast với id tương ứng
     */
    closeToast(id) {
      try {
        this.$store.commit("closeToast", id);
      } catch (err) {
        console.error(err);
      }
    },

    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện tắt dialog
     */
    handleCloseDialog() {
      this.$store.state.dialogNotify.isShow = false;
    },
  },
};
</script>

<style>
@import "../src/css/main.css";
</style>
