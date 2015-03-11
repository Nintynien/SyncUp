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

        public class User
        {
            public string first_name;
            public string last_name;
            public string userid;
            
            public User(string first_name, string last_name, string userid) {
                this.first_name = first_name;
                this.last_name = last_name;
                this.userid = userid;
            }

            public override string ToString() {
                // Generates the text shown in the combo box
                return getDisplayName();
            }

            public string getDisplayName()
            {
                return first_name + " " + last_name;
            }
        }

        public static string AppGUID = "8F701225-6C9F-433E-A9AA-A1ACF1DA7AC6";
        public static string Token = "JgVDeh57nns1LI38GeFQLCen+xdUARJM";
        public static string Username = "test";
        public static string Password = "test";
        public static List<User> Users = new List<User>();

        public static void StartApplication()
        {
            Console.WriteLine("Starting Application");
#if DEBUG
            Users.Add(new User("test", "user", "B2F0B803-06BF-E411-940C-A0B3CCE17F1E"));
            Users.Add(new User("test2", "user2", "B2F0B803-06BF-E411-940C-A0B3CCE17F1E"));
#endif
            if (AppGUID != null && Token != null)
            {
                // TODO:
                // Attempt to log in and verify the account.
                // If login fails, clear GUID and Token so the user is required to log in again
            }

            if (AppGUID == null && Token == null)
            {
                LoginForm loginform = new LoginForm();
                loginform.Show();
            }

            MainForm.mainform.updateForm();
            MainForm.mainform.updateUsers();

            Console.WriteLine("Finished Starting Application");
        }

        public static SyncUpError Register(string username, string password, string email = null)
        {
            if (username == null || password == null)
            {
                return SyncUpError.BadParams;
            }

            SyncUpAPI.ApiJsonUserRegResp resp = SyncUpAPI.RegisterUser(username, password, email);

            if (resp.code != SyncUpAPI.ERR_SUCCESS)
            {
                return SyncUpError.Unknown;
            }

            return SyncUpError.None;
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
