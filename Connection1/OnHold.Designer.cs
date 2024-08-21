namespace Connection1
{
    partial class OnHold
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
            this.PanelHolder = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // PanelHolder
            // 
            this.PanelHolder.AutoScroll = true;
            this.PanelHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelHolder.Location = new System.Drawing.Point(0, 0);
            this.PanelHolder.Name = "PanelHolder";
            this.PanelHolder.Size = new System.Drawing.Size(334, 461);
            this.PanelHolder.TabIndex = 0;
            // 
            // OnHold
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 461);
            this.Controls.Add(this.PanelHolder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OnHold";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OnHold";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelHolder;
    }
}