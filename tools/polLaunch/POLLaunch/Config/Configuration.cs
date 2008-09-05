using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace POLLaunch.Config
{
	class Configuration
	{
		public string uo_path;
		public string pol_path;
		public string pol_exe_path;
		public string uoconvert_exe_path;
		public string ecompile_exe_path;

		public Configuration()
		{
			uo_path = "";
			pol_path = Program.GetPath();
			pol_exe_path = Program.GetPath()+"pol.exe";
			uoconvert_exe_path = Program.GetPath() + "uoconvert.exe";
			ecompile_exe_path = Program.GetPath() + @"scripts\ecompile.exe";
		}

		public void LoadConfiguration()
		{
		}

		public void SaveConfiguration()
		{
		}
	}
}
