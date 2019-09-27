/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: da9f8ee5-0700-4e21-9d55-62a4aa9355af      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation
/////    Project Description:    
/////             Class Name: UnZipper
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/31 15:26:06
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/31 15:26:06
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Resources;
using System.Windows.Shapes;

namespace Gsafety.PTMS.BaseInformation
{
    public class UnZipper:IDisposable 
    {
        private Stream stream;



        public UnZipper(Stream zipFileStream)
        {
            this.stream = zipFileStream;
        }


        public Stream GetFileStream(string filename)
        {
            Uri fileUri = new Uri(filename, UriKind.Relative);
            StreamResourceInfo info = new StreamResourceInfo(this.stream, null);
            if (this.stream is System.IO.FileStream)
                this.stream.Seek(0, SeekOrigin.Begin);
            StreamResourceInfo stream = System.Windows.Application.GetResourceStream(info, fileUri);
            if (stream != null)
                return stream.Stream;
            return null;

            
        }


        public IEnumerable<string> GetFileNamesInZip()
        {
            BinaryReader reader = new BinaryReader(stream);
            stream.Seek(0, SeekOrigin.Begin);
            string name = null;
            List<string> names = new List<string>();
            while (ParseFileHeader(reader, out name))
            {
                names.Add(name);
            }
            return names;
        }


        private static bool ParseFileHeader(BinaryReader reader, out string filename)
        {
            filename = null;
            if (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                int headerSignature = reader.ReadInt32();
                if (headerSignature == 67324752) //ggggggrrrrrrrrrrrrrrrrr
                {
                    reader.BaseStream.Seek(2, SeekOrigin.Current);

                    short genPurposeFlag = reader.ReadInt16();
                    if (((((int)genPurposeFlag) & 0x08) != 0))
                        return false;
                    reader.BaseStream.Seek(10, SeekOrigin.Current);
                    //short method = reader.ReadInt16(); //Unused
                    //short lastModTime = reader.ReadInt16(); //Unused
                    //short lastModDate = reader.ReadInt16(); //Unused
                    //int crc32 = reader.ReadInt32(); //Unused
                    int compressedSize = reader.ReadInt32();
                    int unCompressedSize = reader.ReadInt32();
                    short fileNameLenght = reader.ReadInt16();
                    short extraFieldLenght = reader.ReadInt16();
                    filename = new string(reader.ReadChars(fileNameLenght));
                    if (string.IsNullOrEmpty(filename))
                        return false;

                    reader.BaseStream.Seek(extraFieldLenght + compressedSize, SeekOrigin.Current);
                    if (unCompressedSize == 0)
                        return ParseFileHeader(reader, out filename);
                    else
                        return true;
                }
            }
            return false;
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (this.stream != null)
                {
                    this.stream.Dispose();
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
