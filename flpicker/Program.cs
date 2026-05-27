using System;
using System.Windows.Forms;

namespace flpicker
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();

            string flpPath = args.Length > 0 ? args[0] : null;

            Application.Run(new MainForm(flpPath));
        }
    }
}