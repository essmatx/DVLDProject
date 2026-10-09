using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDManagePeople_PresentationLayer
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 1. Show Splash Form
            FormSplash splash = new FormSplash();
            splash.Show();
            splash.Refresh();

            // 2. Perform loading work asynchronously
            Task.Run(async () =>
            {
                await InitializeApplicationAsync(splash);

                // 3. Switch back to UI thread to open Main Form
                splash.Invoke(new MethodInvoker(() =>
                {
                    frmLogin mainForm = new frmLogin();
                    splash.Hide();
                    mainForm.FormClosed += (s, args) => splash.Close(); // Exit app when Main closes
                    mainForm.Show();
                }));
            });
            Application.Run();


       
        }


        private static async Task InitializeApplicationAsync(FormSplash splash)
        {
            // Simulate initialization steps
            splash.UpdateProgress(20, "Loading system configuration...");
            await Task.Delay(800);

            splash.UpdateProgress(60, "Connecting to database...");
            await Task.Delay(1000);

            splash.UpdateProgress(100, "Ready!");
            await Task.Delay(400);
        }
    }

}
