using Striker_finale;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Stricker
{
    internal class Striker
    {
        public const int Height = 25;
        public const int Width = 40;
        public static bool musica = true;
		public static int Level = 1;
		public static string CurrentUser;
        public static Thread sottofondo = new Thread(Music.SoundTrack);
        static Stopwatch Time = new Stopwatch();
		static String[,] Map = new String[Height, Width];
		static Player player = new Player(Width, Height);
		static List<Enemy> enemies = new List<Enemy>();
		static ConsoleColor PlayerColor = ConsoleColor.DarkBlue, EnemyColor = ConsoleColor.Red,
            BGColor = ConsoleColor.DarkGray, ObsColor = ConsoleColor.Gray, ShColor = ConsoleColor.White;
        
        public static void Main(string[] args)
        {
            Console.CursorVisible = false;
			//Database.DrawClassification();
			//Database.Insert(ref CurrentUser);
			//Console.ReadKey();
			Time.Start();
            //Console.SetWindowPosition(0,0);
            //Console.SetWindowSize(140,50);
            Music.Title();
            Start();
            Music.SoundTrack(true);

        StartGame:
            player.Combo = 0;
            Console.Clear();
            for (int i = 0; i < 6; i++)
            {
                if (i % 2 == 0)
                {
                    LevelScreen(ConsoleColor.Yellow);
                }
                else
                {
                    LevelScreen(ConsoleColor.DarkYellow);
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Thread.Sleep(300);
            }
            Music.Sound("level");
            Console.Clear();    
			Graphic.Initialize_Map(Map);
            Graphic.Draw_Obstacles_Randomly(Map);
            Graphic.Draw_Map(Map, BGColor, EnemyColor, PlayerColor, ObsColor, ShColor);
            Graphic.Draw_Life_Bar(5);
            Graphic.Draw_Score(player.Score, 0);   
			Graphic.Draw_Frame();
			while (player.Life > 0)
			{
                if (Time.ElapsedMilliseconds % 5000 < 100)enemies.Add(new Enemy(Map, Width, Height, 2, 1));
                if (player.Combo > 0)
                {
                    Console.SetCursorPosition(102, 13);
                    Console.Write($"Combo X{player.Combo}");
                    if (player.Combo % 5 == 0 && player.Life < 5)
                    {
                        player.Life++;
                        player.Combo++;
                        Graphic.Draw_Life_Bar(player.Life);
                        Music.Title();
                        Console.SetCursorPosition(90, 16);
                        Console.WriteLine(" ");
                    }
                }
                if (player.Hit(enemies))
				{
					player.Life--;
					Graphic.Draw_Life_Bar(player.Life);
				}
				foreach (Enemy enemy in enemies)
				{
					enemy.Update(Map, player);
					enemy.UpdateShots(Map);
				}
                if(player.Score > 99)
                {
                    Level++;
					Reset();
                    goto StartGame;
                }
				Graphic.Draw_Map(Map, BGColor, EnemyColor, PlayerColor, ObsColor, ShColor);
				player.UpdateShots(Map, enemies);
                player.Move(Map, musica);
			}
            GameOver();
			Database.Update(CurrentUser, 100 * (Level - 1) + player.Score);
        }
		static void Start()
		{
			Thread title = new Thread(Title);
			title.Start();
			musica = true;
            ConsoleKey startinput;
            bool startchoose = false;
            while (!startchoose)
            {
                startinput = Console.ReadKey(false).Key;
                if (startinput == ConsoleKey.M)
                {
                    if (!musica)
                    {
                        musica = true;
                        Console.SetCursorPosition(30, 14);
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write(@"           
           .=%@@.       .:    
         -#@@@@@.       #@%:  
      :*@@@@@@@@.   .*+  =@@- 
+++++%@@@@@@@@@@.    +@@: :@@:
@@@@@@@@@@@@@@@@.-@#. =@@  *@#
@@@@@@@@@@@@@@@@. *@*  @@- -@@
@@@@@@@@@@@@@@@@..%@= .@@: =@%
@@@@@@@@@@@@@@@@..+- .#@*  %@+
     =#@@@@@@@@@.   .@@+  *@% 
       .+%@@@@@@.    .  :%@#  
          :*@@@@.       *#-   ");
                    }
                    else
                    {
                        musica = false;
                        Console.SetCursorPosition(30, 14);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(@"  
   :%+               .     
   .*@+    .+%-       :+@#:   
     .*@+.+@%@+   .#*-   +@*  
       =*@*.=@+    .=%%-  -@# 
#%%%%%@%:.*@*+= .*=   *@-  =@+
@%         .*@+. -%@.  %@   @@
@%          :+*@+.=@-  *@.  %@
@%=====:    =@+.*@*=  .@%   @%
-=====+@#-  =@+  .*@+.#%.  *@:
        =%%=+@+    =*@*.  *@= 
          -#@@=   .#*-  :+@#:  
             ");
                    }
                    
                }
                else
                {
                    if (musica)
                    {
                        Music.SoundTrack();
                    }
                    startchoose = true;
                }
            }
			title.Abort();
			Console.ResetColor();
            Console.CursorVisible = false;

        Menu:
            int index = 0;
            int colorindex = 0;
            bool backmenu = false;
            bool next = false;
            ConsoleKey input;
            while (!next)
            {
                Console.SetCursorPosition(40, index);
                Console.Clear();
                Menu(index);
                input = Console.ReadKey(false).Key;
                if (input == ConsoleKey.UpArrow && index > 0)
                {
                    index--;
                }
                else if (input == ConsoleKey.DownArrow && index < 2)
                {
                    index++;
                }
                else if (input == ConsoleKey.Enter)
                {
                    next = true;
                    while (!backmenu)
                    {
                        backmenu = false;
                        switch (index)
                        {
                            case 0:
                                backmenu = true;
                                next = true;
                                break;
                            case 1:
                                SelectionThemes(colorindex);
                                input = Console.ReadKey().Key;
                                if (input == ConsoleKey.UpArrow && colorindex > 0)
                                {
                                    colorindex--;
                                }
                                else if (input == ConsoleKey.DownArrow && colorindex < 3)
                                {
                                    colorindex++;
                                }
                                else if (input == ConsoleKey.Enter)
                                {
                                    backmenu = true;
                                    Menu(index);
                                    next = false;
                                    goto Menu;
                                }
                                SelectionThemes(colorindex);
                                Console.ResetColor();
                                break;
                            case 2:
                                Environment.Exit(0);
                                break;
                        }
                    }
                }
            }
        }
		static void Reset()
		{
			Map = new String[Height, Width];
			Graphic.Initialize_Map(Map);
			Graphic.Draw_Obstacles_Randomly(Map);
			player = new Player(Width, Height);
			enemies = new List<Enemy>(); 
		}
        public static void Menu(int index)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(@"   
                                 _______.___________.______      __   __  ___  _______ .______      
                                /       |           |   _  \    |  | |  |/  / |   ____||   _  \     
                               |   (----`---|  |----|  |_)  |   |  | |  '  /  |  |__   |  |_)  |    
                                \   \       |  |    |      /    |  | |    <   |   __|  |      /     
                            .----)   |      |  |    |  |\  \----|  | |  .  \  |  |____ |  |\  \----.
                            |_______/       |__|    | _| `._____|__| |__|\__\ |_______|| _| `._____|");
            Console.WriteLine();
            Console.ResetColor();
            for (int i = 0; i < 3; i++)
            {
                if (index == i)
                {
                    Console.ForegroundColor = PlayerColor;
                }
                switch (i)
                {
                    case 0:
            Console.WriteLine(@"    
                        ╔═══╗               
                        ║╔═╗║               
                        ║║ ╚╝╔╗╔══╗╔══╗╔══╗ 
                        ║║╔═╗╠╣║╔╗║║╔═╝╚ ╗║ 
                        ║╚╩═║║║║╚╝║║╚═╗║╚╝╚╗
                        ╚═══╝╚╝╚══╝╚══╝╚═══╝");
                        break;
                    case 1:
            Console.WriteLine(@"
                        ╔═══╗    ╔╗                             ╔════╗             
                        ║╔═╗║    ║║                             ║╔╗╔╗║             
                        ║╚══╗╔══╗║║ ╔══╗╔═══╗╔╗╔══╗╔═╗ ╔══╗     ╚╝║║╚╝╔══╗╔╗╔╗╔══╗ 
                        ╚══╗║║╔╗║║║ ║╔╗║╠══║║╠╣║╔╗║║╔╗╗╚ ╗║       ║║  ║╔╗║║╚╝║╚ ╗║ 
                        ║╚═╝║║║═╣║╚╗║║═╣║║══╣║║║╚╝║║║║║║╚╝╚╗     ╔╝╚╗ ║║═╣║║║║║╚╝╚╗
                        ╚═══╝╚══╝╚═╝╚══╝╚═══╝╚╝╚══╝╚╝╚╝╚═══╝     ╚══╝ ╚══╝╚╩╩╝╚═══╝");

                        break;
                    case 2:
        Console.WriteLine(@"
                        ╔═══╗          
                        ║╔══╝          
                        ║╚══╗╔══╗╔══╗╔╗
                        ║╔══╝║══╣║╔═╝╠╣
                        ║╚══╗╠══║║╚═╗║║
                        ╚═══╝╚══╝╚══╝╚╝");
            break;
                }
                Console.ResetColor();
            }
        }
        public static void Title()
        {
            //Title LOGO
            int Title_color = 0;
            Console.ForegroundColor= ConsoleColor.Yellow;
            while (true)
            {
                Console.SetCursorPosition(0, 0);
                if (Title_color == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
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
                Console.Write(@"Premi un tasto per giocare");
                Console.SetCursorPosition(42, 10);
                Console.Write("Premi M per giocare senza sonoro");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Title_color++;
                Thread.Sleep(200);
                if (Title_color == 2)
                {
                    Title_color = 0;
                }
            }
        }
        public static void SelectionThemes(int index)
        {
            Console.Clear();
            for (int i = 0; i < 4; i++)
            {
                if (i == index)
                {
                    Console.SetCursorPosition(36, 5);
                    Console.WriteLine(@"
        (~ _ | _ _ . _  _  _   .|   _ _ | _  _ _    _  _  _  .|  _|_   _    _  _  _ _ _  _  _  _  _ . _    
        _)(/_|(/_/_|(_)| |(_|  ||  (_(_)|(_)| (/_  |_)(/_|   ||   ||_|(_)  |_)(/_| _\(_)| |(_|(_|(_||(_)...
                                                   |                       |                   _| _|      ");

                    if (i > 0)
                    {
                        Console.WriteLine(@"  
                                                     /\ 
                                                    |/\|");
                    }
                    switch (i)
                    {
                        case 0:
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            PlayerColor = ConsoleColor.DarkBlue;
                            EnemyColor = ConsoleColor.Red;
                            BGColor = ConsoleColor.DarkGray;
                            ObsColor = ConsoleColor.Gray;
                            ShColor = ConsoleColor.White;
                            Console.WriteLine(@"



                                               ____  _       
                                              |  _ \| |      
                                              | |_) | |_   _ 
                                              |  _ <| | | | |
                                              | |_) | | |_| |
                                              |____/|_|\__,_| ");
                            break;
                        case 1:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            PlayerColor = ConsoleColor.DarkYellow;
                            EnemyColor = ConsoleColor.Gray;
                            BGColor = ConsoleColor.DarkBlue;
                            ObsColor = ConsoleColor.DarkGray;
                            ShColor = ConsoleColor.Black;
                            Console.WriteLine(@"  
                                           _____ _       _ _       
                                          / ____(_)     | | |      
                                         | |  __ _  __ _| | | ___  
                                         | | |_ | |/ _` | | |/ _ \ 
                                         | |__| | | (_| | | | (_) |
                                          \_____|_|\__,_|_|_|\___/");
                            break;
                        case 2:
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            PlayerColor = ConsoleColor.DarkRed;
                            EnemyColor = ConsoleColor.DarkMagenta;
                            BGColor = ConsoleColor.Gray;
                            ObsColor = ConsoleColor.DarkGray;
                            ShColor = ConsoleColor.Black;
                            Console.WriteLine(@" 
                                          _____
                                         |  __ \                    
                                         | |__) |___  ___ ___  ___  
                                         |  _  // _ \/ __/ __|/ _ \ 
                                         | | \ \ (_) \__ \__ \ (_) |
                                         |_|  \_\___/|___/___/\___/");
                            break;
                           
                        case 3:
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            PlayerColor = ConsoleColor.DarkGreen;
                            EnemyColor = ConsoleColor.DarkMagenta;
                            BGColor = ConsoleColor.Gray;
                            ObsColor = ConsoleColor.DarkGray;
                            ShColor = ConsoleColor.Black;
                            Console.WriteLine(@"
                                         __      __          _      
                                         \ \    / /         | |     
                                          \ \  / /__ _ __ __| | ___ 
                                           \ \/ / _ \ '__/ _` |/ _ \
                                            \  /  __/ | | (_| |  __/
                                             \/ \___|_|  \__,_|\___|");
                            break;
                    }
                    Console.ResetColor();
                    if (i < 3)
                    {
                        Console.WriteLine(@"       


                                                   |\/|
                                                    \/ ");
                    }

                }
            }
        }
        public static void LevelScreen(ConsoleColor color)
        {
            Graphic.Word(42, 3, "Level", font:2,fg:color);
            Graphic.Word(60, 13, Level.ToString(),fg:color);
        }
        public static void GameOver()
        {
            Console.Clear();
            Graphic.Word(11, 3, "Game Over");
            if (musica)
            {
                Music.GameOver();
                Thread.Sleep(7000);
            }
            else
            {
                Console.ReadKey();
            }
        }

    }
}
