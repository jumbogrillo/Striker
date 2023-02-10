using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stricker
{
	internal class Graphic
	{
		public static int Width = Striker.Width;
		public static int Height = Striker.Height;

		const int Margin_Top = 3;
		const int Margin_Left = Margin_Top * 2;
		const int Min_num_obst = 6;
		const int Max_num_obst = 10;

		public static void Rect(int x, int y, int width = 1, int height = 1, ConsoleColor bg = ConsoleColor.White)
		{
			Console.BackgroundColor = bg;
			for (int i = 0; i < width; i++)
				for (int j = 0; j < height; j++)
				{
					Console.SetCursorPosition((x + i) * 2, y + j);// 2 dimensione dei blocchi
					Console.Write("  ");
				}
			Console.ResetColor();
		}

		public static void Initialize_Map(String[,] map)
        {
			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					map[i, j] = "E";
				}
			}
		}

		public static void Draw_Map(String[,] map, ConsoleColor backGround = ConsoleColor.DarkGray, ConsoleColor enemy = ConsoleColor.Red, ConsoleColor player = ConsoleColor.DarkBlue, ConsoleColor obs = ConsoleColor.Gray)
		{
			Console.SetCursorPosition(Margin_Left +1 , Margin_Top +1 );
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (map[i,j] == "E")
                    {
						Console.BackgroundColor = backGround;						
						Console.Write("  ");
						Console.ResetColor();
                    }
					else if (map[i, j] == "Pl")
                    {
						Console.BackgroundColor = player;
						Console.Write("  ");
						Console.ResetColor();
					}
					else if (map[i,j] == "Enem")
                    {
						Console.BackgroundColor = enemy;
						Console.Write("  ");
						Console.ResetColor();
					}
					else if (map[i,j] == "Obs")
                    {
						Console.BackgroundColor = obs;
						Console.Write("  ");
						Console.ResetColor();
					}
                }
				Console.SetCursorPosition(Margin_Left + 1, Console.CursorTop + 1);
            }
		}

		public static void Draw_Frame(int width = Striker.Width + 1, int height = Striker.Height + 2, int margin_top = Margin_Top, int margin_left = Margin_Left, ConsoleColor fore = ConsoleColor.DarkYellow, ConsoleColor back = ConsoleColor.DarkGray)
		{
			Console.BackgroundColor = back;
			Console.ForegroundColor = fore;
			width *= 2;
			Console.SetCursorPosition(margin_left, margin_top);
			Console.Write("╔");
			for (int i = 0; i < width - 2; i++)
			{
				Console.Write("═");
			}
			Console.SetCursorPosition(margin_left, margin_top + 1);
			for (int i = 1; i < height - 1; i++)
			{
				Console.SetCursorPosition(margin_left, margin_top + i);
				Console.Write("║");
			}
			Console.SetCursorPosition(margin_left, margin_top + height - 1);
			Console.Write("╚");
			for (int i = 0; i < width - 2; i++)
			{
				Console.Write("═");
			}
			Console.Write("╝");
			Console.SetCursorPosition(margin_left + width - 1, margin_top);
			Console.Write("╗");
			Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop - 1);
			for (int i = 1; i < height - 1; i++)
			{
				Console.SetCursorPosition(margin_left + width - 1, margin_top + i);
				Console.Write("║");
			}
			Console.ResetColor();
		}

		public static void Draw_Obstacles_Randomly(String[,] map, int min_num_obst = Min_num_obst, int max_num_obst = Max_num_obst)
		{
			Random Random = new Random();
			int n_obs = Random.Next(min_num_obst, max_num_obst + 1);

            for (int i = 0; i < n_obs; i++)
            {
				int x = Random.Next(0, Height - 2);
				int y = Random.Next(0, Width - 2);
				int n = Random.Next(0, 10);

                for (int j = 0; j < n; j++)
                {
					int pos = Random.Next(0,10);
                    switch (pos) 
					{
						case 1:
							map[x, y] = "Obs";
							break;
						case 2:
							map[x, y +1] = "Obs";
							break;
						case 3:
							map[x, y +2] = "Obs";
							break;
						case 4:
							map[x +1, y] = "Obs";
							break;
						case 5:
							map[x + 1, y + 1] = "Obs";
							break;
						case 6:
							map[x +1, y + 2] = "Obs";
							break;
						case 7:
							map[x + 2, y] = "Obs";
							break;
						case 8:
							map[x + 2, y + 1] = "Obs";
							break;
						case 9:
							map[x +2 , y + 2] = "Obs";
							break;
					}
                }
            }
		}

		public static void Draw_Score(int score, ConsoleColor bg = ConsoleColor.White)
        {
			Console.SetCursorPosition( 30 + Width * 2, Margin_Top);

			Console.BackgroundColor = bg;

			int a = Console.CursorLeft;
			int b = Console.CursorTop;

			if (score % 10 == 0) bg = ConsoleColor.DarkYellow;

			Draw_0(a,b);
			Draw_0(a - 11, b);

			//if(score < 10)
			{

            }
        }

		public static void Draw_0(int a , int b)
        {
			Console.SetCursorPosition(a,b);
			Console.SetCursorPosition(Console.CursorLeft + 2, Console.CursorTop);
			Console.Write("    ");

			Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 1);
			Console.Write("  ");
			Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop + 1);
			Console.Write("  ");
			Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop + 1);
			Console.Write("  ");
			Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop + 1);
			Console.Write("  ");
			Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop + 1);
			Console.Write("  ");


			Console.SetCursorPosition(Console.CursorLeft - 6, Console.CursorTop + 1);
			Console.Write("    ");

			Console.SetCursorPosition(a, b + 1);
			Console.Write("  ");
			Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop + 1);
			Console.Write("  ");
			Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop + 1);
			Console.Write("  ");
			Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop + 1);
			Console.Write("  ");
			Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop + 1);
			Console.Write("  ");
		}
	}
}
