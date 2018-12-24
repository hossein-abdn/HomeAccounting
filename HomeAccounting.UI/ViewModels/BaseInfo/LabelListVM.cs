using HomeAccounting.Business;
using HomeAccounting.Business.BaseInfo;
using HomeAccounting.DataAccess.Models;
using HomeAccounting.UI.Views.BaseInfo;
using Infra.Wpf.Business;
using Infra.Wpf.Common.Helpers;
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
        public RelayCommand<string> GetAllCommand { get; set; }

        public RelayCommand<Label> CreateEditCommand { get; set; }

        public RelayCommand<Label> ChangeStatusCommand { get; set; }

        public RelayCommand LoadedEventCommand { get; set; }

        public string SearchPhrase
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        private AccountingUow accountingUow { get; set; }
        
        public LabelListVM()
        {
            ViewTitle = "لیست برچسب ها";

            GetAllCommand = new RelayCommand<string>(GetAllExecute);
            CreateEditCommand = new RelayCommand<Label>(CreateEditExecute);
            ChangeStatusCommand = new RelayCommand<Label>(ChangeStatusExecute);
            LoadedEventCommand = new RelayCommand(LoadedEventExecute);

            accountingUow = new AccountingUow();
        }

        private void LoadedEventExecute()
        {
            GetAllExecute(SearchPhrase);
            Messenger.Default.Send("id", "LabelListView_SetScrollView");
        }

        private void ChangeStatusExecute(Label model)
        {
            if(ShowMessageBox("آیا از حذف برچسب مطئمن هستید؟","حذف",Infra.Wpf.Controls.MsgButton.YesNo,Infra.Wpf.Controls.MsgIcon.Question,Infra.Wpf.Controls.MsgResult.No)==Infra.Wpf.Controls.MsgResult.Yes)
            {
                Messenger.Default.Send(model, "LabelListView_SaveItemIndex");

                var result = ((LabelRepository)accountingUow.LabelRepository).ChangeStatus(model);
                if(result.HasException == false)
                {
                    BusinessResult<int> saveResult = accountingUow.SaveChange();
                    if (saveResult.HasException)
                        result.Message = saveResult.Message;
                    GetAllExecute(SearchPhrase);
                    Messenger.Default.Send("index", "LabelListView_SetScrollView");
                }

                Billboard.ShowMessage(result.Message.MessageType, result.Message.Message);
            }
        }

        private void CreateEditExecute(Label model)
        {
            NavigationService.NavigateTo(new LabelCreateVM(accountingUow, model?.Copy()));
        }

        private void GetAllExecute(string predicate)
        {
            var result = accountingUow.LabelRepository.GetAll(predicate: predicate);
            if (result.HasException == false)
                ItemsSource = new System.Collections.ObjectModel.ObservableCollection<Label>(result.Data);
            else
                Billboard.ShowMessage(result.Message.MessageType, result.Message.Message);
        }
    }
}
