namespace Playground_v2
{
    partial class Playground
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
            this.panelPlayground = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelPlayground
            // 
            this.panelPlayground.Location = new System.Drawing.Point(189, 70);
            this.panelPlayground.Name = "panelPlayground";
            this.panelPlayground.Size = new System.Drawing.Size(463, 303);
            this.panelPlayground.TabIndex = 0;
            // 
            // Playground
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 413);
            this.Controls.Add(this.panelPlayground);
            this.Name = "Playground";
            this.Text = "Playground";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelPlayground;
    }
}