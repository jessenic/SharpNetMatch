using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpNetMatch
{
    class DotPxi
    {
        public static string ConvertToBitmap(string filename)
        {
            byte maskR = 0;
            byte maskG = 0;
            byte maskB = 0;
            bool usemask = false;
            try
            {
                if (File.Exists(filename.Substring(0, filename.Length - 3) + "map"))
                {
                    using (var fs = File.OpenRead(filename.Substring(0, filename.Length - 3) + "map"))
                    {
                        var w = fs.ReadByte();
                        var h = fs.ReadByte();
                        fs.ReadByte();
                        fs.ReadByte();
                        for (int y = 0; y < h; y++)
                        {
                            for (int x = 0; x < w; x++)
                            {
                                for (byte l = 0; l < 4; l++)
                                {
                                    fs.ReadByte();
                                }
                            }
                        }
                        maskR = (byte)fs.ReadByte();
                        maskG = (byte)fs.ReadByte();
                        maskB = (byte)fs.ReadByte();
                        usemask = true;
                    }
                }
            }
            catch { }
            using (var fs = File.OpenRead(filename))
            {
                var intBuf = new byte[4];
                fs.Read(intBuf, 0, 4);
                var w = BitConverter.ToInt32(intBuf, 0);
                fs.Read(intBuf, 0, 4);
                var h = BitConverter.ToInt32(intBuf, 0);
                var bmp = new System.Drawing.Bitmap(w, h);
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        byte r = (byte)fs.ReadByte();
                        byte g = (byte)fs.ReadByte();
                        byte b = (byte)fs.ReadByte();
                        byte a = 255;
                        if (usemask && r == maskR && g == maskG && b == maskB)
                        {
                            a = 0;
                        }
                        //var c = (r << 16) + (g << 8) + b;
                        bmp.SetPixel(x, y, System.Drawing.Color.FromArgb(a, r, g, b));
                    }
                }
                bmp.Save(filename + ".bmp");
                return filename + ".bmp";
                //For y = 0 To h - 1
                //    For x = 0 To w - 1
                //        r = ReadByte(file)
                //        g = ReadByte(file)
                //        b = ReadByte(file)
                //        c = (r Shl 16) + (g Shl 8) + b
                //        PutPixel2 x, y, c, Image(img)
                //    Next x
                //Next y
            }
        }
    }
}
