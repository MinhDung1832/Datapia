namespace Lib.Utils.Package
{
    #region Using System Library (.NET Framework v4.5.2)

    using ActiveUp.Net.Mail;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    #endregion Using System Library (.NET Framework v4.5.2)

    public enum HostPort
    {
        /// <summary>
        /// IMAP default port: 993 (Secure SSL)
        /// </summary>
        IMAP_SSL = 993,

        /// <summary>
        /// IMAP default port: 143 (Non-Encrypted)
        /// </summary>
        IMAP = 143,

        /// <summary>
        /// POP3 default port: 995 (Secure SSL)
        /// </summary>
        POP3_SSL = 995,

        /// <summary>
        /// POP3 default port: 110 (Non-Encrypted)
        /// </summary>
        POP3 = 110
    }

    public enum HostProtocol
    {
        /// <summary>
        /// Invalid protocol
        /// </summary>
        INVALID = 0,

        /// <summary>
        /// Imap4 protocal (Secure SSL)
        /// </summary>
        IMAP_SSL = 1,

        /// <summary>
        /// Imap4 protocal (Non-Encrypted)
        /// </summary>
        IMAP = 2,

        /// <summary>
        /// Pop3 protocal (Secure SSL)
        /// </summary>
        POP3_SSL = 3,

        /// <summary>
        /// Pop3 protocal (Non-Encrypted)
        /// </summary>
        POP3 = 4
    }

    public class MailConfig
    {
        public HostProtocol Protocol => (HostProtocol)ConfigurationManager.AppSettings.Get("Protocol").ToInt(0).Value;

        public string DomainName => ConfigurationManager.AppSettings.Get("DomainName").LowerCase();

        public string Host => ConfigurationManager.AppSettings.Get("HostMail").LowerCase();

        public int Port
        {
            get
            {
                switch (Protocol)
                {
                    case HostProtocol.IMAP_SSL:
                        return (int)HostPort.IMAP_SSL;

                    case HostProtocol.IMAP:
                        return (int)HostPort.IMAP;

                    case HostProtocol.POP3_SSL:
                        return (int)HostPort.POP3_SSL;

                    case HostProtocol.POP3:
                        return (int)HostPort.POP3;

                    default:
                        return -1;
                }
            }
        }
    }

    public class MailRepository
    {
        protected MailConfig Config;
        protected string Message { get; set; }
        protected Pop3Client Pop3 { get; set; }
        protected Imap4Client Imap4 { get; set; }

        public MailRepository()
        {
            Pop3 = new Pop3Client();
            Imap4 = new Imap4Client();
            Config = new MailConfig();
        }

        public bool IsOk(string Username, string Password)
        {
            bool isOk = false;
            if (Config.Protocol == HostProtocol.IMAP_SSL)
            {
                try
                {
                    Imap4.ConnectSsl(Config.Host, Config.Port);
                    Imap4.LoginFast(Username, Password);
                    isOk = true;
                }
                catch (Imap4Exception er)
                {
                    Message = er.Message;
                    isOk = false;
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                    isOk = false;
                }
                finally
                {
                    if (Imap4.IsConnected)
                        Imap4.Disconnect();
                }
            }
            else if (Config.Protocol == HostProtocol.IMAP)
            {
                try
                {
                    Imap4.Connect(Config.Host, Config.Port);
                    Imap4.LoginFast(Username, Password);
                    isOk = true;
                }
                catch (Imap4Exception er)
                {
                    Message = er.Message;
                    isOk = false;
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                    isOk = false;
                }
                finally
                {
                    if (Imap4.IsConnected)
                        Imap4.Disconnect();
                }
            }
            else if (Config.Protocol == HostProtocol.POP3_SSL)
            {
                try
                {
                    Pop3.ConnectSsl(Config.Host, Config.Port, Username, Password);
                    isOk = true;
                }
                catch (Pop3Exception er)
                {
                    Message = er.Message;
                    isOk = false;
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                    isOk = false;
                }
                finally
                {
                    if (Pop3.IsConnected)
                        Pop3.Disconnect();
                }
            }
            else if (Config.Protocol == HostProtocol.POP3)
            {
                try
                {
                    Pop3.Connect(Config.Host, Config.Port, Username, Password);
                    isOk = true;
                }
                catch (Pop3Exception er)
                {
                    Message = er.Message;
                    isOk = false;
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                    isOk = false;
                }
                finally
                {
                    if (Pop3.IsConnected)
                        Pop3.Disconnect();
                }
            }
            return isOk;
        }

        public string ToMail(string Username) => (Username.ToTrim().Contains("@" + Config.DomainName) ? Username.ToTrim() : Username.ToTrim() + "@" + Config.DomainName).LowerCase();

        public IEnumerable<Message> GetAllMails(string mailBox) => GetMails(mailBox, "ALL").Cast<Message>();

        public IEnumerable<Message> GetUnreadMails(string mailBox) => GetMails(mailBox, "UNSEEN").Cast<Message>();

        private MessageCollection GetMails(string mailBox, string searchPhrase)
        {
            Mailbox mails = Imap4.SelectMailbox(mailBox);
            MessageCollection messages = mails.SearchParse(searchPhrase);
            return messages;
        }
    }
}