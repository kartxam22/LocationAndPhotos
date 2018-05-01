using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Linq;

namespace LocationandPhotos
{
    public class DB
    {
        SQLiteConnection _connection;
        public DB()
        {
            _connection = DependencyService.Get<ISQLite>().GetConnection();
            _connection.CreateTable<loc>();
            _connection.CreateTable<ImageList>();
        }


        public loc GetLocation()
        {
            loc l = (from c in _connection.Table<loc>() select c).FirstOrDefault();

            if (l == null)
                return l = new loc();
            else
                return l;
        }
        public void AppUpdate(loc Appdetails)
        {
            _connection.DeleteAll<loc>();
            _connection.Insert(Appdetails);
        }
        public void DeleteAppData()
        {
            _connection.DeleteAll<loc>();
        }

        public List<ImageList> GetAllImages()
        {
            List<ImageList> countrylist = (from c in _connection.Table<ImageList>() select c).ToList();

            return countrylist;

        }
        public ImageList GetImages(string img)
        {
            ImageList l = _connection.Table<ImageList>().FirstOrDefault(x => x.image == img);

            return l;
        }
        public void AppImageUpdate(ImageList Appdetails)
        {
            //_connection.DeleteAll<ImageList>();
            _connection.Insert(Appdetails);
        }

        public void DeleteAllAppImageData()
        {
            _connection.DeleteAll<ImageList>();
        }
        public void DeleteAppImageData(string img)
        {
            ImageList l = GetImages(img);
            if (l != null)
            {
                _connection.Delete(l);
            }
        }
    }
}
