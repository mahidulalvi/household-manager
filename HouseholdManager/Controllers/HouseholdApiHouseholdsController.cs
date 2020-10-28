using HouseholdManager.Models.BankAccounts;
using HouseholdManager.Models.Categories;
using HouseholdManager.Models.ErrorModels;
using HouseholdManager.Models.HelperClasses;
using HouseholdManager.Models.Households;
using HouseholdManager.Models.Transactions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;

namespace HouseholdManager.Controllers
{
    public class HouseholdApiHouseholdsController : Controller
    {
        private BasicApiConnectionHelpers BasicApiConnectionHelper { get; set; }
        private string UrlToVerifyUserAsOwner { get; set; }

        public HouseholdApiHouseholdsController()
        {
            BasicApiConnectionHelper = new BasicApiConnectionHelpers();
            UrlToVerifyUserAsOwner = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == "VerifyUserAsHouseholdOwner" && p.Method == "Get").Url;
        }


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

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(ViewHousehold) && p.Method == "Get").Url + $"/{householdId}";            

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

                var urlToVerifyUserAsOwner = UrlToVerifyUserAsOwner + householdId;
                var verificationResponse = httpClient.GetAsync(urlToVerifyUserAsOwner).Result;      

                if(verificationResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ViewBag.UserIsOwner = true;
                }
                else
                {
                    ViewBag.UserIsOwner = false;
                }

                //Convert the data back into an object
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

                return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = result.Id });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;

                var error = JsonConvert.DeserializeObject<ErrorModelCommons>(data);

                ModelState.AddModelError("", error.Message);

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
        public ActionResult EditHousehold(string householdId)
        {
            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(EditHousehold) && p.Method == "Get").Url + $"{householdId}";

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
        public ActionResult EditHousehold(string householdId, EditHouseholdBindingModel formdata)
        {
            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(EditHousehold) && p.Method == "Put").Url + $"{householdId}";

            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (householdId == null)
            {
                return RedirectToAction("ViewHouseholds", "HouseholdApiHouseholds");
            }

            if (formdata == null || !ModelState.IsValid)
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

            HouseholdViewModel result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                //Convert the data back into an object
                result = JsonConvert.DeserializeObject<HouseholdViewModel>(data);                

                return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = result.Id });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;

                var error = JsonConvert.DeserializeObject<ErrorModelCommons>(data);

                ModelState.AddModelError("", error.Message);

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
        public ActionResult DeleteHousehold(string householdId)
        {
            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(DeleteHousehold) && p.Method == "Delete").Url + $"{householdId}";

            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
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

                return RedirectToAction("ViewHouseholds", "HouseholdApiHouseholds");
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
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


        [HttpGet]
        public ActionResult GetStats(string householdId)
        {
            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (householdId == null)
            {
                return RedirectToAction("ViewHouseholds", "HouseholdApiHouseholds");
            }

            var urlToGetBankAccounts = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == "GetBankAccountsForStats" && p.Method == "Get").Url + $"{householdId}";
            var urlToGetCategories = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == "GetCategoriesForStats" && p.Method == "Get").Url + $"{householdId}/Categories";

            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var responseForBankAccounts = httpClient.GetAsync(urlToGetBankAccounts).Result;
            var responseForCategories = httpClient.GetAsync(urlToGetCategories).Result;

            List<BankAccountViewModel> resultForBankAccounts;
            List<CategoryViewModel> resultForCategories;
            List<TransactionViewModel> resultForTransactions = new List<TransactionViewModel>();

            if (responseForBankAccounts.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Read the response
                var data = responseForBankAccounts.Content.ReadAsStringAsync().Result;

                //Convert the data back into an object
                resultForBankAccounts = JsonConvert.DeserializeObject<List<BankAccountViewModel>>(data);

                if (resultForBankAccounts == null)
                {
                    ViewBag.NoBankAccounts = true;
                    return View();
                }

                var categoriesData = responseForCategories.Content.ReadAsStringAsync().Result;
                resultForCategories = JsonConvert.DeserializeObject<List<CategoryViewModel>>(categoriesData);

                for(var i = 0; i < resultForBankAccounts.Count(); i++)
                {
                    var urlToGetTransactions = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == "GetTransactions" && p.Method == "Get").Url + $"{householdId}/{resultForBankAccounts[i].Id}";

                    var responseForTransactions = httpClient.GetAsync(urlToGetTransactions).Result;

                    var transactionsData = responseForTransactions.Content.ReadAsStringAsync().Result;

                    var transactionsList = JsonConvert.DeserializeObject<List<TransactionViewModel>>(transactionsData);

                    resultForTransactions.AddRange(transactionsList);
                }

                var model = new StatModel
                {
                    BankAccounts = resultForBankAccounts,
                    Categories = resultForCategories,
                    Transactions = resultForTransactions
                };


                return View(model);
            }
            else if (responseForBankAccounts.StatusCode == System.Net.HttpStatusCode.InternalServerError)
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
    }
}