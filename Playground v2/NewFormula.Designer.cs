namespace Playground_v2
{
    partial class NewFormula
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
            this.txtWarningMessage = new System.Windows.Forms.TextBox();
            this.btnNewFormula = new System.Windows.Forms.Button();
            this.btnNewLine = new System.Windows.Forms.Button();
            this.pnlNewFormula = new System.Windows.Forms.Panel();
            this.pnlNewFormula.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtWarningMessage
            // 
            this.txtWarningMessage.Location = new System.Drawing.Point(3, 110);
            this.txtWarningMessage.Name = "txtWarningMessage";
            this.txtWarningMessage.Size = new System.Drawing.Size(277, 20);
            this.txtWarningMessage.TabIndex = 100000;
            this.txtWarningMessage.Text = "Enter warning message";
            this.txtWarningMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtWarningMessage.Click += new System.EventHandler(this.txtWarningMessageClick);
            // 
            // btnNewFormula
            // 
            this.btnNewFormula.Location = new System.Drawing.Point(287, 110);
            this.btnNewFormula.Name = "btnNewFormula";
            this.btnNewFormula.Size = new System.Drawing.Size(75, 20);
            this.btnNewFormula.TabIndex = 6;
            this.btnNewFormula.Text = "Add formula";
            this.btnNewFormula.UseVisualStyleBackColor = true;
            this.btnNewFormula.Click += new System.EventHandler(this.btnNewFormula_Click);
            // 
            // btnNewLine
            // 
            this.btnNewLine.Location = new System.Drawing.Point(287, 3);
            this.btnNewLine.Name = "btnNewLine";
            this.btnNewLine.Size = new System.Drawing.Size(75, 21);
            this.btnNewLine.TabIndex = 11;
            this.btnNewLine.Text = "new line";
            this.btnNewLine.UseVisualStyleBackColor = true;
            this.btnNewLine.Click += new System.EventHandler(this.btnNewLine_Click);
            // 
            // pnlNewFormula
            // 
            this.pnlNewFormula.Controls.Add(this.btnNewLine);
            this.pnlNewFormula.Controls.Add(this.btnNewFormula);
            this.pnlNewFormula.Controls.Add(this.txtWarningMessage);
            this.pnlNewFormula.Location = new System.Drawing.Point(12, 12);
            this.pnlNewFormula.Name = "pnlNewFormula";
            this.pnlNewFormula.Size = new System.Drawing.Size(395, 176);
            this.pnlNewFormula.TabIndex = 12;
            // 
            // NewFormula
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 196);
            this.Controls.Add(this.pnlNewFormula);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "NewFormula";
            this.Text = "Add a new formula";
            this.pnlNewFormula.ResumeLayout(false);
            this.pnlNewFormula.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtWarningMessage;
        private System.Windows.Forms.Button btnNewFormula;
        private System.Windows.Forms.Button btnNewLine;
        private System.Windows.Forms.Panel pnlNewFormula;
    }
}