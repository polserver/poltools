using System;
using System.IO;
using System.Windows.Forms;

namespace POLLaunch
{
	static class POLChecks
	{
        static public void ScriptChecks(ref TextBox textbox, ref ToolStripProgressBar ProgressBar)
		{
			string[] src = FileSystemUtil.GetAllFileNames((string)Settings.Global.Properties["POLPath"], "*.src");
			string[] ecl = FileSystemUtil.GetAllFileNames((string)Settings.Global.Properties["POLPath"], "*.ecl");
			if (ecl.Length < src.Length)
				textbox.AppendText("* Warning: Not all scripts are compiled." + Environment.NewLine);
			else if (ecl.Length > src.Length)
				textbox.AppendText("* Warning: There are more ecl files than src files." + Environment.NewLine);
			else
				textbox.AppendText("* Pass: All scripts are compiled." + Environment.NewLine);
			textbox.AppendText("Found " + src.Length.ToString() + " .src files and " + ecl.Length.ToString() + " .ecl files." + Environment.NewLine);
            ProgressBar.PerformStep();
		}

		static public void RealmChecks(ref TextBox textbox, ref ToolStripProgressBar ProgressBar)
		{
			if (!Directory.Exists((string)Settings.Global.Properties["POLPath"] + "\\realm"))
				textbox.AppendText("* Fail: Realms have not been generated." + Environment.NewLine);
			else
			{
				string[] tmp = FileSystemUtil.GetAllFileNames((string)Settings.Global.Properties["POLPath"] + "\\realm", "*.*");
				if (tmp.Length > 1)
					textbox.AppendText("* Pass: Realm folder detected." + Environment.NewLine);
				else
					textbox.AppendText("* Fail: Realm folder is empty." + Environment.NewLine);
			}
            ProgressBar.PerformStep();

			foreach (string s in PL_UOConvert.GetConfigFileNames())
			{
                if (File.Exists((string)Settings.Global.Properties["POLPath"] + "\\" + s))
					textbox.AppendText("* Pass: Found config file '" + s + "'." + Environment.NewLine);
				else
					textbox.AppendText("* Fail: Config file '" + s + "' not found." + Environment.NewLine);
                ProgressBar.PerformStep();
			}

		}
	}
}
