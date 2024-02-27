import { createRouter, createWebHistory } from "vue-router";
import MisaEnums from "@/js/helpers/enums";
import store from "@/js/store";

// Anonymous
import MisaLogin from "@/views/anonymous/login/MisaLogin.vue";
import MisaHome from "@/views/anonymous/home/MisaHome.vue";

// Admin
import AdminHomePage from "@/views/admin/home/AdminHome.vue";
import EmployeeAdminPage from "@/views/admin/employee/employee-page/EmployeePage.vue";
import EmployeeStatisticalAdminPage from "@/views/admin/employee-statistical/EmployeeStatistical.vue";
import PurchasePage from "@/views/admin/purchase/PurchasePage.vue";
import ManagementPage from "@/views/admin/management/ManagementPage.vue";
import TaxPage from "@/views/admin/tax/TaxPage.vue";

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
    path: "/admin/purchase",
    name: "PurchasePage",
    component: PurchasePage,
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
  if (to.matched.some((route) => route.meta.requiresAuth)) {
    const roleType = store.state.loginStatus.loginedUserRole;
    if (roleType === MisaEnums.LOGIN_ROLE.ADMIN) {
      if (to.matched.some((route) => route.meta.adminOnly)) {
        next();
      } else {
        next("/admin"); // Chuyển hướng đến trang user nếu admin cố gắng truy cập vào trang user
      }
    } else if (roleType === MisaEnums.LOGIN_ROLE.USER) {
      if (to.matched.some((route) => route.meta.userOnly)) {
        next();
      } else {
        next("/user"); // Chuyển hướng đến trang admin nếu user cố gắng truy cập vào trang admin
      }
    } else {
      next("/login"); // Chuyển hướng đến trang đăng nhập nếu chưa đăng nhập
    }
  } else {
    next();
  }
});

export default router;
