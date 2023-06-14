using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using System.ComponentModel;

namespace MM_DataBinding
{
    public class DBHelper
    {

        private static MySqlConnection? conn = null;

        private DBHelper(
            String host,
            int port,
            string User,
            string Password,
            String database
        )
        {
            var connStr = $"Server={host}; DataBase = {database}; port = {port}; User Id = {User}; password = {Password}";
            conn = new MySqlConnection(connStr);
            conn.Open();
        }

        // Статический метод, который возвращает сущность (инстанс) класса ДБХелпер
        // Он специально сделан, ибо наше подключение существует в единственном экземпляре для конкретной базы
        // Нам нет смысла делать более 1 подключения к ней в нашем проекте

        private static DBHelper instance = null;
        public static DBHelper GetInstance(
            String host = "localhost",
            int port = 0,
            string User = "root",
            string Password = "",
            String database = "")
        {
            if(instance == null)
            {
                instance = new DBHelper(host, port, User, Password, database);
            }
            return instance;
        }

        // Получаем информацию из нашей таблички с группами
        public BindingList<Group> GetGroups()
        {
            BindingList<Group> groups = new BindingList<Group>();
            var queryStr = "SELECT * FROM StudentGroup";
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = queryStr;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        groups.Add(new Group
                        {
                            Num = reader.GetString(nameof(Group.Num)),
                            Year = reader.GetInt32(nameof(Group.Year)),
                            Spec = reader.GetString(nameof(Group.Spec)),
                            Department = reader.GetString(nameof(Group.Department)),
                            Level = reader.GetString(nameof(Group.Level))
                        });
                    }
                }
            }
            return groups;
        }

        public void InsertNew(Group newGr)
        {
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = $"INSERT INTO `StudentGroup` (Num, Year, Spec, Department, Level) VALUES (" +
                $"@num, @year, @spec, @department, @level" +
                $");";
            cmd.Parameters.Add(new MySqlParameter("@num", newGr.Num));
            cmd.Parameters.Add(new MySqlParameter("@year", newGr.Year));
            cmd.Parameters.Add(new MySqlParameter("@spec", newGr.Spec));
            cmd.Parameters.Add(new MySqlParameter("@department", newGr.Department));
            cmd.Parameters.Add(new MySqlParameter("@level", newGr.Level));
            cmd.ExecuteNonQuery();
        }

    }
}
