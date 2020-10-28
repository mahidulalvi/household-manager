using HouseholdManager.Models.BankAccounts;
using HouseholdManager.Models.ErrorModels;
using HouseholdManager.Models.HelperClasses;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;

namespace HouseholdManager.Controllers
{
    public class HouseholdApiBankAccountsController : Controller
    {        
        private BasicApiConnectionHelpers BasicApiConnectionHelper { get; set; }
        private string UrlToVerifyUserAsOwner { get; set; }

        public HouseholdApiBankAccountsController()
        {
            BasicApiConnectionHelper = new BasicApiConnectionHelpers();
            UrlToVerifyUserAsOwner = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == "VerifyUserAsHouseholdOwner" && p.Method == "Get").Url;
        }




        [HttpGet]
        public ActionResult ViewBankAccounts(string householdId)
        {
            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(ViewBankAccounts) && p.Method == "Get").Url + householdId;

            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;

            List<BankAccountViewModel> result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                //Convert the data back into an object
                result = JsonConvert.DeserializeObject<List<BankAccountViewModel>>(data);

                return View(result);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                //Display generic error message
                return View("Error");
            }
            else
            {
                ViewBag.HasErrored = true;
                return View();
            }

        }


        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult ViewBankAccount(string householdId, string bankAccountId)
        {
            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (householdId == null || bankAccountId == null)
            {
                return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = householdId });
            }

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(ViewBankAccount) && p.Method == "Get").Url + $"{householdId}/{bankAccountId}";

            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;

            BankAccountViewModel result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                //Convert the data back into an object
                result = JsonConvert.DeserializeObject<BankAccountViewModel>(data);


                var urlToVerifyUserAsOwner = UrlToVerifyUserAsOwner + householdId;
                var verificationResponse = httpClient.GetAsync(urlToVerifyUserAsOwner).Result;

                if (verificationResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ViewBag.UserIsOwner = true;
                }
                else
                {
                    ViewBag.UserIsOwner = false;
                }

                return View(result);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                //Display generic error message
                return View("Error");
            }
            else
            {
                ViewBag.HasErrored = true;
                return View();
            }
        }


        [HttpGet]
        public ActionResult CreateBankAccount(string householdId)
        {
            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (householdId == null)
            {
                RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = householdId });
            }

            return View();
        }


        [HttpPost]
        public ActionResult CreateBankAccount(string householdId, CreateBankAccountViewModel formdata)
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
            if (formdata == null || !ModelState.IsValid)
            {
                return View(formdata);
            }

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(CreateBankAccount) && p.Method == "Post").Url + householdId;

            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Name", formdata.Name));
            parameters.Add(new KeyValuePair<string, string>("Description", formdata.Description));
            var encodedValues = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedValues).Result;

            BankAccountViewModel result;

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                result = JsonConvert.DeserializeObject<BankAccountViewModel>(data);

                //Convert the data back into an object                

                return RedirectToAction("ViewBankAccount", "HouseholdApiBankAccounts", new { householdId = result.HouseholdId, bankAccountId = result.Id });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;

                var errors = JsonConvert.DeserializeObject<ApiError>(data);

                foreach (var key in errors.ModelState)
                {
                    foreach (var error in key.Value)
                    {
                        ModelState.AddModelError(key.Key, error);
                    }
                }

                return View(formdata);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                //Display generic error message
                return View("Error");
            }
            else
            {
                ViewBag.HasErrored = true;
                return View();
            }
        }


        [HttpGet]
        public ActionResult EditBankAccount(string householdId, string bankAccountId)
        {

            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (householdId == null || bankAccountId == null)
            {
                return RedirectToAction("ViewHouseholds", "HouseholdApiHouseholds");
            }


            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(EditBankAccount) && p.Method == "Get").Url + $"{householdId}/{bankAccountId}";


            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;

            EditBankAccountViewModel result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                //Convert the data back into an object
                result = JsonConvert.DeserializeObject<EditBankAccountViewModel>(data);

                if (result == null)
                {
                    ViewBag.NoCategory = true;
                    return View();
                }

                return View(result);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                //Display generic error message
                return View("Error");
            }
            else
            {
                ViewBag.HasErrored = true;
                return View();
            }
        }


        [HttpPost]
        public ActionResult EditBankAccount(string householdId, string bankAccountId, EditBankAccountViewModel formdata)
        {
            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(EditBankAccount) && p.Method == "Put").Url + $"{bankAccountId}";

            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if(householdId == null)
            {
                return RedirectToAction("ViewHouseholds", "HouseholdApiHouseholds");
            }
            if (bankAccountId == null)
            {
                return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = householdId });
            }
            if(formdata == null || !ModelState.IsValid)
            {
                return View(formdata);
            }

            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Name", formdata.Name));
            parameters.Add(new KeyValuePair<string, string>("Description", formdata.Description));
            var encodedValues = new FormUrlEncodedContent(parameters);

            var response = httpClient.PutAsync(url, encodedValues).Result;

            BankAccountViewModel result;

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                //Convert the data back into an object
                result = JsonConvert.DeserializeObject<BankAccountViewModel>(data);

                return RedirectToAction("ViewBankAccount", "HouseholdApiBankAccounts", new { householdId = result.HouseholdId, bankAccountId = result.Id });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;

                var errors = JsonConvert.DeserializeObject<ApiError>(data);

                foreach (var key in errors.ModelState)
                {
                    foreach (var error in key.Value)
                    {
                        ModelState.AddModelError(key.Key, error);
                    }
                }

                return View(formdata);
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
        public ActionResult UpdateBankAccountBalance(string bankAccountId, string householdId)
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
            if (bankAccountId == null)
            {
                return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = householdId });
            }


            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(UpdateBankAccountBalance) && p.Method == "Put").Url + $"{bankAccountId}/CalculateAccountBalance";

            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");            

            var response = httpClient.PutAsync(url, null).Result;

            BankAccountViewModel result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                //Convert the data back into an object
                result = JsonConvert.DeserializeObject<BankAccountViewModel>(data);

                return RedirectToAction("ViewBankAccount", "HouseholdApiBankAccounts", new { householdId = result.HouseholdId, bankAccountId = result.Id });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                //Display generic error message
                return View("Error");
            }
            else
            {
                ViewBag.HasErrored = true;
                return RedirectToAction("ViewBankAccount", "HouseholdApiBankAccounts", new { householdId = householdId, bankAccountId = bankAccountId });
            }
        }


        [HttpPost]
        public ActionResult DeleteBankAccount(string bankAccountId, string householdId)
        {
            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(DeleteBankAccount) && p.Method == "Delete").Url + $"{bankAccountId}";

            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (bankAccountId == null)
            {
                return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = householdId });
            }
            if (householdId == null)
            {
                return RedirectToAction("ViewHouseholds", "HouseholdApiHouseholds");
            }


            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.DeleteAsync(url).Result;


            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = householdId });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TempData["NoSuchHousehold"] = true;

                return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = householdId });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                //Display generic error message
                return View("Error");
            }
            else
            {
                ViewBag.HasErrored = true;
                return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = householdId });
            }
        }
    }
}