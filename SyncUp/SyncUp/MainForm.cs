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

        private void MainForm_Shown(object sender, EventArgs e)
        {
            // Start the application
            AppGlobals.StartApplication();
        }
    }
}
