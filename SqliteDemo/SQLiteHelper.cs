using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Reflection.Metadata;
using System.Data;
using System.Data.Entity.Infrastructure;

namespace SqliteDemo
{
    public class SQLiteHelper
    {
        ///创建数据库文件
        public static void CreateDBFile(string fileName)
        {
            string path = System.Environment.CurrentDirectory + @"/Data/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string databaseFileName = path + fileName;
            if (!File.Exists(databaseFileName))
            {
                SQLiteConnection.CreateFile(databaseFileName);
            }
        }


        //生成连接字符串
        private static string CreateConnectionString()
        {
            SQLiteConnectionStringBuilder connectionString = new SQLiteConnectionStringBuilder();
            connectionString.DataSource = @"D:\\SoftWare\\sqlite\\demo.db";
            string conStr = connectionString.ToString();
            return conStr;
        }

        /// <summary>
        /// 对插入到数据库中的空值进行处理
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ToDbValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// 对从数据库中读取的空值进行处理
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object FromDbValue(object value)
        {
            if (value == DBNull.Value)
            {
                return null;
            }
            else
            {
                return value;
            }
        }


        /// <summary>
        /// 执行非查询数据库操作
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sqlString, params SQLiteParameter[] parameters)
        {
            string connectionString = CreateConnectionString();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sqlString;
                    foreach (SQLiteParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        /// 执行查询并返回查询结果第一行第一列
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="sqlparams">参数列表</param>
        /// <returns></returns>
        public static object ExecuteScalar(string sqlString, params SQLiteParameter[] parameters)
        {
            string connectionString = CreateConnectionString();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sqlString;
                    foreach (SQLiteParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                    return cmd.ExecuteScalar();
                }
            }
        }


        /// <summary>
        /// 查询多条数据
        /// </summary>         
        /// /// <param name="sqlString">SQL语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>返回查询的数据表</returns>
        public static DataTable GetDataTable(string sqlString, params SQLiteParameter[] parameters)
        {
            string connectionString = CreateConnectionString();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sqlString;
                    foreach (SQLiteParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                    DataSet ds = new DataSet();
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                    adapter.Fill(ds);
                    conn.Close();
                    return ds.Tables[0];
                }
            }
        }


        public List<Company> GetAllCompany()
        {
            List<Company> company = new List<Company>();
            string sql = "select * from company;";
            SQLiteParameter[] parameters = new SQLiteParameter[]
             {
                 //new SQLiteParameter("@IMG_ID", DbType.Int32,4),
                 //new SQLiteParameter("@USER_ID", DbType.String),
             };

            DataTable dt = SQLiteHelper.GetDataTable(sql, parameters);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int id = Convert.ToInt32(dt.Rows[i]["id"]);
                string name = dt.Rows[i]["name"].ToString();
                string address = dt.Rows[i]["address"].ToString();
                decimal salary = Convert.ToDecimal(dt.Rows[i]["salary"]);
                Company clientEty = new Company
                {
                    id = id,
                    name = name,
                    address = address,
                    salary = salary
                };
                company.Add(clientEty);
            }

            return company;
        }


    }
}
