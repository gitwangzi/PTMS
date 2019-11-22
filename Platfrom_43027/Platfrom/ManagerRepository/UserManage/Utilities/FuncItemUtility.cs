using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Manager.Contract.Data;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.Manager.Repository
{
	public class FuncItemUtility
	{
		public static USR_FUNC_ITEM GetEntity(FuncItem model)
		{
            USR_FUNC_ITEM entity = new USR_FUNC_ITEM();
			entity.ID = model.ID;
			entity.NAME = model.Name;
			entity.URL = model.Note;
			entity.VERSION = model.Version;
            entity.MODULE = model.Module;
			return entity;
		}

        public static FuncItem GetModel(USR_FUNC_ITEM entity)
		{
            FuncItem model = new FuncItem();
			model.ID = entity.ID;
			model.Name = entity.NAME;
            model.Note = entity.URL;
            model.Module = entity.MODULE;
			model.Version = entity.VERSION;
			return model;
		}

	}
}

