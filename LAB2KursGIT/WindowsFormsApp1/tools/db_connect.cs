using System.Data.SQLite;

namespace WindowsFormsApp1.tools
{
    internal class db_connect
    {
        public static SQLiteConnection conn = new SQLiteConnection(@"Data Source=C:\Users\denzi\3 курс\СУБД\LAB2\LAB2-финальный вариант\WindowsFormsApp1\bin\Debug\mybd.db;datetimeformat=CurrentCulture");
        public static SQLiteCommand cmd;
        public static SQLiteDataReader reader;
        public static string commandText;
    }
}
