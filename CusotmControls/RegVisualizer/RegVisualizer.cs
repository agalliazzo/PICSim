using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace CusotmControls.RegVisualizer
{
    public class RegVisualizer : FlowLayoutPanel
    {
        int _bitNumber = 8;
        int BitNumber
        {
            get => _bitNumber;
            set
            {
                _bitNumber = value;
                RegenControls();
                UpdateControlSize();
            }
        }
        public Color LedColor { get; set; } = Color.Green;

        private int _bitLedSize = 40;
        public int BitLedSize
        {
            get => _bitLedSize;
            set
            {
                _bitLedSize = value;
                UpdateControlSize();
            }
        }

        private int _currentWord = 0;

        private Dictionary<int, string> _labels = new Dictionary<int, string>();

        public RegVisualizer()
        {
            FlowDirection = FlowDirection.RightToLeft;
            RegenControls();
        }

        public void SetLables(string[] labels)
        {
            for (int i = 0; i < labels.Length; i++)
                _labels[i] = labels[i];
            RegenControls();
        }

        public void SetLabel(string label, int bitNumber)
        {
            _labels[BitNumber] = label;
            RegenControls();
        }

        public void SetValue(int value)
        {
            for (int i = 0; i < _bitNumber; i++)
            {
                SetBit(i, (value & (0x01 << i)) != 0);
            }
        }

        public void SetBit(int bitNumber, bool value)
        {
            BitVisualizer bvis = (BitVisualizer)Controls[bitNumber];
            bvis.Checked = value;
            bvis.Invalidate();
            Invalidate();
        }

        private void UpdateControlSize()
        {
            SuspendLayout();
            Width = (BitLedSize + 5) * _bitNumber + 10;
            Height = BitLedSize + 10;
            ResumeLayout();
            PerformLayout();
        }

        private void RegenControls()
        {
            SuspendLayout();
            this.Controls.Clear();
            for (int i = 0; i < _bitNumber; i++)
            {
                string label = _labels.ContainsKey(i) ? _labels[i] : $"B{i}";
                BitVisualizer bit = new BitVisualizer()
                {
                    OnColor = LedColor,
                    OffColor = Color.DarkGray,
                    Checked = false,
                    BitNumber = i,
                    Size = new Size(BitLedSize, BitLedSize),
                    Text = label
                };
                bit.CheckedChanged += Bit_CheckedChanged;

                Controls.Add(bit);
            }
            ResumeLayout();
            UpdateControlSize();
        }

        private int MakeWordFromControls()
        {
            int mask = 0;
            foreach (Control c in this.Controls)
            {
                if (c is BitVisualizer bit)
                {
                    mask = (bit.Checked) ? mask | (0x01 << bit.BitNumber) : mask & ~(0x01 << bit.BitNumber);
                }
            }
            return mask;
        }

        protected override void OnResize(EventArgs eventargs)
        {
            //base.OnResize(eventargs);
            UpdateControlSize();
        }

        public event EventHandler<RegVisualizerChangedEventArgs> BitChanged;

        private void Bit_CheckedChanged(object sender, EventArgs e)
        {
            int newWord = MakeWordFromControls();
            BitChanged?.Invoke(sender, new RegVisualizerChangedEventArgs()
            {
                OldWord = _currentWord,
                NewWord = newWord
            }
            );
            _currentWord = newWord;
        }
    }

    public class RegVisualizerChangedEventArgs : EventArgs
    {
        public int OldWord { get; set; }
        public int NewWord { get; set; }
    }
}
