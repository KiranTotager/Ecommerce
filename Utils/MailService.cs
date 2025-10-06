using ECommerce.DTOs.CustomerDTOs;
using ECommerce.Interfaces.IUtils;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.Net;
using System.Net.Mail;

namespace ECommerce.Utils
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<MailService> _logger;
        public MailService(IConfiguration configuration,ILogger<MailService> logger)
        {
            _config = configuration;
            _logger = logger;
        }

        public async Task AdminContact(ContactUsDto contactUs)
        {
            string subject = "contact request from customer";
            string body = $@"<html>
                  <body>
                    <p>Dear Admin,</p>
                    <p>A new contact request has been submitted from the website. The details are as follows:</p>
                
                    <p><b>Name:</b> {contactUs.FullName}</p>
                    <p><b>Email:</b> {contactUs.Email}</p>
                    <p><b>Phone number:</b> {contactUs.PhoneNumber}</p>
                    <p><b>Message:</b></p>
                    <p>{contactUs.Message}</p>
                
                    <br/>
                
                    <br/>
                    <p>Best Regards,</p>
                    <p><b>NextWebi E-Commerce System</b></p>
                  </body>
                </html>
                ";
            string adminMail = _config.GetValue<string>("MailSettings:AdminMail");

            await MailSenderHelper(adminMail,subject,body);
           
        }

        public async Task ConfirmationTokenSender(string toMail, string url)
        {
            string subject = "Confirm Your Email Address";
            string domain = _config.GetValue<string>("MailSettings:domain");
            string body = $@"
        <html>
            <body>
                <p>Dear User,</p>
                <p>Thank you for registering with us. To complete your account setup, please confirm your email address by clicking the link below:</p>
                <p>
                    <a href='{url}' 
                       style='display:inline-block;padding:10px 20px;color:#fff;background-color:#007bff;
                              text-decoration:none;border-radius:5px;'>
                        Confirm Email
                    </a>
                </p>
                <p>If you did not create an account, you can safely ignore this email.</p>
                <br/>
                <p>Best Regards,</p>
                <p><b>NextWebi E-Commerce Team</b></p>
            </body>
        </html>";
            await MailSenderHelper(toMail, subject, body);
        }

        public async Task MailSenderHelper(string toMail, string subject, string body)
        {
            string Host = _config.GetValue<string>("MailSettings:Host");
            string From = _config.GetValue<string>("MailSettings:From");
            string PassKey = _config.GetValue<string>("MailSettings:PassKey");
            int Port = _config.GetValue<int>("MailSettings:Port");
            bool EnableSSL = _config.GetValue<bool>("MailSettings:EnableSSL");
            SmtpClient client = new SmtpClient(Host, Port);
            client.Credentials = new NetworkCredential(From, PassKey);
            client.EnableSsl = EnableSSL;
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(From);
            mailMessage.To.Add(toMail);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = body;
            client.Send(mailMessage);
        }

        public async Task OTPSender(string toMail, string OTP)
        {
            string subject = "Your One-Time Password (OTP) for Account Verification";
            string body = $@"
        <html>
            <body>
                <p>Dear User,</p>
                <p>Your One-Time Password (OTP) is:</p>
                <h2 style='color:blue;'>{OTP}</h2>
                <p>This OTP is valid for <b>5 minutes</b>. Please do not share it with anyone.</p>
                <br/>
                <p>Thank you,</p>
                <p><b>NextWebi E-Commerce Team</b></p>
            </body>
        </html>";
            await MailSenderHelper(toMail, subject, body);
        }
    }
}
