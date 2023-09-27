using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShipClient
{
    public partial class ServerConnectionPanel : Form
    {
        public ServerConnectionPanel()
        {
            InitializeComponent();
        }

        private void BConnect_Click(object sender, EventArgs e)
        {

            try
            {
                //Set login
                Program.client = new SocketClient(TBServerIP.Text);
                char comm = (char)11;
                string message = comm + " " + "test" + " <EOF>";
                Program.client.Send(message);
                //Receive answer
                var answer = Program.client.Receive()[0];

                //If answer is OK
                if (answer == (char)10)
                {

                    Program.userLogin = "Player1";
                    DialogResult = DialogResult.Yes;
                }
                else if (answer == (char)21)
                {
                    Program.userLogin = "Player2";
                    DialogResult = DialogResult.Yes;
                }

                else//Else if answer is Fail - char(9)
                {
                    MessageBox.Show("Typed nick is occupied", "Error!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //}

        }

        private void EnterClicked(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Return)
            {
                BConnect_Click(sender, e);
            }
        }

    }
}
