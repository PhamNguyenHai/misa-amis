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
      title: "Đơn vị",
      columnKey: "departmentId",
      align: "left",
      width: 170,
    },
    {
      title: "Chức danh",
      columnKey: "positionName",
      align: "left",
      width: 170,
    },
    {
      title: "Số CCCD",
      tooltips: "Số căn cước công dân",
      columnKey: "identityNumber",
      align: "left",
      width: 150,
    },
    {
      title: "Ngày cấp",
      columnKey: "identityIssuedDate",
      align: "left",
      width: 150,
      formatType: MisaEnums.FORMAT_TYPE.DATE_FOR_FE,
    },
    {
      title: "Nơi cấp",
      columnKey: "identityIssuedPlace",
      align: "left",
      width: 150,
    },
    {
      title: "Địa chỉ",
      columnKey: "address",
      align: "left",
      width: 150,
    },
    {
      title: "Số di động",
      columnKey: "phoneNumber",
      align: "left",
      width: 150,
    },
    {
      title: "SĐT cố định",
      tooltips: "Số điện thoại cố định",
      columnKey: "landlineNumber",
      align: "left",
      width: 150,
    },
    {
      title: "Email",
      columnKey: "email",
      align: "left",
      width: 150,
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
