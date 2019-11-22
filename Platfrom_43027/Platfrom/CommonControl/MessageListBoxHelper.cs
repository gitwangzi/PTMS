using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.Share;

namespace Gsafety.Common.Controls
{
    public class MessageListBoxHelper
    {
        public static ChildWindow ShowDialog(string messageText, MessageDialogButton dialogButton = MessageDialogButton.Ok, Action<object, MessageEventArgs> ClosedAction = null)
        {

            return ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), messageText, dialogButton, ClosedAction);
        }

        public static ChildWindow ShowDialog(string caption, string messageText, MessageDialogButton dialogButton = MessageDialogButton.Ok, Action<object, MessageEventArgs> ClosedAction = null)
        {
            ListMessageBox listMessageBox = new ListMessageBox() { IsHavCancelButton = (dialogButton == MessageDialogButton.OkAndCancel ? true : false), Title = caption, MessageText = messageText };

            listMessageBox.Closing += (sender, e) =>
            {
                var cw = sender as ChildWindow;
                if (cw != null)
                {
                    var r = cw.DialogResult;
                }
            };

            listMessageBox.Closed += (sender, e) =>
            {
                if (ClosedAction != null)
                {
                    MessageEventArgs mesArg = new MessageEventArgs();
                    ChildWindow cw = sender as ChildWindow;
                    if (cw != null)
                    {
                        if (cw.DialogResult == true)
                        {
                            mesArg.DialogResult = MessageDialogResult.OK;
                        }
                        else
                        {
                            mesArg.DialogResult = MessageDialogResult.Cancel;
                        }
                    }
                    ClosedAction(sender, mesArg);
                }
            };

            listMessageBox.Show();
            return listMessageBox;
        }


        public async static Task<MessageDialogResult> ShowDialogMessageTask(string caption, string messageText, MessageDialogButton dialogButton = MessageDialogButton.Ok)
        {
            TaskCompletionSource<MessageDialogResult> tcs = new TaskCompletionSource<MessageDialogResult>();

            ListMessageBox listMessageBox = new ListMessageBox() { IsHavCancelButton = (dialogButton == MessageDialogButton.OkAndCancel ? true : false), Title = caption, MessageText = messageText };
            listMessageBox.Closed += (sender, e) =>
            {
                MessageEventArgs mesArg = new MessageEventArgs();
                ChildWindow cw = sender as ChildWindow;
                if (cw.DialogResult == true)
                {
                    mesArg.DialogResult = MessageDialogResult.OK;
                }
                else
                {
                    mesArg.DialogResult = MessageDialogResult.Cancel;
                }

                tcs.TrySetResult(mesArg.DialogResult);
            };

            listMessageBox.Show();

            return await tcs.Task;
        }
    }
}
