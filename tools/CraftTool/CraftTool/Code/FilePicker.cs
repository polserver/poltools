using System.Windows.Forms;

namespace FilePicker
{
	static class FilePicker
	{
		static public string SelectFolderSimple()
		{
			System.Windows.Forms.FolderBrowserDialog fd = new System.Windows.Forms.FolderBrowserDialog();
			fd.ShowDialog();
			string path = fd.SelectedPath;
			return path;
		}

		static public string SelectFolder()
		{
			return SelectFolder(CraftTool.Program.GetPath(false));
		}
		static public string SelectFolder(string rootdir)
		{
			OpenFileDialog fd = new OpenFileDialog();
			fd.InitialDirectory = rootdir;

			fd.Title = "Please choose a folder";
			fd.CheckFileExists = false;

			fd.FileName = "[Get Folder...]";
			fd.Filter = "Folders|no.files";

			fd.ShowDialog();
			string dir_path = System.IO.Path.GetDirectoryName(fd.FileName);
			if (dir_path != null && dir_path.Length > 0)
				return dir_path;
			else
				return "";
		}

		static public string SelectFile()
		{
			return SelectFile(CraftTool.Program.GetPath(false), "All files (*.*)|*.*");
		}
		static public string SelectFile(string filter)
		{
			return SelectFile(CraftTool.Program.GetPath(false), filter);
		}
		static public string SelectFile(string rootdir, string filter)
		{
			OpenFileDialog fd = new OpenFileDialog();
			fd.InitialDirectory = rootdir;

			fd.Title = "Please select a file";
			fd.CheckFileExists = false;

			fd.FileName = "";
			fd.Filter = filter;

			fd.ShowDialog();
			string file_path = fd.FileName;
			if ( file_path != null && file_path.Length > 0 )
				return file_path;
			else
				return "";
		}
	}
}
