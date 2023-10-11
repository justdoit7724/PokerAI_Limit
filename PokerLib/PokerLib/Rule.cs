using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace PokerLib
{
    public enum Shape
    {
        Clover,
        Spade,
        Heart,
        Diamond
    }

    public enum Choice
    {
        check,
        call,
        raise,
        fold
    }

    public enum Rank
    {
        StraightFlush,
        Four,
        Fullhouse,
        Flush,
        Straight,
        Three,
        TwoPairs,
        Pair,
        High
    }

    public enum Stage
    {
        Start = -1,
        Preflop = 0,
        Flop = 3,
        Turn = 4,
        River = 5
    }


    public struct ChoiceInfo
    {
        // new variable should be added in Equal()
        public Choice choice;
        public int bet;
        public string turn;

        public ChoiceInfo(Choice c, string t, int b = 0)
        {
            if (c == Choice.raise)
            {
                Debug.Assert(b >= 0);
            }
            else
            {
                Debug.Assert(b == 0);
            }

            choice = c;
            bet = b;
            turn = t;
        }

        public static bool operator ==(ChoiceInfo a, ChoiceInfo b)
            => (a.choice == b.choice &&
                a.bet == b.bet &&
                a.turn == b.turn);
        public static bool operator !=(ChoiceInfo a, ChoiceInfo b)
            => (a.choice != b.choice ||
                a.bet != b.bet ||
                a.turn != b.turn);
    }


    public struct Card
    {
        public readonly int num;
        public readonly Shape shape;


        public Card(int n, Shape s)
        {
            num = n;
            shape = s;
        }

        public static bool operator ==(Card a, Card b)
        => (a.num == b.num &&
                a.shape == b.shape);
        public static bool operator !=(Card a, Card b)
        => (a.num != b.num ||
                a.shape != b.shape);

        public static void Sort(ref Card[] cards)
        {
            for (int i = 0; i < cards.Length; ++i)
            {
                for (int j = i + 1; j < cards.Length; ++j)
                {
                    if (cards[i].num > cards[j].num)
                    {
                        Card tmp = cards[i];
                        cards[i] = cards[j];
                        cards[j] = tmp;
                    }
                }
            }
        }


        public class SortIntComparer : IComparer<Card>
        {
            public int Compare(Card x, Card y)
            {
                Card card1 = (Card)x;
                Card card2 = (Card)y;

                if (card1.num == card2.num)
                    return 0;
                else if (card1.num > card2.num)
                    return 1;
                else
                    return -1;
            }

        }
        public class SortCardComparer : IComparer<Card>
        {
            public int Compare(Card x, Card y)
            {
                Card card1 = (Card)x;
                Card card2 = (Card)y;

                if (card1.num == card2.num)
                    return 0;
                else if (card1.num == 1)
                    return 1;
                else if (card2.num == 1)
                    return -1;
                else if (card1.num > card2.num)
                    return 1;
                else
                    return -1;
            }

        }

        public override string ToString()
        {
            string ret = "";


            switch (shape)
            {
                case Shape.Spade:
                    ret += "♤";
                    break;
                case Shape.Diamond:
                    ret += "◆";
                    break;
                case Shape.Heart:
                    ret += "♥";
                    break;
                case Shape.Clover:
                    ret += "♧";
                    break;
            }

            if (num == 10)
            {
                ret += $"T";
            }
            else if (num == 11)
            {
                ret += $"J";
            }
            else if (num == 12)
            {
                ret += $"Q";
            }
            else if (num == 13)
            {
                ret += $"K";
            }
            else if (num == 1)
            {
                ret += $"A";
            }
            else
                ret += $"{num}";


            return ret;
        }
    }

    public class Poker
    {
        public const int ITERATION_WIN_RATE = 250;
        public const int STACK_INITIAL = 500;
        public const int BLIND_BIG = 10;
        public const int BLIND_SMALL = BLIND_BIG / 2;


        private static void Debugging(List<int> values)
        {
            foreach (int i in values)
            {
                Debug.Assert(i != 14);
            }
        }



        private static int CalcValueWinner(List<int> playerValues, List<int> oppValues, int checkNum)
        {
            for (int i = 0; i < checkNum; ++i)
            {
                if (playerValues[i] == oppValues[i])
                    continue;
                else if (playerValues[i] == 1)
                    return 1;
                else if (oppValues[i] == 1)
                    return -1;
                else if (playerValues[i] > oppValues[i])
                    return 1;
                else
                    return -1;
            }

            return 0;
        }

        public static Rank CalcRank(List<Card> comm, List<Card> player, ref List<int> kickers)
        {
            kickers.Clear();

            List<Card> playerSet = new List<Card>(comm);
            playerSet = playerSet.Concat(player).ToList();


            List<int> pKickers = new List<int>();
            if(IsStraightFlush(playerSet, ref kickers))
            {
                return Rank.StraightFlush;
            }


            if(IsFourOfKind(playerSet, ref kickers))
            {
                return Rank.Four;
            }

            if(IsFullhouse(playerSet, ref kickers))
            {
                return Rank.Fullhouse;
            }

            if(IsFlush(playerSet, ref kickers))
            {
                return Rank.Flush;
            }
            if(IsStraight(playerSet, ref kickers))
            {

                return Rank.Straight;
            }

            if(IsThreeOfKind(playerSet, ref kickers))
            {
                return Rank.Three;
            }


            if(IsTwoPair(playerSet, ref kickers))
            {
                return Rank.TwoPairs;
            }

            if(IsPair(playerSet, ref kickers))
            {
                return Rank.Pair;
            }

            return Rank.High;
        }

        public static bool IsPreStraight(List<Card> comm, List<Card> player)
        {
            #region 1 rearrange by required order
            List<Card> set = new List<Card>(comm);
            set = set.Concat(player).ToList();
            set.Sort(new Card.SortIntComparer());
            int size = set.Count;
            for (int i = 0; i < size; ++i)
            {
                if (set[i].num == 1)
                {
                    Card newOne = new Card(14, set[i].shape);

                    set.Add(newOne);
                }
            }
            set.Reverse();
            #endregion

            #region 2 Find Staight cards
            List<Card>[] straight = new List<Card>[5];
            for (int i = 0; i < straight.Length; ++i)
                straight[i] = new List<Card>();
            int curConn = 0;

            for (int i = 0; i < set.Count - 1; ++i)
            {
                straight[curConn].Add(set[i]);

                if ((set[i].num - set[i + 1].num) == 1)
                {
                    curConn++;
                }
                else if (set[i + 1].num == set[i].num)
                {

                }
                else
                {
                    curConn = 0;
                    for (int j = 0; j < straight.Length; ++j)
                        straight[j].Clear();
                }

            }

            if (curConn >= 3)
            {
                return true;
            }


            #endregion



            return false;
        }
        public static bool IsPreFlush(List<Card> comm, List<Card> player)
        {
            List<Card> set = new List<Card>(comm);
            set = set.Concat(player).ToList();

            // 1 divide & get
            Dictionary<Shape, List<Card>> shapeCards = new Dictionary<Shape, List<Card>>();
            foreach (Card c in set)
            {
                if (!shapeCards.ContainsKey(c.shape))
                {
                    shapeCards.Add(c.shape, new List<Card>());
                }
                shapeCards[c.shape].Add(c);

                if (shapeCards[c.shape].Count == 4)
                {
                    return true;
                }
            }



            return false;
        }

        public static int CalcWinner(List<Card> comm, List<Card> player, List<Card> opp)
        {
            if (comm.Count != 5 || player.Count != 2 || opp.Count != 2)
            {
                throw new ArgumentException("wrong size of cards");
            }
            Debug.Assert(comm.Count == 5);
            Debug.Assert(player.Count == 2);
            Debug.Assert(opp.Count == 2);

            List<Card> playerSet = new List<Card>(comm);
            playerSet = playerSet.Concat(player).ToList();
            List<Card> oppSet = new List<Card>(comm);
            oppSet = oppSet.Concat(opp).ToList();


            #region Winner determination
            List<int> pKickers = new List<int>();
            List<int> oKickers = new List<int>();
            bool playerRankDone = false;
            bool oppRankDone = false;
            playerRankDone = IsStraightFlush(playerSet, ref pKickers);
            oppRankDone = IsStraightFlush(oppSet, ref oKickers);

            Debugging(pKickers);
            Debugging(oKickers);

            if (playerRankDone && oppRankDone)
            {
                return CalcValueWinner(pKickers, oKickers, 1);
            }
            else if (playerRankDone)
            {
                return 1;
            }
            else if (oppRankDone)
            {
                return -1;
            }

            playerRankDone = IsFourOfKind(playerSet, ref pKickers);
            oppRankDone = IsFourOfKind(oppSet, ref oKickers);
            Debugging(pKickers);
            Debugging(oKickers);
            if (playerRankDone && oppRankDone)
            {
                return CalcValueWinner(pKickers, oKickers, 2);
            }
            else if (playerRankDone)
            {
                return 1;
            }
            else if (oppRankDone)
            {
                return -1;
            }

            playerRankDone = IsFullhouse(playerSet, ref pKickers);
            oppRankDone = IsFullhouse(oppSet, ref oKickers);
            Debugging(pKickers);
            Debugging(oKickers);
            if (playerRankDone && oppRankDone)
            {
                return CalcValueWinner(pKickers, oKickers, 2);
            }
            else if (playerRankDone)
            {
                return 1;
            }
            else if (oppRankDone)
            {
                return -1;
            }


            playerRankDone = IsFlush(playerSet, ref pKickers);
            oppRankDone = IsFlush(oppSet, ref oKickers);
            Debugging(pKickers);
            Debugging(oKickers);
            if (playerRankDone && oppRankDone)
            {
                return CalcValueWinner(pKickers, oKickers, 5);
            }
            else if (playerRankDone)
            {
                return 1;
            }
            else if (oppRankDone)
            {
                return -1;
            }


            playerRankDone = IsStraight(playerSet, ref pKickers);
            oppRankDone = IsStraight(oppSet, ref oKickers);
            Debugging(pKickers);
            Debugging(oKickers);
            if (playerRankDone && oppRankDone)
            {
                return CalcValueWinner(pKickers, oKickers, 1);
            }
            else if (playerRankDone)
            {
                return 1;
            }
            else if (oppRankDone)
            {
                return -1;
            }

            playerRankDone = IsThreeOfKind(playerSet, ref pKickers);
            oppRankDone = IsThreeOfKind(oppSet, ref oKickers);
            Debugging(pKickers);
            Debugging(oKickers);
            if (playerRankDone && oppRankDone)
            {
                return CalcValueWinner(pKickers, oKickers, 3);
            }
            else if (playerRankDone)
            {
                return 1;
            }
            else if (oppRankDone)
            {
                return -1;
            }

            playerRankDone = IsTwoPair(playerSet, ref pKickers);
            oppRankDone = IsTwoPair(oppSet, ref oKickers);
            Debugging(pKickers);
            Debugging(oKickers);
            if (playerRankDone && oppRankDone)
            {
                return CalcValueWinner(pKickers, oKickers, 3);
            }
            else if (playerRankDone)
            {
                return 1;
            }
            else if (oppRankDone)
            {
                return -1;
            }


            playerRankDone = IsPair(playerSet, ref pKickers);
            oppRankDone = IsPair(oppSet, ref oKickers);
            Debugging(pKickers);
            Debugging(oKickers);
            if (playerRankDone && oppRankDone)
            {
                return CalcValueWinner(pKickers, oKickers, 4);
            }
            else if (playerRankDone)
            {
                return 1;
            }
            else if (oppRankDone)
            {
                return -1;
            }
            #endregion

            return CalcValueWinner(pKickers, oKickers, 5);
        }

        public static void GetWinRate(List<Card> comm, List<Card> hand, out double winRate, out double tieRate)
        {
            Debug.Assert(hand.Count == 2);
            Debug.Assert(comm.Count <= 5);

            winRate = 0;
            tieRate = 0;

            #region make deck
            List<Card> deck = new List<Card>();
            for (int j = 1; j <= 13; ++j)
            {
                deck.Add(new Card(j, Shape.Spade));
                deck.Add(new Card(j, Shape.Heart));
                deck.Add(new Card(j, Shape.Diamond));
                deck.Add(new Card(j, Shape.Clover));
            }


            List<Card> set = new List<Card>(comm);
            set = set.Concat(hand).ToList();

            for (int k = deck.Count - 1; k >= 0; --k)
            {
                for (int i = 0; i < set.Count; ++i)
                {
                    if (deck[k] == set[i])
                    {
                        deck.RemoveAt(k);
                        break;
                    }
                }
            }
            #endregion



            Random r = new Random();
            for (int j = 0; j < ITERATION_WIN_RATE; ++j)
            {
                #region get comm & opponent cards

                List<Card> opp = new List<Card>();
                HashSet<int> seen = new HashSet<int>();
                List<Card> tmpComm = new List<Card>();
                foreach (Card c in comm)
                    tmpComm.Add(c);
                while (tmpComm.Count < 5)
                {
                    int rand = r.Next() % deck.Count;

                    if (seen.Contains(rand))
                        continue;
                    seen.Add(rand);

                    tmpComm.Add(deck[rand]);
                };
                while (opp.Count < 2)
                {
                    int rand = r.Next() % deck.Count;

                    if (seen.Contains(rand))
                        continue;
                    seen.Add(rand);

                    opp.Add(deck[rand]);
                };
                #endregion

                int ret = Poker.CalcWinner(tmpComm, hand, opp);
                if (ret == 0)
                    tieRate += 1;
                else if (ret == 1)
                    winRate += 1;
            }
            winRate /= (double)ITERATION_WIN_RATE;
            tieRate /= (double)ITERATION_WIN_RATE;

        }

        public static bool IsStraightFlush(List<Card> set, ref List<int> values)
        {

            values.Clear();



            // 2 divide & get
            Dictionary<Shape, List<Card>> shapeCards = new Dictionary<Shape, List<Card>>();
            Shape? mainShape = null;
            foreach (Card c in set)
            {
                if (!shapeCards.ContainsKey(c.shape))
                {
                    shapeCards.Add(c.shape, new List<Card>());
                }
                shapeCards[c.shape].Add(c);
                if (shapeCards[c.shape].Count >= 5)
                {
                    mainShape = c.shape;
                }
            }
            if (!mainShape.HasValue)
                return false;

            return IsStraight(shapeCards[mainShape.Value], ref values);

        }

        public static bool IsFourOfKind(List<Card> set, ref List<int> values)
        {
            values.Clear();

            #region 4 kind Check
            Dictionary<int, int> divMap = new Dictionary<int, int>();
            foreach (Card c in set)
            {
                if (!divMap.ContainsKey(c.num))
                    divMap[c.num] = 0;

                divMap[c.num]++;
                if (divMap[c.num] == 4)
                {
                    values.Add(c.num);
                    break;
                }
            }
            if (values.Count == 0)
                return false;
            #endregion

            #region Get Kicker
            int greatestVal = -1;
            foreach (Card c in set)
            {
                if (c.num == values[0])
                    continue;

                if (c.num == 1)
                {
                    greatestVal = 1;
                    break;
                }

                if (c.num > greatestVal)
                {
                    greatestVal = c.num;
                }

            }
            values.Add(greatestVal);
            #endregion

            return true;
        }

        public static bool IsFullhouse(List<Card> set, ref List<int> values)
        {
            values.Clear();


            #region tripple check
            List<int> kickers = new List<int>();
            List<int> tmpValues = new List<int>();
            if (!IsThreeOfKind(set, ref tmpValues))
                return false;
            values.Add(tmpValues[0]);
            #endregion

            #region pair check
            List<Card> tmpSet = new List<Card>();
            foreach (Card c in set)
            {
                if (c.num != values[0])
                    tmpSet.Add(c);
            }
            if (!IsPair(tmpSet, ref tmpValues))
                return false;
            values.Add(tmpValues[0]);
            #endregion



            return true;
        }

        public static bool IsFlush(List<Card> set, ref List<int> values)
        {
            values.Clear();

            // 1 divide & get
            Dictionary<Shape, List<Card>> shapeCards = new Dictionary<Shape, List<Card>>();
            Shape? mainShape = null;
            foreach (Card c in set)
            {
                if (!shapeCards.ContainsKey(c.shape))
                {
                    shapeCards.Add(c.shape, new List<Card>());
                }
                shapeCards[c.shape].Add(c);

                if (shapeCards[c.shape].Count >= 5)
                {
                    mainShape = c.shape;
                }
            }
            if (!mainShape.HasValue)
                return false;

            shapeCards[mainShape.Value].Sort(new Card.SortCardComparer());
            for (int i = shapeCards[mainShape.Value].Count - 1; i >= 0; --i)
            {
                values.Add(shapeCards[mainShape.Value][i].num);
                if (values.Count == 5)
                    break;
            }

            return true;
        }

        public static bool IsStraight(List<Card> set, ref List<int> values)
        {
            values.Clear();

            #region 1 rearrange by required order
            List<Card> tmpSet = new List<Card>();
            foreach (Card c in set)
            {
                tmpSet.Add(c);
            }
            tmpSet.Sort(new Card.SortIntComparer());
            int size = tmpSet.Count;
            for (int i = 0; i < size; ++i)
            {
                if (tmpSet[i].num == 1)
                {
                    Card newOne = new Card(14, tmpSet[i].shape);

                    tmpSet.Add(newOne);
                }
            }
            tmpSet.Reverse();
            #endregion

            #region 2 Find Staight cards
            List<Card>[] straight = new List<Card>[5];
            for (int i = 0; i < straight.Length; ++i)
                straight[i] = new List<Card>();
            int curConn = 0;

            for (int i = 0; i < tmpSet.Count - 1; ++i)
            {
                straight[curConn].Add(tmpSet[i]);

                if (curConn == 4)
                {
                    curConn = 0;
                    break;
                }

                if ((tmpSet[i].num - tmpSet[i + 1].num) == 1)
                {
                    curConn++;
                }
                else if (tmpSet[i + 1].num == tmpSet[i].num)
                {

                }
                else
                {
                    curConn = 0;
                    for (int j = 0; j < straight.Length; ++j)
                        straight[j].Clear();
                }

            }
            if (curConn == 4)
                straight[curConn].Add(tmpSet[tmpSet.Count - 1]);
            #endregion

            #region 3 straight check
            for (int i = 0; i < straight.Length; ++i)
            {
                if (straight[i].Count == 0)
                    return false;
            }
            values.Add(straight[0][0].num);
            if (values[0] == 14)
                values[0] = 1;
            #endregion


            return true;
        }

        public static bool IsThreeOfKind(List<Card> set, ref List<int> values)
        {
            values.Clear();

            #region 3 kind Check
            Dictionary<int, int> divMap = new Dictionary<int, int>();
            foreach (Card c in set)
            {
                if (!divMap.ContainsKey(c.num))
                    divMap[c.num] = 0;

                divMap[c.num]++;
            }
            int val = -1;
            foreach (int k in divMap.Keys)
            {
                if (divMap[k] != 3)
                    continue;

                if (k == 1)
                {
                    val = 1;
                    break;
                }

                if (k > val)
                {
                    val = k;
                }
            }
            if (val == -1)
                return false;
            values.Add(val);
            #endregion

            #region Get Kicker
            List<int> kickerList = new List<int>();
            foreach (Card c in set)
            {
                if (c.num == values[0])
                    continue;

                kickerList.Add(c.num == 1 ? 14 : c.num);
            }
            kickerList.Sort();
            values.Add(kickerList[kickerList.Count - 1] == 14 ? 1 : kickerList[kickerList.Count - 1]);
            values.Add(kickerList[kickerList.Count - 2] == 14 ? 1 : kickerList[kickerList.Count - 2]);
            #endregion

            return true;
        }

        public static bool IsTwoPair(List<Card> set, ref List<int> values)
        {
            values.Clear();

            #region pair Check
            Dictionary<int, int> divMap = new Dictionary<int, int>();
            foreach (Card c in set)
            {
                if (!divMap.ContainsKey(c.num))
                    divMap[c.num] = 0;

                divMap[c.num]++;
            }
            List<int> pairCards = new List<int>();
            foreach (int k in divMap.Keys)
            {
                if (divMap[k] != 2)
                    continue;

                pairCards.Add(k == 1 ? 14 : k);
            }
            if (pairCards.Count < 2)
                return false;

            #endregion

            #region Big Small pair
            pairCards.Sort();
            values.Add(pairCards[pairCards.Count - 1] == 14 ? 1 : pairCards[pairCards.Count - 1]);
            values.Add(pairCards[pairCards.Count - 2] == 14 ? 1 : pairCards[pairCards.Count - 2]);

            #endregion

            #region Get Kicker
            int kicker = -1;
            foreach (Card c in set)
            {
                if (c.num == values[0] || c.num == values[1])
                    continue;

                if (c.num == 1)
                {
                    kicker = 1;
                    break;
                }

                if (c.num > kicker)
                    kicker = c.num;
            }
            values.Add(kicker);
            #endregion

            return true;
        }

        public static bool IsPair(List<Card> set, ref List<int> values)
        {
            values.Clear();

            #region pair Check
            bool ret = false;
            Dictionary<int, int> divMap = new Dictionary<int, int>();
            foreach (Card c in set)
            {
                if (!divMap.ContainsKey(c.num))
                    divMap[c.num] = 0;

                divMap[c.num]++;
            }

            int val = -1;
            foreach (int k in divMap.Keys)
            {
                if (divMap[k] != 2)
                    continue;

                if (k == 1)
                {
                    val = 1;
                    break;
                }

                if (k > val)
                {
                    val = k;
                }
            }
            if (val != -1)
            {
                values.Add(val);
                ret = true;
            }




            #endregion

            #region Get Kickers
            List<int> kickers = new List<int>();
            foreach (Card c in set)
            {
                if (values.Count == 1 && c.num == values[0])
                    continue;

                kickers.Add(c.num == 1 ? 14 : c.num);
            }
            kickers.Sort();
            for (int i = kickers.Count - 1; i >= 0; i--)
            {
                values.Add(kickers[i] == 14 ? 1 : kickers[i]);

                if (values.Count >= 5)
                    break;
            }

            #endregion

            return ret;
        }

    }

    public class Situation
    {
        //if added, edit constructor & ClearPot()
        public readonly string m_p1;
        public readonly string m_p2;
        private List<Card> comm;
        private Dictionary<string, List<Card>> m_hand;
        private Dictionary<string, int> m_stack;

        private string m_turn;
        private string m_dealer;
        private string m_winner;
        private int pot; // actual
        private int bet; // vritual indicator
        private string m_stageHistory;
        public string StageHistory
        {
            get { return m_stageHistory; }
        }

        public List<Card> Comm
        {
            get
            {

                List<Card> ret = new List<Card>();

                foreach (var c in comm)
                {
                    ret.Add(c);
                }

                return ret;
            }
        }
        public List<Card> Hand(string player)
        {
            List<Card> ret = new List<Card>();

            foreach (var c in m_hand[player])
            {
                ret.Add(c);
            }

            return ret;
        }
        public string Turn
        {
            get { return m_turn; }
        }
        public string Winner
        {
            get { return m_winner; }
        }
        public int Stack(string key)
        {
            return m_stack[key];
        }
        public int Bet
        {
            get { return bet; }
        }
        public int Pot
        {
            get { return pot; }
        }

        public Situation(string p1, string p2, string dealer)
        {
            m_p1 = p1;
            m_p2 = p2;
            comm = new List<Card>();
            m_hand = new Dictionary<string, List<Card>>();
            m_hand[m_p1] = new List<Card>();
            m_hand[m_p2] = new List<Card>();
            m_stack = new Dictionary<string, int>();
            m_stack[m_p1] = Poker.STACK_INITIAL;
            m_stack[m_p2] = Poker.STACK_INITIAL;

            m_dealer = dealer;
            m_turn = dealer;
            m_winner = "";
            m_stageHistory = "";
        }
        public Situation(Situation situation)
        {
            m_p1 = situation.m_p1;
            m_p2 = situation.m_p2;
            comm = new List<Card>(situation.comm);
            m_hand = new Dictionary<string, List<Card>>();
            m_stack = new Dictionary<string, int>();
            foreach (var key in situation.m_hand.Keys)
                m_hand[key] = new List<Card>(situation.m_hand[key]);
            foreach (var key in situation.m_stack.Keys)
                m_stack[key] = situation.m_stack[key];

            m_turn = situation.m_turn;
            m_dealer = situation.m_dealer;
            pot = situation.pot;
            bet = situation.bet;
            m_winner = situation.m_winner;
            m_stageHistory = situation.m_stageHistory;
        }

        public void SetPreflop()
        {
            Debug.Assert(m_hand[m_p1].Count == 0);
            Debug.Assert(m_hand[m_p2].Count == 0);
            Debug.Assert(pot == 0);

            m_hand[m_p1].Add(GetNewCard());
            m_hand[m_p1].Add(GetNewCard());
            m_hand[m_p2].Add(GetNewCard());
            m_hand[m_p2].Add(GetNewCard());


            if (m_dealer == m_p1)
            {
                m_stack[m_p1] -= Poker.BLIND_SMALL;
                m_stack[m_p2] -= Poker.BLIND_BIG;
                m_turn = m_p1;
            }
            else
            {
                m_stack[m_p2] -= Poker.BLIND_SMALL;
                m_stack[m_p1] -= Poker.BLIND_BIG;
                m_turn = m_p2;
            }
            pot = Poker.BLIND_SMALL + Poker.BLIND_BIG;
            bet = Poker.BLIND_SMALL;
        }

        public void ChangCards()
        {
            m_hand[m_p2][0] = new Card(-1, Shape.Spade);
            m_hand[m_p2][1] = new Card(-1, Shape.Spade);
            m_hand[m_p2][0] = GetNewCard();
            m_hand[m_p2][1] = GetNewCard();
        }

        public void ClearPot()
        {
            if (m_winner == "d")
            {
                m_stack[m_p1] += pot / 2;
                m_stack[m_p2] += pot / 2;
            }
            else
            {
                m_stack[m_winner] += pot;
            }


            m_winner = "";
            pot = 0;
            bet = 0;
            m_stageHistory = "";
            comm.Clear();
            foreach (var key in m_hand.Keys)
                m_hand[key].Clear();

            m_dealer = m_dealer == m_p1 ? m_p2 : m_p1;
        }

        public bool IsTerminal()
        {
            return (m_winner != "");
        }
        public bool IsBankrupt(out string id)
        {
            id = "";

            bool isP1Bankrupt = (m_stack[m_p1] < Poker.BLIND_BIG);
            bool isP2Bankrupt = (m_stack[m_p2] < Poker.BLIND_BIG);

            if (isP1Bankrupt)
            {
                id = m_p1;
            }
            else if (isP2Bankrupt)
            {
                id = m_p2;
            }

            return (isP1Bankrupt || isP2Bankrupt);
        }
        public Stage GetCurStage()
        {
            if (m_hand[m_p1].Count == 0)
                return Stage.Start;
            switch (comm.Count)
            {
                case 0:
                    return Stage.Preflop;
                case 3:
                    return Stage.Flop;
                case 4:
                    return Stage.Turn;
                case 5:
                    return Stage.River;
                default:
                    Debug.Assert(false);
                    break;
            }

            return Stage.Start;
        }

        public List<ChoiceInfo> GetPossibleChoices()
        {
            List<ChoiceInfo> ret = new List<ChoiceInfo>();
            ret.Add(new ChoiceInfo(Choice.fold, m_turn, 0));

            if (bet == 0)
                ret.Add(new ChoiceInfo(Choice.check, m_turn, 0));
            else if (bet > 0)
                ret.Add(new ChoiceInfo(Choice.call, m_turn, 0));




            //DEBUG
            if (m_stageHistory.Count(c=>c=='R')>=4)
            {
                return ret;
            }



            int tmpBetSize = Poker.BLIND_BIG;
            var curStage = GetCurStage();
            if (curStage == Stage.Turn || curStage == Stage.River)
            {
                tmpBetSize *= 2;
            }

            int validStack;
            if (m_turn == m_p1)
            {
                validStack = Math.Min(m_stack[m_p1] - bet, m_stack[m_p2]);
            }
            else
            {
                validStack = Math.Min(m_stack[m_p1], m_stack[m_p2] - bet);
            }

            if (tmpBetSize <= validStack)
            {
                ret.Add(new ChoiceInfo(Choice.raise, m_turn, tmpBetSize));

            }
            

            return ret;
        }

        public Situation Next(ChoiceInfo choice)
        {
            Debug.Assert(!IsTerminal());
            Debug.Assert(choice.turn == m_turn);

            Situation newSit = new Situation(this);

            bool isBetFinished = false;
            switch (choice.choice)
            {
                case Choice.check:

                    Debug.Assert(newSit.bet == 0);
                    if (newSit.m_stageHistory.Length > 0)
                    {
                        isBetFinished = true;
                    }

                    newSit.m_stageHistory += "H";


                    break;
                case Choice.call:

                    Debug.Assert(newSit.bet > 0);
                    if (newSit.m_stageHistory.Length > 0)
                    {
                        isBetFinished = true;
                    }

                    newSit.pot += newSit.bet;
                    newSit.m_stack[newSit.m_turn] -= newSit.bet;
                    newSit.bet = 0;

                    string id;
                    if (newSit.IsBankrupt(out id))
                        newSit.AllInProcess();

                    newSit.m_stageHistory += "C";

                    break;
                case Choice.raise:

                    isBetFinished = false;

                    newSit.pot += newSit.bet;
                    newSit.m_stack[newSit.m_turn] -= newSit.bet;
                    newSit.m_stack[newSit.m_turn] -= choice.bet;
                    newSit.bet = choice.bet;
                    newSit.pot += choice.bet;

                    newSit.m_stageHistory += "R";

                    break;
                case Choice.fold:
                    newSit.m_winner = newSit.m_turn == m_p1 ? m_p2 : m_p1;

                    newSit.m_stageHistory += "F";

                    break;

                default:
                    Debug.Assert(false);
                    return null;
            }
            newSit.m_turn = newSit.m_turn == m_p1 ? m_p2 : m_p1;


            if (!newSit.IsTerminal() && isBetFinished)// next check
            {
                var curStage = newSit.GetCurStage();
                switch (curStage)
                {
                    case Stage.Preflop:
                        newSit.comm.Add(newSit.GetNewCard());
                        newSit.comm.Add(newSit.GetNewCard());
                        newSit.comm.Add(newSit.GetNewCard());
                        break;
                    case Stage.Flop:
                        newSit.comm.Add(newSit.GetNewCard());
                        break;
                    case Stage.Turn:
                        newSit.comm.Add(newSit.GetNewCard());
                        break;
                    case Stage.River:
                        int result = Poker.CalcWinner(newSit.comm, newSit.m_hand[m_p1], newSit.m_hand[m_p2]);
                        switch (result)
                        {
                            case 1:
                                newSit.m_winner = m_p1;
                                break;
                            case -1:
                                newSit.m_winner = m_p2;
                                break;
                            case 0:
                                newSit.m_winner = "d";
                                break;
                            default:
                                Debug.Assert(false);
                                return null;
                        }
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }

                if (!newSit.IsTerminal())
                {
                    newSit.m_stageHistory = "";

                    newSit.m_turn = newSit.m_dealer == m_p1 ? m_p2 : m_p1;
                }
            }



            return newSit;
        }

        public void AllInProcess()
        {
            while (comm.Count < 5)
            {
                comm.Add(GetNewCard());
            }

            int res = Poker.CalcWinner(comm, m_hand[m_p1], m_hand[m_p2]);
            switch (res)
            {
                case 1:
                    m_winner = m_p1;
                    break;
                case -1:
                    m_winner = m_p2;
                    break;
                case 0:
                    m_winner = "d";
                    break;
                default:
                    Debug.Assert(false);
                    return;
            }
        }

        public Card GetNewCard()
        {
            Random rand = new Random();
            bool isExist = false;
            Card newCard;
            do
            {
                isExist = false;
                newCard = new Card(rand.Next(1, 14), (Shape)rand.Next(0, 4));

                foreach (Card c in comm)
                {
                    if (newCard == c)
                    {
                        isExist = true;
                        break;
                    }
                }
                if (isExist)
                    continue;
                foreach (Card c in m_hand[m_p1])
                {
                    if (newCard == c)
                    {
                        isExist = true;
                        break;
                    }
                }
                if (isExist)
                    continue;
                foreach (Card c in m_hand[m_p2])
                {
                    if (newCard == c)
                    {
                        isExist = true;
                        break;
                    }
                }
                if (isExist)
                    continue;
            } while (isExist);

            return newCard;
        }
    }

}
