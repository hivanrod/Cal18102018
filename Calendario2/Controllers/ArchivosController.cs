using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Calendario2.Models;
using Microsoft.AspNet.Identity;

namespace INTRANETVOID.Controllers
{
    public class ArchivosController : Controller
    {
        private Model1 db = new Model1();

        private xcursorc_calEntities db2 = new xcursorc_calEntities();

        // GET: Archivos
        public ActionResult Index(int? Id,string viene,string Nombre)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var arc = from t in db.Archivos
                       where t.IdTema.ToString() == Id.ToString()
                       orderby t.Nombre
                       select t;
            var tem = from t2 in db.Temas
                      where t2.Id == Id
                      select t2;
            ViewBag.Nombre = tem.SingleOrDefault().Descripcion.ToString();
            ViewBag.IdTema = Id;
            //ViewBag.Nombre = Nombre;
            return View(arc.ToList());
        }

        // GET: Archivos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archivo archivo = db.Archivos.Find(id);
            if (archivo == null)
            {
                return HttpNotFound();
            }
            return View(archivo);
        }

        // GET: Archivos/Create
        public ActionResult Create(string tema,int? IdTema)
        {
            //ViewBag.IdCliente = new SelectList(db.Clientes.OrderBy(s => s.Nombres), "Id", "Nombres");
            //ViewBag.IdTema = new SelectList(db.Temas.OrderBy(s => s.Descripcion), "Id", "Descripcion");
            ViewBag.IdTema = IdTema;
            ViewBag.tema = tema;
            return View();
        }

        // POST: Archivos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdTema,Nombre")] Archivo archivo, HttpPostedFileBase upload)
        {
             string tema = "";
            if (ModelState.IsValid)
            {
                string _path = "";
                var str_desc = "";
                if (upload.ContentLength > 0)
                {
                    //try
                    //{

                    string currentUserId = User.Identity.GetUserId();
                    var IdUser = currentUserId;
                    string _FileName = Path.GetFileName(upload.FileName);
                        tema = (from t in db.Temas
                                       where t.Id.ToString() == archivo.IdTema.ToString()
                                       select t.Descripcion).First().ToString();

                        string strPath = "~/Views/Shared/Upload/" + archivo.IdTema.ToString();
                        _path = Path.Combine(Server.MapPath(strPath), _FileName);
                        // se crea directorio si no hay archivos para ese tema, es el primer archivo que se carga
                        int cuantos = (from s in db.Archivos
                                       where s.Id.ToString() == archivo.IdTema.ToString()
                                       select s).Count();
                        if (cuantos == 0)
                        {
                           Directory.CreateDirectory(Server.MapPath(strPath));
                        }
                        upload.SaveAs(_path);
                        // crea en db
                        db.Archivos.Add(archivo);
                        archivo.Nombre = _FileName;
                        archivo.UserId = IdUser;
                        db.SaveChanges();

                        // agrega el log historico
                        str_desc = "Crea archivo - " + archivo.Nombre.ToString() + " - en carpeta -" + tema.ToString() + " -";
                        //var proc = db.pa_crear_log(1, str_desc, 1, 1, archivo.IdCliente, 1, 0, System.DateTime.Now);

                        ViewBag.Message = "Archivo cargado exitosamente!!" + str_desc.ToString();
                    //}
                    //catch
                    //{
                    //    ViewBag.Message = "No se pudo cargar el archivo!!" + str_desc.ToString();
                    //}

                }

            }
            //ViewBag.IdCliente = new SelectList(db.Clientes.OrderBy(s => s.Nombres), "Id", "Nombres", archivo.IdCliente);
            ViewBag.IdTema = new SelectList(db.Temas.OrderBy(s => s.Descripcion), "Id", "Nombre", archivo.IdTema);
            ViewBag.Message = ViewBag.Message + "-->> Y se creo correctamente el registro en la base de datos!!";
            //return  RedirectToAction("Index", new { tema });
            TempData["Msg"] = ViewBag.Message;

            return RedirectToAction("Index", new { Id = archivo.IdTema, archivo.Nombre });

            //return View(archivo);

        }

        // GET: Archivos/Edit/5 
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archivo archivo = db.Archivos.Find(id);
            if (archivo == null)
            {
                return HttpNotFound();
            }
            //ViewBag.IdCliente = new SelectList(db.Clientes, "Id", "Nombres", archivo.IdCliente);
            ViewBag.IdTema = new SelectList(db.Temas, "Id", "Descripcion", archivo.IdTema);
            return View(archivo);
        }

        // POST: Archivos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdTema,Nombre")] Archivo archivo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(archivo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.IdCliente = new SelectList(db.Clientes, "Id", "Nombres", archivo.IdCliente);
            ViewBag.IdTema = new SelectList(db.Temas, "Id", "Descripcion", archivo.IdTema);
            return View(archivo);
        }

        // GET: Archivos/Delete/5
        public ActionResult Delete(int? id,string Nombre)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archivo archivo = db.Archivos.Find(id);
            ViewBag.Nombre = Nombre;
            ViewBag.IdTema = archivo.IdTema;
            if (archivo == null)
            {
                return HttpNotFound();
            }
            return View(archivo);
        }

        // POST: Archivos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id,string Nombre)
        {
            Archivo archivo = db.Archivos.Find(id);
            string fullPath = Request.MapPath("~/Views/Shared/Upload/" + archivo.IdTema.ToString() + "/" + archivo.Nombre.ToString());
            var str_desc = "";
            var cuantos = 0;
            var IdTema = "";
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
                str_desc = "Elimina archivo - " + archivo.Nombre.ToString() + " - de carpeta -" + archivo.IdTema.ToString() + " -";


                IdTema = archivo.IdTema.ToString();
                // ** Si es el ultimo archivo debe borrar la carpeta
                cuantos = (from s in db.Archivos
                               where s.IdTema.ToString() == IdTema.ToString()
                               select s).Count();
            }
            else
            {
                str_desc = "No se encontró el archivo en el sistema.  Se Elimina archivo de la base de datos - " + archivo.Nombre.ToString() + " - de carpeta -" + archivo.IdTema.ToString() + " -";
            }
            // logs
            //var proc = db.pa_crear_log(3, str_desc, 1, 1, archivo.IdCliente, 3, 0, System.DateTime.Now);
            // bprra de bd
            db.Archivos.Remove(archivo);
            db.SaveChanges();
            ViewBag.Nombre = Nombre;
            ViewBag.IdTema = archivo.IdTema;
            var strPath2 = "";
            // ** Si es el ultimo archivo debe borrar la carpeta
            if (cuantos == 1)
            {

                string strPath = "~/Views/Shared/Upload/" + archivo.IdTema.ToString();
                strPath2 = Path.Combine(Server.MapPath(strPath), "");
                //strPath2 = Server.MapPath(strPath);

                if (Directory.Exists(strPath2))
                {
                    var cantf = Directory.EnumerateFiles(strPath2).Count();
                    if (cantf == 0)
                    //    var _path = Path.Combine(Server.MapPath(strPath),"");
                    {
                        var _path = Path.Combine(Server.MapPath(strPath));
                        Directory.Delete(_path);
                        str_desc = str_desc + "Se borro el directorio SATISFACTORIAMENTE! ";
                    }
                    else
                    {
                        str_desc = str_desc + "No se pudo borrar el directorio - " + archivo.IdTema.ToString() + " - por que no esta vacio ";
                    }
                }
                else
                {
                    str_desc = str_desc + " No se encontró el directorio - " + strPath2 + " - a borrar";
                }
            }
            TempData["Msg"] = str_desc;
            return RedirectToAction("Index",new {Id=archivo.IdTema, Nombre });
            //return View();
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
