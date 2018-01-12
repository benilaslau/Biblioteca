using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LoginBiblioteca.Models;

namespace LoginBiblioteca.Controllers
{
    [Authorize(Users = "admin@admin.com")]
    public class CartiController : Controller
    {
        private BooksDBEnities db = new BooksDBEnities();

        // GET: Carti
        public ActionResult Index()
        {
            return View(db.Cartis.ToList());
        }

        // GET: Carti/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carti carti = db.Cartis.Find(id);
            if (carti == null)
            {
                return HttpNotFound();
            }
            return View(carti);
        }

        // GET: Carti/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Carti/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nume_carte,Autor,Editura,Nr_Total,Nr_Disponibil")] Carti carti)
        {
            if (ModelState.IsValid)
            {
                db.Cartis.Add(carti);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(carti);
        }

        // GET: Carti/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carti carti = db.Cartis.Find(id);
            if (carti == null)
            {
                return HttpNotFound();
            }
            return View(carti);
        }

        // POST: Carti/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nume_carte,Autor,Editura,Nr_Total,Nr_Disponibil")] Carti carti)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carti).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(carti);
        }

        // GET: Carti/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carti carti = db.Cartis.Find(id);
            if (carti == null)
            {
                return HttpNotFound();
            }
            return View(carti);
        }

        // POST: Carti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Carti carti = db.Cartis.Find(id);
            db.Cartis.Remove(carti);
            db.SaveChanges();
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
