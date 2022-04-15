using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ResourceLoader
{
    public class Image : IDisposable, IEquatable<Image>
    {
        private string filePath;

        public string Name
        {
            get
            {
                return Path.GetFileNameWithoutExtension(filePath);
            }
        }

        private Bitmap bitmap;

        public Image(string filePath)
        {
            this.filePath = filePath;
        }

        public Bitmap Bitmap
        {
            get
            {
                if (bitmap == null)
                {
                    bitmap = new Bitmap(filePath);
                    return bitmap;
                }
                else
                {
                    return bitmap;
                }
            }
        }

        public void Dispose()
        {
            bitmap?.Dispose();
        }

        public bool Equals(Image other)
        {
            return filePath == other.filePath;
        }
    }
}
