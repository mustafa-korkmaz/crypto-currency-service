using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyMarket.Common;
using MoneyMarket.DataAccess;
using MoneyMarket.DataAccess.Models;
using Z.EntityFramework.Plus;

namespace MoneyMarket.UnitTests
{
    [TestClass]
    public class BatchUpdateTest
    {
        [TestMethod]
        public void EditAllStatus()
        {
            var userProductIds = new List<int> { 2, 5 };

            var uow = new UnitOfWork();

            //var query = uow.Repository<UserProduct>().AsQueryable().Where(up => userProductIds.Contains(up.ProductId))
            //    .Update(up => new UserProduct() { Name = "test1-2-3-4", Status = Status.Tracking });
            
            uow.Save();
        }

    }
}
