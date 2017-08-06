using System;
using System.Windows.Forms;

namespace PeekSteamIP
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var iniEx = e.ExceptionObject as IniConfigException;
            if (iniEx != null)
            {
                var msg = $"配置错误，请检查配置文件：{Env.ConfigFilePath}。";
                MessageBox.Show(msg);
            }
        }
    }
}
