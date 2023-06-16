using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MedShop.Models;
using MedShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MedShop.Controllers.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> sigInManager;
        private readonly IEmailSender emailSender;
        public AccountApiController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> sigInManager, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.sigInManager = sigInManager;
            this.emailSender = emailSender;

        }

       
        // Post https://localhost:44336/api/AccountApi/GetCredentials/GetCredentials
        [HttpPost]
        public async Task<ActionResult> GetCredentials(LoginApiRequestModel model)
        {
            string cookie = "";

            if (ModelState.IsValid)
            {


                //deo za proveru da li je Email potvrdjen!
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null && !user.EmailConfirmed
                              && (await userManager.CheckPasswordAsync(user, model.Password)))
                {
                    //ModelState.AddModelError("", "Email not confirmed yet");
                    return new JsonResult(new LoginApiResponseModel
                    {
                        StatusE = LoginResultStatus.EmailNotConfirmed

                    });
                }

                if (user == null)
                {
                    return new JsonResult(new LoginApiResponseModel
                    {

                        StatusE = LoginResultStatus.WrongCredentials


                    });
                }

                var result = await sigInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    foreach (var headers in Response.Headers.Values)
                    {
                        foreach (var header in headers)
                            if (header.StartsWith(".AspNetCore.Identity.Application="))
                            {
                                cookie = header.Split("=")[1].Split(";")[0];
                            }
                    }

                    return new JsonResult(new LoginApiResponseModel
                    {
                        Cookie = cookie,
                        StatusE = LoginResultStatus.Success


                    });
                }
                else
                {
                    //ModelState.AddModelError("", "Invalid Login Attempt!");
                    return new JsonResult(new LoginApiResponseModel
                    {
                        StatusE = LoginResultStatus.WrongCredentials
                    });
                }
            }

            return new JsonResult(new LoginApiResponseModel
            {
                StatusE = LoginResultStatus.WrongCredentials
            });

        }


        [HttpPost]
        public async Task<ActionResult> RegisterUser(RegisterApiRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                { UserName = model.Email, Email = model.Email };

                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
 
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                                         new { userId = user.Id, token = token }, Request.Scheme);

                    Message mailMessage = new Message()
                    {
                        Subject = "Email Confirmation",
                        Content = "To Confirm your email, click here: " + confirmationLink,
                        To = new List<MailboxAddress>()
                            {
                                new MailboxAddress(user.Email)
                            }

                    };

                    emailSender.SendEmail(mailMessage);

                    return new JsonResult(new RegisterApiResponseModel
                    {
                        StatusE = RegisterResultStatus.Success
                    });
                }
                else
                {
                    var errorMessage = "";
                    foreach (var error in result.Errors)
                    {
                        //todo: Napraviti stringBuilder ovde!!!!
                        errorMessage = error.Description + "; ";
                    }
                    return new JsonResult(new RegisterApiResponseModel
                    {
                        ErrorMessage = errorMessage
                    }); ;
                }
            }
            else
            {
                return new JsonResult(new RegisterApiResponseModel
                {
                    StatusE = RegisterResultStatus.InvalidInsertValues
                });
            }
        }

        public static Boolean SendMail(String to, String subject, String body, Boolean isBodyHtml, MailPriority mailPriority)
        {
            try
            {
                // Configure mail client (may need additional
                // code for authenticated SMTP servers)
                SmtpClient mailClient = new SmtpClient("smtp.gmail.com", 587);
                mailClient.UseDefaultCredentials = false;

                // set the network credentials
                mailClient.Credentials = new NetworkCredential("yamaica95kw@gmail.com", "montana9000");
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                //enable ssl
                mailClient.EnableSsl = true;

                // Create the mail message (from, to, subject, body)
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("yamaica95kw@gmail.com");
                mailMessage.To.Add(to);

                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = isBodyHtml;
                mailMessage.Priority = mailPriority;

                // send the mail
                mailClient.Send(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            //model state za if email is invalid or required
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null && (await userManager.IsEmailConfirmedAsync(user)))
                {
                    //generate password reset token
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var confirmationLink = Url.Action("ResetPassword", "Account",
                                            new { email = user.Email, token = token }, Request.Scheme);

                    //logger.Log(LogLevel.Warning, confirmationLink);

                    Message mailMessage = new Message()
                    {
                        Subject = "Password Reset",
                        Content = "To reset your password, click here: " + confirmationLink,
                        To = new List<MailboxAddress>()
                        {
                            new MailboxAddress(user.Email)
                        }
                    };

                    emailSender.SendEmail(mailMessage);

                    return new JsonResult(new ForgotPasswordApiResponseModel
                    {
                        StatusE = ForgotPasswordResultStatus.Success

                    });
                }

            }
            return new JsonResult(new ForgotPasswordApiResponseModel
            {
                StatusE = ForgotPasswordResultStatus.WrongCredentials
            });

        }

        [HttpGet]
        public async Task<string> Logout()
        {
            await sigInManager.SignOutAsync();
            return "OK";

        }
    }
}

