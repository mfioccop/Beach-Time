using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using BeachTime.Data;

namespace BeachTime
{
	public class AuthorizeBeachUserAttribute : AuthorizeAttribute
	{
		public AuthorizeBeachUserAttribute(params string[] roles)
		{
			//Roles = string.Join(",", roles);
		}

		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			if (filterContext == null)
			{
				throw new ArgumentNullException("filterContext");
			}


			if (AuthorizeCore(filterContext.HttpContext))
			{
				// ** IMPORTANT **
				// Since we're performing authorization at the action level, the authorization code runs
				// after the output caching module. In the worst case this could allow an authorized user
				// to cause the page to be cached, then an unauthorized user would later be served the
				// cached page. We work around this by telling proxies not to cache the sensitive page,
				// then we hook our custom authorization code into the caching mechanism so that we have
				// the final say on whether a page should be served from the cache.

				HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
				cachePolicy.SetProxyMaxAge(new TimeSpan(0));
				cachePolicy.AddValidationCallback(CacheValidateHandler, null /* data */);
			}
			else
			{
				HandleUnauthorizedRequest(filterContext);
			}
		}

		private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
		{
			validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
		}

		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			if (httpContext == null)
			{
				return false;
			}

			IPrincipal principal = httpContext.User;
			BeachUserManager userManager = HttpContext.Current.GetOwinContext().GetUserManager<BeachUserManager>();
			
			// If the principal or user manager doesn't exist, we can't find the user; not authorized
			if (principal == null || !principal.Identity.IsAuthenticated || userManager == null)
			{
				return false;
			}
			
			BeachUser user = userManager.FindById(principal.Identity.GetUserId());

			// If user doesn't exist, not authorized
			if (user == null)
			{
				return false;
			}
			
			UserStore store = new UserStore();
			IList<string> userRoles = store.GetRolesAsync(user).Result;
			string[] authorizedRoles = Roles.Split(',');

			// If the user does not belong to ANY of the required roles, not authorized
			if (authorizedRoles.Except(userRoles).Any())
			{
				return false;
			}

			// If all that checks out, user belongs to the required roles and is authorized
			return true;
		}

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			filterContext.Result = new RedirectResult("Error/Error403");
		}
	}
}