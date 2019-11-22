using Gsafety.PTMS.Email.Contract.Data;
using Gsafety.Common.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Gsafety.Common.Logging;
//using static System.Net.Mime.MediaTypeNames;

namespace Gsafety.PTMS.Email.Repository
{
    public class EmailOperateRepository
    {
        public string LatStrToDouble(object value)
        {
            double result = 0;
            if (value == null)
            {
                return "";
            }

            string temp = value.ToString().Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            int indflag = temp.IndexOf("-");
            temp = temp.Replace("-", "");

            int du = 0;
            double fen = 0;

            int ind = temp.IndexOf(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            if (ind == -1)
            {
                ind = temp.Length;
            }

            if ((ind - 3 + 1) >= 1)
            {
                du = int.Parse(temp.Substring(0, ind - 3 + 1));
                fen = double.Parse(temp.Substring(ind - 2));
            }
            else
            {
                fen = double.Parse(temp);
            }

            if (indflag > -1)
                result = (-du - fen / 60);
            else
                result = (du + fen / 60);

            return result.ToString("f6");
        }
        public string LonStrToDouble(object value)
        {
            double result = 0;
            if (value == null)
            {
                return "";
            }
            string temp = value.ToString().Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            int indflag = temp.IndexOf("-");
            temp = temp.Replace("-", "");

            int du = 0;
            double fen = 0;

            int ind = temp.IndexOf(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            if (ind == -1)
            {
                ind = temp.Length;
            }

            if ((ind - 3 + 1) >= 1)
            {
                du = int.Parse(temp.Substring(0, ind - 3 + 1));
                fen = double.Parse(temp.Substring(ind - 2));
            }
            else
            {
                fen = double.Parse(temp);
            }

            if (indflag > -1)
                result = (-du - fen / 60);
            else
                result = (du + fen / 60);

            return result.ToString("f6");
        }

        public bool SendEmail(EmailInfo email)
        {
            try
            {
                string info = email.MailBody;


                //email.MailFrom = ConfigurationHelper.EmailFrom;
                //email.MailPwd = ConfigurationHelper.EmailPwd;
                //Using the specified e-mail address
                MailAddress maddr = new MailAddress(ConfigurationHelper.MailFrom);
                // Initialize MailMessage instance
                MailMessage myMail = new MailMessage();

                //Adds e-mail address to the recipient address
                if (email.MailToArray != null && email.MailToArray.Length > 0)
                {
                    email.MailToArray = email.MailToArray.Distinct().ToArray();
                    for (int i = 0; i < email.MailToArray.Length; i++)
                    {
                        if (email.MailToArray[i] != null)
                        {
                            myMail.To.Add(email.MailToArray[i].ToString());
                        }
                    }
                }

                //Adds e-mail address to the Cc recipient address
                if (email.MailCcArray != null && email.MailCcArray.Length > 0)
                {
                    email.MailCcArray = email.MailCcArray.Distinct().ToArray();
                    for (int i = 0; i < email.MailCcArray.Length; i++)
                    {
                        if (email.MailCcArray[i] != null)
                        {
                            myMail.CC.Add(email.MailCcArray[i].ToString());
                        }
                    }
                }
                //Sender Address
                myMail.From = maddr;


                //Email title
                myMail.Subject = email.MailSubject;

                //The subject matter of the use of e-mail encoding
                myMail.SubjectEncoding = Encoding.UTF8;

                //myMail.Attachments.Add(new Attachment(email.bytepicture.ToString()));
                //myMail.Attachments[0].ContentType.Name = Image.Jpeg;
                //myMail.Attachments[0].ContentId = "picture";

                //myMail.Attachments[0].ContentDisposition.Inline = true;
                //myMail.Attachments[0].TransferEncoding = System.Net.Mime.TransferEncoding.Base64;

                ////Email Body
                //myMail.Body = email.MailBody + "<br><img alt='图片' src=cid:picture>";
                //Email Body encoding
                myMail.BodyEncoding = Encoding.Default;

                myMail.Body = info;

                myMail.Priority = MailPriority.High;

                myMail.IsBodyHtml = email.IsbodyHtml;

                //In the case of attachment to add attachments
                try
                {
                    if (email.AttachmentsPath != null && email.AttachmentsPath.Length > 0)
                    {
                        Attachment attachFile = null;
                        foreach (string path in email.AttachmentsPath)
                        {
                            attachFile = new Attachment(path);
                            myMail.Attachments.Add(attachFile);
                        }
                    }
                }
                catch (Exception err)
                {
                    throw new Exception("There is an error when adding attachments:" + err);
                }

                SmtpClient smtp = new SmtpClient();

                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = true;
                smtp.EnableSsl = true;
                //Set the SMTP mail server
                smtp.Host = ConfigurationHelper.SmtpHost;

                smtp.Port = ConfigurationHelper.SmtpPort; ;
                //Specify the sender's e-mail address and password to verify the identity of the sender
                smtp.Credentials = new System.Net.NetworkCredential(ConfigurationHelper.MailFrom, ConfigurationHelper.MailPwd);

                //Send mail to SMTP mail server
                smtp.Send(myMail);
                return true;
            }
            catch (Exception e)
            {
                LoggerManager.Logger.Error(e);
                return false;
            }
        }
       
    }
}
