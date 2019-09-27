using Gsafety.Ant.BaseInformation.Repository.Utilties;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Transactions;

namespace Gsafety.Ant.BaseInformation.Repository
{
    public class ChauffeurRepository : BaseRepository
    {

        public SingleMessage<bool> AddChauffeur(Chauffeur chauffeur)
        {
            BSC_CHAUFFEUR chauEntity = new BSC_CHAUFFEUR();
            chauEntity = ChauffeurUtility.UpdateEntity(chauEntity, chauffeur, true);

            using (var context = new PTMSEntities())
            {
                var result = context.BSC_CHAUFFEUR.Where(x => x.ICARD_ID == chauffeur.ICardID && x.VALID == 1).FirstOrDefault();
                var result2 = context.BSC_CHAUFFEUR.Where(x => x.DRIVER_LICENSE == chauffeur.DriverLicense && x.VALID == 1).FirstOrDefault();
                if (result != null)
                {
                    var res = new SingleMessage<bool>(false, "Validate_IDCardRepeat");
                    res.ErrorMsg = "Validate_IDCardRepeat";
                    return res;
                }
                else if (result2 != null)
                {
                    var res = new SingleMessage<bool>(false, "Validate_DriverLiceseRepeat");
                    res.ErrorMsg = "Validate_DriverLiceseRepeat";
                    return res;
                }

                context.BSC_CHAUFFEUR.Add(chauEntity);
                if (context.SaveChanges() > 0)
                {
                    return new SingleMessage<bool>(true);
                }
                else
                {
                    return new SingleMessage<bool>(false);
                }
            }
        }

