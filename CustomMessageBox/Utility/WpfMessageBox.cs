using CustomMessageBox;
using CustomMessageBox.Common;
using System;

namespace CustomMessageBox
{
    public sealed class WpfMessageBox
    {
        public static CustomMessageBoxResult Show(string messageBoxText, MessageSoundOptions messageSoundEnabled = MessageSoundOptions.No)
        {
            try
            {
                var _viewModel = new DialogModelPopupViewModel();
                var messageBoxObj = new DialogModelPopup
                {
                    DataContext = _viewModel
                };
                _viewModel.OpenDialog(messageBoxObj, messageBoxText);
                if (messageSoundEnabled == MessageSoundOptions.Yes)
                {
                    TextToSpeech.MessageSound(messageBoxText);
                }
                messageBoxObj.ShowDialog();
                return _viewModel.Result;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return CustomMessageBoxResult.None;
            }
        }
        public static CustomMessageBoxResult Show(string messageBoxText, string caption, MessageSoundOptions messageSoundEnabled = MessageSoundOptions.No)
        {
            try
            {
                var _viewModel = new DialogModelPopupViewModel();
                var messageBoxObj = new DialogModelPopup();
                messageBoxObj.DataContext = _viewModel;
                _viewModel.OpenDialog(messageBoxObj, messageBoxText, caption);
                if (messageSoundEnabled == MessageSoundOptions.Yes)
                {
                    TextToSpeech.MessageSound(messageBoxText);
                }
                messageBoxObj.ShowDialog();
                return _viewModel.Result;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return CustomMessageBoxResult.None;
            }
        }
        public static CustomMessageBoxResult Show(string messageBoxText, string caption, CustomMessageBoxButton messageBoxButton, MessageSoundOptions messageSoundEnabled = MessageSoundOptions.No)
        {
            try
            {
                var _viewModel = new DialogModelPopupViewModel();
                var messageBoxObj = new DialogModelPopup();
                messageBoxObj.DataContext = _viewModel;
                _viewModel.OpenDialog(messageBoxObj, messageBoxText, caption, messageBoxButton);
                if (messageSoundEnabled == MessageSoundOptions.Yes)
                {
                    TextToSpeech.MessageSound(messageBoxText);
                }
                messageBoxObj.ShowDialog();
                return _viewModel.Result;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return CustomMessageBoxResult.None;
            }
        }
        public static CustomMessageBoxResult Show(string messageBoxText, string caption, CustomMessageBoxButton messageBoxButton, CustomMessageBoxImage messageBoxImage, MessageSoundOptions messageSoundEnabled = MessageSoundOptions.No)
        {
            try
            {
                var _viewModel = new DialogModelPopupViewModel();
                var messageBoxObj = new DialogModelPopup();
                messageBoxObj.DataContext = _viewModel;
                _viewModel.OpenDialog(messageBoxObj, messageBoxText, caption, messageBoxButton, messageBoxImage);
                if (messageSoundEnabled == MessageSoundOptions.Yes)
                {
                    TextToSpeech.MessageSound(messageBoxText);
                }
                messageBoxObj.ShowDialog();
                return _viewModel.Result;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return CustomMessageBoxResult.None;
            }
        }
    }
}
