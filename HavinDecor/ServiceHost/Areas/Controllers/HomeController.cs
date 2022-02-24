using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace ServiceHost.Areas.Controllers
{
    public class HomeController : Controller
    {

        [HttpPost]
        public JsonResult GetPrice(string json)
        {
           
            dynamic jsondata = JsonConvert.DeserializeObject(json, typeof(object));

            //Get your variables here from AJAX call
            var id = Convert.ToInt32(jsondata["id"]);
            //Get the price based on your id from DB or API call
            var getMyPrice = GetPrice(id);
            return Json(new { status = "true", price = getMyPrice });
        }
    }
}