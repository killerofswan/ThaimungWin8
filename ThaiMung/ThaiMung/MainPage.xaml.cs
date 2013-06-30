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
using Windows.UI.Popups;
using Windows.UI;

using Windows.Devices.Geolocation;
using System.Threading;
using System.Threading.Tasks;
using Bing.Maps;
using Newtonsoft.Json.Linq;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ThaiMung
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Geoposition pos;
        User nowLogin;
        bool allcheck = false;

        String within;
        String resource;
        int solvecheck = 2;

        bool[] checkbox = new bool[12];
        int radiobox = 2;
        int withinbox = 1;
        int checkboxcheck = 0;
        List<Post> post = new List<Post>();

        // member variables to add
        private Geolocator _geolocator = null;
        private CancellationTokenSource _cts = null;
        LocationIcon10m _locationIcon10m;
        LocationIcon100m _locationIcon100m;

        MessageDialog msgDialog = new MessageDialog("Please select at least one tag.");

        ListBoxItem[] dayItem = new ListBoxItem[31];

        //POST
        bool[] postcheckbox = new bool[12];
        bool postallcheck = false;

        public MainPage()
        {
            this.InitializeComponent();
            //ggg
            _geolocator = new Geolocator();
            _locationIcon10m = new LocationIcon10m();
            _locationIcon100m = new LocationIcon100m();
            //end
            NavigationCacheMode = NavigationCacheMode.Enabled;
            filterCanvas.Visibility = Visibility.Collapsed;
            emergencyCanvas.Visibility = Visibility.Collapsed;
            bgpostCanvas.Visibility = Visibility.Collapsed;
            postCanvas.Visibility = Visibility.Collapsed;
            for (int i = 0; i < 12; i++)
            {
                checkbox[i] = true;
                postcheckbox[i] = false;
            }
            checkallcheckbox();
            getCurrentLocation();

            //Post construct
            for (int i = 0; i < 31; i++)
            {  
                dayItem[i] = new ListBoxItem();
                dayItem[i].Content = i+1;
                dayCombo.Items.Add(dayItem[i]);
            }
            DateTime curTime = DateTime.Now;
            switch (curTime.Month)
            {
                case 0:
                case 2:
                case 4:
                case 6:
                case 7:
                case 9:
                case 11: dayItem[28].Visibility = Visibility.Visible;
                    dayItem[29].Visibility = Visibility.Visible;
                    dayItem[30].Visibility = Visibility.Visible;
                    break;
                case 1: dayItem[28].Visibility = Visibility.Collapsed;
                    dayItem[29].Visibility = Visibility.Collapsed;
                    dayItem[30].Visibility = Visibility.Collapsed;
                    break;
                default: dayItem[28].Visibility = Visibility.Visible;
                    dayItem[29].Visibility = Visibility.Visible;
                    dayItem[30].Visibility = Visibility.Collapsed;                    
                    break;
            }

            //Post initial


        }
        public void checkallcheckbox()
        {
            checkboxAll.IsChecked = true;
            checkboxTraffic.IsChecked = true;
            checkboxAccident.IsChecked = true;
            checkboxCrime.IsChecked = true;
            checkboxFire.IsChecked = true;
            checkboxProtesting.IsChecked = true;
            checkboxBlackOut.IsChecked = true;
            checkboxFlood.IsChecked = true;
            checkboxEarthquake.IsChecked = true;
            checkboxConstructing.IsChecked = true;
            checkboxTerrorism.IsChecked = true;
            checkboxOther.IsChecked = true;
            allcheck = true;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            nowLogin = e.Parameter as User;
            Mainusername.Text = nowLogin.name;

            //MapLocationButton.IsEnabled = true;
            CancelGetLocationButton.IsEnabled = false;

        }

        private void filter_Click(object sender, RoutedEventArgs e)
        {
            filterCanvas.Visibility = Visibility.Visible;
            mainAppbar.IsOpen = false;
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void emergency_Click(object sender, RoutedEventArgs e)
        {
            emergencyCanvas.Visibility = Visibility.Visible;
            mainAppbar.IsOpen = false;
        }

        private void bgCanvas_Tap(object sender, TappedRoutedEventArgs e)
        {
            if (emergencyCanvas.Visibility == Visibility.Visible)
            {
                emergencyCanvas.Visibility = Visibility.Collapsed;
            }

            if (filterCanvas.Visibility == Visibility.Visible)
            {
                filterCancel();
                filterCanvas.Visibility = Visibility.Collapsed;
            }
        }

        private void bgCanvas_RTap(object sender, RightTappedRoutedEventArgs e)
        {
            if (emergencyCanvas.Visibility == Visibility.Visible)
            {
                emergencyCanvas.Visibility = Visibility.Collapsed;
            }

            if (filterCanvas.Visibility == Visibility.Visible)
            {
                filterCanvas.Visibility = Visibility.Collapsed;
            }
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //combobox
            if (comboBox.SelectedIndex == 0)
                within = "1 HOUR";
            else if (comboBox.SelectedIndex == 1)
                within = "3 HOUR";
            else if (comboBox.SelectedIndex == 2)
                within = "6 HOUR";
            else if (comboBox.SelectedIndex == 3)
                within = "12 HOUR";
            else if (comboBox.SelectedIndex == 4)
                within = "1 DAY";
            else if (comboBox.SelectedIndex == 5)
                within = "3 DAY";
            else if (comboBox.SelectedIndex == 6)
                within = "7 DAY";
            withinbox = comboBox.SelectedIndex;

            //radio button
            if (radioAll.IsChecked == true)
            {
                solvecheck = 2;
                radiobox = 2;
            }
            else if (radioSolved.IsChecked == true)
            {
                solvecheck = 1;
                radiobox = 1;
            }
            else
            {
                solvecheck = 0;
                radiobox = 0;
            }

            //checkbox
            if (checkboxTraffic.IsChecked == true)
            {
                resource += "1,";
                checkbox[1] = true;
            }
            else
            {
                checkbox[1] = false;
            }
            if (checkboxAccident.IsChecked == true)
            {
                resource += "2,";
                checkbox[2] = true;
            }
            else
            {
                checkbox[2] = false;
            }
            if (checkboxCrime.IsChecked == true)
            {
                resource += "3,";
                checkbox[3] = true;
            }
            else
            {
                checkbox[3] = false;
            }
            if (checkboxFire.IsChecked == true)
            {
                resource += "4,";
                checkbox[4] = true;
            }
            else
            {
                checkbox[4] = false;
            }
            if (checkboxProtesting.IsChecked == true)
            {
                resource += "5,";
                checkbox[5] = true;
            }
            else
            {
                checkbox[5] = false;
            }
            if (checkboxBlackOut.IsChecked == true)
            {
                resource += "6,";
                checkbox[6] = true;
            }
            else
            {
                checkbox[6] = false;
            }
            if (checkboxFlood.IsChecked == true)
            {
                resource += "7,";
                checkbox[7] = true;
            }
            else
            {
                checkbox[7] = false;
            }
            if (checkboxEarthquake.IsChecked == true)
            {
                resource += "8,";
                checkbox[8] = true;
            }
            else
            {
                checkbox[8] = false;
            }
            if (checkboxConstructing.IsChecked == true)
            {
                resource += "9,";
                checkbox[9] = true;
            }
            else
            {
                checkbox[9] = false;
            }
            if (checkboxTerrorism.IsChecked == true)
            {
                resource += "10,";
                checkbox[10] = true;
            }
            else
            {
                checkbox[10] = false;
            }
            if (checkboxOther.IsChecked == true)
            {
                resource += "11,";
                checkbox[11] = true;
            }
            else
            {
                checkbox[11] = false;
            }
            if (checkboxAll.IsChecked == true)
            {
                checkbox[0] = true;
            }
            else
            {
                checkbox[0] = false;
            }
            try
            {
                resource = resource.Substring(0, resource.Length - 1);
                filterCanvas.Visibility = Visibility.Collapsed;


                HttpClient client = new HttpClient();
                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("id", nowLogin.id.ToString()));
                postData.Add(new KeyValuePair<string, string>("time", within));
                postData.Add(new KeyValuePair<string, string>("status", solvecheck.ToString()));
                postData.Add(new KeyValuePair<string, string>("tag_id", resource));
                HttpContent c = new FormUrlEncodedContent(postData);

                // String within;
                //String resource;
                // int solvecheck = 2;



                var response = await client.PostAsync("http://localhost/filters.php", c);
            }
            catch (NullReferenceException)
            {
                msgDialog.ShowAsync();
            }
            

           
        }

        private void checkboxAll_Click(object sender, RoutedEventArgs e)
        {
            if (!allcheck)
            {

                checkboxTraffic.IsChecked = true;
                checkboxAccident.IsChecked = true;
                checkboxCrime.IsChecked = true;
                checkboxFire.IsChecked = true;
                checkboxProtesting.IsChecked = true;
                checkboxBlackOut.IsChecked = true;
                checkboxFlood.IsChecked = true;
                checkboxEarthquake.IsChecked = true;
                checkboxConstructing.IsChecked = true;
                checkboxTerrorism.IsChecked = true;
                checkboxOther.IsChecked = true;
                allcheck = true;
            }
            else
            {
                checkboxTraffic.IsChecked = false;
                checkboxAccident.IsChecked = false;
                checkboxCrime.IsChecked = false;
                checkboxFire.IsChecked = false;
                checkboxProtesting.IsChecked = false;
                checkboxBlackOut.IsChecked = false;
                checkboxFlood.IsChecked = false;
                checkboxEarthquake.IsChecked = false;
                checkboxConstructing.IsChecked = false;
                checkboxTerrorism.IsChecked = false;
                checkboxOther.IsChecked = false;
                allcheck = false;
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            filterCancel();
        }
        private void filterCancel()
        {
            comboBox.SelectedIndex = withinbox;

            if (radiobox == 0)
            {
                radioUnsolved.IsChecked = true;
                radioSolved.IsChecked = false;
                radioAll.IsChecked = false;
            }
            else if (radiobox == 1)
            {
                radioUnsolved.IsChecked = false;
                radioSolved.IsChecked = true;
                radioAll.IsChecked = false;
            }
            else if (radiobox == 2)
            {
                radioUnsolved.IsChecked = false;
                radioSolved.IsChecked = false;
                radioAll.IsChecked = true;
            }

            checkboxAll.IsChecked = checkbox[0];
            checkboxTraffic.IsChecked = checkbox[1];
            checkboxAccident.IsChecked = checkbox[2];
            checkboxCrime.IsChecked = checkbox[3];
            checkboxFire.IsChecked = checkbox[4];
            checkboxProtesting.IsChecked = checkbox[5];
            checkboxBlackOut.IsChecked = checkbox[6];
            checkboxFlood.IsChecked = checkbox[7];
            checkboxEarthquake.IsChecked = checkbox[8];
            checkboxConstructing.IsChecked = checkbox[9];
            checkboxTerrorism.IsChecked = checkbox[10];
            checkboxOther.IsChecked = checkbox[11];

            filterCanvas.Visibility = Visibility.Collapsed;
        }

        

        private void cancelpost_click(object sender, RoutedEventArgs e)
        {
            postCanvas.Visibility = Visibility.Collapsed;
            bgpostCanvas.Visibility = Visibility.Collapsed;
        }

        private void post_Click(object sender, RoutedEventArgs e)
        {
            mainAppbar.IsOpen = false;
            bgpostCanvas.Visibility = Visibility.Visible;
            postCanvas.Visibility = Visibility.Visible;
            postbynameText.Text = nowLogin.name +" "+ nowLogin.lastname;
            string curtime = string.Format("{0:yyyy-MM-dd hh:mm:ss}", DateTime.Now);
            //postdatetimeText.Text = curtime;
        }

        private void filterCanvas_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (checkboxTraffic.IsChecked == true)
            {
                resource += "1,";
                checkbox[1] = true;
            }
            else
            {
                checkbox[1] = false;
                checkbox[0] = false;
                checkboxAll.IsChecked = false;
            }
            if (checkboxAccident.IsChecked == true)
            {
                resource += "2,";
                checkbox[2] = true;
            }
            else
            {
                checkbox[2] = false;
                checkbox[0] = false;
                checkboxAll.IsChecked = false;
            }
            if (checkboxCrime.IsChecked == true)
            {
                resource += "3,";
                checkbox[3] = true;
            }
            else
            {
                checkbox[3] = false;
                checkbox[0] = false;
                checkboxAll.IsChecked = false;
            }
            if (checkboxFire.IsChecked == true)
            {
                resource += "4,";
                checkbox[4] = true;
            }
            else
            {
                checkbox[4] = false;
                checkbox[0] = false;
                checkboxAll.IsChecked = false;
            }
            if (checkboxProtesting.IsChecked == true)
            {
                resource += "5,";
                checkbox[5] = true;
            }
            else
            {
                checkbox[5] = false;
                checkbox[0] = false;
                checkboxAll.IsChecked = false;
            }
            if (checkboxBlackOut.IsChecked == true)
            {
                resource += "6,";
                checkbox[6] = true;
            }
            else
            {
                checkbox[6] = false;
                checkbox[0] = false;
                checkboxAll.IsChecked = false;
            }
            if (checkboxFlood.IsChecked == true)
            {
                resource += "7,";
                checkbox[7] = true;
            }
            else
            {
                checkbox[7] = false;
                checkbox[0] = false;
                checkboxAll.IsChecked = false;
            }
            if (checkboxEarthquake.IsChecked == true)
            {
                resource += "8,";
                checkbox[8] = true;
            }
            else
            {
                checkbox[8] = false;
                checkbox[0] = false;
                checkboxAll.IsChecked = false;
            }
            if (checkboxConstructing.IsChecked == true)
            {
                resource += "9,";
                checkbox[9] = true;
            }
            else
            {
                checkbox[9] = false;
                checkbox[0] = false;
                checkboxAll.IsChecked = false;
            }
            if (checkboxTerrorism.IsChecked == true)
            {
                resource += "10,";
                checkbox[10] = true;
            }
            else
            {
                checkbox[10] = false;
                checkbox[0] = false;
                checkboxAll.IsChecked = false;
            }
            if (checkboxOther.IsChecked == true)
            {
                resource += "11,";
                checkbox[11] = true;
            }
            else
            {
                checkbox[11] = false;
                checkbox[0] = false;
                checkboxAll.IsChecked = false;
            }
            if (checkboxAll.IsChecked == true)
            {
                checkbox[0] = true;
            }
            else
            {
                checkbox[0] = false;
                checkbox[0] = false;
                checkboxAll.IsChecked = false;
            }
        }

        public async void getCurrentLocation()
        {
            // Change the state of our buttons.
            //MapLocationButton.IsEnabled = false;
            CancelGetLocationButton.IsEnabled = true;

            // Remove any previous location icon.
            if (Map.Children.Count > 0)
            {
                Map.Children.RemoveAt(0);
            }

            try
            {
                // Get the cancellation token.
                _cts = new CancellationTokenSource();
                CancellationToken token = _cts.Token;

                MessageTextbox.Text = "Waiting for update...";

                // Get the location.
                pos = await _geolocator.GetGeopositionAsync().AsTask(token);

                MessageTextbox.Text = "";

                Location location = new Location(pos.Coordinate.Latitude, pos.Coordinate.Longitude);

                // Now set the zoom level of the map based on the accuracy of our location data.
                // Default to IP level accuracy. We only show the region at this level - No icon is displayed.
                double zoomLevel = 13.0f;

                // if we have GPS level accuracy
                if (pos.Coordinate.Accuracy <= 10)
                {
                    // Add the 10m icon and zoom closer.
                    Map.Children.Add(_locationIcon10m);
                    MapLayer.SetPosition(_locationIcon10m, location);
                    zoomLevel = 15.0f;
                }
                // Else if we have Wi-Fi level accuracy.
                else if (pos.Coordinate.Accuracy <= 100)
                {
                    // Add the 100m icon and zoom a little closer.
                    Map.Children.Add(_locationIcon100m);
                    MapLayer.SetPosition(_locationIcon100m, location);
                    zoomLevel = 14.0f;
                }

                // Set the map to the given location and zoom level.
                Map.SetView(location, zoomLevel);

                // Display the location information in the textboxes.
                LatitudeTextbox.Text = pos.Coordinate.Latitude.ToString();
                LongitudeTextbox.Text = pos.Coordinate.Longitude.ToString();
                AccuracyTextbox.Text = pos.Coordinate.Accuracy.ToString();
            }
            catch (System.UnauthorizedAccessException)
            {
                MessageTextbox.Text = "Location disabled.";

                LatitudeTextbox.Text = "No data";
                LongitudeTextbox.Text = "No data";
                AccuracyTextbox.Text = "No data";
            }
            catch (TaskCanceledException)
            {
                MessageTextbox.Text = "Operation canceled.";
            }
            finally
            {
                _cts = null;
            }

            /*Pushpin pushpin = new Pushpin();
            pushpin.Text = "1";
            MapLayer.SetPosition(pushpin, new Location(14.27570, 101.2890));
            pushpin.Tapped += new TappedEventHandler(pushpinTapped);
            Map.Children.Add(pushpin);

            Pushpin pushpin2 = new Pushpin();
            pushpin2.Text = "2";
            MapLayer.SetPosition(pushpin2, new Location(14.27590, 101.29100));
            Map.Children.Add(pushpin2);*/




            // Reset the buttons.
            //MapLocationButton.IsEnabled = true;
            CancelGetLocationButton.IsEnabled = false;
            getPost();
            
        }

        public async void getPost() {
            
            string latitude = pos.Coordinate.Latitude.ToString();
            string longtitude = pos.Coordinate.Longitude.ToString();

            HttpClient client = new HttpClient();
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("latitude", latitude));
            postData.Add(new KeyValuePair<string, string>("longtitude", longtitude));
            postData.Add(new KeyValuePair<string, string>("uid", nowLogin.id.ToString()));

            HttpContent c = new FormUrlEncodedContent(postData);
            var response = await client.PostAsync("http://localhost/view.php", c);

            var str = await response.Content.ReadAsStringAsync();
            string s = str.ToString();
            
            Pushpin pushpin2 = new Pushpin();
            pushpin2.Text = "2";
            MapLayer.SetPosition(pushpin2, new Location(14.27590, 101.29100));
            Map.Children.Add(pushpin2);


            if (s.Equals("Not Found"))
            {

            }
            else
            {
                var o = JArray.Parse(s);
                int i = 0;
                while (i < o.Count)
                {
                    Post tmp = new Post();
                    tmp.p_id = (int)o[i]["p_id"];
                    tmp.latitude = (double)o[i]["latitude"];
                    tmp.longtitude = (double)o[i]["longtitude"];
                    tmp.dateTime = (string)o[i]["date_time"];
                    tmp.id = (int)o[i]["id"];
                    tmp.description = (string)o[i]["description"];
                    tmp.status = (int)o[i]["status"];
                    tmp.countSolve = (int)o[i]["count_solved"];
                    tmp.countSeen = (int)o[i]["count_seen"];
                    tmp.countSpam = (int)o[i]["count_spam"];
                    tmp.nameTag.Add((string)o[i]["nametag"]);
                    i++;
                    if (tmp.id != (int)o[i]["p_id"])
                    {
                        post.Add(tmp);
                        continue;
                    }
                    else
                    {
                        while (tmp.id == (int)o[i]["p_id"])
                        {
                            tmp.nameTag.Add((string)o[i]["nametag"]);
                            i++;
                        }
                        post.Add(tmp);

                    }
                }

                
                

        



            }
                

        }

        private void CancelGetLocationButton_Click(object sender, RoutedEventArgs e)
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
            }

            //MapLocationButton.IsEnabled = true;
            CancelGetLocationButton.IsEnabled = false;
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
            }

            base.OnNavigatingFrom(e);

        }
        private async void pushpinTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            MessageDialog dialog = new MessageDialog("Hello from Seattle.");
            await dialog.ShowAsync();
        }

        private void monthCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (monthCombo.SelectedIndex)
            {
                case 0:
                case 2:
                case 4:
                case 6:
                case 7:
                case 9:
                case 11: dayItem[28].Visibility = Visibility.Visible;
                    dayItem[29].Visibility = Visibility.Visible;
                    dayItem[30].Visibility = Visibility.Visible;
                    break;
                case 1: dayItem[28].Visibility = Visibility.Collapsed;
                    dayItem[29].Visibility = Visibility.Collapsed;
                    dayItem[30].Visibility = Visibility.Collapsed;
                    break;
                default: dayItem[28].Visibility = Visibility.Visible;
                    dayItem[29].Visibility = Visibility.Visible;
                    dayItem[30].Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void postCanvas_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (checkboxPostTraffic.IsChecked == true)
            {
                resource += "1,";
                postcheckbox[1] = true;
            }
            else
            {
                postcheckbox[1] = false;
                postcheckbox[0] = false;
                checkPostAll.IsChecked = false;
            }
            if (checkboxPostAccident.IsChecked == true)
            {
                resource += "2,";
                postcheckbox[2] = true;
            }
            else
            {
                postcheckbox[2] = false;
                postcheckbox[0] = false;
                checkPostAll.IsChecked = false;
            }
            if (checkboxPostCrime.IsChecked == true)
            {
                resource += "3,";
                postcheckbox[3] = true;
            }
            else
            {
                postcheckbox[3] = false;
                postcheckbox[0] = false;
                checkPostAll.IsChecked = false;
            }
            if (checkboxPostFire.IsChecked == true)
            {
                resource += "4,";
                postcheckbox[4] = true;
            }
            else
            {
                postcheckbox[4] = false;
                postcheckbox[0] = false;
                checkPostAll.IsChecked = false;
            }
            if (checkboxPostProtesting.IsChecked == true)
            {
                resource += "5,";
                postcheckbox[5] = true;
            }
            else
            {
                postcheckbox[5] = false;
                postcheckbox[0] = false;
                checkPostAll.IsChecked = false;
            }
            if (checkboxPostBlackOut.IsChecked == true)
            {
                resource += "6,";
                postcheckbox[6] = true;
            }
            else
            {
                postcheckbox[6] = false;
                postcheckbox[0] = false;
                checkPostAll.IsChecked = false;
            }
            if (checkboxPostFlood.IsChecked == true)
            {
                resource += "7,";
                postcheckbox[7] = true;
            }
            else
            {
                postcheckbox[7] = false;
                postcheckbox[0] = false;
                checkPostAll.IsChecked = false;
            }
            if (checkboxPostEarthquake.IsChecked == true)
            {
                resource += "8,";
                postcheckbox[8] = true;
            }
            else
            {
                postcheckbox[8] = false;
                postcheckbox[0] = false;
                checkPostAll.IsChecked = false;
            }
            if (checkboxPostConstructing.IsChecked == true)
            {
                resource += "9,";
                postcheckbox[9] = true;
            }
            else
            {
                postcheckbox[9] = false;
                postcheckbox[0] = false;
                checkPostAll.IsChecked = false;
            }
            if (checkboxPostTerrorism.IsChecked == true)
            {
                resource += "10,";
                postcheckbox[10] = true;
            }
            else
            {
                postcheckbox[10] = false;
                postcheckbox[0] = false;
                checkPostAll.IsChecked = false;
            }
            if (checkboxPostOther.IsChecked == true)
            {
                resource += "11,";
                postcheckbox[11] = true;
            }
            else
            {
                postcheckbox[11] = false;
                postcheckbox[0] = false;
                checkPostAll.IsChecked = false;
            }
            if (checkPostAll.IsChecked == true)
            {
                postcheckbox[0] = true;
            }
            else
            {
                postcheckbox[0] = false;
                checkPostAll.IsChecked = false;
            }
            postallcheck = postcheckbox[0];
            
        }

        private void checkPostAll_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!postallcheck)
            {
                checkboxPostTraffic.IsChecked = true;
                checkboxPostAccident.IsChecked = true;
                checkboxPostCrime.IsChecked = true;
                checkboxPostFire.IsChecked = true;
                checkboxPostProtesting.IsChecked = true;
                checkboxPostBlackOut.IsChecked = true;
                checkboxPostFlood.IsChecked = true;
                checkboxPostEarthquake.IsChecked = true;
                checkboxPostConstructing.IsChecked = true;
                checkboxPostTerrorism.IsChecked = true;
                checkboxPostOther.IsChecked = true;
                postallcheck = true;
            }
            else
            {
                checkboxPostTraffic.IsChecked = false;
                checkboxPostAccident.IsChecked = false;
                checkboxPostCrime.IsChecked = false;
                checkboxPostFire.IsChecked = false;
                checkboxPostProtesting.IsChecked = false;
                checkboxPostBlackOut.IsChecked = false;
                checkboxPostFlood.IsChecked = false;
                checkboxPostEarthquake.IsChecked = false;
                checkboxPostConstructing.IsChecked = false;
                checkboxPostTerrorism.IsChecked = false;
                checkboxPostOther.IsChecked = false;
                postallcheck = false;
            }
        }
        
    }
}

