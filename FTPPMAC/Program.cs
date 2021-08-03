using FTPPMAC.Action;
using FTPPMAC.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FTPPMAC
{
    public class Program
    {
        private static Timer _timer = null;

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        public static void Main(string[] args)
        {
            var handle = GetConsoleWindow();

            // Hide
            //ShowWindow(handle, SW_HIDE);

            // Show
            ShowWindow(handle, SW_SHOW);

            // Create a Timer object that knows to call our TimerCallback
            // method once every 2000 milliseconds.
            _timer = new Timer(TimerCallback, null, 0, 60000);
            // Wait for the user to hit <Enter>
            Console.ReadLine();
           
        }
        private static void TimerCallback(Object o)
        {
            // Display the date/time when this method got called.

            DateTime now = DateTime.Now;

            Console.WriteLine("In TimerCallback: " + DateTime.Now);

            if(now.Minute == 5)
            {
                MainController main = new MainController();

                main.Main();
            }
        }
    }
}
