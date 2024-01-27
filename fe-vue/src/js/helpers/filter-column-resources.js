import MisaEnums from "./enums";
import MisaResources from "./resources";

export const filterColumnResources = {
  // Trường hợp filter với các trường text
  textFilter: [
    // Bắt đầu bằng
    {
      filterTypeName: MisaResources.filterType.textFilter.startWith,
      filterTypeValue: MisaEnums.TEXT_FILTER_OPTION.START_WITH,
    },
    // Kết thúc bằng
    {
      filterTypeName: MisaResources.filterType.textFilter.endWith,
      filterTypeValue: MisaEnums.TEXT_FILTER_OPTION.END_WITH,
    },
    // Chứa
    {
      filterTypeName: MisaResources.filterType.textFilter.contain,
      filterTypeValue: MisaEnums.TEXT_FILTER_OPTION.CONTAIN,
    },
    // Không chứa
    {
      filterTypeName: MisaResources.filterType.textFilter.notContain,
      filterTypeValue: MisaEnums.TEXT_FILTER_OPTION.NOT_CONTAIN,
    },
    // Bằng
    {
      filterTypeName: MisaResources.filterType.textFilter.equals,
      filterTypeValue: MisaEnums.TEXT_FILTER_OPTION.EQUALS,
    },
    // Không bằng
    {
      filterTypeName: MisaResources.filterType.textFilter.notEquals,
      filterTypeValue: MisaEnums.TEXT_FILTER_OPTION.NOT_EQUALS,
    },
  ],

  // Trường hợp filter với các trường so sánh (number, date)
  comparisionFilter: [
    // >
    {
      filterTypeName: MisaResources.filterType.comparisionFilter.greater,
      filterTypeValue: MisaEnums.COMPARISON_FILTER_OPTION.GREATER,
    },
    // <
    {
      filterTypeName: MisaResources.filterType.comparisionFilter.less,
      filterTypeValue: MisaEnums.COMPARISON_FILTER_OPTION.LESS,
    },
    // >=
    {
      filterTypeName:
        MisaResources.filterType.comparisionFilter.greaterOrEquals,
      filterTypeValue: MisaEnums.COMPARISON_FILTER_OPTION.GREATER_OR_EQUALS,
    },
    // <=
    {
      filterTypeName: MisaResources.filterType.comparisionFilter.lessOrEquals,
      filterTypeValue: MisaEnums.COMPARISON_FILTER_OPTION.LESS_OR_EQUALS,
    },
    // =
    {
      filterTypeName: MisaResources.filterType.comparisionFilter.equals,
      filterTypeValue: MisaEnums.COMPARISON_FILTER_OPTION.EQUALS,
    },
    // !=
    {
      filterTypeName: MisaResources.filterType.comparisionFilter.notEquals,
      filterTypeValue: MisaEnums.COMPARISON_FILTER_OPTION.NOT_EQUALS,
    },
  ],

  // Trường hợp filter với các trường lựa chọn
  selectionFilter: [
    // =
    {
      filterTypeName: MisaResources.filterType.selectionFilter.equals,
      filterTypeValue: MisaEnums.SELECTION_FILTER_OPTION.EQUALS,
    },
    // !=
    {
      filterTypeName: MisaResources.filterType.selectionFilter.notEquals,
      filterTypeValue: MisaEnums.SELECTION_FILTER_OPTION.NOT_EQUALS,
    },
  ],
};
