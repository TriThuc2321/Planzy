using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace CustomMessageBox.Common
{
    public sealed  class SingletonSpeechObject
    {
        private static readonly object padlock = new object();
        private static SpeechSynthesizer instance = null;
        public static SpeechSynthesizer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new SpeechSynthesizer();
                        }
                    }
                }
                return instance;
            }
        }
    }
}
