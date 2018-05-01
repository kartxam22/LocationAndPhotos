using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace LocationandPhotos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Searchresults : ContentPage
    {
        bool tap = true;
        Weather weather = null; List<Uri> images = null; loc location = null;
        public Searchresults(Weather w, List<Uri> img, loc l)
        {
            InitializeComponent();
            weather = w;
            images = img;
            location = l;
            NavigationPage.SetHasNavigationBar(this, false);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (weather != null)
            {
                mapcontrol.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.lat, location.lng), Distance.FromMiles(2)));
                mapcontrol.IsShowingUser = false;
                var high = Helpers.ConvertFahrenheitToCelsius(weather.query.results.channel.item.forecast[0].high);
                var low = Helpers.ConvertFahrenheitToCelsius(weather.query.results.channel.item.forecast[0].low);
                var date = weather.query.results.channel.item.forecast[0].date;
                var text = weather.query.results.channel.item.forecast[0].text;

                mapcontrol.Pins.Add(new Pin() { Label = "H : " + high + "C, L : " + low + "C, " + text, Position = new Position(location.lat, location.lng) });
            }
            else {
                maingrid.RowDefinitions[1].Height = 0;
            }
            if (images != null)
            {
                for (int i = 0; i < images.Count; i++)
                {
                    Image img = new Image() { Aspect = Aspect.Fill };
                    Image imgfav = new Image() { Source = "unfav.png", BindingContext = images[i], HeightRequest = 30, WidthRequest = 30, VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.End, BackgroundColor = Color.White };
                    var imgfavGR = new TapGestureRecognizer();
                    imgfavGR.Tapped += (s, e) =>
                   {
                       Image im = s as Image;

                       tap = false;
                       var str = imgfav.Source.ToString();
                       if (str.Contains("Sfav.png"))
                       {
                           imgfav.BackgroundColor = Color.White;
                           imgfav.Source = "unfav.png";
                           App.Database.DeleteAppImageData(im.BindingContext.ToString());

                       }
                       else
                       {
                           imgfav.BackgroundColor = Color.Transparent;
                           imgfav.Source = "Sfav.png";
                           var id = App.Database.GetAllImages();
                           var maxid = 1;
                           if (id.Any())
                           {
                               maxid = id.Max(x => x.id);
                           }
                           App.Database.AppImageUpdate(new ImageList() { id = maxid, image = im.BindingContext.ToString() });
                       }
                       tap = true;
                   };
                    imgfav.GestureRecognizers.Add(imgfavGR);
                    img.Source = ImageSource.FromUri(images[i]);
                    imagesgrid.Children.Add(img, i % 2, i / 2);
                    imagesgrid.Children.Add(imgfav, i % 2, i / 2);
                }
            }
            else
            {
                maingrid.RowDefinitions[2].Height = 0;
            }
        }
        private void btnBackClicked(object sender, EventArgs e)
        {
            if (tap)
            {
                tap = false;
                Navigation.InsertPageBefore(new MainPage(), this);
                Navigation.PopAsync(false);
                tap = true;
            }
        }

        private void btnmapfavClicked(object sender, EventArgs e)
        {
            if (tap)
            {
                tap = false;
                var str = mapfav.Source.ToString();
                if (str.Contains("Sfav.png"))
                {
                    mapfav.BackgroundColor = Color.White;
                    mapfav.Source = "unfav.png";
                    App.Database.DeleteAppData();
                }
                else
                {
                    mapfav.BackgroundColor = Color.Transparent;
                    mapfav.Source = "Sfav.png";
                    App.Database.AppUpdate(location);
                }
                tap = true;
            }
        }

    }
}