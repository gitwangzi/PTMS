using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Monitor.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
///// Guid: a2a104b0-cf3f-43b2-b2c8-eefd726551f5      
///// clrversion: 4.0.30319.17929
/////Registered organization: 
///// Machine Name: PC-Shihs
///// Author: Hongsheng Shi
/////======================================================================
/////  Project Name: Gsafety.PTMS.BaseInformation.Contract.Data
/////  Project Description:    
///// Class Name: MonitorRepository
///// Class Version: v1.0.0.0
///// Create Time: 2013/8/19 15:47:53
///// Class Description:  
/////======================================================================
/////  Modified Time: 
/////  Modified by: 
/////  Modified Description: 
/////======================================================================
namespace Gsafety.PTMS.Monitor.Test
{
    public class TestDataModel
    {

        internal MultiMessage<VehicleAlert> Expected1
        {
            get
            {
                return new MultiMessage<VehicleAlert>
                {
                    Result = new List<VehicleAlert> {
                        new VehicleAlert { }
                    },
                    TotalRecord = 0
                };
            }
        }

        public MultiMessage<VedioFile> Expected2
        {
            get
            {
                return new MultiMessage<VedioFile>
                {
                    Result = new List<VedioFile> {
                        new VedioFile { }
                    },
                    TotalRecord = 0
                };
            }
        }

        public MultiMessage<VehicleTrack> Expected3
        {
            get
            {
                return new MultiMessage<VehicleTrack>
                {
                    Result = new List<VehicleTrack> {
                        new VehicleTrack { }
                    },
                    TotalRecord = 0
                };
            }
        }

        public MultiMessage<VedioFile> Expected4
        {
            get
            {
                return new MultiMessage<VedioFile>
                {
                    Result = new List<VedioFile> {
                        new VedioFile { }
                    },
                    TotalRecord = 0
                };
            }
        }
    }
}

