import MisaEnums from "./enums";

const loginLogResources = {
  objectId: "loginId",
  resources: [
    {
      title: "Thời gian đăng nhập",
      columnKey: "createdDate",
      formatType: MisaEnums.FORMAT_TYPE.DATE_TIME_FOR_FE,
    },
    {
      title: "Địa chỉ IP thiết bị",
      columnKey: "ipAddress",
    },
    {
      title: "Thiết bị đăng nhập",
      columnKey: "operatingSystem",
    },
    {
      title: "Thời gian đăng xuất",
      columnKey: "logoutDate",
      formatType: MisaEnums.FORMAT_TYPE.DATE_TIME_FOR_FE,
    },
  ],
};

export default loginLogResources;
