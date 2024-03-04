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
          $store.state.loginStatus.userRole === $_MisaEnums.LOGIN_ROLE.ADMIN
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
          $store.state.loginStatus.userRole === $_MisaEnums.LOGIN_ROLE.USER
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
          v-if="$store.state.loginStatus.userRole === null"
        >
          <router-link to="/login" class="header-login">Đăng nhập</router-link>
        </div>
        <div class="header-user" v-else @click.stop="toggleUserFunction">
          <div class="user-avatar"></div>
          <h3 class="user-name">
            {{ $store.state.loginStatus.fullName }}
          </h3>
          <div class="user-name-arrow">
            <div class="user-option-items" v-if="isShowUserOption">
              <div class="user-option-infor">
                <div class="user-option-avatar"></div>
                <div class="user-title-infor">
                  <div class="user-option-name">
                    {{ $store.state.loginStatus.fullName }}
                  </div>
                  <div class="user-option-email">
                    {{ $store.state.loginStatus.email }}
                  </div>
                </div>
              </div>
              <ul class="user-functions">
                <li class="user-option">
                  <div class="account-change-password-icon"></div>
                  Đổi mật khẩu
                </li>
                <li class="user-option">
                  <div class="account-setting-icon"></div>
                  Thiết lập tài khoản
                </li>
              </ul>
              <div class="user-logout-area" @click.stop="handleLogout">
                <div class="logout-icon"></div>
                Đăng xuất
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </header>
</template>
<script>
// import userService from "@/js/services/user-service";
export default {
  name: "TheHeader",
  data() {
    return {
      isShowUserOption: false,
    };
  },

  methods: {
    /**
     * Author : PNNHai
     * Date :
     * Description : Hàm thực hiện toggle bảng chọn chức năng với user
     */
    toggleUserFunction() {
      this.isShowUserOption = !this.isShowUserOption;
    },

    /**
     * Author : PNNHai
     * Date :
     * Description : Hàm thực hiện gửi thông báo đăng xuất khỏi hệ thống
     */
    handleLogout() {
      this.$emit("notifyLogout");
    },
  },
};
</script>
<style lang="css" scoped>
@import "./header.css";
</style>
