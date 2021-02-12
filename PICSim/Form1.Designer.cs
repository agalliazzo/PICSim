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
            this.lblProgramCounter = new System.Windows.Forms.Label();
            this.statusRegVisualizer = new CusotmControls.RegVisualizer.RegVisualizer();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.regVisualizer1 = new CusotmControls.RegVisualizer.RegVisualizer();
            this.label3 = new System.Windows.Forms.Label();
            this.regVisualizer2 = new CusotmControls.RegVisualizer.RegVisualizer();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1056, 24);
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
            this.btnStartProgram.Text = "Step";
            this.btnStartProgram.UseVisualStyleBackColor = true;
            this.btnStartProgram.Click += new System.EventHandler(this.btnStartProgram_Click);
            // 
            // lblProgramCounter
            // 
            this.lblProgramCounter.AutoSize = true;
            this.lblProgramCounter.Location = new System.Drawing.Point(316, 57);
            this.lblProgramCounter.Name = "lblProgramCounter";
            this.lblProgramCounter.Size = new System.Drawing.Size(24, 13);
            this.lblProgramCounter.TabIndex = 3;
            this.lblProgramCounter.Text = "PC:";
            // 
            // statusRegVisualizer
            // 
            this.statusRegVisualizer.BitLedSize = 40;
            this.statusRegVisualizer.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.statusRegVisualizer.LedColor = System.Drawing.Color.Red;
            this.statusRegVisualizer.Location = new System.Drawing.Point(404, 74);
            this.statusRegVisualizer.Name = "statusRegVisualizer";
            this.statusRegVisualizer.Size = new System.Drawing.Size(370, 50);
            this.statusRegVisualizer.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(316, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Status Register:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(316, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "PORTA";
            // 
            // regVisualizer1
            // 
            this.regVisualizer1.BitLedSize = 40;
            this.regVisualizer1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.regVisualizer1.LedColor = System.Drawing.Color.Red;
            this.regVisualizer1.Location = new System.Drawing.Point(404, 130);
            this.regVisualizer1.Name = "regVisualizer1";
            this.regVisualizer1.Size = new System.Drawing.Size(370, 50);
            this.regVisualizer1.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(316, 204);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "PORTB";
            // 
            // regVisualizer2
            // 
            this.regVisualizer2.BitLedSize = 40;
            this.regVisualizer2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.regVisualizer2.LedColor = System.Drawing.Color.Red;
            this.regVisualizer2.Location = new System.Drawing.Point(404, 186);
            this.regVisualizer2.Name = "regVisualizer2";
            this.regVisualizer2.Size = new System.Drawing.Size(370, 50);
            this.regVisualizer2.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1056, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.regVisualizer2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.regVisualizer1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusRegVisualizer);
            this.Controls.Add(this.lblProgramCounter);
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
        private System.Windows.Forms.Label lblProgramCounter;
        private CusotmControls.RegVisualizer.RegVisualizer statusRegVisualizer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private CusotmControls.RegVisualizer.RegVisualizer regVisualizer1;
        private System.Windows.Forms.Label label3;
        private CusotmControls.RegVisualizer.RegVisualizer regVisualizer2;
    }
}

