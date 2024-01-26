import MisaEnums from "./enums";

const employeeImportResources = {
  objectId: null,
  resources: [
    {
      title: "Mã nhân viên",
      columnKey: "employeeCode",
      align: "left",
      width: 130,
    },
    {
      title: "Tên nhân viên",
      columnKey: "employeeName",
      align: "left",
      width: 180,
    },
    {
      title: "Giới tính",
      columnKey: "gender",
      width: 110,
      align: "left",
      formatType: MisaEnums.FORMAT_TYPE.GENDER,
    },
    {
      title: "Ngày sinh",
      columnKey: "dateOfBirth",
      width: 150,
      align: "center",
      formatType: MisaEnums.FORMAT_TYPE.DATE_FOR_FE,
    },
    {
      title: "Số CCCD",
      tooltips: "Số căn cước công dân",
      columnKey: "identityNumber",
      align: "left",
      width: 150,
    },
    {
      title: "Chức danh",
      columnKey: "positionName",
      align: "left",
      width: 170,
    },
    {
      title: "Đơn vị",
      columnKey: "departmentId",
      align: "left",
      width: 170,
    },
    {
      title: "Số tài khoản",
      columnKey: "bankNumber",
      align: "left",
      width: 150,
    },
    {
      title: "Tên ngân hàng",
      columnKey: "bankName",
      align: "left",
      width: 190,
    },
    {
      title: "Chi nhánh",
      columnKey: "bankBranch",
      align: "left",
      width: 280,
    },
    {
      title: "Trạng thái",
      columnKey: "errors",
      align: "left",
      width: 280,
    },
  ],
};

export default employeeImportResources;
