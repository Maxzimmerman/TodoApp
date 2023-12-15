using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Todo.Models;
using TodoApp.Tests.NUnit.Helper;

namespace TodoApp.Tests.NUnit.Models
{
    [TestFixture]
    public class UserProjectTests
    {
        [Test]
        [TestCase(10, true)]
        [TestCase(50, true)]
        [TestCase(100, true)]
        [TestCase(101, false)]
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
                Title = GenerateRandomWord.Word,
                User = user,
                ApplicationUserId = "1",
                IsDeleted = false,
                IsLiked = false,
            };

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(project, new ValidationContext(project), validationResults, true);

            Assert.AreEqual(expectedOutput, isValid);
        }

        [Test]
        [TestCase(false, true)]
        [TestCase(false, true)]
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(true, false)]
        [TestCase(true, false)]
        public void TestIsLikedDefaultValue(bool defaultValue, bool expectedOutput)
        {
            bool isValid;
            var project = new UserProject(){};

            if(project.IsLiked == defaultValue)
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
        public void TestIsDeletedDefaultValue(bool defaultValue, bool expectedOutput)
        {
            bool isValid;
            var project = new UserProject() { };

            if (project.IsLiked == defaultValue)
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
