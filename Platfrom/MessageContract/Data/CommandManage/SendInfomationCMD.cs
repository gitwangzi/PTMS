using System.Globalization;
using Gsafety.PTMS.Base.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract.Data.CommandManage
{
    [Serializable]
    [DataContract]
    public class SendInfomationCMD : DownwardBase
    {
        [DataMember]
        public SendInfomationType DispatchType;
        [DataMember]
        public bool ManualControl;
        [DataMember]
        public string SendContent;
        [DataMember]
        public DateTime SendTime;
        [DataMember]
        public DisplayPositionType[] DisplayPosition;
        [DataMember]
        public int DisplayTime;

        //public override string ToString()
        //{
        //    StringBuilder strCmd = new StringBuilder();
        //    strCmd.Append("99dcXXXX,")
        //       .Append(this.DvId)
        //       .Append(",")
        //       .Append(this.MsgId)
        //       .Append(",")
        //       .Append(CmType)
        //       .Append(",")
        //       .Append(this.SendTime.ToString("yyMMdd HHmmss"))
        //       .Append(",")
        //       .Append(SendValue)
        //       .Append("#");
        //    strCmd.Replace("XXXX", (strCmd.Length - 8).ToString("D5").Substring(1));
        //    return strCmd.ToString();
        //}
        //99dcxxxx,T0001,Johnny,C57,070729 234015,1,0,0,0A0F,-1,”支援21路，即刻发车。”#
        public string ToString()
        {
            StringBuilder strCmd = new StringBuilder();
              char[] charArr = SendContent.ToCharArray();
            int Count = 0;
            int Index = 0;
            for (int i = 0; i < charArr.Length; i++)
            {
                if (charArr[i] == '?')
                {
                    Count++;
                }
            }
            var result = Encoding.ASCII.GetBytes(SendContent);
            foreach (var b in result)
            {
                if (b == 63)
                {
                    Index++;
                }
            }
            var ascii = 0x0F;
            var ascii35 = 0x01;
            string s = ((char)ascii).ToString();
            s += ((char)ascii35).ToString();       
            SendContent = SendContent.Replace("#", s);
            int ascii44 = 0x03;
             s = ((char)ascii).ToString();
            s += ((char)ascii44).ToString();
            SendContent = SendContent.Replace(",", s);
          
            string _manual;
            if (ManualControl)
            {
                _manual = "1";
            }
            else
            {
                _manual = "0";
            }
            int Position = DisplayPosition.Length;
           string dataTime=    this.SendTime.ToString("yyMMdd HHmmss");
            string dispatchType;
            if (DispatchType == SendInfomationType.Car)
            {
                dispatchType = "1";
            }
            else
            {
                dispatchType = "2";
            }
            strCmd.Append("99dcXXXX,")
                  .Append(this.DvId)
                  .Append(",")
                  .Append(this.MsgId)
                  .Append(",")
                  .Append("C57")
                  .Append(",")
                  .Append(dataTime)
                  .Append(",")

                  .Append(dispatchType)
                  .Append(",")
                  .Append(_manual)
                  .Append(",")
                  .Append("0001")
                  .Append(",")
                  .Append(Position.ToString("x"))
                  .Append(",")
                  .Append(DisplayTime)
                  .Append(",\"" + SendContent+"\"#");
            ////Replace length minus 8 bytes（99dcxxxx）
            strCmd.Replace("XXXX", (strCmd.Length +Index -8-Count).ToString("D5").Substring(1));
          
            return strCmd.ToString();
        }
    }
}
