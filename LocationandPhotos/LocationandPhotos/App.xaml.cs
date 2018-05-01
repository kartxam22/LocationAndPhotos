using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LocationandPhotos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class App : Application
    {
        #region DB Assign
        public static DB database;
        public static DB Database
        {
            get
            {
                if (database == null)
                {
                    database = new DB();
                }
                return database;
            }
        }
        #endregion

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LocationandPhotos.MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes

        }
    }
}
