using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.BaseInformation.Repository
{
    public class InstallStationUtility
	{

        public static BSC_SETUP_STATION UpdateEntity(BSC_SETUP_STATION entity, InstallStation model, bool isAdd)
		{
		    //entity = new BSC_SETUP_STATION();
			if (isAdd)
			{

				entity.ID = model.ID;
				entity.CLIENT_ID = model.ClientID;

			}
			entity.CLIENT_ID = model.ClientID;
			entity.NAME = model.Name;
			entity.DISTRICT_CODE = model.DistrictCode;
			entity.ADDRESS = model.Address;
			entity.DIRECTOR = model.Director;
			entity.DIRECTOR_PHONE = model.DirectorPhone;
			entity.CONTACT = model.Contact;
			entity.CONTACT_PHONE = model.ContactPhone;
			entity.EMAIL = model.Email;
			entity.NOTE = model.Note;
			entity.CREATE_TIME = model.CreateTime;
			if (model.Valid==1)
				entity.VALID = 1;
			else
				entity.VALID = 0;
			return entity;
		}

        public static InstallStation GetModel(BSC_SETUP_STATION entity)
		{
            InstallStation model = new InstallStation();
			model.ID = entity.ID;
            model.ClientID = entity.CLIENT_ID;
			model.Name = entity.NAME;
            model.DistrictCode = entity.DISTRICT_CODE;
			model.Address = entity.ADDRESS;
			model.Director = entity.DIRECTOR;
            model.DirectorPhone = entity.DIRECTOR_PHONE;
			model.Contact = entity.CONTACT;
            model.ContactPhone = entity.CONTACT_PHONE;
			model.Email = entity.EMAIL;
			model.Note = entity.NOTE;
            model.CreateTime = (entity.CREATE_TIME).ToLocalTime();
			if (entity.VALID == 1)
			{
				model.Valid = 1;
			}
			else
			{
				model.Valid = 0;
			}
			return model;
		}

	}
}

