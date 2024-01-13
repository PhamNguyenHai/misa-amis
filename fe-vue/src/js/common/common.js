/**
 * Author: PNNHai
 * Date:
 * @param {String} str : Chuỗi cần loại bỏ âm tiếng Việt
 * @returns : Chuỗi sau khi đã loại bỏ âm tiếng Việt
 * Description: Hàm thực hiện loại bỏ âm tiếng Việt (bỏ dấu)
 */
export const removeVietnameseSigns = (str) => {
  try {
    str = str.toLowerCase();
    str = str.replace(/[áàảãạăắằẳẵặâấầẩẫậ]/g, "a");
    str = str.replace(/[éèẻẽẹêếềểễệ]/g, "e");
    str = str.replace(/[íìỉĩị]/g, "i");
    str = str.replace(/[óòỏõọôốồổỗộơớờởỡợ]/g, "o");
    str = str.replace(/[úùủũụưứừửữự]/g, "u");
    str = str.replace(/[ýỳỷỹỵ]/g, "y");
    str = str.replace(/đ/g, "d");
    return str;
  } catch (err) {
    console.error(err);
  }
};

/**
 * author: PNNHai
 * date:
 * Description: Hàm tìm kiếm chỉ số của phần tử trong mảng obj theo attribute
 * @param {Array[Object]} arr : Mảng để tìm kiếm
 * @param {String} attribute : thuộc tính muốn tìm kiếm theo
 * @param {*} valueToFind : giá trị muốn tìm kiếm
 * @returns Chỉ số của phần tử trong mảng. Nếu ko có thì -> -1
 */
export const findArrayIndexByAttribute = (arr, attribute, valueToFind) => {
  try {
    if (arr.length <= 0) return -1;

    for (let i = 0; i < arr.length; i++) {
      if (arr[i][attribute] === valueToFind) return i;
    }

    return -1;
  } catch (err) {
    console.error(err);
  }
};

/**
 * author: PNNHai
 * date:
 * Description: Hàm để sinh chuỗi Guid
 * @returns Chuối Guid
 */
export const generateGuid = () => {
  try {
    return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(
      /[xy]/g,
      function (c) {
        const r = (Math.random() * 16) | 0,
          v = c == "x" ? r : (r & 0x3) | 0x8;
        return v.toString(16);
      }
    );
  } catch (err) {
    console.error(err);
  }
};

/**
 * Author: PNNHai
 * Date:
 * @param {String} word : camelCase
 * @returns PascalCase
 * Description: Hàm chuyển từ kiểu camelCase -> PascalCase
 */
export const convertCamelCaseToPascelCase = (word) => {
  try {
    const firstLetter = word.slice(0, 1).toUpperCase();
    const otherLetters = word.slice(1);

    return `${firstLetter}${otherLetters}`;
  } catch (err) {
    console.error(err);
  }
};
