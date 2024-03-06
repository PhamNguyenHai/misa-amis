import { createRouter, createWebHistory } from "vue-router";
import MisaEnums from "@/js/helpers/enums";
import store from "@/js/store";
// import { decodeJwtToken } from "@/js/helpers/jwt-helper";

// Anonymous
import MisaLogin from "@/views/anonymous/login/MisaLogin.vue";
import MisaHome from "@/views/anonymous/home/MisaHome.vue";

// Admin
import AdminHomePage from "@/views/admin/home/AdminHome.vue";
import EmployeeAdminPage from "@/views/admin/employee/employee-page/EmployeePage.vue";
import EmployeeStatisticalAdminPage from "@/views/admin/employee-statistical/EmployeeStatistical.vue";
import UserManagementPage from "@/views/admin/user/user-page/UserManagementPage.vue";
import ManagementPage from "@/views/admin/management/ManagementPage.vue";
import TaxPage from "@/views/admin/tax/TaxPage.vue";
import AdminLoginLog from "@/views/admin/login-log/AdminLoginLog.vue";

// User
import UserHomePage from "@/views/user/home/UserHome.vue";
import EmployeeUserPage from "@/views/user/employee/employee-page/EmployeePage.vue";

const routers = [
  //  /
  {
    path: "/",
    name: "MisaHome",
    component: MisaHome,
  },
  // /login
  {
    path: "/login",
    name: "MisaLogin",
    component: MisaLogin,
  },

  //    ADMIN
  {
    path: "/admin",
    name: "AdminHomePage",
    component: AdminHomePage,
    meta: { requiresAuth: true, adminOnly: true },
  },
  {
    path: "/admin/employee",
    name: "EmployeeAdminPage",
    component: EmployeeAdminPage,
    meta: { requiresAuth: true, adminOnly: true },
  },
  {
    path: "/admin/employee-statistical",
    name: "EmployeeStatisticalAdminPage",
    component: EmployeeStatisticalAdminPage,
    meta: { requiresAuth: true, adminOnly: true },
  },
  {
    path: "/admin/user-management",
    name: "UserManagementPage",
    component: UserManagementPage,
    meta: { requiresAuth: true, adminOnly: true },
  },
  {
    path: "/admin/login-log",
    name: "AdminLoginLog",
    component: AdminLoginLog,
    meta: { requiresAuth: true, adminOnly: true },
  },
  {
    path: "/admin/management",
    name: "ManagementPage",
    component: ManagementPage,
    meta: { requiresAuth: true, adminOnly: true },
  },
  {
    path: "/admin/tax",
    name: "TaxPage",
    component: TaxPage,
    meta: { requiresAuth: true, adminOnly: true },
  },

  //    USER
  {
    path: "/user",
    name: "UserHomePage",
    component: UserHomePage,
    meta: { requiresAuth: true, userOnly: true },
  },
  {
    path: "/user/employee",
    name: "EmployeeUserPage",
    component: EmployeeUserPage,
    meta: { requiresAuth: true, userOnly: true },
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes: routers,
});

router.beforeEach((to, from, next) => {
  // const token = localStorage.getItem("accessToken");
  // let roleType = null;
  // if (token) {
  //   try {
  //     roleType = parseInt(decodeJwtToken(token).role);
  //   } catch (err) {
  //     console.err(err);
  //   }
  // }
  const roleType = store.state.loginStatus.userRole;

  if (to.name === "MisaLogin" && roleType != null) {
    // Nếu người dùng đã đăng nhập và truy cập vào trang đăng nhập,
    // chuyển hướng tới trang phù hợp dựa trên vai trò của người dùng
    if (roleType === MisaEnums.LOGIN_ROLE.ADMIN) {
      next("/admin");
    } else if (roleType === MisaEnums.LOGIN_ROLE.USER) {
      next("/user");
    } else {
      next(); // Trường hợp khác, không có vai trò xác định, cho phép truy cập vào trang đăng nhập
    }
  } else if (to.matched.some((route) => route.meta.requiresAuth)) {
    // Kiểm tra xem trang yêu cầu xác thực và người dùng đã đăng nhập hay chưa
    if (roleType != null) {
      // Người dùng đã đăng nhập, tiếp tục kiểm tra vai trò và chuyển hướng tới trang phù hợp
      if (
        roleType === MisaEnums.LOGIN_ROLE.ADMIN &&
        to.matched.some((route) => route.meta.adminOnly)
      ) {
        next();
      } else if (
        roleType === MisaEnums.LOGIN_ROLE.USER &&
        to.matched.some((route) => route.meta.userOnly)
      ) {
        next();
      } else {
        // Người dùng đã đăng nhập nhưng không có quyền truy cập vào trang yêu cầu,
        // chuyển hướng tới trang phù hợp dựa trên vai trò
        if (roleType === MisaEnums.LOGIN_ROLE.ADMIN) {
          next("/admin");
        } else if (roleType === MisaEnums.LOGIN_ROLE.USER) {
          next("/user");
        }
      }
    } else {
      // Người dùng chưa đăng nhập, chuyển hướng tới trang đăng nhập
      next("/login");
    }
  } else {
    next();
  }
});

export default router;
