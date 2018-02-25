using HomeAccounting.DataAccess.Models;
using Infra.Wpf.Business;
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
        private string _userName { get; set; }

        private string _password { get; set; }

        private Logger _logger { get; set; }

        public LoginBusiness(string userName, string password, Logger logger) : base(logger)
        {
            _userName = userName;
            _password = password;
            _logger = logger;

            OnExecute = Loggin;
        }

        private bool Loggin()
        {
            if (string.IsNullOrEmpty(_userName))
            {
                Result.Message = new BusinessMessage("خطا", "نام کاربری را وارد کنید.");
                return false;
            }

            if (string.IsNullOrEmpty(_password))
            {
                Result.Message = new BusinessMessage("خطا", "رمز عبور را وارد کنید.");
                return false;
            }

            var uow = new AccountingUow();

            var result = uow.GetRepository<User>().GetFirst(x => x.UserName.Equals(_userName));
            if (result.HasException)
            {
                Result.Message = new BusinessMessage("خطا", result.Message.Message);
                return false;
            }

            var user = result.Data;
            if (user == null || !HashPassword.VerifyPassword(_password, user.Password))
            {
                Result.Message = new BusinessMessage("خطا", "نام کاربری یا رمز عبور اشتباه است.");
                return false;
            }
            
            var roleResult = uow.GetRepository<V_UserRole>().Select(x => x.UserId == user.UserId, x => new Infra.Wpf.Security.Role { RoleId = x.RoleId, Name = x.RoleTitle });
            if (roleResult.HasException)
            {
                Result.Message = new BusinessMessage("خطا", roleResult.Message.Message);
                return false;
            }

            var permissionResult = uow.GetRepository<V_UserRolePermission>().Select(x => x.UserId == user.UserId, x => new Infra.Wpf.Security.Permission { PermissionId = x.PermmisionId, Url = x.Url }, distinct: true);
            if (permissionResult.HasException)
            {
                Result.Message = new BusinessMessage("خطا", permissionResult.Message.Message);
                return false;
            }

            Identity identity = new Identity(user.UserName, user.UserId, roleResult.Data, permissionResult.Data);
            Principal principal = new Principal(identity, AuthorizeBasedOn.BaseOnPermission);

            AppDomain.CurrentDomain.SetThreadPrincipal(principal);

            _logger.Log(new LogInfo()
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
