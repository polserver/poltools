using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace POLLaunch
{
	class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}

		static public string GetPath()
		{
			string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
			path = path.Substring(0, path.LastIndexOf(@"\") + 1);
			return path;
		}
	}
}
