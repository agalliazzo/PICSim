namespace PICSim
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lstProgramMemory = new System.Windows.Forms.ListBox();
            this.btnStartProgram = new System.Windows.Forms.Button();
            this.lblStatusReg = new System.Windows.Forms.Label();
            this.lblProgramCounter = new System.Windows.Forms.Label();
            this.lblMem = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // lstProgramMemory
            // 
            this.lstProgramMemory.FormattingEnabled = true;
            this.lstProgramMemory.Location = new System.Drawing.Point(12, 27);
            this.lstProgramMemory.Name = "lstProgramMemory";
            this.lstProgramMemory.Size = new System.Drawing.Size(298, 407);
            this.lstProgramMemory.TabIndex = 1;
            // 
            // btnStartProgram
            // 
            this.btnStartProgram.Location = new System.Drawing.Point(316, 27);
            this.btnStartProgram.Name = "btnStartProgram";
            this.btnStartProgram.Size = new System.Drawing.Size(98, 27);
            this.btnStartProgram.TabIndex = 2;
            this.btnStartProgram.Text = "Start program";
            this.btnStartProgram.UseVisualStyleBackColor = true;
            this.btnStartProgram.Click += new System.EventHandler(this.btnStartProgram_Click);
            // 
            // lblStatusReg
            // 
            this.lblStatusReg.AutoSize = true;
            this.lblStatusReg.Location = new System.Drawing.Point(316, 67);
            this.lblStatusReg.Name = "lblStatusReg";
            this.lblStatusReg.Size = new System.Drawing.Size(63, 13);
            this.lblStatusReg.TabIndex = 3;
            this.lblStatusReg.Text = "Status Reg:";
            // 
            // lblProgramCounter
            // 
            this.lblProgramCounter.AutoSize = true;
            this.lblProgramCounter.Location = new System.Drawing.Point(316, 80);
            this.lblProgramCounter.Name = "lblProgramCounter";
            this.lblProgramCounter.Size = new System.Drawing.Size(24, 13);
            this.lblProgramCounter.TabIndex = 3;
            this.lblProgramCounter.Text = "PC:";
            // 
            // lblMem
            // 
            this.lblMem.AutoSize = true;
            this.lblMem.Location = new System.Drawing.Point(316, 126);
            this.lblMem.Name = "lblMem";
            this.lblMem.Size = new System.Drawing.Size(24, 13);
            this.lblMem.TabIndex = 4;
            this.lblMem.Text = "PC:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblMem);
            this.Controls.Add(this.lblProgramCounter);
            this.Controls.Add(this.lblStatusReg);
            this.Controls.Add(this.btnStartProgram);
            this.Controls.Add(this.lstProgramMemory);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ListBox lstProgramMemory;
        private System.Windows.Forms.Button btnStartProgram;
        private System.Windows.Forms.Label lblStatusReg;
        private System.Windows.Forms.Label lblProgramCounter;
        private System.Windows.Forms.Label lblMem;
    }
}

