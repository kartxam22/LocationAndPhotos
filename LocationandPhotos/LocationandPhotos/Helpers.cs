using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LocationandPhotos
{
    public static class Helpers
    {
        public static async Task<Plugin.Geolocator.Abstractions.Position> GetCurrentPosition()
        {
            Plugin.Geolocator.Abstractions.Position position = null;
            try
            {
                var locator = Plugin.Geolocator.CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                position = await locator.GetLastKnownLocationAsync();

                if (position != null)
                {
                    //got a cahched position, so let's use it.
                    return position;
                }

                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                {
                    //not available or enabled
                    return null;
                }

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);

            }
            catch (Exception ex)
            {
                // Debug.WriteLine("Unable to get location: " + ex);
            }

            if (position == null)
                return null;

            //var output = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
            //        position.Timestamp, position.Latitude, position.Longitude,
            //        position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

            // Debug.WriteLine(output);

            return position;
        }

        public static double ConvertCelsiusToFahrenheit(double c)
        {
            return ((9.0 / 5.0) * c) + 32;
        }

        public static string ConvertFahrenheitToCelsius(string f)
        {
            try
            {
                double val = Convert.ToDouble(f);
                return ((5.0 / 9.0) * (val - 32)).ToString("f2");
            }
            catch
            {
                return "";
            }
        }

    }

    public class loc
    {
        public string zipcode { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class ImageList
    {
        [PrimaryKey]
        [AutoIncrement]
        public int id { get; set; }
        public string image { get; set; }
    }

    #region photos

    public class Pics
    {
        public Query query { get; set; }
    }

    public class Query
    {
        public int count { get; set; }
        public string created { get; set; }
        public string lang { get; set; }
        public Results results { get; set; }
    }
    public class Results
    {
        public Channel channel { get; set; }
        public List<Photo> photo { get; set; }
    }
    public class Channel
    {
        public Item item { get; set; }
    }
    public class Item
    {
        public Condition condition { get; set; }
    }
    public class Condition
    {
        public string code { get; set; }
        public string date { get; set; }
        public string temp { get; set; }
        public string text { get; set; }
    }

    public class Photo
    {
        public string dateuploaded { get; set; }
        public string farm { get; set; }
        public string id { get; set; }
        public string isfamily { get; set; }
        public string isfriend { get; set; }
        public string ispublic { get; set; }

        public string secret { get; set; }
        public string server { get; set; }
        public string title { get; set; }
        public string isfavorite { get; set; }
        public string license { get; set; }
        public string media { get; set; }


        public string originalformat { get; set; }
        public string originalsecret { get; set; }
        public string rotation { get; set; }
        public string safety_level { get; set; }

        public string views { get; set; }
        //public string owner { get; set; }

        public string description { get; set; }
        public Visibility visibility { get; set; }
        public Dates dates { get; set; }
        public Editability editability { get; set; }
        public Publiceditability publiceditability { get; set; }
        public Usage usage { get; set; }
        public string comments { get; set; }
        public object notes { get; set; }
        public People people { get; set; }
        public Tags tags { get; set; }
        public Urls urls { get; set; }
        public Location location { get; set; }
        public Geoperms geoperms { get; set; }

    }

    public class Owner
    {
        public string iconfarm { get; set; }
        public string iconserver { get; set; }
        public string location { get; set; }
        public string nsid { get; set; }
        public string path_alias { get; set; }
        public string realname { get; set; }
        public string username { get; set; }
    }

    public class Visibility
    {
        public string isfamily { get; set; }
        public string isfriend { get; set; }
        public string ispublic { get; set; }
    }

    public class Dates
    {
        public string lastupdate { get; set; }
        public string posted { get; set; }
        public string taken { get; set; }
        public string takengranularity { get; set; }
        public string takenunknown { get; set; }
    }

    public class Editability
    {
        public string canaddmeta { get; set; }
        public string cancomment { get; set; }
    }

    public class Publiceditability
    {
        public string canaddmeta { get; set; }
        public string cancomment { get; set; }
    }

    public class Usage
    {
        public string canblog { get; set; }
        public string candownload { get; set; }
        public string canprint { get; set; }
        public string canshare { get; set; }
    }

    public class People
    {
        public string haspeople { get; set; }
    }

    public class Tags
    {
        public Tag[] tag { get; set; }
    }

    public class Tag
    {
        public string author { get; set; }
        public string authorname { get; set; }
        public string id { get; set; }
        public string machine_tag { get; set; }
        public string raw { get; set; }
        public string content { get; set; }
    }

    public class Urls
    {
        public Url url { get; set; }
    }

    public class Url
    {
        public string type { get; set; }
        public string content { get; set; }
    }

    public class Location
    {
        public string accuracy { get; set; }
        public string context { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string place_id { get; set; }
        public string woeid { get; set; }
        public Neighbourhood neighbourhood { get; set; }
        public Locality locality { get; set; }
        public County county { get; set; }
        public Region region { get; set; }
        public Country country { get; set; }
    }

    public class Neighbourhood
    {
        public string place_id { get; set; }
        public string woeid { get; set; }
        public string content { get; set; }
    }

    public class Locality
    {
        public string place_id { get; set; }
        public string woeid { get; set; }
        public string content { get; set; }
    }

    public class County
    {
        public string place_id { get; set; }
        public string woeid { get; set; }
        public string content { get; set; }
    }

    public class Region
    {
        public string place_id { get; set; }
        public string woeid { get; set; }
        public string content { get; set; }
    }

    public class Country
    {
        public string place_id { get; set; }
        public string woeid { get; set; }
        public string content { get; set; }
    }

    public class Geoperms
    {
        public string iscontact { get; set; }
        public string isfamily { get; set; }
        public string isfriend { get; set; }
        public string ispublic { get; set; }
    }
    #endregion

    #region weather
    public class Weather
    {
        public Querywaether query { get; set; }
    }

    public class Querywaether
    {
        public int count { get; set; }
        public DateTime created { get; set; }
        public string lang { get; set; }
        public Resultswaether results { get; set; }
    }

    public class Resultswaether
    {
        public Channelwaether channel { get; set; }
    }

    public class Channelwaether
    {
        public Unitswaether units { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public string description { get; set; }
        public string language { get; set; }
        public string lastBuildDate { get; set; }
        public string ttl { get; set; }
        public Locationwaether location { get; set; }
        public Windwaether wind { get; set; }
        public Atmospherewaether atmosphere { get; set; }
        public Astronomywaether astronomy { get; set; }
        public Imagewaether image { get; set; }
        public Itemwaether item { get; set; }
    }

    public class Unitswaether
    {
        public string distance { get; set; }
        public string pressure { get; set; }
        public string speed { get; set; }
        public string temperature { get; set; }
    }

    public class Locationwaether
    {
        public string city { get; set; }
        public string country { get; set; }
        public string region { get; set; }
    }

    public class Windwaether
    {
        public string chill { get; set; }
        public string direction { get; set; }
        public string speed { get; set; }
    }

    public class Atmospherewaether
    {
        public string humidity { get; set; }
        public string pressure { get; set; }
        public string rising { get; set; }
        public string visibility { get; set; }
    }

    public class Astronomywaether
    {
        public string sunrise { get; set; }
        public string sunset { get; set; }
    }

    public class Imagewaether
    {
        public string title { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string link { get; set; }
        public string url { get; set; }
    }

    public class Itemwaether
    {
        public string title { get; set; }
        public string lat { get; set; }
        public string _long { get; set; }
        public string link { get; set; }
        public string pubDate { get; set; }
        public Conditionwaether condition { get; set; }
        public Forecastwaether[] forecast { get; set; }
        public string description { get; set; }
        public Guidwaether guid { get; set; }
    }

    public class Conditionwaether
    {
        public string code { get; set; }
        public string date { get; set; }
        public string temp { get; set; }
        public string text { get; set; }
    }

    public class Guidwaether
    {
        public string isPermaLink { get; set; }
    }

    public class Forecastwaether
    {
        public string code { get; set; }
        public string date { get; set; }
        public string day { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string text { get; set; }
    }
    #endregion 

}
