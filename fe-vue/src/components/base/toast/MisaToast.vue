<template lang="">
  <div class="toast" :class="'toast--' + toastType">
    <div class="toast__icon"></div>
    <div class="toast__infor">
      <h3 class="toast__title">{{ toastTitle }}</h3>
      <p class="toast__message">
        <slot></slot>
      </p>
    </div>
    <div class="toast__close" @click.stop="$emit('notyfyCloseToast')"></div>
  </div>
</template>
<script>
export default {
  name: "MisaToast",

  props: {
    toastType: {
      type: String,
      default: "infor",
    },
  },

  computed: {
    toastTitle() {
      return this.$_MisaResources.toastTitle[this.toastType];
    },
  },
};
</script>
<style lang="css" scoped>
@import "./toast.css";
</style>
