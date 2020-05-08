namespace SDiC
{
    partial class AuthorizationView
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
            this.LoginTB = new System.Windows.Forms.TextBox();
            this.PasswordTB = new System.Windows.Forms.TextBox();
            this.LoginBt = new System.Windows.Forms.Button();
            this.LoginL = new System.Windows.Forms.Label();
            this.PasswordL = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LoginTB
            // 
            this.LoginTB.Location = new System.Drawing.Point(12, 29);
            this.LoginTB.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.LoginTB.Name = "LoginTB";
            this.LoginTB.Size = new System.Drawing.Size(191, 22);
            this.LoginTB.TabIndex = 0;
            // 
            // PasswordTB
            // 
            this.PasswordTB.Location = new System.Drawing.Point(12, 81);
            this.PasswordTB.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.PasswordTB.Name = "PasswordTB";
            this.PasswordTB.PasswordChar = '*';
            this.PasswordTB.Size = new System.Drawing.Size(191, 22);
            this.PasswordTB.TabIndex = 0;
            // 
            // LoginBt
            // 
            this.LoginBt.Location = new System.Drawing.Point(12, 116);
            this.LoginBt.Name = "LoginBt";
            this.LoginBt.Size = new System.Drawing.Size(75, 30);
            this.LoginBt.TabIndex = 1;
            this.LoginBt.Text = "Login";
            this.LoginBt.UseVisualStyleBackColor = true;
            this.LoginBt.Click += new System.EventHandler(this.LoginBt_Click);
            // 
            // LoginL
            // 
            this.LoginL.AutoSize = true;
            this.LoginL.Location = new System.Drawing.Point(12, 9);
            this.LoginL.Name = "LoginL";
            this.LoginL.Size = new System.Drawing.Size(43, 17);
            this.LoginL.TabIndex = 2;
            this.LoginL.Text = "Login";
            // 
            // PasswordL
            // 
            this.PasswordL.AutoSize = true;
            this.PasswordL.Location = new System.Drawing.Point(12, 61);
            this.PasswordL.Name = "PasswordL";
            this.PasswordL.Size = new System.Drawing.Size(69, 17);
            this.PasswordL.TabIndex = 2;
            this.PasswordL.Text = "Password";
            // 
            // AuthorizationView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(220, 158);
            this.Controls.Add(this.PasswordL);
            this.Controls.Add(this.LoginL);
            this.Controls.Add(this.LoginBt);
            this.Controls.Add(this.PasswordTB);
            this.Controls.Add(this.LoginTB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AuthorizationView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox LoginTB;
        private System.Windows.Forms.TextBox PasswordTB;
        private System.Windows.Forms.Button LoginBt;
        private System.Windows.Forms.Label LoginL;
        private System.Windows.Forms.Label PasswordL;
    }
}

