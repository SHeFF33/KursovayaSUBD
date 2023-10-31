﻿using System;
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

        private void button1_Click(object sender, EventArgs e)
        {
            mydb = new sqliteclass();
            sSql = @"CREATE TABLE if not exists [myphoto]([id] INTEGER PRIMARY KEY AUTOINCREMENT,[name] TEXT,[format] TEXT,[date] REAl,[photo] BLOP);";
            mydb.iExecuteNonQuery(sPath, sSql, 0);

            sSql = @"insert into myphoto (name,format,date) values('Фамилия Имя Отчество','Да','now');";
            if (mydb.iExecuteNonQuery(sPath, sSql, 1) == 0)
            {
                Text = "Ошибка проверки таблицы на запись,";
                Text += " таблица или не создана или не прошла запись тестовой строки!";
                mydb = null;
                return;
            }
            sSql = "select * from myphoto";
            DataRow[] datarows = mydb.drExecute(sPath, sSql);
            if (datarows == null)
            {
                Text = "Ошибка проверки таблицы на чтение!";
                mydb = null;
                return;
            }
            Text = "";
            foreach (DataRow dr in datarows)
            {
                Text += dr["id"].ToString().Trim() + dr["name"].ToString().Trim() + dr["format"].ToString().Trim() + " ";
            }
            sSql = "delete from myphoto";
            if (mydb.iExecuteNonQuery(sPath, sSql, 1) == 0)
            {
                Text = "Ошибка проверки таблицы на удаление записи!";
                mydb = null;
                return;
            }
            Text = "Таблица создана!";
            mydb = null;
            return;
        }
        string imgFormat;
        string imgName;

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = @"C:\Users\Xzidx\Pictures\GameCenter\Desktop";
            openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image photo = new Bitmap(openFileDialog1.FileName);

                string imgPath = openFileDialog1.FileName;
                imgFormat = Path.GetExtension(imgPath).Replace(".", "").ToLower();
                imgName = Path.GetFileName(imgPath).Replace(Path.GetExtension(imgPath), "");

                byte[] pic = ImageToByte(photo, System.Drawing.Imaging.ImageFormat.Png);
                SaveImage(pic);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mydb = new sqliteclass();
            sSql = "delete from myphoto";
            if (mydb.iExecuteNonQuery(sPath, sSql, 1) == 0)
            {
                Text = "Ошибка удаления записи!";
                mydb = null;
                return;
            }
            mydb = null;
            Text = "Записи удалены из БД!";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mydb = new sqliteclass();
            sSql = "select * from myphoto";
            DataRow[] datarows = mydb.drExecute(sPath, sSql);

            if (datarows == null)
            {
                Text = "Ошибка чтения!";
                mydb = null;
                return;
            }
            Text = "";

            dataGridView1.Rows.Clear();

            foreach (DataRow dr in datarows)
            {

                dataGridView1.Rows.Add(dr["id"], dr["name"], dr["format"], dr["date"]);
            }

            foreach (DataRow dr in datarows)
            {
                Text += dr["id"].ToString().Trim() + " " + dr["name"].ToString().Trim() + " " + dr["format"].ToString().Trim() + " " + dr["date"].ToString().Trim() + " ";
            }
            mydb = null;
        }
    }
}


