#region Autor
/*
    Author: David Cusanelly
    Email: cuentacus@gmail.com
    Date: 20180402
    Nothing else but Navi...
*/
#endregion
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CusanellyLibrary.Images
{
    public interface IInterface
    {

    }
    class Class1
    {
        public string CreateImageBase64(IFormFile image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                /* Convert this image back to a base64 string */
                image.CopyTo(ms);
                return Convert.ToBase64String(ms.ToArray());
            }
        }
        public FileStream CreateImageFromBase64(string imagebase64, string filepath) {
            var bytes = Convert.FromBase64String(imagebase64);
            FileStream imagefile;
            using (imagefile = new FileStream(filepath, FileMode.Create))
            {
                imagefile.Write(bytes, 0, bytes.Length);
                imagefile.Flush();
            }
            return imagefile;
        }
    }
}
