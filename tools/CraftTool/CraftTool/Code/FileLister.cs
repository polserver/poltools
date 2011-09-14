using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace FileLister
{
	static class FileSystemUtil
	{
		static public List<string> GetDirectories(string base_dir)
		{
			int i = base_dir.LastIndexOf('\\');
			base_dir = base_dir.Remove(i, base_dir.Length - i);

			string[] directories = Directory.GetDirectories(base_dir);

			List<string> list = new List<string>(directories);
			return list;
		}

		static public List<String> GetAllFileNames(string baseDir, SearchOption options)
		{
			return GetAllFileNames(baseDir, "*.*", options);
		}
		/// <summary>
		/// Find all files in a directory, and all files within every nested
		/// directory.
		/// </summary>
		/// <param name="baseDir">The starting directory you want to use.</param>
		/// <returns>A string array containing all the file names.</returns>
		static public List<string> GetAllFileNames(string baseDir, string filter, SearchOption options)
		{
			string[] file_paths = Directory.GetFiles(baseDir, filter, options);

			List<string> list = new List<string>(file_paths);
			return list;
		}
	}
}
