using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerChatBalakovo.FORM
{
    public class ControlForm
    {
        private CORE.MessageSender _sendMsgFromGroup;
        private FORM.MainForm _mainForm;
                public ControlForm(FORM.MainForm mainForm) {
            Debug.WriteLine("ControlForm(FORM.MainForm mainForm) init..");
            this._mainForm = mainForm;
        }

        private FORM.MassSend _MassSendForm;
        public ControlForm(FORM.MassSend massSend)
        {
            Debug.WriteLine("ControlForm(FORM.MassSend massSend) init..");

            this._MassSendForm = massSend;
            _sendMsgFromGroup = new CORE.MessageSender(this);
        }
        /// <summary>
        /// формирование и добавление строчки в RichTextBox
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Message"></param>
        public void AddMsgRichTextBox(string ID, string Message) {

            _mainForm.BeginInvoke(new MethodInvoker(delegate
            {
                _mainForm.richTextBox1.AppendText("[" + ID + "] " + Message + "" );
                _mainForm.richTextBox1.Select(_mainForm.richTextBox1.Text.Length - 1, 0);
                _mainForm.richTextBox1.ScrollToCaret();
              // _mainForm.richTextBox1.Focus();
                               
            }));


        }
        /// <summary>
        /// устанавливаем строку состояния
        /// </summary>
        /// <param name="text"></param>
        public void SetLabelStateProgramm(string text) {
            _mainForm.BeginInvoke(new MethodInvoker(delegate
            {
                _mainForm.label1.Text = text;
            }));
        }

        public void SetLabelCouterMassSend(int text)
        {

                       
            _MassSendForm.BeginInvoke(new MethodInvoker(delegate
            {
                _MassSendForm.label1.Text = text.ToString();
            }));
        }

        public void StartCountersMassSendLabel()
        {
            new Thread(() => //
            {
                while (true)
                {
                    SetLabelCouterMassSend(_sendMsgFromGroup.Counters(OTHER.Configuration.Counters.allSend));

                }

            }).Start();
        }

    }
}
