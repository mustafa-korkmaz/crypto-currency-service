using System;
using MoneyMarket.Common;
using MoneyMarket.Common.Helper;

namespace MoneyMarket.Business.Mailing
{
    public delegate void MailEventHandler(object sender, EventArgs e);

    public class MailHandler
    {
        #region singleton definition
        static readonly MailHandler _instance = new MailHandler();

        public static MailHandler Instance
        {
            get
            {
                return _instance;
            }
        }

        private MailHandler()
        {
            //initialize
            _mailFrom = Statics.GetConfigKey(ConfigKeys.MailFrom);
            _password = Statics.GetConfigKey(ConfigKeys.MailPass);
            _port = int.Parse(Statics.GetConfigKey(ConfigKeys.MailPort));
            _useSsl = bool.Parse(Statics.GetConfigKey(ConfigKeys.MailUseSsl));
            _mailServer = Statics.GetConfigKey(ConfigKeys.MailServer);
            _displayName = Statics.GetConfigKey(ConfigKeys.MailDisplayName);
        }

        #endregion singleton definition

        public event MailEventHandler MailSent;
        public event MailEventHandler MailSending;
        private readonly string _mailServer;
        private readonly string _mailFrom;
        private readonly string _displayName;
        private readonly string _password;
        private readonly int _port;
        private readonly bool _useSsl;

        private string _subject;
        /// <summary>
        /// sets subject of mail
        /// </summary>
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        private string _content;
        /// <summary>
        ///mail body content
        /// </summary>
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        private string _footer;
        /// <summary>
        /// mail content signature or footer
        /// </summary>
        public string Footer
        {
            get { return _footer; }
            set { _footer = value; }
        }

        #region MailEvents
        /// <summary>
        /// fires when mail successfully sent
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMailSent(EventArgs e)
        {
            MailSent?.Invoke(this, e);
        }

        /// <summary>
        /// fires when mail sending begins
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMailSending(EventArgs e)
        {
            MailSending?.Invoke(this, e);
        }
        #endregion

        /// <summary>
        /// Sends  mail 
        /// </summary>
        /// <returns></returns>
        public bool Send(string mailTo)
        {

#if DEBUG
            return true;
#endif
            OnMailSending(EventArgs.Empty);

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            System.Net.NetworkCredential cred = new System.Net.NetworkCredential(_mailFrom, _password);

            var smtpClient = new System.Net.Mail.SmtpClient(_mailServer, _port)
            {
                EnableSsl = _useSsl,
                UseDefaultCredentials = false,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network
            };

            try
            {
                mail.Subject = _subject;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.From = new System.Net.Mail.MailAddress(_mailFrom, _displayName);
                mail.To.Add(mailTo);
                mail.IsBodyHtml = true;
                mail.Body = SetMailBody();
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                smtpClient.Credentials = cred;
                smtpClient.Send(mail);
                mail.Dispose();
                OnMailSent(EventArgs.Empty);

                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Sets the body for mail
        /// </summary>
        /// <returns></returns>
        protected virtual string SetMailBody()
        {
            string body = string.Empty;

            //body += "Bu e-posta " + _displayName + " tarafından otomatik olarak oluşturulmuştur.</br>";
            body += _content;
            body += _footer;

            return body;
        }
    }


    public class ExceptionMail
    {
        private System.Exception exception;

        private string errorSource;

        public string ErrorSource
        {
            set
            {
                errorSource = value;
            }
        }

        private string errorInfo;

        public string ErrorInfo
        {
            set
            {
                errorInfo = value;
            }
        }

        public ExceptionMail(System.Exception exc)
        {
            this.exception = exc;
        }
        string SetMailBody()
        {
            string body = string.Empty;

            body += "Bu e-posta otomatik olarak oluşturulmuştur.";

            if (errorInfo != null)
            {
                body += Environment.NewLine + "Sistemdeki olası hata: " + errorInfo;
            }
            body += Environment.NewLine + "Sistemdeki hata mesajı: " + exception.Message;

            if (errorSource != null)
            {
                body += Environment.NewLine + "Sistemdeki hata kaynağı: " + errorSource;
            }

            return body;

        }


    }

}
