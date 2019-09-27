using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.BaseInformation.Repository
{
    public class BscDistrictUtility
    {

        public static BSC_DISTRICT UpdateEntity(BSC_DISTRICT entity, BscDistrict model, bool isAdd)
        {
            BSC_DISTRICT _entity = new BSC_DISTRICT();
            if (isAdd)
            {

                _entity.CODE = model.Code;

            }
            _entity.NAME = model.Name;
            _entity.FULLNAME = model.Fullname;
            _entity.SHORTNAME = model.Shortname;
            _entity.NOTE = model.Note;
            if ((short)model.Valid > 0)
                _entity.VALID = 1;
            else
                _entity.VALID = 0;
            return _entity;
        }

        public static BscDistrict GetModel(BSC_DISTRICT entity)
        {
            BscDistrict model = new BscDistrict();
            model.Code = entity.CODE;
            model.Name = entity.NAME;
            model.Fullname = entity.FULLNAME;
            model.Shortname = entity.SHORTNAME;
            model.Note = entity.NOTE;
            if ((short)entity.VALID == 0)
            {
                model.Valid = 0;
            }
            else
            {
                model.Valid = 1;
            }
            return model;
        }

    }
}

