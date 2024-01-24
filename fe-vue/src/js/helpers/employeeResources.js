import MisaEnums from "./enums";
import departmentService from "../services/DepartmentService";

/**
 * Author : PNNHai
 * Date:
 * Description: Hàm để load toàn bộ departments để đẩy vào prop cho combobox
 */
async function getDepartments() {
  try {
    const res = await departmentService.get();
    if (res.success) {
      return res.data;
    }
  } catch (err) {
    console.error(err);
  }
}

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
      filterOption: (await getDepartments())?.map((item) => ({
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
