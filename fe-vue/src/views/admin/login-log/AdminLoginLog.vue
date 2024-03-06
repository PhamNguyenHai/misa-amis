<template lang="">
  <div class="login-log-area">
    <h2 class="login-log-heading">Nhật kí đăng nhập, đăng xuất</h2>
    <div class="login-log-main">
      <ul class="login-log-list">
        <li
          class="login-log-item"
          v-for="loginLog in loginLogs"
          :key="loginLog.loginId"
        >
          <h3
            class="login-log-item-heading login-log-logout-heading"
            v-if="loginLog.logoutDate"
          >
            Bạn đã đăng xuất khỏi hệ thống
          </h3>
          <h3 class="login-log-item-heading" v-else>
            Bạn đã đăng nhập vào hệ thống
          </h3>
          <div>
            <template
              v-for="resource in loginLogResources.resources"
              :key="resource.objectId"
            >
              <div
                class="login-log-infor"
                v-if="
                  loginLog[resource.columnKey] != null &&
                  loginLog[resource.columnKey] !== ''
                "
              >
                {{ resource.title }}:
                {{
                  resource?.formatType === undefined
                    ? loginLog[resource.columnKey]
                    : formatData(
                        resource.formatType,
                        loginLog[resource.columnKey]
                      )
                }}
              </div>
            </template>
          </div>
        </li>
      </ul>
    </div>
  </div>
</template>
<script>
import userService from "@/js/services/user-service";
import { convertDateTimeForFE } from "@/js/helpers/convert-data";
import loginLogResources from "@/js/helpers/login-log-resources";
export default {
  name: "AdminLoginLog",

  data() {
    return {
      // Khuôn mẫu các dữ liệu để hiển thị
      loginLogResources: {},

      // Dữ liệu để hiển thị
      loginLogs: [],
    };
  },

  async created() {
    this.loginLogResources = loginLogResources;
    const userId = this.$store.state.loginStatus.userId;
    this.loginLogs = await this.getLoginLogInfor(userId);
  },

  methods: {
    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện lấy thống tin lịch sử đăng nhập, đăng xuất từ api về
     */
    async getLoginLogInfor(userId) {
      try {
        const res = await userService.getLoginLog(userId);
        if (res?.success) {
          const data = res.data;
          return data;
        }
        return null;
      } catch (err) {
        console.error(err);
      }
    },

    formatData(formatType, value) {
      try {
        if (formatType === this.$_MisaEnums.FORMAT_TYPE.DATE_TIME_FOR_FE) {
          return convertDateTimeForFE(value);
        }
        return value;
      } catch (err) {
        console.error(err);
      }
    },
  },
};
</script>
<style lang="css">
@import "./login-log.css";
</style>
