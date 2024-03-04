<template lang="">
  <div class="form-overlay">
    <form
      class="form"
      :style="`width: ${formWidth}px; min-height: ${formMinHeight}px;`"
    >
      <div class="form-header">
        <slot name="form-header-content"></slot>
      </div>

      <div class="form-main">
        <slot name="form-main-content"></slot>
      </div>

      <div class="form-footer">
        <slot name="form-footer-content"></slot>
      </div>
    </form>
  </div>
</template>
<script>
export default {
  name: "BaseForm",

  props: {
    formWidth: { type: Number, default: 0 },

    formMinHeight: { type: Number, default: 0 },
  },
};
</script>
<style lang="css" scoped>
@import "./base-form.css";
</style>
