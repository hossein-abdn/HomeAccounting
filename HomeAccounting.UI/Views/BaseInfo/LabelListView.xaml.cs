using Infra.Wpf.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HomeAccounting.UI.Views.BaseInfo
{
    public partial class LabelListView : Page
    {
        int _index = 0;
        int _id = 0;

        public LabelListView()
        {
            Messenger.Default.Register<DataAccess.Models.Label>(SaveItemIndex, "LabelListView_SaveItemIndex");
            Messenger.Default.Register<int>(SaveItemId, "LabelListView_SaveItemId");
            Messenger.Default.Register<string>(SetScrollView, "LabelListView_SetScrollView");

            InitializeComponent();

            grid.SelectionMode = C1.WPF.DataGrid.DataGridSelectionMode.MultiRow;
        }

        private void SaveItemId(int id)
        {
            _id = id;
        }

        private void SaveItemIndex(DataAccess.Models.Label item)
        {
            var row = grid.Rows.FirstOrDefault(x => x.DataItem == item);
            if (row != null)
                _index = row.Index;
        }

        private void SetScrollView(string type)
        {
            if(type == "index")
            {
                if (_index == grid.Rows.Count - 1)
                    _index--;
                grid.SelectedIndex = _index;
                grid.ScrollIntoView(_index, 0);

                _index = 0;
            }
            else
            {
                var row = grid.Rows.FirstOrDefault(x => ((DataAccess.Models.Label)x.DataItem)?.LabelId == _id);
                if(row != null)
                {
                    grid.SelectedIndex = row.Index;
                    grid.ScrollIntoView(row.Index, 0);
                }

                _id = 0;
            }
        }
    }
}
