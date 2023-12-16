using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Models;
using TodoApp.Tests.NUnit.Helper;

namespace TodoApp.Tests.NUnit.Models
{
    [TestFixture]
    public class TodoEntryTests
    {
        [Test]
        [TestCase(5, true)]
        [TestCase(15, true)]
        [TestCase(20, true)]
        [TestCase(21, false)]
        [TestCase(201, false)]
        [TestCase(301, false)]
        public void TestNameLenghtIsValid(int length, bool expectedOutput)
        {
            GenerateRandomWord.GenerateRandomString(length);

            var user = new ApplicationUser()
            {
                Id = "1",
                UserName = "Test",
                NormalizedUserName = "Test",
                Email = "Test",
                NormalizedEmail = "Test",
                EmailConfirmed = true,
                PasswordHash = "Test",
                SecurityStamp = "Test",
                ConcurrencyStamp = "Test",
                PhoneNumber = "Test",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = true,
                LockoutEnd = null,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                ApplicationUserName = "Test",
            };

            var project = new UserProject()
            {
                Id = 3,
                Title = GenerateRandomWord.Word,
                User = user,
                ApplicationUserId = "1",
                IsDeleted = false,
                IsLiked = false,
            };

            var category = new Category()
            {
                Id = 1,
                Name = "Test",
                Counter = 1,
            };

            var prioriy = new Priority()
            {
                Id = 2,
                Name = "Test",
                Color = "test",
            };

            var todo = new TodoEntry()
            {
                Id = 3,
                Title = GenerateRandomWord.Word,
                Description = "Test",
                EndDate = DateTime.Now,
                IChecked = true,
                IDeleted = false,
                DateOfCreation = DateTime.Now,
                ChecktedDate = DateTime.Now,
                Category = category,
                CategoryId = category.Id,
                Priority = prioriy,
                PriorityId = prioriy.Id,
                ApplicationUserId = user.Id,
                User = user,
                Project = project,
                ProjectId = project.Id,
            };

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(todo, new ValidationContext(todo), validationResults, true);

            Assert.AreEqual(expectedOutput, isValid);
        }

        [Test]
        [TestCase(30, true)]
        [TestCase(60, true)]
        [TestCase(100, true)]
        [TestCase(101, false)]
        [TestCase(201, false)]
        [TestCase(301, false)]
        public void TestDescriptionLenghtIsValid(int length, bool expectedOutput)
        {
            GenerateRandomWord.GenerateRandomString(length);

            var user = new ApplicationUser()
            {
                Id = "1",
                UserName = "Test",
                NormalizedUserName = "Test",
                Email = "Test",
                NormalizedEmail = "Test",
                EmailConfirmed = true,
                PasswordHash = "Test",
                SecurityStamp = "Test",
                ConcurrencyStamp = "Test",
                PhoneNumber = "Test",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = true,
                LockoutEnd = null,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                ApplicationUserName = "Test",
            };

            var project = new UserProject()
            {
                Id = 3,
                Title = GenerateRandomWord.Word,
                User = user,
                ApplicationUserId = "1",
                IsDeleted = false,
                IsLiked = false,
            };

            var category = new Category()
            {
                Id = 1,
                Name = "Test",
                Counter = 1,
            };

            var prioriy = new Priority()
            {
                Id = 2,
                Name = "Test",
                Color = "test",
            };

            var todo = new TodoEntry()
            {
                Id = 3,
                Title = "test",
                Description = GenerateRandomWord.Word,
                EndDate = DateTime.Now,
                IChecked = true,
                IDeleted = false,
                DateOfCreation = DateTime.Now,
                ChecktedDate = DateTime.Now,
                Category = category,
                CategoryId = category.Id,
                Priority = prioriy,
                PriorityId = prioriy.Id,
                ApplicationUserId = user.Id,
                User = user,
                Project = project,
                ProjectId = project.Id,
            };

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(todo, new ValidationContext(todo), validationResults, true);

            Assert.AreEqual(expectedOutput, isValid);
        }

        [Test]
        [TestCase(false, true)]
        [TestCase(false, true)]
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(true, false)]
        [TestCase(true, false)]
        public void TestICheckedDefaultValue(bool defaultValue, bool expectedOutput)
        {
            bool isValid;
            var todo = new TodoEntry() { };

            if (todo.IChecked == defaultValue)
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }

            Assert.AreEqual(expectedOutput, isValid);
        }

        [Test]
        [TestCase(false, true)]
        [TestCase(false, true)]
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(true, false)]
        [TestCase(true, false)]
        public void TestIDeletedDefaultValue(bool defaultValue, bool expectedOutput)
        {
            bool isValid;
            var todo = new TodoEntry() { };

            if (todo.IDeleted == defaultValue)
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }

            Assert.AreEqual(expectedOutput, isValid);
        }
    }
}