        public SingleMessage<bool> DeleteChauffeur(string id)
        {
            using (var context = new PTMSEntities())
            {
                bool reference = context.BSC_VEHICLE_CHAUFFEUR.Any(n => n.CHAUFFEUR_ID == id);
                if (reference)
                {
                    return new SingleMessage<bool>(false, "ReferenceByUser");
                }
                BSC_CHAUFFEUR entity = context.BSC_CHAUFFEUR.FirstOrDefault(t => t.ID == id);
                if (entity == null)
                {
                    return new SingleMessage<bool>(false, "NotExist");
                }
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

        public SingleMessage<bool> UpdateChauffeur(Chauffeur chauffeur)
        {
            using (var context = new PTMSEntities())
            {
                bool exist = context.BSC_CHAUFFEUR.Any(x => x.ID != chauffeur.ID && x.VALID == 1 && chauffeur.DriverLicense == x.DRIVER_LICENSE);
                if (exist)
                {
                    return new SingleMessage<bool>(false, "DriverLicenseDuplicate");
                }

                exist = context.BSC_CHAUFFEUR.Any(x => x.ID != chauffeur.ID && x.VALID == 1 && chauffeur.ICardID == x.ICARD_ID);
                if (exist)
                {
                    return new SingleMessage<bool>(false, "IDCardDuplicate");
                }
                var result = context.BSC_CHAUFFEUR.Where(x => x.ID == chauffeur.ID && x.VALID == 1).FirstOrDefault();
                if (result == null)
                {
                    return new SingleMessage<bool>(false, "NotExist");
                }
                bool reference = context.BSC_VEHICLE_CHAUFFEUR.Any(n => n.CHAUFFEUR_ID == chauffeur.ID);
                if (reference)
                {
                    return new SingleMessage<bool>(false, "ReferenceByUser");
                }
                ChauffeurUtility.UpdateEntity(result, chauffeur, false);
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

        public List<Chauffeur> GetChauffeurByPage(UserInfoMessageHeader userInfo, PagingInfo page, out int totalRecord, string clientID)
        {
            using (PTMSEntities _context = new PTMSEntities())
            {
                List<Chauffeur> result = new List<Chauffeur>();
                var sour = from x in _context.BSC_CHAUFFEUR

                           where x.VALID == 1 && x.CLIENT_ID == clientID
                           select x;
                List<BSC_CHAUFFEUR> entitylist = null;
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
                    result.Add(ChauffeurUtility.GetModel(item));
                }

                return result;
            }
        }

        public List<Chauffeur> GetChauffeurByCondition(string name, string licence, string icard, PagingInfo page, out int totalRecord, UserInfoMessageHeader userInfoMessageHeader, string clientID)
        {
            using (var _context = new PTMSEntities())
            {

                List<Chauffeur> result = new List<Chauffeur>();
                var sour = from x in _context.BSC_CHAUFFEUR
                           where x.VALID == 1 && x.CLIENT_ID == clientID &&

                            ((name == null || name == "") ? true : x.NAME.ToUpper().Contains(name.ToUpper())) &&
                           ((licence == null || licence == "") ? true : x.DRIVER_LICENSE.Contains(licence)) &&
                           ((icard == null || icard == "") ? true : x.ICARD_ID.Contains(icard))

                           select x;
                List<BSC_CHAUFFEUR> entitylist = null;
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
                    result.Add(ChauffeurUtility.GetModel(item));
                }
                return result;

            }
        }

        public List<ChauffeurVehicle> GetChauffeurVehicle(string ChauffeurID, string clientID)
        {
            List<ChauffeurVehicle> result = new List<ChauffeurVehicle>();
            using (var context = new PTMSEntities())
            {
                var sour = from x in context.BSC_VEHICLE_CHAUFFEUR
                           join i in context.BSC_CHAUFFEUR on x.CHAUFFEUR_ID equals i.ID
                           where i.CLIENT_ID == clientID && i.VALID == 1 && x.CHAUFFEUR_ID == ChauffeurID
                           select x;
                foreach (var item in sour)
                {
                    result.Add(ChauffeurVehicleUtility.GetModel(item));
                }
                return result;
            }
        }

        public SingleMessage<bool> SaveChauffeurVehicle(PTMSEntities context, ObservableCollection<ChauffeurVehicle> chauffeurVehicle)
        {

            try
            {
                List<string> drivers = chauffeurVehicle.Select(n => n.ChauffeurID).ToList();
                List<BSC_VEHICLE_CHAUFFEUR> itemsfromdatabase = context.BSC_VEHICLE_CHAUFFEUR.Where(n => drivers.Contains(n.CHAUFFEUR_ID)).ToList();
                List<BSC_CHAUFFEUR> chaufeurs = context.BSC_CHAUFFEUR.Where(n => drivers.Contains(n.ID)).ToList();

                bool change = false;

                foreach (var item in chaufeurs)
                {
                    List<string> vehicles = new List<string>();
                    foreach (var cv in chauffeurVehicle)
                    {
                        if (item.ID == cv.ChauffeurID && cv.ID == null)
                        {
                            foreach (var exist in itemsfromdatabase)
                            {
                                if (cv.VehicleID == exist.VEHICLE_ID && cv.ChauffeurID == item.ID)
                                {
                                    vehicles.Add(cv.VehicleID);
                                    break;
                                }
                            }
                        }
                    }

                    if (vehicles.Count != 0)
                    {
                        var entity = context.RUN_MOBILE_WORKING.FirstOrDefault(n => n.MOBILE_NUMBER == item.CELLPHONE && vehicles.Contains(n.VEHICLE_ID));
                        if (entity != null)
                        {
                            return new SingleMessage<bool>(false, entity.VEHICLE_ID + "ReferencedbyMobile");
                        }
                    }
                }

                foreach (var chve in chauffeurVehicle)
                {
                    BSC_VEHICLE_CHAUFFEUR sour = itemsfromdatabase.FirstOrDefault(x => (x.CHAUFFEUR_ID == chve.ChauffeurID) && (x.VEHICLE_ID == chve.VehicleID));
                    if (sour == null && chve.ID != null)
                    {
                        BSC_VEHICLE_CHAUFFEUR item = new BSC_VEHICLE_CHAUFFEUR();
                        item = ChauffeurVehicleUtility.UpdateEntity(item, chve);
                        context.BSC_VEHICLE_CHAUFFEUR.Add(item);
                        change = true;
                    }
                    else if (sour != null && chve.ID == null)
                    {
                        context.BSC_VEHICLE_CHAUFFEUR.Remove(sour);
                        change = true;
                    }
                }

                if (change)
                {
                    return context.Save();
                }
                else
                {
                    return new SingleMessage<bool>(true);
                }
            }
            catch (Exception ex)
            {
                return new SingleMessage<bool>(false, ex.Message);
            }
        }

        public static SingleMessage<AuthenticationResult> Authenticate(PTMSEntities context, string simNumber, string vehicleNumber, string license, string operationlicense)
        {
            var result = (from c in context.BSC_CHAUFFEUR
                          join cv in context.BSC_VEHICLE_CHAUFFEUR on c.ID equals cv.CHAUFFEUR_ID
                          join v in context.BSC_VEHICLE on cv.VEHICLE_ID equals v.VEHICLE_ID
                          where c.CELLPHONE == simNumber && cv.VEHICLE_ID == vehicleNumber
                          && c.VALID == 1 && c.DRIVER_LICENSE == license && v.OPERATION_LICENSE == operationlicense
                          select c).FirstOrDefault();
            SingleMessage<AuthenticationResult> ret = new SingleMessage<AuthenticationResult>();
            ret.Result = new AuthenticationResult();
            if (result != null)
            {
                RUN_MOBILE_WORKING working = context.RUN_MOBILE_WORKING.FirstOrDefault(n => n.MOBILE_NUMBER == simNumber);
                if (working != null)
                {
                    if (working.VEHICLE_ID != vehicleNumber)
                    {
                        ret.IsSuccess = false;
                        ret.ErrorMsg = "AlreadyBindingWith " + working.VEHICLE_ID;

                        return ret;
                    }
                }
                else
                {
                    working = new RUN_MOBILE_WORKING();
                    working.CLIENT_ID = result.CLIENT_ID;
                    working.MOBILE_NUMBER = simNumber;
                    working.VEHICLE_ID = vehicleNumber;

                    BSC_VEHICLE vehicle = (from v in context.BSC_VEHICLE
                                           where v.VEHICLE_ID == vehicleNumber && v.VALID == 1
                                           select v).FirstOrDefault();
                    if (vehicle != null)
                    {
                        working.ORGANIZATION_ID = vehicle.ORGNIZATION_ID;
                    }

                    context.RUN_MOBILE_WORKING.Add(working);

                    context.Save();
                }

                ret.IsSuccess = true;

                BSC_CHAUFFEUR chauffeur = context.BSC_CHAUFFEUR.FirstOrDefault(n => n.CELLPHONE == simNumber && n.DRIVER_LICENSE == license);
                ret.Result.Name = chauffeur.NAME;
                ret.Result.ClientID = chauffeur.CLIENT_ID;
                ret.Result.AuthCode = simNumber + ";" + vehicleNumber;
                ret.Result.UserID = chauffeur.ID;

                return ret;
            }
            else
            {
                return new SingleMessage<AuthenticationResult>(false, "NotFound");
            }
        }

        public MultiMessage<Chauffeur> GetChauffeurByVehicle(PTMSEntities context, string vehicleID, string clientID)
        {

            var sour = from c in context.BSC_CHAUFFEUR
                       join cv in context.BSC_VEHICLE_CHAUFFEUR
                       on c.ID equals cv.CHAUFFEUR_ID
                       where c.VALID == 1 && cv.VEHICLE_ID == vehicleID && c.CLIENT_ID == clientID
                       select c;

            List<BSC_CHAUFFEUR> entitylist = sour.ToList();

            List<Chauffeur> result = new List<Chauffeur>();
            foreach (var item in entitylist)
            {
                result.Add(ChauffeurUtility.GetModel(item));
            }

            return new MultiMessage<Chauffeur>(result, result.Count);

        }

        public static SingleMessage<bool> UnBind(PTMSEntities context, string num, string vehicleid)
        {
            RUN_MOBILE_WORKING working = context.RUN_MOBILE_WORKING.FirstOrDefault(n => n.MOBILE_NUMBER == num);
            if (working != null)
            {
                if (working.VEHICLE_ID == vehicleid)
                {
                    context.RUN_MOBILE_WORKING.Remove(working);

                    context.Save();
                }
            }

            return new SingleMessage<bool>(true);
        }

        public SingleMessage<bool> BatchAdd(List<Chauffeur> chauffeurList)
        {
            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    BSC_CHAUFFEUR stationtable = new BSC_CHAUFFEUR();
                    for (int i = 0; i < chauffeurList.Count; i++)
                    {
                        var item = chauffeurList[i];
                        if (context.BSC_CHAUFFEUR.Any(x => x.ICARD_ID == item.ICardID && x.DRIVER_LICENSE == item.DriverLicense && x.VALID == 1))
                        {
                            continue;
                        }
                        context.BSC_CHAUFFEUR.Add(new BSC_CHAUFFEUR
                        {
                            ID = Guid.NewGuid().ToString(),
                            CLIENT_ID = item.ClientID,
                            NAME = item.Name,
                            ICARD_ID = item.ICardID,
                            DRIVER_LICENSE = item.DriverLicense,
                            CELLPHONE = item.CellPhone,
                            CREATOR = item.Creator,
                            PHONE = item.Phone,
                            ADDRESS = item.Address,
                            EMAIL = item.Email,
                            NOTE = item.Note,
                            CREATE_TIME = DateTime.UtcNow,
                            VALID = 1
                        });
                    }
                    context.Save();
                    return new SingleMessage<bool>() { IsSuccess = true };
                }
            }
            catch (Exception ex)
            {
                return new SingleMessage<bool>() { IsSuccess = false, ExceptionMessage = ex }; ;
            }
        }

        public MultiMessage<Chauffeur> BatchCheckInstallStationExist(List<Chauffeur> chauffeurList)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                //Batch Check
                Func<Chauffeur, bool> filter = tv => context.BSC_CHAUFFEUR.Local.Any(v => v.VALID == 1 && v.ICARD_ID == tv.ICardID && v.DRIVER_LICENSE == tv.DriverLicense)
                     || context.BSC_CHAUFFEUR.Any(v => v.VALID == 1 && v.ICARD_ID == tv.ICardID && v.DRIVER_LICENSE == tv.DriverLicense);

                var list = chauffeurList.Where(filter).ToList();
                return new MultiMessage<Chauffeur>(list, list.Count) { IsSuccess = true };
            }
        }
    }
}
