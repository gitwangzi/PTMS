using Gsafety.Ant.BaseInformation.Repository.Utilties;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.DBEntity;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: cd183807-81a3-4567-884e-61e4a02020f1      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Repository
/////    Project Description:    
/////             Class Name: InstallStationRepository
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/15 17:43:57
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/15 17:43:57
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Transactions;

namespace Gsafety.PTMS.BaseInformation.Repository
{
    public class InstallStationRepository : BaseRepository
    {
        public SingleMessage<bool> DeleteInstallStation(string id)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                bool reference = context.BSC_SETUPSTATION_USER.Any(n => n.STATION_ID == id);
                if (reference)
                {
                    return new SingleMessage<bool>(false, "ReferenceByUser");
                }
                reference = context.MTN_INSTALLATION_DETAIL.Any(n => n.STATION_ID == id);
                if (reference)
                {
                    return new SingleMessage<bool>(false, "ReferenceBySuiteInstall");
                }
                reference = context.MTN_GPS_INSTALLATION_DETAIL.Any(n => n.STATION_ID == id);
                if (reference)
                {
                    return new SingleMessage<bool>(false, "ReferenceByGPSInstall");
                }
                reference = context.MTN_MAINTAIN_APPLICATION.Any(n => n.SETUP_STATION == id);
                if (reference)
                {
                    return new SingleMessage<bool>(false, "ReferenceByMaintain");
                }
                BSC_SETUP_STATION entity = context.BSC_SETUP_STATION.FirstOrDefault(t => t.ID == id);

