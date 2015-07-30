using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WpfApplication1
{
    class DatabaseManager
    {
        public DatabaseManager()
        {
            string createTableQuery = @"CREATE TABLE IF NOT EXISTS LASTPROJECTS (
                          [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                          [address] VARCHAR(2048)  NULL
                          )";
            string path = Path.
                GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())); 
            path = path + @"\bin\Debug\databaseFile.db3";

            if(!File.Exists (path))
                System.Data.SQLite.SQLiteConnection.CreateFile("databaseFile.db3");
            
            using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=databaseFile.db3"))
            {
                using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                {
                    con.Open();
                    com.CommandText = createTableQuery;
                    com.ExecuteNonQuery();
                    con.Close();
                }
            }
            Console.WriteLine("ALLO");
        }
        
        public void insertFile(String file)
        {
            using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=databaseFile.db3"))
            {
                using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                {
                    con.Open();                             
                    com.CommandText = "INSERT INTO LASTPROJECTS (address) Values ('"+ file +"')";
                    com.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public void removeProject(List<String> paths)
        {
            using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=databaseFile.db3"))
            {
                using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                {
                    con.Open();
                    for (int i = 0; i < paths.Count; i++)
                    {
                        com.CommandText = "DELETE FROM LASTPROJECTS WHERE address = '" + paths[i] + "'";
                        com.ExecuteNonQuery();
                    }
                    con.Close();
                }
            }
        }

        public List<String> checkProjects()
        {
            List<String> list = new List<String>();
            using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=databaseFile.db3"))
            {
                using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                {
                    con.Open();

                    com.CommandText = "Select * FROM LASTPROJECTS";

                    using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader["address"]);

                            if (!File.Exists(reader["address"].ToString()))
                                list.Add(reader["address"].ToString());
                        }
                    }
                    con.Close();
                }
            }
            if(list.Count > 0)
                removeProject(list);

            return getLastProjects();
        }

        public List<String> getLastProjects()
        {
            List<String> list = new List<String>();
            using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=databaseFile.db3"))
            {
                using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                {
                    con.Open();

                    com.CommandText = "Select * FROM LASTPROJECTS";

                    using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader["address"]);

                           list.Add(reader["address"].ToString());
                        }
                    }
                    con.Close();
                }
            }
            return list;
        }
    }
}
