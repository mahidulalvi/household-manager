using HouseholdManager.Models.Categories;
using HouseholdManager.Models.ErrorModels;
using HouseholdManager.Models.HelperClasses;
using HouseholdManager.Models.Transactions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;

namespace HouseholdManager.Controllers
{
    public class HouseholdApiTransactionsController : Controller
    {
        private BasicApiConnectionHelpers BasicApiConnectionHelper { get; set; }
        private string UrlToVerifyUserAsOwner { get; set; }
        private string UrlToVerifyUserAsHouseholdOwner { get; set; }

        public HouseholdApiTransactionsController()
        {
            BasicApiConnectionHelper = new BasicApiConnectionHelpers();
            UrlToVerifyUserAsOwner = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == "VerifyUserAsTransactionOwner" && p.Method == "Get").Url;
            UrlToVerifyUserAsHouseholdOwner = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == "VerifyUserAsHouseholdOwner" && p.Method == "Get").Url;
        }


        [HttpGet]
        public ActionResult ViewTransactions(string householdId, string bankAccountId)
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
            if(bankAccountId == null)
            {
                return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = householdId });
            }

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(ViewTransactions) && p.Method == "Get").Url + $"{householdId}/{bankAccountId}";

            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;

            List<TransactionViewModel> result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                //Convert the data back into an object
                result = JsonConvert.DeserializeObject<List<TransactionViewModel>>(data);

                ViewBag.HouseholdId = householdId;
                ViewBag.BankAccountId = bankAccountId;


                var urlToVerifyUserAsHouseholdOwner = UrlToVerifyUserAsHouseholdOwner + householdId;
                var verificationResponse = httpClient.GetAsync(urlToVerifyUserAsHouseholdOwner).Result;

                if (verificationResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ViewBag.UserIsHouseholdOwner = true;
                }
                else
                {
                    ViewBag.UserIsHouseholdOwner = false;
                }

                return View(result);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                //Display generic error message
                return View("Error");
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;

                var error = JsonConvert.DeserializeObject<ErrorModelCommons>(data);

                TempData["BadRequestErrorMessage"] = error.Message;

                return View();
            }
            else
            {
                ViewBag.HasErrored = true;
                return View();
            }

        }


        [HttpGet]
        public ActionResult ViewTransaction(string transactionId, string bankAccountId, string householdId)
        {
            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (householdId != null && bankAccountId == null)
            {
                return RedirectToAction("ViewBankAccount", "HouseholdApiBankAccounts", new { householdId = householdId, bankAccountId = bankAccountId });
            }
            else if(bankAccountId == null)
            {
                return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = householdId });
            }
            else if(householdId == null)
            {
                return RedirectToAction("ViewHouseholds", "HouseholdApiHouseholds");
            }

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(ViewTransaction) && p.Method == "Get").Url + transactionId;

            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;

            TransactionViewModel result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                //Convert the data back into an object
                result = JsonConvert.DeserializeObject<TransactionViewModel>(data);

                ViewBag.HouseholdId = householdId;

                var urlToVerifyUserAsOwner = UrlToVerifyUserAsOwner + transactionId;
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
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;

                var error = JsonConvert.DeserializeObject<ErrorModelCommons>(data);

                TempData["BadRequestErrorMessage"] = error.Message;

                return View();
            }
            else
            {
                ViewBag.HasErrored = true;
                return View();
            }
        }


        [HttpGet]
        public SelectList GetCategoryIds(string householdId)
        {
            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return null;
            }

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == "ViewCategories" && p.Method == "Get").Url + householdId + "/Categories";

            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;

            List<HtmlSelectListClassForCategory> result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                //Convert the data back into an object
                result = JsonConvert.DeserializeObject<List<HtmlSelectListClassForCategory>>(data);

                if(result.Count() == 0)
                {
                    return null;
                }

                var selectedDisabledValue = new HtmlSelectListClassForCategory
                {
                    Id = "",
                    Name = "Select Category"
                };

                result.Insert(0, selectedDisabledValue);

                var resultAsSelectList = new SelectList(result, "Id", "Name");

                return resultAsSelectList;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                //Display generic error message
                return null;
            }
            else
            {
                return null;
            }
        }


        [HttpGet]
        public ActionResult CreateTransaction(string householdId, string bankAccountId)
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
            if(bankAccountId == null)
            {
                return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { bankAccountId = householdId });
            }


            ViewBag.ListBoxForCategoryIds = GetCategoryIds(householdId);

            if(ViewBag.ListBoxForCategoryIds == null)
            {
                ViewBag.NoCategories = true;
            }

            return View();
        }


        [HttpPost]
        public ActionResult CreateTransaction(string householdId, string bankAccountId, CreateTransactionViewModel formdata)
        {
            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }


            if(householdId == null)
            {
                return RedirectToAction("ViewHouseholds", "HouseholdApiHousholds");
            }
            if(bankAccountId == null)
            {
                return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = householdId });
            }
            if (formdata == null || !ModelState.IsValid)
            {
                ViewBag.ListBoxForCategoryIds = GetCategoryIds(householdId);
                return View(formdata);
            }

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(CreateTransaction) && p.Method == "Post").Url + $"{householdId}/{bankAccountId}";

            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Title", formdata.Title));
            parameters.Add(new KeyValuePair<string, string>("Description", formdata.Description));
            parameters.Add(new KeyValuePair<string, string>("Date", formdata.Date.ToString()));
            parameters.Add(new KeyValuePair<string, string>("CategoryId", formdata.CategoryId));
            parameters.Add(new KeyValuePair<string, string>("Amount", formdata.Amount.ToString()));
            var encodedValues = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedValues).Result;

            TransactionViewModel result;

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                result = JsonConvert.DeserializeObject<TransactionViewModel>(data);

                //Convert the data back into an object                

                return RedirectToAction("ViewTransaction", "HouseholdApiTransactions", new { transactionId = result.Id, bankAccountId = result.BankAccountId, householdId = householdId });
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

                ViewBag.ListBoxForCategoryIds = GetCategoryIds(householdId);                

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

                ViewBag.ListBoxForCategoryIds = GetCategoryIds(householdId);

                return View();
            }
        }


        //Not implemented, but kept for future usage and development
        [HttpPost]
        public ActionResult CreateTransactionFromHousehold(string householdId, AlternativeCreateTransactionViewModel formdata)
        {
            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }


            if (formdata == null || !ModelState.IsValid || householdId == null)
            {
                return View(formdata);
            }

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(CreateTransactionFromHousehold) && p.Method == "Post").Url + householdId;

            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Title", formdata.Title));
            parameters.Add(new KeyValuePair<string, string>("Description", formdata.Description));
            parameters.Add(new KeyValuePair<string, string>("Date", formdata.Date.ToString()));
            parameters.Add(new KeyValuePair<string, string>("CategoryId", formdata.CategoryId));
            parameters.Add(new KeyValuePair<string, string>("BankAccountId", formdata.BankAccountId));
            parameters.Add(new KeyValuePair<string, string>("Amount", formdata.Amount.ToString()));
            var encodedValues = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedValues).Result;

            TransactionViewModel result;

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                result = JsonConvert.DeserializeObject<TransactionViewModel>(data);

                //Convert the data back into an object                

                return RedirectToAction("ViewTransaction", "HouseholdApiTransactions", new { transactionId = result.Id, bankAccountId = result.BankAccountId, householdId = householdId });
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
        public ActionResult EditTransaction(string transactionId, string householdId)
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
            if(transactionId == null)
            {
                return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = householdId });
            }


            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(EditTransaction) && p.Method == "Get").Url + transactionId;


            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;

            EditTransactionViewModel result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                //Convert the data back into an object
                result = JsonConvert.DeserializeObject<EditTransactionViewModel>(data);

                if (result == null)
                {
                    ViewBag.NoTransaction = true;
                    return View();
                }

                ViewBag.ListBoxForCategoryIds = GetCategoryIds(householdId);
                if (ViewBag.ListBoxForCategoryIds == null)
                {
                    ViewBag.NoCategories = true;
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

                ViewBag.ListBoxForCategoryIds = GetCategoryIds(householdId);
                if (ViewBag.ListBoxForCategoryIds == null)
                {
                    ViewBag.NoCategories = true;
                }

                return View();
            }
        }


        [HttpPost]
        public ActionResult EditTransaction(string transactionId, string householdId, EditTransactionViewModel formdata)
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
            if (transactionId == null)
            {
                return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = householdId });
            }
            if(formdata == null || !ModelState.IsValid)
            {
                ViewBag.ListBoxForCategoryIds = GetCategoryIds(householdId);
                if (ViewBag.ListBoxForCategoryIds == null)
                {
                    ViewBag.NoCategories = true;
                }
                return View(formdata);
            }

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(EditTransaction) && p.Method == "Put").Url + transactionId;

            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Title", formdata.Title));
            parameters.Add(new KeyValuePair<string, string>("Description", formdata.Description));
            parameters.Add(new KeyValuePair<string, string>("Date", formdata.Date.ToString()));
            parameters.Add(new KeyValuePair<string, string>("CategoryId", formdata.CategoryId));
            parameters.Add(new KeyValuePair<string, string>("Amount", formdata.Amount.ToString()));
            var encodedValues = new FormUrlEncodedContent(parameters);

            var response = httpClient.PutAsync(url, encodedValues).Result;

            TransactionViewModel result;

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                //Convert the data back into an object
                result = JsonConvert.DeserializeObject<TransactionViewModel>(data);

                return RedirectToAction("ViewTransaction", "HouseholdApiTransactions", new { transactionId = result.Id, bankAccountId = result.BankAccountId, householdId = householdId });
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


                ViewBag.ListBoxForCategoryIds = GetCategoryIds(householdId);
                if (ViewBag.ListBoxForCategoryIds == null)
                {
                    ViewBag.NoCategories = true;
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
                ViewBag.ListBoxForCategoryIds = GetCategoryIds(householdId);
                if (ViewBag.ListBoxForCategoryIds == null)
                {
                    ViewBag.NoCategories = true;
                }
                return View(formdata);
            }
        }


        [HttpPost]
        public ActionResult ToggleVoidTransaction(string transactionId, string bankAccountId, string householdId)
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
            if (bankAccountId == null)
            {
                return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = householdId });
            }
            if(transactionId == null)
            {
                return RedirectToAction("ViewBankAccount", "HouseholdApiBankAccounts", new { householdId = householdId, bankAccountId = bankAccountId });
            }


            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(ToggleVoidTransaction) && p.Method == "Put").Url + transactionId;

            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.PutAsync(url, null).Result;

            TransactionViewModel result;

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                //Convert the data back into an object
                result = JsonConvert.DeserializeObject<TransactionViewModel>(data);

                return RedirectToAction("ViewBankAccount", "HouseholdApiBankAccounts", new { householdId = householdId, bankAccountId = result.BankAccountId });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                //Display generic error message
                return View("Error");
            }            
            else
            {
                TempData["HasErrored"] = true;
                return RedirectToAction("ViewTransaction", "HouseholdApiTransactions", new { transactionId = transactionId, bankAccountId = bankAccountId, householdId = householdId });
            }
        }


        [HttpPost]
        public ActionResult DeleteTransaction(string transactionId, string bankAccountId, string householdId)
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
            if (bankAccountId == null)
            {
                return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = householdId });
            }
            if(transactionId == null)
            {
                return RedirectToAction("ViewBankAccount", "HouseholdApiBankAccounts", new { householdId = householdId, bankAccountId = bankAccountId });
            }


            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(DeleteTransaction) && p.Method == "Delete").Url + $"{transactionId}";

            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.DeleteAsync(url).Result;


            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("ViewBankAccount", "HouseholdApiBankAccounts", new { householdId = householdId, bankAccountId = bankAccountId });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TempData["NoSuchTransactionAndDeleteFailed"] = true;

                return RedirectToAction("ViewBankAccount", "HouseholdApiBankAccounts", new { householdId = householdId, bankAccountId = bankAccountId });
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
    }
}