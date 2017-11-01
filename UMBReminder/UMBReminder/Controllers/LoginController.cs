using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using UMBReminder.Models;
using UMBReminder.ViewModel;
using UMBReminder.Helper;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace UMBReminder.Controllers
{
    public class LoginController : Controller
    {

        #region Constructor
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private string _sessionName = Helper.StaticData.SessionStatic.SessionName;
        private UserSession _userSession;
        public LoginController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _userSession = new UserSession();
        }
        #endregion        

        public IActionResult Index()
        {            
            return View();
        }  

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(Login model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            //var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        return RedirectToLocal(returnUrl);
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");
            //    case SignInStatus.RequiresVerification:
            //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            //    case SignInStatus.Failure:
            //    default:
            //        ModelState.AddModelError("", "Invalid login attempt.");
            //        return View(model);
            //}
            HttpClient client = new HttpClient();
            var content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("NIK", model.NIK),
                new KeyValuePair<string, string>("Password", model.Password)
            });
            string contentType = "application/x-www-form-urlencoded";
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(contentType));

            HttpResponseMessage response = await client.PostAsync("http://localhost:5900/api/Login/validate_login", content);

            if (response.IsSuccessStatusCode)
            {
                var apiResultStream = await response.Content.ReadAsStringAsync();
                var returnApi = JsonConvert.DeserializeObject<ResultApi<UserSession>>(apiResultStream);
                if (returnApi.Status == true)
                {
                    _userSession = new UserSession();
                    _userSession = returnApi.Payload;
                    _session.SetSession(_sessionName, _userSession);
                    returnUrl = "Index/Dashboard";
                    return RedirectToLocal(returnUrl);

                    
                }
                else
                {
                    //Set The Session
                    _session.Clear();
                    //End

                    ViewBag.Message = String.Format("NIK atau Password tidak sesuai");
                    return View("Index");
                }
            }
            else
            {
                return View();
            }
        }

        public void Logout()
        {
            _session.Clear();
            RedirectToLocal("Login/Login");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Dashboard");
        }

    }
}