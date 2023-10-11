using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using System.IO;
using static PokerLib.CFRAgent;
using System.Security.Policy;

namespace PokerLib
{
    public abstract class AAgent
    {
        // info
        public readonly string ID;

        public AAgent(string id)
        {
            ID = id;
        }



        // play
        public abstract bool Play(Situation situ, out ChoiceInfo outChoice);
    }


    public class HumanAgent : AAgent
    {

        public HumanAgent() : base("human") { }

        public override bool Play(Situation situ, out ChoiceInfo outChoice)
        {
            outChoice = new ChoiceInfo();

            int minBet, maxBet;
            var possibleChoices = situ.GetPossibleChoices();


            Choice select;
            if (possibleChoices.Count > 0)
            {
                while (true)
                {
                    string input = Console.ReadLine();

                    if (Enum.TryParse<Choice>(input.ToLower(), out select))
                    {
                        bool exist = false;
                        foreach (ChoiceInfo choice in possibleChoices)
                        {
                            if (select == choice.choice)
                            {
                                exist = true;
                                break;
                            }
                        }
                        if (!exist)
                        {
                            Console.WriteLine($"invalid choice !!!");
                            continue;
                        }

                        if (select == Choice.raise)
                        {
                            while (true)
                            {
                                //Console.Write($"Type raise amount : ");
                                //int raiseAmount = Convert.ToInt32(Console.ReadLine());
                                //if (minBet <= raiseAmount && raiseAmount <= maxBet)
                                //{
                                //    outChoice = new ChoiceInfo(select, ID, raiseAmount);
                                //    return true;
                                //}
                                //else
                                //{
                                //    Console.WriteLine($"Wrong bet size !!!");
                                //}
                            }
                        }
                        else
                        {
                            outChoice = new ChoiceInfo(select, ID);
                            return true;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Wrong choice !!!");
                    }
                }
            }

            return false;
        }
    }
    public class RandomAgent : AAgent
    {

        public RandomAgent() : base("random") { }

        public override bool Play(Situation situ, out ChoiceInfo outChoice)
        {
            Thread.Sleep(2000);

            outChoice = new ChoiceInfo();

            ////DEBUG
            //outChoice = new ChoiceInfo(Choice.fold, m_player);
            //return true;


            int minBet, maxBet;
            var possibleChoices = situ.GetPossibleChoices();

            Random rand = new Random();

            if (possibleChoices.Count > 0)
            {
                int r = rand.Next(possibleChoices.Count);
                Choice select = possibleChoices[r].choice;
                switch (select)
                {
                    case Choice.raise:
                        outChoice = possibleChoices[r];
                        return true;
                    case Choice.fold:
                        bool isCheckExist = false;
                        foreach (var choice in possibleChoices)
                        {
                            if (choice.choice == Choice.check)
                            {
                                isCheckExist = true;
                                break;
                            }
                        }
                        if (isCheckExist)
                        {
                            outChoice = new ChoiceInfo(Choice.check, ID);
                            return true;
                        }
                        break;
                }

                outChoice = new ChoiceInfo(select, ID);
                return true;
            }

            return false;
        }
    }




    public class ProbDistAgent : AAgent
    {
        //win division
        //0% 10%, 20%, 30% ... 90%, 100%(total 1)


        // check proportion
        private double[] m_RaiseProps;
        private double[] m_ReRaiseProps;
        private double[] m_CallProps;

        // pot size consideration
        private double m_potEfficient;

        public ProbDistAgent() : base("St_AI")
        {
            //customizing
            m_RaiseProps = new double[11] { 0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.5, 0.6, 0.7, 0.8, 0.9 };
            m_CallProps = new double[11] { 0, 0.1, 0.25, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 0.99, 0.99};
            m_potEfficient = 0.3;


        }


