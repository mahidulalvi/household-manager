using HouseholdManagementAPIConsumer.Models.Categories;
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
    public class HouseholdApiCategoriesController : Controller
    {
        private BasicApiConnectionHelpers BasicApiConnectionHelper { get; set; }

        public HouseholdApiCategoriesController()
        {
            BasicApiConnectionHelper = new BasicApiConnectionHelpers();
        }

        [HttpGet]
        public ActionResult ViewCategories(string householdId)
        {
            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(ViewCategories) && p.Method == "Get").Url + householdId + "/Categories";

            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;

            List<CategoryViewModel> result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                //Convert the data back into an object
                result = JsonConvert.DeserializeObject<List<CategoryViewModel>>(data);



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
        public ActionResult ViewCategory(string householdId, string categoryId)
        {
            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (householdId == null)
            {
                return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = householdId });
            }
            if(categoryId == null)
            {
                return RedirectToAction("ViewHouseholds", "HouseholdApiHouseholds");
            }

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(ViewCategory) && p.Method == "Get").Url + $"{householdId}/Categories/{categoryId}";

            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;

            CategoryViewModel result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                //Convert the data back into an object
                result = JsonConvert.DeserializeObject<CategoryViewModel>(data);

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
        public ActionResult CreateCategory(string householdId)
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

            ViewBag.HouseholdId = householdId;

            return View();
        }









        [HttpPost]
        public ActionResult CreateCategory(string householdId, CreateCategoryViewModel formdata)
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
                ViewBag.HouseholdId = householdId;
                return View(formdata);
            }

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(CreateCategory) && p.Method == "Post").Url + householdId;

            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Name", formdata.Name));
            parameters.Add(new KeyValuePair<string, string>("Description", formdata.Description));
            var encodedValues = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedValues).Result;

            CategoryViewModel result;

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                result = JsonConvert.DeserializeObject<CategoryViewModel>(data);

                //Convert the data back into an object                

                return RedirectToAction("ViewCategory", "HouseholdApiCategories", new { householdId = result.HouseholdId, categoryId = result.Id });
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

                ViewBag.HouseholdId = householdId;

                return View(formdata);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                //Display generic error message
                return View("Error");
            }
            else
            {
                ViewBag.HouseholdId = householdId;
                ViewBag.HasErrored = true;
                return View();
            }
        }






        [HttpGet]
        public ActionResult EditCategory(string categoryId)
        {

            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (categoryId == null)
            {
                return RedirectToAction("ViewHouseholds", "HouseholdApiHouseholds");
            }


            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(EditCategory) && p.Method == "Get").Url + $"{categoryId}";


            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;

            EditCategoryViewModel result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                //Convert the data back into an object
                result = JsonConvert.DeserializeObject<EditCategoryViewModel>(data);

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
        public ActionResult EditCategory(string categoryId, EditCategoryViewModel formdata)
        {
            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (categoryId == null)
            {
                return RedirectToAction("ViewHouseholds", "HouseholdApiHouseholds");
            }

            if(formdata == null || !ModelState.IsValid)
            {
                return View(formdata);
            }

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(EditCategory) && p.Method == "Put").Url + $"{categoryId}";

            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Name", formdata.Name));
            parameters.Add(new KeyValuePair<string, string>("Description", formdata.Description));
            var encodedValues = new FormUrlEncodedContent(parameters);

            var response = httpClient.PutAsync(url, encodedValues).Result;

            CategoryViewModel result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                //Convert the data back into an object
                result = JsonConvert.DeserializeObject<CategoryViewModel>(data);

                return RedirectToAction("ViewCategory", "HouseholdApiCategories", new { householdId = result.HouseholdId, categoryId = result.Id });
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
        public ActionResult DeleteCategory(string categoryId, string householdId)
        {
            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(DeleteCategory) && p.Method == "Delete").Url + $"{categoryId}";

            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (categoryId == null)
            {
                return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = householdId });
            }
            if(householdId == null)
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