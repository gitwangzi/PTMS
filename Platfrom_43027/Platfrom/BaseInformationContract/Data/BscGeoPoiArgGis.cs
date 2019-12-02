using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
namespace Gsafety.PTMS.BaseInformation.Contract.Data
{

   // https://www.sojson.com/json2csharp.html
	///<summary>
	///POI
	///</summary>
    [DataContract]
	public class BscGeoPoiArgGis
	{

        string _name;
        ///<summary>
        ///地理位置名称
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
                _name = value;
            }
        }
        double _longitude;
        ///<summary>
        ///经度
        ///</summary>
        [DataMember]
        public double Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                _longitude = value;
            }
        }

        double _latitude;
        ///<summary>
        ///纬度
        ///</summary>
        [DataMember]
        public double Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                _latitude = value;
            }
        }

        string _address;
        ///<summary>
        ///地址
        ///</summary>
        [DataMember]
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
            }
        }
        string _property;
		///<summary>
		///所属类型
		///</summary>
		[DataMember]
		public string Property
		{
			get
			{
				return _property;
			}
			set
			{
				 _property= value;
			}
		}
	}

    //如果好用，请收藏地址，帮忙分享。
    public class SpatialReference
    {
        /// <summary>
        /// 
        /// </summary>
        public int wkid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int latestWkid { get; set; }
    }

    public class Location
    {
        /// <summary>
        /// 
        /// </summary>
        public double x { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double y { get; set; }
    }

    public class Attributes
    {
        /// <summary>
        /// 
        /// </summary>
        public double Score { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Match_addr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Descr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PlaceName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StAddr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Place_addr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Nbrhd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string County { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StateAbbr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LangCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Distance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double DisplayX { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double DisplayY { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Xmin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Xmax { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Ymin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Ymax { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Rank { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Addr_type { get; set; }
    }

    public class Extent
    {
        /// <summary>
        /// 
        /// </summary>
        public double xmin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double ymin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double xmax { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double ymax { get; set; }
    }

    public class CandidatesItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Location location { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double score { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Attributes attributes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Extent extent { get; set; }
    }

    public class ArgGisRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public SpatialReference spatialReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<CandidatesItem> candidates { get; set; }
    }
}

