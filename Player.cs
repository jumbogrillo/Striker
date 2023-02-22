using Striker;
using Striker_finale;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Threading;

namespace Striker_Finale
{
	class Player
	{
		static Random Random = new Random();
		private ConsoleKey lastKey;
		private DateTime Timestamp = DateTime.Now;
		private int keycount = 0;
		public int[] Position { get; set; }
		public int Score { get; set; }
		public int Combo { get; set; }
		public int Life { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public int Stamina { get; set; }
		public List<Shoot> Shots { get; set; }
		public Player(int width, int height)
		{
			Width = width;
			Height = height;
			Life = 5;
			Position = new int[] { Width / 2, Height / 2 };
			while (Striker.Map[Position[1] + 1, Position[0]] == "Obs" & Striker.Map[Position[1] -1, Position[0]] == "Obs" & Striker.Map[Position[1], Position[0] + 1] == "Obs" & Striker.Map[Position[1], Position[0] - 1] == "Obs")
            {
				Position[0] = Random.Next(0, Height - 2);
				Position[1] = Random.Next(0,Width - 2);
            }
			Spawn();
			
			Shots = new List<Shoot>();
		}
		public void Move(string[,] map, bool musica,bool lm_multi = false)
		{
			if (Console.KeyAvailable)
			{
				var key = Console.ReadKey(true).Key;
				if(key == lastKey)
                {
					if(!(DateTime.Now > Timestamp.AddSeconds(0.15)))
					{
						keycount++;
						Timestamp = DateTime.Now;
					}
                    else
                    {
						keycount = 0;
                    }
                }
                else keycount = 0;
				if (keycount < 4)
				{
					if (key == ConsoleKey.A | key == ConsoleKey.D | key == ConsoleKey.W | key == ConsoleKey.S) { map[Position[1], Position[0]] = "E"; }
					if (key == ConsoleKey.A & Position[0] > 0) { if (map[Position[1], Position[0] - 1] == "E") Position[0]--; }
					if (key == ConsoleKey.D & Position[0] < Width - 1) { if (map[Position[1], Position[0] + 1] == "E") Position[0]++; }
					if (key == ConsoleKey.W & Position[1] > 0) { if (map[Position[1] - 1, Position[0]] == "E") Position[1]--; }
					if (key == ConsoleKey.S & Position[1] < Height - 1) { if (map[Position[1] + 1, Position[0]] == "E") Position[1]++; }

					if (key == ConsoleKey.LeftArrow) { Shots.Add(new Shoot(map, Width, Height, new int[] { Position[0], Position[1] }, "L", "Pl", 20, 1, true)); if (musica) { Music.Shoot(); } }
					if (key == ConsoleKey.RightArrow) { Shots.Add(new Shoot(map, Width, Height, new int[] { Position[0], Position[1] }, "R", "Pl", 20, 1, true)); if (musica) { Music.Shoot(); } }
					if (key == ConsoleKey.UpArrow) { Shots.Add(new Shoot(map, Width, Height, new int[] { Position[0], Position[1] }, "U", "Pl", 20, 1, true)); if (musica) { Music.Shoot(); } }
					if (key == ConsoleKey.DownArrow) { Shots.Add(new Shoot(map, Width, Height, new int[] { Position[0], Position[1] }, "D", "Pl", 20, 1, true)); if (musica) { Music.Shoot(); } }

					if (key == ConsoleKey.Q) { Shots.Add(new Shoot(map, Width, Height, new int[] { Position[0], Position[1] }, "LU", "Pl", 20, 1, true)); if (musica) { Music.Shoot(); } }
					if (key == ConsoleKey.E) { Shots.Add(new Shoot(map, Width, Height, new int[] { Position[0], Position[1] }, "RU", "Pl", 20, 1, true)); if (musica) { Music.Shoot(); } }
						if (key == ConsoleKey.C) { Shots.Add(new Shoot(map, Width, Height, new int[] { Position[0], Position[1] }, "RD", "Pl", 20, 1, true)); if (musica) { Music.Shoot(); } }
					if (key == ConsoleKey.Z) { Shots.Add(new Shoot(map, Width, Height, new int[] { Position[0], Position[1] }, "LD", "Pl", 20, 1, true)); if (musica) { Music.Shoot(); } }
					if(key == ConsoleKey.P)
					{ Striker.Pause(); 
						if(musica)Music.SoundTrack(true);}
					map[Position[1], Position[0]] = "Pl";
					lastKey = key;
					Timestamp = DateTime.Now;
				}
			}
		}
		private int FindEnemy(List<Enemy> enemies, int[] position)
		{
			for (int i = 0; i < enemies.Count; i++)
				if (position[0] == enemies[i].Position[0] & position[1] == enemies[i].Position[1]) return i;
			return 0;
		}
		public void UpdateShots(string[,] map, List<Enemy> enemies, bool onlineMP=false)
		{
			for (int i = 0; i < Shots.Count; i++)
			{
				string collision = Shots[i].Collision();
				if (collision == "wall" | collision == "Enem" | collision == "Obs")
				{
					if (collision == "Enem")
					{
						Score += 5 * (Combo + 1);
						Combo++;
						Graphic.Word(12 + Width * 2, Graphic.Margin_Top, this.Score > 99 ? "   " : "  ");
						Graphic.Draw_Score(this.Score, 2);
						if (!onlineMP)
						{
							int enemyIndex = FindEnemy(enemies, Shots[i].Position);
							if (enemies[enemyIndex].Life > 1) enemies[enemyIndex].Life--;
							else
							{
								map[enemies[enemyIndex].Position[1], enemies[enemyIndex].Position[0]] = "E";
								enemies[enemyIndex].DeleteAllShotBeforeDeath(map);
								enemies.RemoveAt(enemyIndex);
							}
						}
					}
					else Combo = 0;
					Shots.RemoveAt(i);
				}
				else Shots[i].Update();
			}
		}
		public bool Hit(List<Enemy> enemies)
		{
			foreach (Enemy enemy in enemies)
			{
				if (Distance(enemy.Position[0], enemy.Position[1], Position[0], Position[1]) < 1) return true;
				foreach (Shoot shot in enemy.Shots)
					if (shot.Collision() == "Pl") return true;
			}
			return false;
		}
		public int Distance(int x1, int y1, int x2, int y2) => (int)Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
		private void Spawn()
		{
			Position = new int[] { new Random().Next(0, Width), new Random().Next(0, Height) };
		}
		public void LM_Spawn(Player player, Boolean type)
        {
            if (type)
            {
				player.Position[0] = 1;
				player.Position[1] = 1;
				//for (int i = 1; i <= 5; i++) Striker.Map[1, i + 1] = "E";
				//for (int i = 1; i <= 5; i++) Striker.Map[i + 1, 1] = "E";
            }
            else
            {
				player.Position[0] = Width - 2;
				player.Position[1] = Height - 2;
				//for (int i = 1; i <= 5; i++) Striker.Map[Position[0], Position[1] - i] = "E";
				//for (int i = 1; i <= 5; i++) Striker.Map[Position[0] - i, Position[1]] = "E";
			}
        }
	}
}