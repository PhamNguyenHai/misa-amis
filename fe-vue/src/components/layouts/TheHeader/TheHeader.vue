<template lang="">
  <header
    class="header"
    :class="{ 'logo-header-mini': $store.state.isSidebarMini }"
  >
    <div class="header-left">
      <div class="header-toggle"></div>
      <div class="header-software-area">
        <div class="header-logo"></div>
        <h3 class="header-software-name">KẾ TOÁN</h3>
      </div>
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
        <div class="header-notify-icon"></div>
        <div class="header-user">
          <div class="user-avatar"></div>
          <h3 class="user-name">Phạm Nguyễn Nguyên Hải</h3>
          <div class="user-name-arrow"></div>
        </div>
      </div>
    </div>
  </header>
</template>
<script>
export default {
  name: "TheHeader",
};
</script>
<style lang="css" scoped>
@import "./header.css";
</style>
