using Stricker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Striker
{
	public class Shoot
	{
		public string Direction { get; set; }
		public string Alliance { get; set; }
		public int Speed { get; set; }
		public int Damage { get; set; }
		public string[,] Map { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public int[] Position { get; set; }
		public string StateBeforeShot { get; set; }
		public int Count { get; set; }
		public Shoot(string[,] map, int width, int height, int[] position, string direction, string alliance, int speed, int damage)
		{
			Direction = direction;
			Speed = speed;
			Damage = damage;
			Map = map;
			Width = width;
			Height = height;
			Position = position;
			Alliance = alliance;
			StateBeforeShot = Map[position[1], position[0]];
		}
		public void Update()
		{
			if (Speed > Count) Count++;
			else
			{
				Count = 0;
				Move();
			}
		}
		public void Move()
		{
			if( Map[Position[1], Position[0]] == "Sh") Map[Position[1], Position[0]] = "E";
			switch (Direction)
			{
				case "L": this.Position[0]--; break;
				case "R": this.Position[0]++; break;
				case "U": this.Position[1]--; break;
				case "D": this.Position[1]++; break;
			}
			StateBeforeShot = Collision();
			if(StateBeforeShot == "E")Map[Position[1], Position[0]] = "Sh";
		}
		public string Collision()
		{
			if (Position[0] < 0 | Position[1] < 0 | Position[0] >= Width | Position[1] >= Height) return "wall";
			return Map[Position[1], Position[0]];
		}
	}
}
