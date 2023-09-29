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
    public partial class FormGame : Form
    {
        string enemyNick;
        public bool enemyGiveUpBeforeStart = false;
        public bool normalEnd = false;
        public Button clickedButton;
        public int sphips = 20;
        public FormGame(string enemyNick)
        {
            InitializeComponent();
            this.enemyNick = enemyNick;
        }

        public bool[,] yourMap = new bool[10, 10];
        public bool[,] yourMapTmp = new bool[10, 10];
        public bool[,] enemyMap = new bool[10, 10];
        List<Button> selectedButtons = new List<Button>();

        //======================================== Prepare logic
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Battleship - you're playing with " + enemyNick;
            SetShips();
            timer1.Start();

        }
        private void timer1_Tick(object sender, EventArgs e)/// play button green
        {

            bool checkResult = false;
            Array.Clear(yourMapTmp, 0, yourMapTmp.Length);
            checkResult = Check1CellShip();
            if (checkResult == false)
            {
                return;
            }
            checkResult = Check2CellShip();
            if (checkResult == false)
            {
                return;
            }
            checkResult = Check3CellShip();
            if (checkResult == false)
            {
                return;
            }
            checkResult = Check4CellShip();
            if (checkResult == false)
            {
                return;
            }
            else if (BPlay.Text != "Wait opponent ....")
            {
                BPlay.BackColor = Color.Green;
                BPlay.Text = "Start Play";
            }

        }
        private void SetShips()
        {
            timer1.Stop();
            GenerateField("P1", 204, 275);
            setDefaultValuesInMap(false, yourMap);
            for (int i = 0; i < yourMapTmp.GetLength(1); i++)
            {
                for (int j = 0; j < yourMapTmp.GetLength(0); j++)
                {
                    yourMapTmp[i, j] = false;
                }
            }
        }

        private void GenerateField(string name, int xStartPos, int yStartPos)
        {
            Panel buttonPanel = new Panel();
            buttonPanel.Name = name;
            buttonPanel.Size = new Size(231, 231);
            int xButtonSize = 21;
            int yButtonSize = 21;
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (i == 0 && j == 0) continue;
                    Button button = new Button();
                    buttonPanel.Controls.Add(button);
                    button.Location = new System.Drawing.Point(j * xButtonSize, i * yButtonSize);
                    button.Size = new Size(xButtonSize, yButtonSize);
                    button.ForeColor = System.Drawing.Color.Navy;
                    if (i > 0 && j > 0) 
                    {
                        button.Name = (i - 1).ToString() + (j - 1).ToString();
                        button.Font = new Font(button.Font.FontFamily, 6);
                        if (name == "P1")
                        {
                            button.Click += new System.EventHandler(this.yourFieldButtonClick);
                        }
                        else if (name == "P2")
                        {
                            button.Click += new System.EventHandler(this.buttonClick);
                        }
                    }
                    else
                    {
                        button.Enabled = false;
                        if (i == 0 && j > 0) // A, B, С
                        {
                            button.Text = ((char)(64 + j)).ToString();
                            button.Name = ((char)(64 + j)).ToString();
                            button.Font = new Font(button.Font.FontFamily, 6);
                        }
                        else if (i != 0 || j != 0) // 1, 2, 3
                        {
                            button.Text = i.ToString();
                            button.Name = "L" + i.ToString();
                            button.Font = new Font(button.Font.FontFamily, 6);
                        }
                    }
                }
            }
            this.Controls.Add(buttonPanel);
            buttonPanel.Location = new Point(xStartPos, yStartPos);
        }
        private void setDefaultValuesInMap(bool value, bool [,] Map)
        {
            for (int i = 0; i < Map.GetLength(1); i++)
            {
                for (int j = 0; j < Map.GetLength(0); j++)
                {
                    Map[i, j] = value;
                }
            }
        }
        public void PrepareEnemyField()
        {

            PanelYourShip.Visible = false;
            Panel matched = (Panel)this.Controls.Find("P1", true).FirstOrDefault();
            matched.Visible = false;
            matched.Enabled = false;
            if (matched != null)
            {
                matched.Location = new Point(matched.Location.X - 164, matched.Location.Y);
            }
            matched.Visible = true;
            GenerateField("P2", 370, 275);
            PMast.Visible = true;// field enemy
            Array.Clear(yourMapTmp, 0, yourMapTmp.Length);
        }


        void yourFieldButtonClick(object sender, EventArgs e)
        {
            var clickedButton = (Button)sender;
            int x = Int32.Parse(clickedButton.Name.Substring(0, 1)); 
            int y = Int32.Parse(clickedButton.Name.Substring(1, 1)); 
            int leftShip = IsLeftNeighbour(x, y);
            int rightShip = IsRightNeighbour(x, y);
            int upShip = IsUpNeighbour(x, y);
            int downShip = IsDownNeighbour(x, y);

            //Select or not
            if (clickedButton.BackColor != Color.MediumBlue) //if button not selected
            {
                if ((leftShip + rightShip < 4) && (upShip + downShip < 4))
                {
                    clickedButton.BackColor = Color.MediumBlue;
                    selectedButtons.Add(clickedButton);
                    DisableOrEnableAllCorners((Panel)clickedButton.Parent, x, y, false);
                    yourMap[x, y] = true;
                }
            }
            else
            {
                clickedButton.BackColor = Color.Transparent;
                DisableOrEnableAllCorners((Panel)clickedButton.Parent, x, y, true);
                selectedButtons.Remove(clickedButton);
                foreach (Button btn in selectedButtons)
                {
                    DisableOrEnableAllCorners((Panel)btn.Parent, Int32.Parse(btn.Name[0].ToString()), Int32.Parse(btn.Name[1].ToString()), false);
                }
                yourMap[x, y] = false;
            }
        }
        private int IsLeftNeighbour(int x, int y)
        {
            int x1=x;
            int y1=y-1;
            if (y1>-1)
            {
                if (yourMap[x1,y1]==true) 
                {
                    return 1 + IsLeftNeighbour(x1, y1);
                }
                else 
                {
                    return 0;
                }
            }
            else 
            {
                return 0;
            }
        }
        private int IsRightNeighbour(int x, int y)
        {
            int x1 = x;
            int y1 = y + 1;
            if (y1 < 10)
            {
                if (yourMap[x1, y1] == true) 
                {
                    return 1 + IsRightNeighbour(x1, y1);
                }
                else 
                {
                    return 0;
                }
            }
            else 
            {
                return 0;
            }
        }
        private int IsUpNeighbour(int x, int y)
        {
            int x1 = x - 1;
            int y1 = y;
            if (x1 > -1)
            {
                if (yourMap[x1, y1] == true) 
                {
                    return 1 + IsUpNeighbour(x1, y1);
                }
                else 
                {
                    return 0;
                }
            }
            else 
            {
                return 0;
            }
        }
        private int IsDownNeighbour(int x, int y)
        {
            int x1 = x + 1;
            int y1 = y;
            if (x1 < 10)
            {
                if (yourMap[x1, y1] == true) 
                {
                    return 1 + IsDownNeighbour(x1, y1);
                }
                else 
                {
                    return 0;
                }
            }
            else 
            {
                return 0;
            }
        }
        private void DisableOrEnableAllCorners(Panel panel, int x, int y, bool trueOrFalse)
        {
            DisableOrEnableButtonIfExists(panel, x-1, y-1, trueOrFalse);
            DisableOrEnableButtonIfExists(panel, x-1, y+1, trueOrFalse);
            DisableOrEnableButtonIfExists(panel, x+1, y+1, trueOrFalse);
            DisableOrEnableButtonIfExists(panel, x+1, y-1, trueOrFalse);
        }
        private void DisableOrEnableButtonIfExists(Panel panel, int x1, int y1, bool Exist)
        {
            Button button;
            button = (Button)panel.Controls.Find(x1.ToString() + y1.ToString(), true).FirstOrDefault();
            if (button != null)
            {
                if (Exist == false)
                {
                    button.Enabled = false;
                }
                else
                {
                    button.Enabled = true;
                }
            }
        }
        bool Check1CellShip()
        {
            int leftShip = 0;
            int rightShip = 0;
            int upShip = 0;
            int downShip = 0;

            int counter = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (yourMap[i, j] == true)
                    {
                        leftShip = IsLeftNeighbour(i, j);
                        rightShip = IsRightNeighbour(i, j);
                        upShip = IsUpNeighbour(i, j);
                        downShip = IsDownNeighbour(i, j);
                        if (leftShip==0 && rightShip==0 && downShip ==0 && upShip==0)
                        {
                            yourMapTmp[i, j] = true;
                            counter++;
                        }
                        if (counter > 4) return false;
                    }

                }
            }
            if (counter < 4) return false;
            return true;
        }
        bool Check2CellShip()
        {
            int leftShip = 0;
            int rightShip = 0;
            int upShip = 0;
            int downShip = 0;
            int counter = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (yourMap[i, j] == true)
                    {
                        if (yourMapTmp[i, j] == true) continue;
                        downShip = IsDownNeighbour(i, j);
                        upShip = IsUpNeighbour(i, j);
                        if (downShip == 1 && upShip==0)
                        {
                            yourMapTmp[i, j] = true;
                            yourMapTmp[i + 1, j] = true;
                            counter++;
                        }
                        else if (downShip==0 && upShip==0)
                        {
                            rightShip = IsRightNeighbour(i, j);
                            leftShip = IsLeftNeighbour(i, j);
                            if (rightShip==1 && leftShip==0)
                            {
                                yourMapTmp[i, j] = true;
                                yourMapTmp[i, j+1] = true;
                                counter++;
                            }
                        }
                        if (counter > 3) return false;
                    }
                }
            }
            if (counter < 3) return false;
            return true;
        }
        bool Check3CellShip()
        {
            int leftShip = 0;
            int rightShip = 0;
            int upShip = 0;
            int downShip = 0;
            int counter = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (yourMap[i, j] == true)
                    {
                        if (yourMapTmp[i, j] == true) continue;
                        downShip = IsDownNeighbour(i, j);
                        upShip = IsUpNeighbour(i, j);
                        if (downShip == 2 && upShip == 0)
                        {
                            yourMapTmp[i, j] = true;
                            yourMapTmp[i + 1, j] = true;
                            yourMapTmp[i + 2, j] = true;
                            counter++;
                        }
                        else if (downShip == 0 && upShip == 0)
                        {
                            rightShip = IsRightNeighbour(i, j);
                            leftShip = IsLeftNeighbour(i, j);
                            if (rightShip == 2 && leftShip == 0)
                            {
                                yourMapTmp[i, j] = true;
                                yourMapTmp[i, j + 1] = true;
                                yourMapTmp[i, j + 2] = true;
                                counter++;
                            }
                        }
                        if (counter > 2) return false;
                    }
                }
            }
            if (counter < 2) return false;
            return true;
        }
        bool Check4CellShip()
        {
            int leftShip = 0;
            int rightShip = 0;
            int upShip = 0;
            int downShip = 0;
            int counter = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (yourMap[i, j] == true)
                    {
                        if (yourMapTmp[i, j] == true) continue;
                        downShip = IsDownNeighbour(i, j);
                        upShip = IsUpNeighbour(i, j);
                        if (downShip == 3 && upShip == 0)
                        {
                            yourMapTmp[i, j] = true;
                            yourMapTmp[i + 1, j] = true;
                            yourMapTmp[i + 2, j] = true;
                            yourMapTmp[i + 3, j] = true;
                            counter++;
                        }
                        else if (downShip == 0 && upShip == 0)
                        {
                            rightShip = IsRightNeighbour(i, j);
                            leftShip = IsLeftNeighbour(i, j);
                            if (rightShip == 3 && leftShip == 0)
                            {
                                yourMapTmp[i, j] = true;
                                yourMapTmp[i, j + 1] = true;
                                yourMapTmp[i, j + 2] = true;
                                yourMapTmp[i, j + 3] = true;
                                counter++;
                            }
                        }
                        if (counter > 1) return false;
                    }
                }
            }
            if (counter < 1) return false;
            return true;
        }
        void playbuttonClick(object sender, EventArgs e)
        {
            clickedButton = (Button)sender;
            //check sphips
            bool checkResult = false;
            Array.Clear(yourMapTmp, 0, yourMapTmp.Length);
            checkResult=Check1CellShip();
            if (checkResult == false)
            {
                MessageBox.Show("You have set wrong number 1 cell ship", "Error");
                return;
            }
            checkResult = Check2CellShip();
            if (checkResult == false)
            {
                MessageBox.Show("You have set wrong number 2 cell ship", "Error");
                return;
            }
            checkResult = Check3CellShip();
            if (checkResult == false)
            {
                MessageBox.Show("You have set wrong number 3 cell ship", "Error");
                return;
            }
            checkResult = Check4CellShip();
            if (checkResult == false)
            {
                MessageBox.Show("You have set wrong number 4 cell ship", "Error");
                return;
            }

            char comm = (char)0;
            string message = comm + " " + Program.userLogin + " " + Program.enemyNick + " <EOF>";
            Program.client.Send(message);
            BPlay.Text = "Wait opponent ....";
            enemyGiveUpBeforeStart = true;
            clickedButton.Enabled = false;
        }
        //======================================== Game logic
        public void GetShotAndResponse(int x, int y)
        {
            Button button;
            Panel panel = (Panel)this.Controls.Find("P1", true).FirstOrDefault();
            button = (Button)panel.Controls.Find(x.ToString() + y.ToString(), true).FirstOrDefault();
            string message = "";           
            //Ship is hit
            if (yourMap[x,y] == true)
            {
                yourMapTmp[x, y] = true;

                sphips--;
                button.BackColor = Color.Red;
                Application.DoEvents();
                if (sphips == 0)
                {
                    Application.DoEvents();
                    return;
                }
                else
                {
                    //Send Hit
                    message = (char)5 + " " + enemyNick + " <EOF>";

                    Program.client.Send(message);
                    //Your turn
                    ((Panel)this.Controls.Find("P2", true).FirstOrDefault()).Enabled = false;
                }  
            }
            else//Send Miss
            {
                message = (char)4 + " " + enemyNick + " <EOF>";
                Program.client.Send(message);
                button.BackColor = Color.Silver;
                //Your turn
                ((Panel)this.Controls.Find("P2", true).FirstOrDefault()).Enabled = true;
            }
            Application.DoEvents();
        }

        void buttonClick(object sender, EventArgs e)
        {
            clickedButton = (Button)sender;
            clickedButton.Enabled = false;
            int x = Int32.Parse(clickedButton.Name.Substring(0, 1)); 
            int y = Int32.Parse(clickedButton.Name.Substring(1, 1)); 
            //Send Shot
            string message = "";
            message = (char)6 + " " + enemyNick + " " + x.ToString() +" " + y.ToString() + " <EOF>";
            Program.client.Send(message);    
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (enemyGiveUpBeforeStart == false)
            {
                char comm = (char)2;
                string message = comm + " " + Program.userLogin + " " + Program.enemyNick + " <EOF>";
                Program.client.Send(message);

            }
            else if (normalEnd == false)
            {
                char comm = (char)15;
                string message = comm + " " + Program.userLogin + " " + Program.enemyNick + " <EOF>";
                Program.client.Send(message);
            }
            DialogResult = DialogResult.Yes;
        }

    }
}
