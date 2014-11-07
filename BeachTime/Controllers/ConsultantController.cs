using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeachTime.Controllers
{
	//[Authorize(Roles = "Consultant")]
    public class ConsultantController : Controller
    {
        // GET: Consultant
        public ActionResult Index()
        {
            return View();
        }

        // GET: Consultant/Edit/5
        public ActionResult Edit(int id)
        {
			// Current Project
			// Skill tags
			// New email
			// Confirm new email
			// New password
			// Confirm new password

			// OLD PASSWORD???

            return View();
        }

        // POST: Consultant/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

				// Current Project
				// Skill tags
				// New email
				// Confirm new email
				// New password
				// Confirm new password

				// OLD PASSWORD???

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


		// GET: Consultant/Upload
	    public ActionResult Upload()
	    {
		    return PartialView("_Upload");
	    }

    }
}
