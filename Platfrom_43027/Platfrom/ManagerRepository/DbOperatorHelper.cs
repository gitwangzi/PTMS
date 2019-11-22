/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ebe8e57b-7f1d-449c-b241-a8ae8d3d0a45      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGJINCAI
/////                 Author: TEST(JinCaiWang)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Contract.Data
/////    Project Description:    
/////             Class Name: AlarmInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/8 11:07:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/8 11:07:41
/////            Modified by:
/////   Modified Description: 
/////======================================================================

using Gsafety.PTMS.Manager.Contract.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Repository;
namespace Gsafety.PTMS.Manager.Repository
{
    public class DbOperatorHelper
    {
        public List<ConfigTree> GetAppConfigs(PTMSEntities context, string name = null)
        {

            var allSection = context.CFG_APP_CONFIG.ToList();
            var roots = new List<ConfigTree>();

            if (allSection != null)
            {
                //Func<CFG_APP_CONFIG, bool> express = x => x.PARENT_ID == null;
                //if (!string.IsNullOrWhiteSpace(name))
                //{
                //    express = x => x.PARENT_ID == null && x.SECTION_NAME == name;
                //}

                //roots = allSection.Where(express)
                //       .Select(x => new ConfigTree { Value = x.ToConvert<ConfigItem>(), })
                //       .ToList();

                roots.ForEach(x => AddChildren(allSection, x));
            }
            return roots;

        }

        private void AddChildren(List<CFG_APP_CONFIG> allSection, ConfigTree parent)
        {
            if (parent == null || parent.Value == null) return;
            //var q = allSection.Where(x => x.PARENT_ID == parent.Value.Id);
            //foreach (var x in q)
            //{
            //    var item = new ConfigItem();
            //    item.Id = x.ID;
            //    //item.ParentId = x.PARENT_ID;
            //    item.SectionName = x.SECTION_NAME;
            //    x.SECTION_DESC = x.SECTION_DESC;
            //    //x.SECTION_LEVEL = x.SECTION_LEVEL;
            //    x.SECTION_TYPE = x.SECTION_TYPE;
            //    x.SECTION_VALUE = x.SECTION_VALUE;
            //    var child = new ConfigTree { Value = item };
            //    AddChildren(allSection, child);
            //    parent.Children.Add(child);
            //}
        }

        public bool IsExistsSection(PTMSEntities _context, string id, string sectionName, string parentName)
        {

            //return _context.CFG_APP_CONFIG.Any(x => x.ID != id && x.PARENT_ID == parentName && x.SECTION_NAME == sectionName);
            return false;
        }

        public Boolean AddOrUpdateModel(ConfigTree model, PTMSEntities _context)
        {

            CollectionChange(_context, model);

            return _context.SaveChanges() > 0 ? true : false;

        }

        public void AddOrUpdateModel(PTMSEntities context, ConfigTree model)
        {
            CollectionChange(context, model);
        }

        private void CollectionChange(PTMSEntities context, ConfigTree model)
        {
            if (model == null || model.Value == null)
            {
                return;
            }

            // var dbValue = model.Value.ToConvert<APP_CONFIG>();
            var dbValue = new CFG_APP_CONFIG();
            dbValue.ID = model.Value.Id;
            //dbValue.PARENT_ID = model.Value.ParentId;
            dbValue.SECTION_DESC = model.Value.SectionDescription;
            //dbValue.SECTION_LEVEL = model.Value.SectionLevel;
            dbValue.SECTION_NAME = model.Value.SectionName;
            dbValue.SECTION_TYPE = model.Value.SectionType;
            dbValue.SECTION_VALUE = model.Value.SectionValue;
            var entity = context.CFG_APP_CONFIG.FirstOrDefault(x => x.ID == model.Value.Id);
            if (entity == null)
            {
                context.CFG_APP_CONFIG.Add(dbValue);
            }
            else
            {
                var props = dbValue.GetType().GetProperties();
                foreach (var x in props)
                {
                    x.SetValue(entity, x.GetValue(dbValue));
                }
            }

            if (model.Children != null)
            {
                model.Children.ForEach(x => AddOrUpdateModel(context, x));
            }
        }

        public Boolean DeleteModel(string id, PTMSEntities _context)
        {

            CollectionDelete(_context, id);

            return _context.SaveChanges() > 0 ? true : false;

        }

        public void DeleteModel(PTMSEntities _context, string id)
        {
            CollectionDelete(_context, id);
        }

        private void CollectionDelete(PTMSEntities _context, string id)
        {
            var entity = _context.CFG_APP_CONFIG.FirstOrDefault(x => x.ID == id);
            if (entity == null) return;

            _context.CFG_APP_CONFIG.Remove(entity);

            //foreach (var x in _context.CFG_APP_CONFIG.Where(x => x.PARENT_ID == id))
            //{
            //    DeleteModel(_context, x.ID);
            //}
        }
    }
}
