using CustomMessageBox.Common;
using System;


namespace CustomMessageBox
{
    public sealed class CustomizeMessageBox
    {
        public  static CustomizeMessageBoxResult Show(string messageBoxText, MessageSoundOptions messageSoundEnabled = MessageSoundOptions.No)
        {
            try
            {
                var _viewModel = new ModelDialogViewModel();
                var messageBoxObj = new ModelDialog
                {
                    DataContext = _viewModel
                };
                _viewModel.OpenDialog(messageBoxObj, messageBoxText);
                if (messageSoundEnabled == MessageSoundOptions.Yes)
                {
                    TextToSpeech.MessageSound(messageBoxText);
                }
                messageBoxObj.ShowDialog();
                return _viewModel.MessageBoxResultValue;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return CustomizeMessageBoxResult.None;
            }
        }
        public static CustomizeMessageBoxResult Show(string messageBoxText, string caption, MessageSoundOptions messageSoundEnabled = MessageSoundOptions.No)
        {
            try
            {
                var _viewModel = new ModelDialogViewModel();
                var messageBoxObj = new ModelDialog
                {
                    DataContext = _viewModel
                };
                _viewModel.OpenDialog(messageBoxObj, messageBoxText, caption);
                if (messageSoundEnabled == MessageSoundOptions.Yes)
                {
                    TextToSpeech.MessageSound(messageBoxText);
                }
                messageBoxObj.ShowDialog();
                return _viewModel.MessageBoxResultValue;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return CustomizeMessageBoxResult.None;
            }
        }
        public static CustomizeMessageBoxResult Show(string messageBoxText, string caption, CustomMessageBoxButton messageBoxButton, MessageSoundOptions messageSoundEnabled = MessageSoundOptions.No)
        {
            try
            {
                var _viewModel = new ModelDialogViewModel();
                var messageBoxObj = new ModelDialog
                {
                    DataContext = _viewModel
                };
                _viewModel.OpenDialog(messageBoxObj, messageBoxText, caption, messageBoxButton);
                if (messageSoundEnabled == MessageSoundOptions.Yes)
                {
                    TextToSpeech.MessageSound(messageBoxText);
                }
                messageBoxObj.ShowDialog();
                return _viewModel.MessageBoxResultValue;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return CustomizeMessageBoxResult.None;
            }
        }
        public static CustomizeMessageBoxResult Show(string messageBoxText, string caption, CustomMessageBoxButton messageBoxButton, CustomMessageBoxImage messageBoxImage, MessageSoundOptions messageSoundEnabled = MessageSoundOptions.No)
        {
            try
            {
                var _viewModel = new ModelDialogViewModel();
                var messageBoxObj = new ModelDialog
                {
                    DataContext = _viewModel
                };
                _viewModel.OpenDialog(messageBoxObj, messageBoxText, caption, messageBoxButton, messageBoxImage);
                if (messageSoundEnabled == MessageSoundOptions.Yes)
                {
                    //TextToSpeech.MessageSound(messageBoxText);
                }
                messageBoxObj.ShowDialog();
                
                return _viewModel.MessageBoxResultValue;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return CustomizeMessageBoxResult.None;
            }
        }
    }
}
