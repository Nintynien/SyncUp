using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyncUp
{
    public partial class MainForm : Form
    {
        public static MainForm mainform = null;
        
        public MainForm()
        {
            mainform = this;
            InitializeComponent();
        }

        public void updateForm()
        {
            textBoxAppUUID.Text = AppGlobals.AppGUID;
            textBoxPassword.Text = AppGlobals.Password;
            textBoxToken.Text = AppGlobals.Token;
            textBoxUsername.Text = AppGlobals.Username;
        }

        public void updateUsers()
        {
            comboBoxUser.Items.Clear();
            foreach (AppGlobals.User user in AppGlobals.Users)
            {
                comboBoxUser.Items.Add(user);
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            // Start the application
            AppGlobals.StartApplication();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            string message = textBoxMessage.Text;
            string userid = ((AppGlobals.User)comboBoxUser.SelectedItem).userid;
            SyncUpAPI.ApiJsonSendMsgResp resp = SyncUpAPI.SendMessage(AppGlobals.AppGUID, AppGlobals.Token, userid, message);
            if (resp == null || resp.code != 200)
            {
                //Send failed!
                ListViewItem lvi = new ListViewItem("Failed");
                //lvi.SubItems.Add(new ListViewItem.ListViewSubItem().Text="Failed");
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem().Text=userid);
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem().Text=message);
                lvMessages.Items.Add(lvi);
            }
            else
            {
                //Send success!
                ListViewItem lvi = new ListViewItem("Success");
                //lvi.SubItems.Add(new ListViewItem.ListViewSubItem().Text = "Success");
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem().Text = userid);
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem().Text = message);
                lvMessages.Items.Add(lvi);
            }
        }
    }
}
