<template lang="">
  <div class="sidebar" :class="{ 'sidebar-mini': $store.state.isSidebarMini }">
    <div class="sidebar-list">
      <router-link to="/" class="sidebar-item">
        <div class="sidebar-item-icon general-page"></div>
        <div class="sidebar-item-name">Tổng quan</div>
      </router-link>
      <router-link to="/purchase" class="sidebar-item">
        <div class="sidebar-item-icon buy-page-icon"></div>
        <div class="sidebar-item-name">Mua hàng</div>
      </router-link>
      <router-link to="sale" class="sidebar-item">
        <div class="sidebar-item-icon sale-page-icon"></div>
        <div class="sidebar-item-name">Bán hàng</div>
      </router-link>
      <router-link to="management" class="sidebar-item">
        <div class="sidebar-item-icon management-icon"></div>
        <div class="sidebar-item-name">Quản lý hóa đơn</div>
      </router-link>
      <router-link to="tax" class="sidebar-item">
        <div class="sidebar-item-icon tax-icon"></div>
        <div class="sidebar-item-name">Thuế thu nhập</div>
      </router-link>
    </div>
    <div class="sidebar-shrink" @click="onClickSidebarShrink">
      <div class="sidebar-shrink-icon"></div>
      <span class="sidebar-shrink-title">Thu gọn</span>
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
