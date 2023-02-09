using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Media;

namespace Stricker
{
    public class Music
    {
        static string cartella = Directory.GetCurrentDirectory();
        public static void SoundTrack()
        {
            var file = cartella + "\\src\\sottofondo.wav";
            SoundPlayer sottofondo = new SoundPlayer(file); 
            sottofondo.Play();
        }
    }
}
