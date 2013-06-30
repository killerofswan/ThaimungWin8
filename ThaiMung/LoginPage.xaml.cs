using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json.Linq;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ThaiMung
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {   
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisPage));
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
           // var s = new SendPHP();
            //s.login(emailText.Text, passwordText.Password);
            login(emailText.Text,passwordText.Password);
            //Frame.Navigate(typeof(MainPage));
        }

        public async void login(string email,string pass) {
            HttpClient client = new HttpClient();
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("email", email));
            postData.Add(new KeyValuePair<string, string>("password", pass));

            HttpContent c = new FormUrlEncodedContent(postData);
            var response = await client.PostAsync("http://localhost/login.php", c);
            var str = await response.Content.ReadAsStringAsync();
            string s = str.ToString();
            errorText.Text = s;
            try
            {
                if (s.Substring(0, 4).Equals("[{pa"))
                {
                    errorText.Text = "Invalid Email Or Password";
                }
                else
                {
                    var o = JArray.Parse(s);
                    int tmpid = (int)o[0]["id"];
                    string e_mail = (string)o[0]["email"];
                    Frame.Navigate(typeof(MainPage), new User
                    {
                        id = tmpid,
                        email = e_mail,
                        name = (string)o[0]["name"],
                        lastname = (string)o[0]["lastname"],
                        phone = (string)o[0]["phone"],
                        password = (string)o[0]["password"],
                        countban = (int)o[0]["countban"],
                        countspam = (int)o[0]["countspam"],
                        status = (int)o[0]["status"]
                    });
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                errorText.Text = "Invalid Email Or Password";
            }

        }


    }
}
