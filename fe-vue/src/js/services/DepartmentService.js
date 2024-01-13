import BaseService from "./BaseService";

class DepartmentService extends BaseService {
  constructor() {
    super("/departments");
  }
}

const departmentService = new DepartmentService();

export default departmentService;
