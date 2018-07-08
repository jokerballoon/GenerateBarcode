using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GenerateBarcode.Models;
using System.Net;
using System.Web;
using System.IO;
using Newtonsoft.Json;

namespace GenerateBarcode.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult GenerateBarcode(string csNo, string contractNo)
        {
            ViewData["Message"] = "Generate Barcode";
            ViewBag.csNo = csNo.Trim();
            ViewBag.contractNo = contractNo.Trim();


            return View();
        }

        public IActionResult GenerateSMS(string csNo, string contractNo)
        {
            ViewData["Message"] = "Generate SMS";
            if (csNo != null && contractNo != null)
            {
                string bitlyUsername = "o_16j05q1kts";
                string bitlyApiKey = "R_d4fb61799ed446b586e6161662585001";
                string longURL = "http://13.229.201.212/Home/GenerateBarcode?csNo=" + csNo.Trim() + "&contractNo=" + contractNo.Trim();
                //string longURL = "https://www.umayplus.com/Contact/Branchlocations";
                string shortURL = ShortenUrl(longURL.Trim(), bitlyUsername.Trim(), bitlyApiKey.Trim());

                dynamic stuff = JsonConvert.DeserializeObject(shortURL.Trim());
                
                ViewBag.shortURL = stuff.data.url;
                ViewBag.msg = "จ่ายบิลด้วย barcode ได้ที่  ";
            }
            return View();
        }

        enum Format
        {
            XML,
            JSON,
            TXT
        }

        enum Domain
        {
            BITLY,
            JMP
        }

        string ShortenUrl(string longURL, string username, string apiKey, Format format = Format.JSON, Domain domain = Domain.BITLY)
        {
            string _domain;
            string output;

            // Build the domain string depending on the selected domain type
            if (domain == Domain.BITLY)
                _domain = "bit.ly";
            else
                _domain = "j.mp";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                string.Format(@"http://api.bit.ly/v3/shorten?login={0}&apiKey={1}&longUrl={2}&format={3}&domain={4}",
                username, apiKey, HttpUtility.UrlEncode(longURL), format.ToString().ToLower(), _domain));

            //WebProxy myProxy = new WebProxy();
            //Uri newUri = new Uri("http://easybuyproxy:8080");
            //myProxy.Address = newUri;
            //myProxy.Credentials = new NetworkCredential("P3405019", "Joker015");
            //request.Proxy = myProxy;

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    output = reader.ReadToEnd();
                }
            }

            return output;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
