using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stricker
{
	internal class Graphic
	{
		const int Margin_Top = 3;
		const int Margin_Left = Margin_Top * 2;

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

		public static void Draw_People()
		{

		}

		public static void Draw_Frame(int width = 40, int height = 25, int margin_top = Margin_Top, int margin_left = Margin_Left)
		{
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
		}
	}
}
