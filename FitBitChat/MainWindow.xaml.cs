using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FitBitChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Step Step { get; set; }

        public Client Client { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Client = LoadClient();
            Step = Step.OpenLoginPage;
            Browser.Address = "https://myhelp.fitbit.com/PreChat3?chatType=support&chatLocation=helpsite&co=US&l=en_US";
            Browser.LoadingStateChanged += Browser_LoadingStateChanged;
        }

        Client LoadClient()
        {
            return new Client()
            {
                Email = "m.dilshod.96@mail.ru",
                Password = "xavi1066",
                FirstName = "Dilshod",
                LastName = "Komilov",
                Country = "Canada",
                Product = "Versa",
                Issue = "Battery"
            };
        }

        public void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {
                if (Step == Step.OpenLoginPage)
                {
                    FirstStep();
                }
                else if (Step == Step.FieldsAfterLogin)
                {
                    SecondStep();
                }
            }
        }

        #region FirstStep

        void FirstStep()
        {
            FillLoginAndPasswordFields();

            WaitForFieldsSettedAndClick();
        }

        void FillLoginAndPasswordFields()
        {
            var script = $"document.getElementById('ember643').focus();";
            Browser.ExecuteScriptAsync(script);
            script = $"document.getElementById('ember643').value = '{Client.Email}';";
            Browser.ExecuteScriptAsync(script);

            script = $"document.getElementById('ember644').value = '{Client.Password}';";
            Browser.ExecuteScriptAsync(script);
            script = $"document.getElementById('ember644').focus();";
            Browser.ExecuteScriptAsync(script);

            script = $"document.getElementById('ember643').focus();";
            Browser.ExecuteScriptAsync(script);
        }

        void ClickLoginButton()
        {
            var script = $"document.getElementById('ember684').click();";
            Browser.ExecuteScriptAsync(script);
            Step = Step.FieldsAfterLogin;
        }

        async void WaitForFieldsSettedAndClick()
        {
            bool loading = true;
            while (loading)
            {
                if (Browser.CanExecuteJavascriptInMainFrame)
                {
                    var script = $"document.getElementById('ember643').value;";
                    var emailFieldResponse = await Browser.EvaluateScriptAsync(script);

                    script = $"document.getElementById('ember644').value";
                    var passwordFieldResponse = await Browser.EvaluateScriptAsync(script);

                    if (emailFieldResponse.Success && passwordFieldResponse.Success)
                    {

                        if (emailFieldResponse.Result.ToString() != "" && passwordFieldResponse.Result.ToString() != "")
                        {
                            loading = false;
                        }

                    }
                    FillLoginAndPasswordFields();
                }
                Thread.Sleep(20);
            }
            ClickLoginButton();
        }

        #endregion

        #region SecondStep

        void SecondStep()
        {
            FillClientDataFields();
            WaitForClientDataSettedAndClick();
        }

        void FillClientDataFields()
        {
            var script = $"document.getElementById('contactFirstName').focus();";
            Browser.ExecuteScriptAsync(script);
            script = $"document.getElementById('contactFirstName').value = '{Client.FirstName}';";
            Browser.ExecuteScriptAsync(script);

            script = $"document.getElementById('contactLastName').focus();";
            Browser.ExecuteScriptAsync(script);
            script = $"document.getElementById('contactLastName').value = '{Client.LastName}';";
            Browser.ExecuteScriptAsync(script);

            script = $"document.getElementById('email').focus();";
            Browser.ExecuteScriptAsync(script);
            script = $"document.getElementById('email').value = '{Client.Email}';";
            Browser.ExecuteScriptAsync(script);

            script = $"document.getElementById('country_input').focus();";
            Browser.ExecuteScriptAsync(script);
            script = $"document.getElementById('country_input').selectedIndex = 5;";
            Browser.ExecuteScriptAsync(script);


            script = $"document.getElementById('caseProduct').focus();";
            Browser.ExecuteScriptAsync(script);
            script = $"document.getElementById('caseProduct').value = '{Client.Product}';";
            Browser.ExecuteScriptAsync(script);


            script = $"document.getElementById('caseLevel1').focus();";
            Browser.ExecuteScriptAsync(script);
            script = $"document.getElementById('caseLevel1').value = '{Client.Issue}';";
            Browser.ExecuteScriptAsync(script);


            script = $"document.getElementById('contactFirstName').focus();";
            Browser.ExecuteScriptAsync(script);
        }

        async void WaitForClientDataSettedAndClick()
        {
            bool loading = true;
            while (loading)
            {
                if (Browser.CanExecuteJavascriptInMainFrame)
                {
                    var script = $"document.getElementById('contactFirstName').value;";
                    var firstName = await Browser.EvaluateScriptAsync(script);

                    script = $"document.getElementById('country_input').value";
                    var contry = await Browser.EvaluateScriptAsync(script);

                    script = $"document.getElementById('caseProduct').value";
                    var product = await Browser.EvaluateScriptAsync(script);

                    script = $"document.getElementById('caseLevel1').value";
                    var issue = await Browser.EvaluateScriptAsync(script);

                    if (firstName.Success && contry.Success)
                    {

                        if (firstName.Result.ToString() != "" && product.Result.ToString() != "" && issue.Result.ToString() != "")
                        {
                            loading = false;
                        }

                    }
                    FillClientDataFields();
                }
                Thread.Sleep(20);
            }
            ClickResponseChatButton();
        }


        void ClickResponseChatButton()
        {
            var script = $"document.getElementById('prechat_submit').click();";
            Browser.ExecuteScriptAsync(script);
            Step = Step.StepCompleted;
        }

        #endregion
    }

    public enum Step
    {
        OpenLoginPage,
        FieldsAfterLogin,
        StepCompleted
    }
}
