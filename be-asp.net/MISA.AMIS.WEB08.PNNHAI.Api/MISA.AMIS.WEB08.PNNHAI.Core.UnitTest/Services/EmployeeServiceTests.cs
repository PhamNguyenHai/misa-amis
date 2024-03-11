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
        public async Task CreateAsync_ValidInput_ReturnIdNotEmpty()
        {
            // Arrange
             var employeeCreateDto = new EmployeeCreateDto();

            var employee = new Employee();

            _mockMapper.Map<Employee>(employeeCreateDto).Returns(employee);

            // Act
            await _mockEmployeeService.CreateAsync(employeeCreateDto);

            // Assert=
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

        [Test]
        public async Task UpdateAsync_ExistedId_ReturnEmployeeIdSameInputId()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var employeeUpdateDto = new EmployeeUpdateDto();

            var employee = new Employee();

            _mockMapper.Map<Employee>(employeeUpdateDto).Returns(employee);

            // Act
            await _mockEmployeeService.UpdateAsync(employeeId, employeeUpdateDto);

            // Assert
            Assert.That(employee.EmployeeId, Is.EqualTo(employeeId));
        }

        [Test]
        public async Task UpdateAsync_ValidInput_Success()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var employeeUpdateDto = new EmployeeUpdateDto();
            var employee = new Employee();

            _mockMapper.Map<Employee>(employeeUpdateDto).Returns(employee);

            // Act
            await _mockEmployeeService.UpdateAsync(employeeId, employeeUpdateDto);

            // Assert
            await _mockEmployeeRepository.Received(1).GetByIdAsync(employeeId);
            await _mockEmployeeService.Received(1).ValidateForUpdating(employeeId, employeeUpdateDto);
            await _mockEmployeeRepository.Received(1).UpdateAsync(employee);
        }

        [Test]
        public async Task DeleteAsync_ValidInput_Success()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var employeeModel = new EmployeeModel();
            var employee = new Employee();

            _mockMapper.Map<Employee>(employeeModel).Returns(employee);

            _mockEmployeeRepository.GetByIdAsync(employeeId).Returns(employeeModel);

            // Act
            await _mockEmployeeService.DeleteAsync(employeeId);

            // Assert
            await _mockEmployeeRepository.Received(1).GetByIdAsync(employeeId);
            await _mockEmployeeRepository.Received(1).DeleteAsync(employee);
        }

        [Test]
        public async Task DeleteMultipalAsync_EmptyListIds_ThrowException()
        {
            // Arrange
            var ids = new List<Guid>();
            var expectedMessage = Core.Resources.AppResource.DeleteIdsEmptyError;

            // Act & Assert
            var exception = Assert.ThrowsAsync<ValidateException>(async () => await _mockEmployeeService.DeleteMultipalAsync(ids));

            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public async Task DeleteMultipalAsync_ValidListIds_Success()
        {
            // Arrange
            var ids = new List<Guid>();
            var employeeToDelete = new List<Employee>();
            for (int i = 1; i<= 10; i++)
            {
                var employeeId = Guid.NewGuid();
                ids.Add(employeeId);
                employeeToDelete.Add(new Employee() { EmployeeId = employeeId });
            }

            _mockEmployeeService.ValidateAndMapDeleteIdsToDeleteEntities(ids).Returns(employeeToDelete);

            // Act
            await _mockEmployeeService.DeleteMultipalAsync(ids);

            // Assert
            await _mockEmployeeService.Received(1).ValidateAndMapDeleteIdsToDeleteEntities(ids);
            await _mockEmployeeRepository.Received(1).DeleteMultipalAsync(employeeToDelete);
        }

        [Test]
        public async Task ValidateAndMapDeleteIdsToDeleteEntities_MoreThan20Ids_ThrowException()
        {
            // Arrange
            var ids = Enumerable.Range(1, 25).Select(x => Guid.NewGuid()).ToList();
            var expectedMessage = String.Format(Core.Resources.AppResource.MoreRecordDeleteError, 20);
            // Act and Assert
            var exception = Assert.ThrowsAsync<ValidateException>(
                async () => await _mockEmployeeService.ValidateAndMapDeleteIdsToDeleteEntities(ids));

            // Assert
            Assert.AreEqual(expectedMessage, exception.Message);
        }
    }
}
