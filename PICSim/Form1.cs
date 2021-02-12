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
        private const int PORTA_Addr = 0x05;
        private const int PORTB_Addr = 0x06;

        public Form1()
        {
            InitializeComponent();
            statusRegVisualizer.SetLables(new string[] { "C", "DC", "Z", "nPD", "nTO", "RP0", "RP1", "IRP" });
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
            statusRegVisualizer.SetValue(_cpu.StatusRegister.Register);
            lblProgramCounter.Text = $"PC: {_cpu.ProgramCounter:X} ";
            regVisualizer1.SetValue(_cpu.RAMMemoryAndRegisters[PORTA_Addr].Register);
            regVisualizer2.SetValue(_cpu.RAMMemoryAndRegisters[PORTB_Addr].Register);
            lstProgramMemory.SelectedIndex = _cpu.ProgramCounter;
            //lblMem.Text = _cpu.RAMMemoryAndRegisters[6].ToString();
            //}
        }
    }
}


