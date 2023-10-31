using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Data.SQLite;
using System.Collections.Generic;
using WindowsFormsApp1.tools;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        private sqliteclass mydb = null;
        private string sPath = string.Empty;
        private string sSql = string.Empty;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            sPath = Path.Combine(Application.StartupPath, "mybd.db");
            Text = sPath;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}


