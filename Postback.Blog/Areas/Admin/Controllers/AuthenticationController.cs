using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using Postback.Blog.App.Data;
using Postback.Blog.App.Messaging;
using Postback.Blog.App.Services;
using Postback.Blog.Areas.Admin.Models;
using Postback.Blog.Models;
using Raven.Client;
using System.Linq;
using Microsoft.Practices.ServiceLocation;

namespace Postback.Blog.Areas.Admin.Controllers
{
    [AppInit]
    public class AuthenticationController : Controller
    {
        private readonly ICryptographer crypto;
        private readonly IDocumentSession session;
        private readonly IAuth auth;
        private readonly IMessagingService messaging;

        public AuthenticationController(ICryptographer cryptographer, IAuth auth, IMessagingService messaging)
        {
            crypto = cryptographer;
            this.session = ServiceLocator.Current.GetInstance<IDocumentSession>();
            this.auth = auth;
            this.messaging = messaging;
        }

        public ActionResult Index()
        {
            return View(new AuthenticationModel());
        }

        [HttpPost]
        public ActionResult Index(AuthenticationModel authentication)
        {
            if (ModelState.IsValid)
            {
                var user = session.Query<User>().SingleOrDefault(u => u.Email == authentication.Email);
                if(user !=null && crypto.GetPasswordHash(authentication.Password,user.PasswordSalt) == user.PasswordHashed)
                {
                   auth.DoAuth(user.Email, true);
                    if (Request.QueryString["ReturnUrl"] != null)
                    {
                        return Redirect(Request.QueryString["ReturnUrl"]);
                    }
                    return RedirectToAction("index", "dashboard");
                }

                ModelState.AddModelError("Email", "Unknown");
            }

            return View("Index", new AuthenticationModel());
        }

        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "authentication");
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetNewPassword(string email)
        {
            var user = session.Query<User>().SingleOrDefault(u => u.Email == email);
            if(user != null)
            {
                var generatedPassword = crypto.CreatePassword(6);

                user.PasswordSalt = crypto.CreateSalt();
                user.PasswordHashed = crypto.GetPasswordHash(generatedPassword, user.PasswordSalt);

                var message = new NewPasswordMessage { User = user, NewPassword = generatedPassword };
                messaging.Send(message);

                session.Store(user);
                session.SaveChanges();

                return RedirectToAction("forgotpasswordconfirm");
            }

            ModelState.AddModelError("Email", "Unknown");
            return View("ForgotPassword");
        }

        public ActionResult ForgotPasswordConfirm()
        {
            return View();
        }
    }
}
