using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerChatBalakovo.DB
{
    class TableOnline
    {

        FORM.BdEdit _bdedit;
        string result;

        /// <summary>
        /// Имена столбцей таблицы онлайн
        /// </summary>




        public TableOnline(FORM.BdEdit bdedit)
        {
            this._bdedit = bdedit;

        }

        public TableOnline()
        {

        }

        public void createTable()
        {
            //smalldatetime ГГГГ-ММ-ДД чч:мм:сс
            SQLiteConnection connection =
            new SQLiteConnection(string.Format("Data Source={0};", Directory.GetCurrentDirectory() + OTHER.Configuration.databaseName));
            SQLiteCommand commandtable1 =
                           new SQLiteCommand("CREATE TABLE online (idvk INT, lastmsg DATETIME);", connection);
            connection.Open();
            commandtable1.ExecuteNonQuery();

            connection.Close();
        }

        public void readTable(string tablename)
        {
            DataSet ds = new DataSet();
            string sql = "SELECT * FROM " + tablename + "";
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(string.Format("Data Source={0};", Directory.GetCurrentDirectory() + OTHER.Configuration.databaseName)))
                {
                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn))
                    {
                        da.Fill(ds);
                        _bdedit.dataGridView2.DataSource = ds.Tables[0].DefaultView;
                    }
                }
            }
            catch (Exception err)
            {
            }

        }

        public void delRow(string id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", Directory.GetCurrentDirectory() + OTHER.Configuration.databaseName)))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {

                    command.CommandText = "delete from online where idvk = @idvk";
                    command.Parameters.AddWithValue("@idvk", id);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        /// <summary>
        /// Добавление строки в табл. онлайн
        /// </summary>
        /// <param name="idvk"></param>
        /// <param name="lastmsgtime"></param>
        public void addRow(int idvk, string lastmsgtime) ///

        {

            SQLiteConnection connection =
                new SQLiteConnection(string.Format("Data Source={0};", Directory.GetCurrentDirectory() + OTHER.Configuration.databaseName));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("INSERT INTO 'online' ('idvk','lastmsg') VALUES ( " +
                 "" + idvk + "," +
                 "'" + lastmsgtime + "');",
                 connection);
            command.ExecuteNonQuery();
            connection.Close();
            Console.WriteLine("Готово");

        }
/// <summary>
/// изменить строку
/// </summary>
/// <param name="idvk">ищем по ИД</param>
/// <param name="lastmsgtime"></param>
        public void changeRow(string idvk, string lastmsgtime) ///string idvk, string lastmsgtime
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", Directory.GetCurrentDirectory() + OTHER.Configuration.databaseName)))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText =
                            "update online set lastmsg=:lastmsg where idvk=:idvk";
                        command.Parameters.Add("lastmsg", DbType.String).Value = lastmsgtime;
                        command.Parameters.Add("idvk", DbType.String).Value = idvk;
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex) { }
        }
        /// <summary>
        /// Возращаем строку из таблицы Онлайн
        /// </summary>
        /// <param name="ColumnNameForReturn">Колонка, строку которой возратим</param>
        /// <param name="value">Значение по которому ищем</param>
        /// <param name="ColumnNameForFind">Колонка по значениям которой ищем строку</param>
        /// <returns></returns>
        public string getRow(OTHER.Configuration.ColumnNameTableOnline ColumnNameForReturn, string value, OTHER.Configuration.ColumnNameTableOnline ColumnNameForFind)
        {
            //Console.WriteLine("ENUM^ " + ColumnNameForReturn + " " + ColumnNameForFind + " " + ColumnName.Idvk);
            int ord;
            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", Directory.GetCurrentDirectory() + OTHER.Configuration.databaseName)))
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                connection.Open();
                if (ColumnNameForFind == OTHER.Configuration.ColumnNameTableOnline.Idvk) { command.CommandText = @"select idvk,lastmsg from online where idvk=" + value + ""; }
                if (ColumnNameForFind == OTHER.Configuration.ColumnNameTableOnline.Lastmsg) { command.CommandText = @"select idvk,lastmsg from online where lastmsg='" + value + "'"; }
                command.ExecuteNonQuery();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read()) // Don't assume we have any rows.
                    {

                        if (ColumnNameForReturn == OTHER.Configuration.ColumnNameTableOnline.Idvk) { ord = reader.GetOrdinal("idvk"); result = reader.GetInt32(ord).ToString(); }
                        if (ColumnNameForReturn == OTHER.Configuration.ColumnNameTableOnline.Lastmsg) { ord = reader.GetOrdinal("lastmsg"); result = reader.GetString(ord); }

                        return result;

                    }

                    return null;
                }
            }

        }

        public string[] LoadIdOnline(OTHER.Configuration.ColumnNameTableOnline ColumnNameForReturn) {

             DataSet ds = new DataSet();
            string sql = "SELECT " + ColumnNameForReturn + " FROM online";
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(string.Format("Data Source={0};", Directory.GetCurrentDirectory() + OTHER.Configuration.databaseName)))
                {
                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn))
                    {
                        da.Fill(ds);
                       string [] load = ds.Tables[0].Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                        return load;
                        //  string[] arrray = ds.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                        //  return new string[] { ds.Tables[0].DefaultView };
                    }
                }
            }
            catch (Exception err)
            {
                return null;
            }            
        }

        public Dictionary<string, string> LoadIdAndLastmsg()
        {
            DataSet ds = new DataSet();
            string sql = "SELECT idvk, lastmsg FROM online";
            Dictionary<string, string> dic = new Dictionary<string, string>();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(string.Format("Data Source={0};", Directory.GetCurrentDirectory() + OTHER.Configuration.databaseName)))
                {
                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn))
                    {
                        da.Fill(ds);
                        string[] idvk = ds.Tables[0].Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                        string[] lastmsg = ds.Tables[0].Rows.OfType<DataRow>().Select(k => k[1].ToString()).ToArray();

                        var listOnlineTable = idvk.Zip(lastmsg, (n, w) => new { Number = n, Word = w });
                        foreach (var nw in listOnlineTable)
                        {        
                            dic.Add(nw.Number, nw.Word);
                        }

                        //foreach (var xz in dic.Values) {
                        //    Console.WriteLine(xz);
                        //}


                        return dic;
                        //  string[] arrray = ds.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                        //  return new string[] { ds.Tables[0].DefaultView };
                    }
                }
            }
            catch (Exception err)
            {
                return null;
            }
        }


    }
}
