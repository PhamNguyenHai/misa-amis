<template lang="">
  <button
    :id="buttonId"
    class="button"
    :class="buttonClass"
    :type="buttonType"
    :title="buttonTooltips"
  >
    <div v-if="iconClass" :class="iconClass" class="icon-with-button"></div>
    {{ buttonName }}
  </button>
</template>
<script>
export default {
  name: "MisaButton",

  props: {
    buttonName: { type: String, default: "" },
    buttonId: { type: String, default: "" },
    buttonClass: { type: String, default: "" },
    buttonType: { type: String, default: "button" },
    iconClass: { type: String, default: "" },
    buttonTooltips: { type: String, required: false },
  },

  data() {
    return {};
  },
};
</script>
<style lang="css" scoped>
@import "./button.css";
</style>
