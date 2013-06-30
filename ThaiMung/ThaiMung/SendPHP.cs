using System;
using System.Net;
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
using System.Text;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using Newtonsoft.Json.Linq;

namespace ThaiMung
{
    class SendPHP
    {
        public async void login(string email , string pass) {
            HttpClient client = new HttpClient();
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string,string>("email",email));
            postData.Add(new KeyValuePair<string,string>("password",pass));

            HttpContent c = new FormUrlEncodedContent(postData);
            var response = await client.PostAsync("http://localhost/login.php",c);
            var str = await response.Content.ReadAsStringAsync();
            string s = str.ToString();
            
        }

        

    }
}
