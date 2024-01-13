import MisaResources from "./resources";

// textLength là biến giá trị max length được gán ở getFieldValidateRuleFunctions
let textLength = 0;

/**
 * Author: PNNHai
 * Date:
 * Description: Object định nghĩa một số phương thức validate
 */
const validateRulesObj = {
  required: (value) => {
    return value ? "" : MisaResources.validateErrorMessages.emptyValue;
  },

  numberOnly: (value) => {
    return value.match(/^\d+$/)
      ? ""
      : MisaResources.validateErrorMessages.numberOnly;
  },

  maxLength: (value) =>
    value.length > textLength
      ? MisaResources.validateErrorMessages.maxLength(textLength)
      : "",

  email: (value) =>
    value.match(
      /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/
    )
      ? ""
      : MisaResources.validateErrorMessages.invalidFormat,

  identityNumber: (value) =>
    value.match(/^\d{10,12}$/)
      ? ""
      : MisaResources.validateErrorMessages.invalidValue,

  phoneNumber: (value) =>
    value.match(/^(?:\+84|0)(?:\d){9}$/)
      ? ""
      : MisaResources.validateErrorMessages.invalidValue,

  noMoreThanCurrentDate: (value) => {
    const currentDate = new Date();
    const enteredDate = new Date(value);

    // So sánh ngày nhập vào với ngày hiện tại
    return enteredDate <= currentDate
      ? ""
      : MisaResources.validateErrorMessages.notMoreThanCurrentDate;
  },
};

/**
 * Author: PNNHai
 * Date:
 * @param {Array} rules : array các rule ứng với field
 * @returns : array các phương thức validate được định nghĩa ứng với các rules được truyền vào
 * Description: Hàm để lấy ra một array gồm các phương thức ứng với các rules được gán vào
 */
const getFieldValidateRuleFunctions = (fieldRules) => {
  try {
    let rulesFunctions = [];

    // Lấy phương thức ứng với rule đó ở object định nghĩa (validateRulesObj)
    // và push nó vào array để trả về array gồm các phương thức rule cho field đó
    fieldRules.forEach((rule) => {
      // Đối với rule nào có maxLength thì sẽ phải cắt rules ở vị trí có kí tự "_" -> cuối để lấy length
      if (rule.includes("maxLength")) {
        textLength = rule.slice(rule.indexOf("_") + 1);
        rulesFunctions.push(validateRulesObj["maxLength"]);
      } else {
        rulesFunctions.push(validateRulesObj[rule]);
      }
    });
    return rulesFunctions;
  } catch (err) {
    console.error(err);
  }
};

/**
 * Author : PNNHai
 * Date:
 * @param {Array} fieldRules : arr rules của trường tương ứng
 * @param {String} fieldData : dữ liệu nhập vào của trường đó
 * @returns error của trường tương ứng
 * (nếu có) | nếu không -> "" ứng với validateRulesObj đã định nghĩa trên
 */
const getFieldInvalidError = (fieldRules, fieldData, fieldLabel) => {
  try {
    // Nếu tồn tại rules thì mới thực hiện tiếp
    if (fieldRules !== "") {
      // Lấy ra các rules functions của field đó
      const fieldRulesFunctions = getFieldValidateRuleFunctions(fieldRules);
      for (const ruleFuntion of fieldRulesFunctions) {
        const fieldErrorMessage = ruleFuntion(fieldData);

        if (fieldErrorMessage !== "") {
          // Nếu thực thi ruleFunction của field đó mà trả về error message -> return error message
          return `${fieldLabel} ${fieldErrorMessage}`;
        }
      }
      // Duyệt toàn bộ các rules functions của field đó
    }

    // Nếu không có rules thì trả về "" luôn
    return "";
  } catch (err) {
    console.error(err);
  }
};

export default getFieldInvalidError;
