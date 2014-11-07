using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeachTime.Controllers
{
    public class ExecutiveController : Controller
    {
        // GET: Executive
        public ActionResult Index()
        {
            return View();
        }

        // GET: Executive/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }



        // GET: Executive/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Executive/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
