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

/*
 * Modified by David Gardner on 2018-12-05 to add additional functionality.
 * 
 * - Added TextEntryLimited for both the barebones style and distro pkg style
 * - Added support for gumppictiled for the distro pkg style
 * - Fixed an invalid placeholder warning in: Gump_WriteGumpPic(ImageElement elem)
 * - Modified the html gump element for distro pkg style output to place the boolean values of showbackground 
 *   and scroll bar regardless of whether they are true or false, probably a personal preference.
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
       private MenuItem _MenuFileExport;
       protected POLExportForm frm_POLExportForm;

       private bool bGetDefaultText = false;

        public override PluginInfo Info { get; } = new PluginInfo("POLGumpExporter", "1.2", "POLServer team", "team@polserver.com", "Exports the Gump into a POL script. Based on Sphere Exporter by Francesco Furiani & Mark Chandler.");
        private readonly Settings _Config = new Settings();
        public override BaseConfig Config => _Config;

        [Serializable]
        public class Settings : BaseConfig
        {
            public override string Name => "POLGumpExporter";
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();

            Designer.MenuFileExport.Enabled = true;

            if (_MenuFileExport == null)
            {
                _MenuFileExport = new MenuItem(Info.Name, mnu_FileExportPOLExport_Click);
            }

            Designer.MenuFileExport.MenuItems.Add(_MenuFileExport);
        }

        protected override void OnUnloaded()
        {
            base.OnUnloaded();

            Designer.MenuFileExport.MenuItems.Remove(_MenuFileExport);

            if (Designer.MenuFileExport.MenuItems.Count == 0)
            {
                Designer.MenuFileExport.Enabled = false;
            }
        }

       private void mnu_FileExportPOLExport_Click(object sender, EventArgs e)
       {
           frm_POLExportForm = new POLExportForm(this);
           frm_POLExportForm.ShowDialog();
       }

       public StringWriter GetPOLScript(bool bUsedistro)
       {
           return GetPOLScript(bUsedistro, true, true, true);
       }

       public StringWriter GetPOLScript(bool bUseDistro, bool bShowComment, bool bShowNames, bool bDefaultTexts)
       {
           if (bUseDistro) return CreateDistroScript(bShowComment, bShowNames, bDefaultTexts);
           else return CreateBareScript();
       }

       private string GetCommentString(BaseElement be_elem, bool bShowComments, bool bShowNames)
       {
           if (be_elem == null)
               return "";

           string elem_comment = "";
           string elem_name = "";
           string comment = "";

           if (be_elem.Comment != null)
                  elem_comment = be_elem.Comment;

           if (bShowNames) {
               if (be_elem.Name != null)
                   elem_name = be_elem.Name;

               comment = be_elem.Name  + ((elem_comment != String.Empty && bShowComments) ? ": " : "");
           }
           if (bShowComments)
               comment += elem_comment;

           return "// " + comment;
       }

       private string GetGumpName()
       {
           string gump_name = frm_POLExportForm.GumpName;
           string default_name = "gump";
           string[] words = null;

           if (gump_name != null)
               if (gump_name != String.Empty)
                    words = gump_name.Split(new char[]{' '},StringSplitOptions.RemoveEmptyEntries);

           if (words == null)
               return default_name;

           if (words.Length > 0)
               return words[0];

           return default_name;
       }

       private StringWriter CreateDistroScript(bool bShowComment, bool bShowNames, bool bDefaultTexts)
       {
           StringWriter sw_Script = new StringWriter();
           List<string> GumpCommands = new List<string>();

           bGetDefaultText = bDefaultTexts; // define if plugin will set default text for empty elems

           string gump_name = GetGumpName();

           GumpCommands.Add(DistroGump_GFCreateGump(gump_name, Designer.GumpProperties.Location));
           GumpCommands.Add("");
           if (!Designer.GumpProperties.Moveable)
               GumpCommands.Add(String.Format("GFMovable( {0}, 0 );", gump_name));

           if (!Designer.GumpProperties.Closeable)
               GumpCommands.Add(String.Format("GFClosable( {0}, 0 );", gump_name));

           if (!Designer.GumpProperties.Disposeable)
               GumpCommands.Add(String.Format("GFDisposable( {0}, 0 );", gump_name));


           if (Designer.Stacks.Count > 0)
           {
               int radiogroup = -1;
               int pageindex = 0;
               // =================

               foreach (GroupElement ge_Elements in Designer.Stacks)
               {
                   if (pageindex > 0)
                       GumpCommands.Add("");

                   GumpCommands.Add(DistroGump_GFPage(gump_name, ref pageindex)); // "page pageid"
                   if (ge_Elements == null)
                       continue;

                   foreach (BaseElement be_Element in ge_Elements.GetElementsRecursive())
                   {
                       if (be_Element == null)
                           continue;

                       if (bShowComment || bShowNames)
                       {
                           string comment = GetCommentString(be_Element, bShowComment, bShowNames);
                           if (comment != String.Empty)
                           {
                               GumpCommands.Add("");
                               GumpCommands.Add(comment);
                           }
                       }

                       Type ElementType = be_Element.GetType();

                       if (ElementType == typeof(HTMLElement))
                       {
                           HTMLElement elem = be_Element as HTMLElement;
                           if (elem.TextType == HTMLElementType.HTML)
                               GumpCommands.Add(DistroGump_GFHTMLArea(gump_name, elem));
                           else
                               GumpCommands.Add(DistroGump_GFAddHTMLLocalized(gump_name, elem));
                       }
                       else if (ElementType == typeof(TextEntryElement))
                       {
                           TextEntryElement elem = be_Element as TextEntryElement;
                           GumpCommands.Add(DistroGump_GFTextEntry(gump_name, elem));
                       }
                       else if (ElementType == typeof(LabelElement))
                       {
                           LabelElement elem = be_Element as LabelElement;
                           GumpCommands.Add(DistroGump_GFTextLine(gump_name, elem));
                       }

                       else if (ElementType == typeof(AlphaElement))
                       {
                           AlphaElement elem = be_Element as AlphaElement;
                           GumpCommands.Add(DistroGump_GFAddAlphaRegion(gump_name, elem));
                       }
                       else if (ElementType == typeof(BackgroundElement))
                       {
                           BackgroundElement elem = be_Element as BackgroundElement;
                           GumpCommands.Add(DistroGump_GFResizePic(gump_name, elem));
                       }
                       else if (ElementType == typeof(ImageElement))
                       {
                           ImageElement elem = be_Element as ImageElement;
                           GumpCommands.Add(DistroGump_GFGumpPic(gump_name, elem));
                       }
                       else if (ElementType == typeof(ItemElement))
                       {
                           ItemElement elem = be_Element as ItemElement;
                           GumpCommands.Add(DistroGump_GFTilePic(gump_name, elem));
                       }
                       else if (ElementType == typeof(TiledElement))
                       {
                           TiledElement elem = be_Element as TiledElement;
                           GumpCommands.Add(DistroGump_GFPicTiled(gump_name, elem));
                       }

                       else if (ElementType == typeof(ButtonElement))
                       {
                           ButtonElement elem = be_Element as ButtonElement;
                           GumpCommands.Add(DistroGump_GFAddButton(gump_name, elem));
                       }

                       else if (ElementType == typeof(CheckboxElement))
                       {
                           CheckboxElement elem = be_Element as CheckboxElement;
                           GumpCommands.Add(DistroGump_GFCheckBox(gump_name, elem));
                       }
                       else if (ElementType == typeof(RadioElement))
                       {
                           RadioElement elem = be_Element as RadioElement;
                           if (elem.Group != radiogroup)
                           {
                               GumpCommands.Add(String.Format("GFSetRadioGroup( {0}, {1} );", gump_name, elem.Group));
                               radiogroup = elem.Group;
                           }
                           GumpCommands.Add(DistroGump_GFRadioButton(gump_name, elem));
                       }
                   }
               }
           }


           sw_Script.WriteLine("// Created {0}, with Gump Studio.", DateTime.Now);
           sw_Script.WriteLine("// Exported with {0} ver {1} for gump pkg", Info.Name, Info.Version);
           sw_Script.WriteLine();
           sw_Script.WriteLine("use uo;");
           sw_Script.WriteLine("use os;");
           sw_Script.WriteLine();
           sw_Script.WriteLine("include \":gumps:gumps\";");
           sw_Script.WriteLine();
           sw_Script.WriteLine("program gump_{0}( who )", gump_name);
           sw_Script.WriteLine();

           foreach (string cmd in GumpCommands)
           {
               sw_Script.WriteLine("\t"+cmd);
           }
           sw_Script.WriteLine();
           sw_Script.WriteLine("\tGFSendGump( who, {0} );", gump_name);
           sw_Script.WriteLine();
           sw_Script.WriteLine("endprogram");


           return sw_Script;
       }

       #region Distro pkg commands
       private string DistroGump_GFRadioButton(string gump_name, RadioElement elem)
       {
           return String.Format("GFRadioButton( {0}, {1}, {2}, {3}, {4}, {5}, {6} );", gump_name, elem.X, elem.Y, elem.UnCheckedID, elem.CheckedID, BoolToString(elem.Checked), elem.Value);
       }

       private string DistroGump_GFCheckBox(string gump_name, CheckboxElement elem)
       {
           return String.Format("GFCheckBox( {0}, {1}, {2}, {3}, {4}, {5}, {6} );", gump_name, elem.X, elem.Y, elem.UnCheckedID, elem.CheckedID, BoolToString(elem.Checked), elem.Group);
       }

       private string DistroGump_GFAddButton(string gump_name, ButtonElement elem)
       {
           string btn_type = "";
           if (elem.ButtonType == ButtonTypeEnum.Page)
               btn_type = "GF_PAGE_BTN";
           else
               btn_type = "GF_CLOSE_BTN";

           return String.Format("GFAddButton( {0}, {1}, {2}, {3}, {4}, {5}, {6} );", gump_name, elem.X, elem.Y, elem.NormalID, elem.PressedID, btn_type, elem.Param);
       }

       private string DistroGump_GFTilePic(string gump_name, ItemElement elem)
       {
           string hue = (elem.Hue != null) ? elem.Hue.Index.ToString() : "0";
           return String.Format("GFTilePic( {0}, {1}, {2}, {3}, {4} );", gump_name, elem.X, elem.Y, elem.ItemID, hue);
       }

       private string DistroGump_GFPicTiled(string gump_name, TiledElement elem)
       {
           return String.Format("GFPicTiled( {0}, {1}, {2}, {3}, {4}, {5} );", gump_name, elem.X, elem.Y, elem.Width, elem.Height, elem.GumpID);
       }

       private string DistroGump_GFGumpPic(string gump_name, ImageElement elem)
       {
           string hue = (elem.Hue != null) ? elem.Hue.Index.ToString() : "0";
           return String.Format("GFGumpPic( {0}, {1}, {2}, {3}, {4} );", gump_name, elem.X, elem.Y, elem.GumpID, hue);
       }

       private string DistroGump_GFResizePic(string gump_name, BackgroundElement elem)
       {
           return String.Format("GFResizePic( {0}, {1}, {2}, {3}, {4}, {5} );", gump_name, elem.X, elem.Y, elem.GumpID, elem.Width, elem.Height);
       }

       private string DistroGump_GFAddAlphaRegion(string gump_name, AlphaElement elem)
       {
           return String.Format("GFAddAlphaRegion( {0}, {1}, {2}, {3}, {4} );", gump_name, elem.X, elem.Y, elem.Width, elem.Height);
       }

       private string DistroGump_GFTextLine(string gump_name, LabelElement elem)
       {
           string hue = (elem.Hue != null) ? elem.Hue.Index.ToString() : "0";

           string text = (bGetDefaultText) ? "TextLine" : "";
           if (elem.Text != null)
               if (elem.Text != String.Empty)
                   text = elem.Text;

           return String.Format("GFTextLine( {0}, {1}, {2}, {3}, \"{4}\" );", gump_name, elem.X, elem.Y, hue, text);
       }

       private string DistroGump_GFTextEntry(string gump_name, TextEntryElement elem)
       {
           string text = (bGetDefaultText) ? "TextEntry" : "";
           if (elem.InitialText != null)
               if (elem.InitialText != String.Empty)
                   text = elem.InitialText;

           string hue = (elem.Hue != null) ? elem.Hue.Index.ToString() : "0";
           if (elem.MaxLength > 0)
               return String.Format("GFTextEntry( {0}, {1}, {2}, {3}, {4}, {5}, \"{6}\", {7}, {8} );", gump_name, elem.X, elem.Y, elem.Width, elem.Height, hue, text, elem.ID, elem.MaxLength);
           else
               return String.Format("GFTextEntry( {0}, {1}, {2}, {3}, {4}, {5}, \"{6}\", {7} );", gump_name, elem.X, elem.Y, elem.Width, elem.Height, hue, text, elem.ID);

       }

       private string DistroGump_GFAddHTMLLocalized(string gump_name, HTMLElement elem)
       {
            //TODO: What about allowing Hue to be placed in HTML when using cliloc?
            if (elem.ShowScrollbar || elem.ShowBackground)
                return String.Format("GFAddHTMLLocalized( {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7} );", gump_name, elem.X, elem.Y, elem.Width, elem.Height, elem.CliLocID, BoolToString(elem.ShowBackground), BoolToString(elem.ShowScrollbar));
            else
                return String.Format("GFAddHTMLLocalized( {0}, {1}, {2}, {3}, {4}, {5}, 0, 0 );", gump_name, elem.X, elem.Y, elem.Width, elem.Height, elem.CliLocID);
       }

        private string DistroGump_GFHTMLArea(string gump_name, HTMLElement elem)
        {
            string text = (bGetDefaultText) ? "HtmlElement" : "";

            if (elem.HTML != null)
                if (elem.HTML != String.Empty)
                    text = elem.HTML;

            if (elem.ShowScrollbar || elem.ShowBackground)
                return String.Format("GFHTMLArea( {0}, {1}, {2}, {3}, {4}, \"{5}\", {6}, {7} );", gump_name, elem.X, elem.Y, elem.Width, elem.Height, text, BoolToString(elem.ShowBackground), BoolToString(elem.ShowScrollbar));
            else
                return String.Format("GFHTMLArea( {0}, {1}, {2}, {3}, {4}, \"{5}\", 0, 0 );", gump_name, elem.X, elem.Y, elem.Width, elem.Height, text);
        }

        private string DistroGump_GFPage(string gump_name, ref int pageindex)
       {
           return String.Format("GFPage( {0}, {1} );", gump_name, pageindex++);
       }

       string DistroGump_GFCreateGump(string gump_name, System.Drawing.Point loc)
       {
           if (loc.X != 0 || loc.Y != 0)
               return String.Format("var {0} := GFCreateGump( {1}, {2} );", gump_name, loc.X, loc.Y);
           else
               return String.Format("var {0} := GFCreateGump( 0, 0 );", gump_name);

       }
       #endregion

       private StringWriter CreateBareScript()
       {
           StringWriter sw_Script = new StringWriter();
           //ArrayList al_Buttons = new ArrayList();
           //ArrayList al_Texts = new ArrayList();

           List<string> GumpCommands = new List<string>();
           List<string> GumpTexts = new List<string>();


           sw_Script.WriteLine("// Created {0}, with Gump Studio.", DateTime.Now);
           sw_Script.WriteLine("// Exported with {0} ver {1}.", Info.Name, Info.Version);
           sw_Script.WriteLine("");

           if (!Designer.GumpProperties.Moveable)
               GumpCommands.Add("NoMove");

           if (!Designer.GumpProperties.Closeable)
               GumpCommands.Add("NoClose");

           if (!Designer.GumpProperties.Disposeable)
               GumpCommands.Add("NoDispose");

           if (Designer.Stacks.Count > 0)
           {
               int radiogroup = -1;
               int pageindex = 0;
               // =================

               foreach (GroupElement ge_Elements in Designer.Stacks)
               {
                   GumpCommands.Add(Gump_WritePage(ref pageindex)); // "page pageid"
                   if (ge_Elements == null)
                       continue;

                   foreach (BaseElement be_Element in ge_Elements.GetElementsRecursive())
                   {
                       if (be_Element == null)
                           continue;

                       Type ElementType = be_Element.GetType();

                       if (ElementType == typeof(HTMLElement))
                       {
                           HTMLElement elem = be_Element as HTMLElement;
                           if (elem.TextType == HTMLElementType.HTML)
                           {
                               string cmd = Gump_WriteHTMLGump(elem, ref GumpTexts);
                               GumpCommands.Add(cmd);
                           }
                           else
                           {
                               string cmd = Gump_WriteXMFHtmlGump(elem);
                               GumpCommands.Add(cmd);
                           }
                       }
                       else if (ElementType == typeof(TextEntryElement))
                       {
                           TextEntryElement elem = be_Element as TextEntryElement;
                           string cmd = Gump_WriteTextEntry(elem, ref GumpTexts);
                           GumpCommands.Add(cmd);
                       }
                       else if (ElementType == typeof(LabelElement))
                       {
                           LabelElement elem = be_Element as LabelElement;
                           string cmd = Gump_WriteText(elem, ref GumpTexts);
                           GumpCommands.Add(cmd);
                       }

                       else if (ElementType == typeof(AlphaElement))
                       {
                           AlphaElement elem = be_Element as AlphaElement;
                           string cmd = Gump_WriteCheckerTrans(elem);
                           GumpCommands.Add(cmd);
                       }
                       else if (ElementType == typeof(BackgroundElement))
                       {
                           BackgroundElement elem = be_Element as BackgroundElement;
                           string cmd = Gump_WriteResizePic(elem);
                           GumpCommands.Add(cmd);
                       }
                       else if (ElementType == typeof(ImageElement))
                       {
                           ImageElement elem = be_Element as ImageElement;
                           string cmd = Gump_WriteGumpPic(elem);
                           GumpCommands.Add(cmd);
                       }
                       else if (ElementType == typeof(ItemElement))
                       {
                           ItemElement elem = be_Element as ItemElement;
                           string cmd = Gump_WriteTilePic(elem);
                           GumpCommands.Add(cmd);
                       }
                       else if (ElementType == typeof(TiledElement))
                       {
                           TiledElement elem = be_Element as TiledElement;
                           string cmd = Gump_WriteGumpPicTiled(elem);
                           GumpCommands.Add(cmd);
                       }

                       else if (ElementType == typeof(ButtonElement))
                       {
                           ButtonElement elem = be_Element as ButtonElement;
                           string cmd = Gump_WriteButton(elem);
                           GumpCommands.Add(cmd);
                       }

                       else if (ElementType == typeof(CheckboxElement))
                       {
                           CheckboxElement elem = be_Element as CheckboxElement;
                           string cmd = Gump_WriteCheckBox(elem);
                           GumpCommands.Add(cmd);
                       }
                       else if (ElementType == typeof(RadioElement))
                       {
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
           sw_Script.WriteLine("program gump_{0}( who )", frm_POLExportForm.GumpName);
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
               sw_Script.Write("\t\t\"{0}\"", tmpSe);
               if (i == GumpTexts.Count)
                   sw_Script.WriteLine("");
               else
                   sw_Script.WriteLine(",");
               i++;
           }
           sw_Script.WriteLine("\t};");
           sw_Script.WriteLine("");
           sw_Script.WriteLine("\tSendDialogGump( who, gump, data{0} );", Gump_Location(Designer.GumpProperties.Location));
           sw_Script.WriteLine("");
           sw_Script.WriteLine("endprogram");
           return sw_Script;
       }

       #region Bare Gump Commands
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
           if (IsHued(elem.Hue.Index.ToString()))
               return string.Format("tilepichue {0} {1} {2} {3}", elem.X, elem.Y, elem.ItemID, elem.Hue.Index);
           else
               return String.Format("tilepic {0} {1} {2}", elem.X, elem.Y, elem.ItemID);
       }

       private string Gump_WriteGumpPic(ImageElement elem)
       {
           if (IsHued(elem.Hue.Index.ToString()))
           {
               return String.Format("gumppic {0} {1} {2} {3}", elem.X, elem.Y, elem.GumpID, elem.Hue.Index);
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

           return String.Format("text {0} {1} {2} {3}", elem.X, elem.Y, elem.Hue.Index, textid);
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
           if (elem.MaxLength > 0)
               return String.Format("textentrylimited {0} {1} {2} {3} {4} {5} {6} {7}", elem.X, elem.Y, elem.Width, elem.Height, elem.Hue.Index, elem.ID, textid, elem.MaxLength);
           else
               return String.Format("textentry {0} {1} {2} {3} {4} {5} {6}", elem.X, elem.Y, elem.Width, elem.Height, elem.Hue.Index, elem.ID, textid);

       }
       #endregion

       #region Helper commands
       string Gump_Location(System.Drawing.Point loc)
       {
           if (loc.X == 0 && loc.Y == 0)
               return "";

           return String.Format(", {0}, {1}", loc.X, loc.Y);
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
       #endregion
   }
}
