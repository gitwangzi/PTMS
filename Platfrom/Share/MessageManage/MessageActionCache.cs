/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ffce9f41-3c3b-4e40-98bc-eb5420d53027      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share
/////    Project Description:    
/////             Class Name: ANTMessageAction
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/6 10:50:48
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/6 10:50:48
/////            Modified by:
/////   Modified Description: 
/////======================================================================
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
    public static class MessageActionCache
    {
        private static List<Tuple<Action<string>, string>> _messageParamAction;
        private static object lockobj = new object();
        private static List<Action> _messageNoParamAction;

        static MessageActionCache()
        {
            _messageParamAction = new List<Tuple<Action<string>, string>>();
            _messageNoParamAction = new List<Action>();
        }

        public static bool Add(Action<string> action, string param)
        {
            lock (lockobj)
            {
                if (_messageParamAction.Any(x => x.Item1.Method.Name == action.Method.Name && x.Item2 == param))
                    return false;

                _messageParamAction.Add(new Tuple<Action<string>, string>(action, param));
            }
            return true;
        }

        public static bool Add(Action action)
        {
            lock (lockobj)
            {
                if (_messageNoParamAction.Any(x => x.Method.Name == action.Method.Name))
                    return false;

                _messageNoParamAction.Add(action);
            }
            return true;
        }

        public static bool Remove(Action<string> action, string param)
        {
            lock (lockobj)
            {
                if (!_messageParamAction.Any(x => x.Item1.Method.Name == action.Method.Name && x.Item2 == param))
                    return false;

                _messageParamAction.RemoveAll(x => x.Item1.Method.Name == action.Method.Name && x.Item2 == param);
            }
            return true;
        }

        public static bool Remove(Action action)
        {
            lock (lockobj)
            {
                if (!_messageNoParamAction.Any(x => x.Method.Name == action.Method.Name))
                    return false;

                _messageParamAction.RemoveAll(x => x.Item1.Method.Name == action.Method.Name);
            }
            return true;
        }

        public static void InvokeAction()
        {
            lock (lockobj)
            {
                for (int i = _messageNoParamAction.Count() - 1; i >= 0; i--)
                {
                    var item = _messageNoParamAction[i];
                    _messageNoParamAction.RemoveAt(i);
                    item.Invoke();
                }

                for (int i = _messageParamAction.Count() - 1; i >= 0; i--)
                {
                    var item = _messageParamAction[i];
                    _messageParamAction.RemoveAt(i);
                    item.Item1.Invoke(item.Item2);
                }
            }
        }
    }
}
