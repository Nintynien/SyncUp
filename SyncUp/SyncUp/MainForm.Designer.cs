namespace SyncUp
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.labelAppUUID = new System.Windows.Forms.Label();
            this.textBoxAppUUID = new System.Windows.Forms.TextBox();
            this.labelToken = new System.Windows.Forms.Label();
            this.textBoxToken = new System.Windows.Forms.TextBox();
            this.lvMessages = new System.Windows.Forms.ListView();
            this.gbSend = new System.Windows.Forms.GroupBox();
            this.labelSendTo = new System.Windows.Forms.Label();
            this.comboBoxUser = new System.Windows.Forms.ComboBox();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.labelMessage = new System.Windows.Forms.Label();
            this.buttonSend = new System.Windows.Forms.Button();
            this.clmStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmTo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gbSend.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(336, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Enabled = false;
            this.textBoxPassword.Location = new System.Drawing.Point(395, 6);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(156, 20);
            this.textBoxPassword.TabIndex = 7;
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Enabled = false;
            this.textBoxUsername.Location = new System.Drawing.Point(73, 6);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(257, 20);
            this.textBoxUsername.TabIndex = 6;
            // 
            // labelAppUUID
            // 
            this.labelAppUUID.AutoSize = true;
            this.labelAppUUID.Location = new System.Drawing.Point(12, 37);
            this.labelAppUUID.Name = "labelAppUUID";
            this.labelAppUUID.Size = new System.Drawing.Size(56, 13);
            this.labelAppUUID.TabIndex = 8;
            this.labelAppUUID.Text = "App UUID";
            // 
            // textBoxAppUUID
            // 
            this.textBoxAppUUID.Enabled = false;
            this.textBoxAppUUID.Location = new System.Drawing.Point(73, 34);
            this.textBoxAppUUID.Name = "textBoxAppUUID";
            this.textBoxAppUUID.Size = new System.Drawing.Size(257, 20);
            this.textBoxAppUUID.TabIndex = 9;
            // 
            // labelToken
            // 
            this.labelToken.AutoSize = true;
            this.labelToken.Location = new System.Drawing.Point(351, 37);
            this.labelToken.Name = "labelToken";
            this.labelToken.Size = new System.Drawing.Size(38, 13);
            this.labelToken.TabIndex = 10;
            this.labelToken.Text = "Token";
            // 
            // textBoxToken
            // 
            this.textBoxToken.Enabled = false;
            this.textBoxToken.Location = new System.Drawing.Point(395, 34);
            this.textBoxToken.Name = "textBoxToken";
            this.textBoxToken.Size = new System.Drawing.Size(156, 20);
            this.textBoxToken.TabIndex = 11;
            // 
            // lvMessages
            // 
            this.lvMessages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmStatus,
            this.clmTo,
            this.clmMessage});
            this.lvMessages.Location = new System.Drawing.Point(13, 181);
            this.lvMessages.Name = "lvMessages";
            this.lvMessages.Size = new System.Drawing.Size(541, 193);
            this.lvMessages.TabIndex = 12;
            this.lvMessages.UseCompatibleStateImageBehavior = false;
            this.lvMessages.View = System.Windows.Forms.View.Details;
            // 
            // gbSend
            // 
            this.gbSend.Controls.Add(this.buttonSend);
            this.gbSend.Controls.Add(this.labelMessage);
            this.gbSend.Controls.Add(this.textBoxMessage);
            this.gbSend.Controls.Add(this.comboBoxUser);
            this.gbSend.Controls.Add(this.labelSendTo);
            this.gbSend.Location = new System.Drawing.Point(13, 65);
            this.gbSend.Name = "gbSend";
            this.gbSend.Size = new System.Drawing.Size(538, 110);
            this.gbSend.TabIndex = 13;
            this.gbSend.TabStop = false;
            this.gbSend.Text = "Send Message";
            // 
            // labelSendTo
            // 
            this.labelSendTo.AutoSize = true;
            this.labelSendTo.Location = new System.Drawing.Point(7, 20);
            this.labelSendTo.Name = "labelSendTo";
            this.labelSendTo.Size = new System.Drawing.Size(51, 13);
            this.labelSendTo.TabIndex = 0;
            this.labelSendTo.Text = "Send To:";
            // 
            // comboBoxUser
            // 
            this.comboBoxUser.FormattingEnabled = true;
            this.comboBoxUser.Location = new System.Drawing.Point(64, 17);
            this.comboBoxUser.Name = "comboBoxUser";
            this.comboBoxUser.Size = new System.Drawing.Size(468, 21);
            this.comboBoxUser.TabIndex = 1;
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Location = new System.Drawing.Point(64, 45);
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(468, 20);
            this.textBoxMessage.TabIndex = 2;
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Location = new System.Drawing.Point(10, 48);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(53, 13);
            this.labelMessage.TabIndex = 3;
            this.labelMessage.Text = "Message:";
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(457, 71);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 23);
            this.buttonSend.TabIndex = 4;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // clmStatus
            // 
            this.clmStatus.Text = "Status";
            this.clmStatus.Width = 77;
            // 
            // clmTo
            // 
            this.clmTo.Text = "To";
            this.clmTo.Width = 157;
            // 
            // clmMessage
            // 
            this.clmMessage.Text = "Message";
            this.clmMessage.Width = 267;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 386);
            this.Controls.Add(this.gbSend);
            this.Controls.Add(this.lvMessages);
            this.Controls.Add(this.textBoxToken);
            this.Controls.Add(this.labelToken);
            this.Controls.Add(this.textBoxAppUUID);
            this.Controls.Add(this.labelAppUUID);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.textBoxUsername);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.gbSend.ResumeLayout(false);
            this.gbSend.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.Label labelAppUUID;
        private System.Windows.Forms.TextBox textBoxAppUUID;
        private System.Windows.Forms.Label labelToken;
        private System.Windows.Forms.TextBox textBoxToken;
        private System.Windows.Forms.ListView lvMessages;
        private System.Windows.Forms.ColumnHeader clmStatus;
        private System.Windows.Forms.ColumnHeader clmTo;
        private System.Windows.Forms.ColumnHeader clmMessage;
        private System.Windows.Forms.GroupBox gbSend;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.ComboBox comboBoxUser;
        private System.Windows.Forms.Label labelSendTo;
    }
}

