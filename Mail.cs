using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class MailHelper
    {
        private static string _smtpAddress = "";
        private static int _smtpPort = 0;
        private static string _body = "";
        private static string _from = "";
        private static string _smtpLogin;
        private static string _smtpPassword;

        public static void Configure()
        {
            _smtpAddress = ConfigurationManager.AppSettings["smtpAddress"];
            _smtpPort = Int32.Parse(ConfigurationManager.AppSettings["smtpPort"]);
            _smtpLogin = ConfigurationManager.AppSettings["smtpLogin"];
            _smtpPassword = ConfigurationManager.AppSettings["smtpPassword"];
            string bodyFile = ConfigurationManager.AppSettings["mailBodyFile"];
            if (File.Exists(bodyFile))
                _body = File.ReadAllText(bodyFile);
            _from = ConfigurationManager.AppSettings["mailFrom"];
        }

        public static void SendMailAndFile(string to, string subject, string file)
        {
            SendMailAndFile(_from, to, subject, _body, file);
        }

        public static void SendMailHtml(string to, string subject, string body)
        {
            SendMail(_from, to, subject, body, true);
        }

        public static void SendMailAndFile(string from, string to, string subject, string body, string file)
        {
            MailMessage message = new MailMessage(from, to)
            {
                Subject = subject,
                Body = body
            };
            SmtpClient client = new SmtpClient(_smtpAddress)
            {
                Port = _smtpPort
            };
            if (_smtpLogin != "" && _smtpLogin != null)
            {
                client.Credentials = new System.Net.NetworkCredential(_smtpLogin, _smtpPassword);
            }
            else
            {
                client.UseDefaultCredentials = true;
            }

            //client.EnableSsl = true;

            Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);

            // Add time stamp information for the file.
            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(file);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(file);

            // Add the file attachment to this email message.
            message.Attachments.Add(data);


            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception caught in CreateTestMessage2(): {0}", ex.ToString());
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}", ex.ToString());
                Console.WriteLine("[Error] MailHelper - " +  ex.ToString());
                Debug.WriteLine("[Error] MailHelper - " +  ex.ToString());
                //throw ex;
            }

        }

        public static void SendMail(string to, string subject, string body)
        {
            SendMail(_from, to, subject,body);
        }

        public static void SendMail(string from, string to, string subject, string body, bool IsBodyHtml = false)
        {
            MailMessage message = new MailMessage(from, to)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = IsBodyHtml
            };
            message.Headers.Add("Message-Id",
                         String.Format("<{0}@{1}>",
                         Guid.NewGuid().ToString(),
                        from));
            message.Headers.Add("Content-type", "text/html; charset= iso-8859-1");
            SmtpClient client = new SmtpClient(_smtpAddress)
            {
                Port = _smtpPort
            };
            if (_smtpLogin != "" && _smtpLogin != null)
            {
                client.Credentials = new System.Net.NetworkCredential(_smtpLogin, _smtpPassword);
            }
            else
            {
                client.UseDefaultCredentials = true;
            }

            client.EnableSsl = true;

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception caught in CreateTestMessage2(): {0}", ex.ToString());
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}", ex.ToString());
                Console.WriteLine("[Error] MailHelper - " + ex.ToString());
                Debug.WriteLine("[Error] MailHelper - " + ex.ToString());
                //throw ex;
            }

        }
    }
}
