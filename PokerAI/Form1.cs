
using PokerLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using static PokerLib.CFRAgent;

namespace PokerAI
{
    public partial class Form1 : Form
    {
        private Thread m_cfrThread;

        public Form1()
        {
            InitializeComponent();

        }

        private void BTN_MAIN_PLAY_Click(object sender, EventArgs e)
        {
            int difficulty = 0;
            if(RD_DIFFICULTY_HARD.Checked)
                difficulty = 1;

            WIN_PLAY form = new WIN_PLAY(difficulty);
            form.ShowDialog();
        }

        private void BTN_GTO_CHART_Click(object sender, EventArgs e)
        {
            Multithread_CFR.Lock();

            GTO form = new GTO();
            form.ShowDialog();

            Multithread_CFR.Unlock();


        }








        private void RD_DIFFICULTY_NORMAL_CheckedChanged(object sender, EventArgs e)
        {
            if (RD_DIFFICULTY_NORMAL.Checked)
            {

            }
        }

        private void RD_DIFFICULTY_HARD_CheckedChanged(object sender, EventArgs e)
        {
            if (RD_DIFFICULTY_HARD.Checked)
            {

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Multithread_CFR.init(new Situation("p1", "p2", "p1"));

            m_cfrThread = new Thread(Multithread_CFR.Train);
            m_cfrThread.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Multithread_CFR.End();

            m_cfrThread.Join();
        }
    }
}
