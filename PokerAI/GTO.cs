using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PokerAI.GTO;

namespace PokerAI
{
    public partial class GTO : Form
    {
        BulletGraphData[][] bulletGraphData;

        string history;


        public GTO()
        {

            InitializeComponent();


            dataGridView1.ShowCellToolTips = true;

            history = "";

            bulletGraphData = new BulletGraphData[13][];

            for (int i = 0; i < 13; i++)
            {
                bulletGraphData[i] = new BulletGraphData[13];
                for (int j = 0; j < 13; j++)
                {
                    bulletGraphData[i][j] = null;
                }
            }


            dataGridView1.Rows.Clear();
            int rAID = dataGridView1.Rows.Add();
            dataGridView1.Rows[rAID].Height = 30;
            dataGridView1.Rows[rAID].HeaderCell.Value = "A";
            for (int i = 13; i >= 2; --i)
            {
                int rID = dataGridView1.Rows.Add();
                dataGridView1.Rows[rID].Height = 30;

                if (i == 10)
                    dataGridView1.Rows[rID].HeaderCell.Value = "T";
                else if (i == 11)
                    dataGridView1.Rows[rID].HeaderCell.Value = "J";
                else if (i == 12)
                    dataGridView1.Rows[rID].HeaderCell.Value = "Q";
                else if (i == 13)
                    dataGridView1.Rows[rID].HeaderCell.Value = "K";
                else
                    dataGridView1.Rows[rID].HeaderCell.Value = $"{i}";
            }

            UpdateGrid();
        }


        private void UpdateGrid()
        {
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    bulletGraphData[i][j] = null;
                }
            }

            string suitKey;

            if(RB_GTO_OFFSUIT.Checked)
            {
                suitKey = "o";
            }
            else
            {
                suitKey = "s";
            }




            //A
            UpdateDataTable(0, 0, $"AA{suitKey}_");
            for (int i = 13; i >= 2; --i)
            {
                string key = "A";
                if (i == 10)
                    key += $"T{suitKey}_";
                else if (i == 11)
                    key += $"J{suitKey}_";
                else if (i == 12)
                    key += $"Q{suitKey}_";
                else if (i == 13)
                    key += $"K{suitKey}_";
                else
                    key += $"{i}{suitKey}_";

                UpdateDataTable(0, 14 - i, key);
            }

            for (int i = 13; i >= 2; --i)
            {
                string key = "";
                if (i == 10)
                    key += "T";
                else if (i == 11)
                    key += "J";
                else if (i == 12)
                    key += "Q";
                else if (i == 13)
                    key += "K";
                else
                    key += $"{i}";

                for (int j = 14 - i; j < 13; ++j)
                {
                    string key2 = key;

                    int cId = 14 - j;

                    if (cId == 10)
                        key2 += "T";
                    else if (cId == 11)
                        key2 += "J";
                    else if (cId == 12)
                        key2 += "Q";
                    else if (cId == 13)
                        key2 += "K";
                    else
                        key2 += $"{cId}";


                    key2 += $"{suitKey}_";

                    UpdateDataTable(14 - i, j, key2);
                }
            }
        }

        private void UpdateDataTable(int row, int col, string key)
        {
            key += history;

            var cfr = Multithread_CFR.Get;
            if (cfr.I.ContainsKey(key))
            {
                var node = cfr.I[key];

                var strategy = node.GetAvgStrategy();

                bulletGraphData[row][col] = new BulletGraphData(strategy[0], strategy[1], strategy[2], strategy[3]);
            }

            dataGridView1.Rows[row].Cells[col].Value = key;
        }
        
        
        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void PaintBulletGraph(Graphics graphics, Rectangle bounds, double p1, double p2, Color col)
        {

            // Calculate the position and size of each element of the bullet graph
            var barWidth = bounds.Width * (p2-p1);


            // Paint the bar
            using (var barBrush = new SolidBrush(col))
            {
                graphics.FillRectangle(barBrush, (float)(bounds.Left + p1*bounds.Width), bounds.Top, (float)barWidth, bounds.Height);
            }

        }
        public class BulletGraphData
        {
            public double ValueCheck { get; set; }
            public double ValueCall { get; set; }
            public double ValueRaise { get; set; }
            public double ValueFold { get; set; }

            public BulletGraphData(double valueCheck, double valueCall, double valueRaise, double valueFold)
            {
                ValueCheck = valueCheck;
                ValueCall = valueCall;
                ValueRaise = valueRaise;
                ValueFold = valueFold;
            }
        }
        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                BulletGraphData data = bulletGraphData[e.RowIndex][e.ColumnIndex];

                if (data != null)
                {
                    e.PaintBackground(e.CellBounds, true);

                    // Calculate the bounds of the bullet graph
                    var graphBounds = new Rectangle(e.CellBounds.Left + 2, e.CellBounds.Top + 2,
                        e.CellBounds.Width - 4, e.CellBounds.Height - 4);


                    double cumulCheck = data.ValueCheck;
                    double cumulCall = data.ValueCall;
                    double cumulRaise = data.ValueRaise;
                    double cumulFold = data.ValueFold;

                    cumulFold += cumulRaise + cumulCall + cumulCheck;
                    cumulRaise += cumulCall + cumulCheck;
                    cumulCall += cumulCheck;

                    // Paint the bullet graph
                    PaintBulletGraph(e.Graphics, graphBounds, 0, cumulCheck, Color.Yellow);

                    PaintBulletGraph(e.Graphics, graphBounds, cumulCheck, cumulCall, Color.Yellow);
                    PaintBulletGraph(e.Graphics, graphBounds, cumulCall, cumulRaise, Color.Green);
                    PaintBulletGraph(e.Graphics, graphBounds, cumulRaise, cumulFold, Color.Red);

                    e.Handled = true;
                }
            }
        }

        private void RB_GTO_SUITED_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGrid();
        }

        private void RB_GTO_OFFSUIT_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGrid();
        }


        private void BTN_GTO_CHECK_Click(object sender, EventArgs e)
        {
            BTN_GTO_CLEAR_Click(sender,e);

            history += "C";
            LB_GTO_HISTORY.Items.Add("Check");

            UpdateGrid();
        }
        private void BTN_GTO_RAISE_Click(object sender, EventArgs e)
        {
            if (history.Count(c => c == 'R') >= 4)
                return;


            history += "R";
            LB_GTO_HISTORY.Items.Add("Raise");

            UpdateGrid();
        }

        private void BTN_GTO_CLEAR_Click(object sender, EventArgs e)
        {
            history = "";

            LB_GTO_HISTORY.Items.Clear();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {


            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Exclude header cells
            {
                TB_GTO_CHECK.Text = "";
                TB_GTO_CALL.Text = "";
                TB_GTO_RAISE.Text = "";
                TB_GTO_FOLD.Text = "";

                BulletGraphData data = bulletGraphData[e.RowIndex][e.ColumnIndex];
                if(data==null)
                {
                    return;
                }

                TB_GTO_CHECK.Text = Convert.ToString($"{data.ValueCheck:F3}");
                TB_GTO_CALL.Text = Convert.ToString($"{data.ValueCall:F3}");
                TB_GTO_RAISE.Text = Convert.ToString($"{data.ValueRaise:F3}");
                TB_GTO_FOLD.Text = Convert.ToString($"{data.ValueFold:F3}");
            }
        }

        //private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Exclude header cells
        //    {
        //        DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
        //        cell.ToolTipText = "sadfassfds";
        //    }
        //}
    }
}
