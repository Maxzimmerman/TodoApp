using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.DataAccess.data;
using Todo.Models;

namespace TodoApp.Tests.Model
{
    [TestFixture]
    public class Priorität
    {
        public Priority priority1;
        private Priority priority2;

        public Priorität()
        {
            priority1 = new Priority()
            {
                Id = 1,
                Name = "Test",
                Color = "White",
            };

            priority2 = new Priority()
            {
                Id = 2,
                Name = "Test",
                Color = "White",
            };
        }

        [Test]
        [Order(0)]
        public void Test_Get_All()
        {
            // Arrange
            var expectedList = new List<Priority>() { priority1, priority2 };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TodoApp").Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.Add(priority1);
                context.Add(priority2);
                context.SaveChanges();
            }

            // Act

            List<Priority> actualList;
            using(var context = new ApplicationDbContext(options))
            {
                actualList = context.priorities.ToList();
            }

            // Assert

            CollectionAssert.AreEqual(expectedList, actualList, new PriorityCompare());
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
                context.Add(priority1);
                context.SaveChanges();
            }

            // Assert

            using(var context = new ApplicationDbContext(options))
            {
                var priority = context.priorities.FirstOrDefault(u => u.Id == priority1.Id);
                Assert.AreEqual(priority1.Id, priority.Id);
            }
        }
    }

    public class PriorityCompare : IComparer
    { 
        public int Compare(object? x, object? y)
        {
            var priority1 = (Priority)x;
            var priority2 = (Priority)y;
            if (priority1.Id != priority2.Id)
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
