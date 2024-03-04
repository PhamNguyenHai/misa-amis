import MisaEnums from "./enums";

const userResources = {
  objectId: "userId",
  resources: [
    {
      title: "Tên người dùng",
      columnKey: "fullName",
      align: "left",
      width: 290,
      columnType: "text",
      filterType: MisaEnums.FILTER_COLUMN_TYPE.TEXT_TYPE,
    },
    {
      title: "Email",
      columnKey: "email",
      width: 290,
      align: "left",
      columnType: "text",
      filterType: MisaEnums.FILTER_COLUMN_TYPE.TEXT_TYPE,
    },
    {
      title: "Số điện thoại",
      columnKey: "phoneNumber",
      align: "left",
      width: 270,
      columnType: "text",
      filterType: MisaEnums.FILTER_COLUMN_TYPE.TEXT_TYPE,
      formatType: MisaEnums.FORMAT_TYPE.PHONE_NUMBER,
    },
    {
      title: "Quyền truy cập",
      columnKey: "role",
      align: "left",
      width: 290,
      columnType: "text",
      filterType: MisaEnums.FILTER_COLUMN_TYPE.TEXT_TYPE,
      formatType: MisaEnums.FORMAT_TYPE.USER_ROLE,
    },
  ],
};

export default userResources;
