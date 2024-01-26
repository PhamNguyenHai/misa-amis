<template lang="">
  <ul
    class="form-import-progress"
    v-if="importSteps.length > 0 && currentStepIndex"
  >
    <li
      class="import-progress-step"
      :class="{ 'progress--current-step': step.stepIndex === currentStepIndex }"
      v-for="(step, index) in importSteps"
      :key="index"
    >
      {{ step?.stepIndex }}. {{ step?.stepName }}
    </li>
  </ul>
</template>
<script>
export default {
  name: "ImportProgress",
  props: {
    // Các bước trong quá trình import
    importSteps: { type: Array, required: true },

    // Bước hiện tại
    currentStepIndex: { type: Number, required: true },
  },
};
</script>
<style lang="css" scoped>
@import "./import-progress.css";
</style>
