using Striker;
using Striker_finale;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Stricker
{
	class Player
	{
		public int[] Position { get; set; }
		public int Score { get; set; }
		public int Combo { get; set; }
		public int Speed { get; set; }
		public int Life { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public List<Shoot> Shots { get; set; }
		public Player(string[,] map, int width, int height)
		{
			Width = width;
			Height = height;
			Life = 5;
			Position = new int[] { Width / 2, Height / 2 };
			map[Position[1], Position[0]] = "Pl";
			Shots = new List<Shoot>();
		}
		public void Move(string[,] map, bool musica)
		{
			if (Console.KeyAvailable)
			{
				var key = Console.ReadKey(true).Key;
				if (key == ConsoleKey.A | key == ConsoleKey.D | key == ConsoleKey.W | key == ConsoleKey.S) { map[Position[1], Position[0]] = "E"; }
				if (key == ConsoleKey.A & Position[0] > 0) { if (map[Position[1], Position[0] - 1] == "E") Position[0]--; }
				if (key == ConsoleKey.D & Position[0] < Width - 1) { if (map[Position[1], Position[0] + 1] == "E") Position[0]++; }
				if (key == ConsoleKey.W & Position[1] > 0) { if (map[Position[1] - 1, Position[0]] == "E") Position[1]--; }
				if (key == ConsoleKey.S & Position[1] < Height - 1) { if (map[Position[1] + 1, Position[0]] == "E") Position[1]++; }
				if (key == ConsoleKey.LeftArrow) { Shots.Add(new Shoot(map, Width, Height, new int[] { Position[0], Position[1]}, "L", "Pl", 1, 1)); if (musica) { Music.Shoot();} }
				if (key == ConsoleKey.RightArrow) { Shots.Add(new Shoot(map, Width, Height, new int[] {Position[0], Position[1]}, "R", "Pl", 1, 1)); if (musica) { Music.Shoot(); } }
				if (key == ConsoleKey.UpArrow) { Shots.Add(new Shoot(map, Width, Height, new int[] {Position[0], Position[1]}, "U", "Pl", 1, 1)); if (musica) { Music.Shoot(); } }
				if (key == ConsoleKey.DownArrow) { Shots.Add(new Shoot(map, Width, Height, new int[] {Position[0], Position[1]}, "D", "Pl", 1, 1)); if (musica) { Music.Shoot(); } }
				map[Position[1], Position[0]] = "Pl";
			}
		}
		public void UpdateShots()
		{
			for (int i = 0; i < Shots.Count; i++)
			{
				string collision = Shots[i].Collision();
				if (collision == "wall" | collision == "Enem" | collision == "Obs") Shots.RemoveAt(i);
				else Shots[i].Update();
			}
		}
		public bool Hit(List<Enemy> enemies)
		{
			foreach (Enemy enemy in enemies)
			{
				if (Distance(enemy.Position[0], enemy.Position[1], Position[0], Position[1]) <= 1) return true;
				foreach (Shoot shot in enemy.Shots)
					if (shot.Collision() == "Pl") return true;
			}
			return false;
		}
		private int Distance(int x1, int y1, int x2, int y2) => (int)Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
	}
}