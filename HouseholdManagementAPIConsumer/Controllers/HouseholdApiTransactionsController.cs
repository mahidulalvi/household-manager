using HouseholdManagementAPIConsumer.Models.HelperClasses;
using HouseholdManagementAPIConsumer.Models.Transactions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace HouseholdManagementAPIConsumer.Controllers
{
    public class HouseholdApiTransactionsController : Controller
    {
        // GET: HouseholdApiTransactions
        public ActionResult Index()
        {
            return View();
        }

        private BasicApiConnectionHelpers BasicApiConnectionHelper { get; set; }

        public HouseholdApiTransactionsController()
        {
            BasicApiConnectionHelper = new BasicApiConnectionHelpers();
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

                return View(result);
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

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(ViewTransaction) && p.Method == "Get").Url + bankAccountId;

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

                return View(result);
            }
            else
            {
                ViewBag.HasErrored = true;
                return View();
            }
        }













        //[HttpGet]
        //public ActionResult CreateBankAccount(string householdId)
        //{
        //    var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
        //    if (cookie == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    if (householdId == null)
        //    {
        //        RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = householdId });
        //    }



        //    return View();
        //}









        //[HttpPost]
        //public ActionResult CreateBankAccount(string householdId, CreateBankAccountViewModel formdata)
        //{
        //    var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
        //    if (cookie == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }


        //    if (formdata == null || !ModelState.IsValid || householdId == null)
        //    {
        //        return View(formdata);
        //    }

        //    var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(CreateBankAccount) && p.Method == "Post").Url + householdId;

        //    var token = cookie.Value;

        //    var httpClient = new HttpClient();

        //    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        //    var parameters = new List<KeyValuePair<string, string>>();
        //    parameters.Add(new KeyValuePair<string, string>("Name", formdata.Name));
        //    parameters.Add(new KeyValuePair<string, string>("Description", formdata.Description));
        //    var encodedValues = new FormUrlEncodedContent(parameters);

        //    var response = httpClient.PostAsync(url, encodedValues).Result;

        //    BankAccountViewModel result;

        //    if (response.StatusCode == System.Net.HttpStatusCode.Created)
        //    {
        //        //Read the response
        //        var data = response.Content.ReadAsStringAsync().Result;

        //        result = JsonConvert.DeserializeObject<BankAccountViewModel>(data);

        //        //Convert the data back into an object                

        //        return View("ViewBankAccount", result);
        //    }
        //    else
        //    {
        //        ViewBag.HasErrored = true;
        //        return View();
        //    }
        //}






        //[HttpGet]
        //public ActionResult EditBankAccount(string householdId, string bankAccountId)
        //{

        //    var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
        //    if (cookie == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    if (householdId == null || bankAccountId == null)
        //    {
        //        return RedirectToAction("ViewHouseholds", "HouseholdApiHouseholds");
        //    }


        //    var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(EditBankAccount) && p.Method == "Get").Url + $"{householdId}/{bankAccountId}";


        //    var token = cookie.Value;

        //    var httpClient = new HttpClient();

        //    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        //    var response = httpClient.GetAsync(url).Result;

        //    EditBankAccountViewModel result;

        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        //Read the response
        //        var data = response.Content.ReadAsStringAsync().Result;

        //        //Convert the data back into an object
        //        result = JsonConvert.DeserializeObject<EditBankAccountViewModel>(data);

        //        if (result == null)
        //        {
        //            ViewBag.NoCategory = true;
        //            return View();
        //        }

        //        return View(result);
        //    }
        //    else
        //    {
        //        ViewBag.HasErrored = true;
        //        return View();
        //    }
        //}









        //[HttpPost]
        //public ActionResult EditBankAccount(string bankAccountId, EditBankAccountViewModel formdata)
        //{
        //    var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(EditBankAccount) && p.Method == "Put").Url + $"{bankAccountId}";

        //    var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
        //    if (cookie == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    if (bankAccountId == null || formdata == null)
        //    {
        //        return RedirectToAction("ViewHouseholds", "HouseholdApiHouseholds");
        //    }

        //    var token = cookie.Value;

        //    var httpClient = new HttpClient();

        //    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        //    var parameters = new List<KeyValuePair<string, string>>();
        //    parameters.Add(new KeyValuePair<string, string>("Name", formdata.Name));
        //    parameters.Add(new KeyValuePair<string, string>("Description", formdata.Description));
        //    var encodedValues = new FormUrlEncodedContent(parameters);

        //    var response = httpClient.PutAsync(url, encodedValues).Result;

        //    BankAccountViewModel result;

        //    if (response.StatusCode == System.Net.HttpStatusCode.Created)
        //    {
        //        //Read the response
        //        var data = response.Content.ReadAsStringAsync().Result;

        //        //Convert the data back into an object
        //        result = JsonConvert.DeserializeObject<BankAccountViewModel>(data);

        //        return View("ViewBankAccount", result);
        //    }
        //    else
        //    {
        //        ViewBag.HasErrored = true;
        //        return View(formdata);
        //    }
        //}









        //[HttpPost]
        //public ActionResult UpdateBankAccountBalance(string bankAccountId, string householdId)
        //{

        //    var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
        //    if (cookie == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }


        //    if (householdId == null)
        //    {
        //        return RedirectToAction("ViewHouseholds", "HouseholdApiHouseholds");
        //    }
        //    if (bankAccountId == null)
        //    {
        //        return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = householdId });
        //    }


        //    var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(UpdateBankAccountBalance) && p.Method == "Put").Url + $"{bankAccountId}/CalculateAccountBalance";

        //    var token = cookie.Value;

        //    var httpClient = new HttpClient();

        //    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        //    var response = httpClient.PutAsync(url, null).Result;

        //    BankAccountViewModel result;

        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        //Read the response
        //        var data = response.Content.ReadAsStringAsync().Result;

        //        //Convert the data back into an object
        //        result = JsonConvert.DeserializeObject<BankAccountViewModel>(data);

        //        return View("ViewBankAccount", result);
        //    }
        //    else
        //    {
        //        ViewBag.HasErrored = true;
        //        return RedirectToAction("ViewBankAccount", "HouseholdApiBankAccounts", new { householdId = householdId, bankAccountId = bankAccountId });
        //    }
        //}







        //[HttpPost]
        //public ActionResult DeleteBankAccount(string bankAccountId, string householdId)
        //{
        //    var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(DeleteBankAccount) && p.Method == "Delete").Url + $"{bankAccountId}";

        //    var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
        //    if (cookie == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    if (bankAccountId == null)
        //    {
        //        return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = householdId });
        //    }
        //    if (householdId == null)
        //    {
        //        return RedirectToAction("ViewHouseholds", "HouseholdApiHouseholds");
        //    }


        //    var token = cookie.Value;

        //    var httpClient = new HttpClient();

        //    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        //    var response = httpClient.DeleteAsync(url).Result;


        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = householdId });
        //    }
        //    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        //    {
        //        TempData["NoSuchHousehold"] = true;

        //        return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = householdId });
        //    }
        //    else
        //    {
        //        ViewBag.HasErrored = true;
        //        return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = householdId });
        //    }
        //}
    }
}