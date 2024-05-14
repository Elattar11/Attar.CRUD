using Attar.CRUD.DAL.Entities;
using Attar.CRUD.PL.Services.EmailService;
using Attar.CRUD.PL.Services.MailKitService;
using Attar.CRUD.PL.ViewModels.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Attar.CRUD.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _conf;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IMailSettings _mailSettings;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            IConfiguration conf,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender, 
            IMailSettings mailSettings
            )
        {
            _userManager = userManager;
            _conf = conf;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _mailSettings = mailSettings;
        }
        #region Sign Up

        public IActionResult SignUp() => View();

        [HttpPost]
        
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);

                if (user is null) 
                {
                    user = new ApplicationUser()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.Username,
                        Email = model.Email,
                        IsAgree = model.IsAgree,

                    };

                    var result = await _userManager.CreateAsync(user ,model.Password);

                    if (result.Succeeded) 
                        return RedirectToAction(nameof(SignIn));

                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }

                ModelState.AddModelError(string.Empty, "Username already exist");

            }

            return View(model);
        }


        #endregion

        #region Sign In

        public IActionResult SignIn() => View();

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is not null) 
                { 
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);

                    if (flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user,model.Password, model.RememberMe , false);

                        if (result.IsLockedOut)
                            ModelState.AddModelError(string.Empty, "Your account is locked");

                        if (result.Succeeded)
                            return RedirectToAction(nameof(HomeController.Index), "Home");

                        if (result.IsNotAllowed)
                            ModelState.AddModelError(string.Empty, "You account is not confirmed");





                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid Login");
            }

            return View(model);
        }


        #endregion

        #region Sign Out

        public async new Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
        #endregion

        #region Forget Passowrd

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordEmail(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var resetPasswordUrl = Url.Action("ResetPassword", "Account", new { email = user.Email , token = resetPasswordToken } , "https" , "localhost:44301");

                    //await _emailSender.SendAsync(
                    //    from: _conf["EmailSettings:senderEmail"],
                    //    recipients: model.Email,
                    //    subject: "Reset your password",
                    //    body: resetPasswordUrl);

                    var mail = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = resetPasswordUrl
                    };
                    _mailSettings.SendMail(mail);
                    return RedirectToAction(nameof(CheckYourInbox));
                }

                ModelState.AddModelError(string.Empty, "There is no account with this Email");
            }

            return View(model);
        }


        public IActionResult CheckYourInbox()
        {
            return View();
        }
        #endregion

        #region Reset Password

        public IActionResult ResetPassword(string email , string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;

                var user = await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    await _userManager.ResetPasswordAsync(user , token,model.NewPassword);
                    return RedirectToAction(nameof(SignIn));
                }

                ModelState.AddModelError(string.Empty, "Url is not valid");
                
            }

            return View(model);
        }
        #endregion

        #region Google Login

        public IActionResult GoogleLogin()
        {
            var prop = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleResponse))
            };

            return Challenge(prop , GoogleDefaults.AuthenticationScheme);
             
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(
                    claim => new
                    {
                        claim.Issuer,
                        claim.OriginalIssuer,
                        claim.Type,
                        claim.Value
                    }
                );

            return RedirectToAction("Index", "Home");
        }
        #endregion



        #region Facebook Login

        public IActionResult FacebookLogin()
        {
            var prop = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(FacebookResponse))
            };

            return Challenge(prop, FacebookDefaults.AuthenticationScheme);

        }

        public async Task<IActionResult> FacebookResponse()
        {
            var result = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(
                    claim => new
                    {
                        claim.Issuer,
                        claim.OriginalIssuer,
                        claim.Type,
                        claim.Value
                    }
                );

            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}
