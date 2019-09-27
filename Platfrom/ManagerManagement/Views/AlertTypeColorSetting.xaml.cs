/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 518f2e53-9100-4de6-9217-3cdff3007a5c      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views
/////    Project Description:    
/////             Class Name: AlertTypeColorSetting
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/11 13:57:52
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/11 13:57:52
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.Common.CommMessage;
using Jounce.Core.View;
using Jounce.Regions.Core;
using Gsafety.PTMS.Manager.ViewModels.UserManageViewModel;

namespace Gsafety.PTMS.Manager.Views
{
    
    
    public partial class AlertTypeColorSetting : UserControl
    {
        public ObservableCollection<PredefinedColor> PreDefinedColors = null;
        
        public AlertTypeColorSetting()
        {
            InitializeComponent();
            Initialize_Colors();
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;

        }

        void AlertTypeColorSetting_Loaded(object sender, RoutedEventArgs e)
        {
             
        }

        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void Initialize_Colors()
        {
            if (PreDefinedColors != null)
            {
                return;
            }

            PreDefinedColors = new ObservableCollection<PredefinedColor>();

            PreDefinedColors.Add(new PredefinedColor(0, 255, 255, 255, "Transparent"));
            PreDefinedColors.Add(new PredefinedColor(255, 0, 0, 0, "Black"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 255, 255, "White"));
            PreDefinedColors.Add(new PredefinedColor(255, 105, 105, 105, "Dim Gray"));
            PreDefinedColors.Add(new PredefinedColor(255, 128, 128, 128, "Gray"));
            PreDefinedColors.Add(new PredefinedColor(255, 169, 169, 169, "Dark Gray"));
            PreDefinedColors.Add(new PredefinedColor(255, 192, 192, 192, "Silver"));
            PreDefinedColors.Add(new PredefinedColor(255, 211, 211, 211, "Light Gray"));
            PreDefinedColors.Add(new PredefinedColor(255, 220, 220, 220, "Gainsboro"));
            PreDefinedColors.Add(new PredefinedColor(255, 245, 245, 245, "White Smoke"));
            PreDefinedColors.Add(new PredefinedColor(255, 128, 0, 0, "Maroon"));
            PreDefinedColors.Add(new PredefinedColor(255, 139, 0, 0, "Dark Red"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 0, 0, "Red"));
            PreDefinedColors.Add(new PredefinedColor(255, 165, 42, 42, "Brown"));
            PreDefinedColors.Add(new PredefinedColor(255, 178, 34, 34, "Firebrick"));
            PreDefinedColors.Add(new PredefinedColor(255, 205, 92, 92, "Indian Red"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 250, 250, "Snow"));
            PreDefinedColors.Add(new PredefinedColor(255, 240, 128, 128, "Light Coral"));
            PreDefinedColors.Add(new PredefinedColor(255, 188, 143, 143, "Rosy Brown"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 228, 225, "Misty Rose"));
            PreDefinedColors.Add(new PredefinedColor(255, 250, 128, 114, "Salmon"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 99, 71, "Tomato"));
            PreDefinedColors.Add(new PredefinedColor(255, 233, 150, 122, "Dark Salmon"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 127, 80, "Coral"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 69, 0, "Orange Red"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 160, 122, "Light Salmon"));
            PreDefinedColors.Add(new PredefinedColor(255, 160, 82, 45, "Sienna"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 245, 238, "Sea Shell"));
            PreDefinedColors.Add(new PredefinedColor(255, 210, 105, 30, "Chocolate"));
            PreDefinedColors.Add(new PredefinedColor(255, 139, 69, 19, "Saddle Brown"));
            PreDefinedColors.Add(new PredefinedColor(255, 244, 164, 96, "Sandy Brown"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 218, 185, "Peach Puff"));
            PreDefinedColors.Add(new PredefinedColor(255, 205, 133, 63, "Peru"));
            PreDefinedColors.Add(new PredefinedColor(255, 250, 240, 230, "Linen"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 228, 196, "Bisque"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 140, 0, "Dark Orange"));
            PreDefinedColors.Add(new PredefinedColor(255, 222, 184, 135, "Burly Wood"));
            PreDefinedColors.Add(new PredefinedColor(255, 210, 180, 140, "Tan"));
            PreDefinedColors.Add(new PredefinedColor(255, 250, 235, 215, "Antique White"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 222, 173, "Navajo White"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 235, 205, "Blanched Almond"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 239, 213, "Papaya Whip"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 228, 181, "Moccasin"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 165, 0, "Orange"));
            PreDefinedColors.Add(new PredefinedColor(255, 245, 222, 179, "Wheat"));
            PreDefinedColors.Add(new PredefinedColor(255, 253, 245, 230, "Old Lace"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 250, 240, "Floral White"));
            PreDefinedColors.Add(new PredefinedColor(255, 184, 134, 11, "Dark Goldenrod"));
            PreDefinedColors.Add(new PredefinedColor(255, 218, 165, 32, "Goldenrod"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 248, 220, "Cornsilk"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 215, 0, "Gold"));
            PreDefinedColors.Add(new PredefinedColor(255, 240, 230, 140, "Khaki"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 250, 205, "Lemon Chiffon"));
            PreDefinedColors.Add(new PredefinedColor(255, 238, 232, 170, "Pale Goldenrod"));
            PreDefinedColors.Add(new PredefinedColor(255, 189, 183, 107, "Dark Khaki"));
            PreDefinedColors.Add(new PredefinedColor(255, 245, 245, 220, "Beige"));
            PreDefinedColors.Add(new PredefinedColor(255, 250, 250, 210, "Light Goldenrod Yellow"));
            PreDefinedColors.Add(new PredefinedColor(255, 128, 128, 0, "Olive"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 255, 0, "Yellow"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 255, 224, "Light Yellow"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 255, 240, "Ivory"));
            PreDefinedColors.Add(new PredefinedColor(255, 107, 142, 35, "Olive Drab"));
            PreDefinedColors.Add(new PredefinedColor(255, 154, 205, 50, "Yellow Green"));
            PreDefinedColors.Add(new PredefinedColor(255, 85, 107, 47, "Dark Olive Green"));
            PreDefinedColors.Add(new PredefinedColor(255, 173, 255, 47, "Green Yellow"));
            PreDefinedColors.Add(new PredefinedColor(255, 127, 255, 0, "Chartreuse"));
            PreDefinedColors.Add(new PredefinedColor(255, 124, 252, 0, "Lawn Green"));
            PreDefinedColors.Add(new PredefinedColor(255, 143, 188, 139, "Dark Sea Green"));
            PreDefinedColors.Add(new PredefinedColor(255, 144, 238, 144, "Light Green"));
            PreDefinedColors.Add(new PredefinedColor(255, 34, 139, 34, "Forest Green"));
            PreDefinedColors.Add(new PredefinedColor(255, 50, 205, 50, "Lime Green"));
            PreDefinedColors.Add(new PredefinedColor(255, 152, 251, 152, "Pale Green"));
            PreDefinedColors.Add(new PredefinedColor(255, 0, 100, 0, "Dark Green"));
            PreDefinedColors.Add(new PredefinedColor(255, 0, 128, 0, "Green"));
            PreDefinedColors.Add(new PredefinedColor(255, 0, 255, 0, "Lime"));
            PreDefinedColors.Add(new PredefinedColor(255, 240, 255, 240, "Honeydew"));
            PreDefinedColors.Add(new PredefinedColor(255, 46, 139, 87, "Sea Green"));
            PreDefinedColors.Add(new PredefinedColor(255, 60, 179, 113, "Medium Sea Green"));
            PreDefinedColors.Add(new PredefinedColor(255, 0, 255, 127, "Spring Green"));
            PreDefinedColors.Add(new PredefinedColor(255, 245, 255, 250, "Mint Cream"));
            PreDefinedColors.Add(new PredefinedColor(255, 0, 250, 154, "Medium Spring Green"));
            PreDefinedColors.Add(new PredefinedColor(255, 102, 205, 170, "Medium Aquamarine"));
            PreDefinedColors.Add(new PredefinedColor(255, 127, 255, 212, "Aquamarine"));
            PreDefinedColors.Add(new PredefinedColor(255, 64, 224, 208, "Turquoise"));
            PreDefinedColors.Add(new PredefinedColor(255, 32, 178, 170, "Light Sea Green"));
            PreDefinedColors.Add(new PredefinedColor(255, 72, 209, 204, "Medium Turquoise"));
            PreDefinedColors.Add(new PredefinedColor(255, 47, 79, 79, "Dark SlateGray"));
            PreDefinedColors.Add(new PredefinedColor(255, 175, 238, 238, "Pale Turquoise"));
            PreDefinedColors.Add(new PredefinedColor(255, 0, 128, 128, "Teal"));
            PreDefinedColors.Add(new PredefinedColor(255, 0, 139, 139, "Dark Cyan"));
            PreDefinedColors.Add(new PredefinedColor(255, 0, 255, 255, "Cyan"));
            PreDefinedColors.Add(new PredefinedColor(255, 0, 255, 255, "Aqua"));
            PreDefinedColors.Add(new PredefinedColor(255, 224, 255, 255, "Light Cyan"));
            PreDefinedColors.Add(new PredefinedColor(255, 240, 255, 255, "Azure"));
            PreDefinedColors.Add(new PredefinedColor(255, 0, 206, 209, "Dark Turquoise"));
            PreDefinedColors.Add(new PredefinedColor(255, 95, 158, 160, "Cadet Blue"));
            PreDefinedColors.Add(new PredefinedColor(255, 176, 224, 230, "Powder Blue"));
            PreDefinedColors.Add(new PredefinedColor(255, 173, 216, 230, "Light Blue"));
            PreDefinedColors.Add(new PredefinedColor(255, 0, 191, 255, "Deep Sky Blue"));
            PreDefinedColors.Add(new PredefinedColor(255, 135, 206, 235, "Sky Blue"));
            PreDefinedColors.Add(new PredefinedColor(255, 135, 206, 250, "Light Sky Blue"));
            PreDefinedColors.Add(new PredefinedColor(255, 70, 130, 180, "Steel Blue"));
            PreDefinedColors.Add(new PredefinedColor(255, 240, 248, 255, "Alice Blue"));
            PreDefinedColors.Add(new PredefinedColor(255, 30, 144, 255, "Dodger Blue"));
            PreDefinedColors.Add(new PredefinedColor(255, 112, 128, 144, "Slate Gray"));
            PreDefinedColors.Add(new PredefinedColor(255, 119, 136, 153, "Light Slate Gray"));
            PreDefinedColors.Add(new PredefinedColor(255, 176, 196, 222, "Light Steel Blue"));
            PreDefinedColors.Add(new PredefinedColor(255, 100, 149, 237, "Cornflower Blue"));
            PreDefinedColors.Add(new PredefinedColor(255, 65, 105, 225, "Royal Blue"));
            PreDefinedColors.Add(new PredefinedColor(255, 25, 25, 112, "Midnight Blue"));
            PreDefinedColors.Add(new PredefinedColor(255, 230, 230, 250, "Lavender"));
            PreDefinedColors.Add(new PredefinedColor(255, 0, 0, 128, "Navy"));
            PreDefinedColors.Add(new PredefinedColor(255, 0, 0, 139, "Dark Blue"));
            PreDefinedColors.Add(new PredefinedColor(255, 0, 0, 205, "Medium Blue"));
            PreDefinedColors.Add(new PredefinedColor(255, 0, 0, 255, "Blue"));
            PreDefinedColors.Add(new PredefinedColor(255, 248, 248, 255, "Ghost White"));
            PreDefinedColors.Add(new PredefinedColor(255, 106, 90, 205, "Slate Blue"));
            PreDefinedColors.Add(new PredefinedColor(255, 72, 61, 139, "Dark Slate Blue"));
            PreDefinedColors.Add(new PredefinedColor(255, 123, 104, 238, "Medium Slate Blue"));
            PreDefinedColors.Add(new PredefinedColor(255, 147, 112, 219, "Medium Purple"));
            PreDefinedColors.Add(new PredefinedColor(255, 138, 43, 226, "Blue Violet"));
            PreDefinedColors.Add(new PredefinedColor(255, 75, 0, 130, "Indigo"));
            PreDefinedColors.Add(new PredefinedColor(255, 153, 50, 204, "Dark Orchid"));
            PreDefinedColors.Add(new PredefinedColor(255, 148, 0, 211, "Dark Violet"));
            PreDefinedColors.Add(new PredefinedColor(255, 186, 85, 211, "Medium Orchid"));
            PreDefinedColors.Add(new PredefinedColor(255, 216, 191, 216, "Thistle"));
            PreDefinedColors.Add(new PredefinedColor(255, 221, 160, 221, "Plum"));
            PreDefinedColors.Add(new PredefinedColor(255, 238, 130, 238, "Violet"));
            PreDefinedColors.Add(new PredefinedColor(255, 128, 0, 128, "Purple"));
            PreDefinedColors.Add(new PredefinedColor(255, 139, 0, 139, "Dark Magenta"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 0, 255, "Fuchsia"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 0, 255, "Magenta"));
            PreDefinedColors.Add(new PredefinedColor(255, 218, 112, 214, "Orchid"));
            PreDefinedColors.Add(new PredefinedColor(255, 199, 21, 133, "Medium Violet Red"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 20, 147, "Deep Pink"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 105, 180, "Hot Pink"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 240, 245, "Lavender Blush"));
            PreDefinedColors.Add(new PredefinedColor(255, 219, 112, 147, "Pale Violet Red"));
            PreDefinedColors.Add(new PredefinedColor(255, 220, 20, 60, "Crimson"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 192, 203, "Pink"));
            PreDefinedColors.Add(new PredefinedColor(255, 255, 182, 193, "Light Pink"));
        }
    }
}
