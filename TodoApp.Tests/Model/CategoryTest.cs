using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Models;
using Todo.DataAccess.data;
using System.Collections;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Tests.Model
{
    [TestFixture]
    public class CategoryTest
    {
        public Category category1 { get; set; }
        public Category category2 { get; set; }

        public CategoryTest()
        {
            category1 = new Category()
            {
                Id = 1,
                Name = "test",
                Counter = 1,

            };

            category2 = new Category()
            {
                Id= 2,
                Name = "test",
                Counter = 2,
            };
        }

        [Test]
        [Order(0)]
        public void Test_Get_AlL()
        {
            // Arrange
            var expectedList = new List<Category>() { category1, category2 };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TodoApp").Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.Add(category1);
                context.Add(category2);
                context.SaveChanges();
            }

            // Act

            List<Category> actualList;
            using (var context = new ApplicationDbContext(options))
            {
                actualList = context.categories.ToList();
            }

            // Assert

            CollectionAssert.AreEqual(expectedList, actualList, new CategoryCompare());
        }

        [Test]
        [Order(1)]
        public void Test_Add()
        {
            // Arrange

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TodoApp").Options;

            // Act

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Add(category1);
                context.SaveChanges();
            }

            // Assert

            using (var context = new ApplicationDbContext(options))
            {
                var priority = context.categories.FirstOrDefault(u => u.Id == category1.Id);
                Assert.AreEqual(category1.Id, priority.Id);
            }
        }
    }

    public class CategoryCompare : IComparer
    {
        public int Compare(object? x, object? y)
        {
            var category1 = (Category)x;
            var category2 = (Category)y;
            if (category1.Id != category2.Id)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
