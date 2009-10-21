/***************************************************************************
 *
 * $Author: Turley
 * 
 * "THE BEER-WARE LICENSE"
 * As long as you retain this notice you can do whatever you want with 
 * this stuff. If we meet some day, and you think this stuff is worth it,
 * you can buy me a beer in return.
 *
 ***************************************************************************/

using System;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;

namespace FiddlerControls
{
    public partial class SkillGrp : UserControl
    {
        public SkillGrp()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        private bool Loaded = false;

        /// <summary>
        /// ReLoads if loaded
        /// </summary>
        private void Reload()
        {
            if (Loaded)
                OnLoad(this, EventArgs.Empty);
        }
        private void OnLoad(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Options.LoadedUltimaClass["SkillGrp"] = true;

            treeView1.BeginUpdate();
            ArrayList cache = new ArrayList();
            
            foreach (Ultima.SkillGroup group in Ultima.SkillGroups.List)
            {
                TreeNode groupnode = new TreeNode();
                groupnode.Text = group.Name;
                if (group.Name == "Misc")
                    groupnode.ForeColor = Color.Blue;
                for (int i = 0; i < Ultima.SkillGroups.SkillList.Count; ++i)
                {
                    if (((int)Ultima.SkillGroups.SkillList[i]) == cache.Count)
                    {
                        TreeNode skillnode = new TreeNode();
                        skillnode.Text = Ultima.Skills.GetSkill(i).Name;
                        groupnode.Nodes.Add(skillnode);
                    }
                }
                cache.Add(groupnode);
            }

            treeView1.Nodes.AddRange((TreeNode[])cache.ToArray(typeof(TreeNode)));
            treeView1.EndUpdate();
            
            if (!Loaded)
                FiddlerControls.Events.FilePathChangeEvent += new FiddlerControls.Events.FilePathChangeHandler(OnFilePathChangeEvent);
            Loaded = true;
            Cursor.Current = Cursors.Default;
        }

        private void OnFilePathChangeEvent()
        {
            Reload();
        }

        private void OnClickSave(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            Ultima.SkillGroups.Save(path);
            MessageBox.Show(
                String.Format("SkillGrp saved to {0}", path),
                "Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }
    }
}
