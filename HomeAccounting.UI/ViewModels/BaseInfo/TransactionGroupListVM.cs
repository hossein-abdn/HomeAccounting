using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Infra.Wpf.Mvvm;
using Infra.Wpf.Security;
using HomeAccounting.Business;
using HomeAccounting.UI.Views;
using HomeAccounting.DataAccess.Enums;
using HomeAccounting.DataAccess.Models;

namespace HomeAccounting.UI.ViewModels
{
    [ViewType(typeof(TransactionGroupListView))]
    public class TransactionGroupListVM : ViewModelBase<TransactionGroupTypeNode>
    {
        public RelayCommand<List<KeyValuePair<string, string>>> GetAllCommand { get; set; }

        public RelayCommand LoadedEventCommand { get; set; }

        public RelayCommand<TransactionGroup> AddCommand { get; set; }

        public RelayCommand<TransactionGroup> EditCommand { get; set; }

        public RelayCommand<TransactionGroup> DeleteCommand { get; set; }

        public RelayCommand<TransactionGroup> UpCommand { get; set; }

        public RelayCommand<TransactionGroup> DownCommand { get; set; }

        public List<KeyValuePair<string, string>> SearchPhraseList
        {
            get { return Get<List<KeyValuePair<string, string>>>(); }
            set { Set(value); }
        }

        private AccountingUow accountingUow { get; set; }

        private TransactionGroupBusinessSet businessSet { get; set; }

        public TransactionGroupListVM()
        {
            ViewTitle = "لیست گروه های هزینه و درآمد";

            GetAllCommand = new RelayCommand<List<KeyValuePair<string, string>>>(GetAllExecute);
            LoadedEventCommand = new RelayCommand(LoadedEventExecute);
            AddCommand = new RelayCommand<TransactionGroup>(AddExecute);
            EditCommand = new RelayCommand<TransactionGroup>(EditExecute);
            DeleteCommand = new RelayCommand<TransactionGroup>(DeleteExecute);
            UpCommand = new RelayCommand<TransactionGroup>(UpExecute);
            DownCommand = new RelayCommand<TransactionGroup>(DownExecute);

            accountingUow = new AccountingUow();
            businessSet = new TransactionGroupBusinessSet(accountingUow.TransactionGroupRepository, accountingUow.Logger);
        }

        private void AddExecute(TransactionGroup obj)
        {

        }

        private void EditExecute(TransactionGroup obj)
        {

        }

        private void DownExecute(TransactionGroup item)
        {

        }

        private void UpExecute(TransactionGroup item)
        {

        }

        private void DeleteExecute(TransactionGroup item)
        {

        }

        private void LoadedEventExecute()
        {
            GetAllExecute(SearchPhraseList);
        }

        private void GetAllExecute(List<KeyValuePair<string, string>> filterList)
        {
            try
            {
                var predicate = GeneratePredicate(filterList);
                object[] values = null;

                var typeId = filterList?.FirstOrDefault(x => x.Key == "TypeId");
                if (typeId != null && typeId.HasValue)
                {
                    var typeIdString = typeId.Value.Value;
                    if (!string.IsNullOrEmpty(typeIdString))
                    {
                        var typeIdValue = Enum.Parse(typeof(TransactionGroupType), typeIdString.Substring(typeIdString.IndexOf("=") + 2));
                        values = new object[] { typeIdValue };
                    }
                }

                var transactoinGroupListResult = businessSet.GetAll(predicate, values);

                if (transactoinGroupListResult.HasException == false)
                {
                    var costNode = new TransactionGroupTypeNode()
                    {
                        Type = TransactionGroupType.Cost,
                        Members = FillTransactionGroupList(transactoinGroupListResult.Data, null, TransactionGroupType.Cost, predicate.Contains("Title")),
                        IsExpanded = true
                    };
                    var IncomeNode = new TransactionGroupTypeNode()
                    {
                        Type = TransactionGroupType.Income,
                        Members = FillTransactionGroupList(transactoinGroupListResult.Data, null, TransactionGroupType.Income, predicate.Contains("Title")),
                        IsExpanded = true
                    };
                    ItemsSource = new System.Collections.ObjectModel.ObservableCollection<TransactionGroupTypeNode>() { costNode, IncomeNode };
                }
                else
                    Billboard.ShowMessage(transactoinGroupListResult.Message.MessageType, transactoinGroupListResult.Message.Message);
            }
            catch (Exception ex)
            {
                Billboard.ShowMessage(Infra.Wpf.Controls.MessageType.Error, "در سامانه خطایی رخ داده است.");
                accountingUow?.Logger.Log(ex, "TransactionGroupListVM.GetAllExecute", (Thread.CurrentPrincipal.Identity as Identity).Id);
            }
        }

        private List<TransactionGroupNode> FillTransactionGroupList(List<TransactionGroup> list, int? parentId, TransactionGroupType type, bool isExpanded)
        {
            List<TransactionGroupNode> result = null;
            foreach (var item in list.Where(x => x.TypeId == type && x.ParentId == parentId))
            {
                if (result == null)
                    result = new List<TransactionGroupNode>();
                result.Add(new TransactionGroupNode
                {
                    Item = item,
                    Members = FillTransactionGroupList(list, item.TransactionGroupId, type, isExpanded),
                    IsExpanded = isExpanded
                });
            }

            return result;
        }

        private string GeneratePredicate(List<KeyValuePair<string, string>> filterList)
        {
            string result = "";
            if (filterList == null)
                return result;

            foreach (var item in filterList)
            {
                if (!string.IsNullOrWhiteSpace(item.Value))
                {
                    if (!string.IsNullOrEmpty(result))
                        result += " AND ";

                    if (item.Key == "TypeId")
                        result += "TypeId == @1";
                    else
                        result += item.Value;
                }
            }

            return result;
        }
    }
}