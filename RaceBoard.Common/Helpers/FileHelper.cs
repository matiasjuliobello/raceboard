using RaceBoard.Common.Extensions;
using RaceBoard.Common.Helpers.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace RaceBoard.Common.Helpers
{
    public class FileHelper : IFileHelper
    {
        #region Private Members

        #endregion

        public FileHelper(IConfiguration configuration)
        {
        }

        #region IFileHelper implementation

        public string GetFileExtension(string filename)
        {
            return Path.GetExtension(filename);
        }

        public string GetSafeFilename(string fullPath, string filename)
        {
            filename = RemoveIllegalCharsFromFilename(filename);

            var fileNameOnly = Path.GetFileNameWithoutExtension(filename);
            var extension = Path.GetExtension(filename);
            var count = 1;

            var fullFileName = Path.Combine(fullPath, filename);
            while (File.Exists(fullFileName))
            {
                var tempFileName = $"{fileNameOnly} ({count++}){extension}";
                
                fullFileName = Path.Combine(fullPath, tempFileName);
            }

            return fullFileName;
        }

        public string RemoveIllegalCharsFromFilename(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        public string SaveFile(Stream stream, string path, string filename)
        {
            var bytesInStream = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(bytesInStream, 0, bytesInStream.Length);

            filename = SaveFile(bytesInStream, path, filename);

            stream.SafeClose();
            stream.SafeDispose();

            return filename;
        }

        public string SaveFile(byte[] content, string fullPath, string filename)
        {
            string fileFullPath = Path.Combine(fullPath, filename);

            //var fileStream = File.Create(fileFullPath, content.Length);
            //fileStream.Write(content, 0, content.Length);

            //fileStream.SafeClose();
            //fileStream.SafeDispose();

            //return fileFullPath;

            File.WriteAllBytes(fileFullPath, content);

            return fileFullPath;
        }

        public DirectoryInfo CreateDirectory(string fullPath)
        {
            return Directory.CreateDirectory(fullPath);
        }

        public void DeleteDirectory(string fullPath)
        {
            try
            {
                Directory.Delete(fullPath, true);
            }
            catch (Exception)
            {
                // throw;
            }
        }

        public Stream ReadFileInfoStream(string filePath)
        {
            var memStream = new MemoryStream();
            using (var fileStream = File.OpenRead(filePath))
            {
                memStream.SetLength(fileStream.Length);
                fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
            }
            return memStream;
        }

        public byte[] ReadBytesFromStream(Stream stream)
        {
            var fileContent = new byte[stream.Length];
            stream.Read(fileContent, 0, fileContent.Length);
            stream.SafeClose();
            stream.SafeDispose();
            return fileContent;
        }

        public void CopyFiles(string sourcePath, string targetPath, string[] files = null)
        {
            if (Directory.Exists(sourcePath))
            {
                if (!Directory.Exists(targetPath))
                    Directory.CreateDirectory(targetPath);

                var allFiles = Directory.GetFiles(sourcePath);
                foreach (var file in allFiles)
                {
                    var fileName = Path.GetFileName(file);
                    var targetFilename = Path.Combine(targetPath, fileName);
                    if (files != null && files.Length > 0)
                    {
                        if (files.Contains(fileName))
                            File.Copy(file, targetFilename, true);
                    }
                    else
                    {
                        File.Copy(file, targetFilename, true);
                    }
                }
            }
        }

        public void RunExecutable(string executableFileName, string arguments = null, Action<string> onExecutedCallback = null)
        {
            string processStandardOutput = string.Empty;

            try
            {
                bool redirectStandardOutput = true;
                bool redirectStandardError = true;

                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = executableFileName,
                        Arguments = arguments,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = (redirectStandardOutput || redirectStandardError) ? false : true,
                        CreateNoWindow = false,
                        WindowStyle = ProcessWindowStyle.Hidden

                    }
                };

                if (!proc.Start())
                    throw new Exception($"Process {executableFileName} could not start.");

                processStandardOutput = proc.StandardOutput.ReadToEnd();

                proc.WaitForExit();
            }
            finally
            {
                if (onExecutedCallback != null)
                    onExecutedCallback.Invoke(processStandardOutput);
            }
        }

        #endregion
    }
}
