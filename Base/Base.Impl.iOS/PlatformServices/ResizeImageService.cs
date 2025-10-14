using System;
using UIKit;
using Foundation;
using CoreGraphics;
using System.IO;
using Microsoft.Maui.Storage;
using System.Reflection;
using Base.Abstractions.PlatformServices;

namespace Base.Impl.iOS.PlatformServices
{
    /// <summary>
    /// Note this resizer can require huge amount of memory to resize image.
    /// If app requires to take memory as less as possible then take a look CGImageSource.CreateThumbnail
    /// </summary>
    public class ResizeImageService : IResizeImageService
    {
        /// <summary>
        /// Gets scaled-down JPG image so it will be lightweight where preserving the aspect ratio
        /// </summary>
        /// <param name="imageData">image bytes</param>
        /// <param name="maxWidth">max width</param>
        /// <param name="maxHeight">max height</param>
        /// <returns>jpg image data as bute array</returns>
        public ImageResizeResult ResizeImage(byte[] imageData, string originalContentType, int maxWidth, int maxHeight, float quality, int rotation = 0, bool shouldSetUniqueName = false)
        {
            using (var data = NSData.FromArray(imageData))
            {
                var originalImage = UIImage.LoadFromData(data);
                var resultImage = ResizeNativeImage(originalImage, originalContentType, maxWidth, maxHeight, rotation, quality, shouldSetUniqueName: shouldSetUniqueName);
                return resultImage;                
            }
        }

        public ImageResizeResult ResizeNativeImage(object image, string originalContentType, int maxWidth, int maxHeight, int rotation, float quality = 97f, bool shouldSetUniqueName = false)
        {
            var sourceImage = (UIImage)image;
            if (rotation > 0)
            {                
                var imgOrientation = ConvertToUIOrientation(rotation);
                sourceImage = RotateImage(sourceImage, imgOrientation);
            }
            else if (sourceImage.Orientation != UIImageOrientation.Up)
            {
                sourceImage = RotateImage(sourceImage, sourceImage.Orientation);
            }

            if (quality == 0f || quality == 97f)
                quality = 0.95f;
            else
                quality = quality / 100;

            UIImage resultImage = null;
            var sourceSize = sourceImage.Size;
            if (sourceSize.Width <= maxWidth && sourceSize.Height <= maxHeight)
            {
                resultImage = sourceImage;
            }
            else
            {
                using (sourceImage)
                {
                    var ratioX = (double)maxWidth / sourceSize.Width;
                    var ratioY = (double)maxHeight / sourceSize.Height;
                    var ratio = Math.Max(ratioX, ratioY);
                    //preserve aspect ration
                    var newWidth = (float)Math.Ceiling(sourceSize.Width * ratio);
                    var newHeight = (float)Math.Ceiling(sourceSize.Height * ratio);

                    UIGraphics.BeginImageContext(new CGSize(newWidth, newHeight));
                    sourceImage.Draw(new CGRect(0, 0, newWidth, newHeight));
                    resultImage = UIGraphics.GetImageFromCurrentImageContext();
                    UIGraphics.EndImageContext();
                }
            }

            using (resultImage)
            {
                var jpegImage = resultImage.AsJPEG(quality);
                using (var stream = jpegImage.AsStream())
                {
                    var bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, bytes.Length);
                    string fileName = string.Empty;
                    if (shouldSetUniqueName)
                        fileName = $"resizedImage_{Guid.NewGuid()}.jpg";
                    else
                        fileName = "sharedImage.jpg";
                    string documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    string jpgFilename = System.IO.Path.Combine(documentsDirectory, fileName);
                    if (File.Exists(jpgFilename))
                        File.Delete(jpgFilename);
                    File.WriteAllBytes(jpgFilename, bytes);

                    return new ImageResizeResult() 
                    {
                        Image = bytes,
                        FilePath = jpgFilename, 
                        ImageSize = new System.Drawing.Size((int)resultImage.Size.Width, (int)resultImage.Size.Height),
                        ContentType = "image/jpeg"
                    };
                }
            }
        }

        private UIImageOrientation ConvertToUIOrientation(int rotation)
        {
            UIImageOrientation uIImageOrientation = UIImageOrientation.Up;
            switch (rotation)
            {
                //Landscape left (device top on left), no rotation
                case 0:
                    uIImageOrientation = UIImageOrientation.Up;
                    break;

                //Landscape right (device top on right), rotate 180
                case 180:
                    uIImageOrientation = UIImageOrientation.Down;
                    break;
                //Upside down (device top on bottom), rotate -90
                case -90:
                    uIImageOrientation = UIImageOrientation.Left;
                    break;

                //Portrait (device top on top), rotate 90
                case 90:
                    uIImageOrientation = UIImageOrientation.Right;
                    break;
            }

            return uIImageOrientation;
        }

        private UIImage RotateImage(UIImage image, UIImageOrientation uIImageOrientation)
        {
            
            
            var imageToRotate = UIImage.FromImage(image.CGImage, image.CurrentScale, uIImageOrientation);
            UIGraphics.BeginImageContextWithOptions(new CGSize((float)image.CGImage.Height, (float)image.CGImage.Width), true, 1.0f);
            imageToRotate.Draw(new CGRect(0, 0, (float)image.CGImage.Height, (float)image.CGImage.Width));

            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            imageToRotate = resultImage;

            return imageToRotate;
        }

#if !iosShareExt
        public int GetRequiredRotation(object fr)
        {
            try
            {
                var fileResult = fr as FileResult;

                var assembly = typeof(FileSystem).Assembly;

                var uiImageType = assembly.GetType("Xamarin.Essentials.UIImageFileResult");

                var uiImageField = uiImageType.GetTypeInfo().GetDeclaredField("uiImage");
                UIImage image = (UIImage)uiImageField.GetValue(fileResult);

                switch (image.Orientation)
                {
                    //Landscape left (device top on left), no rotation
                    case UIImageOrientation.Up:
                        return 0;

                    //Landscape right (device top on right), rotate 180
                    case UIImageOrientation.Down:
                        return 180;

                    //Upside down (device top on bottom), rotate -90
                    case UIImageOrientation.Left:
                        return -90;

                    //Portrait (device top on top), rotate 90
                    case UIImageOrientation.Right:
                        return 90;
                }
            }
            catch (System.ArgumentException)
            {
            }

            return 0;
        }
#else
        public int GetRequiredRotation(object fr)
        {
            return 0;
        }
#endif

        public int GetRequiredRotation(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}