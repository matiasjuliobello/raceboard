//using Microsoft.AspNetCore.Mvc;
//using SkiaSharp;


//namespace RaceBoard.Common.Helpers
//{
//    public class ImageCompressionHelper
//    {
//        public async Task REsize()
//        {
//			using var inputStream = file.OpenReadStream();
//			using var original = SKBitmap.Decode(inputStream);

//			if (original == null)
//				return BadRequest("Invalid image format");

//			// Resize logic (keep aspect ratio)
//			const int maxSize = 1024;
//			int width = original.Width;
//			int height = original.Height;

//			float scale = Math.Min((float)maxSize / width, (float)maxSize / height);
//			int newWidth = (int)(width * scale);
//			int newHeight = (int)(height * scale);

//			using var resized = original.Resize(new SKImageInfo(newWidth, newHeight), SKFilterQuality.Medium);

//			if (resized == null)
//				return StatusCode(500, "Failed to resize image");

//			using var image = SKImage.FromBitmap(resized);
//			using var data = image.Encode(SKEncodedImageFormat.Jpeg, 80); // quality: 0–100


//			var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
//			Directory.CreateDirectory(uploadsPath);
//			var filename = Guid.NewGuid() + ".jpg";
//			var filePath = Path.Combine(uploadsPath, filename);

//			await using var fs = System.IO.File.OpenWrite(filePath);
//			data.SaveTo(fs);

//        }
//    }
//}
