using System;
using System.IO;
using System.Collections;
using System.Threading;
using System.Windows.Forms;

namespace POLLaunch
{
	public class UOConvert
	{
		static private Form1 _form;

        public static ArrayList BuildList = new ArrayList();

        public UOConvert(Form1 form)
		{
			_form = form;
		}

		static public string[] GetConfigFileNames()
		{
			string[] names = {
							"config\\multis.cfg",
							"config\\tiles.cfg",
							"config\\landtiles.cfg"};
			return names;
		}

		static public string CheckBoxIdtoRealmName(int id)
		{
			switch (id)
			{
				case 4: return "britannia";
				case 5: return "britannia ml";
				case 6: return "trammel";
				case 7: return "ilshenar";
				case 8: return "malas";
				case 9: return "tokuno";
			}
			return "";
		}
		/*
		static public string[] GetRealmCommands(string realm)
		{
			realm = realm.ToLower();
			string[] lines = new string[0];
			switch (realm)
			{
				// To-Do: These could be classes too rather than string arrays. But.. laaazy!
				case "britannia":
					lines = new string[] {"map <uopath> realm=britannia mapid=0 usedif=1 width=6144 height=4096",
						"statics <uopath> realm=britannia",
						"maptile <uopath> realm=britannia"};
					break;
				case "britannia ml":
					lines = new string[] {"map <uopath> realm=britannia mapid=0 usedif=1 width=7168 height=4096",
						"statics <uopath> realm=britannia",
						"maptile <uopath> realm=britannia"};
					break;
				case "trammel":
					lines = new string[] {"map <uopath> realm=britannia_alt mapid=0 usedif=1 width=6144 height=4096",
						"statics <uopath> realm=britannia_alt",
						"maptile <uopath> realm=britannia_alt"};
					break;
				case "ilshenar":
					lines = new string[] {"map <uopath> realm=ilshenar mapid=2 usedif=1 width=2304 height=1600",
						"statics <uopath> realm=ilshenar",
						"maptile <uopath> realm=ilshenar"};
					break;
				case "malas":
					lines = new string[] {"map <uopath> realm=malas mapid=3 usedif=1 width=2560 height=2048",
						"statics <uopath> realm=malas",
						"maptile <uopath> realm=malas"};
					break;
				case "tokuno":
					lines = new string[] {"map <uopath> realm=tokuno mapid=4 usedif=1 width=1448 height=1448",
						"statics <uopath> realm=tokuno",
						"statics <uopath> realm=tokuno"};
					break;

				default:
					break;
			}
			return lines;
		}

		public void RunUOConvert()
		{
			new Thread(RunCommands).Start();
		}

		private void RunCommands()
		{
			_form.textBox3.Text = "";
			_form.Cursor = Cursors.WaitCursor;
			_form.button4.Enabled = false;
			if (_form.checkBox1.Checked)
				MakeUOConfigFile("multis", _form.textBox8.Text);
			if (_form.checkBox2.Checked)
				MakeUOConfigFile("tiles", _form.textBox8.Text);
			if (_form.checkBox3.Checked)
				MakeUOConfigFile("landtiles", _form.textBox8.Text);

			if (_form.checkBox4.Checked)
				BuildRealm(CheckBoxIdtoRealmName(4), _form.textBox8.Text);
			if (_form.checkBox5.Checked)
				BuildRealm(CheckBoxIdtoRealmName(5), _form.textBox8.Text);
			if (_form.checkBox6.Checked)
				BuildRealm(CheckBoxIdtoRealmName(6), _form.textBox8.Text);
			if (_form.checkBox7.Checked)
				BuildRealm(CheckBoxIdtoRealmName(7), _form.textBox8.Text);
			if (_form.checkBox8.Checked)
				BuildRealm(CheckBoxIdtoRealmName(8), _form.textBox8.Text);
			if (_form.checkBox9.Checked)
				BuildRealm(CheckBoxIdtoRealmName(9), _form.textBox8.Text);

			_form.button4.Enabled = true;
			_form.Cursor = Cursors.Default;
		}

		private void BuildRealm(string realm_name, string uopath)
		{
			string[] lines = GetRealmCommands(realm_name);
			foreach (string line in lines)
				RunConverter(line, uopath);
		}

		private void MakeUOConfigFile(string key, string uopath)
		{
			RunConverter(key + " <uopath>", _form.textBox8.Text);
			try { File.Delete("config\\" + key + ".cfg"); }
			catch (Exception ex) { MessageBox.Show(ex.Message, "Exception Occured"); }

			try { File.Move(key + ".cfg", "config\\" + key + ".cfg"); }
			catch (Exception ex) { MessageBox.Show(ex.Message, "Exception Occured"); }
		}

		private void RunConverter(string arguments, string uopath)
		{
			if (Directory.Exists(uopath))
			{
				arguments = arguments.Replace("<uopath>", "uodata=\"" + uopath + "\"");
			}
			else
				arguments = arguments.Replace("<uopath>", "");
			_form.textBox3.Text += "Command: uoconvert.exe " + arguments + Environment.NewLine;

			string result;
			string error = "";
			try
			{
				result = CommandLineHelper.Run("uoconvert.exe", arguments, out error);
			}
			catch (Exception ex)
			{
				result = ex.Message;
			}
			_form.textBox3.Text += result + Environment.NewLine + error + Environment.NewLine;
		}
		*/
	}
}
