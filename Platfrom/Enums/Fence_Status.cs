/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2013f823-928a-46d1-8111-93c5e4476124      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share.Enum
/////    Project Description:    
/////             Class Name: Fence_Status
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/1 10:55:53
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/1 10:55:53
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Bases.Enums
{
    /// <summary>
    /// Fence_Status
    /// </summary>
    public enum Fence_Status
    {
        /// <summary>
        /// Fence_New_Wait
        /// </summary>
        Fence_New_Wait = 10,
        /// <summary>
        /// Fence_New_Sucess
        /// </summary>
        Fence_New_Sucess = 11,
        /// <summary>
        /// Fence_New_Faild
        /// </summary>
        Fence_New_Faild = 12,
        /// <summary>
        /// Fence_Edit_Wait
        /// </summary>
        Fence_Edit_Wait = 20,
        /// <summary>
        /// Fence_Edit_Sucess
        /// </summary>
        Fence_Edit_Sucess = 21,
        /// <summary>
        /// Fence_Edit_Faild
        /// </summary>
        Fence_Edit_Faild = 22,
        /// <summary>
        /// Fence_Delete_Wait
        /// </summary>
        Fence_Delete_Wait = 30,
        /// <summary>
        /// Fence_Delete_Faild
        /// </summary>
        Fence_Delete_Faild = 32
    }
}
