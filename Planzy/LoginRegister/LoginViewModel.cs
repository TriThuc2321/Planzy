using Planzy.LoginRegister;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using Condition = System.Windows.Automation.Condition;
using System.Windows.Threading;
using System.Windows.Interop;
using System.IO;
using System.Text.RegularExpressions;
using Planzy.Commands;

namespace Planzy.LoginRegister
{
    class LoginViewModel: INotifyPropertyChanged
    {
        Window loginWindow;
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand LoginGoogleCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public LoginViewModel()
        {
            LoginGoogleCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { LoginGoogleClick(p); });
            ExitCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { Exit(p); });
        }
        private void Exit(Window p)
        {
            p.Close();
        }
        private void LoginGoogleClick(Window p)
        {
            this.loginWindow = p;
            var url = UserResponse.GetAutenticationURI(clientId, redirectURI).ToString();
            var psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(psi);
            //Process.Start(url);
            //
            Thread.Sleep(1000);
            DisplayMemoryUsageInTitleAsync();
        }

        public const string clientId = "147215247319-dgmnt8q8l4rf4lu22hl7njqg7jo29l94.apps.googleusercontent.com";
        public const string clientSecret = "ogfkpR8xxt6fkP2TQw7QNJCU";
        public const string redirectURI = "urn:ietf:wg:oauth:2.0:oob";
        public bool flag = true;

        UserResponse access;       

        private void GetProfile(string approveCode)
        {
            access = UserResponse.Exchange(approveCode, clientId, clientSecret, redirectURI);
            var url = $"https://www.googleapis.com/oauth2/v3/userinfo?access_token={access.Access_token}";
            var wc = new WebClient();

            wc.Encoding = Encoding.UTF8;
            var jsonProfile = wc.DownloadString(url);

            MainWindow mainForm = new MainWindow(jsonProfile);
            mainForm.Show();
            
            loginWindow.Close();

        }


        private void DisplayMemoryUsageInTitleAsync()
        {
            BackgroundWorker wrkr = new BackgroundWorker();
            wrkr.WorkerReportsProgress = true;

            wrkr.DoWork += (object sender, DoWorkEventArgs e) => {

                bool result;
                while (result = GetAppoveCodeGoogle())
                {
                    wrkr.ReportProgress(0, result);
                    Thread.Sleep(100);
                }

                wrkr.Dispose();
                Process[] procsChrome = Process.GetProcessesByName("chrome");
                foreach (Process chrome in procsChrome)
                {
                    if (chrome.MainWindowHandle == IntPtr.Zero)
                        continue;

                    AutomationElement element = AutomationElement.FromHandle(chrome.MainWindowHandle);
                    if (element != null)
                    {
                        Condition conditions = new AndCondition(
                       new PropertyCondition(AutomationElement.ProcessIdProperty, chrome.Id),
                       new PropertyCondition(AutomationElement.IsControlElementProperty, true),
                       new PropertyCondition(AutomationElement.IsContentElementProperty, true),
                       new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));

                        AutomationElement elementx = element.FindFirst(TreeScope.Descendants, conditions);
                        var url = ((ValuePattern)elementx.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
                        if (url.Contains("accounts.google.com/o/oauth2/approval/v2/approvalnativeap"))
                        {
                            ((ValuePattern)elementx.GetCurrentPattern(ValuePattern.Pattern)).SetValue("https://google.com");
                            ChromeWrapper chromes = new ChromeWrapper();

                            chromes.SendKey((char)13, chrome);//enter
                            var arr = url.Split('&');
                            var approvalCode = WebUtility.HtmlDecode(arr[arr.Length - 1].Replace("approvalCode=", ""));
                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                GetProfile(approvalCode);
                            }));
                        }
                    }
                }
                wrkr.Dispose();
            };
            wrkr.RunWorkerAsync();
        }

        public bool GetAppoveCodeGoogle()
        {
            Process[] procsChrome = Process.GetProcessesByName("chrome");
            foreach (Process chrome in procsChrome)
            {
                if (chrome.MainWindowHandle == IntPtr.Zero)
                    continue;

                AutomationElement element = AutomationElement.FromHandle(chrome.MainWindowHandle);
                if (element != null)
                {
                    Condition conditions = new AndCondition(
                    new PropertyCondition(AutomationElement.ProcessIdProperty, chrome.Id),
                    new PropertyCondition(AutomationElement.IsControlElementProperty, true),
                    new PropertyCondition(AutomationElement.IsContentElementProperty, true),
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));

                    AutomationElement elementx = element.FindFirst(TreeScope.Descendants, conditions);
                    var url = ((ValuePattern)elementx.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
                    if (url.Contains("accounts.google.com/o/oauth2/approval/v2/approvalnativeap"))
                    {
                        var arr = url.Split('&');
                        var approvalCode = WebUtility.HtmlDecode(arr[arr.Length - 1].Replace("approvalCode=", ""));
                        return false;
                    }

                }
            }
            return true;
        }


    }
}

