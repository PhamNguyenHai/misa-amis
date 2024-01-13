import MisaEnums from "./enums";
import MisaResources from "./resources";

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
