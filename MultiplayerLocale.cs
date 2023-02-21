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
        public static int Width;
        public static int Height;
        public static Boolean Type = true;
        static StreamReader sharedFileR;
        static StreamWriter sharedFile;
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

        public static Boolean Update(int x, int y)
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
                int[] pos = Download_Position();
                if (pos[0] != -1)
                {
                    Find_Enem(map);
                    map[pos[1], pos[0]] = "Enem";
                }
                else
                {
                    libero = false;
                }
            }
            while (!libero);
            
        }

        static void Find_Enem(String[,] map)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (map[i,j] == "Enem")
                    {
                        map[i, j] = "E";
                    }
                }
            }     
        }

        static int[] Download_Position()
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
                }

                if (y != "" && Convert.ToInt32(y) >= 0)
                {
                    position[1] = Convert.ToInt32(y);
                }
                sharedFileR.Close();
                return position;
            }
            catch (IOException)
            {
                int[] pos = { 5, 5 };
                return pos;
            }            
        }
    }
}
