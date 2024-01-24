<template lang="">
  <div :class="fieldClass">
    <label :for="dateInputId" :title="title" v-if="label">{{ label }}</label>
    <input
      :id="dateInputId"
      type="date"
      class="date-field"
      :class="dateInputClass"
      :title="inputTitle"
      v-model="inputValue"
      @change="onChangeData"
    />
  </div>
</template>
<script>
export default {
  name: "MisaDateField",

  props: {
    // Label cho date field
    label: { type: String, default: "" },

    // tooltips label
    title: { type: String, default: "" },

    // class cho toàn bộ field đó
    fieldClass: { required: false },

    // tooltips cho input
    inputTitle: { required: false },

    // Id của date field
    dateInputId: { type: String },

    // Class cho date field
    dateInputClass: { type: String, default: "" },

    // value cuar model dùng để binding 2 chiều cho component
    modelValue: { required: false },
  },

  data() {
    return {
      inputValue: null, // Dữ liệu đang được edit trong component
    };
  },

  watch: {
    // Theo dõi sự thay đổi của prop value của component đó nếu có thay đổi thì gán vào biến inputValue
    modelValue: function () {
      try {
        if (this.modelValue) {
          const date = new Date(this.modelValue);
          let day = String(date.getDate()).padStart(2, "0");
          let month = String(date.getMonth() + 1).padStart(2, "0");
          const year = date.getFullYear();

          const dateString = `${year}-${month}-${day}`;

          this.inputValue = dateString;
        }
      } catch (err) {
        console.error(err);
      }
    },
  },

  created() {
    try {
      this.inputValue = this.modelValue; // gán prop modelValue cho inputValue để tương tác dữ liệu
    } catch (err) {
      console.error(err);
    }
  },

  methods: {
    /**
     * Author: PNNHai
     * Date:
     * Description: Hàm thực hiện xử lý khi change date
     */
    onChangeData() {
      try {
        // EMIT CHO BINDING 2 CHIỀU CỦA COMPONENT ĐỂ UPDATE LẠI ModelValue
        this.$emit("update:modelValue", this.inputValue);
        this.$emit("notifyChangeDate");
      } catch (err) {
        console.error(err);
      }
    },
  },
};
</script>
<style lang="css">
@import "./date-field.css";
</style>

