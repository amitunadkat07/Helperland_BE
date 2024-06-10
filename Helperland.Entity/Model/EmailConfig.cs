using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;

namespace Helperland.Entity.Model
{
    public class EmailConfig
    {
        public string? From { get; set; }

        public string? SmtpServer { get; set; }

        public int Port { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }

        #region SendMail
        public bool SendMail(String To, String Subject, String Body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("", From));
                message.To.Add(new MailboxAddress("", To));
                message.Subject = Subject;
                message.Body = new TextPart("html")
                {
                    Text = Body
                };
                using var client = new MailKit.Net.Smtp.SmtpClient();
                client.Connect(SmtpServer, Port, false);
                client.Authenticate(UserName, Password);
                client.Send(message);
                client.Disconnect(true);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Encode_Decode
        public string Encode(string encode)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encode);
            return Convert.ToBase64String(encoded);
        }
        public string Decode(string decode)
        {
            byte[] encoded = Convert.FromBase64String(decode);
            return System.Text.Encoding.UTF8.GetString(encoded);
        }
        #endregion
    }
}
