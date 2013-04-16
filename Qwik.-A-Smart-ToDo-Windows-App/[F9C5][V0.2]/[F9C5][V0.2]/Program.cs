using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
namespace QWIK
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            /*---------------Auto Start Code-----------------*/
            string keyname = "ShortcutUIv1.exe";
            string assemblyLocation = Application.ExecutablePath.ToString(); //Assembly.GetExecutingAssembly().Location;

            AutoStart.SetAutoStart(keyname, assemblyLocation);

            /*---------------Auto Start Code-----------------*/

            FrmMain frm = new FrmMain();  
            new Controller();
            
        }
    }
}
