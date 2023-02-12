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
		const int Min_num_obst = 20;
		const int Max_num_obst = 70; // per rendere il gioco più spicy

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

		public static void Draw_Map(String[,] map, ConsoleColor backGround = ConsoleColor.DarkGray, ConsoleColor enemy = ConsoleColor.Red, ConsoleColor player = ConsoleColor.DarkBlue, ConsoleColor obs = ConsoleColor.Gray, ConsoleColor shoot = ConsoleColor.White)
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
					else if (map[i,j] == "Sh")
                    {
						Console.BackgroundColor = backGround;
						Console.ForegroundColor = shoot;
						Console.Write(" ·");
						Console.ResetColor();
                    }
                }
				Console.SetCursorPosition(Margin_Left + 1, Console.CursorTop + 1);
            }
		}

		public static void Draw_Frame(int width = Striker.Width + 1, int height = Striker.Height + 2, int margin_top = Margin_Top, int margin_left = Margin_Left, ConsoleColor fore = ConsoleColor.DarkYellow, ConsoleColor back = ConsoleColor.DarkGray)
		{
			//Console.BackgroundColor = ConsoleColor.Black;
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

			Console.BackgroundColor = ConsoleColor.Black;

            for (int i = 0; i < 7; i++)
            {
				Console.SetCursorPosition(12 + Width * 2, Margin_Top + i);
				Console.Write("                           ");
            }

			Console.SetCursorPosition(27 + Width * 2, Margin_Top);
			Console.BackgroundColor = bg;

			int a = Console.CursorLeft;
			int b = Console.CursorTop;

			if (score % 10 == 0) bg = ConsoleColor.DarkYellow;

			String _score = score.ToString();

			if (_score.Length == 1)
			{
				Draw_0(a - 14, b);

				switch (score)
				{
					case 0:
						Draw_0(a, b);
						break;
					case 1:
						Draw_1(a, b);
						break;
					case 2:
						Draw_2(a, b);
						break;
					case 3:
						Draw_3(a, b);
						break;
					case 4:
						Draw_4(a, b);
						break;
					case 5:
						Draw_5(a, b);
						break;
					case 6:
						Draw_6(a, b);
						break;
					case 7:
						Draw_7(a, b);
						break;
					case 8:
						Draw_8(a, b);
						break;
					case 9:
						Draw_9(a, b);
						break;
				}
			}
			else
			{
				int num1 = Convert.ToInt16(_score[0].ToString());
				int num2 = Convert.ToInt16(_score[1].ToString());

				a -= 14;
				switch (num1)
				{
					case 0:
						Draw_0(a, b);
						break;
					case 1:
						Draw_1(a, b);
						break;
					case 2:
						Draw_2(a, b);
						break;
					case 3:
						Draw_3(a, b);
						break;
					case 4:
						Draw_4(a, b);
						break;
					case 5:
						Draw_5(a, b);
						break;
					case 6:
						Draw_6(a, b);
						break;
					case 7:
						Draw_7(a, b);
						break;
					case 8:
						Draw_8(a, b);
						break;
					case 9:
						Draw_9(a, b);
						break;
				}

				a += 14;
				switch (num2)
				{
					case 0:
						Draw_0(a, b);
						break;
					case 1:
						Draw_1(a, b);
						break;
					case 2:
						Draw_2(a, b);
						break;
					case 3:
						Draw_3(a, b);
						break;
					case 4:
						Draw_4(a, b);
						break;
					case 5:
						Draw_5(a, b);
						break;
					case 6:
						Draw_6(a, b);
						break;
					case 7:
						Draw_7(a, b);
						break;
					case 8:
						Draw_8(a, b);
						break;
					case 9:
						Draw_9(a, b);
						break;
				}
			}
        }

		static void Draw_0(int a, int b, ConsoleColor bg = ConsoleColor.White)
        {
			Console.BackgroundColor = bg;
			Console.SetCursorPosition(a,b);
			Console.SetCursorPosition(Console.CursorLeft + 2, Console.CursorTop);
			Console.Write("      ");

			Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
			Draw_Down_3();

			Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop + 2);
			Draw_Down_3();


			Console.SetCursorPosition(Console.CursorLeft - 9, Console.CursorTop);
			Console.Write("      ");

			Console.SetCursorPosition(a - 1, b);
			Draw_Down_3();

			Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop + 2);
			Draw_Down_3();
		}

		static void Draw_1(int a, int b, ConsoleColor bg = ConsoleColor.White)
        {
			Console.BackgroundColor = bg;
			Console.SetCursorPosition(a, b);
			Console.SetCursorPosition(Console.CursorLeft + 9,Console.CursorTop);
			Draw_Down_3();

			Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop + 2);
			Draw_Down_3();
		}

		static void Draw_2(int a, int b, ConsoleColor bg = ConsoleColor.White)
        {
			Console.BackgroundColor = bg;
			Console.SetCursorPosition(a, b);
			Console.SetCursorPosition(Console.CursorLeft + 2, Console.CursorTop);
			Console.Write("      ");

			Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
			Draw_Down_3();

			Console.SetCursorPosition(Console.CursorLeft - 9, Console.CursorTop + 1);
			Console.Write("      ");

			Console.SetCursorPosition(Console.CursorLeft - 9, Console.CursorTop + 1);
			Draw_Down_3();

			Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
			Console.Write("      ");
		}

		static void Draw_3(int a, int b, ConsoleColor bg = ConsoleColor.White)
        {
			Console.BackgroundColor = bg;
			Console.SetCursorPosition(a, b);
			Console.SetCursorPosition(Console.CursorLeft + 2, Console.CursorTop);
			Console.Write("      ");

			Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
			Draw_Down_3();

			Console.SetCursorPosition(Console.CursorLeft - 9, Console.CursorTop + 1);
			Console.Write("      ");

			Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop + 1);
			Draw_Down_3();

			Console.SetCursorPosition(Console.CursorLeft - 9, Console.CursorTop);
			Console.Write("      ");
		}

		static void Draw_4(int a, int b, ConsoleColor bg = ConsoleColor.White)
		{
			Console.BackgroundColor = bg;
			Console.SetCursorPosition(a, b);

			Console.SetCursorPosition(Console.CursorLeft -1, Console.CursorTop);
			Draw_Down_3();

			Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop + 1);
			Console.Write("      ");

			Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop + 1);
			Draw_Down_3();

			Console.SetCursorPosition(a + 9, b);
			Draw_Down_3();
		}

		static void Draw_5(int a, int b, ConsoleColor bg = ConsoleColor.White)
		{
			Console.BackgroundColor = bg;
			Console.SetCursorPosition(a, b);
			Console.SetCursorPosition(Console.CursorLeft + 2, Console.CursorTop);
			Console.Write("      ");

			Console.SetCursorPosition(Console.CursorLeft - 9, Console.CursorTop);
			Draw_Down_3();

			Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop + 1);
			Console.Write("      ");

			Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop + 1);
			Draw_Down_3();

			Console.SetCursorPosition( Console.CursorLeft - 9, Console.CursorTop);
			Console.Write("      ");
		}

		static void Draw_6(int a, int b, ConsoleColor bg = ConsoleColor.White)
		{
			Console.BackgroundColor = bg;
			Console.SetCursorPosition(a, b);
			Console.SetCursorPosition(Console.CursorLeft + 2, Console.CursorTop);
			Console.Write("      ");

			Console.SetCursorPosition(Console.CursorLeft - 9, Console.CursorTop);
			Draw_Down_3();

			Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop + 1);
			Console.Write("      ");

			Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop + 1);
			Draw_Down_3();

			Console.SetCursorPosition(Console.CursorLeft - 9, Console.CursorTop);
			Console.Write("      ");

			Console.SetCursorPosition(a - 1, b + 4);
			Draw_Down_3();
		}

		static void Draw_7(int a, int b, ConsoleColor bg = ConsoleColor.White)
		{
			Console.BackgroundColor = bg;
			Console.SetCursorPosition(a, b);
			Console.SetCursorPosition(Console.CursorLeft + 2, Console.CursorTop);
			Console.Write("      ");

			Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
			Draw_Down_3();

			Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop + 2);
			Draw_Down_3();
		}

		static void Draw_8(int a, int b, ConsoleColor bg = ConsoleColor.White)
		{
			Console.BackgroundColor = bg;
			Console.SetCursorPosition(a, b);
			Console.SetCursorPosition(Console.CursorLeft + 2, Console.CursorTop);
			Console.Write("      ");

			Console.SetCursorPosition(Console.CursorLeft - 9, Console.CursorTop);
			Draw_Down_3();

			Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop + 1);
			Console.Write("      ");

			Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop + 1);
			Draw_Down_3();

			Console.SetCursorPosition(Console.CursorLeft - 9, Console.CursorTop);
			Console.Write("      ");

			Console.SetCursorPosition(a - 1, b + 4);
			Draw_Down_3();

			Console.SetCursorPosition(a + 9, b);
			Draw_Down_3();

			Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop + 2);
			Draw_Down_3();
		}

		static void Draw_9(int a, int b, ConsoleColor bg = ConsoleColor.White)
		{
			Console.BackgroundColor = bg;
			Console.SetCursorPosition(a, b);
			Console.SetCursorPosition(Console.CursorLeft + 2, Console.CursorTop);
			Console.Write("      ");

			Console.SetCursorPosition(Console.CursorLeft - 9, Console.CursorTop);
			Draw_Down_3();

			Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop + 1);
			Console.Write("      ");

			Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop + 1);
			Draw_Down_3();

			Console.SetCursorPosition(Console.CursorLeft - 9, Console.CursorTop);
			Console.Write("      ");

			Console.SetCursorPosition(a + 9, b);
			Draw_Down_3();

			Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop + 2);
			Draw_Down_3();
		}

		static void Draw_Down_3()
        {
			Console.Write("  ");
			Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop + 1);
			Console.Write("  ");
			Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop + 1);
			Console.Write("  ");
		}

		public static void Draw_Life_Bar(int life)
        {
			int life_bar_width = Width - (Width % 5) - 4;
			int margin_left_bar = Margin_Left + Convert.ToInt32((Width - life_bar_width) / 2);
			int margin_top_bar = Margin_Top - 2;
			int life_bar_unit = life_bar_width / 5;

			Console.SetCursorPosition(margin_left_bar, margin_top_bar);
			Console.BackgroundColor = ConsoleColor.Black;
			for (int i = 0; i < life_bar_width * 2 + 3; i++)            
			{
				
				Console.Write(" ");
            }

			Console.SetCursorPosition(margin_left_bar, margin_top_bar);

            switch (life)
            {
				case 1:
					Console.BackgroundColor = ConsoleColor.DarkRed;
					break;
				case 2:
					Console.BackgroundColor = ConsoleColor.Red;
					break;
				case 3:
					Console.BackgroundColor = ConsoleColor.DarkYellow;
					break;
				case 4:
					Console.BackgroundColor = ConsoleColor.DarkGreen;
					break;
				case 5:
					Console.BackgroundColor = ConsoleColor.Green;
					break;
			}

            for (int i = 0; i < life; i++)
            {
                for (int j = 0; j < life_bar_unit; j++)
                {
					Console.Write("  ");
                }
				Console.Write(" ");
            }

			Console.ResetColor();

			Console.SetCursorPosition(margin_left_bar - 1, margin_top_bar);
			Console.Write("║");
			Console.SetCursorPosition(margin_left_bar - 1, margin_top_bar-1);
			Console.Write("╔");
            for (int i = 0; i < life_bar_width * 2 + 2; i++)
            {
				Console.Write("═");
            }
			Console.Write("╗");
			Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop + 1);
			Console.Write("║");
			Console.SetCursorPosition(margin_left_bar - 1, margin_top_bar + 1);
			Console.Write("╚");
			
			for (int i = 0; i < life_bar_width * 2 + 2; i++)
			{
				Console.Write("═");
			}
			Console.Write("╝");
		}
	}
}
