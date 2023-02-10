using Stricker;
using Striker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Striker_finale
{
	class Enemy : Player
	{
		public Enemy(string[,] map, int width, int height) : base(map, width, height)
		{
			Position = new int[2];
			Shots = new List<Shoot>();
			Width = width;
			Height = height;
			Spawn(map);
		}
		private void Spawn(string[,] map)
		{
			int x, y;
			bool vertical;
			do
			{
				vertical = new Random().Next(0, 2) == 1;
				x = vertical ? new Random().Next(0, 2) * Width : new Random().Next(0, Width);
				y = vertical ? new Random().Next(0, Height) * Height : new Random().Next(0, 2) * Height;
			} while (map[y, x] != "E");
			Position = new int[] { x, y };
			map[y, x] = "Enem";
		}
		public void Move(string[,] map, Player player)
		{
			if (Math.Abs(Position[0] - player.Position[0]) > Math.Abs(Position[1] - player.Position[1]))// La distanza delle y è minore
			{
				if (Position[1] < player.Position[1] & Position[1] + 1 < Height & map[Position[1] + 1, Position[0]] == "E") Position[1]++;
				else if (Position[1] > 0 & map[Position[1] - 1, Position[0]] == "E") Position[1]--;
			}
			else if (Position[0] < player.Position[0] & Position[0] + 1 < Width & map[Position[1], Position[0] - 1] == "E") Position[0]++;
			else if (Position[0] > 0 & map[Position[1], Position[0] + 1] == "E") Position[0]--;
		}
	}
}
