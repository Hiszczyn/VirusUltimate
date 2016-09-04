using Virus_Ultimate.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Virus_Ultimate.Services;
using Virus_Ultimate.Data;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Virus_Ultimate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RankPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private RankService _rankService;
        public RankPage()
        {
            InitializeComponent();

           
            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += NavigationHelper_LoadState;
            navigationHelper.SaveState += NavigationHelper_SaveState;
            _rankService = new RankService();
            getDataFromService();
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return defaultViewModel; }
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
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
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
            base.OnNavigatedTo(e);
            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        #endregion

        private void setServices()
        {
            _rankService = new RankService();
        }

        private async void getDataFromService()
        {
            var topScores = await _rankService.getTopScores();
            UpdateLists(topScores);
        }
 
        private void UpdateLists(List<Score> topScores)
        {
            int i = 1;
            TextBlock control;
            foreach (var result in _rankService.ScoreByTypeFilter(topScores,0))
            {
                control = (TextBlock)FindName("scorePlaceTB"+i);
                control.Visibility = Visibility.Visible;

                control = (TextBlock)FindName("scoreNameTB" + i);
                control.Text = result.PlayerName;
                control.Visibility = Visibility.Visible;

                control = (TextBlock)FindName("scoreResTB" + i);
                control.Text = result.Result.ToString();
                control.Visibility = Visibility.Visible;
                i++;
            }
            i = 1;
            foreach (var result in _rankService.ScoreByTypeFilter(topScores, 2))
            {
                control = (TextBlock)FindName("movesPlaceTB" + i);
                control.Visibility = Visibility.Visible;

                control = (TextBlock)FindName("movesNameTB" + i);
                control.Text = result.PlayerName;
                control.Visibility = Visibility.Visible;

                control = (TextBlock)FindName("movesResTB" + i);
                control.Text = result.Result.ToString();
                control.Visibility = Visibility.Visible;
                i++;
            }

            i = 1;
            foreach (var result in _rankService.ScoreByTypeFilter(topScores, 1))
            {
                control = (TextBlock)FindName("timePlaceTB" + i);
                control.Visibility = Visibility.Visible;

                control = (TextBlock)FindName("timeNameTB" + i);
                control.Text = result.PlayerName;
                control.Visibility = Visibility.Visible;

                control = (TextBlock)FindName("timeResTB" + i);
                control.Text = result.Result.ToString();
                control.Visibility = Visibility.Visible;
                i++;
            }
        }
    }
}
