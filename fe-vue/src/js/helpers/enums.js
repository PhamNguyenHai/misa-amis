const MisaEnums = {
  SORT_TYPE: {
    ASC: 1,
    DESC: 2,
  },
  FORM_MODE: {
    ADD: 0,
    EDIT: 1,
    DUPLICATE: 2,
  },
  FORM_SUBMIT_MODE: {
    SAVE: 0,
    SAVE_ADD: 1,
  },
  ROW_MODE: {
    EDIT: 0,
    DUPLICATE: 1,
    DELETE: 2,
  },
  GENDER: {
    MALE: 0,
    FEMALE: 1,
    ORTHER: 2,
  },
  PAGE_TYPE: {
    PREVIOUS: 0,
    PAGE_NUMBER: 1,
    NEXT: 2,
  },
  FORMAT_TYPE: {
    GENDER: 0,
    DATE_FOR_FE: 1,
  },
  DIALOG_TYPE_BUTTON: {
    NOTIFY: 0,
    ONE_BUTTON: 1,
    TWO_BUTTONS: 2,
    THREE_BUTTONS: 3,
  },
  DIALOG_RESPONSE: {
    NO: 0,
    YES: 1,
  },
  CONFIRM_ACTION: {
    CLOSE: 0,
    DELETE: 1,
    DELETE_MANY: 2,
  },
  FILTER_COLUMN_TYPE: {
    TEXT_TYPE: 0,
    COMPARABLE_TYPE: 1,
    SELECTION: 2,
  },
  TEXT_FILTER_OPTION: {
    START_WITH: 0,
    END_WITH: 1,
    CONTAIN: 2,
    NOT_CONTAIN: 3,
    EQUALS: 4,
    NOT_EQUALS: 5,
  },
  COMPARISON_FILTER_OPTION: {
    GREATER: 0,
    LESS: 1,
    GREATER_OR_EQUALS: 2,
    LESS_OR_EQUALS: 3,
    EQUALS: 4,
    NOT_EQUALS: 5,
  },
  SELECTION_FILTER_OPTION: {
    EQUALS: 0,
    NOT_EQUALS: 1,
  },

  FILTER_POPUP_SELECTION: {
    CANCEL: 0,
    FILTER_APPLY: 1,
  },

  SHORT_KEY: {
    ARROW_UP: 38,
    ARROW_DOWN: 40,
    ENTER: 13,
    S_KEY: 229,
  },

  DATE_PICKER_SELECTION: {
    PREVIOUS_MONTH: 0,
    THIS_SELECTED_MONTH: 1,
    NEXT_MONTH: 2,
  },

  EXPORT_TYPE: {
    EXPORT_ALL: 0,
    EXPORT_WITH_FILTER_CONDITION: 1,
  },
};

export default MisaEnums;
