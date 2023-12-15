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
    public class PriorityTests
    {
        [Test]
        [TestCase(10, true)]
        [TestCase(20, true)]
        [TestCase(30, true)]
        [TestCase(31, false)]
        [TestCase(41, false)]
        [TestCase(51, false)]
        public void TestNameLengthIsValid(int length, bool expectedLength)
        {
            GenerateRandomWord.GenerateRandomString(length);

            var priority = new Priority(GenerateRandomWord.Word, "color");

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(priority, new ValidationContext(priority), validationResults, true);

            Assert.AreEqual(expectedLength, isValid);
        }

        [Test]
        [TestCase(10, true)]
        [TestCase(20, true)]
        [TestCase(25, true)]
        [TestCase(31, false)]
        [TestCase(41, false)]
        [TestCase(51, false)]
        public void TestColorLengthIsValid(int length, bool expectedLength)
        {
            GenerateRandomWord.GenerateRandomString(length);

            var priority = new Priority("Name" ,GenerateRandomWord.Word);

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(priority, new ValidationContext(priority), validationResults, true);

            Assert.AreEqual(expectedLength, isValid);
        }
    }
}
