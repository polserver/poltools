/***************************************************************************
 *
 * $Author: MuadDib & Turley
 * 
 * "THE BEER-WARE LICENSE"
 * As long as you retain this notice you can do whatever you want with 
 * this stuff. If we meet some day, and you think this stuff is worth it,
 * you can buy me a beer in return.
 *
 ***************************************************************************/

using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Reflection;

namespace FiddlerPlugin
{
    public partial class MultiEditor : UserControl
    {

        private static MultiEditor refMarkerMulti = null;

        public MultiEditor()
        {
            InitializeComponent();
            refMarkerMulti = this;
            InitializeToolBox();
        }

        private void InitializeToolBox()
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string FileName = Path.Combine(path, @"plugins/multieditor.xml");
            if (!File.Exists(FileName))
                return;

            XmlDocument dom = new XmlDocument();
            dom.Load(FileName);
            XmlElement xTiles = dom["TileGroups"];

            foreach (XmlElement xRootGroup in xTiles)
            {
                TreeNode mainNode = new TreeNode();
                mainNode.Text = xRootGroup.GetAttribute("name");
                mainNode.Tag = null;

                mainNode.ImageIndex = 0;

                AddChildren(mainNode, xRootGroup);

                treeViewTilesXML.Nodes.Add(mainNode);
            }
        }

        private void AddChildren(TreeNode node, XmlElement mainNode)
        {
            foreach (XmlElement e in mainNode)
            {
                if (e.Name == "subgroup")
                {
                    TreeNode tempNode = new TreeNode();

                    tempNode.Text = e.GetAttribute("name");
                    tempNode.Tag = e.GetAttribute("index");
                    tempNode.ImageIndex = 0;

                    if (e.HasChildNodes)
                    {
                        AddChildren(tempNode, e);
                    }

                    node.Nodes.Add(tempNode);
                }
            }
        }

    }
}
