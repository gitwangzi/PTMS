using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.SecuritySuite.Contract.Data;
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
///// Create Time: 2013/8/27 15:47:53
///// Class Description:  
/////======================================================================
/////  Modified Time: 
/////  Modified by: 
/////  Modified Description: 
/////======================================================================
namespace Gsafety.PTMS.SecuritySuite.Test
{
    class TestDataModel
    {
        public MultiMessage<VehicleStatus> Expected1
        {
            get
            {
                return new MultiMessage<VehicleStatus>
                {
                    Result = new List<VehicleStatus> {
                        new VehicleStatus { SutieInfoId="Sut0001", CarNumber="BJ0001", MdvrCoreId="MDVR0001", Status=2, IsOnline=1, CarType=3, AbnormalCause="400"}
                    },
                    TotalRecord = 0
                };
            }
        }

        public MultiMessage<VehicleStatus> Expected2
        {
            get
            {
                return new MultiMessage<VehicleStatus>
                {
                    Result = new List<VehicleStatus> {
                        new VehicleStatus { SutieInfoId="Sut0001", CarNumber="BJ0001", MdvrCoreId="MDVR0001", Status=2, IsOnline=1, CarType=3, AbnormalCause="400"}
                    },
                    TotalRecord = 0
                };
            }
        }

        public MultiMessage<VehicleStatus> Expected3
        {
            get
            {
                return new MultiMessage<VehicleStatus>
                {
                    Result = new List<VehicleStatus> {
                        new VehicleStatus { SutieInfoId="Sut0001", CarNumber="BJ0001", MdvrCoreId="MDVR0001", Status=2, IsOnline=1, CarType=1, AbnormalCause="400"}
                    },
                    TotalRecord = 0
                };
            }
        }

        public MultiMessage<VehicleStatus> Expected4
        {
            get
            {
                return new MultiMessage<VehicleStatus>
                {
                    Result = new List<VehicleStatus> {
                        new VehicleStatus { SutieInfoId="Sut0001", CarNumber="BJ0001", MdvrCoreId="MDVR0001", Status=2, IsOnline=1, CarType=3, AbnormalCause="400"}
                    },
                    TotalRecord = 0
                };
            }
        }

        public MultiMessage<VehicleStatus> Expected5
        {
            get
            {
                return new MultiMessage<VehicleStatus>
                {
                    Result = new List<VehicleStatus> {
                        new VehicleStatus { SutieInfoId="Sut0001", CarNumber="BJ0001", MdvrCoreId="MDVR0001", Status=2, IsOnline=1, CarType=3, AbnormalCause="400"}
                    },
                    TotalRecord = 0
                };
            }
        }

        public MultiMessage<VehicleStatus> Expected6
        {
            get
            {
                return new MultiMessage<VehicleStatus>
                {
                    Result = new List<VehicleStatus> {
                        new VehicleStatus { SutieInfoId="Sut0001", CarNumber="BJ0001", MdvrCoreId="MDVR0001", Status=2, IsOnline=1, CarType=3, AbnormalCause="400"}
                    },
                    TotalRecord = 0
                };
            }
        }

        public SingleMessage<VehicleStatus> Expected7
        {
            get
            {
                return new SingleMessage<VehicleStatus>
                {
                    Result = new VehicleStatus { }
                };
            }
        }
    }
}
