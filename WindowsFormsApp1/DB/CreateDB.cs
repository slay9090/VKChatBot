using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerChatBalakovo.DB
{
    class CreateDB
    {
        FORM.BdEdit _bdedit;
        private DB.TableData _tableData ;
        private DB.TableOnline _tableOnline;


        public CreateDB(FORM.BdEdit bdedit)
        {
            this._bdedit = bdedit;
            _tableData = new DB.TableData(_bdedit);
            _tableOnline = new DB.TableOnline(_bdedit);

        }

        public void CreateFile()
        {
            if (File.Exists(Directory.GetCurrentDirectory() + OTHER.Configuration.databaseName) == false)
            {
                SQLiteConnection.CreateFile(Directory.GetCurrentDirectory() + OTHER.Configuration.databaseName);
                _tableData.createTable();
                _tableOnline.createTable();
                MessageBox.Show("БД создана", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("БД уже существует", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Console.WriteLine(File.Exists(Directory.GetCurrentDirectory() + OTHER.Configuration.databaseName) ? "База данных создана" : "Возникла ошиюка при создании базы данных");
        }
    }
}
