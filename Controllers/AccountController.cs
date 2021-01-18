using System;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using allpax_service_record.Models;
using allpax_service_record.Models.View_Models;
using System.Data.SqlClient;
using System.Data;

using System.Net.Mail;
using System.Net;

namespace allpax_service_record.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private allpaxServiceRecordEntities db = new allpaxServiceRecordEntities();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Require the user to have a confirmed email before they can log on.
            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    ViewBag.errorMessage = "You must have a confirmed email to log on.";
                    return View("Error");
                }
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            //var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]//added by Goss
        public async Task<ActionResult> Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //comment out the line below to prevent the newly registered user from being logged in afterward
                    //the reason being that an admin will be creating new users, not the users themselves
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link

                    //BEGIN COMMENT OUT EMAIL CONFIRMATION
                    //string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //END COMMENT OUT EMAIL CONFIRMATION

                    //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");+

                    //BEGIN COMMENT OUT EMAIL CONFIRMATION
                    //SmtpClient smtp = new SmtpClient();
                    //END COMMENT OUT EMAIL CONFIRMATION

                    //BEGIN LOCAL SERVER HOSTED VERSION
                    //smtp.Host = "smtp.gmail.com";
                    //smtp.Port = 587;
                    //smtp.EnableSsl = true;
                    //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //smtp.UseDefaultCredentials = false;
                    //smtp.Credentials = new NetworkCredential("allpaxtesting@gmail.com", "Allpax_1234");

                    //string body = "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>";

                    //using (var message = new MailMessage("allpaxtesting@gmail.com", model.Email))
                    //{
                    //    message.Subject = "Test";
                    //    message.Body = body;
                    //    message.IsBodyHtml = true;
                    //    smtp.Send(message);
                    //}
                    //END LOCAL SERVER HOSTED VERSION

                    //BEGIN COMMENT OUT EMAIL CONFIRMATION
                    //BEGIN WEB SERVER HOSTED VERSION
                    //smtp.Host = "relay-hosting.secureserver.net";
                    //smtp.Port = 25;
                    //smtp.EnableSsl = false;
                    //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //smtp.UseDefaultCredentials = false;
                    //smtp.Credentials = new NetworkCredential("ph14185669651", "Allpax_1");

                    //string body = "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>";

                    //using (var message = new MailMessage("ph14185669651@secureserver.net", model.Email))
                    //{
                    //    message.Subject = "Confirm your e-mail address.";
                    //    message.Body = body;
                    //    message.IsBodyHtml = true;
                    //    smtp.Send(message);
                    //}
                    //END WEB SERVER HOSTED VERSION
                    //BEGIN COMMENT OUT EMAIL CONFIRMATION

                    //need to remove the password field from tbl_Users because the password is handled by ASP.net Identity

                    // Uncomment to debug locally 
                    // TempData["ViewBagLink"] = callbackUrl;

                    //BEGIN COMMENT OUT EMAIL CONFIRMATION
                    //ViewBag.Message = "Check your email and confirm your account, you must be confirmed "
                    //                + "before you can log in.";
                    //END COMMENT OUT EMAIL CONFIRMATION

                    //if user is selected as an administrator...
                    if (model.admin)
                    {
                        UserManager.AddToRole(user.Id, "Admin");//add this user to the Admin role
                        //System.Diagnostics.Debug.WriteLine(user.Id);
                    }

                    db.Database.ExecuteSqlCommand(
                    "UPDATE AspNetUsers " +
                    "SET " +

                    "name = {1}, shortName = {2}, admin = {3}, active = {4}, EmailConfirmed = {5} " +

                    "WHERE UserName = {0}",
                    model.UserName, model.name, model.ShortName, model.admin, model.active, "1");

                    //update the application table
                    //db.Database.ExecuteSqlCommand("Insert into tbl_Users (userName, password, name, shortName, admin, active) " +
                    //    "Values({0}, {1}, {2}, {3}, {4}, {5})", model.UserName, "password", model.name, model.ShortName, model.admin, model.active);

                    return RedirectToAction("GetUserAcctInfo", "Account");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
            //return View("got it");
        }

        //only a role of Admin can access this ActionResult
        [Authorize(Roles = "Admin")]//added by Goss
        public ActionResult GetUserAcctInfo()
        {
            RegisterViewModel regViewModel = new RegisterViewModel();
            List<vm_userAcctInfo> userAcctInfos = new List<vm_userAcctInfo>();
            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            sqlconn.Open();

            string sqlquery1 =
                "SELECT AspNetUsers.name, AspNetUsers.shortName, AspNetUsers.UserName, AspNetUsers.email, AspNetUsers.admin, AspNetUsers.active, AspNetUsers.Id " +

                "FROM [allpax_service_record].[dbo].[AspNetUsers]";

            SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
            SqlDataAdapter sda1 = new SqlDataAdapter(sqlcomm1);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                vm_userAcctInfo userAcctInfo = new vm_userAcctInfo();

                userAcctInfo.name = dr1[0].ToString();
                userAcctInfo.ShortName = dr1[1].ToString();
                userAcctInfo.UserName = dr1[2].ToString();
                userAcctInfo.Email = dr1[3].ToString();
                userAcctInfo.admin = (bool)dr1[4];
                userAcctInfo.active = (bool)dr1[5];
                userAcctInfo.aspNetId = dr1[6].ToString();

                userAcctInfos.Add(userAcctInfo);//add all of the revelevant data objects to dailyReportByID...
            }

            regViewModel.userAcctInfo = userAcctInfos;

            sqlconn.Close();

            return View(regViewModel);//...to be passed to the view
        }

        //only a role of Admin can access this ActionResult
        //UPDATE A USER'S ACCOUNT INFORMATION
        [Authorize(Roles = "Admin")]//added by Goss
        public ActionResult UpdateUserAcctInfo(vm_userAcctInfo userAcctInfoUpdate)
        {
            if (ModelState.IsValid)
            {
                //load the roles held by this user into a list for later comparison
                List<string> roles = UserManager.GetRoles(userAcctInfoUpdate.aspNetId).ToList();
                bool currentlyAdmin = roles.Contains("Admin");//determine if this user is currently an admin
                                                              //System.Diagnostics.Debug.WriteLine(currentlyAdmin);

                if ((currentlyAdmin) && (!userAcctInfoUpdate.admin))//if currently an admin, but selected to longer be an admin...
                {
                    UserManager.RemoveFromRole(userAcctInfoUpdate.aspNetId, "Admin");
                }

                if ((!currentlyAdmin) && (userAcctInfoUpdate.admin))//if not currently an admin, but selected to be an admin...
                {
                    UserManager.AddToRole(userAcctInfoUpdate.aspNetId, "Admin");
                }

                db.Database.ExecuteSqlCommand(
                    "UPDATE AspNetUsers " +
                    "SET " +

                    "Email = {1}, UserName = {2}, name = {3}, shortName = {4}, admin = {5}, active = {6} " +

                    "WHERE Id = {0}",
                    userAcctInfoUpdate.aspNetId, userAcctInfoUpdate.Email, userAcctInfoUpdate.UserName, userAcctInfoUpdate.name, userAcctInfoUpdate.ShortName,
                    userAcctInfoUpdate.admin, userAcctInfoUpdate.active);

                return Json(Url.Action("GetUserAcctInfo", "Account"));
                //return Json("complete");  
            }
           
            var modelErrors = new List<string>();

            foreach (var modelState in ModelState.Values)
            {
                foreach (var modelError in modelState.Errors)
                {
                    modelErrors.Add(modelError.ErrorMessage);
                }
            }
            
            userAcctInfoUpdate.ModelErrors = modelErrors;

            // If we got this far, something failed, redisplay form
            return View(userAcctInfoUpdate);
            //return View("got it");
        }

        //only a role of Admin can access this ActionResult
        //DELETE A USER'S ACCOUNT INFORMATION
        [Authorize(Roles = "Admin")]//added by Goss
        public ActionResult DeleteUserAcctInfo(vm_userAcctInfo userAcctInfoDelete)
        {
            db.Database.ExecuteSqlCommand("DELETE FROM AspNetUsers WHERE Id=({0})", userAcctInfoDelete.aspNetId);

            return Json(Url.Action("GetUserAcctInfo", "Account"));
            //return Json("complete");            
        }

        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var user = await UserManager.FindByNameAsync(model.Email);
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                //if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                SmtpClient smtp = new SmtpClient();

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                ////BEGIN ORIGINAL SEND E-MAIL LOGIC
                //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                ////await UserManager.SendEmailAsync(user.Email, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                ////BEGIN ORIGINAL SEND E-MAIL LOGIC

                //BEGIN LOCAL SERVER HOSTED VERSION
                //smtp.Host = "smtp.gmail.com";
                //smtp.Port = 587;
                //smtp.EnableSsl = true;
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //smtp.UseDefaultCredentials = false;
                //smtp.Credentials = new NetworkCredential("allpaxtesting@gmail.com", "Allpax_1234");

                //string body = "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>";

                //using (var message = new MailMessage("allpaxtesting@gmail.com", model.Email))
                //{
                //    message.Subject = "Reset password.";
                //    message.Body = body;
                //    message.IsBodyHtml = true;
                //    smtp.Send(message);
                //}
                //END LOCAL SERVER HOSTED VERSION

                //BEGIN WEB SERVER HOSTED VERSION
                smtp.Host = "relay-hosting.secureserver.net";
                smtp.Port = 25;
                smtp.EnableSsl = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("ph14185669651", "Allpax_1");

                string body = "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>";

                using (var message = new MailMessage("ph14185669651@secureserver.net", model.Email))
                {
                    message.Subject = "Reset your password.";
                    message.Body = body;
                    message.IsBodyHtml = true;
                    smtp.Send(message);
                }
                //END WEB SERVER HOSTED VERSION

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //canned logic was for the user name and the email address to be the same
            //var user = await UserManager.FindByNameAsync(model.Email);
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //return RedirectToAction("Index", "Home");
            return RedirectToAction("Login", "Account");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}