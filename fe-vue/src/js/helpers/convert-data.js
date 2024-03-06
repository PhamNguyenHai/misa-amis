import MisaEnums from "./enums";
import MisaResources from "./resources";

/**
 * Author: PNNHai
 * Date:
 * @param {*} inputDate : dữ liệu date truyền vào
 * @returns date sau khi format định dạng dd/mm/yyyy
 * Description: Hàm thực hiện format date cho FE
 */
export const convertDateForFE = (inputDate) => {
  try {
    if (inputDate) {
      const date = new Date(inputDate);
      let day = String(date.getDate()).padStart(2, "0");
      let month = String(date.getMonth() + 1).padStart(2, "0");
      const year = date.getFullYear();

      return `${day}/${month}/${year}`;
    }
    return "";
  } catch (err) {
    console.error(err);
  }
};

/**
 * Author: PNNHai
 * Date:
 * @param {*} inputDate : dữ liệu date truyền vào
 * @returns date sau khi format định dạng yyyy-mm-dd
 * Description: Hàm thực hiện format date cho BE
 */
export const convertDateForBE = (inputDate) => {
  try {
    if (inputDate) {
      const date = new Date(inputDate);
      let day = String(date.getDate()).padStart(2, "0");
      let month = String(date.getMonth() + 1).padStart(2, "0");
      const year = date.getFullYear();

      return `${year}-${month}-${day}`;
    }
    return "";
  } catch (err) {
    console.error(err);
  }
};

/**
 * Author: PNNHai
 * Date:
 * @param {*} genderValue : giá trị của gender (0: Nam, 1: Nữ, 2: Khác)
 * @returns (0: Nam, 1: Nữ, 2: Khác)
 * Description: Hàm thực hiện convert giới tính
 */
export const convertGender = (genderValue) => {
  try {
    if (genderValue == MisaEnums.GENDER.MALE)
      return MisaResources.dataFormat.gender.male;
    else if (genderValue == MisaEnums.GENDER.FEMALE)
      return MisaResources.dataFormat.gender.female;
    else return MisaResources.dataFormat.gender.another;
  } catch (err) {
    console.error(err);
  }
};

/**
 * Author: PNNHai
 * Date:
 * @param {*} userRoleValue : giá trị của vai trò người dùng (0: admin, 1: user)
 * @returns (0: Nam, 1: Nữ, 2: Khác)
 * Description: Hàm thực hiện convert vai trò người dùng
 */
export const convertUserRole = (userRoleValue) => {
  try {
    if (userRoleValue == MisaEnums.LOGIN_ROLE.ADMIN) return "Quản trị viên";
    else if (userRoleValue == MisaEnums.LOGIN_ROLE.USER) return "Người sử dụng";
  } catch (err) {
    console.error(err);
  }
};

/**
 * Author: PNNHai
 * Date:
 * @param {*} phoneNumber
 * @returns : Số điện thoại cách nhau bởi dấu .
 * Description: Hàm thực hiện format số điện thoại 3 số cách nhau bởi dấu .
 */
export const formatPhoneNumber = (phoneNumber) => {
  const cleaned = phoneNumber.replace(/\D/g, ""); // Xóa tất cả các ký tự không phải là số trong số điện thoại
  return cleaned.replace(/(\d{4})(\d{3})(\d{3})/, "$1.$2.$3"); // Định dạng số điện thoại với dấu chấm
};

/**
 * Author: PNNHai
 * Date:
 * @param {*} inputDate : thời gian đầu vào
 * @returns
 * Description: Hàm thực hiện format thời gian
 */
export const convertDateTimeForFE = (inputDate) => {
  try {
    if (inputDate) {
      const date = new Date(inputDate);
      let day = String(date.getDate()).padStart(2, "0");
      let month = String(date.getMonth() + 1).padStart(2, "0");
      const year = date.getFullYear();
      const hour = String(date.getHours()).padStart(2, "0");
      const minute = String(date.getMinutes()).padStart(2, "0");

      return `${hour}:${minute} ${day}/${month}/${year}`;
    }
    return "";
  } catch (err) {
    console.error(err);
  }
};
