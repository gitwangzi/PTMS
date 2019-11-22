using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
namespace Gsafety.PTMS.Manager.Contract.Data
{
	///<summary>
	///权限基础表
	///</summary>
	[DataContract]
	public class FuncItem
	{
		string _id;
		///<summary>
		///唯一标识
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

		string _note;
		///<summary>
		///路径记录
		///</summary>
		[DataMember]
		public string Note
		{
			get
			{
				return _note;
			}
			set
			{
				 _note= value;
			}
		}

		string _version;
		///<summary>
		///版本标识
		///</summary>
		[DataMember]
		public string Version
		{
			get
			{
				return _version;
			}
			set
			{
				 _version= value;
			}
		}

        string _module;
        ///<summary>
        ///版本标识
        ///</summary>
        [DataMember]
        public string Module
        {
            get
            {
                return _module;
            }
            set
            {
                _module = value;
            }
        }

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			if (!string.IsNullOrEmpty(Convert.ToString(ID)))
			{
				builder.AppendLine("ID:" + ID.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Name)))
			{
				builder.AppendLine("Name:" + Name.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Note)))
			{
				builder.AppendLine("Note:" + Note.ToString());
			}
			if (!string.IsNullOrEmpty(Convert.ToString(Version)))
			{
				builder.AppendLine("Version:" + Version.ToString());
			}

			return builder.ToString();
		}
	}
}

