
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokerAI
{
    using PokerLib;
    using System.Diagnostics;
    using System.Threading;

    enum GameDisplayStage
    {
        Start,
        Idle,
        AITurn,
        Showdown,
        End,
        Bankrupt
    }

    public partial class WIN_PLAY : Form
    {
        private static int gameCount = 0;

        const int IMG_WIDTH_CARD = 180;
        const int IMG_HEIGHT_CARD = 256;


        readonly Bitmap m_cardFrontImage;
        readonly Bitmap m_cardBackImage;

        Situation m_situation;
        AAgent m_player;
        AAgent m_ai;

        GameDisplayStage m_curDispStage;
        Choice m_lastChoice;

        public WIN_PLAY(int difficulty)
        {
            InitializeComponent();

            Multithread_CFR.Lock();

            m_player = new HumanAgent();
            if(difficulty == 0)
                m_ai = new ProbDistAgent();
            else
                m_ai = new CFRAgent(Multithread_CFR.Get);


            Multithread_CFR.Unlock();

            m_situation = new Situation(m_player.ID, m_ai.ID, m_player.ID);

            string imagePath = Application.ExecutablePath;
            string image1FullName = imagePath+"\\..\\..\\..\\images\\Card_Total.png";
            string image2FullName = imagePath+"\\..\\..\\..\\images\\Card_Back.png";
            m_cardFrontImage = new Bitmap(image1FullName);

            m_cardBackImage = new Bitmap(image2FullName);

            m_curDispStage = GameDisplayStage.Start;

            PIC_PLAYER_HAND1.BackColor = Color.DarkGray;
            PIC_PLAYER_HAND2.BackColor = Color.DarkGray;
            PIC_AI_HAND1.BackColor = Color.DarkGray;
            PIC_AI_HAND2.BackColor = Color.DarkGray;
            PIC_COMM1.BackColor = Color.DarkGray;
            PIC_COMM2.BackColor = Color.DarkGray;
            PIC_COMM3.BackColor = Color.DarkGray;
            PIC_COMM4.BackColor = Color.DarkGray;
            PIC_COMM5.BackColor = Color.DarkGray;

            m_lastChoice = Choice.fold;

            GB_TURN_AI.Visible = false;
            GB_TURN_PLAYER.Visible = false;
        }

        private void WriteHistory(string s)
        {
            RTB_OUTPUT.Text += $"{s}\n"; 
            RTB_OUTPUT.SelectionStart = RTB_OUTPUT.TextLength;
            RTB_OUTPUT.ScrollToCaret();
        }

        private void InitTable()
        {
            WriteHistory($"Game({++gameCount}th) Start");

            m_situation.SetPreflop();

            m_curDispStage = GameDisplayStage.Idle;



        }

        private Image GetCardImg(Card c, bool open=true)
        {
            if (open)
            {
                int x = c.num == 1 ? 12 : c.num - 2;
                int y = (int)c.shape;

                Rectangle cropRect = new Rectangle(x * IMG_WIDTH_CARD, y * IMG_HEIGHT_CARD, IMG_WIDTH_CARD, IMG_HEIGHT_CARD);
                return m_cardFrontImage.Clone(cropRect, m_cardFrontImage.PixelFormat);
            }
            else
            {
                return m_cardBackImage;
                
            }
        }

        private void UpdateDisplayTurn()
        {
            switch(m_curDispStage)
            {
                case GameDisplayStage.Idle:

                    GB_TURN_PLAYER.Visible = true;
                    GB_TURN_AI.Visible = false;

                    break;
                case GameDisplayStage.AITurn:

                    GB_TURN_PLAYER.Visible = false;
                    GB_TURN_AI.Visible = true;
                    break;
                default:

                    GB_TURN_PLAYER.Visible = false;
                    GB_TURN_AI.Visible = false;

                    break;
            }
        }

        private void UpdateChoices(bool forceDisable=false)
        {
            if (forceDisable)
            {
                BTN_CHECK.Enabled = false;
                BTN_RAISE.Enabled = false;
                BTN_FOLD.Enabled = false;


            }
            else
            {

                var choices = m_situation.GetPossibleChoices();

                bool isCheck = false;
                bool isCall = false;
                bool isRaise = false;
                int betSize = -1;
                bool isFold = false;
                foreach (var c in choices)
                {
                    switch (c.choice)
                    {
                        case Choice.check:
                            isCheck = true;
                            break;
                        case Choice.call:
                            isCall = true;
                            break;
                        case Choice.raise:
                            isRaise = true;
                            betSize = c.bet;
                            break;
                        case Choice.fold:
                            isFold = true;
                            break;
                        default:
                            Debug.Assert(false);
                            return;
                    }
                }


                BTN_CHECK.Enabled = true;
                if ((isCheck && BTN_CHECK.Text== "CALL") || (!isCheck && BTN_CHECK.Text == "CHECK"))
                {
                    BTN_CHECK.Text = isCheck?"CHECK":"CALL";
                }
                if ((isRaise && !BTN_RAISE.Enabled) || (!isRaise && BTN_RAISE.Enabled))
                {


                    BTN_RAISE.Text = $"{Convert.ToString(betSize):C}";

                    BTN_RAISE.Enabled = !BTN_RAISE.Enabled;
                }
                if ((isFold && !BTN_FOLD.Enabled) || (!isFold && BTN_FOLD.Enabled))
                {
                    BTN_FOLD.Enabled = !BTN_FOLD.Enabled;
                }
            }

        }

        private bool Play()
        {
            
            //if (m_situation.GetCurStage() == Stage.Start)
            //{
            //    m_situation.SetPreflop();
            //}
            //else
            //{


            //    if (m_situation.Turn == "human")
            //    {
            //        Console.WriteLine(m_situation.ToString());
            //    }

            //    Console.WriteLine($"\nTurn:{m_situation.Turn}");
            //    Thread.Sleep(1000);


            //    ChoiceInfo choice;
            //    if (!m_agents[m_situation.Turn].Play(m_situation, out choice))
            //    {
            //        Debug.Assert(false);
            //        return false;
            //    }

            //    if (choice.choice == Choice.raise)
            //        Console.WriteLine($"Choice:{Enum.GetName(typeof(Choice), choice.choice)}({choice.bet})");
            //    else
            //        Console.WriteLine($"Choice:{Enum.GetName(typeof(Choice), choice.choice)}");

            //    m_situation = m_situation.Next(choice);
            //}

            //if (m_situation.IsTerminal())
            //{
            //    Console.Write($"{m_player.ID}:");
            //    foreach (Card c in m_situation.Hand(m_player.ID))
            //        Console.Write($"{c.ToString()} ");
            //    Console.Write($" {m_ai.ID}:");
            //    foreach (Card c in m_situation.Hand(m_ai.ID))
            //        Console.Write($"{c.ToString()} ");
            //    Console.Write($"\n{m_situation.Winner} Win $$$$$$$$$$$$$$$$$$$$$-");
            //    m_situation.ClearPot();
            //    Console.WriteLine($"{m_player.ID}:{m_situation.Stack(m_player.ID)} {m_ai.ID}:{m_situation.Stack(m_ai.ID)} -$$$$$$$$$$$$$$$$$$$$$$\n");

            //    return m_situation.IsBankrupt();
            //}

            return false;
        }

        private void ChoiceExecuted(ChoiceInfo choiceInfo)
        {
            m_situation = m_situation.Next(choiceInfo);

            m_lastChoice = choiceInfo.choice;
        }

        private void BTN_CHECK_Click(object sender, EventArgs e)
        {
            ChoiceExecuted(new ChoiceInfo(BTN_CHECK.Text=="CHECK"? Choice.check:Choice.call, m_player.ID));
            // play
            UpdateChoices(true);
            WriteHistory("CHECK <-player");
        }

        private void BTN_RAISE_Click(object sender, EventArgs e)
        {
            var choices = m_situation.GetPossibleChoices();

            bool isReRaise = false;
            int betSize = -1;
            foreach (var c in choices)
            {
                if (c.choice == Choice.call)
                {
                    isReRaise = true;
                }
                else if(c.choice==Choice.raise)
                {
                    betSize = c.bet;
                    ChoiceExecuted(c);
                    UpdateChoices(true);
                }
            }


            if (isReRaise)
            {
                WriteHistory($"RE-RAISE({betSize}) <-player");
            }
            else
            {
                WriteHistory($"RAISE({betSize}) <-player");
            }
        }

        private void BTN_CALL_Click(object sender, EventArgs e)
        {
            ChoiceExecuted(new ChoiceInfo(Choice.call, m_player.ID));
            UpdateChoices(true);

            WriteHistory($"CALL <-player");
        }

        private void BTN_FOLD_Click(object sender, EventArgs e)
        {
            ChoiceExecuted(new ChoiceInfo(Choice.fold, m_player.ID));
            UpdateChoices(true);

            WriteHistory($"FOLD <-player");
        }

        private void UpdateDisplayImage()
        {
            if(m_curDispStage == GameDisplayStage.Idle || 
                m_curDispStage == GameDisplayStage.AITurn)
            {

                var comm = m_situation.Comm;
                var curStage = m_situation.GetCurStage();

                switch (curStage)
                {
                    case Stage.Start:
                        break;
                    case Stage.Preflop:
                        if (PIC_PLAYER_HAND1.Image == null)
                        {
                            WriteHistory("1.PREFLOP--------------------------");

                            PIC_PLAYER_HAND1.Image = GetCardImg(m_situation.Hand(m_player.ID)[0]);
                            PIC_PLAYER_HAND2.Image = GetCardImg(m_situation.Hand(m_player.ID)[1]);
                            PIC_AI_HAND1.Image = GetCardImg(m_situation.Hand(m_ai.ID)[0], false);
                            PIC_AI_HAND2.Image = GetCardImg(m_situation.Hand(m_ai.ID)[1], false);
                        }
                        break;
                    case Stage.Flop:
                        if (PIC_COMM1.Image == null)
                        {
                            WriteHistory("2.FLOP-----------------------------");
                            PIC_COMM1.Image = GetCardImg(comm[0]);
                            PIC_COMM2.Image = GetCardImg(comm[1]);
                            PIC_COMM3.Image = GetCardImg(comm[2]);
                        }
                        break;
                    case Stage.Turn:
                        if (PIC_COMM4.Image == null)
                        {
                            WriteHistory("3.TURN-----------------------------");
                            PIC_COMM4.Image = GetCardImg(comm[3]);
                        }
                        break;
                    case Stage.River:
                        if (PIC_COMM5.Image == null)
                        {
                            WriteHistory("4.RIVER----------------------------");
                            PIC_COMM5.Image = GetCardImg(comm[4]);
                        }
                        break;
                }
            }
            else if(m_curDispStage == GameDisplayStage.Showdown)
            {
                PIC_AI_HAND1.Image = GetCardImg(m_situation.Hand(m_ai.ID)[0]);
                PIC_AI_HAND2.Image = GetCardImg(m_situation.Hand(m_ai.ID)[1]);

                var comm = m_situation.Comm;
                if (comm.Count == 5)
                {
                    PIC_COMM1.Image = GetCardImg(comm[0]);
                    PIC_COMM2.Image = GetCardImg(comm[1]);
                    PIC_COMM3.Image = GetCardImg(comm[2]);
                    PIC_COMM4.Image = GetCardImg(comm[3]);
                    PIC_COMM5.Image = GetCardImg(comm[4]);
                }
            }
            else
            {

                PIC_PLAYER_HAND1.Image = null;
                PIC_PLAYER_HAND2.Image = null;
                PIC_AI_HAND1.Image = null;
                PIC_AI_HAND2.Image = null;
                PIC_COMM1.Image = null;
                PIC_COMM2.Image = null;
                PIC_COMM3.Image = null;
                PIC_COMM4.Image = null;
                PIC_COMM5.Image = null;
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (m_curDispStage)
            {
                case GameDisplayStage.Start:
                    {
                        Thread.Sleep(200);

                        InitTable();

                        m_curDispStage = GameDisplayStage.Idle;

                        break;
                    }
                case GameDisplayStage.Idle:
                    {
                        #region Update Game Play

                        if(m_situation.IsTerminal())
                        {
                            if (m_lastChoice == Choice.fold)
                            {
                                m_curDispStage = GameDisplayStage.End;
                            }
                            else
                            {
                                WriteHistory($"showdown");
                                m_curDispStage = GameDisplayStage.Showdown;
                                UpdateDisplayImage();
                            }

                        }
                        else if (m_situation.Turn == m_ai.ID)
                        {
                            m_curDispStage = GameDisplayStage.AITurn;
                        }
                        else
                        {
                            UpdateChoices();
                        }


                        #endregion


                        #region Update LABEL
                        TB_PLAYER_STACK.Text = Convert.ToString(m_situation.Stack(m_player.ID));
                        TB_AI_STACK.Text = Convert.ToString(m_situation.Stack(m_ai.ID));
                        TB_POT.Text = Convert.ToString(m_situation.Pot);
                        TB_BET.Text = Convert.ToString(m_situation.Bet);
                        
                        #endregion

                    break;
                    }
                case GameDisplayStage.AITurn:
                    {
                        Thread.Sleep(400);

                        ChoiceInfo aiChoice;

                        Multithread_CFR.Lock();

                        m_ai.Play(m_situation, out aiChoice);

                        Multithread_CFR.Unlock();

                        switch (aiChoice.choice)
                        {
                            case Choice.check:
                                WriteHistory($"CHECK <-ai");
                                break;
                            case Choice.call:
                                WriteHistory($"CALL <-ai");
                                break;
                            case Choice.raise:
                                bool isReRaise = m_situation.Bet > 0;
                                if (isReRaise)
                                    WriteHistory($"RE-RAISE({aiChoice.bet}) <-ai");
                                else
                                    WriteHistory($"RAISE({aiChoice.bet}) <- ai");
                                break;
                            case Choice.fold:
                                WriteHistory($"FOLD <-ai");
                                break;
                        }

                        ChoiceExecuted(aiChoice);


                        m_curDispStage = GameDisplayStage.Idle;


                        break;
                    }
                case GameDisplayStage.Showdown:
                    {
                        if (m_lastChoice == Choice.fold)
                            Thread.Sleep(500);
                        else
                            Thread.Sleep(4000);


                        m_curDispStage = GameDisplayStage.End;

                        break;
                    }
                case GameDisplayStage.End:
                    {
                        Thread.Sleep(500);
                        string h = "player( ";
                        foreach (var c in m_situation.Hand(m_player.ID))
                            h+=$"{c.ToString()} ";
                        h += " ) ai( ";
                        foreach (var c in m_situation.Hand(m_ai.ID))
                            h+=$"{c.ToString()} ";
                        WriteHistory($"{h}) ");
                        h = "comm( ";
                        foreach (var c in m_situation.Comm)
                           h +=$"{c.ToString()} ";
                        WriteHistory($"{h} )");
                        WriteHistory($"winner : {m_situation.Winner} earning pot : {m_situation.Pot}\n");


                        m_situation.ClearPot();


                        string bankruptID;
                        if (m_situation.IsBankrupt(out bankruptID))
                        {
                            m_curDispStage = GameDisplayStage.Bankrupt;

                            if (bankruptID == m_player.ID)
                                MessageBox.Show($"YOU LOSE !!!, sorry~");
                            else
                                MessageBox.Show($"YOU WIN !!!");

                            UpdateChoices(true);
                        }
                        else
                            m_curDispStage = GameDisplayStage.Start;


                        break;
                    }
                case GameDisplayStage.Bankrupt:
                    break;
            }


            UpdateDisplayTurn();
            UpdateDisplayImage();

        }


    }
}
