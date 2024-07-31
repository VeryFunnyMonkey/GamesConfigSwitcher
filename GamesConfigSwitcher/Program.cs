using System.Runtime.InteropServices;

namespace GamesConfigSwitcher
{
    internal static class Program
    {

        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            AttachConsole(ATTACH_PARENT_PROCESS);
            if (args.Length == 0)
            {
                RunCommandLine(args);
                //System.Windows.Forms.SendKeys.SendWait("{ENTER}");
                Application.Exit();
            }

            else
            {
                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();
                Application.Run(new Form1());
            }
        }

        static void RunCommandLine(string[] args)
        {
            CommandLineHandler.Handle(args);
        }
    }
}