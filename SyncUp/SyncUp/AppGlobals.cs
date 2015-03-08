using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncUp
{
    public class AppGlobals
    {
        public enum SyncUpError
        {
            None,
            InvalidUsernamePassword,
            InvalidAppUUID,
            InvalidToken,
            BadParams,
            NoInternet,
            UserOffline,
            Unknown
        }

        public static string AppGUID = null;
        public static string Token = null;
        public static string Username = null;
        public static string Password = null;

        public static void StartApplication()
        {
            LoginForm loginform = new LoginForm();
            loginform.Show();
        }

        public static SyncUpError Login(string username, string password)
        {
            if (username == null || password == null)
            {
                return SyncUpError.BadParams;
            }

            int error = SyncUpAPI.LoginApp(username, password);

            if (error != SyncUpAPI.ERR_SUCCESS)
            {
                return SyncUpError.InvalidUsernamePassword;
            }

            MainForm.mainform.updateForm();

            return SyncUpError.None;
        }
    }
}
