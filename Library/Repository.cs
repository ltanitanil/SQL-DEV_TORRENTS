using EFCore.BulkExtensions;
using Library.Concrete;
using Library.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public class Repository : IDisposable
    {
        private EFDbContext dbContext;

        public Repository()
        {
            dbContext = new EFDbContext();
        }

        public void BulkInsert<T>(IList<T> items, int packageSize = 1000)
            where T : class, IAggregateRoot
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                dbContext.BulkInsert(items, new BulkConfig { BatchSize = packageSize });
                transaction.Commit();
            }
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
