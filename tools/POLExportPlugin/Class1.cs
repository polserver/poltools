/* 	Copyright (c) 2004-2006 Francesco Furiani & Mark Chandler
 *	
 *	Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation 
 *	files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, 
 *	modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software 
 *	is furnished to do so, subject to the following conditions:
 *
 *	The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 *
 *	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 *	OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE 
 *	LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR 
 *	IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

/*
 * Modified by Fernando Rozenblit in 2008 to create POL Scripts.
 * 
 * Rewrote some parts of Class1.cs for a better reading / updating
 * 
 * TODO: Add distro pkg support
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using GumpStudio;
using GumpStudio.Elements;
using GumpStudio.Plugins;
using Ultima;

namespace POLGumpExport
{
	public class POLExporter : BasePlugin
	{
		protected DesignerForm m_Designer;
		protected MenuItem mnu_FileExportPOLExport;
		protected MenuItem mnu_MenuItem;
		protected POLExportForm frm_POLExportForm;

		public override string Name
		{
			get
			{
				return this.GetPluginInfo().PluginName;
			}
		}

		public override PluginInfo GetPluginInfo()
		{
			PluginInfo pluginInfo = new PluginInfo();
			pluginInfo.AuthorEmail = "rozenblit@gmail.com";
			pluginInfo.AuthorName = "Fernando Rozenblit";
            pluginInfo.Description = "Exports the Gump into a POL script. Based on Sphere Exporter by Francesco Furiani & Mark Chandler.";
			pluginInfo.PluginName = "POLGumpExporter";
			pluginInfo.Version = "1.0";
			return pluginInfo;
		}

		public override void Load(DesignerForm frmDesigner)
		{
			this.m_Designer = frmDesigner;
			this.mnu_FileExportPOLExport = new MenuItem("POL");
			this.mnu_FileExportPOLExport.Click += new EventHandler(this.mnu_FileExportPOLExport_Click);
			if ( !this.m_Designer.mnuFileExport.Enabled )
			{
				this.m_Designer.mnuFileExport.Enabled = true;
			}
			this.m_Designer.mnuFileExport.MenuItems.Add(this.mnu_FileExportPOLExport);
			base.Load(frmDesigner);
		}

		private void mnu_FileExportPOLExport_Click(object sender, EventArgs e)
		{
			frm_POLExportForm = new POLExportForm(this);
			frm_POLExportForm.ShowDialog();
		}

		public StringWriter GetPOLScript(bool bIsRevision)
		{
			StringWriter sw_Script = new StringWriter();
			//ArrayList al_Buttons = new ArrayList();
			//ArrayList al_Texts = new ArrayList();
            
            List<string> GumpCommands = new List<string>();
            List<string> GumpTexts = new List<string>();
           

			sw_Script.WriteLine("// Created {0}, with Gump Studio.", DateTime.Now);
			sw_Script.WriteLine("// Exported with {0} ver {1}.", this.GetPluginInfo().PluginName, this.GetPluginInfo().Version);
		//	sw_Script.WriteLine("// Script for {0}", bIsRevision ? "0.55R*/0.56/0.57" : "0.99/1.0");
			sw_Script.WriteLine("");
		//	sw_Script.WriteLine("[DIALOG {0}]", frm_POLExportForm.GumpName);
		//	sw_Script.WriteLine("{0}", Gump_WriteLocation(bIsRevision, m_Designer.GumpProperties.Location.X, m_Designer.GumpProperties.Location.Y));

            if (!m_Designer.GumpProperties.Moveable)
                GumpCommands.Add("NoMove");

            if (!m_Designer.GumpProperties.Closeable)
                GumpCommands.Add("NoClose");
						
			if ( !m_Designer.GumpProperties.Disposeable )
				GumpCommands.Add("NoDispose");

			if ( m_Designer.Stacks.Count > 0 )
			{
				int radiogroup = -1;
                int pageindex = 0;
				// =================
               
				foreach(GroupElement ge_Elements in m_Designer.Stacks)
				{
                    GumpCommands.Add(Gump_WritePage(ref pageindex)); // "page pageid"
                    if (ge_Elements == null)
                        continue;

                    foreach (BaseElement be_Element in ge_Elements.GetElementsRecursive())
                    {
                        if (be_Element == null)
                            continue;

                        Type ElementType = be_Element.GetType();

                        if (ElementType == typeof(HTMLElement)) {
                            HTMLElement elem = be_Element as HTMLElement;
                            if (elem.TextType == HTMLElementType.HTML) {
                                string cmd = Gump_WriteHTMLGump(elem, ref GumpTexts);
                                GumpCommands.Add(cmd);
                            }
                            else {
                                string cmd = Gump_WriteXMFHtmlGump(elem);
                                GumpCommands.Add(cmd);
                            }
                        }
                        else if (ElementType == typeof(TextEntryElement)) {
                            TextEntryElement elem = be_Element as TextEntryElement;
                            string cmd = Gump_WriteTextEntry(elem, ref GumpTexts);
                            GumpCommands.Add(cmd);
                        }
                        else if (ElementType == typeof(LabelElement)) {
                            LabelElement elem = be_Element as LabelElement;
                            string cmd = Gump_WriteText(elem, ref GumpTexts);
                            GumpCommands.Add(cmd);
                        }

                        else if (ElementType == typeof(AlphaElement)) {
                            AlphaElement elem = be_Element as AlphaElement;
                            string cmd = Gump_WriteCheckerTrans(elem);
                            GumpCommands.Add(cmd);
                        }
                        else if (ElementType == typeof(BackgroundElement)) {
                            BackgroundElement elem = be_Element as BackgroundElement;
                            string cmd = Gump_WriteResizePic(elem);
                            GumpCommands.Add(cmd);
                        }
                        else if (ElementType == typeof(ImageElement)) {
                            ImageElement elem = be_Element as ImageElement;
                            string cmd = Gump_WriteGumpPic(elem);
                            GumpCommands.Add(cmd);
                        }
                        else if (ElementType == typeof(ItemElement)) {
                            ItemElement elem = be_Element as ItemElement;
                            string cmd = Gump_WriteTilePic(elem);
                            GumpCommands.Add(cmd);
                        }
                        else if (ElementType == typeof(TiledElement)) {
                            TiledElement elem = be_Element as TiledElement;
                            string cmd = Gump_WriteGumpPicTiled(elem);
                            GumpCommands.Add(cmd);
                        }

                        else if (ElementType == typeof(ButtonElement)) {
                            ButtonElement elem = be_Element as ButtonElement;
                            string cmd = Gump_WriteButton(elem);
                            GumpCommands.Add(cmd);
                        }

                        else if (ElementType == typeof(CheckboxElement)) {
                            CheckboxElement elem = be_Element as CheckboxElement;
                            string cmd = Gump_WriteCheckBox(elem);
                            GumpCommands.Add(cmd);
                        }
                        else if (ElementType == typeof(RadioElement)) {
                            RadioElement elem = be_Element as RadioElement;
                            if (elem.Group != radiogroup)
                            {
                                GumpCommands.Add("group " + elem.Group);
                                radiogroup = elem.Group;
                            }
                            string cmd = Gump_WriteRadioBox(elem);
                            GumpCommands.Add(cmd);
                        }
                    }
                }
			}
			sw_Script.WriteLine("");
            sw_Script.WriteLine("use uo;");
            sw_Script.WriteLine("use os;");
            sw_Script.WriteLine("");
            sw_Script.WriteLine("program gump_{0}(who)", frm_POLExportForm.GumpName);
            sw_Script.WriteLine("");

            sw_Script.WriteLine("\tvar gump := array {");

            int i = 1;
            foreach (string tmpCmd in GumpCommands)
            {
                sw_Script.Write("\t\t\"{0}\"", tmpCmd);

                if (i == GumpCommands.Count)
                    sw_Script.WriteLine("");
                else
                    sw_Script.WriteLine(",", tmpCmd);
                i++;
            }
            sw_Script.WriteLine("\t};");

            sw_Script.WriteLine("\tvar data := array {");

            i = 1;
            foreach (string tmpSe in GumpTexts)
			{
				sw_Script.Write("\t\t\"{0}\"",tmpSe);
                if (i == GumpTexts.Count)
                    sw_Script.WriteLine("");
                else
                    sw_Script.WriteLine(",");
                i++;
			}
            sw_Script.WriteLine("\t};");
            sw_Script.WriteLine("");
            sw_Script.WriteLine("\tSendDialogGump(who, gump, data{0});", Gump_Location(m_Designer.GumpProperties.Location));
            sw_Script.WriteLine("");
            sw_Script.WriteLine("endprogram");
			return sw_Script;
		}

        private string Gump_WriteRadioBox(RadioElement elem)
        {
            return String.Format("radio {0} {1} {2} {3} {4} {5}", elem.X, elem.Y, elem.UnCheckedID, elem.CheckedID, BoolToString(elem.Checked), elem.Value);
        }

        private string Gump_WriteButton(ButtonElement elem)
        {
            bool quit = true;
            string page_id = "0";
            string ret_value = elem.Param.ToString();

            if (elem.ButtonType == ButtonTypeEnum.Page)
            {
                quit = false;
                page_id = elem.Param.ToString();
                ret_value = "0";
            }
            
            return string.Format("button {0} {1} {2} {3} {4} {5} {6}", elem.X, elem.Y, elem.NormalID, elem.PressedID, BoolToString(quit), page_id, ret_value);
        }

        private string Gump_WriteCheckBox(CheckboxElement elem)
        {
            return string.Format("checkbox {0} {1} {2} {3} {4} {5}", elem.X, elem.Y, elem.UnCheckedID, elem.CheckedID, BoolToString(elem.Checked), elem.Group);        
        }

        private string Gump_WriteGumpPicTiled(TiledElement elem)
        {
            return string.Format("gumppictiled {0} {1} {2} {3} {4}", elem.X, elem.Y, elem.Width, elem.Height, elem.GumpID);
        }

        private string Gump_WriteTilePic(ItemElement elem)
        {
            if (IsHued(elem.Hue.ToString()))
            {
                return string.Format("tilepichue {1} {2} {3} {4}", elem.X, elem.Y, elem.ItemID, elem.Hue);
            }
            else return String.Format("tilepic {0} {1} {2}", elem.X, elem.Y, elem.ItemID);
        }

        private string Gump_WriteGumpPic(ImageElement elem)
        {
            if (IsHued(elem.Hue.ToString()))
            {
                return String.Format("gumppic {0} {1} {2} {3}", elem.X, elem.Y, elem.GumpID, elem.Hue);
            }
            else return String.Format("gumppic {0} {1} {2}", elem.X, elem.Y, elem.GumpID);
        }

        private string Gump_WriteResizePic(BackgroundElement elem)
        {
            return String.Format("resizepic {0} {1} {2} {3} {4}", elem.X, elem.Y, elem.GumpID, elem.Width, elem.Height);
        }

        private string Gump_WriteCheckerTrans(AlphaElement elem)
        {
            return String.Format("checkertrans {0} {1} {2} {3}", elem.X, elem.Y, elem.Width, elem.Height);
        }

        private string Gump_WriteText(LabelElement elem, ref List<string> texts)
        {
            int textid = texts.Count;

            string text = String.Concat("Text id.", textid);

            if (elem.Text != null)
            {
                text = (elem.Text == String.Empty) ? text : elem.Text;
            }

            texts.Add(text);

            return String.Format("text {0} {1} {2} {3}", elem.X, elem.Y, elem.Hue, textid);
        }

        string Gump_Location(System.Drawing.Point loc)
        {
            if (loc.X == 0 && loc.Y == 0)
                return "";

            return String.Format(", {0}, {1}", loc.X, loc.Y);
        }

        string Gump_WritePage(ref int pageid)
        {
            return String.Format("page {0}", pageid++);
        }

        string Gump_WriteHTMLGump(HTMLElement elem, ref List<string> texts) {
            int textid = texts.Count;

            string text = String.Concat("HtmlGump id.", textid);

            if (elem.HTML != null) {
                text = (elem.HTML == String.Empty) ? text : elem.HTML;
            }

            texts.Add(text);
            
            return String.Format("htmlgump {0} {1} {2} {3} {4} {5} {6}", elem.X, elem.Y, elem.Width, elem.Height, textid, BoolToString(elem.ShowBackground), BoolToString(elem.ShowScrollbar));    
        }

        string Gump_WriteXMFHtmlGump(HTMLElement elem)
        {
            return String.Format("xmfhtmlgump {0} {1} {2} {3} {4} {5} {6}", elem.X, elem.Y, elem.Width, elem.Height, elem.CliLocID, BoolToString(elem.ShowBackground), BoolToString(elem.ShowScrollbar));
        }

        private string Gump_WriteTextEntry(TextEntryElement elem, ref List<string> texts)
        {
            int textid = texts.Count;

            string text = String.Concat("TextEntry id.", textid);

            if (elem.InitialText != null)
            {
                text = (elem.InitialText == String.Empty) ? text : elem.InitialText;
            }

            texts.Add(text);

            return String.Format("textentry {0} {1} {2} {3} {4} {5} {6}", elem.X, elem.Y, elem.Width, elem.Height, elem.Hue, elem.ID, textid);  
         }

        string BoolToString(bool check)
        {
            return (check) ? "1" : "0";
        }

        bool IsHued(string hue)
        {
            if (hue == null)
                return false;
            if (hue == string.Empty)
                return false;
            if (hue == "0")
                return false;
            return true;
        }
	}
}
