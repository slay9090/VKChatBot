using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerChatBalakovo.DB
{
    class TableData : IQuereDB //тут этот интерфейс нахер не нужОн
    {

        FORM.BdEdit _bdedit;
             

        public TableData (FORM.BdEdit bdedit)
            {
            this._bdedit = bdedit;

        }
        public TableData() { } // !!!проверить

        public void addRow(int idvk, string nickname, int accesslvl, string bantodate, string banreason, string note) /// 'idvk','nickname','accesslvl','bantodate',banreason,note

        {

            SQLiteConnection connection =
                new SQLiteConnection(string.Format("Data Source={0};", Directory.GetCurrentDirectory() + OTHER.Configuration.databaseName));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("INSERT INTO 'data' ('idvk','nickname','accesslvl','bantodate',banreason,note) VALUES ( " +
                 "" + idvk + "," +
                 "'" + nickname + "'," +
                 "" + accesslvl + "," +
                 "'" + bantodate + "'," +
                 "'" + banreason + "'," +
                 "'" + note + "');",
              
                connection);
            command.ExecuteNonQuery();
            connection.Close();
            Console.WriteLine("Готово");
                       
        }
        /// <summary>
        /// изменить строку поиск по ид
        /// </summary>
        /// <param name="idvk"></param>
        /// <param name="nickname"></param>
        /// <param name="accesslvl"></param>
        /// <param name="bantodate"></param>
        /// <param name="banreason"></param>
        /// <param name="note"></param>
        public void changeRow(string idvk, string nickname, string accesslvl, string bantodate, string banreason, string note) /// !!! idvk - is WHERESQL, 'nickname','accesslvl','bantodate',banreason,note 
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", Directory.GetCurrentDirectory() + OTHER.Configuration.databaseName)))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {

                        command.CommandText =
                            "update data set nickname = :nickname, accesslvl =:accesslvl, bantodate=:bantodate, banreason=:banreason, note=:note where idvk=:idvk";
                        command.Parameters.Add("nickname", DbType.String).Value = nickname;
                        command.Parameters.Add("accesslvl", DbType.String).Value = accesslvl;
                        command.Parameters.Add("bantodate", DbType.String).Value = bantodate;
                        command.Parameters.Add("banreason", DbType.String).Value = banreason;
                        command.Parameters.Add("note", DbType.String).Value = note;
                        command.Parameters.Add("idvk", DbType.String).Value = idvk;
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex) { }
        }

        public void createTable() {
            SQLiteConnection connection =
            new SQLiteConnection(string.Format("Data Source={0};", Directory.GetCurrentDirectory() + OTHER.Configuration.databaseName));
            SQLiteCommand commandtable1 =
                           new SQLiteCommand("CREATE TABLE data (idvk INT, nickname TINYTEXT, accesslvl TINYINT, bantodate DATETIME, banreason TEXT, note TINYTEXT);", connection);
            connection.Open();
            commandtable1.ExecuteNonQuery();
          
            connection.Close();
        }

        public void delRow()
        {
            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", Directory.GetCurrentDirectory() + OTHER.Configuration.databaseName)))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {

                    command.CommandText = "delete from data where idvk = @idvk";
                   command.Parameters.AddWithValue("@idvk",_bdedit.textBox1.Text);

                    command.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// Получаем инфу о пользщовате
        /// </summary>
        /// <param name="columnNameForReturn">Имя колонки значение которой return (чстрока)
        /// <param name="valueByColumn">Значение которое ищем (строка)</param>
        /// <param name="columnNameForFind">Имя колонки по которой искать (строка)</param>
        /// idvk,nickname,accesslvl,bantodate,banreason,note
        /// <returns></returns>

        public string getRow(OTHER.Configuration.ColumnNameTableData columnNameForReturn, string valueByColumn, OTHER.Configuration.ColumnNameTableData columnNameForFind) {
         
            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", Directory.GetCurrentDirectory() + OTHER.Configuration.databaseName)))
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                connection.Open();
               if (columnNameForFind == OTHER.Configuration.ColumnNameTableData.Idvk) { command.CommandText = @"select idvk,nickname,accesslvl,bantodate,banreason,note from data where idvk=" + valueByColumn + ""; }
                if (columnNameForFind == OTHER.Configuration.ColumnNameTableData.Nickname) { command.CommandText = @"select idvk,nickname,accesslvl,bantodate,banreason,note from data where nickname='" + valueByColumn + "'"; }

                command.ExecuteNonQuery();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read()) // Don't assume we have any rows.
                    {
                        OTHER.Configuration.ColumnNameTableData selection = columnNameForReturn;
                        switch (selection)
                        {
                            case OTHER.Configuration.ColumnNameTableData.Idvk : return reader.GetInt32(0).ToString();
                            case OTHER.Configuration.ColumnNameTableData.Nickname : return reader.GetString(1);
                            case OTHER.Configuration.ColumnNameTableData.Accesslvl : return reader.GetInt32(2).ToString(); 
                            case OTHER.Configuration.ColumnNameTableData.Bantodate : return reader.GetDateTime(3).ToString(); //
                            case OTHER.Configuration.ColumnNameTableData.Banreason: return reader.GetString(4);
                            case OTHER.Configuration.ColumnNameTableData.Note: return reader.GetString(5);
                            default: Console.WriteLine("Таких колонок нет"); break;
                        }                        
                    }
                    return "unknown"; //не нашли                   
                }              
            }
        }

        public void readTable(string tablename)
        {
            DataSet ds = new DataSet();
            string sql = "SELECT * FROM "+tablename+"";
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(string.Format("Data Source={0};", Directory.GetCurrentDirectory() + OTHER.Configuration.databaseName)))
                {
                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn))
                    {
                        da.Fill(ds);
                        _bdedit.dataGridView1.DataSource = ds.Tables[0].DefaultView;
                    }
                }
            }
            catch (Exception err)
            {
            }

        }

       
    }
}
