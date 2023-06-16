using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MedShop.Models;
using MedShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace MedShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> sigInManager;
        private readonly ILogger<AccountController> logger;
        private readonly IEmailSender emailSender;


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> sigInManager,
                                 ILogger<AccountController> logger, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.sigInManager = sigInManager;
            this.logger = logger;
            this.emailSender = emailSender;

        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            //externalLogins sadrzi exernal providers u StartUp-u: .AddGoggle, AddFacebook....
            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await sigInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                model.ExternalLogins = (await sigInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null && !user.EmailConfirmed
                              && (await userManager.CheckPasswordAsync(user, model.Password)))
                {
                    ModelState.AddModelError("", "Email još nije potvrđen!");
                    return View(model);
                }

                var result = await sigInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Neuspešan pokušaj prijave!");
                }

            }
            return View(model);
        }



        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                { UserName = model.Email, Email = model.Email };

                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (sigInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }
                    //generate confirmation link for email:
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                                         new { userId = user.Id, token = token }, Request.Scheme);

                    //logger.Log(LogLevel.Warning, confirmationLink);

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

                    ViewBag.ErrorTitle = "Registracija je uspešno izvršena!";
                    ViewBag.ErrorMessage = "Pre prijave, molimo vas potvrdite vaš email pomoću linka koji smo vam poslali na vašu email adresu!";

                    return View("Error");
                    //await sigInManager.SignInAsync(user, false);

                    //return RedirectToAction("Index","Home");
                }

                foreach (var error in result.Errors)
                {

                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await sigInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {

                return Json($"Email {email} is already in use");
            }
            else
            {
                return Json(true);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {

            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string returnUrl, string provider)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl = returnUrl });

            var properties = sigInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await sigInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError("", "Error from external provider: " + remoteError);
                return View("Login", model);
            }

            var info = await sigInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                ModelState.AddModelError("", "Error loading external login info.");
                return View("Login", model);

            }

            //or var email=info.Principal.FindFirstValue(ClaimTypes.Email)
            var email = info.Principal.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;

            ApplicationUser user = null;
            if (email != null)
            {

                user = await userManager.FindByEmailAsync(email);

                //check if email is confirmed

                if (user != null && !user.EmailConfirmed)

                {
                    ModelState.AddModelError("", "Email not confirmed yet");
                    return View("Login", model);
                }


            }
       
            var signInResult = await sigInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                //or var email=onfo.Principal.FindFirstValue(ClaimTypes.Email)
                // var email = info.Principal.Claims.FirstOrDefault(x=>x.Type=="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;

                if (email != null)
                {

                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = email,
                            Email = email
                        };
                        await userManager.CreateAsync(user);

                        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                        var confirmationLink = Url.Action("ConfirmEmail", "Account",
                                             new { userId = user.Id, token = token }, Request.Scheme);

                        //logger.Log(LogLevel.Warning, confirmationLink);

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

                        ViewBag.ErrorTitle = "Registration successfull";
                        ViewBag.ErrorMessage = "Before you can Login, please confirm your email by clicking on the confirmation link we have emailed you";

                        return View("Error");
                    }

                    await userManager.AddLoginAsync(user, info);

                    await sigInManager.SignInAsync(user, false);

                    return LocalRedirect(returnUrl);

                }
                else
                {
                    return View("Error");
                }
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                RedirectToAction("Index", "Home");
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User Id {userId} is invalid";
                return View("NotFound");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View();
            }
            else
            {
                ViewBag.ErrorTitle = "Email can not be confirmed";
                return View("Error");
            }

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null && (await userManager.IsEmailConfirmedAsync(user)))
                {

                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var confirmationLink = Url.Action("ResetPassword", "Account",
                                            new { email = user.Email, token = token }, Request.Scheme);

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


                    return View("ForgotPasswordConfirmation");
                }

            }
            return View("ForgotPasswordConfirmation");

        }

        [HttpGet]
        [AllowAnonymous]
        public  IActionResult ResetPassword(string email, string token)
        {
            if (email != null && token != null)
            {
                ResetPasswordViewModel model = new ResetPasswordViewModel()
                {
                    Email = email,
                    Token = token
                };
 
                return View(model);
            }
            ModelState.AddModelError("", "Invalid password reset token");
            return View();

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                  var result=  await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }
                    else
                    {
                        foreach(var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(model);
                    }
                }
                else
                {
                    return View("ResetPasswordConfirmation");
                }
            }
            return View(model);
        }
    }
}