        public override bool Play(Situation situ, out ChoiceInfo outChoice)
        {
            double winRate, tieRate;
            Poker.GetWinRate(situ.Comm, situ.Hand(ID), out winRate, out tieRate);

            //win rate 2 index
            int winRateIndex = (int)(winRate * 10);
            double winRateRange = winRate * 10 - winRateIndex;

            double curRaiseProp = m_RaiseProps[winRateIndex] + winRateRange * (m_RaiseProps[winRateIndex + 1] - m_RaiseProps[winRateIndex]);
            double curCallProp = m_CallProps[winRateIndex] + winRateRange * (m_CallProps[winRateIndex + 1] - m_CallProps[winRateIndex]);

            //get raise possibility
            var possibleChoices = situ.GetPossibleChoices();
            bool isCheckExist = false;
            bool isRaiseExist = false;
            bool isCallExist = false;
            foreach (var choice in possibleChoices)
            {
                if (choice.choice == Choice.check)
                {
                    isCheckExist = true;
                }
                if (choice.choice == Choice.call)
                {
                    isCallExist = true;
                }
                if (choice.choice == Choice.raise)
                {
                    isRaiseExist = true;
                }

            }
            Random rand = new Random();






            if (isRaiseExist)
            {
                //raise
                if (rand.NextDouble() < curRaiseProp)
                {
                    foreach (var choice in possibleChoices)
                    {
                        if (choice.choice == Choice.raise)
                        {
                            outChoice = choice;
                            outChoice.turn = ID;
                            return true;
                        }

                    }
                }
            }


            if (isCallExist)// call
            {


                if (rand.NextDouble() < curCallProp)
                {
                    outChoice = new ChoiceInfo(Choice.call, ID);
                }
                else
                {
                    outChoice = new ChoiceInfo(Choice.fold, ID);
                }
                return true;
            }


            // check
            outChoice = new ChoiceInfo(Choice.check, ID);
            return true;
        }
    }


    public class MCCFR
    {
        const int NUM_ACTIONS = 4;



        public class Node
        {
            public string infoSet;
            public double[] regretSum;
            public double[] strategySum;
            public bool[] possibleChoice;
            public int possibleChoiceNum;

            public Node()
            {
                infoSet = "";
                regretSum = new double[NUM_ACTIONS];
                strategySum = new double[NUM_ACTIONS];
                possibleChoice = new bool[NUM_ACTIONS];
                possibleChoiceNum = 0;
                for (int i = 0; i < NUM_ACTIONS; i++)
                {
                    regretSum[i] = 0;
                    strategySum[i] = 0;
                    possibleChoice[i] = false;
                }
            }
            public Node(string h, List<ChoiceInfo> possibleChoices)
            {
                infoSet = h;
                regretSum = new double[NUM_ACTIONS];
                strategySum = new double[NUM_ACTIONS];
                possibleChoice = new bool[NUM_ACTIONS];
                possibleChoiceNum = 0;

                for (int i = 0; i < NUM_ACTIONS; i++)
                {
                    regretSum[i] = 0;
                    strategySum[i] = 0;
                    possibleChoice[i] = false;
                }
                foreach (var c in possibleChoices)
                {
                    possibleChoice[(int)c.choice] = true;
                    possibleChoiceNum++;
                }
            }


            public List<double> GetStrategy()
            {
                double normalizingSum = 0;
                List<double> strategy = new List<double>();
                for (int a = 0; a < NUM_ACTIONS; a++)
                {
                    double curVal = Math.Max(0, regretSum[a]);

                    normalizingSum += curVal;

                    strategy.Add(curVal);
                }

                if (normalizingSum > 0)
                    for (int a = 0; a < NUM_ACTIONS; a++)
                    {
                        strategy[a] /= normalizingSum;
                    }
                else
                {
                    for (int a = 0; a < NUM_ACTIONS; a++)
                    {
                        if (possibleChoice[a])
                            strategy[a] = 1.0 / possibleChoiceNum;
                        else
                            strategy[a] = 0;

                    }
                }

                return strategy;
            }
            public List<double> GetAvgStrategy()
            {
                double normalizingSum = 0;
                List<double> strategy = new List<double>();



                for (int a = 0; a < NUM_ACTIONS; a++)
                {
                    double curVal = Math.Max(0, strategySum[a]);

                    normalizingSum += curVal;

                    strategy.Add(curVal);
                }

                if (normalizingSum > 0)
                {
                    for (int a = 0; a < NUM_ACTIONS; a++)
                    {
                        strategy[a] /= normalizingSum;
                    }
                }
                else
                {
                    for (int a = 0; a < NUM_ACTIONS; a++)
                    {
                        if (possibleChoice[a])
                            strategy[a] = 1.0 / possibleChoiceNum;
                        else
                            strategy[a] = 0;
                    }
                }

                return strategy;
            }

