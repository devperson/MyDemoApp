using Android.Graphics;
using Android.Media;
using Base.Abstractions.Platform;
using Microsoft.Maui.Storage;
using Orientation = Android.Media.Orientation;

namespace Base.Impl.Droid.PlatformServices;
public class ResizeImageService : IResizeImageService
{
    /// <summary>
    /// Gets scaled-down JPG image so it will be lightweight where preserving the aspect ratio
    /// </summary>
    /// <param name="imageData">image bytes</param>
    /// <param name="maxWidth">max width</param>
    /// <param name="maxHeight">max height</param>
    /// <returns>jpg image data as bute array</returns>
    public ImageResizeResult ResizeImage(byte[] imageData, string originalContentType, int maxWidth, int maxHeight, float quality = 97f, int rotation = 0, bool shouldSetUniqueName = false)
    {
        if (quality == 0f) quality = 97f;

        GC.Collect();
        // Load the bitmap 
        using (Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length))
        {
            return ResizeNativeImage(originalImage, originalContentType, maxWidth, maxHeight, rotation, quality, shouldSetUniqueName: shouldSetUniqueName);
        }
    }

    public ImageResizeResult ResizeNativeImage(object image, string originalContentType, int maxWidth, int maxHeight, int rotation = 0, float quality = 97, bool shouldSetUniqueName = false)
    {
        //
        //if (originalImage.Width <= maxWidth && originalImage.Height <= maxHeight)
        //    return sourceImage;
        var originalImage = (Bitmap)image;
        var ratioX = (double)maxWidth / originalImage.Width;
        var ratioY = (double)maxHeight / originalImage.Height;
        var ratio = Math.Max(ratioX, ratioY);

        var newWidth = (float)Math.Ceiling(originalImage.Width * ratio);
        var newHeight = (float)Math.Ceiling(originalImage.Height * ratio);

        Bitmap bitmapToScale = null;
        var shouldRotate = rotation > 0;
        if (shouldRotate)
        {
            Matrix matrix = new Matrix();
            matrix.PostRotate(rotation);
            bitmapToScale = Bitmap.CreateBitmap(originalImage, 0, 0, originalImage.Width, originalImage.Height, matrix, true);
            bitmapToScale = Bitmap.CreateScaledBitmap(bitmapToScale, originalImage.Height, originalImage.Width, false);
            var w = newWidth;
            newWidth = newHeight;
            newHeight = w;
        }
        else
        {
            bitmapToScale = originalImage;
        }

        using (Bitmap resizedImage = Bitmap.CreateScaledBitmap(bitmapToScale, (int)newWidth, (int)newHeight, false))
        {
            // 
            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, (int)quality, ms);
                var bytes = ms.ToArray();
                string fileName = string.Empty;
                if (shouldSetUniqueName)
                    fileName = $"resizedImage_{Guid.NewGuid()}.jpg";
                else
                    fileName = "sharedImageScaled_.jpg";
                string documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string jpgFilename = System.IO.Path.Combine(documentsDirectory, fileName);
                if (File.Exists(jpgFilename))
                    File.Delete(jpgFilename);
                File.WriteAllBytes(jpgFilename, bytes);

                return new ImageResizeResult()
                {
                    FilePath = jpgFilename,
                    IsResized = true,
                    Image = bytes,
                    ImageSize = new System.Drawing.Size((int)newWidth, (int)newHeight),
                    ContentType = "image/jpeg"
                };
            }
        }
    }



    public int GetRequiredRotation(object fileResult)
    {
        var path = ((FileResult)fileResult).FullPath;
        return GetRequiredRotation(path);
    }

    public int GetRequiredRotation(string imagePath)
    {
        var ei = new ExifInterface(imagePath);
        var orientation = (Orientation)ei.GetAttributeInt(ExifInterface.TagOrientation, 0);

        switch (orientation)
        {
            case Orientation.Rotate90:
                return 90;

            case Orientation.Rotate180:
                return 180;

            case Orientation.Rotate270:
                return 270;

            default:
                return 0;
        }
    }
}
