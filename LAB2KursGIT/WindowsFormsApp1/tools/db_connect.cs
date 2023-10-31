using System.Data.SQLite;

namespace WindowsFormsApp1.tools
{
    internal class db_connect
    {
        public static SQLiteConnection conn = new SQLiteConnection(@"Data Source=C:\Users\denzi\OneDrive\Документы\GitHub\KursovayaSUBD\LAB2KursGIT\WindowsFormsApp1\bin\Debug\mybd.db;datetimeformat=CurrentCulture");
        public static SQLiteCommand cmd;
        public static SQLiteDataReader reader;
        public static string commandText;
    }
}
