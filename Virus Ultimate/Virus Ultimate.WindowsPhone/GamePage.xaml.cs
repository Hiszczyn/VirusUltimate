using Virus_Ultimate.Common;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Virus_Ultimate.Services;
using Windows.UI.Popups;
using Windows.Phone.UI.Input;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media.Animation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Virus_Ultimate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        System.Diagnostics.Stopwatch _sw = new System.Diagnostics.Stopwatch();
        private readonly BoardService _boardService;
        private readonly GameService _gameService;
        private RankService _rankservice;
        private PlayerDialog _playerDialog;
        public GamePage()
        {
            InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += NavigationHelper_LoadState;
            navigationHelper.SaveState += NavigationHelper_SaveState;

            _rankservice = new RankService();
            _playerDialog = new PlayerDialog();
            _boardService = new BoardService();
            _gameService = new GameService();
            _sw.Start();
            _boardService.resetBoard(1);
            updatetBoard();
            setUserNameDialog();
        }

        private void setUserNameDialog()
        {
            PlayerDialog _playerDialog = new PlayerDialog();
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

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
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

        #region messages
        private async void WinMessageBoxDisplay()
        {
            //Creating instance for the MessageDialog Class  
            //and passing the message in it's Constructor  
            MessageDialog msgbox = new MessageDialog("Level complete. Get ready for next level!");
            //Calling the Show method of MessageDialog class  
            //which will show the MessageBox  
            await msgbox.ShowAsync();
        }

        private async Task RecordMessageBoxDisplay()
        {
            //Creating instance for the MessageDialog Class  
            //and passing the message in it's Constructor  
            MessageDialog msgbox = new MessageDialog("You have a new record. Your result will be saved as "+_gameService._game.Playername);
            //Calling the Show method of MessageDialog class  
            //which will show the MessageBox  
            await msgbox.ShowAsync();
        }

        private async void LoseMessageBoxDisplay()
        {

            MessageDialog msgbox = new MessageDialog("This is the end my friend. Your score is : "+_gameService._game.Score +". Would you like to play again?");

            msgbox.Commands.Clear();
            msgbox.Commands.Add(new UICommand { Label = "Yes", Id = 0 });
            msgbox.Commands.Add(new UICommand { Label = "No", Id = 1 });

            var res = await msgbox.ShowAsync();

            if ((int)res.Id == 0)
            {
                _boardService.resetBoard(1);
                _gameService.cleanGame();
                updatetBoard();
                _sw.Reset();
                _sw.Start();
            }

            if ((int)res.Id == 1)
            {
                Frame.Navigate(typeof(MainPage));
            }
        }

        #endregion
        private bool hasRecord()
        {
            return false;
        }

        private void saveRecord()
        {
            
        }

        private void updatetBoard()
        {
            foreach (var square in _boardService._board.Squares)
            {
                string squareName = _boardService._squareName[square.Column] + _boardService._squareName[square.Row];
                Border control = (Border)FindName(squareName);
                if (control != null)
                {
                    control.Background = new SolidColorBrush(_boardService._squareColor[square.Color]);
                    var mar = control.Margin;
                    if (square.Status < 1)
                    {
                        mar.Left = 0;
                        mar.Right = 0;
                        mar.Top = 0;
                        mar.Bottom = 0; 
                        var animationForBorder = (Storyboard)FindName("an"+squareName);
                        animationForBorder.Begin();
                    }
                    else
                    {
                        mar.Left = 2;
                        mar.Right = 2;
                        mar.Top = 2;
                        mar.Bottom = 2;
                    }
                    control.Margin = mar;

                }
            }
            updateScore();
        }

        private void updateScore()
        {
            int calc;
            TextBlock control = (TextBlock)FindName("ScoreTextBox");
            calc = _boardService._board.Infected + _gameService._game.Score;
            control.Text = control.Text.Split(':')[0] + ": " + calc;


            ((TextBlock)FindName("RoundTextBox")).Text= "Round: " + _boardService._board.RoundObject.RoundNumber;

 
            control = (TextBlock)FindName("BestMoveTextBox");
            control.Text = "Best move: " + _boardService._board.BestMove; 

            control = (TextBlock)FindName("MovesTextBox");
            control.Text = control.Text.Split(':')[0] + ": " + _boardService._board.DoneMoves + " of " + _boardService._board.RoundObject.Limit;
        }

        

        private void choseColor(int color)
        {
            _boardService.play(color);
            updatetBoard();
            updateScore();
            if(_boardService.hasWon())
            {
                _sw.Stop();
                _gameService.updateScore(_boardService._board.Infected,_boardService._board.BestMove,_sw.Elapsed.Minutes*60+_sw.Elapsed.Seconds);
                _sw.Reset();
                _boardService.resetBoard(_boardService._board.RoundObject.RoundNumber + 1);
                WinMessageBoxDisplay();
                updatetBoard();
                _sw.Start();
            }
                if(_boardService.hasLost())
            {
                _gameService.updateScore(_boardService._board.Infected, _boardService._board.BestMove, -1);
                checkIfRecord();
            }
        }

        private async void checkIfRecord()
        {
            bool hasBeenShowed = false;
            bool time = false;
            bool score = false;
            bool move = false;
            var scores = await _rankservice.getTopScores();
            if (_rankservice.ScoreByTypeFilter(scores, 0)[_rankservice.ScoreByTypeFilter(scores, 0).Count - 1].Result < _gameService._game.Score)
            {
                hasBeenShowed = true;
                score = true;
            }
            if (_rankservice.ScoreByTypeFilter(scores, 1)[_rankservice.ScoreByTypeFilter(scores, 1).Count - 1].Result > _gameService._game.BestTime)
            {
                hasBeenShowed = true;
                time = true;
            }
            if (_rankservice.ScoreByTypeFilter(scores, 2)[_rankservice.ScoreByTypeFilter(scores, 2).Count - 1].Result < _gameService._game.BestMove)
            {
                hasBeenShowed = true;
                move = true;
            }

            if (hasBeenShowed)
            {
                if (_gameService._game.Playername == null || _gameService._game.Playername == "")
                {
                    await _playerDialog.ShowAsync();
                    _gameService._game.Playername = _playerDialog._name;
                }
                string response;
                if (score)
                    response = await _rankservice.addNewScore(_gameService._game.Playername, _gameService._game.Score, 0);
                if (time)
                    response = await _rankservice.addNewScore(_gameService._game.Playername, _gameService._game.BestTime, 1);
                if (move)
                    response = await _rankservice.addNewScore(_gameService._game.Playername, _gameService._game.BestMove, 2);
                await RecordMessageBoxDisplay();
            }
            LoseMessageBoxDisplay();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            choseColor(0);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            choseColor(1);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            choseColor(2);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            choseColor(3);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            choseColor(4);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            choseColor(5);
        }
    }
}
