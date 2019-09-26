﻿using HomeAccounting.DataAccess.Enums;
using Infra.Wpf.Mvvm;
using System.Collections.Generic;

namespace HomeAccounting.UI.ViewModels
{
    public class TransactionGroupTypeNode : ViewModelBase
    {
        public string Title
        {
            get { return Type == TransactionGroupType.Income ? "درآمد" : "هزینه"; }
        }

        public List<TransactionGroupNode> Members
        {
            get { return Get<List<TransactionGroupNode>>(); }
            set { Set(value); }
        }

        public TransactionGroupType Type
        {
            get { return Get<TransactionGroupType>(); }
            set { Set(value); }
        }

        public bool IsExpanded
        {
            get { return Get<bool>(); }
            set { Set(value); }
        }
    }
}
