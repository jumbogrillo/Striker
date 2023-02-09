﻿using System;
using System.Threading;

namespace Stricker
{
    internal class Striker
    {
        public const int Height = 25;
        public const int Width = 40;
		public static string[,] Map = new string[Height, Width];
		static bool Playing = true;
		static Player player = new Player(Map, Height, Width, ConsoleColor.White);
        public static void Main(string[] args)
        {
            Console.CursorVisible = false;
			Graphic.Draw_Frame();
			while (Playing)
			{
				player.Move();
			}
			Console.ReadKey();
        }
		static void Start()
		{
			Thread title = new Thread(Title);
			title.Start();
			Thread sottofondo = new Thread(Music.SoundTrack);
			bool musica = false;
			if (Console.ReadKey().Key != ConsoleKey.M)
			{
				sottofondo.Start();
				musica = true;
			}
			title.Abort();
			Console.ResetColor();

			Console.Clear();
			int indexmenu = 0;
			bool next = false;
			ConsoleKey input;
			while (!next)
			{
				Console.SetCursorPosition(40, indexmenu);
				Console.Clear();
				Menu(indexmenu);
				input = Console.ReadKey().Key;
				if (input == ConsoleKey.UpArrow && indexmenu > 0)
				{
					indexmenu--;
				}
				else if (input == ConsoleKey.DownArrow && indexmenu < 2)
				{
					indexmenu++;
				}
				else if (input == ConsoleKey.Enter)
				{
					next = true;
				}
			}
		}
        public static void Menu(int index)
        {
            Console.WriteLine(@"   
     _______.___________.______      __   __  ___  _______ .______      
    /       |           |   _  \    |  | |  |/  / |   ____||   _  \     
   |   (----`---|  |----|  |_)  |   |  | |  '  /  |  |__   |  |_)  |    
    \   \       |  |    |      /    |  | |    <   |   __|  |      /     
.----)   |      |  |    |  |\  \----|  | |  .  \  |  |____ |  |\  \----.
|_______/       |__|    | _| `._____|__| |__|\__\ |_______|| _| `._____|");
            Console.WriteLine();
            for (int i = 0; i < 3; i++)
            {
                if (index == i)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                switch (i)
                {
                    case 0:
                        Console.WriteLine("Gioca");
                        break;
                    case 1:
                        Console.WriteLine("Seleziona Tema");
                        break;
                    case 2:
                        Console.WriteLine("Esci");
                        break;
                }
                Console.ResetColor();
            }
        }
        public static void Title()
        {
            //TITLE LOGO
            int title_color = 0;
            while (true)
            {
                Console.SetCursorPosition(0, 0);
                if (title_color == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }

                Console.WriteLine($@" 
            ╔══════════════════════════════════════════════════════════════════════════════════════════╗
            ║             _______.___________..______       __   __  ___  _______ .______              ║
            ║            /       |           ||   _  \     |  | |  |/  / |   ____||   _  \             ║
            ║           |   (----`---|  |----`|  |_)  |    |  | |  '  /  |  |__   |  |_)  |            ║ 
            ║            \   \       |  |     |      /     |  | |    <   |   __|  |      /             ║
            ║        .----)   |      |  |     |  |\  \----.|  | |  .  \  |  |____ |  |\  \----.        ║ 
            ║        |_______/       |__|     | _| `._____||__| |__|\__\ |_______|| _| `._____|        ║  
            ║                                                                                          ║
            ║                                                                                          ║      
            ║                                                                                          ║              
            ║                                                                                          ║ 
            ║                                                                                          ║
            ╚══════════════════════════════════════════════════════════════════════════════════════════╝


");
                Console.ResetColor();
                Console.SetCursorPosition(45, 9);
                Console.Write("Premi un tasto per giocare");
                Console.SetCursorPosition(42, 10);
                Console.Write("Premi M per giocare senza sonoro");
                title_color++;
                Thread.Sleep(100);
                if (title_color == 2)
                {
                    title_color = 0;
                }
            }
        }
    }
}
