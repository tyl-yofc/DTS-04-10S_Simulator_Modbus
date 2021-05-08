using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace DTS
{
    static class Program
    {
      //  [DllImport("kernel32.dll")]
     //   public static extern bool AllocConsole();
        
     //   [DllImport("kernel32.dll")]
     //   public static extern bool FreeConsole();
        //  static extern bool AttachConsole(int dwProcessId);
        //   private const int ATTACH_PARENT_PROCESS = -1;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //   AttachConsole(ATTACH_PARENT_PROCESS);
            ///    Console.WriteLine("!!!");
            //     AllocConsole();
            //     Console.ForegroundColor = ConsoleColor.Red;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());

         //   FreeConsole();
            
        }
    }
}
