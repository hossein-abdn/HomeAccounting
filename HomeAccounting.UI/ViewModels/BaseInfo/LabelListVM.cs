using System.Collections.Generic;
using System.Collections.ObjectModel;
using HomeAccounting.Business;
using HomeAccounting.DataAccess.Models;
using Infra.Wpf.Mvvm;
using Infra.Wpf.Business;
using Infra.Wpf.Common.Helpers;
using HomeAccounting.UI.Views;

namespace HomeAccounting.UI.ViewModels
{
    [ViewType(typeof(LabelListView))]
    public class LabelListVM : ViewModelBase<Label>
    {
        public RelayCommand<List<KeyValuePair<string, string>>> GetAllCommand { get; set; }

        public RelayCommand<Label> CreateEditCommand { get; set; }

        public RelayCommand<Label> ChangeStatusCommand { get; set; }

        public RelayCommand LoadedEventCommand { get; set; }

        public List<KeyValuePair<string, string>> SearchPhraseList
        {
            get { return Get<List<KeyValuePair<string, string>>>(); }
            set { Set(value); }
        }

        private AccountingUow accountingUow { get; set; }

        private LabelBusinessSet businessSet { get; set; }

        public LabelListVM()
        {
            ViewTitle = "لیست برچسب ها";

            GetAllCommand = new RelayCommand<List<KeyValuePair<string, string>>>(GetAllExecute);
            CreateEditCommand = new RelayCommand<Label>(CreateEditExecute);
            ChangeStatusCommand = new RelayCommand<Label>(ChangeStatusExecute);
            LoadedEventCommand = new RelayCommand(LoadedEventExecute);

            accountingUow = new AccountingUow();
            businessSet = new LabelBusinessSet(accountingUow.LabelRepository, accountingUow.Logger);
        }

        private void LoadedEventExecute()
        {
            GetAllExecute(SearchPhraseList);
            Messenger.Default.Send("id", "LabelListView_SetScrollView");
        }

        private void ChangeStatusExecute(Label model)
        {
            if (ShowMessageBox("آیا از حذف برچسب مطئمن هستید؟", "حذف", Infra.Wpf.Controls.MsgButton.YesNo, Infra.Wpf.Controls.MsgIcon.Question, Infra.Wpf.Controls.MsgResult.No) == Infra.Wpf.Controls.MsgResult.Yes)
            {
                Messenger.Default.Send(model, "LabelListView_SaveItemIndex");

                var result = businessSet.Delete(model);
                if (result.HasException == false)
                {
                    BusinessResult<int> saveResult = accountingUow.SaveChange();
                    if (saveResult.HasException)
                        result.Message = saveResult.Message;
                    GetAllExecute(SearchPhraseList);
                    Messenger.Default.Send("index", "LabelListView_SetScrollView");
                }

                Billboard.ShowMessage(result.Message.MessageType, result.Message.Message);
            }
        }

        private void CreateEditExecute(Label model)
        {
            NavigationService.NavigateTo(new LabelCreateVM(accountingUow, model?.Copy()));
        }

        private void GetAllExecute(List<KeyValuePair<string, string>> filterList)
        {
            var predicate = GeneratePredicate(filterList);
            var result = businessSet.GetAll(predicate);
            if (result.HasException == false)
                ItemsSource = new ObservableCollection<Label>(result.Data);
            else
                Billboard.ShowMessage(result.Message.MessageType, result.Message.Message);
        }

        private string GeneratePredicate(List<KeyValuePair<string, string>> filterList)
        {
            string result = "";
            if (filterList != null)
            {
                foreach (var item in filterList)
                {
                    if (!string.IsNullOrWhiteSpace(item.Value))
                    {
                        if (!string.IsNullOrEmpty(result))
                            result += " AND ";
                        result += item.Value;
                    }
                }
            }

            return result;
        }
    }
}
