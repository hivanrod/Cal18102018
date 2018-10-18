using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Calendario2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Calendario2.Controllers
{
    public class PrioridadesController : Controller
    {
        private Model1 db = new Model1();

        // GET: Prioridades
        [Authorize]
        public ActionResult Index()
        {
            var IdUser = "";
            if (User.Identity.IsAuthenticated)
            {
                string currentUserId = User.Identity.GetUserId();
                IdUser = currentUserId;
            }
            //var temas = from t in db.Temas
            //            where t.UserId.Equals(IdUser)
            //            select t;



            var prior = from s in db.Prioridads
                        where s.UserId.Equals(IdUser)
                        //orderby s.Orden
                        //select new Prioridad {
                        //    Id = s.Id,
                        //    Nombre = s.Nombre,
                        //    Orden = s.Orden,
                        //    Temas = s.Temas,
                        //    UserId = s.UserId
                        select s;
            //};
            prior = prior.OrderByDescending(s => s.Temas.Count());

            //ViewBag.Temas = prior.Sum(s => s.Temas.Where(st => st.IdPrioridad == s.Id).Count());
            return View(prior);
        }

        // Post : Prioridades
        [HttpPost]
        [Authorize]
        public ActionResult Index(Int32? Id,Int16? Orden)
        {
            //if (!String.IsNullOrEmpty(Id.ToString()) && !String.IsNullOrEmpty(Orden.ToString()))
            if (!String.IsNullOrEmpty(Id.ToString()))
            {
                Prioridad prioridad = db.Prioridads.Find(Id);
                if (prioridad == null)
                {
                    return HttpNotFound();
                }
                var IdUser = "";
                if (User.Identity.IsAuthenticated)
                {
                    string currentUserId = User.Identity.GetUserId();
                    IdUser = currentUserId;
                }
                if (Orden == prioridad.Orden)
                {
                    prioridad.Orden = Convert.ToInt16(Orden + 1);
                }
                else
                {
                    prioridad.Orden = Convert.ToInt16(Orden - 1);
                }
                db.Prioridads.Attach(prioridad);
                db.SaveChanges();


            }
            ViewBag.orden = Id;
            return View();
        }

        // GET: Prioridades/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prioridad prioridad = db.Prioridads.Find(id);
            if (prioridad == null)
            {
                return HttpNotFound();
            }
            return View(prioridad);
        }

        // GET: Prioridades/Create
        [Authorize]      
        public ActionResult Create()
        {
            return View();
        }

        // POST: Prioridades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Nombre,Orden")] Prioridad prioridad)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    string currentUserId = User.Identity.GetUserId();
                    prioridad.UserId = currentUserId;
                }
                db.Prioridads.Add(prioridad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(prioridad);
        }

        // GET: Prioridades/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prioridad prioridad = db.Prioridads.Find(id);
            var IdUser = "";
            if (User.Identity.IsAuthenticated)
            {
                string currentUserId = User.Identity.GetUserId();
                IdUser = currentUserId;
            }
            var LstOrden = db.Prioridads.Where(x => x.UserId == IdUser).OrderBy(x => x.Orden);
            ViewBag.Orden = new SelectList(LstOrden, "Id", "Orden",prioridad.Orden);
            if (prioridad == null)
            {
                return HttpNotFound();
            }
            return View(prioridad);
        }

        // POST: Prioridades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,Nombre,Orden")] Prioridad prioridad)
        {
            if (ModelState.IsValid)
            {

                var IdUser = "";
                if (User.Identity.IsAuthenticated)
                {
                    string currentUserId = User.Identity.GetUserId();
                    IdUser = currentUserId;
                }
                var LstOrden_1 = db.Prioridads.Where(x => x.UserId == IdUser).OrderBy(x => x.Orden);
                var Cuantas = LstOrden_1.Count();
                ViewBag.Orden = new SelectList(LstOrden_1, "Id", "Orden", prioridad.Orden);
                //var norden = prioridad.Orden;
                //if (norden < 0)
                //{
                //    prioridad.Orden = 1;
                //}
                //else
                //{

                //}
                prioridad.UserId = IdUser;
                db.Entry(prioridad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(prioridad);
        }



        // GET: Prioridades/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prioridad prioridad = db.Prioridads.Find(id);
            if (prioridad == null)
            {
                return HttpNotFound();
            }
            return View(prioridad);
        }

        // POST: Prioridades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Prioridad prioridad = db.Prioridads.Find(id);
            db.Prioridads.Remove(prioridad);
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
