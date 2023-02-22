using Striker_finale;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Striker_Finale
{
    public class Striker
    {
        public static int[] Position = new int[2]; //per LM_MULTI
        public static String Direction, Alliance;
        public static int Speed, Damage;

        public const int Height = 25;
        public const int Width = 40;
        public static bool musica = true;
        public static int Level = 1;
        public static string CurrentUser;
        public static Thread sottofondo = new Thread(Music.SoundTrack);
        static Stopwatch Time = new Stopwatch();
        public static String[,] Map = new String[Height, Width];
        static Player player;
        static List<Enemy> enemies = new List<Enemy>();
        static ConsoleColor PlayerColor = ConsoleColor.DarkRed, EnemyColor = ConsoleColor.DarkMagenta,
            BGColor = ConsoleColor.Gray, ObsColor = ConsoleColor.DarkGray, ShColor = ConsoleColor.Black;

        public static void Main(string[] args)
        {
            //Online_Multiplayer();
            Local_Multiplayer_Start();
            Console.Title = "Striker";
            Console.CursorVisible = false;
            MultiplayerLocale.Type = false;
            Local_Multiplayer_Start();



            MultiplayerLocale.Height = Height;
            MultiplayerLocale.Width = Width;
            Graphic.Initialize_Map(Map);
            Graphic.Draw_Obstacles_Randomly(Map);
            MultiplayerLocale.Initialize_Get(Map);
            Console.ReadKey();

            Console.Title = "Striker";
            Console.CursorVisible = false;
            Console.SetBufferSize(140, 70);
            Console.SetWindowSize(140, 70);
            Console.SetWindowPosition(0, 0);
            Database.DrawClassification();
            Console.CursorVisible = true;
            Database.Register(ref CurrentUser);
            Console.SetWindowSize(120, 30);
            Console.SetBufferSize(120, 30);
          //Database.Insert(ref CurrentUser);
            Console.CursorVisible = false;
            Console.Clear();
            Time.Start();
            Music.Title();
            Console.SetBufferSize(120, 30);
            Start();
            Music.SoundTrack(true);

        StartGame:
            Graphic.Initialize_Map(Map);
            Graphic.Draw_Obstacles_Randomly(Map);
            player.Combo = 0;
            Graphic.Clear(1);
            for (int i = 0; i < 6; i++)
            {
                LevelScreen(i % 2 == 0 ? ConsoleColor.Yellow : ConsoleColor.DarkYellow);
                Console.BackgroundColor = ConsoleColor.Black;
                Thread.Sleep(300);
            }
            if (musica) Music.Sound("level");
            Graphic.Clear();
            Console.Clear();
            Graphic.Draw_Map(Map, BGColor, EnemyColor, PlayerColor, ObsColor, ShColor);
            Graphic.Draw_Life_Bar(player.Life);
            Graphic.Draw_Score(player.Score, 2);
            Graphic.Draw_Frame();
            while (player.Life > 0)
            {
                Console.SetBufferSize(140, 70);
                if (Time.ElapsedMilliseconds % 5000 < 100) enemies.Add(new Enemy(Map, Width, Height, 2, 1));
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
                if (player.Score > 99)
                {
                    Level++;
                    Reset();
                    goto StartGame;
                }
                Graphic.Draw_Map(Map, BGColor, EnemyColor, PlayerColor, ObsColor, ShColor);//Width / 2 - player.Position[0], Height / 2 - player.Position[1], 
                player.UpdateShots(Map, enemies);
                player.Move(Map, musica);
            }
            GameOver();
            Database.Update(CurrentUser, 100 * (Level - 1) + player.Score, Time.ElapsedMilliseconds);
            Database.DrawClassification();
            Console.ReadKey();
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
                if (input == ConsoleKey.UpArrow && index >= 2)
                {
                    index = index - 2;
                }
                else if (input == ConsoleKey.DownArrow && index < 2)
                {
                    index += 2;
                }
                else if (input == ConsoleKey.LeftArrow && index >= 1)
                {
                    index--;
                }
                else if (input == ConsoleKey.RightArrow && index < 3)
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
                                Console.Clear();
                                SelectionThemes(colorindex);
                                Console.ResetColor();
                                break;
                            case 2:
                                Graphic.Clear();
                                Commands();
                                Console.ReadKey();
                                goto Menu;
                            case 3:
                                Environment.Exit(0);
                                break;
                        }
                    }
                }
            }
        }
        static void Reset()
        {
            int lastLife = player.Life;
            Map = new String[Height, Width];
            Graphic.Initialize_Map(Map);
            Graphic.Draw_Obstacles_Randomly(Map);
            player = new Player(Width, Height);
            player.Life = lastLife;
            enemies = new List<Enemy>();
            Map[player.Position[1], player.Position[0]] = "Pl";
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
            string word = "";
            for (int i = 0; i < 4; i++)
            {

                switch (i)
                {
                    case 0:
                        word = "Gioca";
                        if (index == i) Graphic.Word(Width - 33, 9, word, 2, PlayerColor);
                        else Graphic.Word(Width - 33, 9, word, 2);
                        break;
                    case 1:
                        word = "Temi";
                        if (index == i) Graphic.Word(Width + 33, 9, word, 2, PlayerColor);
                        else Graphic.Word(Width + 33, 9, word, 2);
                        break;
                    case 2:
                        word = "Comandi";
                        if (index == i) Graphic.Word(Width - 33, 20, word, 2, PlayerColor);
                        else Graphic.Word(Width - 33, 20, word, 2);
                        break;
                    case 3:
                        word = "Esci";
                        if (index == i) Graphic.Word(Width + 35, 20, word, 2, PlayerColor);
                        else Graphic.Word(Width + 35, 20, word, 2);
                        break;
                }

                Console.ResetColor();
            }
        }
        public static void Title()
        {
            //Title LOGO
            int Title_color = 0;
            Console.ForegroundColor = ConsoleColor.Yellow;
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
        static void Commands()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(@"

                         Movimento:                                                    Sparo:
                                                        
                     .----------------.                                          .----------------. 
                    | .--------------. |                                        | .--------------. |
                    | | _____  _____ | |                                        | |       _      | |
                    | ||_   _||_   _|| |                                        | |      / \     | |
                    | |  | | /\ | |  | |                                        | |     /   \    | |
                    | |  | |/  \| |  | |                                        | |      | |     | |
                    | |  |   /\   |  | |                                        | |      | |     | |
                    | |  |__/  \__|  | |                                        | |      |_|     | |
                    | |              | |                                        | |              | |
                    | '--------------' |                                        | '--------------' |
                     '----------------'                                          '----------------' 
 .----------------.  .----------------.  .----------------.  .----------------.  .----------------.  .----------------. 
| .--------------. || .--------------. || .--------------. || .--------------. || .--------------. || .--------------. |
| |      __      | || |    _______   | || |  ________    | || |       _      | || |       _      | || |      _       | |
| |     /  \     | || |   /  ___  |  | || | |_   ___ `.  | || |      / /     | || |      | |     | || |     \ \      | |
| |    / /\ \    | || |  |  (__ \_|  | || |   | |   `. \ | || |     / /      | || |      | |     | || |      \ \     | |
| |   / ____ \   | || |   '.___`-.   | || |   | |    | | | || |    < <       | || |     _| |_    | || |       > >    | |
| | _/ /    \ \_ | || |  |`\____) |  | || |  _| |___.' / | || |     \ \      | || |     \   /    | || |      / /     | |
| ||____|  |____|| || |  |_______.'  | || | |________.'  | || |      \_\     | || |      \_/     | || |     /_/      | |
| |              | || |              | || |              | || |              | || |              | || |              | |
| '--------------' || '--------------' || '--------------' || '--------------' || '--------------' || '--------------' |
 '----------------'  '----------------'  '----------------'  '----------------'  '----------------'  '----------------' 

");
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
            Graphic.Word(42, 3, "Level", 2, fg: color);
            Graphic.Word(62, 13, Level.ToString(), 2, fg: color);
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
        public static void Pause()
        {
            if(musica)Music.SoundTrack();
            bool backmenu = false, next = false;
            int colorindex = 0;
            int index = 0;
            ConsoleKey input;
            Console.Clear();
            while (!next)
            {
            PauseMenuWritings:
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(@"   
                              _______.___________.______      __   __  ___  _______ .______      
                             /       |           |   _  \    |  | |  |/  / |   ____||   _  \     
                            |   (----`---|  |----|  |_)  |   |  | |  '  /  |  |__   |  |_)  |    
                             \   \       |  |    |      /    |  | |    <   |   __|  |      /     
                         .----)   |      |  |    |  |\  \----|  | |  .  \  |  |____ |  |\  \----.
                         |_______/       |__|    | _| `._____|__| |__|\__\ |_______|| _| `._____|");
                //Console.WriteLine();
                Console.ResetColor();
                string word = "";
                for (int i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0:
                            word = "Resume";
                            if (index == i) Graphic.Word(Width - 33, 9, word, 2, PlayerColor);
                            else Graphic.Word(Width - 33, 9, word, 2);
                            break;
                        case 1:
                            word = "Temi";
                            if (index == i) Graphic.Word(Width + 33, 9, word, 2, PlayerColor);
                            else Graphic.Word(Width + 33, 9, word, 2);
                            break;
                        case 2:
                            word = "Comandi";
                            if (index == i) Graphic.Word(Width - 33, 20, word, 2, PlayerColor);
                            else Graphic.Word(Width - 33, 20, word, 2);
                            break;
                        case 3:
                            word = "Esci";
                            if (index == i) Graphic.Word(Width + 35, 20, word, 2, PlayerColor);
                            else Graphic.Word(Width + 35, 20, word, 2);
                            break;
                    }
                }
                backmenu = false;
                Console.ResetColor();
                Console.SetCursorPosition(40, index);
                input = Console.ReadKey(false).Key;
                if (input == ConsoleKey.UpArrow && index >= 2)
                {
                    index = index - 2;
                }
                else if (input == ConsoleKey.DownArrow && index < 2)
                {
                    index += 2;
                }
                else if (input == ConsoleKey.LeftArrow && index >= 1)
                {
                    index--;
                }
                else if (input == ConsoleKey.RightArrow && index < 3)
                {
                    index++;
                }
                else if (input == ConsoleKey.Enter)
                {
                    while (!backmenu)
                    {
                        backmenu = false;
                        switch (index)
                        {
                            case 0:
                                backmenu = true;
                                Graphic.Clear();
                                Graphic.Draw_Life_Bar(player.Life);
                                Graphic.Draw_Score(player.Score, 2);
                                Graphic.Draw_Frame();
                                return;
                            case 1:
                                Console.Clear();
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
                                    Graphic.Clear();
                                    goto PauseMenuWritings;
                                }
                                Console.ResetColor();
                                if(input == ConsoleKey.Enter) 
                                { index = 0; 
                                  backmenu = true;}
                                break;
                            case 2:
                                Graphic.Clear();
                                Commands();
                                Console.ReadKey();
                                Graphic.Clear();
                                goto PauseMenuWritings;
                            case 3:
                                Environment.Exit(0);
                                break;
                        }

                    }
                }
            }
        }

        public static void Local_Multiplayer_Start()
        {
            MultiplayerLocale.Type = true;
            MultiplayerLocale.Height = Height;
            MultiplayerLocale.Width = Width;
            Graphic.Initialize_Map(Map);
            //Enemy enemy = new Enemy(Map, Width, Height, 0, 5);
            Player player = new Player(Width, Height);
            Handshake();
            Console.ReadKey();
            Time.Start();
            player.LM_Spawn(player, MultiplayerLocale.Type);
            Graphic.Draw_Map(Map, BGColor, EnemyColor, PlayerColor, ObsColor, ShColor);
            Graphic.Draw_Life_Bar(player.Life);
            Graphic.Draw_Score(player.Score, 2);
            Graphic.Draw_Frame();
            //update = new Thread(Update);
            //update.Start();

            while (player.Life > 0)
            {
                MultiplayerLocale multiLocal = new MultiplayerLocale();
                Console.SetBufferSize(140, 70);
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
                Set_Shoots(Position,ref Direction,ref Alliance,ref Speed,ref Damage);
                player.Move(Map, musica);
                Update(Map, player, Position, Direction, Alliance, Speed, Damage);
                player.LM_Shoots(Map);
                //if (Position[0] != -1) enemy.LM_Shot(Map, Position, Direction, Alliance, Speed, Damage);
                Graphic.Draw_Map(Map, BGColor, EnemyColor, PlayerColor, ObsColor, ShColor);//Width / 2 - player.Position[0], Height / 2 - player.Position[1], 
                player.UpdateShots(Map, enemies);
            }
        }

        

        public static void Handshake()
        {
            Riprova:
            if (MultiplayerLocale.Type)
            {
                Graphic.Draw_Obstacles_Randomly(Map);
                if(!MultiplayerLocale.Initialize_Set(Map))
                {
                    goto Riprova;
                }
            }
            else
            {
                if (!MultiplayerLocale.Initialize_Get(Map))
                {
                    goto Riprova;
                }
            }
        }

                }
            }
            goto Menu;

        }
        static void Update(String[,] Map, Player player,int[] Position, String Direction, String Alliance, int Speed, int Damage)
        {
            MultiplayerLocale.Update(player.Position[0], player.Position[1], Position[0], Position[1], Direction, Alliance, Speed, Damage);
            MultiplayerLocale.Enemy_Update(Map);
        }

        public static void Set_Shoots(int[] Position, ref String Direction, ref String Alliance, ref int Speed, ref int Damage)
        {
            MultiplayerLocale.shx = -1;
            MultiplayerLocale.shy = -1;
            MultiplayerLocale.dir = "//";
            MultiplayerLocale.alli = "//";
            MultiplayerLocale.speed = -1;
            MultiplayerLocale.dam = -1;

            Position[0] =MultiplayerLocale.shx;
            Position[1] =MultiplayerLocale.shy;
            Direction = MultiplayerLocale.dir;
            Alliance = MultiplayerLocale.alli;
            Speed = MultiplayerLocale.speed;
            Damage = MultiplayerLocale.dam;
        }        
    }
}
