using System;
using System.Linq;
using System.Collections.Generic;
using HomeAccounting.DataAccess.Enums;
using HomeAccounting.DataAccess.Models;
using Infra.Wpf.Common;
using Infra.Wpf.Business;
using Infra.Wpf.Repository;

namespace HomeAccounting.Business
{
    public class TransactionGroupBusinessSet
    {
        private ILogger logger;

        private IRepository<TransactionGroup> repository;

        public TransactionGroupBusinessSet(IRepository<TransactionGroup> repository,ILogger logger = null)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public BusinessResult<List<TransactionGroup>> GetAll(string predicate, object[] values = null)
        {
            BusinessBase<List<TransactionGroup>> getAll = new BusinessBase<List<TransactionGroup>>(logger);

            getAll.Execute(() =>
            {
                if (values == null)
                    values = new object[] { RecordStatus.Exist };
                else
                {
                    Array.Resize(ref values, 2);
                    values[1] = values[0];
                    values[0] = RecordStatus.Exist;
                }

                if (string.IsNullOrEmpty(predicate))
                    predicate = "RecordStatusId == @0";
                else
                    predicate = predicate + " AND RecordStatusId == @0";

                string orderBy = "OrderItem";

                var searchResult = repository.GetAll(predicate, values);
                var parentIds = searchResult.Select(x => x.ParentPath?.Split(',')).Where(x => x != null).SelectMany(x => x).Select(int.Parse).Union(searchResult.Select(x => x.TransactionGroupId));

                getAll.Result.Data = repository.GetAll(x => parentIds.Contains(x.TransactionGroupId), orderBy);
                return true;
            });

            return getAll.Result;
        }
    }
}
