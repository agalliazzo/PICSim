using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PICSim
{
    public partial class Form1 : Form
    {
        PIC16FCpu _cpu;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "Intel HEX File (*.hex) | *.hex",
                Multiselect = false,
                FileName = Properties.Settings.Default.LastOpenedFile
            };
            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.OK)
                return;
            _cpu = new PIC16FCpu(256, 1024);
            _cpu.FillProgramMemory(dialog.FileName);

            foreach (UInt16 cell in _cpu.ProgramMemory)
            {
                lstProgramMemory.Items.Add(cell.ToString("X"));
            }


            Properties.Settings.Default.LastOpenedFile = dialog.FileName;
            Properties.Settings.Default.Save();
        }

        private void btnStartProgram_Click(object sender, EventArgs e)
        {
            //while (true)
            //{
            _cpu.ExecuteInstruction();
            lblStatusReg.Text = "Status Reg: " + _cpu.StatusRegister.ToString();
            lblProgramCounter.Text = $"PC: {_cpu.ProgramCounter.ToString("X")} ";
            lblMem.Text = _cpu.RAMMemoryAndRegisters[6].ToString();
            //}
        }
    }
}


