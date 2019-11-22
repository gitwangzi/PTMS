using System.ComponentModel;

/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
///// Guid: 9d8c2ee2-20b4-434b-ae6b-6f354a75cb63      
/////clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
///// Machine Name: PC-ShiHS
///// Author: TEST(ShiHongSheng)
/////======================================================================
/////Project Name: 
/////Project Description:    
/////Class Name: 
/////Class Version: v1.0.0.0
/////Create Time: 2013/11/08
/////Class Description:  
/////======================================================================
/////Modified Time: 2013/11/08
/////Modified by:(ShiHS)
/////Modified Description: 

namespace Gsafety.PTMS.Bases.Enums
{
    public class EnumAttribute : DescriptionAttribute
    {
        /// <summary>
        /// ResourceName
        /// </summary>
        public string ResourceName { get; set; }
        /// <summary>
        /// Flag
        /// </summary>
        public string Flag { get; set; }
        /// <summary>
        ///  Description
        /// </summary>
        public new string Description { get; set; }
    }
    /// <summary>
    /// EnumInfos
    /// </summary>
    public struct EnumInfos
    {
        /// <summary>
        /// Index
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///Value
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// EnumAttributeInfo
        /// </summary>
        public EnumAttribute EnumAttributeInfo { get; set; }
        /// <summary>
        /// LocalizedString
        /// </summary>
        public string LocalizedString { get; set; }
    }
}