<!-- <template lang="">
  <div class="date-picker-field" :class="fieldClass">
    <label :for="dateInputId" :title="title" v-if="label">{{ label }}</label>
    <div class="date-picker" :class="datePickerClass">
      <div class="date-picker-area">
        <input
          :id="dateInputId"
          type="text"
          placeholder="dd/MM/yyyy"
          class="date-picker-display"
          :title="inputTitle"
          @keydown="handleKeyDown"
          @mouseup="handleMouseUp"
          value="dd/mm/yyyy"
          ref="dateInput"
        />
        <div
          class="date-picker-icon"
          @click.stop="toggleDatePickerSelector"
        ></div>
      </div>
      <div class="date-picker-selector" v-if="isShowDatePicker">
        <div class="date-picker-heading">
          <div class="date-picker-infor">
            <div class="date-infor-heading">
              {{ `${currentMonth}, ${selectedYear}` }}
            </div>
            <div class="date-infor-dropdown"></div>
          </div>
          <div class="date-picker-directions">
            <div
              class="date-picker-previous"
              @click.stop="goToPreviousMonth"
            ></div>
            <div class="date-picker-next" @click.stop="goToNextMonth"></div>
          </div>
        </div>

        <div class="date-calendar">
          <div class="calendar-area">
            <div class="date-weekdays">
              <div
                v-for="(weekday, index) in weekdays"
                :key="index"
                class="date-weekday"
              >
                {{ weekday }}
              </div>
            </div>
            <div class="date-days">
              <div
                v-for="(date, index) in firstDayOfSelectedMonth > 0
                  ? firstDayOfSelectedMonth - 1
                  : 6"
                :key="index"
                :class="['date-day-inactive']"
                @click.stop="
                  selectDate(
                    lastDateOfLastMonth -
                      (firstDayOfSelectedMonth > 0
                        ? firstDayOfSelectedMonth
                        : 7) +
                      date +
                      1,
                    $_MisaEnums.DATE_PICKER_SELECTION.PREVIOUS_MONTH
                  )
                "
              >
                {{
                  lastDateOfLastMonth -
                  (firstDayOfSelectedMonth > 0 ? firstDayOfSelectedMonth : 7) +
                  date +
                  1
                }}
              </div>

              <div
                v-for="(date, index) in lastDateOfSelectedMonth"
                :key="index"
                :class="[
                  'date-day',
                  { 'date-day-selected': isDateSelected(date) },
                ]"
                @click.stop="
                  selectDate(
                    date,
                    $_MisaEnums.DATE_PICKER_SELECTION.THIS_SELECTED_MONTH
                  )
                "
              >
                {{ date }}
              </div>

              <div
                v-for="(date, index) in 7 -
                (lastDayOfSelectedMonth === 0 ? 7 : lastDayOfSelectedMonth)"
                :key="index"
                :class="['date-day-inactive']"
                @click.stop="
                  selectDate(
                    date,
                    this.$_MisaEnums.DATE_PICKER_SELECTION.NEXT_MONTH
                  )
                "
              >
                {{ date }}
              </div>
            </div>
          </div>
          <div class="date-picker-footer" @click.stop="handleSelectTodayDate">
            {{ $_MisaResources.datePicker.today }}
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import { convertDateForFE, convertDateForBE } from "@/js/helpers/convert-data";
import { DAYS_IN_WEEK, MONTHS_IN_YEAR } from "@/js/helpers/date-picker";
export default {
  name: "MisaDateField",

  props: {
    // Label cho date field
    label: { type: String, default: "" },

    // tooltips label
    title: { type: String, default: "" },

    // class cho toàn bộ field đó
    fieldClass: { required: false },

    // tooltips cho input
    inputTitle: { required: false },

    // Id của date field
    dateInputId: { type: String },

    // Class cho date field
    datePickerClass: { type: String, default: "" },

    // value cuar model dùng để binding 2 chiều cho component
    modelValue: { required: false },
  },

  data() {
    return {
      isShowDatePicker: false,
      selectedDate: "",
      selectedYear: new Date().getFullYear(),
      selectedMonth: new Date().getMonth(),
    };
  },

  watch: {
    modelValue: function () {
      if (this.modelValue) {
        const dateValue = new Date(this.modelValue);
        if (dateValue) {
          const date = dateValue.getDate();
          this.selectedMonth = dateValue.getMonth();
          this.selectedYear = dateValue.getFullYear();

          this.selectedDate = this.handleSelectDate(date);
          this.handleDisplayDateValue();
        }
      }
    },
  },

  computed: {
    currentMonth() {
      const months = MONTHS_IN_YEAR;
      // const months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
      return months[this.selectedMonth];
    },

    lastDateOfLastMonth() {
      return new Date(this.selectedYear, this.selectedMonth, 0).getDate();
    },

    lastDateOfSelectedMonth() {
      return new Date(this.selectedYear, this.selectedMonth + 1, 0).getDate();
    },

    firstDayOfSelectedMonth() {
      return new Date(this.selectedYear, this.selectedMonth, 1).getDay();
    },

    lastDayOfSelectedMonth() {
      return new Date(
        this.selectedYear,
        this.selectedMonth,
        this.lastDateOfSelectedMonth
      ).getDay();
    },

    weekdays() {
      return DAYS_IN_WEEK;
    },

    quickMonths() {
      const months = MONTHS_IN_YEAR;
      return months;
    },

    years() {
      const currentYear = new Date().getFullYear();
      return Array.from({ length: 10 }, (_, index) => currentYear - 5 + index);
    },
  },

  methods: {
    isOtherMonthDay(weekIndex, dayIndex) {
      try {
        const firstWeekOfMonth = this.calendar[0];
        const lastWeekOfMonth = this.calendar[this.calendar.length - 1];

        // Check if the day is in the first or last week of the month
        if (weekIndex === 0 && firstWeekOfMonth[dayIndex] === null) {
          return true; // Day belongs to the previous month
        }
        if (
          weekIndex === this.calendar.length - 1 &&
          lastWeekOfMonth[dayIndex] === null
        ) {
          return true; // Day belongs to the next month
        }

        return false; // Day belongs to the current month
      } catch (err) {
        console.error(err);
      }
    },

    toggleDatePickerSelector() {
      try {
        this.isShowDatePicker = !this.isShowDatePicker;
        this.$emit("notyfyClickChooseDate");
      } catch (err) {
        console.error(err);
      }
    },

    hideDatePickerSelector() {
      try {
        this.isShowDatePicker = false;
      } catch (err) {
        console.error(err);
      }
    },

    selectDate(day, selectType) {
      try {
        if (
          selectType === this.$_MisaEnums.DATE_PICKER_SELECTION.PREVIOUS_MONTH
        ) {
          if (this.selectedMonth - 1 < 0) {
            this.selectedMonth = 11;
            this.selectedYear--;
          } else {
            this.selectedMonth--;
          }
        } else if (
          selectType === this.$_MisaEnums.DATE_PICKER_SELECTION.NEXT_MONTH
        ) {
          if (this.selectedMonth > 11) {
            (this.selectedMonth = 0), this.selectedYear++;
          } else {
            this.selectedMonth++;
          }
        }
        this.selectedDate = this.handleSelectDate(day);
        this.handleDisplayDateValue();
        this.handleUpdateDateValue();
        this.toggleDatePickerSelector();
      } catch (err) {
        console.error(err);
      }
    },

    goToPreviousMonth() {
      try {
        if (this.selectedMonth === 0) {
          this.selectedYear--;
        }
        this.selectedMonth = (this.selectedMonth - 1 + 12) % 12;
      } catch (err) {
        console.error(err);
      }
    },

    goToNextMonth() {
      try {
        if (this.selectedMonth === 11) {
          this.selectedYear++;
        }
        this.selectedMonth = (this.selectedMonth + 1) % 12;
      } catch (err) {
        console.error(err);
      }
    },

    selectYear() {
      // Handle year selection
    },

    isDateSelected(day) {
      try {
        const selectedDate = this.handleSelectDate(day);
        return selectedDate === this.selectedDate;
      } catch (err) {
        console.error(err);
      }
    },

    handleSelectDate(day) {
      try {
        const selectedDate = new Date(
          this.selectedYear,
          this.selectedMonth,
          day
        );
        return convertDateForBE(selectedDate);
      } catch (err) {
        console.error(err);
      }
    },

    handleSelectTodayDate() {
      try {
        const now = new Date();
        this.selectedMonth = now.getMonth();
        this.selectedYear = now.getFullYear();
        this.selectedDate = this.handleSelectDate(now.getDate());
      } catch (err) {
        console.error(err);
      }
    },

    getMaxDays(month, year) {
      if (month < 1 || month > 12) {
        return 0;
      }

      if (year < 1) {
        return 0;
      }

      if (month === 2) {
        if ((year % 4 === 0 && year % 100 !== 0) || year % 400 === 0) {
          return 29; // Năm nhuận
        } else {
          return 28; // Năm không nhuận
        }
      } else if ([4, 6, 9, 11].includes(month)) {
        return 30; // Tháng có 30 ngày
      } else {
        return 31; // Tháng có 31 ngày
      }
    },

    // handleInputDate(event) {
    //   let enteredValue = event.target.value;
    //   let formattedValue = "";

    //   // Loại bỏ các ký tự không phải số
    //   enteredValue = enteredValue.replace(/\D/g, "");

    //   // Tự động thêm "/" vào vị trí phù hợp
    //   if (enteredValue.length > 0) {
    //     formattedValue += enteredValue.substr(0, 2);
    //   }
    //   if (enteredValue.length > 2) {
    //     formattedValue += "/" + enteredValue.substr(2, 2);
    //   }
    //   if (enteredValue.length > 4) {
    //     formattedValue += "/" + enteredValue.substr(4, 4);
    //   }

    //   // // Kiểm tra ngày và tháng
    //   // const parts = formattedValue.split("/");
    //   // let day = parseInt(parts[0], 10) || 0;
    //   // const month = parseInt(parts[1], 10) - 1 || 0;
    //   // const year = parseInt(parts[2], 10) || 0;

    //   // if (month >= 0 && month <= 11 && year > 0) {
    //   //   this.selectedMonth = month;
    //   //   this.selectedYear = year;
    //   // } else {
    //   //   if (day > 31) {
    //   //     day = 31;
    //   //   }
    //   //   // this.handleSelectTodayDate();
    //   // }

    //   // console.log(day, this.selectedMonth, this.selectedYear);

    //   // Gán giá trị đã được định dạng vào ô input
    //   event.target.value = formattedValue;
    // },

    handleMouseUp() {
      const dateInput = this.$refs.dateInput;
      const start = dateInput.selectionStart;

      console.log(dateInput.selectionStart, dateInput.selectionEnd);

      if (start <= 2) {
        // Bôi đậm phần "dd"
        dateInput.setSelectionRange(0, 2);
      } else if (start <= 5) {
        // Bôi đậm phần "mm"
        dateInput.setSelectionRange(3, 5);
      } else {
        dateInput.setSelectionRange(6, 10);
      }
    },

    handleMouseDown() {
      const dateInput = this.$refs.dateInput;
      const start = dateInput.selectionStart;

      if (dateInput.selectionStart === dateInput.selectionEnd) {
        if (start <= 2) {
          // Bôi đậm phần "dd"
          dateInput.setSelectionRange(0, 2);
        } else if (start <= 5) {
          // Bôi đậm phần "mm"
          dateInput.setSelectionRange(3, 5);
        } else {
          dateInput.setSelectionRange(6, 10);
        }
      }
    },

    handleKeyDown() {
      event.preventDefault();

      const dateInput = this.$refs.dateInput;
      const selectionStart = dateInput.selectionStart;

      if (event.key === "ArrowRight") {
        if (selectionStart <= 2) {
          dateInput.setSelectionRange(3, 5);
        } else if (selectionStart <= 5) {
          dateInput.setSelectionRange(6, 10);
        }
        event.preventDefault();
      } else if (event.key === "ArrowLeft") {
        if (selectionStart > 5) {
          dateInput.setSelectionRange(3, 5);
        } else if (selectionStart > 2) {
          dateInput.setSelectionRange(0, 2);
        }
        event.preventDefault();
      } else if (event.key === "Backspace" || event.key === "Delete") {
        if (selectionStart <= 2) {
          // Bôi đậm phần "dd"
          dateInput.value = "dd" + dateInput.value.substring(2);
          dateInput.setSelectionRange(0, 2);
        } else if (selectionStart <= 5) {
          const firstDateArea = dateInput.value.substring(0, 3);
          const lastDateArea = dateInput.value.substring(5);
          dateInput.value = firstDateArea + "mm" + lastDateArea;
          dateInput.setSelectionRange(3, 5);
        } else {
          dateInput.value = dateInput.value.substring(0, 6) + "yyyy";
          dateInput.setSelectionRange(6, 10);
        }
      } else {
        const key = event.key;
        const isNumeric = /^\d$/.test(key);
        const selectionEnd = dateInput.selectionEnd;

        //Nếu là số mới xử lý
        if (isNumeric) {
          // Lấy ra đoạn đg đc select
          const selectedArea = dateInput.value.substring(
            selectionStart,
            selectionEnd
          );

          if (
            selectedArea === "dd" ||
            selectedArea === "mm" ||
            selectedArea === "yyyy" ||
            !this.validateStringStartWithZeroLetter(selectedArea)
          ) {
            // Thay thế số trong vùng chọn bằng số mới và định dạng lại chuỗi
            const newValue = this.replaceSelectedArea(
              dateInput.value,
              selectionStart,
              selectionEnd,
              key
            );

            this.$refs.dateInput.value = newValue;
            dateInput.setSelectionRange(selectionStart, selectionEnd);
          } else {
            // Thay thế số tiếp theo trong vùng chọn và định dạng lại chuỗi
            let newValue = this.replaceNextNumber(
              dateInput.value,
              selectionStart,
              selectionEnd,
              key
            );

            this.$refs.dateInput.value = newValue;

            // alert(1);
            if (selectionStart === 0) {
              newValue = this.validateAndFormatDay(newValue);
            } else if (selectionStart === 3 || selectionStart === 6) {
              newValue = this.validateAndFormatMonth(newValue);
            }
            this.$refs.dateInput.value = newValue;
            dateInput.setSelectionRange(selectionStart, selectionEnd);
          }
        }

        const selectedDate = new Date(this.$refs.dateInput.value);
        if (selectedDate) {
          this.selectedMonth = selectedDate.getMonth();
          this.selectedYear = selectedDate.getFullYear();
          this.handleSelectDate(selectedDate.getDate());
        }
      }
    },

    validateAndFormatDay(value) {
      let day = parseInt(value.substring(0, 2));
      if (day > 31) {
        day = 31;
      } else if (day < 1) {
        day = 1;
      } else if (day < 10 && value.substring(0, 1) !== "0") {
        value = `0${day}${value.substring(2)}`;
      }
      return `${day}${value.substring(2)}`;
    },

    getMaxDay(month, year) {
      const isLeapYear = !year || year % 4 === 0;
      const daysInMonth = {
        1: 31,
        2: isLeapYear ? 29 : 28,
        3: 31,
        4: 30,
        5: 31,
        6: 30,
        7: 31,
        8: 31,
        9: 30,
        10: 31,
        11: 30,
        12: 31,
      };

      return daysInMonth[month];
    },

    validateAndFormatMonth(value) {
      let day = parseInt(value.substring(0, 2));
      let month = parseInt(value.substring(3, 5));
      let year = parseInt(value.substring(6));

      // Nếu có month và month thỏa mãn số
      if (month && !isNaN(month)) {
        if (month > 12) {
          month = 12;
        }
        year = !isNaN(year) ? year : null;
        const maxDay = this.getMaxDay(month, year);

        if (day > parseInt(maxDay)) {
          day = parseInt(maxDay);
        }
        return `${day.toString().padStart(2, "0")}${value.substring(
          2,
          3
        )}${month.toString().padStart(2, "0")}${value.substring(5)}`;
      }
      return value;
    },

    replaceSelectedArea(value, selectionStart, selectionEnd, key) {
      const selectedArea = value.substring(selectionStart, selectionEnd);
      const formattedNumber = key.toString().padStart(selectedArea.length, "0");
      return (
        value.substring(0, selectionStart) +
        formattedNumber +
        value.substring(selectionEnd)
      );
    },

    replaceNextNumber(value, selectionStart, selectionEnd, key) {
      if (value.length >= selectionEnd) {
        const reaplaceArea = value.substring(selectionStart, selectionEnd);
        return (
          value.substring(0, selectionStart) +
          reaplaceArea.substring(1) +
          key +
          value.substring(selectionEnd)
        );
      }
    },

    validateStringStartWithZeroLetter(inputString) {
      const regex = /^0\d*$/;
      return regex.test(inputString);
    },

    // validateDateFormat(dateString) {
    //   const dateRegex = /^(0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/\d{4}$/;
    //   return dateRegex.test(dateString);
    // },

    handleDisplayDateValue() {
      const dateInput = this.$refs.dateInput;
      if (dateInput) {
        dateInput.value = convertDateForFE(this.selectedDate);
      }
    },

    handleUpdateDateValue() {
      try {
        this.$emit("update:modelValue", this.selectedDate);
      } catch (err) {
        console.error(err);
      }
    },
  },
};
</script>
<style lang="css" scoped>
@import "./date-field.css";
</style> -->
