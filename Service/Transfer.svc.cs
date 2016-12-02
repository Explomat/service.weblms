using System;
using Service.Model.Utils;
using Service.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Transfer" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Transfer.svc or Transfer.svc.cs at the Solution Explorer and start debugging.
    public class Transfer : ITransfer
    {
        #region Common Methods
        /// <summary>
        /// проверка соединения
        /// </summary>
        /// <returns> OK </returns>
        public string TestConnection()
        {
            return "OK";
        }

        #endregion

        #region File Methods

        public RemoteFileInfo DownloadFile(DownloadRequest request)
        {
            RemoteFileInfo result = new RemoteFileInfo();
            try
            {
                string filePath = Path.Combine(@"c:\Uploadfiles", request.FileName);
                FileInfo fileInfo = new FileInfo(filePath);

                // check if exists
                if (!fileInfo.Exists)
                {
                    throw new FileNotFoundException("File not found", request.FileName);
                }

                // open stream
                FileStream stream = new FileStream(filePath,FileMode.Open, FileAccess.Read);

                // return result 
                result.FileName = request.FileName;
                result.Length = fileInfo.Length;
                result.FileByteStream = stream;
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public ResponseFileInfo ConvertFile(UploadFileInfo request)
        {
            //FileStream targetStream = null;
            //Stream sourceStream = new MemoryStream(request.ByteArray);
            //string destDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WRecords", "test.zip");

            //using (targetStream = new FileStream(destDirectory, FileMode.Create, FileAccess.Write, FileShare.None))
            //{
            //    //read from the input stream in 65000 byte chunks

            //    const int bufferLen = 65000;
            //    byte[] buffer = new byte[bufferLen];
            //    int count = 0;
            //    while ((count = sourceStream.Read(buffer, 0, bufferLen)) > 0)
            //    {
            //        // save to output stream
            //        targetStream.Write(buffer, 0, count);
            //    }
            //    targetStream.Close();
            //    sourceStream.Close();
            //}
            //return new ResponseFileInfo();

            Stream sourceStream = new MemoryStream(request.ByteArray);
            using (MD5 md5 = MD5.Create())
            {
                string hash = Hash.GetMD5Hash(md5, sourceStream);
                string destDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WRecords");
                string destFullDirectory = Path.Combine(destDirectory, request.Email, hash);
                string videoDirectory = Path.Combine(destFullDirectory, "video");

                if (!Directory.Exists(destDirectory))
                {
                    Directory.CreateDirectory(destDirectory);
                }

                if (!Directory.Exists(destFullDirectory))
                {
                    Directory.CreateDirectory(destFullDirectory);
                    Directory.CreateDirectory(videoDirectory);
                    try
                    {
                        Zip.Unzip(sourceStream, destFullDirectory);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Некорректный архив!", e);
                    }
                    
                }
                sourceStream.Close();

                try
                {
                    VideoConverter converter = new VideoConverter(destFullDirectory, videoDirectory);
                    string outFilePath = converter.Start();
                    converter.Dispose();
                    ResponseFileInfo fileInfo = new ResponseFileInfo();
                    fileInfo.Path = outFilePath;
                    return fileInfo;
                }
                catch (Exception e)
                {
                    throw new Exception("Произошла ошибка при конвертации", e);
                }
            }

        }
        #endregion
    }
}
