<template lang="">
  <div class="sidebar" :class="{ 'sidebar-mini': $store.state.isSidebarMini }">
    <div
      class="sidebar-list"
      v-if="
        $store.state.loginStatus.loginedUserRole ===
        $_MisaEnums.LOGIN_ROLE.ADMIN
      "
    >
      <router-link to="/admin" class="sidebar-item">
        <div class="sidebar-item-icon general-page"></div>
        <div class="sidebar-item-name">
          {{ $_MisaResources.sidebar.home }}
        </div>
      </router-link>
      <router-link to="/admin/employee" class="sidebar-item">
        <div class="sidebar-item-icon sale-page-icon"></div>
        <div class="sidebar-item-name">Quản lý nhân viên</div>
      </router-link>
      <router-link to="/admin/employee-statistical" class="sidebar-item">
        <div class="sidebar-item-icon sale-page-icon"></div>
        <div class="sidebar-item-name">
          {{ $_MisaResources.sidebar.employeeStatistical }}
        </div>
      </router-link>
      <router-link to="/admin/purchase" class="sidebar-item">
        <div class="sidebar-item-icon buy-page-icon"></div>
        <div class="sidebar-item-name">{{ $_MisaResources.sidebar.buy }}</div>
      </router-link>
      <router-link to="/admin/management" class="sidebar-item">
        <div class="sidebar-item-icon management-icon"></div>
        <div class="sidebar-item-name">
          {{ $_MisaResources.sidebar.invoiceManagement }}
        </div>
      </router-link>
      <router-link to="/admin/tax" class="sidebar-item">
        <div class="sidebar-item-icon tax-icon"></div>
        <div class="sidebar-item-name">{{ $_MisaResources.sidebar.tax }}</div>
      </router-link>
    </div>

    <div
      class="sidebar-list"
      v-else-if="
        $store.state.loginStatus.loginedUserRole === $_MisaEnums.LOGIN_ROLE.USER
      "
    >
      <router-link to="/user" class="sidebar-item">
        <div class="sidebar-item-icon general-page"></div>
        <div class="sidebar-item-name">
          {{ $_MisaResources.sidebar.home }}
        </div>
      </router-link>
      <router-link to="/user/employee" class="sidebar-item">
        <div class="sidebar-item-icon sale-page-icon"></div>
        <div class="sidebar-item-name">Nhân viên</div>
      </router-link>
    </div>

    <div class="sidebar-list" v-else>
      <router-link to="/" class="sidebar-item">
        <div class="sidebar-item-icon general-page"></div>
        <div class="sidebar-item-name">
          {{ $_MisaResources.sidebar.home }}
        </div>
      </router-link>
    </div>

    <div class="sidebar-shrink" @click="onClickSidebarShrink">
      <div class="sidebar-shrink-icon"></div>
      <span class="sidebar-shrink-title">{{
        $_MisaResources.appText.sidebarShrinkTitle
      }}</span>
    </div>
  </div>
</template>
<script>
export default {
  name: "TheSidebar",

  methods: {
    /**
     * Author : PNNHai
     * Date :
     * Description : Hàm sẽ truyền emit ra bên ngoài báo hiệu thay đổi sidebar khi click vào nút thu gọn
     */
    onClickSidebarShrink() {
      try {
        this.$store.state.isSidebarMini = !this.$store.state.isSidebarMini;
      } catch (err) {
        console.error(err);
      }
    },
  },
};
</script>
<style lang="css">
@import "./sidebar.css";
</style>
