using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Manager.Contract.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Gsafety.PTMS.Manager.Repository
{
    public class FuncItemRepository
    {
        public static MultiMessage<FuncItem> GetAllItems(PTMSEntities context)
        {
            ObservableCollection<FuncItem> list = new ObservableCollection<FuncItem>();
            
            foreach (var item in context.USR_FUNC_ITEM)
            {
                FuncItem model = FuncItemUtility.GetModel(item);
                list.Add(model);
            }

            return new MultiMessage<FuncItem>(list, list.Count);
        }
    }
}
