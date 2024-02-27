import MisaEnums from "./enums";
// import departmentService from "../services/department-service";

/**
 * Author : PNNHai
 * Date:
 * Description: Hàm để load toàn bộ departments để đẩy vào prop cho combobox
 */
// async function getDepartments() {
//   try {
//     if (store.state.loginStatus.loginedUserRole === null) {
//       return null;
//     }
//     const res = await departmentService.get();
//     if (res?.success) {
//       return res.data;
//     }
//   } catch (err) {
//     console.error(err);
//   }
// }

const departments = [
  {
    departmentId: "11452b0c-768e-5ff7-0d63-eeb1d8ed8cef",
    departmentName: "Phòng Nhân Sự",
  },
  {
    departmentId: "142cb08f-7c31-21fa-8e90-67245e8b283e",
    departmentName: "Phòng Kỹ Thuật",
  },
  {
    departmentId: "17120d02-6ab5-3e43-18cb-66948daf6128",
    departmentName: "Phòng Kinh Doanh",
  },
  {
    departmentId: "4577565a-7e3e-493a-74dd-867949feb8b5",
    departmentName: "Phòng Hành Chính",
  },
  {
    departmentId: "469b3ece-744a-45d5-957d-e8c757976496",
    departmentName: "Phòng Bảo An",
  },
  {
    departmentId: "4e272fc4-7875-78d6-7d32-6a1673ffca7c",
    departmentName: "Phòng Đào Tạo",
  },
  {
    departmentId: "768f8e64-7d10-20c9-967d-e8c757976496",
    departmentName: "Phòng Chiến Lược",
  },
];

const employeeResources = {
  objectId: "employeeId",
  resources: [
    {
      title: "Mã nhân viên",
      columnKey: "employeeCode",
      align: "left",
      width: 150,
      columnType: "text",
      filterType: MisaEnums.FILTER_COLUMN_TYPE.TEXT_TYPE,
    },
    {
      title: "Tên nhân viên",
      columnKey: "employeeName",
      align: "left",
      width: 200,
      columnType: "text",
      filterType: MisaEnums.FILTER_COLUMN_TYPE.TEXT_TYPE,
    },
    {
      title: "Giới tính",
      columnKey: "gender",
      width: 110,
      align: "left",
      formatType: MisaEnums.FORMAT_TYPE.GENDER,
      columnType: "selection",
      filterType: MisaEnums.FILTER_COLUMN_TYPE.SELECTION,
      filterOption: [
        {
          keyDisplayName: "Nam",
          keyValue: 0,
        },
        {
          keyDisplayName: "Nữ",
          keyValue: 1,
        },
        {
          keyDisplayName: "Khác",
          keyValue: 2,
        },
      ],
    },
    {
      title: "Ngày sinh",
      columnKey: "dateOfBirth",
      width: 150,
      align: "center",
      formatType: MisaEnums.FORMAT_TYPE.DATE_FOR_FE,
      columnType: "date",
      filterType: MisaEnums.FILTER_COLUMN_TYPE.COMPARABLE_TYPE,
    },
    {
      title: "Số CCCD",
      tooltips: "Số căn cước công dân",
      columnKey: "identityNumber",
      align: "left",
      width: 150,
      columnType: "text",
      filterType: MisaEnums.FILTER_COLUMN_TYPE.TEXT_TYPE,
    },
    {
      title: "Chức danh",
      columnKey: "positionName",
      align: "left",
      width: 170,
      columnType: "text",
      filterType: MisaEnums.FILTER_COLUMN_TYPE.TEXT_TYPE,
    },
    {
      title: "Đơn vị",
      columnKey: "departmentName",
      align: "left",
      width: 170,
      columnType: "selection",
      filterType: MisaEnums.FILTER_COLUMN_TYPE.SELECTION,
      filterOption: departments?.map((item) => ({
        keyDisplayName: item.departmentName,
        keyValue: item.departmentId,
      })), // Lấy danh sách department
    },
    {
      title: "Số tài khoản",
      columnKey: "bankNumber",
      align: "left",
      width: 150,
      columnType: "text",
      filterType: MisaEnums.FILTER_COLUMN_TYPE.TEXT_TYPE,
    },
    {
      title: "Tên ngân hàng",
      columnKey: "bankName",
      align: "left",
      width: 190,
      columnType: "text",
      filterType: MisaEnums.FILTER_COLUMN_TYPE.TEXT_TYPE,
    },
    {
      title: "Chi nhánh",
      columnKey: "bankBranch",
      align: "left",
      width: 280,
      columnType: "text",
      filterType: MisaEnums.FILTER_COLUMN_TYPE.TEXT_TYPE,
    },
  ],
};

export default employeeResources;
