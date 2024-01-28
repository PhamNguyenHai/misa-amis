import { createRouter, createWebHistory } from "vue-router";
import EmployeePage from "@/views/employee/employee-page/EmployeePage.vue";
import PurchasePage from "@/views/purchase/PurchasePage.vue";
import EmployeeStatistical from "@/views/employee-statistical/EmployeeStatistical.vue";
import ManagementPage from "@/views/management/ManagementPage.vue";
import TaxPage from "@/views/tax/TaxPage.vue";

const routers = [
  {
    path: "/",
    name: "EmployeePage",
    component: EmployeePage,
  },
  {
    path: "/purchase",
    name: "PurchasePage",
    component: PurchasePage,
  },
  {
    path: "/employee-statistical",
    name: "EmployeeStatistical",
    component: EmployeeStatistical,
  },
  {
    path: "/management",
    name: "ManagementPage",
    component: ManagementPage,
  },
  {
    path: "/tax",
    name: "TaxPage",
    component: TaxPage,
  },
];
const router = createRouter({
  history: createWebHistory(),
  routes: routers,
});

export default router;
