using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using GithubApi.Interfaces;
using log4net;

namespace GitHubBrowser.Controllers
{
    public class HomeController : Controller
    {
        ILog log = log4net.LogManager.GetLogger(typeof(HomeController));

        private readonly IGitHubUserService _userService;

        public HomeController(IGitHubUserService userService)
        {
            this._userService = userService;
        }

        public ActionResult Index()
        {
            log.Info("Home Index called");
            return View();
        }

        [HttpGet()]
        public async Task<ActionResult> GetUserInfo(string userName)
        {
            log.Info("Home GetUserInfo called");
            try
            {
                log.Debug($"GetUserInfo invoked - username - {userName}");
                var userInfo = await _userService.GetGitHubUserInformation(userName);
                log.Debug($"userinfo for user - {userName} - OK");
                var usersRepos = await _userService.GetUserRepoInformation(userInfo.ReposUrl);
                log.Debug($"userrepos for user {userName} - OK - {usersRepos.Count} repos found");
                userInfo.Repos = usersRepos;
                return PartialView("_GetUserInfo", userInfo);
            }
            catch (Exception ex)
            {
                log.Error("Error occured", ex);
                return PartialView("_Error");
            }
        }
       
    }
}