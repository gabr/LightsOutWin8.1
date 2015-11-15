using System;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace LightsOutWin8._1
{
    class BoardControler
    {
        private Grid _board;
        private SolidColorBrush _brushEnabled;
        private SolidColorBrush _brushDisabled;

        public BoardControler(Grid board)
        {
            if (board == null)
                throw new ArgumentException("Given board object is null");

            _board = board;
            _brushEnabled = new SolidColorBrush(Color.FromArgb(0xFF, 0x3E, 0x65, 0xFF));
            _brushDisabled = (SolidColorBrush) (new Button()).Background;
        }

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
        }

        public void Clear()
        {
            for (int c = 0; c < _board.ColumnDefinitions.Count; c++)
                for (int r = 0; r < _board.ColumnDefinitions.Count; r++)
                    SetState(GetButton(c, r), false);
        }

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

        private void SwitchState(int column, int row)
        {
            if (column < 0 || column > _board.ColumnDefinitions.Count - 1 ||
                row < 0 || row > _board.RowDefinitions.Count - 1)
                return;

            Button button = GetButton(column, row);
            SetState(button, !GetState(button));
        }

        private Button GetButton(int column, int row)
        {
            return _board.Children
                .Cast<Button>()
                .First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
        }

        private bool GetState(Button button)
        {
            return button.Background == _brushEnabled ? true : false;
        }

        private void SetState(Button button, bool state)
        {
            button.Background = state ? _brushEnabled : _brushDisabled;
        }
    }
}
