﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShipClient
{
    static class Program
    {
        public static string userLogin;
        public static string serverAddress;
        public static SocketClient client;
        public static EnemySelectionPanel enemySelect;
        public static FormGame main;
        public static string enemyNick;
        public static volatile bool isThreadRunning = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            userLogin = "";
            serverAddress = "";

            // основная логика подключения клиента 
            ServerConnectionPanel servConn;
            DialogResult dialogResult = DialogResult.No;
            while (dialogResult != DialogResult.Cancel)
            {
                if (dialogResult == DialogResult.No/*dialog==0*/)
                {
                    isThreadRunning = false;
                    servConn = new ServerConnectionPanel();
                    dialogResult = servConn.ShowDialog();
                }
                if (dialogResult == DialogResult.Yes/*dialog == 1*/) //user is logIn
                {
                    if (isThreadRunning == false) //if you first login
                    {
                        isThreadRunning = true;
                        new Thread(() =>
                        {
                            Thread.CurrentThread.IsBackground = true;
                            ReceivingMessages();
                        }).Start();
                    }
                    enemySelect = new EnemySelectionPanel();
                    dialogResult = enemySelect.ShowDialog();
                }
                if (dialogResult == DialogResult.OK /*dialog == 2*/) //user want to register
                {
                    main = new FormGame(enemyNick);
                    dialogResult = main.ShowDialog();
                }
            }
        }
        public static void ReceivingMessages() // основная логика игры
        {
            while (isThreadRunning == true)
            {
                try
                {
                    var answer = client.Receive();
                    switch (answer[0])
                    {

                        case (char)12:
                            {
                                List<string> onlineEnemyListTmp = new List<string>();

                                //Get Enemies
                                string[] enemies = enemies = answer.Split(' ');//in enemy[0] is communique, in enemy[n] is EOF
                                string[] enemy;
                                foreach (var item in enemies)
                                {
                                    enemy = item.Split(';');
                                    if (enemy.Count() > 1)
                                    {
                                        onlineEnemyListTmp.Add(enemy[0]); //create actual list of online enemies
                                                                          //if list contains don't add, 
                                        if (enemySelect.onlineEnemyList.Contains(enemy[0]))
                                        {
                                            enemySelect.onlineEnemyList.Remove(enemy[0]);
                                        }
                                        else //if not add, to dgv and remove from onlineEnemyList
                                        {
                                            DataGridViewRow dgvRow = new DataGridViewRow();
                                            dgvRow.Cells.Add(new DataGridViewTextBoxCell { Value = enemy[1] });//address ipv4:port
                                            dgvRow.Cells.Add(new DataGridViewTextBoxCell { Value = enemy[0] });//nick
                                            MethodInvoker inv1 = delegate
                                            {
                                                enemySelect.DGVAvailableEnemies.Rows.Add(dgvRow);
                                            }; enemySelect.Invoke(inv1);

                                        }
                                    }
                                }
                                //Remove enemies who are not online
                                string enemyVal = "";
                                for (int i = enemySelect.DGVAvailableEnemies.Rows.Count - 1; i >= 0; i--)
                                {
                                    enemyVal = enemySelect.DGVAvailableEnemies.Rows[i].Cells[1].Value.ToString();
                                    if (enemySelect.onlineEnemyList.Contains(enemyVal))
                                    {
                                        enemySelect.onlineEnemyList.Remove(enemyVal);
                                        MethodInvoker inv2 = delegate
                                        {
                                            enemySelect.DGVAvailableEnemies.Rows.RemoveAt(i);
                                        }; enemySelect.Invoke(inv2);

                                    }
                                }
                                //Set onlineEnemyList to actual list of online enemies
                                enemySelect.onlineEnemyList = new List<string>(onlineEnemyListTmp);
                                enemySelect.DGVAvailableEnemies.ClearSelection();
                                break;
                            }
                        //GetOffers
                        case (char)7:
                            {
                                MethodInvoker inv2 = delegate
                                {
                                    string message = "";
                                    DialogResult dlg = DialogResult.No;
                                    //If answer is GetOffers //By this i get also enemies who want to play with me
                                    if (answer.Split(' ').Count() > 2) //msg value and eof
                                    {
                                        //Find out who want to play with you                               
                                        OfferingGame offeringGame = new OfferingGame(answer);
                                        dlg = offeringGame.ShowDialog();
                                    }
                                    if (dlg == DialogResult.Yes/*Program.dialog == 1*/)
                                    {
                                        if (enemyNick != "")
                                        {
                                            //Inform server with who you want to play -> Send Server Agree
                                            message = (char)12 + " " + userLogin + " " + enemyNick + " <EOF>"; //enemies except me
                                            client.Send(message);
                                            enemyNick = enemySelect.enemyNick;
                                            enemySelect.DialogResult = DialogResult.OK;
                                            //Program.dialog = 2;
                                        }
                                    }
                                    else
                                    {
                                        if (answer.Split(' ').Count() > 2)
                                        {
                                            //Inform server with who you want to play
                                            message = (char)12 + " " + userLogin + " " + userLogin + " <EOF>"; //enemies except me
                                            client.Send(message);
                                        }
                                        enemySelect.updateTimer.Enabled = true;
                                    }
                                }; enemySelect.Invoke(inv2);

                                break;
                            }
                        //OK - Enemy accepted my offer to play
                        case (char)10:
                            {
                                enemyNick = enemySelect.enemyNick;
                                enemySelect.DialogResult = DialogResult.OK;
                                break;
                            }
                        //Fail - Enemy declined my offer to play
                        case (char)9:
                            {
                                MethodInvoker inv = delegate
                                {
                                    enemySelect.agreeButton.Enabled = true;
                                    enemySelect.updateTimer.Enabled = true;
                                }; enemySelect.Invoke(inv);
                                //enemySelect.updateTimer.Enabled = true;
                                break;
                            }
                        //Form1
                        //
                        case (char)16://Enemy wants to play and it's his turn
                            {
                                MethodInvoker inv = delegate
                                {
                                    //Hide button
                                    main.clickedButton.Visible = false;
                                    main.PrepareEnemyField();
                                    Application.DoEvents();
                                    ((Panel)main.Controls.Find("P2", true).FirstOrDefault()).Enabled = false;
                                }; main.Invoke(inv);
                                break;
                            }
                        //Enemy wants to play and it's my turn
                        case (char)0:
                            {
                                MethodInvoker inv = delegate
                                {
                                    //Hide button
                                    main.clickedButton.Visible = false;
                                    main.PrepareEnemyField();
                                    ((Panel)main.Controls.Find("P2", true).FirstOrDefault()).Enabled = true;
                                }; main.Invoke(inv);
                                break;
                            }
                        //Enemy Gave Up in  FormGame
                        case (char)17:
                            {
                                MethodInvoker inv = delegate
                                {
                                    main.enemyGiveUpBeforeStart = true;
                                    ((Panel)main.Controls.Find("P2", true).FirstOrDefault()).Enabled = false;
                                    MessageBox.Show("Enemy gave up, you won!", "Congratulations!");
                                    main.normalEnd = true;
                                    main.DialogResult = DialogResult.Yes;
                                }; main.Invoke(inv);
                                break;
                            }
                        //Miss
                        case (char)4:
                            {
                                MethodInvoker inv = delegate
                                {
                                    main.clickedButton.BackColor = Color.Silver;
                                    ((Panel)main.Controls.Find("P2", true).FirstOrDefault()).Enabled = false;
                                }; main.Invoke(inv);
                                break;
                            }
                        //Hit
                        case (char)5:
                            {
                                MethodInvoker inv = delegate
                                {
                                    int x = Int32.Parse(main.clickedButton.Name.Substring(0, 1)); //get x button co-ordinates
                                    int y = Int32.Parse(main.clickedButton.Name.Substring(1, 1)); //get y button co-ordinates
                                    main.enemyMap[x, y] = true;
                                    main.clickedButton.BackColor = Color.Crimson;
                                    ((Panel)main.Controls.Find("P2", true).FirstOrDefault()).Enabled = true;
                                }; main.Invoke(inv);
                                break;
                            }
                        //SinkShip
                        case (char)3:
                            {
                                MethodInvoker inv = delegate
                                {
                                    int x = Int32.Parse(main.clickedButton.Name.Substring(0, 1)); //get x button co-ordinates
                                    int y = Int32.Parse(main.clickedButton.Name.Substring(1, 1)); //get y button co-ordinates
                                    main.enemyMap[x, y] = true;
                                    main.clickedButton.BackColor = Color.Tomato;
                                    ((Panel)main.Controls.Find("P2", true).FirstOrDefault()).Enabled = true;
                                }; main.Invoke(inv);
                                break;
                            }
                        //WinGame
                        case (char)18:
                            {
                                MethodInvoker inv = delegate
                                {
                                    int x = Int32.Parse(main.clickedButton.Name.Substring(0, 1)); //get x button co-ordinates
                                    int y = Int32.Parse(main.clickedButton.Name.Substring(1, 1)); //get y button co-ordinates
                                    main.enemyMap[x, y] = true;
                                    main.clickedButton.BackColor = Color.Crimson;
                                    ((Panel)main.Controls.Find("P2", true).FirstOrDefault()).Enabled = false;
                                    MessageBox.Show("You win!", "Success!");
                                    main.normalEnd = true;
                                    main.DialogResult = DialogResult.Yes;
                                }; main.Invoke(inv);
                                break;
                            }
                        //loseGame
                        case (char)6:
                            {
                                MethodInvoker inv = delegate
                                {
                                    string message = "";
                                    int x = -1;
                                    int y = -1;
                                    if (answer[0] == (char)6)
                                    {
                                        x = Int32.Parse(answer.Split(' ')[1]);
                                        y = Int32.Parse(answer.Split(' ')[2]);
                                        main.GetShotAndResponse(x, y);
                                        if (main.sphips == 0)
                                        {
                                            ((Panel)main.Controls.Find("P2", true).FirstOrDefault()).Enabled = false;
                                            message = (char)1 + " " + userLogin + " " + enemyNick + " <EOF>";
                                            client.Send(message);
                                            main.normalEnd = true;
                                            MessageBox.Show("You lose!", ":(");
                                            main.DialogResult = DialogResult.Yes;
                                        }
                                    }
                                }; main.Invoke(inv);
                                break;
                            }

                    }
                }
                catch (Exception)
                {
                    isThreadRunning = false;
                }
            }
        }
    }
}
