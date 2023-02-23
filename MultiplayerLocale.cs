using Striker;
using Striker_finale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Striker_finale
{
    class MultiplayerLocale
    {
        public static List<Shoot> Shots { get; set; }
        public static int Width;
        public static int Height;
        public static Boolean Type = true;
        static StreamReader sharedFileR;
        static StreamWriter sharedFile;
        static int X = 23, Y = 38;
        public static int shx = -1, shy = -1, speed = -1, dam = -1;
        public static String dir = "//", alli = "//";
        public static string Host_Path = "C:\\Users\\Public\\sharedFile", Host_UpDown = "C:\\Users\\Public\\sharedFile1";
        public static string Guest_Path = "F:\\Public\\sharedFile", Guest_UpDown = "F:\\Public\\sharedFile1";

        public static Boolean Initialize_Set(String[,] map)
        {
            try
            {
                StreamWriter sharedFile = new StreamWriter(Host_Path);

                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        sharedFile.WriteLine(map[i, j]);
                    }
                }

                sharedFile.Close();            
                return true;
            }
            catch (IOException)
            {
                return false;
            }            
        }

        public static Boolean Initialize_Get(String[,] map)
        {
            try
            {
                StreamReader sharedFile = new StreamReader(Guest_Path); //percorso unità condivisa

                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        map[i, j] = sharedFile.ReadLine().ToString();
                    }
                }

                sharedFile.Close();
                return true;
            }
            catch (IOException)
            {
                return false;
                //Striker_Finale.Striker.LM_Game();
            }
        }

        public static Boolean Update(int x, int y, int sh_x,int sh_y, String dir, String alliance, int speed, int damage)
        {
            try
            {
                if (Type)
                {
                    sharedFile = new StreamWriter(Host_Path);
                }
                else
                {
                    sharedFile = new StreamWriter(Guest_UpDown);
                }

                sharedFile.WriteLine(x);
                sharedFile.WriteLine(y);

                if (sh_x != -1)
                {
                    sharedFile.WriteLine(sh_x);
                    sharedFile.WriteLine(sh_y);
                    sharedFile.WriteLine(dir);
                    sharedFile.WriteLine(alliance);
                    sharedFile.WriteLine(speed);
                    sharedFile.WriteLine(damage);
                }

                sharedFile.Close();
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        public static void Enemy_Update(String[,] map)
        {
            bool libero = false;
            do
            {
                int[] pos = Download_Position(map);
                if (pos[0] != -1)
                {
                    libero = true;
                    map[pos[1], pos[0]] = "Enem";                    
                }
                else
                {
                    libero = false;
                }
            }
            while (!libero);

        }

        static int[] Download_Position(String[,] map)
        {
            try
            {
                int[] position = new int[2];

                if (Type)
                {
                    sharedFileR = new StreamReader(Host_UpDown);
                }
                else
                {
                    sharedFileR = new StreamReader(Guest_Path);
                }

                String x = sharedFileR.ReadLine(), y = sharedFileR.ReadLine();

                if (x != "" && Convert.ToInt32(x) >= 0)
                {
                    position[0] = Convert.ToInt32(x);
                    //lastX = position[0];
                }

                if (y != "" && Convert.ToInt32(y) >= 0)
                {
                    position[1] = Convert.ToInt32(y);
                    //lastY = position[1];
                }
                map[X, Y] = "E";
                X = position[1];
                Y = position[0];

                String Ssh_x = sharedFileR.ReadLine();

                if (Ssh_x != "" && Convert.ToInt32(Ssh_x) > 0)
                {
                    shx = Convert.ToInt32(Ssh_x);
                    shy = Convert.ToInt32(sharedFileR.ReadLine());
                    dir = sharedFileR.ReadLine();
                    alli = sharedFileR.ReadLine();
                    alli = "Enem";
                    speed = Convert.ToInt32(sharedFileR.ReadLine());
                    dam = Convert.ToInt32(sharedFileR.ReadLine());

                    
                    //Shots.Add(new Shoot(map, Width, Height,new int[] {sh_x, sh_y }, dir, alli, speed, dam));
                    
                }

                sharedFileR.Close();
                return position;
            }
            catch (IOException)
            {
                int[] pos = { -1, 5 };
                return pos;
            }
        }

        /*public static Boolean Reset()
        {
            try
            {
                if (Type)
                {
                    sharedFile = new StreamWriter(Host_Path);
                }
                else
                {
                    sharedFile = new StreamWriter(Guest_UpDown);
                }

                sharedFile.WriteLine("");
                sharedFile.Close();
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }*/
    }
}
