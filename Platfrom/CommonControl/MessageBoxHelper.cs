using Gsafety.PTMS.Share;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gsafety.Common.Controls
{
    public class MessageBoxHelper
    {
        public static ChildWindow ShowDialog(string messageText, MessageDialogButton dialogButton = MessageDialogButton.Ok, Action<object, MessageEventArgs> ClosedAction = null)
        {

            return ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), messageText, dialogButton, ClosedAction);
        }

        public static ChildWindow ShowDialog(string caption, string messageText, MessageDialogButton dialogButton = MessageDialogButton.Ok, Action<object, MessageEventArgs> ClosedAction = null)
        {
            SelfMessageBox selfMessageBox = new SelfMessageBox() { Width = 320, Height = 180, IsHavCancelButton = (dialogButton == MessageDialogButton.OkAndCancel ? true : false), Title = caption, MessageText = messageText };

            selfMessageBox.Closing += (sender, e) =>
            {
                var cw = sender as ChildWindow;
                if (cw != null)
                {
                    var r = cw.DialogResult;
                }
            };

            selfMessageBox.Closed += (sender, e) =>
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

            selfMessageBox.Show();
            return selfMessageBox;
        }

        public static ChildWindow GisShowDialog(string caption, string messageText, MessageDialogButton dialogButton = MessageDialogButton.Ok, Action<object, MessageEventArgs> ClosedAction = null)
        {
            SelfMessageBox selfMessageBox = new SelfMessageBox() { Width = 400, Height = 300, IsHavCancelButton = (dialogButton == MessageDialogButton.OkAndCancel ? true : false), Title = caption, MessageText = messageText };

            selfMessageBox.Closing += (sender, e) =>
            {
                var cw = sender as ChildWindow;
                if (cw != null)
                {
                    var r = cw.DialogResult;
                }
            };

            selfMessageBox.Closed += (sender, e) =>
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

            selfMessageBox.Show();
            return selfMessageBox;
        }


        public async static Task<MessageDialogResult> ShowDialogMessageTask(string caption, string messageText, MessageDialogButton dialogButton = MessageDialogButton.Ok)
        {
            TaskCompletionSource<MessageDialogResult> tcs = new TaskCompletionSource<MessageDialogResult>();

            SelfMessageBox selfMessageBox = new SelfMessageBox() { Width = 320, Height = 180, IsHavCancelButton = (dialogButton == MessageDialogButton.OkAndCancel ? true : false), Title = caption, MessageText = messageText };
            selfMessageBox.Closed += (sender, e) =>
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

            selfMessageBox.Show();

            return await tcs.Task;
        }
    }

    public enum MessageDialogButton
    {
        Ok,
        OkAndCancel
    }

    public enum MessageDialogResult
    {
        OK,
        Cancel
    }

    public class MessageEventArgs : EventArgs
    {
        public MessageDialogResult DialogResult { get; set; }
    }
}
