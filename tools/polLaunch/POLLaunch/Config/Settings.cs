using System;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace POLLaunch
{
	class Settings
	{
		const string filename = "POLLaunch.cfg";
		protected string uo_path = "";

		public bool LoadConfiguration()
		{
			if (!File.Exists(filename))
				return false;

			try
			{
				TextReader tr = new StreamReader(filename);
				string line = tr.ReadLine();
				while (line != null)
				{
					string[] pair = line.Split(new char[] { '=' }, 2);
					pair[0] = pair[0].ToLower();
					pair[1] = pair[1].TrimEnd(new char[] { ' ', '\t', '\n', '\r' });
					switch (pair[0])
					{
						case "uopath": this.uo_path = pair[1]; break;
					}
					line = tr.ReadLine();
				}
				tr.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "ERROR");
			}

			return true;
		}

		public bool SaveConfiguration()
		{
			if (File.Exists(filename))
				File.Delete(filename);
			try
			{
				TextWriter tw = new StreamWriter(filename);
				tw.WriteLine("UOPath=" + this.uo_path);
				
				tw.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error");
			}
			return true;
		}
	}
	/*
	static class XMLSettings
	{
		const string filename = "POLLaunch.xml";

		static public void TestXMLFile()
		{
			XmlTextReader xml_tr = new XmlTextReader(filename);
			string output = "";
			while (xml_tr.Read())
			{
				switch ( xml_tr.NodeType )
				{
					case XmlNodeType.Element: // The node is an element.
						output += "<" + xml_tr.Name + ">" + Environment.NewLine;
						break;
					case XmlNodeType.Text: //Display the text in each element.
						output += xml_tr.Value + Environment.NewLine;
						break;
					case XmlNodeType.EndElement: //Display the end of the element.
						output += "</" + xml_tr.Name + ">" + Environment.NewLine;
						break;
				}
			}
			MessageBox.Show(output);
		}

		static public void WriteXML()
		{
			try
			{
				XmlDocument xml_doc = new XmlDocument();
				try
				{
					xml_doc.Load(filename);
				}
				catch (System.IO.FileNotFoundException)
				{
					//if file is not found, create a new xml file
					XmlTextWriter xml_writer = new XmlTextWriter(filename, System.Text.Encoding.UTF8);
					xml_writer.Formatting = Formatting.Indented;
					xml_writer.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
					xml_writer.WriteStartElement("Root");
					xml_writer.Close();
					xml_doc.Load(filename);
				}

				XmlNode root = xml_doc.DocumentElement;
				XmlElement child_node = xml_doc.CreateElement("childNode");
				XmlElement child_node2 = xml_doc.CreateElement("SecondChildNode");
				XmlText text_node = xml_doc.CreateTextNode("hello");
				text_node.Value = "hello, world";

				root.AppendChild(child_node);
				child_node.AppendChild(child_node2);
				child_node2.SetAttribute("Name", "Value");
				child_node2.AppendChild(text_node);

				text_node.Value = "replacing hello world";
				xml_doc.Save(filename);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}
	}
	*/
}