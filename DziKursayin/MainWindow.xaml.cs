using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace DziKursayin
{
    

    public partial class MainWindow : Window
    {
        int[,] board = new int[8, 8];

        const int N = 8;
        readonly static int[,] moves = { 
            {+1,-2},
            {+2,-1},
            {+2,+1},
            {+1,+2},
            {-1,+2},
            {-2,+1},
            {-2,-1},
            {-1,-2}
        };

        struct ListMoves
        {
            public int x, y;
            public ListMoves(int _x, int _y) { x = _x; y = _y; }
        }

        public async Task horseMoveAsync()
        {
            int[,] board = new int[N, N];
            board.Initialize();

            int x = 0,
                y = 0;

            List<ListMoves> list = new List<ListMoves>(N * N);
            list.Add(new ListMoves(x, y));
            do
            {
                if (Move_Possible(board, x, y))
                {
                    int move = board[x, y];
                    board[x, y]++;
                    x += moves[move, 0];
                    y += moves[move, 1];
                    list.Add(new ListMoves(x, y));
                }
                else
                {
                    if (board[x, y] >= 8)
                    {
                        board[x, y] = 0;
                        list.RemoveAt(list.Count - 1);
                        if (list.Count == 0)
                        {
                            return;
                        }
                        x = list[list.Count - 1].x;
                        y = list[list.Count - 1].y;
                    }
                    board[x, y]++;
                }
            }
            while (list.Count < N * N);

            int last_x = list[0].x,
                last_y = list[0].y;
           
            for (int i = 1; i < list.Count; i++)
            {   
                await movieAsync(list[i].x, list[i].y);
                board[list[i].x, list[i].y] = 1;
                last_x = list[i].x;
                last_y = list[i].y;
            }
        }

        static bool Move_Possible(int[,] board, int cur_x, int cur_y)
        {
            if (board[cur_x, cur_y] >= 8)
                return false;

            int new_x = cur_x + moves[board[cur_x, cur_y], 0],
                new_y = cur_y + moves[board[cur_x, cur_y], 1];

            if (new_x >= 0 && new_x < N && new_y >= 0 && new_y < N && board[new_x, new_y] == 0)
                return true;

            return false;
        }
    
        public MainWindow()
        { InitializeComponent(); }

        async Task movieAsync(int i, int j)
        {
            await Task.Delay(1000);
            Grid.SetRow(Horse, i);
            Grid.SetColumn(Horse, j);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            horseMoveAsync();
        }

    }
}
