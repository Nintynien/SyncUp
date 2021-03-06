﻿using System;
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
    public partial class LoginForm : Form
    {
        public enum LoginFormType
        {
            FirstLogin,
            Register,
            Reauthenticate
        }

        private LoginFormType type;

        public LoginForm(LoginFormType type = LoginFormType.FirstLogin)
        {
            this.type = type;
            InitializeComponent();
            loadType();
            this.Activate();
        }

        // Sets up the form for the type that it is (shows and hides the controls that are needed)
        private void loadType()
        {
            switch (type)
            {
                case LoginFormType.FirstLogin:
                {
                    textBoxUsername.Enabled = true;
                    textBoxPassword.Enabled = true;
                    buttonLogin.Visible = true;
                    buttonRegister.Visible = true;
                    break;
                }
                case LoginFormType.Reauthenticate:
                {
                    textBoxUsername.Text = AppGlobals.Username;
                    textBoxUsername.Enabled = false; // User can't change the username if we are reauthenticating
                    textBoxPassword.Enabled = true;
                    buttonLogin.Visible = true;
                    buttonRegister.Visible = false;
                    break;
                }
                case LoginFormType.Register:
                {
                    textBoxUsername.Enabled = true;
                    textBoxPassword.Enabled = true;
                    buttonLogin.Visible = false;
                    buttonRegister.Visible = true;
                    break;
                }
                default:
                    break;
            }
        }

        private void disableInput()
        {
            textBoxPassword.Enabled = false;
            textBoxUsername.Enabled = false;
            buttonLogin.Enabled = false;
            buttonRegister.Enabled = false;
        }

        private void enableInput()
        {
            textBoxPassword.Enabled = true;
            textBoxUsername.Enabled = true;
            buttonLogin.Enabled = true;
            buttonRegister.Enabled = true;
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            TopMost = false;
            disableInput();
            AppGlobals.SyncUpError error = AppGlobals.Register(textBoxUsername.Text, textBoxPassword.Text);
            if (error != AppGlobals.SyncUpError.None)
            {
                MessageBox.Show("Error Registering! Error:" + error);
            }
            else
            {
                // Successful register, login
                buttonLogin_Click(sender, e);
            }
            enableInput();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            TopMost = false;
            disableInput();
            AppGlobals.SyncUpError error = AppGlobals.Login(textBoxUsername.Text, textBoxPassword.Text);
            if (error != AppGlobals.SyncUpError.None)
            {
                MessageBox.Show("Error logging in! Error:" + error);
            }
            else
            {
                // Successful login, close the form
                this.Close();
            }
            enableInput();
        }
    }
}
