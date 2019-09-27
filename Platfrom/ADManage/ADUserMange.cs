/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: dfcf072e-67d8-411e-a9e3-6da4ea76567e      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.Common.AD
/////    Project Description:    
/////             Class Name: ADUserMange
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/7/31 10:03:30
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/7/31 10:03:30
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using Gsafety.PTMS.Manager.Contract.Data;
using System.Collections.Specialized;
using System.Web.Security;
using System.IO;
using System.Xml.Linq;
using Gsafety.Common.Logging;

namespace Gsafety.Common.AD
{
    public class ADUserMange
    {
        public ADUserMange(string SdominName, string SADuserName, string SADpassword, string SAdRootPath, string SADRootPwd, string SADRootAdmin, string SADServiceConStr)
        {
            dominName = SdominName;
            ADuserName = SADuserName;
            ADpassword = SADpassword;
            AdRootPath = SAdRootPath;
            ADRootAdmin = SADRootAdmin;
            ADRootPwd = SADRootPwd;
            ADServiceConStr = SADServiceConStr;
        }
        public DirectoryEntry entry = null;
        public static string dominName = string.Empty;
        public static string ADuserName = string.Empty;
        public static string ADpassword = string.Empty;
        public static string AdRootPath = string.Empty;
        public static string ADRootPwd = string.Empty;
        public static string ADRootAdmin = string.Empty;
        public static string ADServiceConStr = string.Empty;


