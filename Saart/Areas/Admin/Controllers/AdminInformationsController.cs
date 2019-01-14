using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Saart.Models;

namespace Saart.Areas.Admin.Controllers
{
    public class AdminInformationsController : AsyncController
    {
        private SaartEntities1 db = new SaartEntities1();

        // GET: Admin/AdminInformations
        public async Task<ActionResult> Index()
        {
            return View(await db.AdminInformations.ToListAsync());
        }

        // GET: Admin/AdminInformations/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminInformation adminInformation =await db.AdminInformations.FindAsync(id);
            if (adminInformation == null)
            {
                return HttpNotFound();
            }
            return View(adminInformation);
        }

        // GET: Admin/AdminInformations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminInformations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Name,Value")] AdminInformation adminInformation)
        {
            if (ModelState.IsValid)
            {
                db.AdminInformations.Add(adminInformation);
               await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(adminInformation);
        }

        // GET: Admin/AdminInformations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminInformation adminInformation =await db.AdminInformations.FindAsync(id);
            if (adminInformation == null)
            {
                return HttpNotFound();
            }
            return View(adminInformation);
        }

        // POST: Admin/AdminInformations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,Name,Value")] AdminInformation adminInformation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adminInformation).State = EntityState.Modified;
               await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(adminInformation);
        }

        // GET: Admin/AdminInformations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminInformation adminInformation =await db.AdminInformations.FindAsync(id);
            if (adminInformation == null)
            {
                return HttpNotFound();
            }
            return View(adminInformation);
        }

        // POST: Admin/AdminInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AdminInformation adminInformation =await db.AdminInformations.FindAsync(id);
            db.AdminInformations.Remove(adminInformation);
           await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
