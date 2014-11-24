using MountainGuideBG.Common;
using MountainGuideBG.Data;
using MountainGuideBG.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Networking.Connectivity;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace MountainGuideBG
{
   
    public sealed partial class HubPage : Page
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        public HubPage()
        {
            this.InitializeComponent();

            // Hub is only supported in Portrait orientation
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

       
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }
               
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

      
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            this.LoadingBar.IsActive = true;
            this.LoadingBar.Visibility = Visibility.Visible;
            try
            {
                var sampleDataGroups = await AppViewModel.GetMountainsAsync();
                var cabinsData = AppViewModel.GetCabins();
                var visitedVabinsData = AppViewModel.GetVisitedCabins();
                this.DefaultViewModel["Groups"] = sampleDataGroups;
                this.DefaultViewModel["Cabins"] = cabinsData;
                this.DefaultViewModel["VisitedCabins"] = visitedVabinsData;
            }
            catch (Exception ex)
            {
                ShowMessageBox();
            }
            

            this.LoadingBar.IsActive = false;
            this.LoadingBar.Visibility = Visibility.Collapsed;
        }
        private async void ShowMessageBox()
        {
            //try
            //{
            //    ConnectionProfile InternetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
            //    if (InternetConnectionProfile == null)
            //    {
                    MessageDialog dialog = new MessageDialog("Could not get data from server, you are not connected to the internet :(", "Oops, Sorry!");
                    await dialog.ShowAsync();
            //    }
                
            //}
            //catch (Exception ex)
            //{
                
            //}

        }
     
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
        }

      
        private void GroupSection_ItemClick(object sender, ItemClickEventArgs e)
        {
            var mountain = (MountainModel)e.ClickedItem;
            if (!Frame.Navigate(typeof(MountainInfo), mountain))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

     
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var itemId = ((CabinModel)e.ClickedItem).UniqueId;
            if (!Frame.Navigate(typeof(ItemPage), itemId))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
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
        /// <param name="e">Event data that describes how this page was reached.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CheckList));
        }

        private void GroupSection_Holding(object sender, HoldingRoutedEventArgs e)
        {

            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }

        private void MountainInfoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem item = sender as MenuFlyoutItem;
            MountainModel mountain = item.DataContext as MountainModel;

            if (!Frame.Navigate(typeof(MountainInfo), mountain))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        private void AllCabinsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem item = sender as MenuFlyoutItem;
            MountainModel mountain = item.DataContext as MountainModel;

            if (!Frame.Navigate(typeof(SectionPage), mountain))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        private async void Cabin_Holding(object sender, HoldingRoutedEventArgs e)
        {
            var item = sender as StackPanel;
            CabinModel cabin = item.DataContext as CabinModel;
            AppViewModel.RemoveFromCabins(cabin.UniqueId);
            AppViewModel.AddToVisitedCabins(cabin);
            MessageDialog dialog = new MessageDialog(string.Format("Браво, хижа {0} вече е посетена :)", cabin.Name), "Продължавай в този дух!");
            await dialog.ShowAsync();
        }

        private void ComboBoxItem_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }
    }
}