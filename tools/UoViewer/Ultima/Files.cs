﻿using System;
using System.Collections;
using System.IO;
using Microsoft.Win32;

namespace Ultima
{
    public sealed class Files
    {
        private static bool m_CacheData = true;
        private static bool m_UseHashFile = false;
        private static IDictionary m_MulPath;
        private static string m_Directory;

        /// <summary>
        /// Should loaded Data be cached
        /// </summary>
        public static bool CacheData { get { return m_CacheData; } set { m_CacheData = value; } }
        /// <summary>
        /// Should a Hashfile be used to speed up loading
        /// </summary>
        public static bool UseHashFile { get { return m_UseHashFile; } set { m_UseHashFile = value; } }
        /// <summary>
        /// Contains the path infos
        /// </summary>
        public static IDictionary MulPath { get { return m_MulPath; } set { m_MulPath = value; } }
        /// <summary>
        /// Gets a list of paths to the Client's data files.
        /// </summary>
        public static string Directory { get { return m_Directory; } }

        private static string[] m_Files = new string[]
		{
			"anim.idx",
			"anim.mul",
			"anim2.idx",
			"anim2.mul",
			"anim3.idx",
			"anim3.mul",
			"anim4.idx",
			"anim4.mul",
            "anim5.idx",
			"anim5.mul",
            "animdata.mul",
			"art.mul",
			"artidx.mul",
			"body.def",
			"bodyconv.def",
            "cliloc.deu",
            "cliloc.enu",
            "equipconv.def",
            "fonts.mul",
            "gump.def",
			"gumpart.mul",
			"gumpidx.mul",
			"hues.mul",
            "light.mul",
            "lightidx.mul",
			"map0.mul",
            "map1.mul",
			"map2.mul",
			"map3.mul",
			"map4.mul",
			"mapdif0.mul",
			"mapdif1.mul",
			"mapdif2.mul",
			"mapdif3.mul",
			"mapdif4.mul",
			"mapdifl0.mul",
			"mapdifl1.mul",
			"mapdifl2.mul",
			"mapdifl3.mul",
			"mapdifl4.mul",
            "multi.idx",
            "multi.mul",
            "multimap.rle",
			"radarcol.mul",
            "skills.idx",
            "skills.mul",
            "sound.def",
            "sound.mul",
            "soundidx.mul",
            "speech.mul",
			"stadif0.mul",
			"stadif1.mul",
			"stadif2.mul",
			"stadif3.mul",
			"stadif4.mul",
			"stadifi0.mul",
			"stadifi1.mul",
			"stadifi2.mul",
			"stadifi3.mul",
			"stadifi4.mul",
			"stadifl0.mul",
			"stadifl1.mul",
			"stadifl2.mul",
			"stadifl3.mul",
			"stadifl4.mul",
			"staidx0.mul",
            "staidx1.mul",
			"staidx2.mul",
			"staidx3.mul",
			"staidx4.mul",
			"statics0.mul",
            "statics1.mul",
			"statics2.mul",
			"statics3.mul",
			"statics4.mul",
            "texidx.mul",
            "texmaps.mul",
            "tiledata.mul",
            "unifont.mul",
            "unifont1.mul",
            "unifont2.mul",
            "unifont3.mul",
            "unifont4.mul",
            "unifont5.mul",
            "unifont6.mul",
			"verdata.mul"
        };

        static Files()
        {
            m_Directory = LoadDirectory();
            LoadMulPath();
        }

        /// <summary>
        /// Fills <see cref="MulPath"/> with <see cref="Files.Directory"/>
        /// </summary>
        public static void LoadMulPath()
        {
            m_MulPath = new Hashtable();
            string path = Directory;
            if (path == null)
                path = "";
            foreach (string file in m_Files)
            {
                string filePath = Path.Combine(path, file);
                if (File.Exists(filePath))
                    m_MulPath[file] = filePath;
                else
                    m_MulPath[file] = "";
            }
        }

