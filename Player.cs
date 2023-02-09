using Striker;
using System;
using System.Collections.Generic;

namespace Stricker
{
	class Player
	{
		private int[] position = new int[2];
		private ConsoleColor color;
		private int score;
		private int combo;
		private int speed;
		private int life;

		public int[] Position { get => position; set => position = value; }
		public ConsoleColor Color { get => color; set => color = value; }
		public int Score { get => score; set => score = value; }
		public int Combo { get => combo; set => combo = value; }
		public int Speed { get => speed; set => speed = value; }
		public int Life { get => life; set => life = value; }
		public string[,] Map { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public List<Shoot> Shooting { get; set; } 
		public Player(string[,] map, int width, int height, ConsoleColor bg)
		{
			Color = bg;
			Map = map;
			Width = width;
			Height = height;
			Position = new int[] { Width / 2, Height / 2 };
			Map[Position[0], Position[1]] = "player";
			Shooting = new List<Shoot>();
		}
		public void Move()
		{
			if (Console.KeyAvailable)
			{
				var key = Console.ReadKey(true).Key;
				if (key == ConsoleKey.A | key == ConsoleKey.D | key == ConsoleKey.D | key == ConsoleKey.S) Map[Position[0], Position[1]] = "e";
				if (key == ConsoleKey.A & Position[0] > 0) Position[0]--;
				if (key == ConsoleKey.D & Position[0] < Width - 1) Position[0]++;
				if (key == ConsoleKey.D & Position[1] > 0) Position[1]--;
				if (key == ConsoleKey.S & Position[1] < Height - 1) Position[1]++;
				if(key == ConsoleKey.LeftArrow)Shooting.Add(new Shoot(Map, Width, Height, Position, "left", 1, 1));
				Map[Position[0], Position[1]] = "player";
			}
		}
		public void UpdateShoot()
		{
			for(int i = 0; i < Shooting.Count; i++)
			{
				string collision = Shooting[i].Collision();
				if (collision == "e") Shooting[i].Update();
				else if (collision == "obs" | collision == "wall" | collision == "enemy") Shooting.RemoveAt(i);
			}
		}
	}


}