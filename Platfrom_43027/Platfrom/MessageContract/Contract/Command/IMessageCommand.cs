using Gsafety.PTMS.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Common.Data.Data.Video;

namespace Gsafety.PTMS.Message.Contract
{
    [ServiceContract]
    public interface IMessageCommand
    {
        [OperationContract]
        void SendGetVideoListCMD(QueryMdvrFileList model);

        [OperationContract]
        void SendDownloadMdvrFile(DownloadFile model);

        [OperationContract]
        void SendTakePictureCMD(TakePictureArgs model);
    }
}
