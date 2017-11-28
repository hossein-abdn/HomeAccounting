using HomeAccounting.DataAccess.Models;
using HomeAccounting.UI.Views.BaseInfo;
using Infra.Wpf.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.UI.ViewModels.BaseInfo
{
    [ViewType(typeof(TransactionGroupListView))]
    public class TransactionGroupListVM : ViewModelBase<Person>
    {
        public TransactionGroupListVM()
        {
            ViewTitle = "لیست گروه های هزینه و درآمد";
        }
    }
}
