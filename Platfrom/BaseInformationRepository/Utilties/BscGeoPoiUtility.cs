using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.BaseInformation.Repository
{
	public class BscGeoPoiUtility
	{

        public static BSC_GEO_POI UpdateEntity(BSC_GEO_POI entity, BscGeoPoi model, bool isAdd)
		{
			if (isAdd)
			{

				entity.ID = model.ID;

			}
			entity.NAME = model.Name;
			entity.LONGITUDE = model.Longitude;
			entity.LATIDUE = model.Latidue;
			entity.ADDRESS = model.Address;
			entity.CONTRY = model.Contry;
			entity.PROPERTY = model.Property;
			entity.DATASTATUS = (short)model.Datastatus;
			return entity;
		}

		public static BscGeoPoi GetModel(BSC_GEO_POI entity)
		{
			BscGeoPoi model = new BscGeoPoi();
			model.ID = entity.ID;
			model.Name = entity.NAME;
			model.Longitude = entity.LONGITUDE;
			model.Latidue = entity.LATIDUE;
			model.Address = entity.ADDRESS;
			model.Contry = entity.CONTRY;
			model.Property = entity.PROPERTY;
			model.Datastatus = entity.DATASTATUS;
			return model;
		}

	}
}

