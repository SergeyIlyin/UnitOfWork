using Host.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.EntityFrameworkCore
{
    public class IQueryablePageListExtensionsTests
    {
        [Fact]
        public async Task ToPagedListAsyncTest()
        {
            using (var db = new CustomerContext())
            {
                var testItems = TestItems();
                await db.AddRangeAsync(testItems);
                db.SaveChanges();

                var items = db.Customers.Where(t => t.Age > 1);

                var page = await items.ToPagedListAsync(1, 2);
                Assert.NotNull(page);

                Assert.Equal(4, page.TotalCount);
                Assert.Equal(2, page.Items.Count);
                Assert.Equal("E", page.Items[0].Name);

              page = await items.ToPagedListAsync(0, 2);
                Assert.NotNull(page);
                Assert.Equal(4, page.TotalCount);
                Assert.Equal(2, page.Items.Count);
                Assert.Equal("C", page.Items[0].Name);
            }





        }



        public List<Customer> TestItems()
        {
            return new List<Customer>()
            {
                new Customer(){Name="A", Age=1},
                new Customer(){Name="B", Age=1},
                new Customer(){Name="C", Age=2},
                new Customer(){Name="D", Age=3},
                new Customer(){Name="E", Age=4},
                new Customer(){Name="F", Age=5},
            };
        }

        public class Customer
        {
            [Key]
            public string Name { get; set; }
            public int Age { get; set; }
        }
        public class CustomerContext : DbContext
        {
            public DbSet<Customer> Customers { get; set; }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseInMemoryDatabase("test");
            }
        }


    }
}
