using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;

namespace Gsafety.PTMS.Share
{
    public class DisplayParameter
    {
        public bool DisplayParameterMode { get; set; }

        public Dictionary<string, List<string>> VideoFileDic = new Dictionary<string, List<string>>();

        public void ParseVideoFileParameter(string parameter)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(parameter))
                {
                    return;
                }

                var devices = parameter.Split(';').ToList();
                foreach (var device in devices)
                {
                    var values = device.Split(':').ToList();
                    if (values.Count < 2)
                    {
                        continue;
                    }

                    if (VideoFileDic.ContainsKey(values[0]))
                    {
                        continue;
                    }

                    var files = values.Skip(1).Take(values.Count - 1).ToList();
                    VideoFileDic[values[0]] = files;
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
