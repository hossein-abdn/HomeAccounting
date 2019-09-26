using HomeAccounting.DataAccess.Models;
using Infra.Wpf.Mvvm;
using System.Collections.Generic;

namespace HomeAccounting.UI.ViewModels
{
    public class TransactionGroupNode : ViewModelBase
    {
        public TransactionGroup Item
        {
            get { return Get<TransactionGroup>(); }
            set { Set(value); }
        }

        public List<TransactionGroupNode> Members
        {
            get { return Get<List<TransactionGroupNode>>(); }
            set { Set(value); }
        }

        public bool IsExpanded
        {
            get { return Get<bool>(); }
            set { Set(value); }
        }
    }
}
