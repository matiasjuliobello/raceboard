using Ionic.Zip;
using Ionic.Zlib;
using RaceBoard.Common.Helpers.Interfaces;

namespace RaceBoard.Common.Helpers
{
    public class CompressionHelper : ICompressionHelper
    {

        #region ICompressionHelper implementation

        public MemoryStream CreateZipFile(string[] fileNames, string directory = null)
        {
            var outputStream = new MemoryStream();

            try
            {
                using (var zip = new ZipFile())
                {
                    zip.CompressionLevel = CompressionLevel.BestCompression;
                    zip.AddFiles(fileNames, directory);
                    zip.Save(outputStream);
                }
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

            return outputStream;
        }

        #endregion
    }
}