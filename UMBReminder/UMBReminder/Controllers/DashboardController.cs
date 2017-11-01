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
    public class DashboardController : Controller
    {

        #region Constructor
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private string _sessionName = Helper.StaticData.SessionStatic.SessionName;
        private UserSession _userSession;
        public DashboardController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _userSession = new UserSession();
        }
        #endregion        

        //public IActionResult Login()
        //{
        //    return View();
        //}

        public IActionResult Index()
        {
            if (_session.GetSession<UserSession>(_sessionName) == null) {
                return RedirectToLogin();
            }
            return View();
        }

        public IActionResult Schedule()
        {
            if (_session.GetSession<UserSession>(_sessionName) == null)
            {
                return RedirectToLogin();
            }
            return View();
        }

        public IActionResult Students()
        {
            if (_session.GetSession<UserSession>(_sessionName) == null)
            {
                return RedirectToLogin();
            }
            return View();
        }

        public IActionResult Calendar()
        {
            if (_session.GetSession<UserSession>(_sessionName) == null)
            {
                return RedirectToLogin();
            }
            return View();
        }        

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Login(Login model, string returnUrl)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // This doesn't count login failures towards account lockout
        //    // To enable password failures to trigger account lockout, change to shouldLockout: true
        //    //var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
        //    //switch (result)
        //    //{
        //    //    case SignInStatus.Success:
        //    //        return RedirectToLocal(returnUrl);
        //    //    case SignInStatus.LockedOut:
        //    //        return View("Lockout");
        //    //    case SignInStatus.RequiresVerification:
        //    //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
        //    //    case SignInStatus.Failure:
        //    //    default:
        //    //        ModelState.AddModelError("", "Invalid login attempt.");
        //    //        return View(model);
        //    //}
        //    HttpClient client = new HttpClient();
        //    var content = new FormUrlEncodedContent(new[] {
        //        new KeyValuePair<string, string>("NIK", model.NIK),
        //        new KeyValuePair<string, string>("Password", model.Password)
        //    });
        //    string contentType = "application/x-www-form-urlencoded";
        //    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(contentType));

        //    HttpResponseMessage response = await client.PostAsync("http://localhost:5900/api/Login/validate_login", content);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var apiResultStream = await response.Content.ReadAsStringAsync();
        //        var returnApi = JsonConvert.DeserializeObject<ResultApi>(apiResultStream);
        //        if (returnApi.Status == true)
        //        {
        //            returnUrl = "Index/Dashboard";
        //            return RedirectToLocal(returnUrl);
        //        }
        //        else
        //        {
        //            ViewBag.Message = String.Format("NIK atau Password tidak sesuai");
        //            return View();
        //        }
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}

        [HttpPost]
        public async Task<JsonResult> GetSchedule() {            
            try
            {
                HttpClient client = new HttpClient();

                string url = "http://localhost:5900/api/Scheduler/get_schedule";
                ResultApi<List<KelasProgram>> lKelasProgram = new ResultApi<List<KelasProgram>>();
                ResultApi<KelasProgram> result = new ResultApi<KelasProgram>();
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    lKelasProgram = JsonConvert.DeserializeObject<ResultApi<List<KelasProgram>>>(data);
                }

                if(lKelasProgram.Payload == null)
                {
                    lKelasProgram.Payload = new List<KelasProgram>();
                }
                //return Json(new { Draw = "1", iTotalRecords = lKelasProgram.Payload.Count, iTotalDisplayRecords = lKelasProgram.Payload.Count, Data = lKelasProgram.Payload });
                return Json(new { aaData = lKelasProgram.Payload, iTotalRecords = lKelasProgram.Payload.Count, iTotalDisplayRecords = lKelasProgram.Payload.Count }, new JsonSerializerSettings() { Formatting = Formatting.Indented });
            }
            catch (Exception ex) {
                throw ex;
            }
            
        }

        [HttpPost]
        public async Task<JsonResult> Schedule(KelasProgram param)
        {
            try
            {
                #region HTTP Request
                HttpClient client = new HttpClient();
                var content = new FormUrlEncodedContent(param.ToKeyValue());
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                content.Headers.ContentType.CharSet = "UTF-8";
                ResultApi<KelasProgram> resultKelasProgram = new ResultApi<KelasProgram>();
                HttpResponseMessage response = await client.PostAsync("http://localhost:5900/api/Scheduler/create_kelas_program", content);

                if (response.IsSuccessStatusCode)
                {
                    string stringResponse = await response.Content.ReadAsStringAsync();
                    resultKelasProgram = JsonConvert.DeserializeObject<ResultApi<KelasProgram>>(stringResponse);

                    if(resultKelasProgram.Status == false)
                    {
                        return Json(new { message = resultKelasProgram.Message, status = resultKelasProgram.Status });
                    }
                    else if(resultKelasProgram.Status == true)
                    {
                        return Json(new { data = resultKelasProgram.Payload, status = resultKelasProgram.Status });
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, status = false });
            }

            return Json(new { message = "Fail", status = false });

        }

        private ActionResult RedirectToLogin()
        {            
            return RedirectToAction("Index", "Login");
        }

    }

}