using HomeAccounting.DataAccess.Models;
using HomeAccounting.UI.Views.BaseInfo;
using Infra.Wpf.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infra.Wpf.Common.Helpers;
using HomeAccounting.Business;
using Infra.Wpf.Business;

namespace HomeAccounting.UI.ViewModels.BaseInfo
{
    [ViewType(typeof(PersonCreateView))]
    public class PersonCreateVM : ViewModelBase<Person>
    {
        public bool isEdit = false;

        public RelayCommand SubmitCommand { get; set; }

        public PersonCreateVM(Person model = null)
        {
            SubmitCommand = new RelayCommand(SubmitExecute);

            if (model == null)
            {
                ViewTitle = "افزودن شخص";
                Model = new Person();
            }
            else
            {
                isEdit = true;
                ViewTitle = "ویرایش شخص";
                Model = model.Copy();
            }
        }

        private void SubmitExecute()
        {
            Model.UserId = 1;
            Model.CreateDate = DateTime.Now;
            Model.RecordStatusId = 1;

            var uow = new AccountingUow();
            BusinessResult<bool> result;

            if (isEdit)
                result = uow.PersonRepository.Update(Model);
            else
                result = uow.PersonRepository.Add(Model);

            uow.SaveChange();
            Billboard.ShowMessage(result.Message.MessageType, result.Message.Message);
            NavigationService.GoBack();
        }
    }
}