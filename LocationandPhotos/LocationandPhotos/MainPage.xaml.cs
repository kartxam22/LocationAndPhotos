
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace LocationandPhotos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        bool tap = true;
        public static MainPage current;
        public MainPage()
        {
            InitializeComponent();
            current = this;
            NavigationPage.SetHasNavigationBar(this, false);
        }
        protected async override void OnAppearing()
        {

            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);


                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                //Best practice to always check that the key exists
                if (results.ContainsKey(Permission.Location))
                    status = results[Permission.Location];

                if (status == PermissionStatus.Granted)
                {
                    var p = await Helpers.GetCurrentPosition();
                    if (p != null)
                    {
                        mapcontrol.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(p.Latitude, p.Longitude), Distance.FromMiles(2)));
                        mapcontrol.IsShowingUser = false;
                        mapcontrol.Pins.Add(new Pin() { Label = "", Position = new Position(p.Latitude, p.Longitude) });
                    }
                }

            }
            catch (Exception ex)
            {


            }

        }
        public static void LoadMap()
        {
            current.OnAppearing();
        }
        async void btnsearch_clicked(object sender, EventArgs e)
        {
            if (tap)
            {
                tap = false;
                await PopupNavigation.Instance.PushAsync(new LoadingPopup());

                Weather zip = null; loc l = null;
                if (!string.IsNullOrEmpty(entryzipcode.Text) && !string.IsNullOrWhiteSpace(entryzipcode.Text))
                {
                    var geo = await CrossGeolocator.Current.GetPositionsForAddressAsync(entryzipcode.Text);

                    if (geo.Any())
                    {
                        var p = geo.AsEnumerable().ToList();
                        var address = await CrossGeolocator.Current.GetAddressesForPositionAsync(p[0]);
                        if (address.Any())
                        {
                            l = new loc();
                            var a = address.AsEnumerable().ToList();
                            l.country = a[0].CountryName;
                            l.city = a[0].Locality;
                            l.zipcode = entryzipcode.Text;
                            l.lat = p[0].Latitude;
                            l.lng = p[0].Longitude;
                        }
                    }
                    if (l != null)
                    {
                        zip = PlacesSearch(entryzipcode.Text, l);
                    }
                }
                List<Uri> images = null;
                if (!string.IsNullOrEmpty(entrykeyword.Text) && !string.IsNullOrWhiteSpace(entrykeyword.Text))
                {
                    images = ImagesSearch(entrykeyword.Text);
                }
                await PopupNavigation.Instance.PopAsync();
                if (zip == null && images == null)
                {
                    await DisplayAlert("", "No Data", "Ok");
                }
                else if (images != null && !images.Any())
                { await DisplayAlert("", "No Data", "Ok"); }
                else
                {
                    Navigation.InsertPageBefore(new Searchresults(zip, images, l), this);
                    await Navigation.PopAsync(false);
                }
                tap = true;
            }

        }

        public Weather PlacesSearch(string zipcode, loc l)
        {
            string addr = l.city + "%2C" + l.country;
            addr = addr.Replace(" ", "%20");
            Weather items = null;
            try
            {
                HttpClient client = new HttpClient();
                client.MaxResponseContentBufferSize = 956000;
                var uri = new Uri("https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20weather.forecast%20where%20woeid%20in%20(select%20woeid%20from%20geo.places(1)%20where%20text%3D%22" + addr + "%22)&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys");
                var response = client.GetAsync(uri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    items = JsonConvert.DeserializeObject<Weather>(content);
                }
            }
            catch (Exception ex)
            {

            }
            return items;
        }

        public List<Uri> ImagesSearch(string location, string apikey = "1cdc7039d13d375d3e224e2398ac4bb6")
        {
            Pics items = null;
            List<Uri> images = new List<Uri>();
            try
            {
                HttpClient client = new HttpClient();
                client.MaxResponseContentBufferSize = 956000;
                var uri = new Uri("https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20flickr.photos.search%20where%20has_geo%3D%22true%22%20and%20text%3D%22" + location + "%22%20and%20api_key%3D%22" + apikey + "%22%3B&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys");
                var response = client.GetAsync(uri).Result;
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var content = response.Content.ReadAsStringAsync().Result;
                        items = JsonConvert.DeserializeObject<Pics>(content);
                        if (items != null && items.query != null && items.query.results != null && items.query.results.photo != null)
                        {
                            var photoidsarray = items.query.results.photo.Select(x => "%22" + x.id + "%22").ToArray();
                            if (photoidsarray.Any())
                            {
                                string photoids = string.Join("%2C", photoidsarray);
                                var photosurl = new Uri("https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20flickr.photos.info%20where%20photo_id%20in%20(" + photoids + ")%20and%20api_key%3D%22" + apikey + "%22%3B&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys");
                                var responsephotodetail = client.GetAsync(photosurl).Result;
                                if (responsephotodetail.IsSuccessStatusCode)
                                {
                                    var photodetailcontent = responsephotodetail.Content.ReadAsStringAsync().Result;
                                    items = JsonConvert.DeserializeObject<Pics>(photodetailcontent);
                                    if (items != null && items.query != null && items.query.results != null && items.query.results.photo != null)
                                    {
                                        items.query.results.photo.ForEach(x =>
                                        {
                                            try
                                            {
                                                //https://c1.staticflickr.com/{farm}/{server}/{id}_{secret}.{originalformat}

                                                images.Add(new Uri("https://c1.staticflickr.com/" + x.farm + "/" + x.server + "/" + x.id + "_" + x.secret + "." + x.originalformat));
                                            }
                                            catch { }
                                        });
                                    }
                                }
                            }

                        }
                    }
                    catch { }
                }
            }
            catch
            {

            }
            return images;
        }


    }
}
