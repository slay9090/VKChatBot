
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerChatBalakovo.FORM
{
    public partial class BdEdit : Form
    {
        
        private DB.TableData _tableData;
        private DB.TableOnline _tableOnline;
        private DB.CreateDB _createDB;
        private static object lockObj = new object();
        /// <summary>
        /// какой dataGridView выбран 1- правый, 2- левый
        /// </summary>
        public int isSelectedTableNumber = 0;

        BlockingCollection<string> q = new BlockingCollection<string>();

        public BdEdit()
        {
            InitializeComponent();
            DateTime myDateTime = DateTime.MinValue;
            string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            textBox4.Text = sqlFormattedDate;

            _tableData = new DB.TableData(this);
            _tableOnline = new DB.TableOnline(this);
            _createDB = new DB.CreateDB(this);
          
        }

        private void button1_Click(object sender, EventArgs e) //CREATE BASE
        {
            _createDB.CreateFile();

                      
        }

        private void button2_Click(object sender, EventArgs e) //ADD ROW
        {
            if (isSelectedTableNumber == 1) {
                if (textBox1.Text != "" && textBox3.Text != "")
                {

                    _tableData.addRow(Convert.ToInt32(textBox1.Text), textBox2.Text, Convert.ToInt32(textBox3.Text), textBox4.Text, textBox5.Text, textBox6.Text);

                }
                else { MessageBox.Show("Заполните все поля", "Не всё", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }

            if (isSelectedTableNumber == 2)
            {
                if (textBox1.Text != "")
                {
                    DateTime myDateTime = DateTime.Now;
                    string sqlFormattedDate2 = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    _tableOnline.addRow(Convert.ToInt32(textBox1.Text), sqlFormattedDate2);

                }
                else { MessageBox.Show("Заполните все поля", "Не всё", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }



        }

        private void button4_Click(object sender, EventArgs e) //READ TABLES
        {
            _tableData.readTable("data");
            _tableOnline.readTable("online");
                
    }

   
        private void button5_Click(object sender, EventArgs e) //CHANGEROW BD
        {
            if (isSelectedTableNumber == 1)
            {
                _tableData.changeRow(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text);
            }
        }

      
        private void button3_Click(object sender, EventArgs e) //DEL ROW
        {
            if (isSelectedTableNumber == 1) { _tableData.delRow(); }
            if (isSelectedTableNumber == 2) { _tableOnline.delRow(textBox1.Text); }

        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e) //
        {
            //0001-01-01 00:00:00.000
            //01.01.0001 0:00:00.000 //dd.MM.yyyy H:mm:ss.fff
            isSelectedTableNumber = 1; 
            int rownuber = dataGridView1.SelectedCells[0].RowIndex;
            if (dataGridView1.Rows[rownuber].Cells[0].Value.ToString() != "") { 
            Console.WriteLine(dataGridView1.Rows[rownuber].Cells[3].Value.ToString());
            DateTime myDate = DateTime.ParseExact(dataGridView1.Rows[rownuber].Cells[3].Value.ToString(), "dd.MM.yyyy H:mm:ss",
                                     System.Globalization.CultureInfo.InvariantCulture);
            string sqlFormattedDate = myDate.ToString("yyyy-MM-dd HH:mm:ss.fff");

            textBox1.Text = dataGridView1.Rows[rownuber].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.Rows[rownuber].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.Rows[rownuber].Cells[2].Value.ToString();
            textBox4.Text = sqlFormattedDate;
            textBox5.Text = dataGridView1.Rows[rownuber].Cells[4].Value.ToString();
            textBox6.Text = dataGridView1.Rows[rownuber].Cells[5].Value.ToString();
        }

        }

        private void dataGridView2_Click(object sender, EventArgs e)
        {
            isSelectedTableNumber = 2;
            int rownuber = dataGridView2.SelectedCells[0].RowIndex;
            if (dataGridView2.Rows[rownuber].Cells[0].Value.ToString() != "")
            {
         
                textBox1.Text = dataGridView2.Rows[rownuber].Cells[0].Value.ToString();
               
            }
        }

     

     
        private void button6_Click(object sender, EventArgs e)
        {
            CORE.AutoExit autoex = CORE.AutoExit.GetInstance();

        }




    }
    }

