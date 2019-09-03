using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerChatBalakovo.OTHER;

namespace ServerChatBalakovo.CORE
{
    class User : IGetInfo, ISetInfo
    {
        private DB.TableData _tableData;
        private DB.TableOnline _tableOnline;
        DateTime myDateTime = DateTime.MinValue;
        DateTime bantodate;
        DateTime lastmsg;

        public User() {
            _tableData = new DB.TableData();
            _tableOnline = new DB.TableOnline();
            //DateTime myDateTime = DateTime.MinValue;

        }

        public void AddDB(string id, string nickname)
        {
            string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");


            _tableData.addRow(Convert.ToInt32(id), nickname, 3, sqlFormattedDate, "", "");

        }
        public DateTime GetBanDateTimeValue(string id)
        {
            return DateTime.ParseExact(_tableData.getRow(OTHER.Configuration.ColumnNameTableData.Bantodate, id, OTHER.Configuration.ColumnNameTableData.Idvk), "dd.MM.yyyy H:mm:ss",
                                      System.Globalization.CultureInfo.InvariantCulture);
        }
        public bool CheckIsBan(string id)
        {
            //  Debug.WriteLine("ban to date in DB: " +_tableData.getRow("idvk", id, "bantodate"));
            bantodate = GetBanDateTimeValue(id);
            DateTime dateTimeNow = DateTime.Now;

            // Debug.WriteLine("Забанен до: "+ bantodate + " сейчас время: "+ dateTimeNow);

            if (bantodate >= dateTimeNow)
            {
                //   Debug.WriteLine("Забанен ");
                return true;
            }
            else
            {
                //  Debug.WriteLine("НЕ Забанен ");
                return false;
            }
        }


        public bool CheckIsNickExist(string nickname)
        {
            try
            {
                Debug.WriteLine("_tableData.getRow " + _tableData.getRow(OTHER.Configuration.ColumnNameTableData.Nickname, nickname, OTHER.Configuration.ColumnNameTableData.Nickname));
                if (_tableData.getRow(OTHER.Configuration.ColumnNameTableData.Nickname, nickname, OTHER.Configuration.ColumnNameTableData.Nickname) != nickname)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine("ex.Message " + ex.Message);
                return true;
            }
        }

        public bool CheckIsOnline(OTHER.Configuration.ColumnNameTableOnline ColumnNameForReturn, string id, OTHER.Configuration.ColumnNameTableOnline ColumnNameForFind)
        {
            if (_tableOnline.getRow(ColumnNameForReturn, id, ColumnNameForFind) != null) { return true; } else { return false; }
        }

        public bool CheckIsRegistration(string id)
        {
            try
            {
                if (_tableData.getRow(OTHER.Configuration.ColumnNameTableData.Accesslvl, id, OTHER.Configuration.ColumnNameTableData.Idvk) != "unknown")
                {
                    return true;
                }
                else { return false; }
            }
            catch (SQLiteException ex)
            {
                //  Debug.WriteLine("ex.Message " + ex.Message);
                return false;
            }
        }

        public bool CheckIsServiceCmd(string msg)
        {
            string check_service_msg = msg.Substring(0, 1);
            Debug.WriteLine(check_service_msg);
            // чекаем на команду
            if (check_service_msg == "!")
            {
                return true;
            }
            else { return false; }
        }

        public string SetLenghtLimitNick(string inputString, int maxChars, string postfix = "...")
        {
            if (maxChars <= 0)
                throw new ArgumentOutOfRangeException("maxChars");
            if (inputString == null || inputString.Length < maxChars)
                return inputString;

            var truncatedString = inputString.Substring(0, maxChars) + postfix;

            return truncatedString;
        }

      
        public string[] GetIdOnlineList()
        {
            return _tableOnline.LoadIdOnline(OTHER.Configuration.ColumnNameTableOnline.Idvk);
        }
        public string CheckIdAleardyInOnline(string id) {
          return  _tableOnline.getRow(Configuration.ColumnNameTableOnline.Idvk, id, Configuration.ColumnNameTableOnline.Idvk);
        }

        public string GetNickName(string id)
        {
            return _tableData.getRow(OTHER.Configuration.ColumnNameTableData.Nickname,id, OTHER.Configuration.ColumnNameTableData.Idvk);
        }

     
        public string[] GetIdOnlineForExit()
        {
               
            List<string> list = new List<string>();
            DateTime dateTimeNow = DateTime.Now;    
                              
            foreach (string s in _tableOnline.LoadIdAndLastmsg().Values)
            {
                
                lastmsg = DateTime.ParseExact(s, "dd.MM.yyyy H:mm:ss", System.Globalization.CultureInfo.InvariantCulture).AddMinutes(OTHER.Configuration.timeWaitNoActiveUser);

                if (lastmsg <= dateTimeNow)
                {              
                    list.AddRange(_tableOnline.LoadIdAndLastmsg().Where(x => x.Value == s).Select(x => x.Key));                 
                }
                
            }
            return list.ToArray();
           
        }

        public void RemoveNoActiveUser(string id)
        {
            _tableOnline.delRow(id); 
        }

        public void AddOnline(string id)
        {
            DateTime myDateTime = DateTime.Now;
            string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            _tableOnline.addRow(Int32.Parse(id), sqlFormattedDate);
        }

        public void ChangeOnlineActivity(string id)
        {
            DateTime myDateTime = DateTime.Now;
            string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
           
            _tableOnline.changeRow(id, sqlFormattedDate);
        }

        public void ChangeNickName(string id, string nickname)
        {
            DateTime myDateTime = DateTime.ParseExact(_tableData.getRow(OTHER.Configuration.ColumnNameTableData.Bantodate, id, OTHER.Configuration.ColumnNameTableData.Idvk), "dd.MM.yyyy H:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);
            string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            
            _tableData.changeRow(id,
              nickname,
              _tableData.getRow(OTHER.Configuration.ColumnNameTableData.Accesslvl, id, OTHER.Configuration.ColumnNameTableData.Idvk),
              sqlFormattedDate,
              _tableData.getRow(OTHER.Configuration.ColumnNameTableData.Banreason, id, OTHER.Configuration.ColumnNameTableData.Idvk),
              _tableData.getRow(OTHER.Configuration.ColumnNameTableData.Note, id, OTHER.Configuration.ColumnNameTableData.Idvk));
        }

     

        public void RemoveLeavingOnlineUser(string id)
        {
            _tableOnline.delRow(id);
        }
    }


}
