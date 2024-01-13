<template lang="">
  <div class="dialog-overlay">
    <div class="dialog" :class="'dialog--' + dialogType">
      <div class="dialog__header">
        <h3 class="dialog__heading">{{ dialogHeading }}</h3>
        <div class="dialog__close" @click.stop="onClickCloseDialog"></div>
      </div>
      <div class="dialog__content">
        <div class="dialog__icon"></div>
        <slot name="dialog-content"></slot>
      </div>

      <div
        class="dialog__footer justify--right"
        v-if="numberOfButton === $_MisaEnums.DIALOG_TYPE_BUTTON.ONE_BUTTON"
      >
        <button
          class="button primary-button"
          ref="closeButton"
          @click.stop="onClickCloseDialog"
        >
          {{ $_MisaResources.buttons.agree }}
        </button>
      </div>

      <div
        class="dialog__footer dialog-two-btn"
        v-else-if="
          numberOfButton === $_MisaEnums.DIALOG_TYPE_BUTTON.TWO_BUTTONS
        "
      >
        <button
          class="button normal-button"
          @click.stop="handleDialogResponded($_MisaEnums.DIALOG_RESPONSE.NO)"
        >
          {{ $_MisaResources.buttons.no }}
        </button>
        <button
          class="button primary-button"
          ref="yesButton"
          @click.stop="handleDialogResponded($_MisaEnums.DIALOG_RESPONSE.YES)"
        >
          {{ $_MisaResources.buttons.yes }}
        </button>
      </div>

      <div
        class="dialog__footer"
        v-else-if="
          numberOfButton === $_MisaEnums.DIALOG_TYPE_BUTTON.THREE_BUTTONS
        "
      >
        <div class="dialog-footer--left">
          <button class="button normal-button" @click.stop="onClickCloseDialog">
            {{ $_MisaResources.buttons.cancel }}
          </button>
        </div>
        <div class="dialog-footer--right">
          <button
            class="button normal-button"
            @click.stop="handleDialogResponded($_MisaEnums.DIALOG_RESPONSE.NO)"
          >
            {{ $_MisaResources.buttons.no }}
          </button>
          <button
            class="button primary-button"
            ref="yesButton"
            @click.stop="handleDialogResponded($_MisaEnums.DIALOG_RESPONSE.YES)"
          >
            {{ $_MisaResources.buttons.yes }}
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: "MisaDialog",
  props: {
    dialogType: { type: String, required: true },
    numberOfButton: { type: Number, required: true },
  },

  emits: ["notifyCloseDialog", "notifyDialogResponded"],

  mounted() {
    // Thực hiện focus vào các button trong dialog
    if (
      this.numberOfButton ===
        this.$_MisaEnums.DIALOG_TYPE_BUTTON.THREE_BUTTONS ||
      this.numberOfButton === this.$_MisaEnums.DIALOG_TYPE_BUTTON.TWO_BUTTONS
    ) {
      this.$refs.yesButton.focus();
    } else if (
      this.numberOfButton === this.$_MisaEnums.DIALOG_TYPE_BUTTON.ONE_BUTTON
    ) {
      this.$refs.closeButton.focus();
    }
  },

  computed: {
    dialogHeading() {
      if (this.dialogType === "danger")
        return this.$_MisaResources.dialogText.heading.danger;
      else if (this.dialogType === "confirm")
        return this.$_MisaResources.dialogText.heading.confirm;
      return this.$_MisaResources.dialogText.heading.warning;
    },
  },

  methods: {
    onClickCloseDialog() {
      try {
        this.$emit("notifyCloseDialog");
      } catch (err) {
        console.error(err);
      }
    },

    handleDialogResponded(response) {
      try {
        this.$emit("notifyDialogResponded", response);
      } catch (err) {
        console.error(err);
      }
    },
  },
};
</script>
<style lang="css" scoped>
@import "./dialog.css";
</style>
