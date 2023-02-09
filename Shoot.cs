using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Striker
{
	class Shoot
	{
		public string Direction { get; set; }
		public int Speed { get; set; }
		public int Damage { get; set; }
		public string[,] Map { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public int[] Position { get; set; }
		public Shoot(string[,] map, int width, int height, int[] position, string direction, int speed, int damage)
		{
			Direction = direction;
			Speed = speed;
			Damage = damage;
			Map = map;
			Width = width;
			Height = height;
			Position = position;
			Map[Position[0], Position[1]] = "shoot";
		}
		public void Update()
		{
			Map[Position[0], Position[1]] = "e";
			switch (Direction)
			{
				case "left": Position[0]--;break;
				case "right":Position[0]++;break;
				case "up":Position[0]--;break;
				case "down":Position[0]++;break;
			}
			Map[Position[0], Position[1]] = "shoot";
		}
		public string Collision()
		{
			if (Position[0] < 0 | Position[1] < 0 | Position[0] >= Width | Position[1] >= Height) return "wall";
			return Map[Position[0], Position[1]];
		}
	}
}
