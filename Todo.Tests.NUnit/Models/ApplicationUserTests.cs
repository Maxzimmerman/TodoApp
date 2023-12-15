using System.ComponentModel.DataAnnotations;
using Todo.Models;
using TodoApp.Tests.NUnit.Helper;

namespace TodoApp.Tests.NUnit.Models
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase(10, true)]
        [TestCase(60, true)]
        [TestCase(100, true)]
        [TestCase(101, false)]
        [TestCase(200, false)]
        [TestCase(500, false)]
        public void TestNameLenghtisValid(int length, bool expectedOutput)
        {
            GenerateRandomWord.GenerateRandomString(length);

            var user = new ApplicationUser(GenerateRandomWord.Word);

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(user, new ValidationContext(user), validationResults, true);

            Assert.AreEqual(expectedOutput, isValid);
        }
    }
}