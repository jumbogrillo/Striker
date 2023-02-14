using Striker_finale;
using System;
using MongoDB.Driver;
using MongoDB.Bson;// To write in the cluster
using System.Threading;

namespace Stricker
{
	internal class Graphic
	{
		public static int Width = Striker.Width;
		public static int Height = Striker.Height;

		public const int Margin_Top = 3;
		public const int Margin_Left = Margin_Top * 2;
		const int Min_num_obst = 20;
		const int Max_num_obst = 70; // per rendere il gioco più spicy
		public static void Rect(int x, int y, string text = "  ", ConsoleColor bg = ConsoleColor.Black, ConsoleColor fg = ConsoleColor.Black, int size = 2, int marginLeft = 0, int marginTop = 0, bool setBG = true)
		{
			if (setBG) Console.BackgroundColor = bg;
			Console.ForegroundColor = fg;
			Console.SetCursorPosition(x * size + marginLeft, y + marginTop);
			Console.Write(text);
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
			Console.SetCursorPosition(Margin_Left + 1, Margin_Top + 1);
			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					if (map[i, j] == "E")
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
					else if (map[i, j] == "Enem")
					{
						Console.BackgroundColor = enemy;
						Console.Write("  ");
						Console.ResetColor();
					}
					else if (map[i, j] == "Obs")
					{
						Console.BackgroundColor = obs;
						Console.Write("  ");
						Console.ResetColor();
					}
					else if (map[i, j] == "Sh")
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
					int pos = Random.Next(0, 10);
					switch (pos)
					{
						case 1:
							map[x, y] = "Obs";
							break;
						case 2:
							map[x, y + 1] = "Obs";
							break;
						case 3:
							map[x, y + 2] = "Obs";
							break;
						case 4:
							map[x + 1, y] = "Obs";
							break;
						case 5:
							map[x + 1, y + 1] = "Obs";
							break;
						case 6:
							map[x + 1, y + 2] = "Obs";
							break;
						case 7:
							map[x + 2, y] = "Obs";
							break;
						case 8:
							map[x + 2, y + 1] = "Obs";
							break;
						case 9:
							map[x + 2, y + 2] = "Obs";
							break;
					}
				}
			}
		}

		/*public static void Draw_Score(int score, ConsoleColor bg = ConsoleColor.White)
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

			//if (score % 10 == 0) bg = ConsoleColor.DarkYellow;

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
			Console.SetCursorPosition(a, b);
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
			Console.SetCursorPosition(Console.CursorLeft + 9, Console.CursorTop);
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

			Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
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

			Console.SetCursorPosition(Console.CursorLeft - 9, Console.CursorTop);
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
		}*/

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
			Console.SetCursorPosition(margin_left_bar - 1, margin_top_bar - 1);
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
		public static void Draw_Score(int score, int font = 0)
		{
			Word(12 + Width * 2, Margin_Top, score.ToString(), font);
		}
		public static int Word(int x, int y, string text, int font = 0, ConsoleColor fg = ConsoleColor.White, int delay = 0)
		{
			int posX = x;
			for (int i = 0; i < text.Length; i++)
				DrawLetter(ref x, y, Index(text[i]), font, UpperCase(text[i]), fg, delay);
			return x - posX;
		}
		static void DrawLetter(ref int x, int y, int index, int font, bool upperCase = false, ConsoleColor fg = ConsoleColor.White, int delay = 0)
		{
			string[][,] fonts ={new string[,]{{"          ", "          ", "  ______  ", " |      \\ ", "  \\▓▓▓▓▓▓\\", " /      ▓▓", "|  ▓▓▓▓▓▓▓", " \\▓▓    ▓▓", "  \\▓▓▓▓▓▓▓", "          ", "          ", "          " },{" __       ", "|  \\      ", "| ▓▓____  ", "| ▓▓    \\ ", "| ▓▓▓▓▓▓▓\\", "| ▓▓  | ▓▓", "| ▓▓__/ ▓▓", "| ▓▓    ▓▓", " \\▓▓▓▓▓▓▓ ", "          ", "          ", "          " },{"          ", "          ", "  _______ ", " /       \\", "|  ▓▓▓▓▓▓▓", "| ▓▓      ", "| ▓▓_____ ", " \\▓▓     \\", "  \\▓▓▓▓▓▓▓", "          ", "          ", "          " },{"       __ ", "      |  \\", "  ____| ▓▓", " /      ▓▓", "|  ▓▓▓▓▓▓▓", "| ▓▓  | ▓▓", "| ▓▓__| ▓▓", " \\▓▓    ▓▓", "  \\▓▓▓▓▓▓▓", "          ", "          ", "          " },{"          ", "          ", "  ______  ", " /      \\ ", "|  ▓▓▓▓▓▓\\", "| ▓▓    ▓▓", "| ▓▓▓▓▓▓▓▓", " \\▓▓     \\", "  \\▓▓▓▓▓▓▓", "          ", "          ", "          " },{"  ______  ", " /      \\ ", "|  ▓▓▓▓▓▓\\", "| ▓▓_  \\▓▓", "| ▓▓ \\    ", "| ▓▓▓▓    ", "| ▓▓      ", "| ▓▓      ", " \\▓▓      ", "          ", "          ", "          " },{"          ", "          ", "  ______  ", " /      \\ ", "|  ▓▓▓▓▓▓\\", "| ▓▓  | ▓▓", "| ▓▓__| ▓▓", " \\▓▓    ▓▓", " _\\▓▓▓▓▓▓▓", "|  \\__| ▓▓", " \\▓▓    ▓▓", "  \\▓▓▓▓▓▓ " },{" __       ", "|  \\      ", "| ▓▓____  ", "| ▓▓    \\ ", "| ▓▓▓▓▓▓▓\\", "| ▓▓  | ▓▓", "| ▓▓  | ▓▓", "| ▓▓  | ▓▓", " \\▓▓   \\▓▓", "          ", "          ", "          " },{" __ ", "|  \\", " \\▓▓", "|  \\", "| ▓▓", "| ▓▓", "| ▓▓", "| ▓▓", " \\▓▓", "    ", "    ", "    " },{"          ", "          ", "       __ ", "      |  \\", "       \\▓▓", "      |  \\", "      | ▓▓", "      | ▓▓", " __   | ▓▓", "|  \\__/ ▓▓", " \\▓▓    ▓▓", "  \\▓▓▓▓▓▓ " },{" __       ", "|  \\      ", "| ▓▓   __ ", "| ▓▓  /  \\", "| ▓▓_/  ▓▓", "| ▓▓   ▓▓ ", "| ▓▓▓▓▓▓\\ ", "| ▓▓  \\▓▓\\", " \\▓▓   \\▓▓", "          ", "          ", "          " },{" __ ", "|  \\", "| ▓▓", "| ▓▓", "| ▓▓", "| ▓▓", "| ▓▓", "| ▓▓", " \\▓▓", "    ", "    ", "    " },{"              ", "              ", " ______ ____  ", "|      \\    \\ ", "| ▓▓▓▓▓▓\\▓▓▓▓\\", "| ▓▓ | ▓▓ | ▓▓", "| ▓▓ | ▓▓ | ▓▓", "| ▓▓ | ▓▓ | ▓▓", " \\▓▓  \\▓▓  \\▓▓", "              ", "              ", "              " },{"          ", "          ", " _______  ", "|       \\ ", "| ▓▓▓▓▓▓▓\\", "| ▓▓  | ▓▓", "| ▓▓  | ▓▓", "| ▓▓  | ▓▓", " \\▓▓   \\▓▓", "          ", "          ", "          " },{"          ", "          ", "  ______  ", " /      \\ ", "|  ▓▓▓▓▓▓\\", "| ▓▓  | ▓▓", "| ▓▓__/ ▓▓", " \\▓▓    ▓▓", "  \\▓▓▓▓▓▓ ", "          ", "          ", "          " },{"          ", "          ", "  ______  ", " /      \\ ", "|  ▓▓▓▓▓▓\\", "| ▓▓  | ▓▓", "| ▓▓__/ ▓▓", "| ▓▓    ▓▓", "| ▓▓▓▓▓▓▓ ", "| ▓▓      ", "| ▓▓      ", " \\▓▓      " },{"          ", "          ", "  ______  ", " /      \\ ", "|  ▓▓▓▓▓▓\\", "| ▓▓  | ▓▓", "| ▓▓__| ▓▓", " \\▓▓    ▓▓", "  \\▓▓▓▓▓▓▓", "      | ▓▓", "      | ▓▓", "       \\▓▓" },{"          ", "          ", "  ______  ", " /      \\ ", "|  ▓▓▓▓▓▓\\", "| ▓▓   \\▓▓", "| ▓▓      ", "| ▓▓      ", " \\▓▓      ", "          ", "          ", "          " },{"          ", "          ", "  _______ ", " /       \\", "|  ▓▓▓▓▓▓▓", " \\▓▓    \\ ", " _\\▓▓▓▓▓▓\\", "|       ▓▓", " \\▓▓▓▓▓▓▓ ", "          ", "          ", "          " },{"   __     ", "  |  \\    ", " _| ▓▓_   ", "|   ▓▓ \\  ", " \\▓▓▓▓▓▓  ", "  | ▓▓ __ ", "  | ▓▓|  \\", "   \\▓▓  ▓▓", "    \\▓▓▓▓ ", "          ", "          ", "          " },{"          ", "          ", " __    __ ", "|  \\  |  \\", "| ▓▓  | ▓▓", "| ▓▓  | ▓▓", "| ▓▓__/ ▓▓", " \\▓▓    ▓▓", "  \\▓▓▓▓▓▓ ", "          ", "          ", "          " },{"           ", "           ", " __     __ ", "|  \\   /  \\", " \\▓▓\\ /  ▓▓", "  \\▓▓\\  ▓▓ ", "   \\▓▓ ▓▓  ", "    \\▓▓▓   ", "     \\▓    ", "           ", "           ", "           " },{"              ", "              ", " __   __   __ ", "|  \\ |  \\ |  \\", "| ▓▓ | ▓▓ | ▓▓", "| ▓▓ | ▓▓ | ▓▓", "| ▓▓_/ ▓▓_/ ▓▓", " \\▓▓   ▓▓   ▓▓", "  \\▓▓▓▓▓\\▓▓▓▓ ", "              ", "              ", "              " },{"          ", "          ", " __    __ ", "|  \\  /  \\", " \\▓▓\\/  ▓▓", "  >▓▓  ▓▓ ", " /  ▓▓▓▓\\ ", "|  ▓▓ \\▓▓\\", " \\▓▓   \\▓▓", "          ", "          ", "          " },{"          ", "          ", " __    __ ", "|  \\  |  \\", "| ▓▓  | ▓▓", "| ▓▓  | ▓▓", "| ▓▓__/ ▓▓", " \\▓▓    ▓▓", " _\\▓▓▓▓▓▓▓", "|  \\__| ▓▓", " \\▓▓    ▓▓", "  \\▓▓▓▓▓▓ " },{"          ", "          ", " _______", "|        \\", " \\▓▓▓▓▓▓▓▓", "  /    ▓▓ ", " /  ▓▓▓▓_ ", "|  ▓▓    \\", " \\▓▓▓▓▓▓▓▓", "          ", "          ", "          " }, { "          ", "          ", "          ", "          ", "          ", "          ", "          ", "          ", "          ", "          ", "          ", "          " }, { "  ______  ", " /      \\ ", "|  ▓▓▓▓▓▓\\", "| ▓▓▓\\| ▓▓", "| ▓▓▓▓\\ ▓▓", "| ▓▓\\▓▓\\▓▓", "| ▓▓_\\▓▓▓▓", " \\▓▓  \\▓▓▓", "  \\▓▓▓▓▓▓ ", "          ", "          ", "          " },{ "   __   ", " _/  \\  ", "|   ▓▓  ", " \\▓▓▓▓  ", "  | ▓▓  ", "  | ▓▓  ", " _| ▓▓_ ", "|   ▓▓ \\", " \\▓▓▓▓▓▓", "        ", "        ", "        "},{"  ______  ", " /      \\ ", "|  ▓▓▓▓▓▓\\", " \\▓▓__| ▓▓", " /      ▓▓", "|  ▓▓▓▓▓▓ ", "| ▓▓_____ ", "| ▓▓     \\", " \\▓▓▓▓▓▓▓▓", "          ", "          ", "          " },{"  ______  ", " /      \\ ", "|  ▓▓▓▓▓▓\\", " \\▓▓__| ▓▓", "  |     ▓▓", " __\\▓▓▓▓▓\\", "|  \\__| ▓▓", " \\▓▓    ▓▓", "  \\▓▓▓▓▓▓ ", "          ", "          ", "          " },{" __    __ ", "|  \\  |  \\", "| ▓▓  | ▓▓", "| ▓▓__| ▓▓", "| ▓▓    ▓▓", " \\▓▓▓▓▓▓▓▓", "      | ▓▓", "      | ▓▓", "       \\▓▓", "          ", "          ", "          " },{" _______  ", "|       \\ ", "| ▓▓▓▓▓▓▓ ", "| ▓▓____  ", "| ▓▓    \\ ", " \\▓▓▓▓▓▓▓\\", "|  \\__| ▓▓", " \\▓▓    ▓▓", "  \\▓▓▓▓▓▓ ", "          ", "          ", "          " },{"  ______  ", " /      \\ ", "|  ▓▓▓▓▓▓\\", "| ▓▓___\\▓▓", "| ▓▓    \\ ", "| ▓▓▓▓▓▓▓\\", "| ▓▓__/ ▓▓", " \\▓▓    ▓▓", "  \\▓▓▓▓▓▓ ", "          ", "          ", "          " },{" ________ ", "|        \\", " \\▓▓▓▓▓▓▓▓", "    /  ▓▓ ", "   /  ▓▓  ", "  /  ▓▓   ", " /  ▓▓    ", "|  ▓▓     ", " \\▓▓      ", "          ", "          ", "          " },{"  ______  ", " /      \\ ", "|  ▓▓▓▓▓▓\\", "| ▓▓__/ ▓▓", " >▓▓    ▓▓", "|  ▓▓▓▓▓▓ ", "| ▓▓__/ ▓▓", " \\▓▓    ▓▓", "  \\▓▓▓▓▓▓ ", "          ", "          ", "          " },{"  ______  ", " /      \\ ", "|  ▓▓▓▓▓▓\\", "| ▓▓__/ ▓▓", " \\▓▓    ▓▓", " _\\▓▓▓▓▓▓▓", "|  \\__/ ▓▓", " \\▓▓    ▓▓", "  \\▓▓▓▓▓▓ ", "          ", "          ", "          " },{"  ______  ", " /      \\ ", "|  ▓▓▓▓▓▓\\", "| ▓▓__| ▓▓", "| ▓▓    ▓▓", "| ▓▓▓▓▓▓▓▓", "| ▓▓  | ▓▓", "| ▓▓  | ▓▓", " \\▓▓   \\▓▓", "          ", "          ", "          " },{" _______  ", "|       \\ ", "| ▓▓▓▓▓▓▓\\", "| ▓▓__/ ▓▓", "| ▓▓    ▓▓", "| ▓▓▓▓▓▓▓\\", "| ▓▓__/ ▓▓", "| ▓▓    ▓▓", " \\▓▓▓▓▓▓▓ ", "          ", "          ", "          " },{"  ______  ", " /      \\ ", "|  ▓▓▓▓▓▓\\", "| ▓▓   \\▓▓", "| ▓▓      ", "| ▓▓   __ ", "| ▓▓__/  \\", " \\▓▓    ▓▓", "  \\▓▓▓▓▓▓ ", "          ", "          ", "          " },{" _______  ", "|       \\ ", "| ▓▓▓▓▓▓▓\\", "| ▓▓  | ▓▓", "| ▓▓  | ▓▓", "| ▓▓  | ▓▓", "| ▓▓__/ ▓▓", "| ▓▓    ▓▓", " \\▓▓▓▓▓▓▓ ", "          ", "          ", "          " },{" ________ ", "|        \\", "| ▓▓▓▓▓▓▓▓", "| ▓▓__    ", "| ▓▓  \\   ", "| ▓▓▓▓▓   ", "| ▓▓_____ ", "| ▓▓     \\", " \\▓▓▓▓▓▓▓▓", "          ", "          ", "          " },{" ________ ", "|        \\", "| ▓▓▓▓▓▓▓▓", "| ▓▓__    ", "| ▓▓  \\   ", "| ▓▓▓▓▓   ", "| ▓▓      ", "| ▓▓      ", " \\▓▓      ", "          ", "          ", "          " },{"  ______  ", " /      \\ ", "|  ▓▓▓▓▓▓\\", "| ▓▓ __\\▓▓", "| ▓▓|    \\", "| ▓▓ \\▓▓▓▓", "| ▓▓__| ▓▓", " \\▓▓    ▓▓", "  \\▓▓▓▓▓▓ ", "          ", "          ", "          " },{" __    __ ", "|  \\  |  \\", "| ▓▓  | ▓▓", "| ▓▓__| ▓▓", "| ▓▓    ▓▓", "| ▓▓▓▓▓▓▓▓", "| ▓▓  | ▓▓", "| ▓▓  | ▓▓", " \\▓▓   \\▓▓", "          ", "          ", "          " },{" ______ ", "|      \\", " \\▓▓▓▓▓▓", "  | ▓▓  ", "  | ▓▓  ", "  | ▓▓  ", " _| ▓▓_ ", "|   ▓▓ \\", " \\▓▓▓▓▓▓", "        ", "        ", "        " },{"    _____ ", "   |     \\", "    \\▓▓▓▓▓", "      | ▓▓", " __   | ▓▓", "|  \\  | ▓▓", "| ▓▓__| ▓▓", " \\▓▓    ▓▓", "  \\▓▓▓▓▓▓ ", "          ", "          ", "          " },{" __    __ ", "|  \\  /  \\", "| ▓▓ /  ▓▓", "| ▓▓/  ▓▓ ", "| ▓▓  ▓▓  ", "| ▓▓▓▓▓\\  ", "| ▓▓ \\▓▓\\ ", "| ▓▓  \\▓▓\\", " \\▓▓   \\▓▓", "          ", "          ", "          " },{" __       ", "|  \\      ", "| ▓▓      ", "| ▓▓      ", "| ▓▓      ", "| ▓▓      ", "| ▓▓_____ ", "| ▓▓     \\", " \\▓▓▓▓▓▓▓▓", "          ", "          ", "          " },{" __       __ ", "|  \\     /  \\", "| ▓▓\\   /  ▓▓", "| ▓▓▓\\ /  ▓▓▓", "| ▓▓▓▓\\  ▓▓▓▓", "| ▓▓\\▓▓ ▓▓ ▓▓", "| ▓▓ \\▓▓▓| ▓▓", "| ▓▓  \\▓ | ▓▓", " \\▓▓      \\▓▓", "             ", "             ", "             " },{" __    __ ", "|  \\  |  \\", "| ▓▓\\ | ▓▓", "| ▓▓▓\\| ▓▓", "| ▓▓▓▓\\ ▓▓", "| ▓▓\\▓▓ ▓▓", "| ▓▓ \\▓▓▓▓", "| ▓▓  \\▓▓▓", " \\▓▓   \\▓▓", "          ", "          ", "          " },{"  ______  ", " /      \\ ", "|  ▓▓▓▓▓▓\\", "| ▓▓  | ▓▓", "| ▓▓  | ▓▓", "| ▓▓  | ▓▓", "| ▓▓__/ ▓▓", " \\▓▓    ▓▓", "  \\▓▓▓▓▓▓ ", "          ", "          ", "          " },{" _______  ", "|       \\ ", "| ▓▓▓▓▓▓▓\\", "| ▓▓__/ ▓▓", "| ▓▓    ▓▓", "| ▓▓▓▓▓▓▓ ", "| ▓▓      ", "| ▓▓      ", " \\▓▓      ", "          ", "          ", "          " },{"  ______  ", " /      \\ ", "|  ▓▓▓▓▓▓\\", "| ▓▓  | ▓▓", "| ▓▓  | ▓▓", "| ▓▓ _| ▓▓", "| ▓▓/ \\ ▓▓", " \\▓▓ ▓▓ ▓▓", "  \\▓▓▓▓▓▓\\", "      \\▓▓▓", "          ", "          " },{" _______  ", "|       \\ ", "| ▓▓▓▓▓▓▓\\", "| ▓▓__| ▓▓", "| ▓▓    ▓▓", "| ▓▓▓▓▓▓▓\\", "| ▓▓  | ▓▓", "| ▓▓  | ▓▓", " \\▓▓   \\▓▓", "          ", "          ", "          " },{"  ______  ", " /      \\ ", "|  ▓▓▓▓▓▓\\", "| ▓▓___\\▓▓", " \\▓▓    \\ ", " _\\▓▓▓▓▓▓\\", "|  \\__| ▓▓", " \\▓▓    ▓▓", "  \\▓▓▓▓▓▓ ", "          ", "          ", "          " },{" ________ ", "|        \\", " \\▓▓▓▓▓▓▓▓", "   | ▓▓   ", "   | ▓▓   ", "   | ▓▓   ", "   | ▓▓   ", "   | ▓▓   ", "    \\▓▓   ", "          ", "          ", "          " },{" __    __ ", "|  \\  |  \\", "| ▓▓  | ▓▓", "| ▓▓  | ▓▓", "| ▓▓  | ▓▓", "| ▓▓  | ▓▓", "| ▓▓__/ ▓▓", " \\▓▓    ▓▓", "  \\▓▓▓▓▓▓ ", "          ", "          ", "          " },{" __     __ ", "|  \\   |  \\", "| ▓▓   | ▓▓", "| ▓▓   | ▓▓", " \\▓▓\\ /  ▓▓", "  \\▓▓\\  ▓▓ ", "   \\▓▓ ▓▓  ", "    \\▓▓▓   ", "     \\▓    ", "           ", "           ", "           " },{" __       __ ", "|  \\  _  |  \\", "| ▓▓ / \\ | ▓▓", "| ▓▓/  ▓\\| ▓▓", "| ▓▓  ▓▓▓\\ ▓▓", "| ▓▓ ▓▓\\▓▓\\▓▓", "| ▓▓▓▓  \\▓▓▓▓", "| ▓▓▓    \\▓▓▓", " \\▓▓      \\▓▓", "             ", "             ", "             " },{" __    __ ", "|  \\  |  \\", "| ▓▓  | ▓▓", " \\▓▓\\/  ▓▓", "  >▓▓  ▓▓ ", " /  ▓▓▓▓\\ ", "|  ▓▓ \\▓▓\\", "| ▓▓  | ▓▓", " \\▓▓   \\▓▓", "          ", "          ", "          " },{" __      __ ", "|  \\    /  \\", " \\▓▓\\  /  ▓▓", "  \\▓▓\\/  ▓▓ ", "   \\▓▓  ▓▓  ", "    \\▓▓▓▓   ", "    | ▓▓    ", "    | ▓▓    ", "     \\▓▓    ", "            ", "            ", "            " },{" ________ ", "|        \\", " \\▓▓▓▓▓▓▓▓", "    /  ▓▓ ", "   /  ▓▓  ", "  /  ▓▓   ", " /  ▓▓___ ", "|  ▓▓    \\", " \\▓▓▓▓▓▓▓▓", "          ", "          ", "          " }, { "          ", "          ", "          ", "          ", "          ", "          ", "          ", "          ", "          ", "          ", "          ", "          " }},new string[,]{ { "▄▀█", "█▀█", }, { "█▄▄", "█▄█" }, { "█▀▀", "█▄▄" }, { "█▀▄", "█▄▀" }, { "█▀▀", "██▄" }, { "█▀▀", "█▀░" }, { "█▀▀", "█▄█" }, { "█░█", "█▀█" }, { "█", "█" }, { "░░█", "█▄█" }, { "█▄▀", "█░█" }, { "█░░", "█▄▄" }, { "█▀▄▀█", "█░▀░█" }, { "█▄░█", "█░▀█" }, { "█▀█", "█▄█" }, { "█▀█", "█▀▀" }, { "█▀█", "▀▀█" }, { "█▀█", "█▀▄" }, { "█▀", "▄█" }, { "▀█▀", "░█░" }, { "█░█", "█▄█" }, { "█░█", "▀▄▀" }, { "█░█░█", "▀▄▀▄▀" }, { "▀▄▀", "█░█" }, { "█▄█", "░█░" }, { "▀█", "█▄" }, { "  ", "  "},{" █▀█", " █▄█" },{" ▄█", "  █" },{" ▀█", " █▄" },{" ▀██", " ▄▄█" },{" █▄", "  █" },{" █▄", " ▄█" },{" █▀", " ██" },{" ▀█", "  █" },{" █▄█", " █▄█" },{" ██", " ▄█" },},new string[,]{{" █████╗ ", "██╔══██╗", "███████║", "██╔══██║", "██║  ██║", "╚═╝  ╚═╝" },{"██████╗ ", "██╔══██╗", "██████╔╝", "██╔══██╗", "██████╔╝", "╚═════╝ " },{" ██████╗", "██╔════╝", "██║     ", "██║     ", "╚██████╗", " ╚═════╝" },{"██████╗ ", "██╔══██╗", "██║  ██║", "██║  ██║", "██████╔╝", "╚═════╝ " },{"███████╗", "██╔════╝", "█████╗  ", "██╔══╝  ", "███████╗", "╚══════╝" },{"███████╗", "██╔════╝", "█████╗  ", "██╔══╝  ", "██║     ", "╚═╝     " },{" ██████╗ ", "██╔════╝ ", "██║  ███╗", "██║   ██║", "╚██████╔╝", " ╚═════╝ " },{"██╗  ██╗", "██║  ██║", "███████║", "██╔══██║", "██║  ██║", "╚═╝  ╚═╝" },{"██╗", "██║", "██║", "██║", "██║", "╚═╝" },{"     ██╗", "     ██║", "     ██║", "██   ██║", "╚█████╔╝", " ╚════╝ " },{"██╗  ██", "n██║ ██╔╝", "█████╔╝ ", "██╔═██╗ ", "██║  ██╗", "╚═╝  ╚═╝" },{"██╗     ", "██║     ", "██║     ", "██║     ", "███████╗", "╚══════╝" },{"███╗   ███╗", "████╗ ████║", "██╔████╔██║", "██║╚██╔╝██║", "██║ ╚═╝ ██║", "╚═╝     ╚═╝" },{"███╗   ██╗", "████╗  ██║", "██╔██╗ ██║", "██║╚██╗██║", "██║ ╚████║", "╚═╝  ╚═══╝" },{" ██████╗ ", "██╔═══██╗", "██║   ██║", "██║   ██║", "╚██████╔╝", " ╚═════╝ " },{"██████╗ ", "██╔══██╗", "██████╔╝", "██╔═══╝ ", "██║     ", "╚═╝     " },{" ██████╗ ", "██╔═══██╗", "██║   ██║", "██║▄▄ ██║", "╚██████╔╝", " ╚══▀▀═╝ " },{"██████╗ ", "██╔══██╗", "██████╔╝", "██╔══██╗", "██║  ██║", "╚═╝  ╚═╝" },{"███████╗", "██╔════╝", "███████╗", "╚════██║", "███████║", "╚══════╝" },{"████████╗", "╚══██╔══╝", "   ██║   ", "   ██║   ", "   ██║   ", "   ╚═╝   " },{"██╗   ██╗", "██║   ██║", "██║   ██║", "██║   ██║", "╚██████╔╝", " ╚═════╝ " },{"██╗   ██╗", "██║   ██║", "██║   ██║", "╚██╗ ██╔╝", " ╚████╔╝ ", "  ╚═══╝  " },{"██╗    ██╗", "██║    ██║", "██║ █╗ ██║", "██║███╗██║", "╚███╔███╔╝", " ╚══╝╚══╝ " },{"██╗  ██╗", "╚██╗██╔╝", " ╚███╔╝ ", " ██╔██╗ ", "██╔╝ ██╗", "╚═╝  ╚═╝" },{"██╗   ██╗", "╚██╗ ██╔╝", " ╚████╔╝ ", "  ╚██╔╝  ", "   ██║   ", "   ╚═╝   " },{"███████╗", "╚══███╔╝", "  ███╔╝ ", " ███╔╝  ", "███████╗", "╚══════╝" }, {"        ", "        ", "        ", "        ", "        ", "        "},{" ██████╗ ", "██╔═████╗", "██║██╔██║", "████╔╝██║", "╚██████╔╝", " ╚═════╝ " },{" ██", "n███║", "╚██║", " ██║", " ██║", " ╚═╝" },{"██████╗ ", "╚════██╗", " █████╔╝", "██╔═══╝ ", "███████╗", "╚══════╝" },{"██████╗ ", "╚════██╗", " █████╔╝", " ╚═══██╗", "██████╔╝", "╚═════╝ " },{"██╗  ██╗", "██║  ██║", "███████║", "╚════██║", "     ██║", "     ╚═╝" },{"███████╗", "██╔════╝", "███████╗", "╚════██║", "███████║", "╚══════╝" },{" ██████╗ ", "██╔════╝ ", "███████╗ ", "██╔═══██╗", "╚██████╔╝", " ╚═════╝ " },{"███████╗", "╚════██║", "   ██╔╝ ", "  ██╔╝  ", "  ██║   ", "  ╚═╝   " },{" █████╗ ", "██╔══██╗", "╚█████╔╝", "██╔══██╗", "╚█████╔╝", " ╚════╝ " },{" █████╗ ", "██╔══██╗", "╚██████║", " ╚═══██║", " █████╔╝", " ╚════╝ " },},new string[,]{{"a" },{"b" },{"c" },{"d" },{"e" },{"f" },{"g" },{"h" },{"i" },{"j" },{"k" },{"l" },{"m" },{"n" },{"o" },{"p" },{"q" },{"r" },{"s" },{"t" },{"u" },{"v" },{"w" },{"x" },{"y" },{"z" },{" " },{"0" },{"1" },{"2" },{"3" },{"4" },{"5" },{"6" },{"7" },{"8" },{"9" },{"A" },{"B" },{"C" },{"D" },{"E" },{"F" },{"G" },{"H" },{"I" },{"J" },{"K" },{"L" },{"M" },{"N" },{"O" },{"P" },{"Q" },{"R" },{"S" },{"T" },{"U" },{"V" },{"W" },{"X" },{"Y" },{"Z" },
				}
			};
			int[] sizes = { 12, 2, 6 };
			for (int j = 0; j < fonts[font][font == 0 & upperCase ? index + 37 : index, 0].Length; j++)
				for (int i = 0; i < sizes[font]; i++)
				{ Rect(j + x, i + y, fonts[font][font == 0 & upperCase ? index + 37 : index, i][j].ToString(), size: 1, setBG: false, fg: fg); Thread.Sleep(delay); }
			x += fonts[font][index, 0].Length + 1;// Incrementa la x così che la lettera non sovrascriva quelle precedenti
		}
		static int Index(char letter)
		{
			const string alphabet = "abcdefghijklmnopqrstuvwxyz 0123456789";
			for (int i = 0; i < alphabet.Length; i++)
				if (alphabet[i].ToString().ToLower() == letter.ToString().ToLower()) return i;
			return -1;
		}
		static bool UpperCase(char letter)
		{
			string alphabet = "abcdefghijklmnopqrstuvwxyz ".ToUpper();
			for (int i = 0; i < alphabet.Length; i++) if (alphabet[i] == letter) return true;
			return false;
		}
		public static void Clear(int type = 0, int delay = 0)
		{
			Action[] transitions =
			{
				delegate ()
				{
					for(int i = 0; i < Console.WindowHeight; i++)
						for(int j = 0; j < Console.WindowWidth; j++){
							Rect(j, i, " ", size:1, setBG:false); Thread.Sleep(delay); }
				},
				delegate ()
				{
					for(int i = 0; i < Console.WindowWidth; i++)
						for(int j = 0; j < Console.WindowHeight; j++){
							Rect(i, j, " ", size:1, setBG:false); Thread.Sleep(delay); }
				},
				delegate ()
				{
					int[] center = {Console.WindowWidth / 2, Console.WindowHeight / 2};
					for(int radius = 0; radius < (int)Math.Sqrt(Math.Pow(center[0], 2) + Math.Pow(center[1], 2)); radius++)
					{
						for(double angle = 0; angle < Math.PI * 2; angle += Math.PI / 180)
						{
							if(Math.Cos(angle) * radius + center[0] < 0 | Math.Cos(angle) * radius + center[0] >= Console.WindowWidth | Math.Sin(angle) * radius + center[1] < 0 | Math.Sin(angle) * radius + center[1] >= Console.WindowHeight)continue;
							Rect((int)Math.Cos(angle) * radius + center[0], (int)Math.Sin(angle) * radius + center[1], setBG:false, size:1);
							Thread.Sleep(delay);
						}
					}
				},
				delegate ()
				{
					for(int i = 0; i < Console.WindowWidth; i++)
						for(int j = 0; j < Console.WindowHeight; j++)
							if((i + j) % 2 == 0)
							{
								Rect(i, j, setBG:false, size:1);
								Thread.Sleep(delay);
							}

					for(int i = 0; i < Console.WindowWidth; i++)
						for(int j = 0; j < Console.WindowHeight; j++)
							if((i + j) % 2 != 0)
							{
								Rect(i, j, setBG:false, size:1);
								Thread.Sleep(delay);
							}
				}
			};
			transitions[type % transitions.Length]();
		}
	}
}