            public void UpdateRegretSum(double[] sums)
            {
                if (sums.Length != NUM_ACTIONS)
                    throw new ArgumentException("invalid count of sums");

                for (int a = 0; a < NUM_ACTIONS; ++a)
                {
                    if (!possibleChoice[a])
                        continue;

                    regretSum[a] += sums[a];
                }
            }
            public void UpdateStrategySum(List<double> sums, double weight)
            {
                if (sums.Count != NUM_ACTIONS)
                    throw new ArgumentException("invalid count of sums");

                for (int a = 0; a < NUM_ACTIONS; ++a)
                {
                    if (!possibleChoice[a])
                        continue;

                    strategySum[a] += sums[a] * weight;
                }
            }
        }

        public Dictionary<string, Node> I;

        Random rand;

        public MCCFR()
        {
            I = new Dictionary<string, Node>();
            rand = new Random();
        }


        public double Process(Situation situ, double p1 = 1.0, double p2 = 1.0)
        {
            // termianl
            if (situ.IsTerminal())
            {
                if (situ.StageHistory.Last() == 'F')
                {
                    return ((situ.Pot - situ.Bet) / 2.0) / Poker.BLIND_BIG;
                }

                if (situ.Winner == situ.m_p1)
                {
                    if (situ.Turn == situ.m_p1)
                        return ((situ.Pot - situ.Bet) / 2.0) / Poker.BLIND_BIG;
                    else
                        return (-(situ.Pot - situ.Bet) / 2.0) / Poker.BLIND_BIG;
                }
                else if (situ.Winner == situ.m_p2)
                {
                    if (situ.Turn == situ.m_p1)
                        return (-(situ.Pot - situ.Bet) / 2.0) / Poker.BLIND_BIG;
                    else
                        return ((situ.Pot - situ.Bet) / 2.0) / Poker.BLIND_BIG;
                }
                else
                {
                    return 0;
                }

            }

            #region Get Possible Choice hash
            var choices = situ.GetPossibleChoices();
            Dictionary<Choice, ChoiceInfo> choiceHash = new Dictionary<Choice, ChoiceInfo>();
            foreach (var c in choices)
            {
                choiceHash.Add(c.choice, c);
            }
            #endregion

            #region Information set
            string infoSet = GetIKey(situ);
            if (!I.ContainsKey(infoSet))
            {
                I.Add(infoSet, new Node(infoSet, choices));
            }
            Node curI = I[infoSet];
            #endregion

            #region Get node Strategy
            var strategy = curI.GetStrategy();
            curI.UpdateStrategySum(strategy, situ.Turn == situ.m_p1 ? p1 : p2);
            #endregion



            #region get utils
            double E = 0;
            double[] utils = new double[NUM_ACTIONS];
            for (int a = 0; a < NUM_ACTIONS; a++)
            {
                utils[a] = 0;

                if (!choiceHash.ContainsKey((Choice)a))
                {
                    continue;
                }

                double p1Ef = p1;
                double p2Ef = p2;
                if (situ.Turn == situ.m_p1)
                {
                    p1Ef *= strategy[a];
                }
                else
                {
                    p2Ef *= strategy[a];
                }

                utils[a] = -Process(
                    situ.Next(choiceHash[(Choice)a]),
                    p1Ef, p2Ef);

                E += strategy[a] * utils[a];

            }
            #endregion


            // OPP% & sum up

            double[] regrets = new double[NUM_ACTIONS];
            for (int a = 0; a < NUM_ACTIONS; a++)
            {
                regrets[a] = 0;

                regrets[a] = (utils[a] - E) * (situ.Turn == situ.m_p1 ? p2 : p1);
            }

            curI.UpdateRegretSum(regrets);

            return E;
        }

