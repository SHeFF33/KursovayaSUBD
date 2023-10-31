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
        public byte[] ImageToByte(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();
                return imageBytes;
            }
        }
        void SaveImage(byte[] imagen)
        {
            DateTime now = DateTime.Now;
            db_connect.cmd = db_connect.conn.CreateCommand();
            db_connect.cmd.CommandText = String.Format("INSERT INTO myphoto ([name], [format], [date], [photo]) VALUES ('" + imgName + "','" + imgFormat + "','" + now + "',@0);");
            SQLiteParameter param = new SQLiteParameter("@0", System.Data.DbType.Binary);
            param.Value = imagen;
            db_connect.cmd.Parameters.Add(param);
            db_connect.conn.Open();
            try
            {
                db_connect.cmd.ExecuteNonQuery();
                MessageBox.Show("Изображение добавлено (обновите таблицу)");
            }
            catch (Exception exc1)
            {
                MessageBox.Show(exc1.Message);
            }
            db_connect.conn.Close();
        }
        public Image ByteToImage(byte[] imageBytes)
        {
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = new Bitmap(ms);
            return image;
        }

        void LoadImage()
        {
            string query = "SELECT photo FROM myphoto WHERE ID=@id;";
            db_connect.cmd = new SQLiteCommand(query, db_connect.conn);
            try
            {
                db_connect.cmd.Parameters.AddWithValue("@id", textBox1.Text);
            }
            catch (Exception exc1)
            {
                MessageBox.Show("Ошибка! Введите числовое значение, совпадающее с ID изображения.");
            }
            db_connect.conn.Open();
            try
            {
                IDataReader rdr = db_connect.cmd.ExecuteReader();
                try
                {
                    while (rdr.Read())
                    {
                        byte[] a = (System.Byte[])rdr[0];
                        pictureBox1.Image = ByteToImage(a);
                    }
                }
                catch (Exception exc) { MessageBox.Show(exc.Message); }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            db_connect.conn.Close();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            LoadImage();
        }
    }
}


