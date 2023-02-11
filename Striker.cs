﻿using Striker_finale;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Stricker
{
    internal class Striker
    {
        public const int Height = 25;
        public const int Width = 40;
        public static string[,] Map = new string[Height, Width];
        static bool Playing = true;
        static Player player = new Player(Map, Width, Height);
		static List<Enemy> enemies = new List<Enemy>();
        static ConsoleColor PlayerColor = ConsoleColor.Red;
        
        public static void Main(string[] args)
        {
            //Console.SetWindowPosition(0,0);
            //Console.SetWindowSize(200,200);
            Console.CursorVisible = false;
            Start();
            Console.Clear();
            String[,] Map = new String[Height, Width];
            Graphic.Initialize_Map(Map);
            Graphic.Draw_Score(0);
            Graphic.Draw_Obstacles_Randomly(Map);
            //Graphic.Draw_Frame();
            Graphic.Draw_Map(Map);
            Graphic.Draw_Life_Bar(5);

            Console.ReadKey();
			Graphic.Draw_Frame();            

			while (Playing)
			{
				Graphic.Draw_Map(Map);
				player.Move(Map);
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
                input = Console.ReadKey().Key;
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
                Console.Write(@"Premi un tasto per giocare");
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
                            Console.ForegroundColor = ConsoleColor.Red;
                            PlayerColor = ConsoleColor.Red;
                            Console.WriteLine(@" 



                                          _____
                                         |  __ \                    
                                         | |__) |___  ___ ___  ___  
                                         |  _  // _ \/ __/ __|/ _ \ 
                                         | | \ \ (_) \__ \__ \ (_) |
                                         |_|  \_\___/|___/___/\___/");
                            break;
                        case 1:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            PlayerColor = ConsoleColor.Yellow;
                            Console.WriteLine(@"  
                                           _____ _       _ _       
                                          / ____(_)     | | |      
                                         | |  __ _  __ _| | | ___  
                                         | | |_ | |/ _` | | |/ _ \ 
                                         | |__| | | (_| | | | (_) |
                                          \_____|_|\__,_|_|_|\___/");
                            break;
                        case 2:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            PlayerColor = ConsoleColor.Blue;
                            Console.WriteLine(@"  
                                                ____  _       
                                                |  _ \| |      
                                                | |_) | |_   _ 
                                                |  _ <| | | | |
                                                | |_) | | |_| |
                                                |____/|_|\__,_|");
                            break;
                        case 3:
                            Console.ForegroundColor = ConsoleColor.Green;
                            PlayerColor = ConsoleColor.Green;
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
    }
}
