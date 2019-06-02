using HouseholdManagementAPIConsumer.Models.HelperClasses;
using HouseholdManagementAPIConsumer.Models.HouseholdMemberInvites;
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
    public class HouseholdApiHouseholdMembersController : Controller
    {
        private BasicApiConnectionHelpers BasicApiConnectionHelper { get; set; }

        public HouseholdApiHouseholdMembersController()
        {
            BasicApiConnectionHelper = new BasicApiConnectionHelpers();
        }

        // GET: HouseholdApiHouseholdMembers
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult _ViewHouseholdMembers(string householdId)
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

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(_ViewHouseholdMembers) && p.Method == "Get").Url + $"{householdId}";

            var token = cookie.Value;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;

            List<HouseholdMember> result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Read the response
                var data = response.Content.ReadAsStringAsync().Result;

                //Convert the data back into an object
                result = JsonConvert.DeserializeObject<List<HouseholdMember>>(data);
                
                return View(result);
            }
            else
            {
                ViewBag.HasErrored = true;
                return View();
            }
        }

        [HttpGet]        
        public ActionResult _InviteHouseholdMember(string householdId)
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

            ViewBag.HouseholdId = householdId;
            return View();
        }


        [HttpPost]
        public ActionResult _InviteHouseholdMember(string householdId,  InviteHouseholdMemberBindingModel formdata)
        {
            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (householdId == null || formdata == null || !ModelState.IsValid)
            {
                return View(formdata);
            }            

            var token = cookie.Value;

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(_InviteHouseholdMember) && p.Method == "Post").Url + $"{householdId}" + "/InviteMember";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Email", formdata.Email));            
            var encodedValues = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedValues).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {                                
                return RedirectToAction("ViewHousehold", "HouseholdApiHouseholds", new { householdId = householdId });
            }
            else
            {
                ViewBag.HasErrored = true;
                return View(formdata);
            }            
        }

        [HttpGet]
        public ActionResult JoinHousehold(string inviteId, string householdName)
        {
            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (inviteId == null || householdName == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new JoinHouseholdBindingModel
            {
                InviteId = inviteId,
                HouseholdName = householdName
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult JoinHousehold(JoinHouseholdBindingModel formdata)
        {
            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (formdata == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var token = cookie.Value;

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(JoinHousehold) && p.Method == "Put").Url + $"{formdata.InviteId}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.PutAsync(url, null).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("ViewHouseholds", "HouseholdApiHouseholds");
            }
            else
            {
                ViewBag.HasErrored = true;
                return View(formdata);
            }
        }

        [HttpPost]
        public ActionResult LeaveHousehold(string householdId)
        {
            var cookie = Request.Cookies["LoginCookieForHouseholdApi"];
            if (cookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (householdId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var token = cookie.Value;

            var url = BasicApiConnectionHelper.AllUrls.FirstOrDefault(p => p.Name == nameof(LeaveHousehold) && p.Method == "Put").Url + $"/{householdId}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.PutAsync(url, null).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("ViewHouseholds", "HouseholdApiHouseholds");
            }
            else
            {
                ViewBag.HasErrored = true;
                return RedirectToAction("ViewHouseholds", "HouseholdApiHouseholds");
            }
        }
    }
}