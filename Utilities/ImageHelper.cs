using Microsoft.AspNetCore.Http;
using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Utilities
{
    public class ImageHelper
    {
        public static string GetImage(byte[] image, string filename)
        {
            if (image != null)
            {
                var binary = Convert.ToBase64String(image);
                var ext = Path.GetExtension(filename);
                string imageDataURl = $"data:image/{ext};base64,{binary}";
                return imageDataURl;
            }
            return "../../Images/default_post_img.jpg";
        }

        //Updating the image
        public static byte[] PutImage(IFormFile file)
        {
                //This is entry level code that turns our image into a storable form
                var ms = new MemoryStream();
                file.CopyTo(ms);
                var image = ms.ToArray();

                ms.Close();
                ms.Dispose();

            return image;
        }

    }
}
