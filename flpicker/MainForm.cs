using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace flpicker
{
    public partial class MainForm : Form
    {
        private List<FlInstall> installs;
        private string flpPath;

        public MainForm(string flpPath)
        {
            InitializeComponent();

            Text = "flpicker - picking fl studio versions since 2026";

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            if (string.IsNullOrWhiteSpace(flpPath) || !File.Exists(flpPath))
            {
                MessageBox.Show(
                    "That’s not an .flp file… nice try.",
                    "flpicker - picking fl studio versions since 2026",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                Environment.Exit(0);
                return;
            }

            this.flpPath = flpPath;

            installs = FlScanner.FindInstalls();

            BuildUI();
        }

        private void BuildUI()
        {
            Controls.Clear();

            int y = 10;

            var title = new Label
            {
                Text = flpPath != null
                    ? Path.GetFileName(flpPath)
                    : "No FLP provided",
                Left = 10,
                Top = y,
                Width = 400
            };

            Controls.Add(title);
            y += 40;

            if (flpPath == null)
            {
                var info = new Label
                {
                    Text = "open flp pls thanks",
                    Left = 10,
                    Top = y,
                    Width = 400
                };

                Controls.Add(info);
                return;
            }

            var x64 = installs.Where(i => i.Is64Bit).ToList();
            var x86 = installs.Where(i => !i.Is64Bit).ToList();

            y = AddButtons(x64, y);
            y = AddButtons(x86, y);

            this.Height = y + 60;
            this.Width = 450;
        }

        private int AddButtons(List<FlInstall> list, int y)
        {
            foreach (var fl in list)
            {
                var label = fl.Name + (fl.Is64Bit ? " (x64)" : " (x86)");

                var btn = new Button
                {
                    Text = label,
                    Left = 10,
                    Top = y,
                    Width = 400
                };

                btn.Click += (s, e) =>
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = fl.Path,
                            Arguments = $"\"{flpPath}\"",
                            UseShellExecute = true
                        });

                        Environment.Exit(0);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("failed to launch FL Studio:\n" + ex.Message);
                    }
                };

                Controls.Add(btn);
                y += 45;
            }

            return y;
        }
    }
}