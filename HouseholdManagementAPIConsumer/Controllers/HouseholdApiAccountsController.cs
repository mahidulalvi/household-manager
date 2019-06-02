using HouseholdManagementAPIConsumer.Models.AccountDomain;
using HouseholdManagementAPIConsumer.Models.ErrorModels;
using HouseholdManagementAPIConsumer.Models.HelperClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
//using static HouseholdManagementAPIConsumer.Models.AccountDomain.AccountBindingModel;

namespace HouseholdManagementAPIConsumer.Controllers
{
    public class HouseholdApiAccountsController : Controller
    {
        public BasicApiConnectionHelpers BasicApiConnectionHelper { get; set; }

        public HouseholdApiAccountsController()
        {
            BasicApiConnectionHelper = new BasicApiConnectionHelpers();
        }

        // GET: HouseholdApiAccounts
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult RegisterApiConsumer()
        {
            return View();
        }


        [HttpPost]
        public ActionResult RegisterApiConsumer(RegisterBindingModel formdata)
        {
            if (formdata == null || !ModelState.IsValid)
            {
                return View(formdata);
            }

            var urlObject = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(RegisterApiConsumer) && p.Method == "Post");
            var url = urlObject.Url;

            var httpClient = new HttpClient();

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Email", formdata.Email));
            parameters.Add(new KeyValuePair<string, string>("Password", formdata.Password));
            parameters.Add(new KeyValuePair<string, string>("ConfirmPassword", formdata.ConfirmPassword));

            var encodedValues = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedValues).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                //Display generic error message
                return View("Error");
            }
            else
            {
                ViewBag.HasErrored = true;
                return View(formdata);
            }


        }


        [HttpGet]
        [Route("ConfirmEmail")]
        public ActionResult ConfirmEmail(string email, string confirmEmailToken)
        {
            if (confirmEmailToken == null || email == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new ConfirmEmailBindingModel
            {
                Email = email,
                ConfirmEmailToken = confirmEmailToken
            };

            return View(model);
        }

        [HttpPost]
        [Route("ConfirmEmail")]
        public ActionResult ConfirmEmail(ConfirmEmailBindingModel formdata)
        {
            if (formdata == null || !ModelState.IsValid)
            {
                return View(formdata);
            }

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(ConfirmEmail) && p.Method == "Post").Url;

            var httpClient = new HttpClient();

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Email", formdata.Email));
            parameters.Add(new KeyValuePair<string, string>("ConfirmEmailToken", formdata.ConfirmEmailToken));

            var encodedValues = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedValues).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.HasErrored = true;
                return View(formdata);
            }
        }


        [HttpGet]
        public ActionResult LoginApiConsumer()
        {
            return View();
        }


        [HttpPost]
        public ActionResult LoginApiConsumer(LoginBindingModel formdata)
        {
            if (formdata == null || !ModelState.IsValid)
            {
                return View(formdata);
            }

            var urlObject = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(LoginApiConsumer) && p.Method == "Post");
            var url = urlObject.Url;

            var httpClient = new HttpClient();

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("UserName", formdata.UserName));
            parameters.Add(new KeyValuePair<string, string>("Password", formdata.Password));
            parameters.Add(new KeyValuePair<string, string>("grant_type", "password"));

            var encodedValues = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedValues).Result;


            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;

                var result = JsonConvert.DeserializeObject<LoginData>(data);

                var cookie = new HttpCookie("LoginCookieForHouseholdApi", result.AccessToken);

                Response.Cookies.Add(cookie);

                return RedirectToAction("Index", "Home");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                //Display generic error message
                return View("Error");
            }
            else
            {
                ViewBag.HasErrored = true;
                return View(formdata);
            }
        }



        [HttpPost]
        public ActionResult LogoutApiConsumer(LoginBindingModel formdata)
        {
            var cookie = Request.Cookies.Get("LoginCookieForHouseholdAPI");

            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-10);
                Response.Cookies.Add(cookie);
            }            

            return RedirectToAction("Index", "Home");
        }





        [HttpGet]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordBindingModel formdata)
        {
            if (formdata == null || !ModelState.IsValid)
            {
                return View(formdata);
            }

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(ForgotPassword) && p.Method == "Post").Url;            

            var httpClient = new HttpClient();

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Email", formdata.Email));

            var encodedValues = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedValues).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return View("ForgotPasswordConfirmation");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                //Display generic error message
                return View("Error");
            }
            else
            {
                ViewBag.HasErrored = true;
                return View(formdata);
            }
        }


        [HttpGet]
        [Route("ForgotPasswordReset")]
        public ActionResult ForgotPasswordReset(string code)
        {
            if(code == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new ForgotPasswordResetViewModel
            {
                Code = code
            };

            return View(model);
        }


        [HttpPost]
        [Route("ForgotPasswordReset")]
        public ActionResult ForgotPasswordReset(ForgotPasswordResetViewModel formdata)
        {
            if (formdata == null || !ModelState.IsValid)
            {
                return View(formdata);
            }

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(ForgotPasswordReset) && p.Method == "Post").Url;

            var httpClient = new HttpClient();

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Email", formdata.Email));
            parameters.Add(new KeyValuePair<string, string>("Password", formdata.Password));
            parameters.Add(new KeyValuePair<string, string>("ConfirmPassword", formdata.ConfirmPassword));
            parameters.Add(new KeyValuePair<string, string>("Code", formdata.Code));

            var encodedValues = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedValues).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["PasswordResetSuccessful"] = true;
                return RedirectToAction("Index", "Home");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                //Display generic error message
                return View("Error");
            }
            else
            {
                ViewBag.HasErrored = true;
                ViewBag.ResponseMessage = response.Content;
                return View(formdata);
            }
        }

    }
}