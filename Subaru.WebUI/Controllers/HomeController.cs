using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Graph=Microsoft.Graph;
using Subaru.Domain.AzureAd.Services;
using Subaru.Domain.AzureAd.Constants;
using Subaru.Domain.AzureAd.Client;
using Subaru.WebUI.Models;
using System.Collections.Generic;
using Microsoft.Graph;

namespace Subaru.WebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        readonly ITokenAcquisition tokenAcquisition;
        readonly WebOptions webOptions;

        public HomeController(ITokenAcquisition tokenAcquisition,
                              IOptions<WebOptions> webOptionValue)
        {
            this.tokenAcquisition = tokenAcquisition;
            this.webOptions = webOptionValue.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        [MsalUiRequiredExceptionFilter(Scopes = new[] { ClaimConstants.ScopeUserRead })]
        public async Task<IActionResult> Profile()
        {
            GraphServiceClient graphClient = GetGraphServiceClient(new[] { ClaimConstants.ScopeUserRead });

            var me = await graphClient.Me.Request().GetAsync();
            ViewData["Me"] = me;

            IUserMemberOfCollectionWithReferencesPage memberOfGroups = null;
            IList<Microsoft.Graph.Group> groups = new List<Microsoft.Graph.Group>();

            try
            {
                memberOfGroups = await graphClient.Me.MemberOf.Request().GetAsync();
                if (memberOfGroups != null)
                {
                    do
                    {
                        foreach (var directoryObject in memberOfGroups.CurrentPage)
                        {
                            if (directoryObject is Microsoft.Graph.Group)
                            {
                                Group group = directoryObject as Group;
                                Trace.WriteLine("Got group: " + group.Id +  " , Group Name :" + group.DisplayName);
                                groups.Add(group as Microsoft.Graph.Group);
                            }
                        }
                        if (memberOfGroups.NextPageRequest != null)
                        {
                            memberOfGroups = await memberOfGroups.NextPageRequest.GetAsync();
                        }
                        else
                        {
                            ViewData["memberOfGroups"] = memberOfGroups;
                            memberOfGroups = null;
                        }
                    } while (memberOfGroups != null);
                }
            }
            catch (Exception e)
            {
                Trace.Fail("We could not get user groups: " + e.Message);
            }
                        
            try
            {
                var photoStream = await graphClient.Me.Photo.Content.Request().GetAsync();
                byte[] photoByte = ((MemoryStream)photoStream).ToArray();
                ViewData["Photo"] = Convert.ToBase64String(photoByte);
            }
            catch (System.Exception)
            {
                ViewData["Photo"] = null;
            }

            return View();
        }

        private GraphServiceClient GetGraphServiceClient(string[] scopes)
        {
            return GraphServiceClientFactory.GetAuthenticatedGraphClient(async () =>
            {
                string result = await tokenAcquisition.GetAccessTokenOnBehalfOfUser(
                       HttpContext, scopes);
                return result;
            }, webOptions.GraphApiUrl);
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}