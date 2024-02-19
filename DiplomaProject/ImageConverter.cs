using System;
using System.IO;
using System.ServiceModel;

namespace DiplomProject
{
    public class ImageConverter
    {
        public static byte[] ConvertImageToBytes(string imagePath)
        {
            try
            {
                return File.ReadAllBytes(imagePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting image to bytes: {ex.Message}");
                return null;
            }
        }
    }
}
