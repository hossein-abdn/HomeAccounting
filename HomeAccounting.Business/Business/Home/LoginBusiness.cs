using HomeAccounting.DataAccess.Models;
using Infra.Wpf.Business;
using Infra.Wpf.Common;
using Infra.Wpf.Repository;
using Infra.Wpf.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Business
{
    public class LoginBusiness : BusinessBase
    {
        private string userName { get; set; }

        private string password { get; set; }

        private IRepository<User> repository;

        private AccountingUow accountingUow;

        public LoginBusiness(string userName, string password, AccountingUow accountingUow) : base(accountingUow.Logger)
        {
            this.userName = userName;
            this.password = password;
            this.accountingUow = accountingUow;

            OnExecute = Login;
        }

        private bool Login()
        {
            if (string.IsNullOrEmpty(userName))
            {
                Result.Message = new BusinessMessage("خطا", "نام کاربری را وارد کنید.");
                return false;
            }

            if (string.IsNullOrEmpty(password))
            {
                Result.Message = new BusinessMessage("خطا", "رمز عبور را وارد کنید.");
                return false;
            }

            var user = accountingUow.GetRepository<User>().GetFirst(x => x.UserName.Equals(userName));
            if (user == null || !HashPassword.VerifyPassword(password, user.Password))
            {
                Result.Message = new BusinessMessage("خطا", "نام کاربری یا رمز عبور اشتباه است.");
                return false;
            }
            
            var roleResult = accountingUow.GetRepository<V_UserRole>().Select(x => x.UserId == user.UserId, x => new Infra.Wpf.Security.Role { RoleId = x.RoleId, Name = x.RoleTitle });
            var permissionResult = accountingUow.GetRepository<V_UserRolePermission>().Select(x => x.UserId == user.UserId, x => new Infra.Wpf.Security.Permission { PermissionId = x.PermmisionId, Url = x.Url }, distinct: true);

            Identity identity = new Identity(user.UserName, user.UserId, roleResult, permissionResult);
            Principal principal = new Principal(identity, AuthorizeBasedOn.BaseOnPermission);

            AppDomain.CurrentDomain.SetThreadPrincipal(principal);

            accountingUow.Logger.Log(new LogInfo()
            {
                CallSite = this.GetType().FullName,
                LogType = LogType.Information,
                UserId = user.UserId,
                Message = "Login: UserName = " + user.UserName
            });

            return true;
        }
    }
}
