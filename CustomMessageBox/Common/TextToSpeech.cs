using System;
using System.Speech.Synthesis;

namespace CustomMessageBox.Common
{
    public class TextToSpeech
    {
      private static readonly SpeechSynthesizer  speechSynthesizer = SingletonSpeechObject.Instance;
        public static void MessageSound(string textMessage)
        {
            try
            {
                PromptStyle promptStyle = new PromptStyle
                {
                    Volume = PromptVolume.ExtraSoft,
                    Rate = PromptRate.Slow
                };
                speechSynthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen);
                speechSynthesizer.SpeakAsync(textMessage);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }
        public static void CloseMessageVoice()
        {
          speechSynthesizer.SpeakAsyncCancelAll();
        }
    }
}
