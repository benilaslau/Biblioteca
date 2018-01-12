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
    [Authorize]
    public class BorrowsController : Controller
    {
        private BooksDBEnities db = new BooksDBEnities();

        // GET: Borrows
        public ActionResult Index()
        {
            var borrows = db.Borrows.Include(b => b.AspNetUser).Include(b => b.Carti).Where(b => b.AspNetUser.Email == User.Identity.Name);
            return View(borrows.ToList());
        }

        // GET: Borrows/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Borrow borrow = db.Borrows.Find(id);
            if (borrow == null)
            {
                return HttpNotFound();
            }
            return View(borrow);
        }

        // GET: Borrows/Create
        public ActionResult Create()
        {
            ViewBag.Id_user = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.Id_carte = new SelectList(db.Cartis, "Id", "Nume_carte");
            
            return View();
        }

        // POST: Borrows/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_imprumut,Id_user,Id_carte,Date,Nr_carti")] Borrow borrow)
        {
            if (ModelState.IsValid)
            {
                borrow.Date = DateTime.Today.ToString();
                var us = db.Database.SqlQuery<string>("SELECT Id FROM AspNetUsers WHERE Email = @p0", User.Identity.Name).ToArray();
                borrow.Id_user = us[0];
                db.Borrows.Add(borrow);                
                db.Database.ExecuteSqlCommand("UPDATE Carti SET Nr_Disponibil=Nr_Disponibil-@p0 WHERE Id=@p1", borrow.Nr_carti, borrow.Id_carte);
                db.SaveChanges();                
                //db.Database.ExecuteSqlCommand(cmd);
                return RedirectToAction("Index");
            }

            ViewBag.Id_user = new SelectList(db.AspNetUsers, "Id", "Email", borrow.Id_user);
            ViewBag.Id_carte = new SelectList(db.Cartis, "Id", "Nume_carte", borrow.Id_carte);
            return View(borrow);
        }

        // GET: Borrows/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Borrow borrow = db.Borrows.Find(id);
            if (borrow == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_user = new SelectList(db.AspNetUsers, "Id", "Email", borrow.Id_user);
            ViewBag.Id_carte = new SelectList(db.Cartis, "Id", "Nume_carte", borrow.Id_carte);
            return View(borrow);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_imprumut,Id_user,Id_carte,Date,Nr_carti")] Borrow borrow)
        {
            if (ModelState.IsValid)
            {
                db.Entry(borrow).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_user = new SelectList(db.AspNetUsers, "Id", "Email", borrow.Id_user);
            ViewBag.Id_carte = new SelectList(db.Cartis, "Id", "Nume_carte", borrow.Id_carte);
            return View(borrow);
        }

        // GET: Borrows/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Borrow borrow = db.Borrows.Find(id);
            if (borrow == null)
            {
                return HttpNotFound();
            }
            return View(borrow);
        }

        // POST: Borrows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Borrow borrow = db.Borrows.Find(id);
            db.Database.ExecuteSqlCommand("UPDATE Carti SET Nr_Disponibil=Nr_Disponibil+@p0 WHERE Id=@p1", borrow.Nr_carti, borrow.Id_carte);
            db.Borrows.Remove(borrow); 
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
