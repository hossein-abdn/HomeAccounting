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
    [ViewType(typeof(LabelListView))]
    public class LabelListVM : ViewModelBase<Label>
    {
        public LabelListVM()
        {
            ViewTitle = "لیست برچسب ها";
        }
    }
}
