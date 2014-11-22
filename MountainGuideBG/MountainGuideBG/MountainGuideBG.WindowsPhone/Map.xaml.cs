using MountainGuideBG.Common;
using MountainGuideBG.Data;
using MountainGuideBG.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Services.Maps;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MountainGuideBG
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Map : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public Map()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            this.LoadingBar.IsActive = true;
            this.LoadingBar.Visibility = Visibility.Visible;

            var item = (CabinModel)e.NavigationParameter;
            this.DefaultViewModel["Item"] = item;

            Geolocator locator = new Geolocator();
            locator.DesiredAccuracy = PositionAccuracy.High;
            Geoposition position = null;
            try
            {
                position = await locator.GetGeopositionAsync();
            }
            catch (Exception ex)
            {
                ShowMessageBox();
            }

            string uriToLaunch =String.Format( @"bingmaps:?rtp=pos.{0}_{1}~pos.{2}_{3}",position.Coordinate.Latitude,position.Coordinate.Longitude,item.Latitude,item.Longtitude);
            var uri = new Uri(uriToLaunch);

            // Launch the URI
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);
           
            //ShowRouteToCabin(item);

            BasicGeoposition cabinLocation = new BasicGeoposition();
            cabinLocation.Latitude = item.Latitude;
            cabinLocation.Longitude = item.Longtitude;
            Geopoint cabinPoint = new Geopoint(cabinLocation);

            await this.myMap.TrySetViewAsync(cabinPoint, 14D);

            this.LoadingBar.IsActive = false;
            this.LoadingBar.Visibility = Visibility.Collapsed;
        }

        private async void ShowRouteToCabin(CabinModel item)
        {

            Geolocator locator = new Geolocator();
            locator.DesiredAccuracy = PositionAccuracy.High;

            Geoposition startPosition = null;
            try
            {
                startPosition = await locator.GetGeopositionAsync();
            }
            catch (Exception ex)
            {
                ShowMessageBox();
            }

            BasicGeoposition startLocation = new BasicGeoposition();
            startLocation.Latitude = startPosition.Coordinate.Latitude;
            startLocation.Longitude = startPosition.Coordinate.Longitude;
            Geopoint startPoint = new Geopoint(startLocation);

            // End at the city of Seattle, Washington.
            BasicGeoposition endLocation = new BasicGeoposition();
            endLocation.Latitude = item.Latitude;
            endLocation.Longitude = item.Longtitude;
            Geopoint endPoint = new Geopoint(endLocation);

            // Get the route between the points.
            MapRouteFinderResult routeResult = await MapRouteFinder.GetWalkingRouteAsync(startPoint, endPoint);

            if (routeResult.Status == MapRouteFinderStatus.Success)
            {
                // Use the route to initialize a MapRouteView.
                MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
                viewOfRoute.RouteColor = Colors.Yellow;
                viewOfRoute.OutlineColor = Colors.Black;

                // Add the new MapRouteView to the Routes collection
                // of the MapControl.
                this.myMap.Routes.Add(viewOfRoute);

                // Fit the MapControl to the route.
                await this.myMap.TrySetViewBoundsAsync(
                    routeResult.Route.BoundingBox,
                    null,
                    Windows.UI.Xaml.Controls.Maps.MapAnimationKind.None);
            }
        }

        private async void ShowMessageBox()
        {
            MessageDialog dialog = new MessageDialog("Could not get location, maybe you dont have internet or are in a cave :)", "Oops, Sorry!");
            await dialog.ShowAsync();
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
