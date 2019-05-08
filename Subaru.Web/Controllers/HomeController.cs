using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Client;
using Subaru.Domain.AzureAd.Services;
using Subaru.Web.Models;

namespace Subaru.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [MsalUiRequiredExceptionFilter(Scopes = new[] { ClaimConstants.ScopeUserRead })]
        public async Task<IActionResult> Profile()
        {
            // Initialize the GraphServiceClient. 
            Graph::GraphServiceClient graphClient = GetGraphServiceClient(new[] { ClaimConstants.ScopeUserRead });

            var me = await graphClient.Me.Request().GetAsync();
            ViewData["Me"] = me;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Microsoft.Graph.IUserMemberOfCollectionWithReferencesPage memberOfGroups = null;
            System.Collections.Generic.IList<Microsoft.Graph.Group> groups = new System.Collections.Generic.List<Microsoft.Graph.Group>();

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
                                Microsoft.Graph.Group group = directoryObject as Microsoft.Graph.Group;
                                Trace.WriteLine("Got group: " + group.Id + " , Group Name :" + group.DisplayName);
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

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            try
            {
                // Get user photo
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

        private Graph::GraphServiceClient GetGraphServiceClient(string[] scopes)
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
