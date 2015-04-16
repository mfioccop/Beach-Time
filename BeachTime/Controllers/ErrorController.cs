using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeachTime.Controllers
{
    /// <summary>
    /// Controller for the error pages of the application.
    /// </summary>
    public class ErrorController : Controller
    {

	    /// <summary>
	    /// Unknown HTTP status code
	    /// </summary>
	    /// <returns></returns>
	    public ActionResult Unknown()
	    {
		    return View();
	    }

	    /// <summary>
	    /// 403 - Not authorized.
	    /// </summary>
	    /// <returns></returns>
	    public ActionResult Error403()
	    {
		    Response.StatusCode = 403;
			Response.TrySkipIisCustomErrors = true;
			return View();
	    }
        
        /// <summary>
        /// 404 - Page not found.
        /// </summary>
        /// <returns></returns>
        public ActionResult Error404()
        {
			Response.StatusCode = 404;
	        Response.TrySkipIisCustomErrors = true;
            return View();
        }

	    /// <summary>
	    /// 500 - Internal server error.
	    /// </summary>
	    /// <returns></returns>
	    public ActionResult Error500()
	    {
			Response.StatusCode = 500;
			Response.TrySkipIisCustomErrors = true;
			return View();
	    }


    }
}