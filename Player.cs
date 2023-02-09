using System;
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


	}


}