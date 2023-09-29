using BattleShipServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipServerConsole.Classes
{

    class SocketListener
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        public SocketListener() { }

        public static void Start()
        {

            byte[] bytes = new byte[1024];
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[4];
            //IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                Console.WriteLine($"Server is running {ipAddress} ");
                while (true)
                {
                    allDone.Reset();
                    listener.BeginAccept(new AsyncCallback(Accept), listener);
                    allDone.WaitOne();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.Read();

        }

        public static void Accept(IAsyncResult res)
        {

            allDone.Set();
            Socket listener = (Socket)res.AsyncState;
            Socket handler = listener.EndAccept(res);
            ReadObject state = new ReadObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, ReadObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult res) // основная логика сервера
        {
            string content = string.Empty;
            ReadObject state = (ReadObject)res.AsyncState;
            Socket handler = state.workSocket;
            int bytesRead = 0;
            bytesRead = handler.EndReceive(res);

            if (bytesRead > 0)
            {
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                content = state.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {

                    Console.WriteLine("Read {0} bytes from socket. \nData : {1}", content.Length, content);
                    string messageAction = Message.getBinaryMessage(content);
                    int action = Convert.ToInt32(messageAction.Substring(0, 8), 2);
                    string[] parameters = content.Split(' ');
                    string nick = string.Empty;
                    string IPport = string.Empty;
                    string port = string.Empty;
                    string enemyNick = string.Empty;
                    string players = string.Empty;
                    bool result = false;
                    string whoSent = "";
                    string whomSent = "";
                    switch (action)
                    {
                        case 0://начало игры 
                            {
                                whoSent = parameters[1];
                                whomSent = parameters[2];

                                if (Program.loggedplayingNicks.ContainsKey(whomSent))
                                {
                                    if (!Program.whowhomSentStart.Contains(whomSent + whoSent) && !Program.whowhomSentGiveUp.Contains(whomSent + whoSent))
                                    {
                                        Program.whowhomSentStart.Add(whoSent + whomSent);
                                        state.buffer = new byte[1024];
                                        state.sb = new StringBuilder();
                                        handler.BeginReceive(state.buffer, 0, ReadObject.BufferSize, 0,
                                        new AsyncCallback(ReadCallback), state);
                                        break;
                                    }
                                    else if (Program.whowhomSentStart.Contains(whomSent + whoSent))
                                    {
                                        //Send OK to both players
                                        if (Program.loggedplayingNicks.ContainsKey(whoSent))
                                        {
                                            Send(Program.loggedplayingNicks[whoSent], ((char)16).ToString() + " <EOF>");
                                        }
                                        if (Program.loggedplayingNicks.ContainsKey(whomSent))
                                        {
                                            Send(Program.loggedplayingNicks[whomSent], ((char)0).ToString() + " <EOF>");
                                        }
                                        Program.whowhomSentStart.Remove(whomSent + whoSent);
                                        state.buffer = new byte[1024];
                                        state.sb = new StringBuilder();
                                        handler.BeginReceive(state.buffer, 0, ReadObject.BufferSize, 0,
                                        new AsyncCallback(ReadCallback), state);
                                        using (MyApplicationContext context = new MyApplicationContext())
                                        {
                                            context.Moves.Add(new Move() { Description = "Game start", Date = DateTime.Now.ToString() });
                                            context.SaveChanges();
                                        }
                                            break;

                                    }
                                    else if (Program.whowhomSentGiveUp.Contains(whomSent + whoSent))//Check if whom+who is on whowhomSentGiveUp
                                    {
                                        if (Program.loggedplayingNicks.ContainsKey(whoSent))
                                        {
                                            Send(Program.loggedplayingNicks[whoSent], ((char)17).ToString() + " <EOF>");
                                        }
                                        Program.whowhomSentGiveUp.Remove(whomSent + whoSent);
                                    }
                                }
                                else if (Program.loggedplayingNicks.ContainsKey(whoSent))
                                {
                                    Send(Program.loggedplayingNicks[whoSent], ((char)17).ToString() + " <EOF>");
                                    Program.loggedNicks.Add(whoSent, Program.loggedplayingNicks[whoSent]);
                                    Program.loggedplayingNicks.Remove(whoSent);
                                }
                                else if (Program.loggedNicks.ContainsKey(whoSent))
                                {
                                    Send(Program.loggedNicks[whoSent], ((char)17).ToString() + " <EOF>");
                                }
                                state.buffer = new byte[1024];
                                state.sb = new StringBuilder();
                                handler.BeginReceive(state.buffer, 0, ReadObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                                break;
                            }
                        case 1://EndGame
                            {
                                nick = parameters[1];
                                enemyNick = parameters[2];

                                if (Program.loggedplayingNicks.ContainsKey(nick))
                                {
                                    if (!Program.loggedNicks.ContainsKey(nick))
                                    {
                                        Program.loggedNicks.Add(nick, Program.loggedplayingNicks[nick]);
                                        Program.loggedplayingNicks.Remove(nick);
                                    }
                                }
                                if (Program.loggedplayingNicks.ContainsKey(enemyNick))
                                {
                                    if (!Program.loggedNicks.ContainsKey(enemyNick))
                                    {
                                        Program.loggedNicks.Add(enemyNick, Program.loggedplayingNicks[enemyNick]);
                                        Program.loggedplayingNicks.Remove(enemyNick);
                                        Send(Program.loggedNicks[enemyNick], ((char)18).ToString() + " <EOF>");
                                        using (MyApplicationContext context = new MyApplicationContext())
                                        {
                                            context.Moves.Add(new Move() { Description = $"{nick} is win", Date = DateTime.Now.ToString() });
                                            context.SaveChanges();
                                        }
                                        break;
                                    }
                                }
                                state.buffer = new byte[1024];
                                state.sb = new StringBuilder();
                                handler.BeginReceive(state.buffer, 0, ReadObject.BufferSize, 0,
                                new AsyncCallback(ReadCallback), state);
                                break;
                            }
                        case 2: //Give up
                            {
                                //Get nick
                                whoSent = parameters[1];
                                //Get nick
                                whomSent = parameters[2];
                                //Check if whomSent has sent message earlier

                                //Check if whom+who is on whowhomSentStart & whowhomSentGiveUp
                                if (!Program.whowhomSentStart.Contains(whomSent + whoSent) && !Program.whowhomSentGiveUp.Contains(whomSent + whoSent))
                                {
                                    Program.whowhomSentGiveUp.Add(whoSent + whomSent);
                                    if (Program.loggedplayingNicks.ContainsKey(whoSent))
                                    {
                                        if (!Program.loggedNicks.ContainsKey(whoSent))
                                        {
                                            Program.loggedNicks.Add(whoSent, Program.loggedplayingNicks[whoSent]);
                                            Program.loggedplayingNicks.Remove(whoSent);
                                            Send(Program.loggedNicks[whoSent], ((char)10).ToString() + " <EOF>");
                                        }
                                    }
                                    state.buffer = new byte[1024];
                                    state.sb = new StringBuilder();
                                    handler.BeginReceive(state.buffer, 0, ReadObject.BufferSize, 0,
                                    new AsyncCallback(ReadCallback), state);
                                    break;
                                }
                                else if (Program.whowhomSentStart.Contains(whomSent + whoSent))//Check if whom+who is on whowhomSentStart
                                {
                                    //Send Fail to whom player
                                    if (Program.loggedplayingNicks.ContainsKey(whomSent))
                                    {
                                        Send(Program.loggedplayingNicks[whomSent], ((char)9).ToString() + " <EOF>");
                                    }
                                    //Remove both players from whowhomSentStart
                                    Program.whowhomSentStart.Remove(whomSent + whoSent);

                                    //Remove both players from loggedplayingNicks
                                    if (Program.loggedplayingNicks.ContainsKey(whomSent))
                                    {
                                        if (!Program.loggedNicks.ContainsKey(whomSent))
                                        {
                                            Program.loggedNicks.Add(whoSent, Program.loggedplayingNicks[whomSent]);
                                            Program.loggedplayingNicks.Remove(whomSent);
                                        }
                                    }
                                    if (Program.loggedplayingNicks.ContainsKey(whoSent))
                                    {
                                        if (!Program.loggedNicks.ContainsKey(whoSent))
                                        {
                                            Program.loggedNicks.Add(whoSent, Program.loggedplayingNicks[whoSent]);
                                            Program.loggedplayingNicks.Remove(whoSent);
                                            Send(Program.loggedNicks[whoSent], ((char)10).ToString() + " <EOF>");
                                        }
                                    }
                                    state.buffer = new byte[1024];
                                    state.sb = new StringBuilder();
                                    handler.BeginReceive(state.buffer, 0, ReadObject.BufferSize, 0,
                                    new AsyncCallback(ReadCallback), state);
                                    break;
                                }
                                else if (Program.whowhomSentGiveUp.Contains(whomSent + whoSent))//Check if whom+who is on whowhomSentGiveUp
                                {
                                    Program.whowhomSentGiveUp.Remove(whomSent + whoSent);
                                    if (Program.loggedplayingNicks.ContainsKey(whoSent))
                                    {
                                        if (!Program.loggedNicks.ContainsKey(whoSent))
                                        {
                                            Program.loggedNicks.Add(whoSent, Program.loggedplayingNicks[whoSent]);
                                            Program.loggedplayingNicks.Remove(whoSent);
                                            Send(Program.loggedNicks[whoSent], ((char)10).ToString() + " <EOF>");
                                        }

                                    }
                                }
                                state.buffer = new byte[1024];
                                state.sb = new StringBuilder();
                                handler.BeginReceive(state.buffer, 0, ReadObject.BufferSize, 0,
                                new AsyncCallback(ReadCallback), state);
                                break;
                            }
                        case 3: //Start
                            {
                                if (Program.loggedNicks == null )
                                {
                                    string message = ((char)21).ToString() + " <EOF>";
                                    Send(handler, message);
                                }
                                else
                                {
                                    string message = ((char)22).ToString() + " <EOF>";
                                    Send(handler, message);
                                }


                                break;
                            }
                        case 4: //Miss
                            {
                                enemyNick = parameters[1];
                                if (Program.loggedplayingNicks.ContainsKey(enemyNick))
                                {
                                    string message = ((char)4).ToString() + " <EOF>";
                                    Send(Program.loggedplayingNicks[enemyNick], message);
                                }
                                state.buffer = new byte[1024];
                                state.sb = new StringBuilder();
                                handler.BeginReceive(state.buffer, 0, ReadObject.BufferSize, 0,
                                new AsyncCallback(ReadCallback), state);
                                using (MyApplicationContext context = new MyApplicationContext())
                                {
                                    if (parameters[1] == "Player1")
                                        context.Moves.Add(new Move() { Description = "Player 2 is miss", Date = DateTime.Now.ToString() });
                                    else
                                        context.Moves.Add(new Move() { Description = "Player 1 is miss", Date = DateTime.Now.ToString() });
                                    context.SaveChanges();
                                }
                                break;
                            }
                        case 5: //Hit
                            {
                                enemyNick = parameters[1];
                                if (Program.loggedplayingNicks.ContainsKey(enemyNick))
                                {
                                    string message = ((char)5).ToString() + " <EOF>";
                                    Send(Program.loggedplayingNicks[enemyNick], message);
                                }
                                state.buffer = new byte[1024];
                                state.sb = new StringBuilder();
                                handler.BeginReceive(state.buffer, 0, ReadObject.BufferSize, 0,
                                new AsyncCallback(ReadCallback), state);
                                using (MyApplicationContext context = new MyApplicationContext())
                                {
                                    if (parameters[1] == "Player1")
                                        context.Moves.Add(new Move() { Description = "Player 2 is hit", Date = DateTime.Now.ToString() });
                                    else
                                        context.Moves.Add(new Move() { Description = "Player 1 is hit", Date = DateTime.Now.ToString() });
                                    context.SaveChanges();
                                }
                                break;
                            }
                        case 6://Shot
                            {
                                string Nick = parameters[1];
                                string x = parameters[2];
                                string y = parameters[3];
                                if (Program.loggedplayingNicks.ContainsKey(Nick))
                                {
                                    string message = ((char)6).ToString() + " " + x + " " + y + " <EOF>";
                                    Send(Program.loggedplayingNicks[Nick], message);
                                }

                                char fieldy = (char)(65 + int.Parse(y));
                                int fieldx = int.Parse(x) + 1;
                                using (MyApplicationContext context = new MyApplicationContext())
                                {
                                    if (parameters[1] == "Player1")
                                        context.Moves.Add(new Move() { Description = $"Player 1 shoot in {fieldx} - {fieldy} field ", Date = DateTime.Now.ToString() });
                                    else
                                        context.Moves.Add(new Move() { Description = $"Player 2 shoot in {fieldx} - {fieldy} field", Date = DateTime.Now.ToString() });
                                    context.SaveChanges();
                                }
                                state.buffer = new byte[1024];
                                state.sb = new StringBuilder();
                                handler.BeginReceive(state.buffer, 0, ReadObject.BufferSize, 0,
                                new AsyncCallback(ReadCallback), state);





                                break;
                            }
                        case 7://Запрос от  соперника
                            {
                                nick = parameters[1];
                                if (Program.enemiesoffers.ContainsKey(nick))
                                {
                                    string enemiesString = "";
                                    foreach (var item in Program.enemiesoffers[nick])
                                    {
                                        enemiesString += item + " ";
                                    }
                                    string message = ((char)7).ToString() + " " + enemiesString + "<EOF>";
                                    Send(handler, message);
                                }
                                else//Fail
                                {
                                    Send(handler, ((char)7).ToString() + " <EOF>");
                                }
                                state.buffer = new byte[1024];
                                state.sb = new StringBuilder();
                                handler.BeginReceive(state.buffer, 0, ReadObject.BufferSize, 0,
                                new AsyncCallback(ReadCallback), state);
                                break;
                            }
                        case 8://Запрос от игрока
                            {

                                nick = parameters[1];
                                enemyNick = parameters[2];
                                bool nickOffers = false;
                                if (Program.enemiesoffers.ContainsKey(nick))
                                {
                                    if (Program.enemiesoffers[nick].Contains(enemyNick))
                                    {
                                        Send(handler, ((char)10).ToString() + " <EOF>");
                                        nickOffers = true;
                                    }
                                }
                                if (nickOffers == false)
                                {
                                    if (Program.enemiesoffers.ContainsKey(enemyNick))
                                    {
                                        Program.enemiesoffers[enemyNick].Add(nick);
                                    }
                                    else
                                    {
                                        Program.enemiesoffers.Add(enemyNick, new List<string>() { nick });
                                    }
                                }
                                state.buffer = new byte[1024];
                                state.sb = new StringBuilder();
                                handler.BeginReceive(state.buffer, 0, ReadObject.BufferSize, 0,
                                new AsyncCallback(ReadCallback), state);
                                break;
                            }
                        case 9: //Enemy declined my offer to play
                            {
                                Send(handler, ((char)10).ToString() + " <EOF>");
                                state.buffer = new byte[1024];
                                state.sb = new StringBuilder();
                                handler.BeginReceive(state.buffer, 0, ReadObject.BufferSize, 0,
                                new AsyncCallback(ReadCallback), state);
                                break;
                            }
                        case 10: //Enemy accepted my offer to play
                            {
                                Send(handler, ((char)10).ToString() + " <EOF>");

                                state.buffer = new byte[1024];
                                state.sb = new StringBuilder();
                                handler.BeginReceive(state.buffer, 0, ReadObject.BufferSize, 0,
                                new AsyncCallback(ReadCallback), state);
                                break;
                            }
                        case 11: //Conecting
                            {
                                //Get nick
                                if (Program.loggedNicks.Count==0)
                                //nick = parameters[1];
                                nick = "Player1";
                                else
                                nick = "Player2";
                                IPport = handler.RemoteEndPoint.ToString().Split(':')[0]; //get IP
                                port = parameters[2];
                                IPport += ":" + port;
                                Program.loggedNicks.Add(nick, handler);
                                if (nick == "Player1")
                                Send(handler, ((char)10).ToString() + " <EOF>");
                                else 
                                Send(handler, ((char)21).ToString() + " <EOF>");
                                

                                state.buffer = new byte[1024];
                                state.sb = new StringBuilder();
                                handler.BeginReceive(state.buffer, 0, ReadObject.BufferSize, 0,
                                new AsyncCallback(ReadCallback), state);
                                break;
                            }
                        case 12: //Agree to play
                            {
                                nick = parameters[1];
                                enemyNick = parameters[2];
                                //Send OK to enemy you want to play
                                if (Program.enemiesoffers.ContainsKey(nick))
                                {
                                    if (nick != enemyNick) //If i don't decline
                                    {
                                        if (Program.enemiesoffers[nick].Contains(enemyNick))
                                        {
                                            if (Program.loggedNicks.ContainsKey(enemyNick))
                                            {
                                                Send(Program.loggedNicks[enemyNick], ((char)10).ToString() + " <EOF>");
                                                Program.loggedplayingNicks.Add(nick, Program.loggedNicks[nick]);
                                                Program.loggedplayingNicks.Add(enemyNick, Program.loggedNicks[enemyNick]);
                                                if (Program.enemiesoffers.ContainsKey(enemyNick))
                                                {
                                                    //decline offers to enemy
                                                    foreach (var item in Program.enemiesoffers[enemyNick])
                                                    {
                                                        //Send Fail to Rest
                                                        if (Program.loggedNicks.ContainsKey(item))
                                                        {
                                                            Send(Program.loggedNicks[item], ((char)9).ToString() + " <EOF>");
                                                        }
                                                    }
                                                }
                                                Program.loggedNicks.Remove(nick);
                                                Program.loggedNicks.Remove(enemyNick);
                                            }
                                            else
                                            {
                                                //Enemy Give up! You win!
                                                Send(Program.loggedNicks[nick], ((char)17).ToString() + " <EOF>");
                                            }
                                            Program.enemiesoffers[nick].Remove(enemyNick);
                                            //Program.loggedNicks.Remove(enemyNick);
                                        }
                                    }
                                    foreach (var item in Program.enemiesoffers[nick])
                                    {
                                        //Send Fail to Rest
                                        if (Program.loggedNicks.ContainsKey(item))
                                        {
                                            Send(Program.loggedNicks[item], ((char)9).ToString() + " <EOF>");
                                        }
                                    }
                                    Program.enemiesoffers[nick].Clear(); //Clear list
                                }

                                state.buffer = new byte[1024];
                                state.sb = new StringBuilder();
                                handler.BeginReceive(state.buffer, 0, ReadObject.BufferSize, 0,
                                new AsyncCallback(ReadCallback), state);
                                break;
                            }
                        case 13: //GetEnemies <nick>
                            {
                                //Get nick
                                nick = parameters[1];
                                //Check if nick is in dict
                                result = Program.loggedNicks.ContainsKey(nick);
                                    players = "";
                                    foreach (var item in Program.loggedNicks)
                                    {
                                        if (!item.Key.Equals(nick))
                                            players += item.Key + ";" + item.Value.LocalEndPoint + " ";
                                    }
                                    if (players == "") //if nobody's online send Fail Communique
                                    {
                                        Send(handler, ((char)12).ToString() + " <EOF>");
                                    }
                                    else //Send Send Enemies Communique
                                    {
                                        players += "<EOF>";
                                        players = ((char)12).ToString() + " " + players;

                                        //Send enemies
                                        Send(handler, players);
                                    }
                                //}

                                state.buffer = new byte[1024];
                                state.sb = new StringBuilder();
                                handler.BeginReceive(state.buffer, 0, ReadObject.BufferSize, 0,
                                new AsyncCallback(ReadCallback), state);
                                break;
                            }
                        case 14://Close app
                            {
                                whoSent = parameters[1];
                                //Get nick
                                whomSent = parameters[2];
                                //

                                //Check if nick is in dict
                                if (Program.loggedNicks.ContainsKey(whoSent) == true)
                                {
                                    //Program.whowhomSentGiveUp.Add(whoSent + whomSent);
                                    Program.loggedNicks.Remove(whoSent);
                                }
                                if (Program.loggedplayingNicks.ContainsKey(whoSent) == true)
                                {
                                    Program.loggedplayingNicks.Remove(whoSent);
                                }

                                if (Program.enemiesoffers.ContainsKey(whoSent))
                                {
                                    //decline offers to enemy
                                    foreach (var item in Program.enemiesoffers[whoSent])
                                    {
                                        //Send Fail to Rest
                                        if (Program.loggedNicks.ContainsKey(item))
                                        {
                                            Send(Program.loggedNicks[item], ((char)9).ToString() + " <EOF>");
                                        }
                                    }
                                }

                                handler.Shutdown(SocketShutdown.Both);
                                handler.Close();
                                break;
                            }
                        case 15://GiveUp in MainGame
                            {
                                nick = parameters[1];
                                enemyNick = parameters[2];

                                if (Program.loggedplayingNicks.ContainsKey(nick))
                                {
                                    if (!Program.loggedNicks.ContainsKey(nick))
                                    {
                                        Program.loggedNicks.Add(nick, Program.loggedplayingNicks[nick]);
                                        Program.loggedplayingNicks.Remove(nick);
                                    }
                                }
                                if (Program.loggedplayingNicks.ContainsKey(enemyNick))
                                {
                                    if (!Program.loggedNicks.ContainsKey(enemyNick))
                                    {
                                        Program.loggedNicks.Add(enemyNick, Program.loggedplayingNicks[enemyNick]);
                                        Program.loggedplayingNicks.Remove(enemyNick);
                                        Send(Program.loggedNicks[enemyNick], ((char)17).ToString() + " <EOF>");
                                    }
                                }
                                state.buffer = new byte[1024];
                                state.sb = new StringBuilder();
                                handler.BeginReceive(state.buffer, 0, ReadObject.BufferSize, 0,
                                new AsyncCallback(ReadCallback), state);
                                break;
                            }
                    }
                }

            }
            else
            {
                Console.WriteLine("Not all data received");
                handler.BeginReceive(state.buffer, 0, ReadObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
            }
        }

        private static void Send(Socket handler, string data)
        {

            byte[] byteData = Encoding.ASCII.GetBytes(data);
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult res)
        {
            try
            {
                Socket handler = (Socket)res.AsyncState; // возвращает объект состояния задачи
                int bytesSent = handler.EndSend(res);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
