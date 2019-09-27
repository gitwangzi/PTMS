using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Email.Contract.Data
{
    [DataContract]
    public class EmailInfo
    {
        /// <summary>
        /// Sender
        /// </summary>
        //[DataMember]
        //public string MailFrom { get; set; }

        /// <summary>
        /// Addressee
        /// </summary>
        [DataMember]
        public string[] MailToArray { get; set; }

        /// <summary>
        /// make a copy for
        /// </summary>
        [DataMember]
        public string[] MailCcArray { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [DataMember]
        public string MailSubject { get; set; }

        /// <summary>
        /// Main Body
        /// </summary>
        [DataMember]
        public string MailBody { get; set; }

        /// <summary>
        /// Sender password
        /// </summary>
        //[DataMember]
        //public string MailPwd { get; set; }

        /// <summary>
        /// SMTP mail server
        /// </summary>
        //[DataMember]
        //public string Host { get; set; }

        /// <summary>
        /// Main Body is HTML format
        /// </summary>
        [DataMember]
        public bool IsbodyHtml { get; set; }

        /// <summary>
        /// Attachment
        /// </summary>
        [DataMember]
        public string[] AttachmentsPath { get; set; }
        /// <summary>
        /// Picture
        /// </summary>
        [DataMember]
        public string stream { get; set; }
        [DataMember]
        public Stream picturestream { get; set; }
        [DataMember]
        public byte[] bytepicture { get; set; }
    }
}