        public static string GetIKey(Situation situ)
        {
            string key = "";

            var curStage = situ.GetCurStage();

            switch (curStage)
            {
                case Stage.Preflop:
                    {
                        var c1 = situ.Hand(situ.Turn)[0];
                        var c2 = situ.Hand(situ.Turn)[1];

                        var comp = new Card.SortCardComparer();
                        if (comp.Compare(c1, c2) == 1)
                        {
                            key += $"{c1.ToString()[1]}{c2.ToString()[1]}";
                        }
                        else
                        {
                            key += $"{c2.ToString()[1]}{c1.ToString()[1]}";
                        }

                        if (c1.shape == c2.shape)
                        {
                            key += "s";
                        }
                        else
                        {
                            key += "o";
                        }
                    }
                    break;
                case Stage.Flop:
                    {
                        // hand cards
                        List<int> kList = new List<int>();
                        var rank = Poker.CalcRank(situ.Comm, situ.Hand(situ.Turn), ref kList);
                        key += $"{rank}{kList[0]}f";
                        if ((int)rank < (int)Rank.Straight)
                        {
                            if (Poker.IsPreFlush(situ.Comm, situ.Hand(situ.Turn)))
                            {
                                key += $"_F";
                            }
                            if (Poker.IsPreStraight(situ.Comm, situ.Hand(situ.Turn)))
                            {
                                key += $"_S";
                            }
                        }

                    }
                    break;
                case Stage.Turn:
                    {
                        // hand cards
                        List<int> kList = new List<int>();
                        var rank = Poker.CalcRank(situ.Comm, situ.Hand(situ.Turn), ref kList);
                        key += $"{rank}{kList[0]}t";
                        if ((int)rank < (int)Rank.Straight)
                        {
                            if (Poker.IsPreFlush(situ.Comm, situ.Hand(situ.Turn)))
                            {
                                key += $"_F";
                            }
                            if (Poker.IsPreStraight(situ.Comm, situ.Hand(situ.Turn)))
                            {
                                key += $"_S";
                            }
                        }

                    }
                    break;
                case Stage.River:
                    {
                        List<int> kList = new List<int>();
                        var rank = Poker.CalcRank(situ.Comm, situ.Hand(situ.Turn), ref kList);
                        key += $"{rank}{kList[0]}";
                    }
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }


            key += $"_{situ.StageHistory}";

            return key;
        }

        public override string ToString()
        {
            string ret = "";

            var sortedI = I.Keys.OrderBy(k => k);



            foreach (var key in sortedI)
            {
                var avgStrategy = I[key].GetAvgStrategy();

                ret += $"{I[key].infoSet,15} : [";
                foreach (var strategy in avgStrategy)
                {
                    ret += $"{strategy:F2},";
                }

                ret.Remove(ret.Length - 1);
                ret += "] [";

                foreach (var regret in I[key].regretSum)
                {
                    ret += $"{regret:F2},";
                }

                ret.Remove(ret.Length - 1);
                ret += "] [";
                foreach (var str in I[key].strategySum)
                {
                    ret += $"{str:F2},";
                }

                ret.Remove(ret.Length - 1);
                ret += "]\n";

            }

            return ret;
        }
    }


    public class CFRAgent : AAgent
    {

        //DEBUG
        public static Dictionary<string, double> values = new Dictionary<string, double>();

        private MCCFR m_cfr;

        public CFRAgent(MCCFR cfr) : base("CFR")
        {
            m_cfr = cfr;
        }




        public override bool Play(Situation situ, out ChoiceInfo outChoice)
        {
            Thread.Sleep(1000);

            outChoice = new ChoiceInfo(Choice.fold, ID);

            List<double> strategy = new List<double>();
            Random r = new Random();
            string key = MCCFR.GetIKey(situ);
            var possChoices = situ.GetPossibleChoices();
            if (!m_cfr.I.ContainsKey(key))
            {
                outChoice = possChoices[r.Next(possChoices.Count())];
                return true;
            }



            
            var curI = m_cfr.I[key];

            strategy = curI.GetAvgStrategy();
            for (int a = strategy.Count - 2; a >= 0; a--)
            {
                for (int i = a + 1; i < strategy.Count; i++)
                {
                    strategy[i] += strategy[a];
                }
            }
            for (int a = 0; a < strategy.Count; a++)
            {
                if (!curI.possibleChoice[a])
                {
                    strategy[a] = -1;
                }
            }

            var sel = r.NextDouble();

            Choice? selChoice = null;
            for (int a = 0; a < strategy.Count; a++)
            {
                if (sel < strategy[a])
                {
                    selChoice = (Choice)a;
                    break;
                }
            }

            Debug.Assert(selChoice.HasValue);

            bool exist = false;
            foreach(var choice in possChoices)
            {
                if(selChoice.Value == choice.choice)
                {
                    exist = true;
                    outChoice = choice;
                    break;
                }
            }
            Debug.Assert(exist);

            outChoice.turn = ID;
            


            return true;
        }


    }
}
