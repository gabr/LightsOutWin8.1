using System;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace LightsOutWin8._1
{
    /// <summary>
    /// Class responsible for whole board actions
    /// </summary>
    class BoardControler
    {
        #region Private fields: board, enable and disable brush
        /// <summary>
        /// Instance of Grid with buttons
        /// </summary>
        private Grid _board;

        /// <summary>
        /// Brush used for enabled buttons
        /// </summary>
        private SolidColorBrush _brushEnabled;

        /// <summary>
        /// Brush used for disabled buttons
        /// </summary>
        private SolidColorBrush _brushDisabled;
        #endregion

        #region Events: OnFinish
        /// <summary>
        /// Event raised when board is all enabled or disabled
        /// </summary>
        public event Action OnFinish;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor requires Grid instance with buttons inside
        /// </summary>
        /// <param name="board">Grid with buttons inside</param>
        public BoardControler(Grid board)
        {
            if (board == null)
                throw new ArgumentException("Given board object is null");

            _board = board;
            _brushEnabled = new SolidColorBrush(Color.FromArgb(0xFF, 0x3E, 0x65, 0xFF));
            _brushDisabled = (SolidColorBrush) (new Button()).Background;
        }
        #endregion

        #region Public methods: Move(), Clear(), RandomBoard(), CheckIfFinished()
        /// <summary>
        /// Performs single move on the board
        /// </summary>
        /// <param name="column">Column</param>
        /// <param name="row">Row</param>
        public void Move(int column, int row)
        {
            if (column < 0 || column > _board.ColumnDefinitions.Count - 1 ||
                row < 0 || row > _board.RowDefinitions.Count - 1)
                return;

            SwitchState(column, row);
            SwitchState(column - 1, row);
            SwitchState(column + 1, row);
            SwitchState(column, row - 1);
            SwitchState(column, row + 1);

            CheckIfFinished();
        }

        /// <summary>
        /// Clears whole board and enables it if disable
        /// </summary>
        public void Clear()
        {
            EnableBloard(true);

            for (int c = 0; c < _board.ColumnDefinitions.Count; c++)
                for (int r = 0; r < _board.ColumnDefinitions.Count; r++)
                    SetState(GetButton(c, r), false);
        }

        /// <summary>
        /// Generate random movements on the board
        /// </summary>
        public void RandomBoard()
        {
            Random rand = new Random(DateTime.Now.Millisecond);

            int c = _board.ColumnDefinitions.Count;
            int r = _board.RowDefinitions.Count;
            int maximumMoves = c * r;

            int moves = rand.Next(0, maximumMoves);
            for (int i = 0; i < moves; i++)
                Move(rand.Next(0, c), rand.Next(0, r));
        }

        /// <summary>
        /// Checks if board is all enabled or disabled
        /// </summary>
        public void CheckIfFinished()
        {
            bool state = GetState(GetButton(0, 0));

            for (int c = 0; c < _board.ColumnDefinitions.Count; c++)
                for (int r = 0; r < _board.ColumnDefinitions.Count; r++)
                    if (state != GetState(GetButton(c, r)))
                        return;

            if (OnFinish != null)
                OnFinish();

            EnableBloard(false);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Switches state of the single button
        /// </summary>
        /// <param name="column">Column</param>
        /// <param name="row">Row</param>
        private void SwitchState(int column, int row)
        {
            if (column < 0 || column > _board.ColumnDefinitions.Count - 1 ||
                row < 0 || row > _board.RowDefinitions.Count - 1)
                return;

            Button button = GetButton(column, row);
            SetState(button, !GetState(button));
        }

        /// <summary>
        /// Finds button in the Grid control
        /// </summary>
        /// <param name="column">Column</param>
        /// <param name="row">Row</param>
        /// <returns>Button</returns>
        private Button GetButton(int column, int row)
        {
            return _board.Children
                .Cast<Button>()
                .First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
        }

        /// <summary>
        /// Returns button enable/disable state based on its Background brush
        /// </summary>
        /// <param name="button">Button instance</param>
        /// <returns>True if button is enabled</returns>
        private bool GetState(Button button)
        {
            return button.Background == _brushEnabled ? true : false;
        }

        /// <summary>
        /// Sets given status to the given button
        /// </summary>
        /// <param name="button">Button instance</param>
        /// <param name="state">True for enable status</param>
        private void SetState(Button button, bool state)
        {
            button.Background = state ? _brushEnabled : _brushDisabled;
        }

        /// <summary>
        /// Enables/disables all buttons on the board
        /// </summary>
        /// <param name="enable">Board state</param>
        private void EnableBloard(bool enable)
        {
            for (int c = 0; c < _board.ColumnDefinitions.Count; c++)
                for (int r = 0; r < _board.ColumnDefinitions.Count; r++)
                    GetButton(c, r).IsEnabled = enable;
        }
        #endregion
    }
}
