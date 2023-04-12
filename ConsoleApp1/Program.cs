using Microsoft.SqlServer.Server;
using SqliteDemo;
using static System.Net.Mime.MediaTypeNames;

internal static class Program
{
    /// <summary>
    /// 应用程序的主入口点。
    /// </summary>
    [STAThread]
    static void Main()
    {
        SQLiteHelper qLiteHelper = new SQLiteHelper();
        qLiteHelper.GetAllCompany();
    }
}