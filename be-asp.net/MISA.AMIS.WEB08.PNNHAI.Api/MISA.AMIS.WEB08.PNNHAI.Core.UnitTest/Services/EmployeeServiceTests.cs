using AutoMapper;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core.UnitTest
{
    [TestFixture]
    public class EmployeeServiceTests
    {
        private IEmployeeRepository _mockEmployeeRepository;
        private IDepartmentRepository _mockDepartmentRepository;
        private IMapper _mockMapper;
        private EmployeeService _mockEmployeeService;
        [SetUp]
        public void Setup()
        {
            _mockEmployeeRepository = Substitute.For<IEmployeeRepository>();
            _mockDepartmentRepository = Substitute.For<IDepartmentRepository>();
            _mockMapper = Substitute.For<IMapper>();
            _mockEmployeeService = Substitute.For<EmployeeService>(_mockEmployeeRepository, _mockDepartmentRepository, _mockMapper);
        }

        [Test]
        public async Task CheckEmployeeCodeNotExistedByCode_EmployeeIdExisted_ThrowException()
        {
            // Arrange
            var employeeCode = "NV-00001";
            var employee = new Employee()
            {
                EmployeeCode = employeeCode
            };

            var expectedMessage = string.Format(Core.Resources.AppResource.ExistedEmployeeCode, employeeCode);

            _mockEmployeeRepository.FindByCodeAsync(employeeCode).Returns(employee);

            // Act
            var exception = Assert.ThrowsAsync<ValidateException>(
                async () => await _mockEmployeeService.CheckEmployeeCodeNotExistedByCode(employeeCode));

            // Assert
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
            await _mockEmployeeRepository.Received(1).FindByCodeAsync(employeeCode);
        }

        [Test]
        public async Task CheckEmployeeCodeNotExistedByCode_EmployeeIdNotExisted_Success()
        {
            // Arrange
            string employeeCode = "NV-KhongTonTai";
            _mockEmployeeRepository.FindByCodeAsync(employeeCode).ReturnsNull();

            // Act
            await _mockEmployeeService.CheckEmployeeCodeNotExistedByCode(employeeCode);

            // Assert
            await _mockEmployeeRepository.Received(1).FindByCodeAsync(employeeCode);
        }

        [Test]
        public async Task CheckDepartmentExisted_DepartmentIdNotExisted_ThrowException()
        {
            // Arrange
            var departmentId = Guid.NewGuid();
            _mockDepartmentRepository.FindByIdAsync(departmentId).ReturnsNull();

            var expectedMessage = Core.Resources.AppResource.DepartmentNotExistedError;

            // Act
            var exception = Assert.ThrowsAsync<ValidateException>(
                async () => await _mockEmployeeService.CheckDepartmentExisted(departmentId));

            // Assert
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
            await _mockDepartmentRepository.Received(1).FindByIdAsync(departmentId);
        }

        [Test]
        public async Task CheckDepartmentExisted_DepartmentIdExisted_Success()
        {
            // Arrange
            var departmentId = Guid.NewGuid();
            var department = new DepartmentModel()
            {
                DepartmentId = departmentId
            };
            _mockDepartmentRepository.FindByIdAsync(departmentId).Returns(department);

            // Act
            await _mockEmployeeService.CheckDepartmentExisted(departmentId);

            // Assert
            await _mockDepartmentRepository.Received(1).FindByIdAsync(departmentId);
        }

        [Test]
        public async Task ValidateForInserting_ExistedEmployeeCodeAndExistedDepartmentId_ThrowExcepton()
        {
            // Arrange
            var employeeCode = "NV-00001";
            var departmentId = Guid.NewGuid();

            var employeeCreateDto = new EmployeeCreateDto();

            var expectedMessage = string.Format(Core.Resources.AppResource.ExistedEmployeeCode, employeeCode);

            _mockEmployeeService.When(a => a.CheckDepartmentExisted(departmentId)).Do(async _ => await Task.CompletedTask);

            _mockEmployeeService.CheckEmployeeCodeNotExistedByCode(employeeCode)
                .ThrowsAsync(new ValidateException(expectedMessage));

            // Act
            var exception = Assert.ThrowsAsync<ValidateException>(
                async () => await _mockEmployeeService.ValidateForInserting(employeeCreateDto));

            // Assert
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
            await _mockEmployeeService.Received(1).CheckEmployeeCodeNotExistedByCode(employeeCode);
            await _mockEmployeeService.Received(1).CheckDepartmentExisted(departmentId);
        }

        [Test]
        public async Task ValidateForInserting_ExistedEmployeeCodeAndNotExistedDepartmentId_ThrowExcepton()
        {
            // Arrange
            var employeeCreateDto = new EmployeeCreateDto();
            var employeeCode = "NV-00001";

            var employee = new Employee()
            {
                EmployeeCode = employeeCode
            };

            var departmentId = Guid.NewGuid();

            var expectedMessage = Core.Resources.AppResource.DepartmentNotExistedError;

            _mockEmployeeService.CheckDepartmentExisted(departmentId)
                .ThrowsAsync(new ValidateException(expectedMessage));


            // Act
            var exception = Assert.ThrowsAsync<ValidateException>(
                async () => await _mockEmployeeService.ValidateForInserting(employeeCreateDto));

            // Assert
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
            await _mockEmployeeService.Received(1).CheckEmployeeCodeNotExistedByCode(employeeCode);
            await _mockEmployeeService.Received(1).CheckDepartmentExisted(departmentId);
        }

        //[Test]
        //public async Task ValidateForInserting_NotExistedEmployeeCodeAndExistedDepartmentId_Success()
        //{

        //}

        //[Test]
        //public async Task ValidateForInserting_NotExistedEmployeeCodeAndNotExistedDepartmentId_ThrowException()
        //{

        //}


        [Test]
        public async Task CreateAsync_ValidInput_ReturnIdNotEmpty()
        {
            // Arrange
             var employeeCreateDto = new EmployeeCreateDto();

            var employee = new Employee();

            //_mockEmployeeService.When(x => x.ValidateForInserting(employeeCreateDto))
            //    .Do(async _ => await Task.CompletedTask);

            _mockMapper.Map<Employee>(employeeCreateDto).Returns(employee);

            // Act
            await _mockEmployeeService.CreateAsync(employeeCreateDto);

            // Assert
            Assert.That(employee.EmployeeId, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public async Task CreateAsync_ValidInput_Success()
        {
            // Arrange
            var employeeCreateDto = new EmployeeCreateDto();

            var employee = new Employee()
            {
                EmployeeId = Guid.NewGuid(),
            };

            _mockMapper.Map<Employee>(employeeCreateDto).Returns(employee);

            // Act
            await _mockEmployeeService.CreateAsync(employeeCreateDto);

            // Assert
            await _mockEmployeeService.Received(1).ValidateForInserting(employeeCreateDto);
            await _mockEmployeeRepository.Received(1).InsertAsync(employee);
        }
    }
}
