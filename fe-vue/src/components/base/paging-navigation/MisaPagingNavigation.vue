<template lang="">
  <div class="paging-navigation-bar">
    <span
      class="direct-btn"
      @click.stop="onClickChangePage($_MisaEnums.PAGE_TYPE.PREVIOUS)"
    >
      {{ $_MisaResources.appText.previousPage }}
    </span>
    <span
      v-for="page in pages"
      class="page-number-btn"
      :class="{
        'current-page': currentPageProcessing === page,
        space: page == '...',
      }"
      :key="page"
      @click.stop="onClickChangePage($_MisaEnums.PAGE_TYPE.PAGE_NUMBER, page)"
    >
      {{ page }}
    </span>
    <span
      class="direct-btn"
      @click.stop="onClickChangePage($_MisaEnums.PAGE_TYPE.NEXT)"
    >
      {{ $_MisaResources.appText.nextPage }}
    </span>
  </div>
</template>
<script>
export default {
  name: "MisaPagingNavigation",

  props: {
    currentPage: { type: Number, default: 1 },
    pageSize: { type: Number, default: 10 },
    totalPage: { type: Number, required: true },
  },

  data() {
    return {
      currentPageProcessing: this.currentPage,
      pageSizeProcessing: this.pageSize,
    };
  },

  computed: {
    pages() {
      const pages = [];
      for (let i = 1; i <= this.totalPage; i++) {
        if (
          i === 1 ||
          i === this.totalPage ||
          (i <= this.currentPageProcessing + 2 &&
            i >= this.currentPageProcessing - 2)
        ) {
          pages.push(i);
        } else if (
          i === this.currentPageProcessing + 3 ||
          i == this.currentPageProcessing - 3
        ) {
          pages.push("...");
        }
      }
      return pages;
    },
  },

  methods: {
    onClickChangePage(pageType, page) {
      try {
        // Trường hợp click vào nút Trước
        if (pageType === this.$_MisaEnums.PAGE_TYPE.PREVIOUS) {
          if (this.currentPageProcessing > 1) this.currentPageProcessing--;
        }
        // Trường hợp click vào nút sau
        else if (pageType === this.$_MisaEnums.PAGE_TYPE.NEXT) {
          if (this.currentPageProcessing < this.totalPage)
            this.currentPageProcessing++;
        }
        // Trường hợp click vào page số cụ thể
        else {
          if (page !== "...") this.currentPageProcessing = page;
        }

        // Truyền emit cho component cha báo hiệu đã thay đổi trang
        this.$emit("update:currentPage", this.currentPageProcessing);
      } catch (err) {
        console.error(err);
      }
    },
  },
};
</script>
<style lang="css" scoped>
@import "./paging-navigation.css";
</style>
