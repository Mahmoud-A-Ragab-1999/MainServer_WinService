using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Utilities;

namespace Utilities
{
    public class EmailModel
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<KeyValuePair<string, string>> AttachmentVPaths { get; set; }
        public List<string> CC { get; set; }
        public List<string> BCC { get; set; }

        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        
        public string RecipientName { get; set; }
        public string RecipientEmail { get; set; }

        public string SMTP { get; set; }
        public int? SMTPPort { get; set; }// = 2525;
        public string CredentialUsername { get; set; }
        public string CredentialPassword { get; set; }
    }

    public static class EmailUtilities
    {
        public static bool Send(EmailModel model, out string error)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.Subject) && !string.IsNullOrEmpty(model.Body) && !string.IsNullOrEmpty(model.RecipientEmail))
                {
                    if (string.IsNullOrEmpty(model.RecipientName) || string.IsNullOrWhiteSpace(model.RecipientName))
                        model.RecipientName = null;

                    #region Credentials
                    string credentialUsername = (string.IsNullOrEmpty(model.CredentialUsername) ? ConfigurationManager.AppSettings["EmailCredentialUsername"] : model.CredentialUsername);
                    string credentialPassword = (string.IsNullOrEmpty(model.CredentialPassword) ? ConfigurationManager.AppSettings["EmailCredentialPassword"] : model.CredentialPassword);

                    string smtpName = (string.IsNullOrEmpty(model.SMTP) ? ConfigurationManager.AppSettings["SMTP"] : model.SMTP);
                    int sMTPPort = (!model.SMTPPort.HasValue || model.SMTPPort <= 0 ? int.Parse(ConfigurationManager.AppSettings["SMTPPort"]) : model.SMTPPort.Value);
                    #endregion

                    if (string.IsNullOrEmpty(model.SenderEmail))
                    {
                        model.SenderEmail = ConfigurationManager.AppSettings["SaloonyEmail"];
                        model.SenderName = model.SenderName ?? "Saloony";
                    }
                    
                    using (MailMessage mailMSG = new MailMessage(new MailAddress(model.SenderEmail, model.SenderName), new MailAddress(model.RecipientEmail, model.RecipientName)))
                    {
                        #region Prepare Email
                        mailMSG.Subject = model.Subject;
                        mailMSG.Body = model.Body;
                        mailMSG.IsBodyHtml = true;
                        mailMSG.BodyEncoding = Encoding.UTF8;
                        mailMSG.SubjectEncoding = Encoding.UTF8;

                        #region CC
                        if (model.CC != null && model.CC.Any())
                        {
                            foreach (string ccEmail in model.CC)
                            {
                                MailAddress cc = new MailAddress(ccEmail);
                                mailMSG.CC.Add(cc);
                            }
                        }
                        #endregion
                        #region BCC
                        if (model.BCC != null && model.BCC.Any())
                        {
                            foreach (string bccEmail in model.BCC)
                            {
                                MailAddress bcc = new MailAddress(bccEmail);
                                mailMSG.Bcc.Add(bcc);
                            }
                        }
                        #endregion                       
                        #endregion

                        #region Send
                        using (SmtpClient smtp = new SmtpClient())
                        {
                            smtp.Host = smtpName;
                            smtp.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["EmailEnableSsl"]);
                            NetworkCredential networkCred = new NetworkCredential(credentialUsername, credentialPassword);
                            smtp.Timeout = 60000000;
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = networkCred;
                            smtp.Port = sMTPPort;

                            smtp.Send(mailMSG);
                        } 
                        #endregion
                    }
                    error = string.Empty;
                    return true;
                }
                else
                {
                    error = "Invalid Data";
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return false;
        }


        public static bool SendEmail(string userEmail, string userName, string subject, string body, List<KeyValuePair<string, string>> attachmentsPaths, List<string> ccEmails, out string error)
        {
            try
            {
                if (!string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(body) && !string.IsNullOrEmpty(userEmail))
                {
                    if (string.IsNullOrEmpty(userName) || string.IsNullOrWhiteSpace(userName))
                        userName = null;

                    string saloonyEmail = ConfigurationManager.AppSettings["SaloonyEmail"];                    
                    string password = ConfigurationManager.AppSettings["EmailCredentialPassword"];
                    string smtpName = ConfigurationManager.AppSettings["SMTP"];
                    int sMTPPort = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);

                    string toEmail, fromEmail;
                    string toName, fromName;

                    toEmail = userEmail;
                    toName = userName;
                    fromEmail = saloonyEmail;
                    fromName = "Smart Monitoring";

                    using (MailMessage mailMSG = new MailMessage(new MailAddress(fromEmail, fromName), new MailAddress(toEmail, toName)))
                    {
                        mailMSG.Subject = subject;
                        mailMSG.Body = body;
                        mailMSG.IsBodyHtml = true;
                        mailMSG.BodyEncoding = Encoding.UTF8;
                        mailMSG.SubjectEncoding = Encoding.UTF8;

                        if (ccEmails != null && ccEmails.Any())
                        {
                            foreach (string email in ccEmails)
                            {
                                MailAddress cc = new MailAddress(email);
                                mailMSG.CC.Add(cc);
                            }
                        }                       

                        using (SmtpClient smtp = new SmtpClient())
                        {
                            smtp.UseDefaultCredentials = true;
                            smtp.Host = smtpName;
                            smtp.EnableSsl = true;
                            NetworkCredential networkCred = new NetworkCredential(saloonyEmail, password);
                            smtp.Timeout = 60000000;                            
                            smtp.Credentials = networkCred;
                            smtp.Port = sMTPPort;                            
                            smtp.Send(mailMSG);
                        }
                    }
                    error = string.Empty;
                    return true;
                }
                else
                {
                    error = "Invalid Data";
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return false;
        }


        public static bool SendEmailVersion2(string userEmail, string userName, string subject, string body, List<KeyValuePair<string, string>> attachmentsPaths, List<string> ccEmails, bool toSaloony, out string error)
        {
            try
            {
                if (!string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(body) && !string.IsNullOrEmpty(userEmail))
                {
                    if (string.IsNullOrEmpty(userName) || string.IsNullOrWhiteSpace(userName))
                        userName = null;
                    string saloonyEmail = "info@saloony.net";
                    string password = "H_28113_s";
                    string smtpName = "mail.saloony.net";//"mi3-sr8.supercp.com"; // 
                    int sMTPPort = 2525;//465;// 

                    string toEmail, fromEmail;
                    string toName, fromName;

                    toEmail = userEmail;
                    toName = userName;
                    fromEmail = saloonyEmail;
                    fromName = "Saloony";
                    using (MailMessage mailMSG = new MailMessage(new MailAddress(fromEmail, fromName), new MailAddress(toEmail, toName)))
                    {
                        mailMSG.Subject = subject;
                        mailMSG.Body = body;
                        mailMSG.IsBodyHtml = true;
                        mailMSG.BodyEncoding = Encoding.UTF8;
                        mailMSG.SubjectEncoding = Encoding.UTF8;

                        if (ccEmails != null && ccEmails.Any())
                        {
                            foreach (string email in ccEmails)
                            {
                                MailAddress cc = new MailAddress(email);
                                mailMSG.CC.Add(cc);
                            }
                        }                       

                        using (SmtpClient smtp = new SmtpClient())
                        {
                            smtp.Host = smtpName;
                            smtp.EnableSsl = true;
                            NetworkCredential networkCred = new NetworkCredential(saloonyEmail, password);
                            smtp.Timeout = 60000000;
                            smtp.UseDefaultCredentials = true;
                            smtp.Credentials = networkCred;
                            smtp.Port = sMTPPort;

                            smtp.Send(mailMSG);
                        }
                    }
                    error = string.Empty;
                    return true;
                }
                else
                {
                    error = "Invalid Data";
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return false;
        }
    }
}
