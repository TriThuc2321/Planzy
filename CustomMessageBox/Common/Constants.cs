using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace CustomMessageBox
{
   public class Constants
    {

        public const string NoContent = "No";
        public const string YesContent = "Yes";
        public const string CancelContent = "Cancel";
        public const string OKContent = "OK";
        public const string OkImageIcon = "pack://application:,,,/CustomMessageBox;component/Resources/Images/OkImageIcon.png";
        public const string HandImageIcon = "pack://application:,,,/CustomMessageBox;component/Resources/Images/HandPngIcon.png";
        public const string StopImageIcon = "pack://application:,,,/CustomMessageBox;component/Resources/Images/StopIcon.png";
        public const string InfoIconImageIcon = "pack://application:,,,/CustomMessageBox;component/Resources/Images/InfoIcon.png";
        public const string ErrorIconImageIcon = "pack://application:,,,/CustomMessageBox;component/Resources/Images/ErrorIcon.png";
        public const string ExclIconImageIcon = "pack://application:,,,/CustomMessageBox;component/Resources/Images/ExclIcon.png";
        public const string WarningImageIcon = "pack://application:,,,/CustomMessageBox;component/Resources/Images/Warning.png";
        public const string QuestionmarkIconImageIcon = "pack://application:,,,/CustomMessageBox;component/Resources/Images/QuestionmarkIcon.png";
        public const string OKIconImageIcon = "pack://application:,,,/CustomMessageBox;component/Resources/Images/OkImageIcon.png";
        //Return Values
        public static CustomMessageBoxImage messageBoxImage = CustomMessageBoxImage.None;
    }
}
