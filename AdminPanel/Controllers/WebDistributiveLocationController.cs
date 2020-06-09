using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AdminPanel.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class WebDistributiveLocationController : Controller
    {
        private readonly IConfiguration _configuration;

        public WebDistributiveLocationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("GetDistributiveInfo")]
        public IActionResult GetDistributiveInfo(string OS, string Arch, string Version)
        {
            long size = 0;
            string url = "";

            return Json(new { root = new { size, url } });
        }
    }
}