        /// <summary>
        /// ReSets <see cref="MulPath"/> with given path
        /// </summary>
        /// <param name="path"></param>
        public static void SetMulPath(string path)
        {
            foreach (string file in m_Files)
            {
                string filePath = Path.Combine(path, file);
                if (File.Exists(filePath))
                    m_MulPath[file] = filePath;
                else
                    m_MulPath[file] = "";
            }
        }

        /// <summary>
        /// Looks up a given <paramref name="file" /> in <see cref="Files.MulPath"/>
        /// </summary>
        /// <returns>The absolute path to <paramref name="file" /> -or- <c>null</c> if <paramref name="file" /> was not found.</returns>
        public static string GetFilePath(string file)
        {
            //if (MulPath == null)
            //    LoadMulPath();
            if (MulPath.Count > 0)
            {
                string path = MulPath[file.ToLower()].ToString();
                if (File.Exists(path))
                    return path;
            }
            return null;
        }

        internal static string GetFilePath(string format, params object[] args)
        {
            return GetFilePath(String.Format(format, args));
        }

        private static string LoadDirectory()
        {
            string dir;

            dir = GetExePath(@"SOFTWARE\Origin Worlds Online\Ultima Online\1.0");
            if (dir != null)
                return dir;
            dir = GetExePath(@"SOFTWARE\Origin Worlds Online\Ultima Online Third Dawn\1.0");
            if (dir != null)
                return dir;
            dir = GetInstallPath(@"SOFTWARE\EA GAMES\Ultima Online Samurai Empire");
            if (dir != null)
                return dir;

            dir = GetExePath(@"SOFTWARE\Wow6432Node\Origin Worlds Online\Ultima Online\1.0");
            if (dir != null)
                return dir;
            dir = GetExePath(@"SOFTWARE\Wow6432Node\Origin Worlds Online\Ultima Online Third Dawn\1.0");
            if (dir != null)
                return dir;
            dir = GetInstallPath(@"SOFTWARE\Wow6432Node\EA GAMES\Ultima Online Samurai Empire");

            return dir;
        }

        private static string GetExePath(string regkey)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(regkey))
                {
                    if (key == null)
                        return null;

                    string v = key.GetValue("ExePath") as string;

                    if (v == null || v.Length <= 0 || !File.Exists(v))
                        return null;

                    v = Path.GetDirectoryName(v);

                    if (v == null)
                        return null;

                    return v;
                }
            }
            catch
            {
                return null;
            }
        }

        private static string GetInstallPath(string regkey)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(regkey))
                {
                    if (key == null)
                        return null;

                    string v = key.GetValue("Install Dir") as string;

                    if (v == null || v.Length <= 0)
                        return null;

                    string file = Path.Combine(v, "client.exe");

                    if (!File.Exists(file))
                        return null;

                    return v;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Compares given MD5 hash with hash of given file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static bool CompareMD5(string file, string hash)
        {
            if (file == null)
                return false;
            System.IO.FileStream FileCheck = System.IO.File.OpenRead(file);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] md5Hash = md5.ComputeHash(FileCheck);
            FileCheck.Close();
            string md5string = BitConverter.ToString(md5Hash).Replace("-", "").ToLower();
            if (md5string == hash)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns MD5 hash from given file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static byte[] GetMD5(string file)
        {
            if (file == null)
                return null;
            System.IO.FileStream FileCheck = System.IO.File.OpenRead(file);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] md5Hash = md5.ComputeHash(FileCheck);
            FileCheck.Close();
            return md5Hash;
        }

        /// <summary>
        /// Compares MD5 hash from given mul file with hash in responsible hash-file
        /// </summary>
        /// <param name="what"></param>
        /// <returns></returns>
        public static bool CompareHashFile(string what)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(path, String.Format("UOViewer{0}.hash", what));
            if (File.Exists(FileName))
            {
                try
                {
                    using (BinaryReader bin = new BinaryReader(new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read)))
                    {
                        int length = bin.ReadInt32();
                        byte[] buffer = new byte[length];
                        bin.Read(buffer, 0, length);
                        string hashold = BitConverter.ToString(buffer).Replace("-", "").ToLower();
                        return Files.CompareMD5(Files.GetFilePath(String.Format("{0}.mul", what)), hashold);
                    }
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
    }
}
