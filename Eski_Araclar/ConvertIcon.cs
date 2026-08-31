using System;
using System.Drawing;
using System.IO;

class Program
{
    static void Main()
    {
        string pngPath = "logo.png";
        string icoPath = "logo.ico";

        if (!File.Exists(pngPath))
        {
            Console.WriteLine("logo.png not found.");
            return;
        }

        using (FileStream fs = new FileStream(icoPath, FileMode.Create))
        using (Image bmp = Image.FromFile(pngPath))
        {
            // Write ICONDIR
            fs.WriteByte(0);
            fs.WriteByte(0);
            fs.WriteByte(1); // 1 = Icon
            fs.WriteByte(0);
            fs.WriteByte(1); // 1 Image
            fs.WriteByte(0);

            // Write ICONDIRENTRY
            int width = bmp.Width;
            int height = bmp.Height;
            if (width >= 256) width = 0; // 0 means 256
            if (height >= 256) height = 0;

            fs.WriteByte((byte)width);
            fs.WriteByte((byte)height);
            fs.WriteByte(0); // Color count
            fs.WriteByte(0); // Reserved
            fs.WriteByte(1); // Color planes
            fs.WriteByte(0);
            fs.WriteByte(32); // Bits per pixel
            fs.WriteByte(0);

            // We need to write the PNG as the image data directly (supported in Vista+)
            byte[] pngData = File.ReadAllBytes(pngPath);
            int dataSize = pngData.Length;

            // Size of image data
            fs.Write(BitConverter.GetBytes(dataSize), 0, 4);

            // Offset of image data (6 + 16 = 22)
            fs.Write(BitConverter.GetBytes(22), 0, 4);

            // Write PNG data
            fs.Write(pngData, 0, pngData.Length);
        }
        Console.WriteLine("Converted logo.png to logo.ico");
    }
}
