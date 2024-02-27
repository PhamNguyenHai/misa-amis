<template lang="">
  <header
    class="header"
    :class="{ 'logo-header-mini': $store.state.isSidebarMini }"
  >
    <div class="header-left">
      <div class="header-toggle"></div>
      <router-link
        to="/admin"
        class="header-login"
        v-if="
          $store.state.loginStatus.loginedUserRole ===
          $_MisaEnums.LOGIN_ROLE.ADMIN
        "
      >
        <div class="header-software-area">
          <div class="header-logo"></div>
          <h3 class="header-software-name">KẾ TOÁN</h3>
        </div>
      </router-link>
      <router-link
        to="/user"
        class="header-login"
        v-else-if="
          $store.state.loginStatus.loginedUserRole ===
          $_MisaEnums.LOGIN_ROLE.USER
        "
      >
        <div class="header-software-area">
          <div class="header-logo"></div>
          <h3 class="header-software-name">KẾ TOÁN</h3>
        </div>
      </router-link>
      <router-link to="/" class="header-login" v-else>
        <div class="header-software-area">
          <div class="header-logo"></div>
          <h3 class="header-software-name">KẾ TOÁN</h3>
        </div>
      </router-link>
    </div>

    <div class="header-right">
      <div class="header-company-current">
        <div class="header-company-option"></div>
        <h3 class="header-company-name">
          CÔNG TY TNHH SẢN XUẤT - THƯƠNG MẠI - DỊCH VỤ QUY PHÚC
        </h3>
        <div class="header-company-arrow"></div>
      </div>

      <div class="header-infor">
        <div
          class="header-unlogin"
          v-if="$store.state.loginStatus.loginedUserName === null"
        >
          <router-link to="/login" class="header-login">Đăng nhập</router-link>
        </div>
        <div class="header-user" v-else>
          <div class="user-avatar"></div>
          <h3 class="user-name">
            {{ $store.state.loginStatus.loginedUserName }}
          </h3>
          <div class="user-name-arrow" @click.stop="toggleUserFunction">
            <ul class="user-functions" v-if="isShowUserOption">
              <li
                class="user-option"
                @click.stop="handleShowPersonalInfor"
                v-if="
                  $store.state.loginStatus.loginedAccountRole !==
                  $_MisaEnums.LOGIN_ROLE.ADMIN
                "
              >
                Thông tin cá nhân
              </li>
              <li class="user-option" @click.stop="handleLogout">Đăng xuất</li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  </header>
</template>
<script>
import userService from "@/js/services/user-service";
export default {
  name: "TheHeader",

  data() {
    return {
      isShowUserOption: false,
    };
  },

  methods: {
    toggleUserFunction() {
      this.isShowUserOption = !this.isShowUserOption;
    },

    handleShowPersonalInfor() {
      alert("Show infor");
      this.isShowUserOption = false;
    },

    async handleLogout() {
      try {
        this.$store.state.isLoading = true;
        const res = await userService.logout();
        if (res?.success) {
          this.$store.commit("addToast", {
            type: "success",
            message: "Đăng xuất thành công",
          });
          localStorage.removeItem("accessToken");
          localStorage.removeItem("userRole");
          localStorage.removeItem("userId");
          localStorage.removeItem("userName");

          this.isShowUserOption = false;
          this.$store.commit("logout");

          this.$router.push("/");
        }
      } catch (err) {
        console.error(err);
      } finally {
        this.$store.state.isLoading = false;
      }
    },
  },
};
</script>
<style lang="css" scoped>
@import "./header.css";
</style>
