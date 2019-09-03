using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerChatBalakovo.FORM
{
    public partial class MassSend : Form
    {
        private CORE.MessageSender _messageSender;
        private FORM.ControlForm _controlForm;
 
        public MassSend()
        {
            InitializeComponent();
            _messageSender = new CORE.MessageSender();
            _controlForm = new ControlForm(this);
            // _controlForm= new ControlForm(new FORM.MassSend());

        }

        public void Send(CORE.ISendingMsg sendingMsg, string msg)
        {
            var sort = VkNet.Enums.SafetyEnums.GroupsSort.IdAsc;
            switch (textBox2.Text)  //id_asc; id_desc; time_asc; time_desc
            {
                case "id_asc": sort = VkNet.Enums.SafetyEnums.GroupsSort.IdAsc; ; break;
                case "id_desc": sort = VkNet.Enums.SafetyEnums.GroupsSort.IdDesc; ; break;
                case "time_asc": sort = VkNet.Enums.SafetyEnums.GroupsSort.TimeAsc; ; break;
                case "time_desc": sort = VkNet.Enums.SafetyEnums.GroupsSort.TimeDesc; ; break;
                default: sort = VkNet.Enums.SafetyEnums.GroupsSort.IdAsc; ; break;
            }
            sendingMsg.SendAll(textBox1.Text, Convert.ToInt32(textBox3.Text), sort, msg);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text != "") {
                Send(_messageSender, richTextBox1.Text);
             //   _controlForm.StartCountersMassSendLabel();

            }
          

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
