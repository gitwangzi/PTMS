﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Media.TransportStream.TsParser
{
    public interface ITsProgramAssociationTableFactory
    {
        TsProgramAssociationTable Create(ITsDecoder decoder, Func<int, bool> programFilter, Action<IProgramStreams> streamFilter);
    }
}