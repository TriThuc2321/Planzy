using CustomMessageBox.Common;
using System;
using System.Media;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CustomMessageBox
{
    public class DialogModelPopupViewModel : ViewModelBase  
    {
        #region Initialization of properties
        private Visibility _yesNoVisibility = Visibility.Collapsed;
        private Visibility _cancelVisibility = Visibility.Visible;
        private Visibility _okVisibility = Visibility.Collapsed;
        private Visibility _messageBoxIconVisibility = Visibility.Visible;
        private RelayCommand _yesCommand;
        private RelayCommand _cancelCommand;
        private RelayCommand _okCommand;
        private RelayCommand _closeCommand;
        private ImageSource _messageImageSource;
        private DialogModelPopup _view;
        private string _title;
        private string _message;
        private string _yesContent = Constants.YesContent;
        private string _cancelContent = Constants.CancelContent;
        private string _okContent = Constants.OKContent;
        public CustomMessageBoxResult Result { get; set; }
        public string YesContent
        {
            get
            {
                return _yesContent;
            }
            set
            {
                base.SetProperty<string>(ref _yesContent, value, "YesContent");
            }
        }
        public string CancelContent
        {
            get
            {
                return _cancelContent;
            }
            set
            {
                base.SetProperty<string>(ref _cancelContent, value, "CancelContent");
            }
        }
        public string OKContent
        {
            get
            {
                return _okContent;
            }
            set
            {
                base.SetProperty<string>(ref _okContent, value, "NoContent");
            }
        }
        public Visibility MessageBoxIconVisibility
        {
            get
            {
                return _messageBoxIconVisibility;
            }
            set
            {
                if (_yesNoVisibility != value)
                {
                    base.SetProperty<Visibility>(ref _messageBoxIconVisibility, value, "MessageBoxIconVisibility");
                }
            }
        }
        public Visibility YesNoVisibility
        {
            get { return _yesNoVisibility; }
            set
            {
                if (_yesNoVisibility != value)
                {
                    base.SetProperty<Visibility>(ref _yesNoVisibility, value, "YesNoVisibility");
                }
            }
        }

        public Visibility CancelVisibility
        {
            get { return _cancelVisibility; }
            set
            {
                if (_cancelVisibility != value)
                {
                    base.SetProperty<Visibility>(ref _cancelVisibility, value, "CancelVisibility");
                }
            }
        }

        public Visibility OkVisibility
        {
            get { return _okVisibility; }
            set
            {
                if (_okVisibility != value)
                {
                    base.SetProperty<Visibility>(ref _okVisibility, value, "OkVisibility");
                }
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    base.SetProperty<string>(ref _title, value, "Title");
                }
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    base.SetProperty<string>(ref _message, value, "Message");
                }
            }
        }

        public ImageSource MessageImageSource
        {
            get { return _messageImageSource; }
            set
            {
                _messageImageSource = value;
                base.SetProperty<ImageSource>(ref _messageImageSource, value, "MessageImageSource");
            }
        }
        #endregion

        #region Button Style
        private void MessageBoxButtonStyle(CustomMessageBoxButton messageBoxButton)
        {
            try
            {
                switch (messageBoxButton)
                {
                    case CustomMessageBoxButton.OK:
                        CancelVisibility = Visibility.Collapsed;
                        YesNoVisibility = Visibility.Collapsed;
                        OkVisibility = Visibility.Visible;
                        OKContent = Constants.OKContent;
                        break;
                    case CustomMessageBoxButton.OKCancel:
                        YesNoVisibility = Visibility.Collapsed;
                        CancelVisibility = Visibility.Visible;
                        OkVisibility = Visibility.Visible;
                        OKContent = Constants.OKContent;
                        CancelContent = Constants.CancelContent;
                        break;
                    case CustomMessageBoxButton.YesNo:
                        YesNoVisibility = Visibility.Collapsed;
                        CancelContent = Constants.NoContent;
                        OKContent = Constants.YesContent;
                        OkVisibility = Visibility.Visible;
                        break;
                    case CustomMessageBoxButton.YesNoCancel:
                        YesNoVisibility = Visibility.Visible;
                        YesContent = Constants.YesContent;
                        OKContent = Constants.NoContent;
                        CancelContent = Constants.CancelContent;
                        OkVisibility = Visibility.Visible;
                        break;
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
        }
        private void MessageSound()
        {
            try
            {
                switch (Constants.messageBoxImage)
                {
                    case CustomMessageBoxImage.None:
                        SystemSounds.Beep.Play();
                        break;
                    case CustomMessageBoxImage.Stop:
                        SystemSounds.Hand.Play();
                        break;
                    case CustomMessageBoxImage.Asterisk:
                        SystemSounds.Asterisk.Play();
                        break;
                    case CustomMessageBoxImage.Information:
                        SystemSounds.Asterisk.Play();
                        break;
                    case CustomMessageBoxImage.Hand:
                        SystemSounds.Hand.Play();
                        break;
                    case CustomMessageBoxImage.Error:
                        SystemSounds.Beep.Play();
                        break;
                    case CustomMessageBoxImage.Exclamation:
                        SystemSounds.Exclamation.Play();
                        break;
                    case CustomMessageBoxImage.Warning:
                        SystemSounds.Beep.Play();
                        break;
                    case CustomMessageBoxImage.Question:
                        SystemSounds.Question.Play();
                        break;
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
        }
        private void MessageBoxImageIcon(CustomMessageBoxImage messageBoxImage)
        {
            try
            {
                Constants.messageBoxImage = messageBoxImage;
                switch (messageBoxImage)
                {
                    case CustomMessageBoxImage.None:
                        MessageBoxIconVisibility = Visibility.Hidden;
                        break;
                    case CustomMessageBoxImage.Stop:
                        MessageBoxIconVisibility = Visibility.Visible;
                        MessageImageSource = new BitmapImage(new Uri(Constants.StopImageIcon));
                        break;
                    case CustomMessageBoxImage.Asterisk:
                        MessageBoxIconVisibility = Visibility.Visible;
                        MessageImageSource = new BitmapImage(new Uri(Constants.InfoIconImageIcon));
                        break;
                    case CustomMessageBoxImage.Information:
                        MessageBoxIconVisibility = Visibility.Visible;
                        MessageImageSource = new BitmapImage(new Uri(Constants.InfoIconImageIcon));
                        break;
                    case CustomMessageBoxImage.Hand:
                        MessageBoxIconVisibility = Visibility.Visible;
                        MessageImageSource = new BitmapImage(new Uri(Constants.HandImageIcon));
                        break;
                    case CustomMessageBoxImage.Error:
                        MessageBoxIconVisibility = Visibility.Visible;
                        MessageImageSource = new BitmapImage(new Uri(Constants.ErrorIconImageIcon));
                        break;
                    case CustomMessageBoxImage.Exclamation:
                        MessageBoxIconVisibility = Visibility.Visible;
                        MessageImageSource = new BitmapImage(new Uri(Constants.ExclIconImageIcon));
                        break;
                    case CustomMessageBoxImage.Warning:
                        MessageBoxIconVisibility = Visibility.Visible;
                        MessageImageSource = new BitmapImage(new Uri(Constants.WarningImageIcon));
                        break;
                    case CustomMessageBoxImage.Question:
                        MessageBoxIconVisibility = Visibility.Visible;
                        MessageImageSource = new BitmapImage(new Uri(Constants.QuestionmarkIconImageIcon));
                        break;
                    
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
        }
        #endregion

        #region Commands events
        public RelayCommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                    _closeCommand = new RelayCommand(obj=>CloseCommandFunction());
                return _closeCommand;
            }
        }
        public RelayCommand YesCommand
        {
            get
            {
                if (_yesCommand == null)
                    _yesCommand = new RelayCommand(obj => YesCommandFunction());
                return _yesCommand;
            }
        }
        private void YesCommandFunction()
        {
            try
            {
                Result = CustomMessageBoxResult.Yes;
                MessageSound();
                TextToSpeech.CloseMessageVoice();
                _view.Close();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }
        private void OKCommandFunction()
        {
            try
            {
                var textBox = OKContent;
                if (textBox.ToString() == Constants.OKContent)
                {
                    Result = CustomMessageBoxResult.OK;
                    SystemSounds.Beep.Play();
                }
                else if (textBox.ToString() == Constants.NoContent)
                {
                    Result = CustomMessageBoxResult.No;
                }
                else if (textBox.ToString() == Constants.YesContent)
                {
                    Result = CustomMessageBoxResult.Yes;
                     SystemSounds.Beep.Play();
                }
                MessageSound();
                TextToSpeech.CloseMessageVoice();
                _view.Close();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }
        private void CancelCommandFunction()
        {
            try
            {
                var textBox = CancelContent;
                if (textBox.ToString() == Constants.CancelContent)
                {
                    Result = CustomMessageBoxResult.Cancel;
                }
                else if (textBox.ToString() == Constants.NoContent)
                {
                    Result = CustomMessageBoxResult.No;
                }
                else if (textBox.ToString() == Constants.OKContent)
                {
                    Result = CustomMessageBoxResult.OK;
                    SystemSounds.Beep.Play();
                }
                MessageSound();
                TextToSpeech.CloseMessageVoice();
                _view.Close();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }
        private void CloseCommandFunction()
        {
            try
            {
                Result = CustomMessageBoxResult.None;
                MessageSound();
                TextToSpeech.CloseMessageVoice();
                _view.Close();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }

        }
        // called when the cancel button is pressed
        public RelayCommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new RelayCommand(obj=>CancelCommandFunction());
                return _cancelCommand;
            }
        }

        // called when the ok button is pressed
        public RelayCommand OkCommand
        {
            get
            {
                if (_okCommand == null)
                    _okCommand = new RelayCommand(obj=>OKCommandFunction());
                return _okCommand;
            }
        }
        #endregion

        #region Show Dialog
        public void OpenDialog(DialogModelPopup viewObject, string MessageBoxText, string Caption, CustomMessageBoxButton messageBoxButton, CustomMessageBoxImage messageBoxImage)
        {
            try
            {
                _view = viewObject;
                Title = Caption;
                Message = MessageBoxText;
                MessageBoxImageIcon(messageBoxImage);
                if (messageBoxButton == CustomMessageBoxButton.OK)
                {
                    OkVisibility = Visibility.Collapsed;
                    CancelVisibility = Visibility.Visible;
                    YesNoVisibility = Visibility.Collapsed;
                    CancelContent = Constants.OKContent;
                    MessageBoxIconVisibility = Visibility.Visible;
                    SystemSounds.Beep.Play();
                    MessageImageSource = new BitmapImage(new Uri(Constants.OKIconImageIcon));
                }
                else
                {
                    MessageBoxButtonStyle(messageBoxButton);
                }
            }
            catch (Exception ex)
            {
                _view.Close();
                _ = ex.Message;
            }

        }
        public void OpenDialog(DialogModelPopup viewObject, string MessageBoxText)
        {
            try
            {
                _view = viewObject;
                Title = string.Empty;
                Message = MessageBoxText;
                OkVisibility = Visibility.Collapsed;
                CancelVisibility = Visibility.Visible;
                YesNoVisibility = Visibility.Collapsed;
                CancelContent = Constants.OKContent;
                MessageBoxIconVisibility = Visibility.Visible;
                SystemSounds.Beep.Play();
                MessageImageSource = new BitmapImage(new Uri(Constants.OKIconImageIcon));
            }
            catch (Exception ex)
            {
                _view.Close();
                _ = ex.Message;
            }

        }
        public void OpenDialog(DialogModelPopup viewObject, string MessageBoxText, string Caption)
        {
            try
            {
                _view = viewObject;
                Title = Caption;
                Message = MessageBoxText;
                OkVisibility = Visibility.Collapsed;
                CancelVisibility = Visibility.Visible;
                YesNoVisibility = Visibility.Collapsed;
                CancelContent = Constants.OKContent;
                MessageBoxIconVisibility = Visibility.Visible;
                SystemSounds.Beep.Play();
                MessageImageSource = new BitmapImage(new Uri(Constants.OKIconImageIcon));
            }
            catch (Exception ex)
            {
                _view.Close();
                _ = ex.Message;
            }

        }
        public void OpenDialog(DialogModelPopup viewObject, string MessageBoxText, string Caption, CustomMessageBoxButton messageBoxButton)
        {
            try
            {
                _view = viewObject;
                Title = Caption;
                Message = MessageBoxText;

                if (messageBoxButton == CustomMessageBoxButton.OK)
                {
                    OkVisibility = Visibility.Collapsed;
                    CancelVisibility = Visibility.Visible;
                    YesNoVisibility = Visibility.Collapsed;
                    CancelContent = Constants.OKContent;
                    MessageBoxIconVisibility = Visibility.Visible;
                    SystemSounds.Beep.Play();
                    MessageImageSource = new BitmapImage(new Uri(Constants.OKIconImageIcon));
                }
                else
                {
                    MessageBoxButtonStyle(messageBoxButton);
                }
            }
            catch (Exception ex)
            {
                _view.Close();
                _ = ex.Message;
            }

        }
        #endregion
    }
}
