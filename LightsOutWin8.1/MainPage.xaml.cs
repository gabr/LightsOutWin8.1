using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace LightsOutWin8._1
{
    public sealed partial class MainPage : Page
    {
        private int _movesCounter;
        private BoardControler _boardControler;

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            // create board controler and subscribe events
            _boardControler = new BoardControler(Board);
            _boardControler.OnFinish += _boardControler_OnFinish;
        }

        private void _boardControler_OnFinish()
        {
            Moves.Text = "Finished in: " + _movesCounter;
        }

        private void IncrementMovesCounter()
        {
            ++_movesCounter;
            Moves.Text = "Moves: " + _movesCounter;
        }

        private void ResetMovesCounter()
        {
            _movesCounter = 0;
            Moves.Text = "Moves: " + _movesCounter;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Reset_Click(null, null);
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            _boardControler.Clear();
            _boardControler.RandomBoard();
            ResetMovesCounter();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IncrementMovesCounter();
            Button button = (Button)sender;
            _boardControler.Move(Grid.GetColumn(button), Grid.GetRow(button));
        }
    }
}
