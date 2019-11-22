/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 49f94c8b-8ef8-4109-9a2e-405fb1455294      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LINGL
/////                 Author: TEST(zhangzl)
/////======================================================================
/////           Project Name: GisManagement
/////    Project Description:    
/////             Class Name: ElementToPNG
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/25 15:18:31
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/25 15:18:31
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Gsafety.PTMS.Share;
using Gsafety.Common.Controls;

namespace GisManagement
{
    public class ElementToPNG
    {
        public bool ShowSaveDialog(UIElement elementToExport)
        {

            //Canvas canvasToExport = e as Canvas;
            // Instantiate SaveFileDialog
            // and set defautl settings (just PNG export)

            SaveFileDialog sfd = new SaveFileDialog()
            {
                DefaultExt = "png",
                Filter = "Png files (*.png)|*.png",
                FilterIndex = 1
            };

            if (sfd.ShowDialog() == true)
            {
                if ((!sfd.SafeFileName.EndsWith(".png")) && (!sfd.SafeFileName.EndsWith(".PNG")))
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_InputPngFileName"), MessageDialogButton.Ok);
                    return false;
                }
                SaveAsPNG(sfd, elementToExport);
                return true;
            }
            else
            {
                return false;
            }

        }

        private void SaveAsPNG(SaveFileDialog sfd, UIElement elementToExport)
        {
            WriteableBitmap bitmap = new WriteableBitmap(elementToExport, new TranslateTransform());
            EditableImage imageData = new EditableImage(bitmap.PixelWidth, bitmap.PixelHeight);

            try
            {
                for (int y = 0; y < bitmap.PixelHeight; ++y)
                {

                    for (int x = 0; x < bitmap.PixelWidth; ++x)
                    {

                        int pixel = bitmap.Pixels[bitmap.PixelWidth * y + x];
                        imageData.SetPixel(x, y,

                        (byte)((pixel >> 16) & 0xFF),
                        (byte)((pixel >> 8) & 0xFF),

                        (byte)(pixel & 0xFF), (byte)((pixel >> 24) & 0xFF)
                        );

                    }

                }

            }
            catch (System.Security.SecurityException)
            {
                throw new Exception("Cannot print images from other domains");
            }

            // Save it to disk
            Stream pngStream = imageData.GetStream();

            //StreamReader sr = new StreamReader(pngStream);
            byte[] binaryData = new Byte[pngStream.Length];

            long bytesRead = pngStream.Read(binaryData, 0, (int)pngStream.Length);

            //using (Stream stream = sfd.OpenFile())
            //{

            //    stream.Write(binaryData, 0, binaryData.Length);

            //    stream.Close();

            //}

            Stream stream = sfd.OpenFile();

            stream.Write(binaryData, 0, binaryData.Length);

            stream.Close();


        }
    }
}