        /// <summary>
        /// get the names of all security groups 
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllgroupnames()
        {
            List<DirectoryEntry> entrylist = new List<DirectoryEntry>();
            List<string> grouplist = new List<string>();
            entry = new DirectoryEntry(ADServiceConStr, ADuserName, ADpassword, AuthenticationTypes.None);
            DirectorySearcher searcher = new DirectorySearcher(entry);
            searcher.Filter = ("objectClass=group");
            SearchResultCollection resultCollection = searcher.FindAll();
            if (resultCollection != null)
            {
                foreach (SearchResult result in resultCollection)
                {
                    string grname = result.GetDirectoryEntry().Name;
                    grouplist.Add(grname.Substring(3, grname.Length - 3));
                }
            }

            return grouplist;
        }

        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>Create by wzs</remarks>
        /// <returns></returns>
        public bool AddUserAccountByModel(ADAccountEntity model)
        {
            bool result = false;
            // ADCreateResultModel resultModel = new ADCreateResultModel();
            try
            {
                if (model == null)
                {
                    return false;
                }

                if (ObjectExists(model.UserName, "User"))
                {
                    return false;
                }
                entry = new DirectoryEntry(ADServiceConStr, "Administrator@" + dominName, ADpassword, AuthenticationTypes.Secure);
                using (DirectoryEntry user = entry.Children.Add("cn=" + model.UserName, "User"))
                {

                    user.Properties["userPrincipalName"].Add(model.UserName + "@" + dominName);
                    user.Properties["samAccountName"].Add(model.UserName);
                    user.Properties["displayName"].Add(model.DisplayName);
                    user.Properties["userPassword"].Add(model.UserPassword);
                    if (!string.IsNullOrEmpty(model.Email))
                    {
                        user.Properties["mail"].Add(model.Email);
                    }
                    if (!string.IsNullOrEmpty(model.Address))
                    {
                        user.Properties["homePostalAddress"].Add(model.Address);
                    }
                    if (!string.IsNullOrEmpty(model.Phone))
                    {
                        user.Properties["telephoneNumber"].Add(model.Phone);
                    }
                    if (!string.IsNullOrEmpty(model.Description))
                    {
                        user.Properties["Description"].Add(model.Description);
                    }
                    if (!string.IsNullOrEmpty(model.Company))
                    {
                        user.Properties["company"].Add(model.Company);
                    }
                    user.Properties["userAccountControl"].Value = 544;
                    user.Properties["PwdLastSet"].Value = -1;
                    user.CommitChanges();
                    user.AuthenticationType = AuthenticationTypes.Secure;
                    user.Invoke("SetPassword", new Object[] { model.UserPassword });

                    user.CommitChanges();

                    DirectoryEntry groupentry = GetGroupByGroupName(model.SecurityGroup);
                    if (groupentry != null)
                    {
                        groupentry.Invoke("Add", new object[] { user.Path });

                    }
                    groupentry.CommitChanges();
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }
        /// <summary>
        /// check object is exits
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="catalog"></param>
        /// <returns></returns>
        public bool ObjectExists(string objectName, string catalog)
        {
            /// DirectoryEntry de = GetDirectoryEntry();
            DirectoryEntry de = new DirectoryEntry(ADServiceConStr, "Administrator@" + dominName, ADpassword, AuthenticationTypes.Secure);
            DirectorySearcher deSearch = new DirectorySearcher();
            deSearch.SearchRoot = de;
            switch (catalog)
            {
                case "User": deSearch.Filter = "(&(objectClass=user) (cn=" + objectName + "))";
                    break;
                case "Group": deSearch.Filter = "(&(objectClass=group) (cn=" + objectName + "))";
                    break;
                case "OU": deSearch.Filter = "(&(objectClass=OrganizationalUnit) (cn=" + objectName + "))";
                    break;
                default:
                    break;
            }
            SearchResultCollection results = deSearch.FindAll();
            if (results.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool IsUserExits(string UserName)
        {
            DirectoryEntry UserDe = new DirectoryEntry(AdRootPath, "Administrator", ADpassword);
            DirectorySearcher search = new DirectorySearcher(UserDe);
            // search.Filter = "(&(&(objectCategory=person)(objecClass=user))(cn=" + UserName + "))";
            search.Filter = string.Format("Cn={0}", UserName);
            SearchResultCollection results = search.FindAll();
            if (results.Count == 0)

                return false;
            else
                return true;
        }
        /// <summary>
        /// reset user password
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ResetPassword(string accountName, string password)
        {
            entry = new DirectoryEntry(ADServiceConStr, "Administrator", ADpassword);
            using (DirectoryEntry user = entry.Children.Find("cn=" + accountName, "User"))
            {

                user.Invoke("SetPassword", new Object[] { password });
                ///
                ///changed by XiangboLiu, fixed bug ...
                ///begin
                String[] temp = { "userPassword" };
                user.RefreshCache(temp);
                ///end 
                ///
                user.CommitChanges();
            }
            return true;
        }
        /// <summary>
        /// get DirectoryEntry object by gourp name
        /// </summary>
        /// <param name="groupname"></param>
        /// <returns></returns>
        public DirectoryEntry GetGroupByGroupName(string groupname)
        {
            DirectoryEntry groupentry = new DirectoryEntry(ADServiceConStr, "Administrator", ADpassword);

            DirectorySearcher search = new DirectorySearcher(groupentry);
            search.Filter = string.Format("(&(objectClass=group)(CN={0}))", groupname);
            SearchResult searchresult = search.FindOne();
            if (searchresult != null)
            {
                entry = searchresult.GetDirectoryEntry();
            }
            return entry;
        }
        /// <summary>
        /// 
        /// </summary>                                                                                                                                                                                            
        /// <param name="AccountName"></param>
        /// <returns></returns>
        public ADAccountEntity GetUserModelbyname(string AccountName)
        {
            if (string.IsNullOrEmpty(AccountName))
            {
                return null;
            }

            ADAccountEntity model = new ADAccountEntity();
            try
            {
                //string[] strarray = AccountName.Split(',');
                //string targetname = strarray[0];
                //string namestr = strarray[0].Substring(3, targetname.Length - 3);
                DirectoryEntry userDE = ReadUserAccountInfo(AccountName);
                DirectorySearcher ds = new DirectorySearcher(userDE);
                ds.Filter = "(&(objectClass=user)(sAMAccountName=" + AccountName + "))";
                ds.PropertiesToLoad.Add("userPassword");
                ds.PropertiesToLoad.Add("sAMAccountName");
                ds.PropertiesToLoad.Add("displayName");
                ds.PropertiesToLoad.Add("description");
                ds.PropertiesToLoad.Add("telephoneNumber");
                ds.PropertiesToLoad.Add("department");
                ds.PropertiesToLoad.Add("company");
                ds.PropertiesToLoad.Add("userPrincipalName");
                SearchResult result = null;
                result = ds.FindOne();
                model = GetADUserFromSearchResult(result);
            }
            catch (Exception ex)
            {
                // LoggerManager.Logger.Error(ex);
                model = null;
            }


            return model;
        }
        /// <summary>
        /// Validate User 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ADAccountEntity ValidateUser(string username, string password)
        {
            ADAccountEntity adinfo = new ADAccountEntity();
            if (Membership.ValidateUser(username, password))
            {
                LoggerManager.Logger.Info("the username and password are correct" + username);
                if (username.Contains('@'))
                {
                    //string[] sa = username.Split('@');
                    //aduserinfo = GetUserModelbyLogin(username);
                    adinfo = GetUserModelbyname(username.Split('@')[0]);
                }
                else
                {
                    adinfo = GetUserModelbyname(username);
                }

                return adinfo;
            }
            else
            {
                LoggerManager.Logger.Info("the username and password are not correct" + username);
                return null;
            }
        }


        /// <summary>
        /// Create security group
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public bool AddGroup(string groupName)
        {
            try
            {
                if (string.IsNullOrEmpty(groupName))
                {
                    return false;
                }
                if (ObjectExists(groupName, "Group"))
                {
                    return false;
                }
                DirectoryEntry Ad = new DirectoryEntry(ADServiceConStr, "Administrator", ADpassword);

                DirectoryEntry newgroup = Ad.Children.Add("CN=" + groupName, "group");
                newgroup.Properties["samAccountName"].Add(groupName);
                newgroup.CommitChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        /// <summary>
        /// delete security group
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public bool DeleteGroup(string groupName)
        {

            try
            {
                if (string.IsNullOrEmpty(groupName))
                {
                    return false;
                }
                using (DirectoryEntry Ad = new DirectoryEntry(ADServiceConStr, "Administrator", ADpassword))
                {
                    using (DirectoryEntry group = Ad.Children.Find(groupName, "Group"))
                    {
                        Ad.Children.Remove(group);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        /// <summary>
        /// Disable user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool DisableAccount(String userName)
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                {
                    return false;
                }
                entry = new DirectoryEntry(ADServiceConStr, "Administrator", ADpassword);
                using (DirectoryEntry user = entry.Children.Find("cn=" + userName, "User"))
                {
                    user.InvokeSet("AccountDisabled", true);
                    user.CommitChanges();
                    user.Close();

                }
                return true;
            }
            catch (Exception ex)
            {
                return false;

            }
        }
        /// <summary>
        /// Enable user
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public bool EnableUserAccount(string accountName)
        {
            try
            {
                if (string.IsNullOrEmpty(accountName))
                {
                    return false;
                }
                entry = new DirectoryEntry(ADServiceConStr, "Administrator", ADpassword);
                using (DirectoryEntry user = entry.Children.Find("cn=" + accountName, "User"))
                {
                    user.InvokeSet("AccountDisabled", false);
                    user.CommitChanges();
                    user.Close();

                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// get user information by gourp name
        /// </summary>
        /// <param name="groupname"></param>
        /// <returns></returns>
        public List<ADAccountEntity> GetUserInfoBygroupname(string groupname)
        {
            StringCollection s = GetGroupMembers(groupname);
            return GetAccountinfo(s);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupname"></param>
        /// <returns></returns>
        public StringCollection GetGroupMembers(string groupname)
        {
            StringCollection groupMembers = new StringCollection();
            string ladpath = AdRootPath;
            DirectoryEntry de = new DirectoryEntry(ladpath, ADRootAdmin, ADRootPwd);
            DirectorySearcher ds = new DirectorySearcher(de);
            ds.Filter = "(&(objectClass=group)(CN=" + groupname + "))";
            SearchResultCollection resultCol = null;
            try
            {
                resultCol = ds.FindAll();
            }
            catch
            {
                return null;
            }

            foreach (SearchResult rs in resultCol)
            {
                ResultPropertyCollection resultPropColl = rs.Properties;

                foreach (Object memberColl in resultPropColl["member"])
                {

                    if (memberColl != null)
                    {
                        groupMembers.Add(memberColl.ToString());
                    }
                }
            }
            //  ds.Filter();
            return groupMembers;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strcollection"></param>
        /// <returns></returns>
        public List<ADAccountEntity> GetAccountinfo(StringCollection strcollection)
        {
            List<ADAccountEntity> accountinfolist = new List<ADAccountEntity>();
            try
            {
                if (strcollection == null)
                {
                    return null;
                }
                foreach (var item in strcollection)
                {
                    accountinfolist.Add(GetUserModel(item));
                }
            }
            catch (Exception ex)
            {
                //LoggerManager.Logger.Error(ex);
                accountinfolist = null;
            }


            return accountinfolist;
        }
        /// <summary>
        /// get userinformation by name
        /// </summary>
        /// <param name="AccountName"></param>
        /// <returns></returns>
        public ADAccountEntity GetUserModel(string AccountName)
        {
            if (string.IsNullOrEmpty(AccountName))
            {
                return null;
            }

            ADAccountEntity model = new ADAccountEntity();
            try
            {
                string[] strarray = AccountName.Split(',');//
                string targetname = strarray[0];
                string namestr = strarray[0].Substring(3, targetname.Length - 3);
                DirectoryEntry userDE = ReadUserAccountInfo(namestr);
                DirectorySearcher ds = new DirectorySearcher(userDE);
                ds.Filter = "(&(objectClass=user)(sAMAccountName=" + namestr + "))";
                ds.PropertiesToLoad.Add("userPassword");
                ds.PropertiesToLoad.Add("sAMAccountName");
                ds.PropertiesToLoad.Add("displayName");
                ds.PropertiesToLoad.Add("description");
                ds.PropertiesToLoad.Add("telephoneNumber");
                ds.PropertiesToLoad.Add("department");
                ds.PropertiesToLoad.Add("company");
                ds.PropertiesToLoad.Add("userPrincipalName");
                ds.PropertiesToLoad.Add("mail");
                ds.PropertiesToLoad.Add("homePostalAddress");
                SearchResult result = null;
                result = ds.FindOne();
                model = GetADUserFromSearchResult(result);
            }
            catch (Exception ex)
            {
                // LoggerManager.Logger.Error(ex);
                model = null;
            }


            return model;

        }
        public ADAccountEntity GetADUserFromSearchResult(SearchResult r)
        {
            ADAccountEntity result = new ADAccountEntity();
            try
            {
                if (r.GetDirectoryEntry().InvokeGet("sAMAccountName") != null)
                {
                    result.UserName = r.GetDirectoryEntry().InvokeGet("sAMAccountName").ToString();
                }
                else
                {
                    result.UserName = "";
                }
                if (r.GetDirectoryEntry().InvokeGet("mail") != null)
                {
                    result.Email = r.GetDirectoryEntry().InvokeGet("mail").ToString();
                }
                else
                {
                    result.Email = "";
                }
                if (r.GetDirectoryEntry().InvokeGet("homePostalAddress") != null)
                {
                    result.Address = r.GetDirectoryEntry().InvokeGet("homePostalAddress").ToString();
                }
                else
                {
                    result.Address = "";
                }
                if (r.GetDirectoryEntry().InvokeGet("telephoneNumber") != null)
                {
                    result.Phone = r.GetDirectoryEntry().InvokeGet("telephoneNumber").ToString();
                }
                else
                {
                    result.Phone = "";
                }
                if (r.GetDirectoryEntry().InvokeGet("displayName") != null)
                {
                    result.DisplayName = r.GetDirectoryEntry().InvokeGet("displayName").ToString();
                }
                else
                {
                    result.DisplayName = string.Empty;
                }
                if (r.GetDirectoryEntry().InvokeGet("description") != null)
                {
                    result.Description = r.GetDirectoryEntry().InvokeGet("description").ToString();
                }
                else
                {
                    result.Description = "";
                }

                if (r.GetDirectoryEntry().InvokeGet("userPrincipalName") != null)
                {
                    result.UserPrincipalName = r.GetDirectoryEntry().InvokeGet("userPrincipalName").ToString();
                }
                else
                {
                    result.UserPrincipalName = "";
                }
                if (r.GetDirectoryEntry().InvokeGet("company") != null)
                {
                    result.Company = r.GetDirectoryEntry().InvokeGet("company").ToString();
                }
                else
                {
                    result.Company = "";
                }
                if (r.GetDirectoryEntry().InvokeGet("memberOf") != null)
                {
                    List<string> grouplist = new List<string>();

                    object abc = r.GetDirectoryEntry().InvokeGet("memberOf");
                    if (abc is string)
                    {

                        grouplist.Add(abc.ToString().Split(',')[0].Substring(3));
                    }
                    else if (abc is object[])
                    {
                        object[] bb = abc as object[];
                        for (int i = 0; i < bb.Length; i++)
                        {
                            string cc = bb[i].ToString();
                            grouplist.Add(cc.Split(',')[0].Substring(3));
                        }

                    }
                    if (grouplist.Count > 0)
                    {
                        result.SecurityGroup = grouplist[0];
                    }
                }

            }
            catch (Exception ex)
            {
                // LoggerManager.Logger.Error(ex);
                result = null;
            }
            return result;
        }
        private DirectoryEntry ReadUserAccountInfo(string accountName)
        {
            if (string.IsNullOrEmpty(accountName))
            {
                return null;
            }
            try
            {
                DirectoryEntry de = GetADRootEntry();
                DirectorySearcher search = new DirectorySearcher(de);
                search.Filter = "(&(objectcategory=person)(objectclass=user)(samAccountName=" + ValidAccountNameBefore2000(accountName) + "))";
                SearchResult searchresult = search.FindOne();
                if (searchresult != null)
                    return searchresult.GetDirectoryEntry();
                return null;
            }
            catch (Exception ex)
            {
                // LoggerManager.Logger.Error(ex);
                return null;
            }
        }
        private string ValidAccountNameBefore2000(string accountName)
        {
            string accountname = string.Empty;
            try
            {
                accountname = accountName.Replace(@"/", "_").Replace(@"\", "_").Replace(@"[", "_").Replace(@"]", "_")
                   .Replace(@":", "_").Replace(@";", "_").Replace(@"|", "_").Replace(@"=", "_").Replace(@",", "_")
                   .Replace(@"+", "_").Replace(@"*", "_").Replace(@"?", "_").Replace(@"<", "_").Replace(@">", "_");
            }
            catch (Exception ex)
            {
                // LoggerManager.Logger.Error(ex);
            }
            return accountname;
        }
        /// <summary>
        /// get root catalog
        /// </summary>
        /// <returns></returns>
        private DirectoryEntry GetADRootEntry()
        {
            DirectoryEntry de = new DirectoryEntry(AdRootPath, ADRootAdmin, ADRootPwd);
            try
            {
                Guid guid = de.Guid;
            }
            catch (Exception ex)
            {
                // LoggerManager.Logger.Error(ex);
                de = null;
            }
            return de;
        }
        /// <summary>
        /// update userinformation
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool ChangeUserInfo(ADAccountEntity user)
        {
            try
            {
                DirectoryEntry userEbtry = ReadUserAccountInfo(user.UserPrincipalName);
                if (userEbtry == null)
                {
                    return false;
                }
                else
                {
                    //displayName
                    userEbtry = SetDEProperty(userEbtry, "displayName", user.DisplayName);
                    userEbtry = SetDEProperty(userEbtry, "telephoneNumber", user.Phone);
                    userEbtry = SetDEProperty(userEbtry, "Description", user.Description);
                    userEbtry = SetDEProperty(userEbtry, "mail", user.Email);
                    userEbtry = SetDEProperty(userEbtry, "homePostalAddress", user.Address);
                    userEbtry = SetDEProperty(userEbtry, "company", user.Company);
                }
                userEbtry.CommitChanges();

            }
            catch (Exception ex)
            {
                // LoggerManager.Logger.Error(ex);
                return false;
            }

            return true;
        }

        private DirectoryEntry SetDEProperty(DirectoryEntry userDE, string propertyName, string value)
        {
            try
            {
                if (userDE == null)
                {
                    return null;
                }
                if (value == "")
                {
                    if (userDE.Properties[propertyName] != null)
                    {
                        userDE.Properties[propertyName].Value = null;
                    }

                }
                else
                {
                    userDE.Properties[propertyName].Value = value;
                }
            }
            catch (Exception ex)
            {
                //  LoggerManager.Logger.Error(ex);
                return null;
            }

            return userDE;
        }

        public bool DeleteAccount(string UserAccountname)
        {
            if (string.IsNullOrEmpty(UserAccountname))
            {
                return false;
            }
            entry = GetDirectoryObject();
            DirectoryEntry NewUser = entry.Children.Find("CN=" + UserAccountname, "User");
            //entry.Children.Find("CN=" + CNName, UserAccountname);
            entry.Children.Remove(NewUser);
            entry.CommitChanges();
            return true;

        }

        private DirectoryEntry GetDirectoryObject()
        {
            try
            {
                entry = new DirectoryEntry(ADServiceConStr, ADuserName, ADpassword, AuthenticationTypes.None);
                object native = entry.NativeObject;
            }
            catch (Exception ex)
            {
                //  LoggerManager.Logger.Error(ex);
                entry = null;
            }
            return entry;
        }
    }
}
