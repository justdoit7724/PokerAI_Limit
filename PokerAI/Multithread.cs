using PokerLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace PokerAI
{

    public static class Multithread_CFR
    {
        private static MCCFR m_cfr;

        private static Situation m_startSItu;
        private static bool stopTrain;
        private static bool isInTrain;
        private static bool endThread=false;

        public static void init(Situation startSitu)
        {
            m_cfr = new MCCFR();

            string path = Application.ExecutablePath + "\\..\\..\\..\\data\\cfrData.json";
            if (File.Exists(path))
            {
                JavaScriptSerializer serial = new JavaScriptSerializer();
                m_cfr.I = serial.Deserialize<Dictionary<string,MCCFR.Node>>(File.ReadAllText(path));
            }

            m_startSItu = startSitu;
            isInTrain = false;
            stopTrain = false;
        }

        public static void Lock()
        {
            stopTrain = true;

            while (isInTrain)
            {
                Thread.Sleep(50);
            }
        }
        public static void Unlock()
        {
            isInTrain = true;
            stopTrain = false;
        }
        public static MCCFR Get
        {
            get { return m_cfr; }
        }

        public static void End()
        {
            Lock();
            endThread = true;

            string path = Application.ExecutablePath + "\\..\\..\\..\\data\\cfrData.json";
            
            JavaScriptSerializer serial = new JavaScriptSerializer();

            if(File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllText(path, serial.Serialize(m_cfr.I));
        }

        public static void Train()
        {
            while (!endThread)
            {
                while (!stopTrain)
                {
                    isInTrain = true;

                    Situation newSitu = new Situation(m_startSItu);
                    if (newSitu.GetCurStage() == Stage.Start)
                        newSitu.SetPreflop();


                    m_cfr.Process(newSitu);

                }

                isInTrain = false;
            }

        }
    }
}
