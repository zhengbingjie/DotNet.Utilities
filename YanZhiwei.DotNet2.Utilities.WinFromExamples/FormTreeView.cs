﻿using System;
using System.Drawing;
using System.Windows.Forms;
using YanZhiwei.DotNet2.Utilities.WinForm;

namespace YanZhiwei.DotNet2.Utilities.WinFromExamples
{
    public partial class FormTreeView : Form
    {
        public FormTreeView()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            treeView1.ApplyNodeHighLight(Brushes.Blue);
        }

        private void winTreeView_Load(object sender, EventArgs e)
        {
            //treeView1.ApplyNodeHighLight(Brushes.Blue);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TreeViewHelper.AttachMenu(treeView1, contextMenuStrip1, (node) => true);
        }
    }
}