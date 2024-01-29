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
