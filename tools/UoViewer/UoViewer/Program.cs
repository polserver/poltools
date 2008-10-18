using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Collections;
using Ultima;



namespace UoViewer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        [STAThread]
        static void Main()
        {
            try
            {
                Options.Load();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new UoViewer());
                Options.Save();
            }
            catch (Exception err)
            {
                Clipboard.SetDataObject(err.ToString(), true);
                Application.Run(new ExceptionForm(err));
            }
        }
    }
}