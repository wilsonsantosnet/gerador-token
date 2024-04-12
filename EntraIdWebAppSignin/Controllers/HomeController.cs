using Azure.Core;
using EntraIdWebAppSignin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using System.Diagnostics;

namespace EntraIdWebAppSignin.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, ITokenAcquisition tokenAcquisition, IConfiguration config)
        {
            _logger = logger;
            _tokenAcquisition = tokenAcquisition;
            _configuration = config;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var accessToken = await getAccessToken();
                ViewBag.AccessToken = accessToken;
            }
            catch (Exception)
            {
                ClearCookies();
                return RedirectToAction("Index");
            }
            return View();

        }

       
        private async Task<string> getAccessToken()
        {
            var scopes = _configuration["Roles:Scope"]?.Split(" ");
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new string[] { scopes?.LastOrDefault() ?? string.Empty });
            return accessToken;
        }

        public IActionResult Logout()
        {
            ClearCookies();
            var api = _configuration["Roles:Api"];
            var absoluteUri = new Uri(_configuration["Roles:Api"] ?? "").AbsoluteUri;
            var redirect_uri = $"{absoluteUri}Home/LogoutCallback";
            var urllogout = $"https://login.microsoftonline.com/common/oauth2/v2.0/logout?post_logout_redirect_uri={redirect_uri}";
            return Redirect(urllogout);
        }

        public IActionResult LogoutCallback()
        {
            ClearCookies();
            var api = _configuration["Roles:Api"];
            var absoluteUri = new Uri(_configuration["Roles:Api"] ?? "").AbsoluteUri;
            return Redirect(absoluteUri);
        }

        private void ClearCookies()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
