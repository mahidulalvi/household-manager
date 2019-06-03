using HouseholdManagementAPIConsumer.Models.ErrorModels;
using HouseholdManagementAPIConsumer.Models.HelperClasses;
using HouseholdManagementAPIConsumer.Models.Households;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace HouseholdManagementAPIConsumer.Controllers
{
    public class HouseholdApiHouseholdsController : Controller
    {
        private BasicApiConnectionHelpers BasicApiConnectionHelper { get; set; }

        public HouseholdApiHouseholdsController()
        {
            BasicApiConnectionHelper = new BasicApiConnectionHelpers();
        }

        // GET: HouseholdApiHouseholds
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public ActionResult ViewHouseholds()
        {
            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(ViewHouseholds) && p.Method == "Get").Url;            

            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;

            List<HouseholdViewModel> result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                //Convert the data back into an object
                result = JsonConvert.DeserializeObject<List<HouseholdViewModel>>(data);

                return View(result);
            }
            else
            {
                ViewBag.HasErrored = true;
                return View();
            }

        }


        [HttpGet]
        public ActionResult ViewHousehold(string householdId)
        {
            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if(householdId == null)
            {
                return RedirectToAction("ViewHouseholds", "HouseholdApiHouseholds");
            }

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(ViewHousehold) && p.Method == "Get").Url += $"/{householdId}";            

            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;

            HouseholdViewModel result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                //Convert the data back into an object
                result = JsonConvert.DeserializeObject<HouseholdViewModel>(data);

                if(TempData["HasErrored"] != null)
                {
                    ViewBag.HasErrored = TempData["HasErrored"];
                }

                if (TempData["NoSuchUser"] != null)
                {
                    ViewBag.NoSuchUser = TempData["NoSuchUser"];
                }

                return View(result);
            }
            else
            {
                ViewBag.HasErrored = true;
                return View();
            }
        }        

        [HttpGet]
        public ActionResult CreateHousehold()
        {
            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult CreateHousehold(CreateHouseholdBindingModel formdata)
        {

            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }


            if (formdata == null || !ModelState.IsValid)
            {
                return View(formdata);
            }

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(CreateHousehold) && p.Method == "Post").Url;            

            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Name", formdata.Name));
            parameters.Add(new KeyValuePair<string, string>("Description", formdata.Description));
            var encodedValues = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedValues).Result;

            HouseholdViewModel result;

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                result = JsonConvert.DeserializeObject<HouseholdViewModel>(data);

                //Convert the data back into an object                

                return View("ViewHousehold", result);
            }
            else
            {
                ViewBag.HasErrored = true;
                return View();
            }
        }

        [HttpGet]
        public ActionResult EditHousehold(string householdId)
        {
            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(EditHousehold) && p.Method == "Get").Url += $"{householdId}";

            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if(householdId == null)
            {
                return RedirectToAction("ViewHouseholds", "HouseholdApiHouseholds");
            }

            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;

            EditHouseholdBindingModel result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                //Convert the data back into an object
                result = JsonConvert.DeserializeObject<EditHouseholdBindingModel>(data);

                if(result == null)
                {
                    ViewBag.NoHouseholds = true;
                    return View();
                }

                return View(result);
            }
            else
            {
                ViewBag.HasErrored = true;
                return View();
            }
        }


        [HttpPost]
        public ActionResult EditHousehold(string householdId, EditHouseholdBindingModel formdata)
        {
            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(EditHousehold) && p.Method == "Put").Url += $"{householdId}";

            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (householdId == null || formdata == null)
            {
                return RedirectToAction("ViewHouseholds", "HouseholdApiHouseholds");
            }

            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Name", formdata.Name));
            parameters.Add(new KeyValuePair<string, string>("Description", formdata.Description));
            var encodedValues = new FormUrlEncodedContent(parameters);

            var response = httpClient.PutAsync(url, encodedValues).Result;

            HouseholdViewModel result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                //Convert the data back into an object
                result = JsonConvert.DeserializeObject<HouseholdViewModel>(data);                

                return View("ViewHousehold", result);
            }
            else
            {
                ViewBag.HasErrored = true;
                return View(formdata);
            }
        }
    }
}