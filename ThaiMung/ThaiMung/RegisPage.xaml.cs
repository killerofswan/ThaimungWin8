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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace ThaiMung
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class RegisPage : ThaiMung.Common.LayoutAwarePage
    {
        public RegisPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            /*int chkstate = 0;
            ////if email ซ้ำ 
            //{
            
            //}

            if (!passRegis.Password.Equals(conpassRegis.Password))
            {
                passerror.Text = "Confirm Paasword not correct";
                chkstate = 1;
            }
            if (phoneRegis.Text.Length != 10) 
            {
                phoneerror.Text = "Wrong phone number";
                chkstate = 1;
            }

            if (chkstate == 0)
            {
                this.Frame.GoBack();
            }*/
            registration(emailRegis.Text,passRegis.Password,fnameRegis.Text,lnameRegis.Text,phoneRegis.Text);
           
        }

        public async void registration(string email,string pass,string name,string lastName,string phone) {
            HttpClient client = new HttpClient();

            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("email", email));
            postData.Add(new KeyValuePair<string, string>("password", pass));
            postData.Add(new KeyValuePair<string, string>("name", name));
            postData.Add(new KeyValuePair<string, string>("lastname", lastName));
            postData.Add(new KeyValuePair<string, string>("phone", phone));

            HttpContent c = new FormUrlEncodedContent(postData);
            var response = await client.PostAsync("http://localhost/regis.php",c);
            var str = await response.Content.ReadAsStringAsync();
            string s = str.ToString();
            if (!s.Equals("Registration succesfull!"))
            {
                usererror.Text = s;
            }
            else {
                usererror.Text = s;
                this.Frame.GoBack();
            }
        }
    }
}
