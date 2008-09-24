using System.Collections.Generic;
using System.IO;

namespace POLLaunch
{
	/// <summary>
	/// http://dotnetperls.com/Content/Recursively-Find-Files.aspx
	/// Use this method to explore a directory and all of its files. Then, it
	/// recurses into the next level of directories, and collects a listing
	/// of all the file names you want.
	/// </summary>
	static class FileSystemUtil
	{
		static public string[] GetAllFileNames(string baseDir)
		{
			return GetAllFileNames(baseDir, "*.*");
		}
		/// <summary>
		/// Find all files in a directory, and all files within every nested
		/// directory.
		/// </summary>
		/// <param name="baseDir">The starting directory you want to use.</param>
		/// <returns>A string array containing all the file names.</returns>
		static public string[] GetAllFileNames(string baseDir, string filter)
		{
			// Store results in the file results list.
			List<string> fileResults = new List<string>();

			// Store a stack of our directories.
			Stack<string> directoryStack = new Stack<string>();
			directoryStack.Push(baseDir);

			// While there are directories to process and we don't have too many results
			while (directoryStack.Count > 0 && fileResults.Count < 1000)
			{
				string currentDir = directoryStack.Pop();

				// Add all files at this directory.
				foreach (string fileName in Directory.GetFiles(currentDir, filter))
				{
					fileResults.Add(fileName);
				}

				// Add all directories at this directory.
				foreach (string directoryName in Directory.GetDirectories(currentDir))
				{
					directoryStack.Push(directoryName);
				}
			}
			return fileResults.ToArray();
		}
	}
}