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
    public class CategoryTests
    {
        [Test]
        [TestCase(5, true)]
        [TestCase(10, true)]
        [TestCase(15, true)]
        [TestCase(16, false)]
        [TestCase(26, false)]
        [TestCase(36, false)]
        public void TestNameLengthIsValid(int length, bool expectedLength)
        {
            GenerateRandomWord.GenerateRandomString(length);

            var category = new Category(GenerateRandomWord.Word, 4);

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(category, new ValidationContext(category), validationResults, true);

            Assert.AreEqual(expectedLength, isValid);
        }

        [Test]
        [TestCase(10, true)]
        [TestCase(20, true)]
        [TestCase(365, true)]
        [TestCase(366, false)]
        [TestCase(500, false)]
        [TestCase(390, false)]
        public void TestColorLengthIsValid(int length, bool expectedLength)
        {
            var category = new Category("Name", length);

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(category, new ValidationContext(category), validationResults, true);

            Assert.AreEqual(expectedLength, isValid);
        }
    }
}
