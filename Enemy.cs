using Stricker;
using Striker;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Striker_finale
{
	class Enemy : Player
	{
		public int Speed { get; set; }
		private int Count { get; set; }
		public Enemy(string[,] map, int width, int height, int speed, int life) : base(width, height)
		{
			Count = 0;
			Speed = speed;
			Life = life;
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
				x = vertical ? new Random().Next(0, 2) * (Width - 1) : new Random().Next(0, Width);
				y = vertical ? new Random().Next(0, Height) : new Random().Next(0, 2) * (Height - 1);
			} while (map[y, x] != "E");
			Position = new int[] { x, y };
			map[y, x] = "Enem";
		}
		public void Update(string[,] map, Player player)
		{
			if (10 / Speed > Count) Count++;
			else
			{
				Count = 0;
				Move(map, player);
			}
		}
		public void Move(string[,] map, Player player)
		{
			map[Position[1], Position[0]] = "E";
			// Segue il player
			if (Math.Abs(this.Position[0] - player.Position[0]) > Math.Abs(this.Position[1] - player.Position[1])) AlignX(map, player);
			else AlignY(map, player);
			Shot(map, player);
			map[Position[1], Position[0]] = "Enem";
		}
		private void AlignX(string[,] map, Player player)
		{
			if (player.Position[0] > this.Position[0])
			{
				if (this.Position[0] < Width - 1) if (map[this.Position[1], this.Position[0] + 1] == "E") this.Position[0]++;
			}
			else
			{
				if (this.Position[0] > 0) if (map[this.Position[1], this.Position[0] - 1] == "E") this.Position[0]--;
			}
		} 
		private void AlignY(string[,] map, Player player)
		{

			if (player.Position[1] > this.Position[1])
			{
				if (this.Position[1] < Height - 1) if (map[this.Position[1] + 1, this.Position[0]] == "E") this.Position[1]++;
			}
			else
			{
				if (this.Position[1] > 0) if (map[this.Position[1] - 1, this.Position[0]] == "E") this.Position[1]--;
			}
		}
		private void Shot(string[,] map, Player player)
		{
			if (this.Position[0] == player.Position[0] | this.Position[1] == player.Position[1]) Shots.Add(new Shoot(map, Width, Height, new int[] { Position[0], Position[1] }, DirectionToShot(player), "Enem", 10, 1));
		}
		public void DeleteAllShotBeforeDeath(string[,] map)
		{
			for(int i = 0; i < Shots.Count; i++)map[Shots[i].Position[1], Shots[i].Position[0]] = "E";
			for (int i = 0; i < Shots.Count; i++) Shots.RemoveAt(0);
		}
		private string DirectionToShot(Player player)
		{
			if (this.Position[0] == player.Position[0])// Stessa x direzione o su o giù
			{
				if (this.Position[1] < player.Position[1]) return "D";
				else return "U";
			}
			else if (this.Position[1] == player.Position[1])
			{
				if (this.Position[0] < player.Position[0]) return "R";
				else return "L";
			}
			return "R";
		}
		public void UpdateShots(string[,] map)
		{
			for (int i = 0; i < Shots.Count; i++)
			{
				string collision = Shots[i].Collision();
				if (collision == "wall" | collision == "Pl" | collision == "Obs")
				{
					Shots.RemoveAt(i);
				}
				else Shots[i].Update();
			}
		}
	}
}
