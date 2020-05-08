namespace SDiC.Main
{
    partial class MainView
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
            this.SignOutBt = new System.Windows.Forms.Button();
            this.GreetingsL = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SignOutBt
            // 
            this.SignOutBt.Location = new System.Drawing.Point(692, 12);
            this.SignOutBt.Name = "SignOutBt";
            this.SignOutBt.Size = new System.Drawing.Size(96, 70);
            this.SignOutBt.TabIndex = 0;
            this.SignOutBt.Text = "Выйти из аккаунта";
            this.SignOutBt.UseVisualStyleBackColor = true;
            this.SignOutBt.Click += new System.EventHandler(this.SignOutBt_Click);
            // 
            // GreetingsL
            // 
            this.GreetingsL.AutoSize = true;
            this.GreetingsL.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GreetingsL.Location = new System.Drawing.Point(12, 13);
            this.GreetingsL.Name = "GreetingsL";
            this.GreetingsL.Size = new System.Drawing.Size(379, 44);
            this.GreetingsL.TabIndex = 1;
            this.GreetingsL.Text = "Здравствуйте, name";
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.GreetingsL);
            this.Controls.Add(this.SignOutBt);
            this.Name = "MainView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SignOutBt;
        private System.Windows.Forms.Label GreetingsL;
    }
}