                entity.VALID = 0;
                if (context.SaveChanges() > 0)
                {
                    return new SingleMessage<bool>(true);
                }
                else
                {
                    return new SingleMessage<bool>(false, "FailedToSave");
                }

            }
        }

        public SingleMessage<bool> UpdateInstallStation(InstallStation installStation)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                var result = context.BSC_SETUP_STATION.Where(x => x.ID != installStation.ID && x.VALID == 1 && x.NAME == installStation.Name).FirstOrDefault();
                if (result != null)
                {
                    return new SingleMessage<bool>(false, "SameNameExist");
                }
                result = context.BSC_SETUP_STATION.Where(x => x.ID == installStation.ID && x.VALID == 1).FirstOrDefault();

                if (result == null)
                {
                    return new SingleMessage<bool>(false, "NotExist");
                }
                InstallStationUtility.UpdateEntity(result, installStation, false);
                context.Entry(result).State = EntityState.Modified;
                if (context.SaveChanges() > 0)
                {
                    return new SingleMessage<bool>(true);
                }
                else
                {
                    return new SingleMessage<bool>(false, "FailedToSave");
                }
            }
        }
        /// <summary>
        /// save install
        /// </summary>
        /// <param name="installStation"></param>
        /// <returns></returns>
        public SingleMessage<bool> AddInstallStation(InstallStation installStation)
        {
            BSC_SETUP_STATION item = new BSC_SETUP_STATION();
            item = InstallStationUtility.UpdateEntity(item, installStation, true);
            using (var context = new PTMSEntities())
            {
                var result = context.BSC_SETUP_STATION.Where(x => x.NAME == installStation.Name && x.VALID == 1 && installStation.ClientID == x.CLIENT_ID).FirstOrDefault();
                if (result != null)
                {
                    return new SingleMessage<bool>(false, "SameNameExist");
                }
                else
                {
                    context.BSC_SETUP_STATION.Add(item);
                    if (context.SaveChanges() > 0)
                    {
                        return new SingleMessage<bool>(true);
                    }
                    else
                    {
                        return new SingleMessage<bool>(false, "FailedToSave");
                    }
                }
            }
        }

        // 多条件查询
        public List<InstallStation> GetInstallStationsFuzzy(string districtCode, string param, string name, PagingInfo page, out int totalRecord, UserInfoMessageHeader header, string clientID)
        {
            using (var context = new PTMSEntities())
            {
                #region oldsourse
                //Expression<Func<DISTRICT_LEVEL_VIEW, bool>> Exp = base.GetDistrictExpression(header);
                //var InstalledStationID = _context.MTN_INSTALLATION_DETAIL.Where(x => x.VALID == 1).GroupBy(x => x.STATION_ID).Select(x => x.Key).ToList();
                //var result = (from x in _context.BSC_SETUP_STATION.Where(item => item.VALID == 1)
                //              join y in _context.DISTRICT_LEVEL_VIEW.Where(Exp) on x.DISTRICT_CODE equals y.CODE
                //              where (string.IsNullOrEmpty(name) ? true : x.NAME.Contains(name))
                //              && (string.IsNullOrEmpty(districtCode) ? true : y.CODE.StartsWith(districtCode))
                //              select new InstallStation
                //              {
                //                  Address = x.ADDRESS,
                //                  Contact = x.CONTACT,
                //                  ContactPhone = x.CONTACT_PHONE,
                //                  Director = x.DIRECTOR,
                //                  DirectorPhone = x.DIRECTOR_PHONE,
                //                  Email = x.EMAIL,
                //                  //Id = x.ID,
                //                  //Name = x.NAME,
                //                  //Note = x.NOTE,
                //                  //CityCode = y.CODE,
                //                  //CityName = y.CITY,
                //                  //ProvinceCode = y.CODE.Substring(0, 2),
                //                  //ProvinceName = y.PROVINCE,
                //                  //DeleteFlag = InstalledStationID.Contains(x.ID) ? false : true,
                //              });
                //if (page == null || page.PageIndex == -1)
                //{
                //    totalRecord = result.Count();
                //    return result.ToList();
                //}
                //else
                //{
                //    totalRecord = result.Count();
                //    return result.OrderBy(x => x.Name).Skip(page.PageSize * (page.PageIndex - 1)).Take(page.PageSize).ToList();
                //} 
                #endregion

                List<InstallStation> result = new List<InstallStation>();
                var sour = from x in context.BSC_SETUP_STATION
                           join c in context.BSC_DISTRICT on x.DISTRICT_CODE equals c.CODE
                           into comdir
                           from c in comdir.DefaultIfEmpty()
                           // join y in _context.BSC_DISTRICT on x.DISTRICT_CODE equals y.CODE
                           //join z in _context.BSC_DISTRICT on y.CODE.Substring(0, 2) equals z.CODE                             
                           where x.VALID == 1 && x.CLIENT_ID == clientID &&
                               //x.DIRECTOR.Contains(param)
                               //x.DISTRICT_CODE.Contains(districtCode)
                               //x.NAME.Contains(name)
                               //(districtCode == null || districtCode == "") ? true : x.DISTRICT_CODE.Contains(districtCode) &&
                               //(param == null || param == "") ? true : x.DIRECTOR.Contains(param) &&
                               //(name == null || name == "") ? true : x.NAME.Contains(name)
                           (string.IsNullOrEmpty(districtCode) ? true : c.NAME.Contains(districtCode.ToUpper())) &&
                           (string.IsNullOrEmpty(param) ? true : x.DIRECTOR.ToUpper().Contains(param.ToUpper())) &&
                           (string.IsNullOrEmpty(name) ? true : x.NAME.ToUpper().Contains(name.ToUpper()))
                           select x;

                List<BSC_SETUP_STATION> entitylist = null;
                if (page == null || page.PageIndex == -1)
                {
                    totalRecord = sour.Count();
                    entitylist = sour.OrderBy(t => t.CREATE_TIME)
                        .Skip(page.PageSize * (page.PageIndex - 1))
                        .Take(page.PageSize)
                        .ToList();
                }
                else
                {
                    totalRecord = sour.Count();
                    entitylist = sour.OrderByDescending(t => t.CREATE_TIME).Skip(page.PageSize * (page.PageIndex - 1)).Take(page.PageSize).ToList();
                }

                foreach (var item in entitylist)
                {
                    result.Add(InstallStationUtility.GetModel(item));
                }

                return result;
            }
        }

        public List<InstallStation> GetInstallStations(string clientID)
        {
            using (PTMSEntities _context = new PTMSEntities())
            {
                var result = (from x in _context.BSC_SETUP_STATION.Where(item => item.VALID == 1)
                              join y in _context.BSC_DISTRICT on x.DISTRICT_CODE equals y.CODE
                              join z in _context.BSC_DISTRICT on y.CODE.Substring(0, 2) equals z.CODE
                              where x.CLIENT_ID == clientID && x.VALID == 1
                              select new InstallStation
                              {
                                  Address = x.ADDRESS,
                                  Contact = x.CONTACT,
                                  ContactPhone = x.CONTACT_PHONE,
                                  Director = x.DIRECTOR,
                                  DirectorPhone = x.DIRECTOR_PHONE,
                                  Email = x.EMAIL,
                                  ID = x.ID,
                                  Name = x.NAME,
                                  Note = x.NOTE,
                                  //CityCode = y.CODE,
                                  //CityName = y.NAME,
                                  //ProvinceCode = z.CODE,
                                  //ProvinceName = z.NAME,
                              });
                return result.ToList();
            }
        }

        public List<InstallStation> GetInstallStationsByAlphabet(UserInfoMessageHeader userInfo, PagingInfo page, out int totalRecord, string clientID)
        {
            using (PTMSEntities _context = new PTMSEntities())
            {
                List<InstallStation> result = new List<InstallStation>();
                var sour = from x in _context.BSC_SETUP_STATION
                           // join y in _context.BSC_DISTRICT on x.DISTRICT_CODE equals y.CODE
                           //join z in _context.BSC_DISTRICT on y.CODE.Substring(0, 2) equals z.CODE                             
                           where x.VALID == 1 && x.CLIENT_ID == clientID
                           select x;
                List<BSC_SETUP_STATION> entitylist = null;
                if (page == null || page.PageIndex == -1)
                {
                    totalRecord = sour.Count();
                    entitylist = sour.OrderBy(t => t.CREATE_TIME)
                        .Skip(page.PageSize * (page.PageIndex - 1))
                        .Take(page.PageSize)
                        .ToList();

                }
                else
                {
                    totalRecord = sour.Count();
                    entitylist = sour.OrderBy(t => t.CREATE_TIME).Skip(page.PageSize * (page.PageIndex - 1)).Take(page.PageSize).ToList();
                }

                foreach (var item in entitylist)
                {
                    result.Add(InstallStationUtility.GetModel(item));
                }

                return result;
            }
        }

        public InstallStation GetInstallStationByID(string id)
        {
            using (PTMSEntities _context = new PTMSEntities())
            {
                var result = (from x in _context.BSC_SETUP_STATION
                              join y in _context.BSC_DISTRICT on x.DISTRICT_CODE equals y.CODE
                              join z in _context.BSC_DISTRICT on y.CODE.Substring(0, 2) equals z.CODE
                              where x.ID == id && x.VALID == 1
                              select new InstallStation
                              {
                                  Address = x.ADDRESS,
                                  Contact = x.CONTACT,
                                  ContactPhone = x.CONTACT_PHONE,
                                  Director = x.DIRECTOR,
                                  DirectorPhone = x.DIRECTOR_PHONE,
                                  Email = x.EMAIL,
                                  //Id = x.ID,
                                  //Name = x.NAME,
                                  //Note = x.NOTE,
                                  //CityCode = y.CODE,
                                  //CityName = y.NAME,
                                  //ProvinceCode = z.CODE,
                                  //ProvinceName = z.NAME,
                              }).FirstOrDefault();
                return result;
            }
        }

        public bool CheckInstallStationExistByName(string name)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                var result = context.BSC_SETUP_STATION.Any(x => x.NAME == name && x.VALID == 1);
                return result;
            }
        }

        public bool CheckInstallDetailById(string Id)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                var result = (from x in context.MTN_INSTALLATION_DETAIL
                              where x.STATION_ID == Id && x.VALID == 1
                              select x).ToList();
                if (result.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool BatchAdd(List<InstallStation> installBatchList)
        {
            TransactionOptions optons = new TransactionOptions();
            optons.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            var scope = new TransactionScope(TransactionScopeOption.Required, optons);

            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    if (installBatchList != null)
                    {
                        BSC_SETUP_STATION stationtable = new BSC_SETUP_STATION();
                        for (int i = 0; i < installBatchList.Count; i++)
                        {
                            var item = installBatchList[i];
                            if (context.BSC_SETUP_STATION.Any(x => x.NAME == item.Name && x.VALID == 1))
                            {
                                continue;
                            }
                            context.BSC_SETUP_STATION.Add(new BSC_SETUP_STATION
                            {
                                ID = Guid.NewGuid().ToString(),
                                CLIENT_ID = item.ClientID,
                                NAME = item.Name,
                                ADDRESS = item.Address,
                                CONTACT = item.Contact,
                                CONTACT_PHONE = item.ContactPhone,
                                DIRECTOR = item.Director,
                                DIRECTOR_PHONE = item.DirectorPhone,
                                DISTRICT_CODE = item.DistrictCode,
                                EMAIL = item.Email,
                                NOTE = item.Note,
                                CREATE_TIME = DateTime.UtcNow,
                                VALID = 1
                            });
                        }
                        context.SaveChanges();
                        scope.Complete();
                        return true;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                scope.Dispose();
            }
        }

        public MultiMessage<InstallStation> BatchCheckInstallStationExist(List<InstallStation> installStationList)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                //Batch Check
                Func<InstallStation, bool> filter = tv => context.BSC_DISTRICT.Local.Any(v => v.VALID == 1 && v.NAME == tv.Name)
                     || context.BSC_DISTRICT.Any(v => v.VALID == 1 && v.NAME == tv.Name);

                var list = installStationList.Where(filter).ToList();
                return new MultiMessage<InstallStation>(list, list.Count) { IsSuccess = true };
            }
        }

        public List<InstallStationUser> GetInstallStationUser(string installStationID, string clientID)
        {
            List<InstallStationUser> result = new List<InstallStationUser>();
            using (var context = new PTMSEntities())
            {
                var sour = from x in context.BSC_SETUPSTATION_USER
                           join i in context.BSC_SETUP_STATION on x.STATION_ID equals i.ID
                           where i.CLIENT_ID == clientID && i.VALID == 1 && x.STATION_ID == installStationID
                           select x;
                foreach (var item in sour)
                {
                    result.Add(InstallStationUserUtility.GetModel(item));
                }
                return result;
            }
        }

        public SingleMessage<bool> SaveInstallStationUser(ObservableCollection<InstallStationUser> installStationUser)
        {
            try
            {
                using (var context = new PTMSEntities())
                {
                    TransactionOptions optons = new TransactionOptions();
                    optons.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                    var scope = new TransactionScope(TransactionScopeOption.Required, optons);
                    try
                    {
                        foreach (var chve in installStationUser)
                        {
                            BSC_SETUPSTATION_USER item = new BSC_SETUPSTATION_USER();
                            item = InstallStationUserUtility.UpdateEntity(item, chve);
                            BSC_SETUPSTATION_USER source = context.BSC_SETUPSTATION_USER.FirstOrDefault(x => (x.USER_ID == chve.UserID) && (x.STATION_ID == chve.InstallStationID));
                            if (source == null && chve.ID != null)
                            {
                                context.BSC_SETUPSTATION_USER.Add(item);
                            }
                            else if (source != null && chve.ID == null)
                            {
                                context.BSC_SETUPSTATION_USER.Attach(source);
                                context.BSC_SETUPSTATION_USER.Remove(source);
                            }
                        }

                        context.SaveChanges();

                        scope.Complete();

                        return new SingleMessage<bool>(true);
                    }
                    catch (Exception ex)
                    {
                        return new SingleMessage<bool>(false, ex.Message);
                    }
                    finally
                    {
                        scope.Dispose();
                    }

                }
            }
            catch (Exception ex)
            {
                return new SingleMessage<bool>(false, ex.Message);
            }
            return new SingleMessage<bool>(true);
        }

        public List<InstallStation> GetInstallStationsByUser(string userID)
        {
            using (PTMSEntities _context = new PTMSEntities())
            {
                var result = (from x in _context.BSC_SETUP_STATION.Where(item => item.VALID == 1)
                              join su in _context.BSC_SETUPSTATION_USER on x.ID equals su.STATION_ID
                              where su.USER_ID == userID
                              select new InstallStation
                              {
                                  Name = x.NAME,
                                  ID = x.ID
                              });
                return result.ToList();
            }
        }
    }
}
