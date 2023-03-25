using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using System.Drawing.Imaging;
using System.IO;

namespace TE_ManagementSystem.Models
{
    public class ImageViewModel
    {
        public int ImageId { get; set; }
        public string ImageTitle { get; set; }
        public byte[] ImageByte { get; set; }
        public string ImagePath { get; set; }
        public HttpPostedFileWrapper ImageFile { get; set; }

        public byte[] CreateThumbnailImage(int width, int height, byte[] buffer, bool center)
        {
            System.Drawing.Image image = this.Buffer2Img(buffer);

            // Create our new image
            Bitmap newImage = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                if (center && image.Width != image.Height)
                {
                    Rectangle srcRect = new Rectangle();

                    if (image.Width > image.Height)
                    {
                        srcRect.Width = image.Height;
                        srcRect.Height = image.Height;
                        srcRect.X = (image.Width - image.Height) / 2;
                        srcRect.Y = 0;
                    }
                    else
                    {
                        srcRect.Width = image.Width;
                        srcRect.Height = image.Width;
                        srcRect.Y = (image.Height - image.Width) / 2;
                        srcRect.X = 0;
                    }

                    g.DrawImage(image, new Rectangle(0, 0, newImage.Width, newImage.Height), srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, GraphicsUnit.Pixel);
                }
                else
                {
                    g.DrawImage(image, 0, 0, width, height);
                }
            }

            byte[] data = this.Img2Buffer(newImage, ImageFormat.Png);

            return data;
        }

        private System.Drawing.Image Buffer2Img(byte[] Buffer)
        {
            if (Buffer == null || Buffer.Length == 0) { return null; }
            byte[] data = null;
            System.Drawing.Image oImage = null;
            Bitmap oBitmap = null;
            data = (byte[])Buffer.Clone();
            try
            {
                MemoryStream oMemoryStream = new MemoryStream(Buffer);
                oMemoryStream.Position = 0;
                oImage = System.Drawing.Image.FromStream(oMemoryStream);
                oBitmap = new Bitmap(oImage);
            }
            catch
            {
                throw;
            }
            return oBitmap;
        }
        private byte[] Img2Buffer(System.Drawing.Image Image, ImageFormat imageFormat)
        {
            if (Image == null) { return null; }
            byte[] data = null;
            using (MemoryStream oMemoryStream = new MemoryStream())
            {
                using (Bitmap oBitmap = new Bitmap(Image))
                {
                    oBitmap.Save(oMemoryStream, imageFormat);
                    oMemoryStream.Position = 0;
                    data = new byte[oMemoryStream.Length];
                    oMemoryStream.Read(data, 0, Convert.ToInt32(oMemoryStream.Length));
                    oMemoryStream.Flush();
                }
            }
            return data;
        }
    }
}