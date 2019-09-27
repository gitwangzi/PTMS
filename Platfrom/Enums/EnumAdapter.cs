using System;
using System.Reflection;
using Gsafety.Common.Localization.Resource;
using System.Collections.Generic;
using System.Resources;

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
/////Create Time: 2013/11/06 16:57:29
/////Class Description:  
/////======================================================================
/////Modified Time: 2013/11/06 16:57:29
/////Modified by:(ShiHS)
/////Modified Description: 

namespace Gsafety.PTMS.Bases.Enums
{
    public class EnumAdapter<EnumType>
    {
        public IList<EnumInfos> EnumInfo
        {
            get { return GetEnumInfos(); }
        }

        public IList<EnumInfos> GetEnumInfos()
        {
            List<EnumInfos> result = new List<EnumInfos>();
            string[] names = System.Enum.GetNames(typeof(EnumType));
            //var valueArray = Enum.GetValues(typeof(EnumType));
            try
            {
                int index = 0;
                foreach (var name in names)
                {
                    int value = Convert.ToInt32((EnumType)Enum.Parse(typeof(EnumType), name, true));
                    FieldInfo info = typeof(EnumType).GetField(name);
                    EnumAttribute[] attreibutes = info.GetCustomAttributes(typeof(EnumAttribute), false) as EnumAttribute[];
                    string localizedString = string.Empty;
                    EnumAttribute enumAttributeInfo = null;
                    if (attreibutes.Length > 0)
                    {
                        string resourceName = string.IsNullOrEmpty(attreibutes[0].ResourceName) ? name : attreibutes[0].ResourceName;
                        string localizedStr = StringResource.ResourceManager.GetString(resourceName);
                        localizedString = string.IsNullOrEmpty(localizedStr) ? resourceName : localizedStr;
                        string description = string.IsNullOrEmpty(attreibutes[0].Description) ? string.Empty : attreibutes[0].Description;
                        string flag = string.IsNullOrEmpty(attreibutes[0].Flag) ? string.Empty : attreibutes[0].Flag;
                        enumAttributeInfo = new EnumAttribute { ResourceName = resourceName, Description = description, Flag = flag };
                    }
                    else
                    {
                        string localizedStr = StringResource.ResourceManager.GetString(name);
                        localizedString = string.IsNullOrEmpty(localizedStr) ? name : localizedStr;
                    }
                    var infos = new EnumInfos()
                    {
                        Index = index++,
                        Name = name,
                        Value = value,
                        EnumAttributeInfo = enumAttributeInfo,
                        LocalizedString = localizedString
                    };
                    result.Add(infos);
                }
            }
            catch (Exception) { throw; }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="getSource"> a delegate process  get localizing information from resource-dictionary</param>
        /// <returns></returns>
        public IList<EnumInfos> GetEnumInfos(Func<string, string> getSource)
        {
            List<EnumInfos> result = new List<EnumInfos>();
            string[] names = System.Enum.GetNames(typeof(EnumType));
            try
            {
                int index = 0;
                foreach (var name in names)
                {
                    //int value = System.Enum.Parse(typeof(EnumType), name, true).GetHashCode();
                    int value = Convert.ToInt32((EnumType)Enum.Parse(typeof(EnumType), name, true));
                    FieldInfo info = typeof(EnumType).GetField(name);
                    EnumAttribute[] attreibutes = info.GetCustomAttributes(typeof(EnumAttribute), false) as EnumAttribute[];
                    string localizedString = string.Empty;
                    EnumAttribute enumAttributeInfo = null;
                    if (attreibutes.Length > 0)
                    {
                        string resourceName = string.IsNullOrEmpty(attreibutes[0].ResourceName) ? name : attreibutes[0].ResourceName;
                        string localizedStr = StringResource.ResourceManager.GetString(resourceName);
                        localizedString = string.IsNullOrEmpty(localizedStr) ? resourceName : localizedStr;
                        string description = string.IsNullOrEmpty(attreibutes[0].Description) ? string.Empty : attreibutes[0].Description;
                        string flag = string.IsNullOrEmpty(attreibutes[0].Flag) ? string.Empty : attreibutes[0].Flag;
                        enumAttributeInfo = new EnumAttribute { ResourceName = resourceName, Description = description, Flag = flag };
                    }
                    else
                    {
                        string localizedStr = StringResource.ResourceManager.GetString(name);
                        localizedString = string.IsNullOrEmpty(localizedStr) ? name : localizedStr;
                    }
                    var infos = new EnumInfos()
                    {
                        Index = index++,
                        Name = name,
                        Value = value,
                        EnumAttributeInfo = enumAttributeInfo,
                        LocalizedString = localizedString
                    };
                    result.Add(infos);
                }
            }
            catch (Exception) { throw; }
            return result;
        }
    }
}

