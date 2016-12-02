using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Service
{
    [ServiceContract]
    public interface ITransfer
    {
        [OperationContract]
        RemoteFileInfo DownloadFile(DownloadRequest request);

        [OperationContract]
        ResponseFileInfo ConvertFile(UploadFileInfo request);
    }
    [MessageContract]
    public class DownloadRequest
    {
        [MessageBodyMember]
        public string FileName;
    }

    [MessageContract]
    public class ResponseFileInfo
    {
        [MessageBodyMember]
        public string Path;
    }

    [MessageContract]
    public class UploadFileInfo
    {
        [MessageHeader(MustUnderstand = true)]
        public string Email;

        [MessageHeader(MustUnderstand = true)]
        public byte[] ByteArray;
    }

    [MessageContract]
    public class RemoteFileInfo : IDisposable
    {
        [MessageHeader(MustUnderstand = true)]
        public string FileName;

        [MessageHeader(MustUnderstand = true)]
        public long Length;

        [MessageBodyMember(Order = 1)]
        public System.IO.Stream FileByteStream;

        public void Dispose()
        {
            if (FileByteStream != null)
            {
                FileByteStream.Close();
                FileByteStream = null;
            }
        }
    }
}
