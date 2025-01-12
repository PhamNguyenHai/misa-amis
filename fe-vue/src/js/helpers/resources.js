const MisaResources = {
  sidebar: {
    home: "Tổng quan",
    employeeStatistical: "Thống kê",
    buy: "Mua hàng",
    invoiceManagement: "Quản lý hóa đơn",
    tax: "Thuế thu nhập",
  },
  validateErrorMessages: {
    emptyValue: "không được phép để trống.",
    invalidValue: "không hợp lệ.",
    invalidFormat: "sai định dạng.",
    numberOnly: "chỉ cho phép nhập số.",
    strongPassword: "không đủ mạnh",
    notMoreThanCurrentDate: "không được lớn hơn ngày hiện tại.",
    maxLength: (max) => `chỉ được phép nhập tối đa ${max} kí tự`,
    minLength: (min) => `cần ít nhất ${min} kí tự`,
  },

  errorHandle: {
    invalidPostData: "Dữ liệu gửi lên không hợp lệ.",
    notFoundError: "Không tìm thấy tài nguyên.",
    noValidRecordsToImport: "Không có nhân viên nào hợp lệ để nhập khẩu.",
    serveError: "Có lỗi xảy ra, vui lòng liên hệ Misa để được giúp đỡ.",
  },

  appText: {
    softwareName: "KẾ TOÁN",
    currentCompanyName: "CÔNG TY TNHH SẢN XUẤT - THƯƠNG MẠI - DỊCH VỤ QUY PHÚC",
    currentUserName: "Phạm Nguyễn Nguyên Hải",
    selectedCount: "Đã chọn : ",
    totalCount: "Tổng số : ",
    records: "bản ghi",
    nextPage: "Sau",
    previousPage: "Trước",
    sidebarShrinkTitle: "Thu gọn",
    emptyDataImage: "Không có dữ liệu",
    findInforTitle: "Tìm kiếm theo mã, tên nhân viên",
    refreshTooltip: "Nạp lại dữ liệu",
    excelWorkingTooltip: "Xuất, nhập khẩu excel",
    recordsPerPage: "bản ghi / trang",

    excelExportTitle: {
      exportAll: "Xuất khẩu tất cả bản ghi",
      exportWithFilerCondition: "Xuất khẩu bản ghi đã lọc",
    },

    excelImportTitle: {
      import: "Nhập khẩu bản ghi",

      importText: {
        employeeImportTitle: "Nhập khẩu nhân viên",
        prepareFile:
          "Chọn dữ liệu đã chuẩn bị dữ liệu để nhập khẩu vào phần mềm",
        noFileSelected: "Chưa có tệp nào được chọn.",
        buttons: {
          helper: "Giúp",
          previous: "Quay lại",
          next: "Tiếp Theo",
          execute: "Thực hiện",
          cancel: "Đóng",
        },
      },

      importEmployeeSteps: {
        step: "Bước",
        chooseSourceFile: "Chọn tệp nguồn",
        checkingData: "Kiểm tra dữ liệu",
        importResult: "Kết quả nhập khẩu",
      },

      importResult: {
        valid: "Hợp lệ",
        importResultTitle: "Kết quả nhập khẩu",
        validRecordsNumber: "Số dòng nhập khẩu thành công",
        invalidRecordsNumber: "Số dòng nhập khẩu không thành công",
        validRecords: "dòng hợp lệ",
        invalidRecords: "dòng không hợp lệ",
      },

      dowload: {
        select: "Chọn",
        here: "tại đây",
        invalidRecord: "Tải về tập tin chứa những dòng dữ liệu không hợp lệ",
        templateFile:
          "Chưa có tệp mẫu để chuẩn bị dữ liệu? Tải tệp excel mẫu mà phần mềm cung cấp để chuẩn bị dữ liệu nhập khẩu",
        importResult: "Tải về tập tin chứa kết quả nhập khẩu",
      },
    },

    employeePageText: {
      sidebarTitle: "Tổng quan",
      pageTitle: "Nhân viên",
      successAction: {
        exportAllSuccess: "Xuất thành công tất cả bản ghi ra file excel.",
        dowloadTemplateExcelImportFileSuccess:
          "Tải tệp mẫu nhập khẩu thành công.",
        exportWithFilterConditionSuccess:
          "Xuất thành công tất cả bản ghi đã lọc ra file excel.",
        deleteSucess: "Xóa thành công 1 bản ghi.",
        deleteManySucess: "Xóa thành công các bản ghi đã chọn.",
        saveDataSuccess: "Lưu thông tin thành công.",
        importSuccess: "Nhập khẩu dữ liệu thành công.",
      },
      confirmTitle: {
        ConfirmToDeleteMany:
          "Bạn có chắc chắn muốn xóa những nhân viên đã chọn không ?",
        ConfirmToDelete: "Bạn có chắc chắn muốn xóa 1 nhân viên không ?",
        ConfirmToSaveWhileDataChanged:
          "Dữ liệu đã bị thay đổi. Bạn có muốn cất không ?",
      },
      notifyTitle: {
        notifyNothingChange:
          "Dữ liệu không có gì thay đổi. Không thể thực hiện cập nhật.",
      },
      statisticalTitle: {
        statisticalByGender: "Thống kê nhân viên theo giới tính",
        statisticalByYearOlds: "Thống kê nhân viên theo độ tuổi",
        statisticalByDepartment: "Thống kê nhân viên theo phòng ban",
      },
    },

    userPageText: {
      sidebarTitle: "Người dùng",
      pageTitle: "Quản lý người dùng",
      successAction: {
        deleteSucess: "Xóa thành công 1 bản ghi.",
        deleteManySucess: "Xóa thành công các bản ghi đã chọn.",
        saveDataSuccess: "Lưu thông tin thành công.",
        importSuccess: "Nhập khẩu dữ liệu thành công.",
      },
      confirmTitle: {
        ConfirmToDeleteMany:
          "Bạn có chắc chắn muốn xóa những người dùng đã chọn không ?",
        ConfirmToDelete:
          "Bạn có chắc chắn muốn xóa 1 người dùng đã chọn không ?",
        ConfirmToSaveWhileDataChanged:
          "Dữ liệu đã bị thay đổi. Bạn có muốn cất không ?",
      },
      notifyTitle: {
        notifyNothingChange:
          "Dữ liệu không có gì thay đổi. Không thể thực hiện cập nhật.",
      },
    },

    purchasePageText: {
      sidebarTitle: "Mua hàng",
      pageTitle: "Trang Mua hàng",
    },

    salePageText: {
      sidebarTitle: "Bán hàng",
      pageTitle: "Trang Bán hàng",
    },

    managementPageText: {
      sidebarTitle: "Quản lý hóa đơn",
      pageTitle: "Trang Quản lý hóa đơn",
    },

    taxPageText: {
      sidebarTitle: "Thuế thu nhập",
      pageTitle: "Trang Thuế thu nhập",
    },
  },

  buttons: {
    unselectedAll: "Bỏ chọn",
    delete: "Xóa tất cả",
    addEmployee: {
      name: "Thêm nhân viên",
      tooltip: "Thêm mới nhân viên (insert)",
    },
    addUser: {
      name: "Thêm người dùng",
      tooltip: "Thêm mới người dùng (insert)",
    },
    cancel: "Hủy",
    save: {
      name: "Cất",
      tooltip: "Cất (Ctrl + s)",
    },
    saveAndAdd: {
      name: "Cất và Thêm",
      tooltip: "Cất và Thêm (Ctrl + Shift + s)",
    },
    no: "Không",
    yes: "Có",
    agree: "Đồng ý",
    close: "Đóng",
  },

  filterType: {
    textFilter: {
      startWith: "Bắt đầu với",
      endWith: "Kết thúc với",
      contain: "Chứa",
      notContain: "Không chứa",
      equals: "Bằng",
      notEquals: "Không bằng (Khác)",
    },
    comparisionFilter: {
      greater: "Lớn hơn ( > )",
      less: "Nhỏ hơn ( < )",
      greaterOrEquals: "Lớn hơn hoặc bằng ( ≥ )",
      lessOrEquals: "Nhỏ hơn hoặc bằng ( ≤ )",
      equals: "Bằng ( = )",
      notEquals: "Không bằng ( ≠ )",
    },
    selectionFilter: {
      equals: "Bằng",
      notEquals: "Không bằng (Khác)",
    },
  },

  combobox: {
    selectAll: "Tất cả giá trị",
  },

  filterPopup: {
    tooltipForFilterIcon: "Lọc dữ liệu",
    filterPopupHeading: "Điều kiện lọc",
    filterCancelButton: "Bỏ lọc",
    filterApplyButton: "Áp dụng",
    inputValue: "Nhập giá trị",
  },

  filterPopupErrorMessage: {
    accessFilterWithoutValue: "Vui lòng nhập đầy đủ các giá trị thực hiện lọc.",
    removeAFilterColumnWasNotAccessed:
      "Bạn chưa thực hiện lọc trường này nên không thể bỏ lọc.",
  },

  tableFunctions: {
    title: "Chức năng",
    edit: "Sửa",
    duplicate: "Nhân bản",
    delete: "Xóa",
    resetPassword: "Đặt lại mật khẩu",
  },

  formText: {
    loginForm: {
      username: {
        title: "Số điện thoại / email",
        tooltip: "Số điện thoại hoặc email đăng nhập",
      },
      password: {
        title: "Mật khẩu",
        tooltip: "Mật khẩu đăng nhập",
      },
    },
    changePasswordForm: {
      oldPassword: {
        title: "Mật khẩu hiện tại",
        tooltip: "Mật khẩu hiện tại của bạn",
      },
      newPassword: {
        title: "Mật khẩu mới",
        tooltip: "Mật khẩu bạn muốn thay đổi",
      },
      repeatedNewPassword: {
        title: "Xác nhận mật khẩu mới",
        tooltip: "Nhập lại mật khẩu mới của bạn",
      },
    },
    userForm: {
      addFormTitle: "Thêm mới người dùng",
      editFormTitle: "Sửa thông tin người dùng",
      fullName: {
        title: "Họ và tên",
        tooltip: "Họ và tên",
      },
      phoneNumber: {
        title: "Số điện thoại",
        tooltip: "Số điện thoại người dùng",
      },
      email: {
        title: "Email",
        tooltip: "Email người dùng",
      },
      role: {
        title: "Quyền tài khoản",
        tooltip: "Quyền tài khoản người dùng",
      },
      password: {
        title: "Mật khẩu",
        tooltip: "Mật khẩu người dùng",
      },
      repeatedPassword: {
        title: "Nhập lại mật khẩu",
        tooltip: "Mật khẩu nhập lại của người dùng",
      },
    },

    employeeForm: {
      addFormTitle: "Thêm mới nhân viên",
      editFormTitle: "Sửa thông tin nhân viên",
      isCustomer: "Là khách hàng",
      isProvider: "Là nhà cung cấp",
      employeeCode: {
        title: "Mã",
        tooltip: "Mã nhân viên",
      },
      employeeName: {
        title: "Tên",
        tooltip: "Tên nhân viên",
      },
      dateOfBirth: {
        title: "Ngày sinh",
        tooltip: "Ngày sinh nhân viên",
      },
      gender: {
        genderTitle: "Giới tính",
        male: "Nam",
        female: "Nữ",
        other: "Khác",
      },
      departmentId: {
        title: "Đơn vị",
        tooltip: "Phòng ban nhân viên",
      },
      positionName: {
        title: "Chức danh",
        tooltip: "Chức danh nhân viên",
      },
      identityNumber: {
        title: "Số CCCD",
        tooltip: "Số căn cước công dân",
      },
      identityIssuedDate: {
        title: "Ngày cấp",
        tooltip: "Ngày cấp căn cước công dân",
      },
      identityIssuedPlace: {
        title: "Nơi cấp",
        tooltip: "Nơi cấp căn cước công dân",
      },
      address: {
        title: "Địa chỉ",
        tooltip: "Địa chỉ nhân viên",
      },
      phoneNumber: {
        title: "ĐT di động",
        tooltip: "Điện thoại di động",
      },
      landlineNumber: {
        title: "ĐT cố định",
        tooltip: "Điện thoại cố định",
      },
      email: {
        title: "Email",
        tooltip: "Email nhân viên",
      },
      bankNumber: {
        title: "Tài khoản nhân hàng",
        tooltip: "Số tài khoản ngân hàng",
      },
      bankName: {
        title: "Tên ngân hàng",
        tooltip: "Tên ngân hàng",
      },
      bankBranch: {
        title: "Chi nhánh",
        tooltip: "Chi nhánh ngân hàng",
      },
    },
  },

  dialogText: {
    heading: {
      danger: "Cảnh báo",
      warning: "Thông báo",
      confirm: "Xác nhận",
    },
  },

  toastTitle: {
    success: "Thành công !",
    warning: "Cảnh báo !",
    infor: "Thông tin !",
    error: "Lỗi !",
  },

  dataFormat: {
    gender: {
      male: "Nam",
      female: "Nữ",
      another: "Khác",
    },
  },
};

export default MisaResources;
