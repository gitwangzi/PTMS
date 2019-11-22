using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
using Gsafety.PTMS.Common.Enum;

namespace Gsafety.PTMS.Common.Data
{
	///<summary>
	///安全套件配件
	///</summary>
	[DataContract]
	public class DevSuitePart
	{
		string _id;
		///<summary>
		///主键
		///</summary>
		[DataMember]
		public string ID
		{
			get
			{
				return _id;
			}
			set
			{
				 _id= value;
			}
		}

		string _partsn;
		///<summary>
		///资产号
		///</summary>
		[DataMember]
		public string PartSn
		{
			get
			{
				return _partsn;
			}
			set
			{
				 _partsn= value;
			}
		}

		string _name;
		///<summary>
		///名称
		///</summary>
		[DataMember]
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				 _name= value;
			}
		}

		string _model;
		///<summary>
		///配件型号
		///</summary>
		[DataMember]
		public string Model
		{
			get
			{
				return _model;
			}
			set
			{
				 _model= value;
			}
		}

        BscDevSuitePartTypeEnum _parttype;
		///<summary>
		///类型
		///</summary>
		[DataMember]
        public BscDevSuitePartTypeEnum PartType
		{
			get
			{
				return _parttype;
			}
			set
			{
				 _parttype= value;
			}
		}

        string _showParttype;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string ShowParttype
        {
            get
            {
                return _showParttype;
            }
            set
            {
                _showParttype = value;
            }
        }


		DateTime? _producetime;
		///<summary>
		///出厂日期
		///</summary>
		[DataMember]
		public DateTime? ProduceTime
		{
			get
			{
				return _producetime;
			}
			set
			{
				 _producetime= value;
			}
		}

		string _suiteinfoid;
		///<summary>
		///安全套件
		///</summary>
		[DataMember]
		public string SuiteInfoID
		{
			get
			{
				return _suiteinfoid;
			}
			set
			{
				 _suiteinfoid= value;
			}
		}

		DateTime _createtime;
		///<summary>
		///创建日期
		///</summary>
		[DataMember]
		public DateTime CreateTime
		{
			get
			{
				return _createtime;
			}
			set
			{
				 _createtime= value;
			}
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			if (!string.IsNullOrEmpty(Convert.ToString(ID)))
			{
				builder.AppendLine("ID:" + ID.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(PartSn)))
			{
				builder.AppendLine("PartSn:" + PartSn.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Name)))
			{
				builder.AppendLine("Name:" + Name.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Model)))
			{
				builder.AppendLine("Model:" + Model.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(PartType)))
			{
				builder.AppendLine("PartType:" + PartType.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(ProduceTime)))
			{
				builder.AppendLine("ProduceTime:" + ProduceTime.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(SuiteInfoID)))
			{
				builder.AppendLine("SuiteInfoID:" + SuiteInfoID.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
			{
				builder.AppendLine("CreateTime:" + CreateTime.ToString());
			}

			return builder.ToString();
		}
	}
}

