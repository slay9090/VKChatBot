using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Flurl.Http;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;
using System.Data.SQLite;

namespace ServerChatBalakovo.FORM
{

    //ТОДО
    // в ядре реализовать сортировщик
 // сделать синглтон в апи гетауз

    public partial class MainForm : Form
    {
        private CORE.Receiver _receiver;
        private FORM.ControlForm _controlForm;

        /// <summary>
        /// флаг остановки сервера, false - работаем, true - останавливаем
        /// </summary>
        public static bool stopRun;


        public MainForm()
        {
            InitializeComponent();
            _receiver = new CORE.Receiver(this);
            _controlForm = new ControlForm(this);
     
        }
      

        private void Button1_Click(object sender, EventArgs e)
        {
            stopRun = false;
           
        _receiver.GetMsgFromFroup();
 

        }




      

        private void button3_Click(object sender, EventArgs e)
        {
            FORM.BdEdit bdedit = new FORM.BdEdit();
            bdedit.Show();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            stopRun = true;
            CORE.AutoExit.TimerStop();
            _controlForm.SetLabelStateProgramm("Завершаем потоки..");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MassSend massSend = new MassSend();
            massSend.Show();
        }
    }


}
