using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Calendario2.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace Calendario.Controllers
{
    public class TemasController : Controller
    {
        private Model1 db = new Model1();
        private xcursorc_calEntities db2 = new xcursorc_calEntities();

        // GET: Temas
        [Authorize]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, Int32? IdPrioridad, Int32? pageSize)
        {
            var IdUser = "";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.DescripcionSortParm = String.IsNullOrEmpty(sortOrder) ? "descripcion_desc" : "";
                ViewBag.PasadasSortParm = sortOrder == "Pasadas" ? "pasadas_desc" : "Pasadas";
                ViewBag.HoySortParm = sortOrder == "Hoy" ? "hoy_desc" : "Hoy";
                ViewBag.FuturasSortParm = sortOrder == "Futuras" ? "futuras_desc" : "Futuras";
                ViewBag.PrioridadSortParm = sortOrder == "Prioridad" ? "prioridad_desc" : "Prioridad";
                ViewBag.IdPrioridad = IdPrioridad;

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }
                string currentUserId = User.Identity.GetUserId();
                IdUser = currentUserId;
                // *****************Se actualizan los campos de historico hoy y futuras *************///
                db2.pa_act_temas_pas_hoy_fut(IdUser);
                // **********************************************************************************///
                var tem = from s in db.Temas
                          where s.FechaHora == null && s.UserId == IdUser
                          select s;
                if (!String.IsNullOrEmpty(searchString))
                {
                    tem = tem.Where(s => s.Descripcion.Contains(searchString)
                                    || s.Notas.Contains(searchString));
                }
                if (!String.IsNullOrEmpty(IdPrioridad.ToString()))
                {
                    tem = tem.Where(s => s.IdPrioridad == IdPrioridad);
                }
                tem = tem.OrderByDescending(s => s.Pasadas).ThenByDescending(s => s.Hoy).ThenByDescending(s => s.Futuras).ThenBy(s => s.Prioridad.Orden).ThenBy(s => s.Descripcion);
                Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CO");
                ViewBag.fecha = DateTime.Now.AddHours(-5).ToLongDateString();
                switch (sortOrder)
                {
                    case "descripcion_desc":
                        tem = tem.OrderBy(s => s.Descripcion);
                        break;
                    case "Prioridad":
                        tem = tem.OrderBy(s => s.Prioridad.Orden);
                        break;
                    case "prioridad_desc":
                        tem = tem.OrderByDescending(s => s.Prioridad.Orden);
                        break;
                    case "Pasadas":
                        tem = tem.OrderByDescending(s => s.Pasadas).ThenBy(s => s.Prioridad.Orden).ThenBy(s => s.Descripcion);
                        break;
                    case "pasadas_desc":
                        tem = tem.OrderBy(s => s.Pasadas).ThenBy(s => s.Prioridad.Orden).ThenBy(s => s.Descripcion);
                        break;
                    case "Hoy":
                        tem = tem.OrderByDescending(s => s.Hoy).ThenBy(s => s.Descripcion);
                        break;
                    case "hoy_desc":
                        tem = tem.OrderBy(s => s.Hoy).ThenBy(s => s.Descripcion); 
                        break;
                    case "Futuras":
                        tem = tem.OrderByDescending(s => s.Futuras).ThenBy(s => s.Prioridad.Orden).ThenBy(s => s.Descripcion);
                        break;
                    case "futuras_desc":
                        tem = tem.OrderBy(s => s.Futuras).ThenBy(s => s.Descripcion).ThenBy(s => s.Prioridad.Orden).ThenBy(s => s.Descripcion);
                        break;
                    //default:  // Name ascending 
                      //  tem = tem.OrderBy(s => s.Descripcion);
                    //    tem = tem.OrderByDescending(s => s.Prioridad.Orden);
                    //    break;
                }
                int pageSizeR = 200;
                if (!String.IsNullOrEmpty(pageSize.ToString()))
                {
                    pageSizeR =  pageSize.Value;
                    ViewBag.pageSize = pageSizeR;
                }
                int pageNumber = (page ?? 1);
                return View(tem.ToPagedList(pageNumber, pageSizeR));
            }
            else
            {
                return View();
            }
        }

        // Hoy: Temas
        [Authorize]
        public ActionResult Historico(int? IdTema,string Tema, String Fecha,string vieneIt, string viene)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CO");
            // C# (use instead of DateTime.Now)
            DateTime Fecha00 = DateTime.UtcNow;
            Fecha00 = Fecha00.AddHours(-5).Date;
            var tem9 = from s in db.Temas
                       select s;
            var Fecha01 = Fecha00.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
            if (!String.IsNullOrEmpty(Fecha))
            {
                Fecha00 = DateTime.Parse(Fecha);
                Fecha01 = Fecha00.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
            }
            ViewBag.fecha1 = Fecha01;
            ViewBag.tema = Tema;
            ViewBag.fecha = Fecha00.Date.ToLongDateString();
            var Fecha1 = Fecha01.Date.ToLongDateString();
            ViewBag.dia1 = Fecha1.ToString().Split(',');

            var IdUser = "";
            if (User.Identity.IsAuthenticated)
            {
                string currentUserId = User.Identity.GetUserId();
                IdUser = currentUserId;
                // *****************Se actualizan los campos de historico hoy y futuras *************///
                db2.pa_act_temas_pas_hoy_fut(IdUser);
                // **********************************************************************************///

            }
            tem9 = tem9.Where(s => s.FechaHora < Fecha01);
            tem9 = tem9.Where(s => s.UserId.Equals(IdUser));
            tem9 = tem9.Where(s => s.Descripcion.StartsWith(Tema.ToString()));
            tem9 = tem9.OrderByDescending(s => s.FechaHora).ThenBy(s => s.Prioridad.Orden);
            ViewBag.totIngreso = tem9.Sum(x => x.Ingreso);
            ViewBag.totPresupuesto = tem9.Sum(x => x.Presupuesto);
            ViewBag.totCompras = tem9.Sum(x => x.Compras);
            ViewBag.totPagos = tem9.Sum(x => x.Pagos);
            ViewBag.vieneIt = vieneIt;
            ViewBag.viene = viene;

            return View(tem9);
        }

        // Hoy: Temas
        [Authorize]
        public ActionResult Futuras(int? IdTema,string Tema, String Fecha,string vieneIt,string viene)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CO");
            // C# (use instead of DateTime.Now)
            DateTime Fecha00 = DateTime.UtcNow;
            Fecha00 = Fecha00.AddHours(-5).Date;

            var tem10 = from s in db.Temas
                       select s;
            //var Fecha01 = Fecha00.AddDays(1).AddHours(-23).AddMinutes(-59).AddSeconds(-59);
            var Fecha01 = Fecha00;
            if (!String.IsNullOrEmpty(Fecha))
            {
                Fecha00 = DateTime.Parse(Fecha);
                Fecha01 = Fecha00.AddDays(1).AddHours(-23).AddMinutes(-59).AddSeconds(-59);
            }
            ViewBag.fecha1 = Fecha01;
            ViewBag.tema = Tema;
            ViewBag.fecha = Fecha00.Date.ToLongDateString();
            var Fecha1 = Fecha01.Date.ToLongDateString();
            ViewBag.dia1 = Fecha1.ToString().Split(',');

            var IdUser = "";
            if (User.Identity.IsAuthenticated)
            {
                string currentUserId = User.Identity.GetUserId();
                IdUser = currentUserId;
                // *****************Se actualizan los campos de historico hoy y futuras *************///
                db2.pa_act_temas_pas_hoy_fut(IdUser);
                // **********************************************************************************///

            }
            tem10 = tem10.Where(s => s.FechaHora > Fecha01);
            tem10 = tem10.Where(s => s.UserId.Equals(IdUser));
            tem10 = tem10.Where(s => s.Descripcion.StartsWith(Tema.ToString()));
            tem10 = tem10.OrderBy(s => s.FechaHora).ThenBy(s => s.Prioridad.Orden);
            ViewBag.totIngreso = tem10.Sum(x => x.Ingreso);
            ViewBag.totPresupuesto = tem10.Sum(x => x.Presupuesto);
            ViewBag.totCompras = tem10.Sum(x => x.Compras);
            ViewBag.totPagos = tem10.Sum(x => x.Pagos);
            ViewBag.vieneIt = vieneIt;
            ViewBag.viene = viene;

            return View(tem10);
        }

        // Hoy: Temas
        [Authorize]
        public ActionResult Hoy(String Fecha,string vieneIt, string viene)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CO");
           // C# (use instead of DateTime.Now)
            DateTime Fecha00 = DateTime.UtcNow;
            Fecha00 = Fecha00.AddHours(-5).Date;

            var tem2 = from s in db.Temas
                       select s;
            //var Fecha00 = DateTime.Today.Date;
            var Fecha01 = Fecha00.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
            var Fecha02 = Fecha00.AddDays(1);
            if (!String.IsNullOrEmpty(Fecha))
            {
                Fecha00 = DateTime.Parse(Fecha);
                Fecha01 = Fecha00.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                Fecha02 = Fecha00.AddDays(1);
            }
            else
            {
                Fecha = Fecha00.Year.ToString() + "-" + Fecha00.Month.ToString() + "-" + Fecha00.Day.ToString();
            }
            tem2 = tem2.Where(s => s.FechaHora < Fecha02 && s.FechaHora > Fecha01);
            ViewBag.fecha1 = Fecha01;
            ViewBag.fecha2 = Fecha02;
            ViewBag.fecha = Fecha00.Date.ToLongDateString();
            ViewBag.fechaC = Fecha;
            var Fecha1 = Fecha01.Date.ToLongDateString();
            var Fecha2 = Fecha02.Date.ToLongDateString();

            ViewBag.dia1 = Fecha1.ToString().Split(',');
            ViewBag.dia2 = Fecha2.ToString().Split(',');

            var IdUser = "";
            if (User.Identity.IsAuthenticated)
            {
                string currentUserId = User.Identity.GetUserId();
                IdUser = currentUserId;
                // *****************Se actualizan los campos de historico hoy y futuras *************///
                db2.pa_act_temas_pas_hoy_fut(IdUser);
                // **********************************************************************************///

            }
            //if (IdUser != null && !User.IsInRole("Admin"))
            //{
            tem2 = tem2.Where(s => s.UserId.Equals(IdUser));
            tem2 = tem2.OrderBy(s => s.FechaHora).ThenBy(s => s.Prioridad.Orden);
            ViewBag.totIngreso = tem2.Sum(x => x.Ingreso);
            ViewBag.totPresupuesto = tem2.Sum(x => x.Presupuesto);
            ViewBag.totCompras = tem2.Sum(x => x.Compras);
            ViewBag.totPagos = tem2.Sum(x => x.Pagos);
            ViewBag.vieneIt = vieneIt;
            ViewBag.viene = viene;
            //}
            if (!String.IsNullOrEmpty(vieneIt))
            {
            return RedirectToAction("Hoy", new { vieneIt} );
            }
            else
            {
            return View(tem2);
            }

        }

        // Semana: Temas
        [Authorize]
        public ActionResult Semana(string Fecha)
        {


            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CO");
            // C# (use instead of DateTime.Now)
            DateTime Fecha00 = DateTime.UtcNow;
            Fecha00 = Fecha00.AddHours(-5).Date;
            ViewBag.fechaC = Fecha;

            var tem3 = from s in db.Temas
                       select s;

            if (!String.IsNullOrEmpty(Fecha))
            {
                Fecha00 = DateTime.Parse(Fecha).Date;
            }

            var int0 = 0;
            var int1 = 0;
            var int2 = 0;
            var int3 = 0;
            var int4 = 0;
            var int5 = 0;
            var int6 = 0;
            switch ((int)Fecha00.DayOfWeek)
            {
                case 1:
                    int0 = 0;
                    int1 = 1;
                    int2 = 2;
                    int3 = 3;
                    int4 = 4;
                    int5 = 5;
                    int6 = 8;
                    break;
                case 2:
                    int0 = -1;
                    int1 = 0;
                    int2 = 1;
                    int3 = 2;
                    int4 = 3;
                    int5 = 4;
                    int6 = 5;
                    break;
                case 3:
                    int0 = -2;
                    int1 = -1;
                    int2 = 0;
                    int3 = 1;
                    int4 = 2;
                    int5 = 3;
                    int6 = 4;
                    break;
                case 4:
                    int0 = -3;
                    int1 = -2;
                    int2 = -1;
                    int3 = 0;
                    int4 = 1;
                    int5 = 2;
                    int6 = 3;
                    break;
                case 5:
                    int0 = -4;
                    int1 = -3;
                    int2 = -2;
                    int3 = -1;
                    int4 = 0;
                    int5 = 1;
                    int6 = 2;
                    break;
                case 6:
                    int0 = -5;
                    int1 = -4;
                    int2 = -3;
                    int3 = -2;
                    int4 = -1;
                    int5 = 0;
                    int6 = 1;
                    break;
                case 0:
                    int0 = -6;
                    int1 = -5;
                    int2 = -4;
                    int3 = -3;
                    int4 = -2;
                    int5 = -1;
                    int6 = 0;
                    break;
            }
            var Fecha01 = Fecha00.AddDays(int0).Date;
            var Fecha02 = Fecha00.AddDays(int6+1).AddHours(23).AddMinutes(59).AddSeconds(59).Date;
            var strweek = (int)Fecha00.DayOfWeek;
            var currentCulture = CultureInfo.CurrentCulture;
            var weekNo = currentCulture.Calendar.GetWeekOfYear(Fecha00,  currentCulture.DateTimeFormat.CalendarWeekRule, currentCulture.DateTimeFormat.FirstDayOfWeek);
            ViewBag.Semana = "Semana " + weekNo.ToString() + " Día:" + strweek.ToString();
            ViewBag.intweek = strweek;
            tem3 = tem3.Where(s => s.FechaHora > Fecha01 && s.FechaHora < Fecha02);
            ViewBag.fecha1 = Fecha01;
            ViewBag.fecha2 = Fecha02;
            ViewBag.fecha = Fecha00.Date.ToLongDateString();
            var Intdaysmenos = strweek;
            //ViewBag.FechaI = Fecha00.AddDays(Intdaysmenos);
            ViewBag.FechaI = Fecha00;


            var Fecha1 = Fecha01.Date.ToLongDateString();
            var Fecha2 = Fecha02.Date.ToLongDateString();

            ViewBag.dia1 = Fecha1.ToString().Split(',');
            ViewBag.dia2 = Fecha2.ToString().Split(',');

            // Se saca la parte 


            var IdUser = "";
            if (User.Identity.IsAuthenticated)
            {
                string currentUserId = User.Identity.GetUserId();
                IdUser = currentUserId;
            }
            //if (IdUser != null && !User.IsInRole("Admin"))
            //{
            tem3 = tem3.Where(s => s.UserId.Equals(IdUser));
            tem3 = tem3.OrderBy(s => s.FechaHora).ThenBy(s => s.Prioridad.Orden);
            //}


            //ViewBag.Semana = "Semana 48";

            return View(tem3);
        }


        // Ok: Temas
        [HttpGet]
        [Authorize]
        public ActionResult Ok(int? id, int? IdPrioridad, string fecha, string temad, int? IdContacto, int? IdUsuario, string Notas,string submit)
        {
            if (String.IsNullOrEmpty(fecha))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CO");
                // C# (use instead of DateTime.Now)
                DateTime Fecha00 = DateTime.UtcNow;
                fecha = Fecha00.AddHours(-5).ToString();
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var IdUser = "";
            if (User.Identity.IsAuthenticated)
            {
                string currentUserId = User.Identity.GetUserId();
                IdUser = currentUserId;
            }


            Tema temaf = db.Temas.Find(id);
            temaf.Verificado = true;
            temaf.VerificaFechaHora = DateTime.Parse(fecha);
            temaf.Notas = Notas.ToString();


            var idcontacto = temaf.IdContacto;
            var entra = false;
            if (!String.IsNullOrEmpty(submit))
            {
                if (submit == "Ok2")
                {
                    entra = true;
                }
            }
            // buscas el usuario del tema y si es diferente a lresponsable, envía correo de verificación 
            if (temaf.IdUsuario.ToString() != idcontacto.ToString() && (entra))
            {
                //envia correo de verificacion al usuario de contacto.
                var Email = "";
                if (!String.IsNullOrEmpty(temaf.IdContacto.ToString()))
                {
                    Email = (from s in db.Contactos where (s.Id.ToString() == idcontacto.ToString()) select new { Correo = s.CorreoElectronico }).SingleOrDefault().Correo.ToString();
                    //// ****
                    MailMessage mailMsg = new MailMessage();
                    //// To
                    mailMsg.To.Add(new MailAddress(Email.ToString(), Email.ToString()));
                    ////// From
                    mailMsg.From = new MailAddress("info@xcursor.com", "Sistema Automatico de Correo CALENDARIO XcursoR");
                    ////// Subject and multipart/alternative Body
                    mailMsg.Subject = "Alerta Calendario XcursoR - > " + temaf.Descripcion;
                    //////string text = "text body";
                    string html = "<BR><BR>Se hace verificacion del tema " + temaf.Descripcion + " en fecha y hora " + temaf.VerificaFechaHora + "<br><br>Notas:" + temaf.Notas.ToString() + "<br><br>Gracias por su atención cal.xcursor.com";
                    //////mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                    mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
                    ////// Init SmtpClient and send
                    SmtpClient smtpClient = new SmtpClient("mail.xcursor.com", Convert.ToInt32(587));
                    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("info@xcursor.com", "124mateoH!!");
                    smtpClient.Credentials = credentials;
                    smtpClient.Send(mailMsg);
                }
                else
                {
                    Email = "No hay idcontacto" + idcontacto.ToString();
                }
                temaf.Notas = temaf.Notas.ToString() + " de contacto enviado " + temaf.FechaHora + " " + Email.ToString() + " Notas: " + temaf.Notas.ToString();
            }





            db.Entry(temaf).State = EntityState.Modified;
            db.SaveChanges();
            if (temaf == null)
            {
                return HttpNotFound();
            }
            //if (!String.IsNullOrEmpty(fecha))
            //{
            //    return RedirectToAction("Hoy", new { FechaC = fecha });
            //}
            //else
            //{
                return RedirectToAction("Hoy");
           // }
        }


        // Ok2: Temas
        [HttpGet]
        [Authorize]
        public ActionResult Ok2(int? id, int? IdPrioridad, string fecha, string temad, int? IdContacto, int? IdUsuario,string Notas,string submit)
        {
            if (String.IsNullOrEmpty(fecha))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CO");
                // C# (use instead of DateTime.Now)
                DateTime Fecha00 = DateTime.UtcNow;
                fecha = Fecha00.AddHours(-5).ToString();
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var IdUser = "";
            if (User.Identity.IsAuthenticated)
            {
                string currentUserId = User.Identity.GetUserId();
                IdUser = currentUserId;
            }
            Tema tema = new Tema
            {
                FechaHora = DateTime.Parse(fecha),
                Verificado = true,
                Notas = Notas + "-Ok",
                UserId = IdUser,
                IdPrioridad = IdPrioridad,
                VerificaFechaHora = DateTime.Parse(fecha),
                IdContacto = IdContacto,
                IdUsuario = IdUsuario,
                Descripcion = temad.ToString() + "=>Ok-" + Notas.ToString() 
            };
            var idcontacto = tema.IdContacto;
            var entra = false;
            if (!String.IsNullOrEmpty(submit))
            {
                if (submit == "Ok2")
                {
                    entra = true;
                }
            }
            // buscas el usuario del tema y si es diferente a lresponsable, envía correo de verificación 
            if (tema.IdUsuario.ToString() != tema.IdContacto.ToString() && (entra))
            {
                //envia correo de verificacion al usuario de contacto.
                var Email = "";
                if (!String.IsNullOrEmpty(idcontacto.ToString()))
                {
                    Email = (from s in db.Contactos where (s.Id.ToString() == idcontacto.ToString()) select new { Correo = s.CorreoElectronico }).SingleOrDefault().Correo.ToString();
                    //// ****
                    MailMessage mailMsg = new MailMessage();
                    //// To
                    mailMsg.To.Add(new MailAddress(Email.ToString(), Email.ToString()));
                    ////// From
                    mailMsg.From = new MailAddress("info@xcursor.com", "Sistema Automatico de Correo CALENDARIO XcursoR");
                    ////// Subject and multipart/alternative Body
                    mailMsg.Subject = "Alerta Calendario XcursoR - > " + tema.Descripcion;
                    //////string text = "text body";
                    string html = "<BR><BR>Se hace verificacion del tema " + tema.Descripcion + " en fecha y hora " + tema.FechaHora + "<br><br>Notas:" + tema.Notas.ToString() + "<br><br>Gracias por su atención cal.xcursor.com";
                    //////mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                    mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
                    ////// Init SmtpClient and send
                    SmtpClient smtpClient = new SmtpClient("mail.xcursor.com", Convert.ToInt32(587));
                    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("info@xcursor.com", "124mateoH!!");
                    smtpClient.Credentials = credentials;
                    smtpClient.Send(mailMsg);
                }
                else
                {
                    Email = "No hay idcontacto" + idcontacto.ToString();
                }
                tema.Notas = tema.Notas.ToString() + " de contacto enviado " + tema.FechaHora + " " + Email.ToString() + " Notas: " + tema.Notas.ToString();
            }
            db.Temas.Add(tema);
            db.SaveChanges();
            // cambia dato de notas de tema de id que viene para que quede en el listado principal la ultima nota.
            Tema temae = db.Temas.Find(id);
            temae.Notas = Notas;
            db.Entry(temae).State = EntityState.Modified;
            db.SaveChanges();
            // fin cambia
            if (tema == null)
            {
                return HttpNotFound();
            }
            //if (!String.IsNullOrEmpty(fecha))
            //{
            //    return RedirectToAction("Index", new { FechaC = fecha });
            //}
            //else
            //{
                return RedirectToAction("Index","Temas", new {  temae.IdPrioridad });
            //}
        }


        // Mes: Temas
        [Authorize]
        public ActionResult Mes(string Fecha)
        {

            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CO");
            // C# (use instead of DateTime.Now)
            DateTime Fecha00 = DateTime.UtcNow;
            Fecha00 = Fecha00.AddHours(-5).Date;
            ViewBag.fechaC = Fecha;

            var tem3 = from s in db.Temas
                       select s;

            if (!String.IsNullOrEmpty(Fecha))
            {
                Fecha00 = DateTime.Parse(Fecha).Date;
            }

            var int0 = 0;
            var int1 = 0;
            var int2 = 0;
            var int3 = 0;
            var int4 = 0;
            var int5 = 0;
            var int6 = 0;
            switch ((int)Fecha00.DayOfWeek)
            {
                case 1:
                    int0 = 0;
                    int1 = 1;
                    int2 = 2;
                    int3 = 3;
                    int4 = 4;
                    int5 = 5;
                    int6 = 8;
                    break;
                case 2:
                    int0 = -1;
                    int1 = 0;
                    int2 = 1;
                    int3 = 2;
                    int4 = 3;
                    int5 = 4;
                    int6 = 5;
                    break;
                case 3:
                    int0 = -2;
                    int1 = -1;
                    int2 = 0;
                    int3 = 1;
                    int4 = 2;
                    int5 = 3;
                    int6 = 4;
                    break;
                case 4:
                    int0 = -3;
                    int1 = -2;
                    int2 = -1;
                    int3 = 0;
                    int4 = 1;
                    int5 = 2;
                    int6 = 3;
                    break;
                case 5:
                    int0 = -4;
                    int1 = -3;
                    int2 = -2;
                    int3 = -1;
                    int4 = 0;
                    int5 = 1;
                    int6 = 2;
                    break;
                case 6:
                    int0 = -5;
                    int1 = -4;
                    int2 = -3;
                    int3 = -2;
                    int4 = -1;
                    int5 = 0;
                    int6 = 1;
                    break;
                case 0:
                    int0 = -6;
                    int1 = -5;
                    int2 = -4;
                    int3 = -3;
                    int4 = -2;
                    int5 = -1;
                    int6 = 0;
                    break;
            }
            switch (Fecha00.Month)
            {
                case 1:
                    ViewBag.Mes = "Enero";
                    break;
                case 2:
                    ViewBag.Mes = "Febrero";
                    break;
                case 3:
                    ViewBag.Mes = "Marzo";
                    break;
                case 4:
                    ViewBag.Mes = "Abril";
                    break;
                case 5:
                    ViewBag.Mes = "Mayo";
                    break;
                case 6:
                    ViewBag.Mes = "Junio";
                    break;
                case 7:
                    ViewBag.Mes = "Julio";
                    break;
                case 8:
                    ViewBag.Mes = "Agosto";
                    break;
                case 9:
                    ViewBag.Mes = "Septiembre";
                    break;
                case 10:
                    ViewBag.Mes = "Octubre";
                    break;
                case 11:
                    ViewBag.Mes = "Noviembre";
                    break;
                case 12:
                    ViewBag.Mes = "Diciembre";
                    break;
            }
            Fecha00 = DateTime.Parse(Fecha00.Year.ToString() + "-" + Fecha00.Month.ToString() + "-01");
            var Fecha01 = Fecha00.AddMonths(-1);
            var Fecha02 = Fecha00.AddMonths(1);
            var strweek = (int)Fecha00.DayOfWeek;
            var currentCulture = CultureInfo.CurrentCulture;
            var weekNo = currentCulture.Calendar.GetWeekOfYear(Fecha00, currentCulture.DateTimeFormat.CalendarWeekRule, currentCulture.DateTimeFormat.FirstDayOfWeek);
            ViewBag.Mes = ViewBag.Mes.ToString() + " Semana " + weekNo.ToString() + " Día:" + strweek.ToString();
            ViewBag.intweek = strweek;
            tem3 = tem3.Where(s => s.FechaHora > Fecha01 && s.FechaHora < Fecha02);
            ViewBag.fecha1 = Fecha01;
            ViewBag.fecha2 = Fecha02;
            ViewBag.fecha = Fecha00.Date.ToLongDateString();
            var Intdaysmenos = strweek;
            //ViewBag.FechaI = Fecha00.AddDays(Intdaysmenos);
            ViewBag.FechaI = Fecha00;


            var Fecha1 = Fecha01.Date.ToLongDateString();
            var Fecha2 = Fecha02.Date.ToLongDateString();

            ViewBag.dia1 = Fecha1.ToString().Split(',');
            ViewBag.dia2 = Fecha2.ToString().Split(',');

            // Se saca la parte 


            var IdUser = "";
            if (User.Identity.IsAuthenticated)
            {
                string currentUserId = User.Identity.GetUserId();
                IdUser = currentUserId;
            }
            //if (IdUser != null && !User.IsInRole("Admin"))
            //{
            tem3 = tem3.Where(s => s.UserId.Equals(IdUser));
            tem3 = tem3.OrderBy(s => s.FechaHora).ThenBy(s => s.Prioridad.Orden);
            //}


            //ViewBag.Semana = "Semana 48";

            return View(tem3);
        }

        // Año: Temas
        [Authorize]
        public ActionResult Año(string Fecha)
        {
            ViewBag.año = "Año 2018";
          
            //Año = Fecha.
            //Año = string.Concat("Año ", Año);
            return View();
        }

        // GET: Temas/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tema tema = db.Temas.Find(id);
            if (tema == null)
            {
                return HttpNotFound();
            }
            return View(tema);
        }

        // GET: Temas/Create
        [Authorize]
        public ActionResult Create(Int32? IdPrioridad, Int32? IdTema, string TemA)
        {
            var prior = from s in db.Prioridads
                        select s;
            var IdUser = "";
            if (User.Identity.IsAuthenticated)
            {
                string currentUserId = User.Identity.GetUserId();
                IdUser = currentUserId;
            }
            if (IdUser != null && !User.IsInRole("Admin"))
            {
                prior = prior.Where(s => s.UserId.Equals(IdUser));
            }


            ViewBag.IdContacto = new SelectList(db.Contactos, "Id", "Nombres");
            ViewBag.IdPrioridad = new SelectList(prior.ToList(), "Id", "Nombre");
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Nombre");

            return View();
        }

        // POST: Temas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,IdPrioridad,Descripcion,FechaHora,Verificado,IdUsuario,VerificaFechaHora,Notas,Compras,Pagos,IdContacto")] Tema tema6)
        {
            // C# (use instead of DateTime.Now)
            DateTime currentTime = DateTime.UtcNow;
            currentTime = currentTime.AddHours(-5);
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    string currentUserId = User.Identity.GetUserId();
                    tema6.UserId = currentUserId;
                }
                ViewBag.Descripcion = tema6.Descripcion;
                ViewBag.FechaHora = currentTime;
                db.Temas.Add(tema6);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdContacto = new SelectList(db.Contactos, "Id", "Nombres", tema6.IdContacto);
            ViewBag.IdPrioridad = new SelectList(db.Prioridads, "Id", "Nombre", tema6.IdPrioridad);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Nombre", tema6.IdUsuario);

            return View(tema6);
        }


        // GET: Temas/HoyCreate
        [Authorize]
        public ActionResult HoyCreate(Int32? IdPrioridad, Int32? IdTema, string TemA, bool? Verificado  )
        {
            // C# (use instead of DateTime.Now)
            DateTime currentTime = DateTime.Now;
            currentTime = currentTime.AddHours(-5);

            ViewBag.IdContacto = new SelectList(db.Contactos, "Id", "Nombres","Seleccione...");
            ViewBag.IdPrioridad = new SelectList(db.Prioridads, "Id", "Nombre");
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Nombre", "Seleccione...");
            ViewBag.Descripcion = TemA;
            ViewBag.FechaHora = currentTime;
            ViewBag.VerificaFechaHora = currentTime;
            ViewBag.Id = IdTema;
            ViewBag.Verificado = Verificado;

            return View();
        }

        // POST: Temas/HoyCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult HoyCreate([Bind(Include = "Id,IdPrioridad,Descripcion,FechaHora,Verificado,IdUsuario,VerificaFechaHora,Notas,Compras,Pagos,IdContacto")] Tema tema2)
        {
            if (ModelState.IsValid)
            {
                DateTime currentTime = DateTime.Now;
                currentTime = currentTime.AddHours(-5);

                if (User.Identity.IsAuthenticated)
                {
                    string currentUserId = User.Identity.GetUserId();
                    tema2.UserId = currentUserId;
                }
                ViewBag.Descripcion = tema2.Descripcion;
                if (!String.IsNullOrEmpty(tema2.FechaHora.ToString()))
                {
                    ViewBag.FechaHora = tema2.FechaHora.ToString();
                }
                else
                {
                    ViewBag.FechaHora = tema2.FechaHora = currentTime;
                }
                if (!String.IsNullOrEmpty(tema2.VerificaFechaHora.ToString()))
                {
                    ViewBag.VerificaFechaHora = tema2.VerificaFechaHora.ToString();
                }
                if (tema2.Verificado.Equals(true))
                {
                    tema2.VerificaFechaHora = currentTime;
                    ViewBag.VerificaFechaHora = tema2.VerificaFechaHora.ToString();
                }
                db.Temas.Add(tema2);
                db.SaveChanges();
                return RedirectToAction("Index","Temas", new { IdPrioridad = tema2.IdPrioridad.ToString() } );
            }
            ViewBag.IdContacto = new SelectList(db.Contactos, "Id", "Nombres", tema2.IdContacto);
            ViewBag.IdPrioridad = new SelectList(db.Prioridads, "Id", "Nombre", tema2.IdPrioridad);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Nombre", tema2.IdUsuario);
            ViewBag.Verificado = tema2.Verificado;
            return View(tema2);
        }

        // GET: Temas/HoyCreateNE
        [Authorize]
        public ActionResult HoyCreateNE(Int32? IdPrioridad, Int32? IdTema, string TemA)
        {
            // C# (use instead of DateTime.Now)
            DateTime currentTime = DateTime.Now;
            currentTime = currentTime.AddHours(-5);

            ViewBag.IdContacto = new SelectList(db.Contactos, "Id", "Nombres");
            ViewBag.IdPrioridad = new SelectList(db.Prioridads, "Id", "Nombre",IdPrioridad);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Nombre");
            ViewBag.Descripcion = TemA;
            ViewBag.FechaHora = currentTime;
            ViewBag.VerificaFechaHora = currentTime;
            ViewBag.IdTema = IdTema;
            return View();
        }

        // POST: Temas/HoyCreateNE
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult HoyCreateNE([Bind(Include = "Id,IdPrioridad,Descripcion,FechaHora,Verificado,IdUsuario,VerificaFechaHora,Notas,Compras,Pagos,IdContacto")] Tema tema2)
        {
            if (ModelState.IsValid)
            {
                DateTime currentTime = DateTime.Now;
                currentTime = currentTime.AddHours(-5);

                if (User.Identity.IsAuthenticated)
                {
                    string currentUserId = User.Identity.GetUserId();
                    tema2.UserId = currentUserId;
                }
                ViewBag.Descripcion = tema2.Descripcion;
                if (!String.IsNullOrEmpty(tema2.FechaHora.ToString()))
                {
                    ViewBag.FechaHora = tema2.FechaHora;
                }
                else
                {
                    ViewBag.FechaHora = tema2.FechaHora = currentTime;
                }
                if (!String.IsNullOrEmpty(tema2.VerificaFechaHora.ToString()))
                {
                    ViewBag.VerificaFechaHora = tema2.VerificaFechaHora;
                }
                else
                {
                    ViewBag.VerificaFechaHora = tema2.VerificaFechaHora = currentTime;
                }
                db.Temas.Add(tema2);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdContacto = new SelectList(db.Contactos, "Id", "Nombres", tema2.IdContacto);
            ViewBag.IdPrioridad = new SelectList(db.Prioridads, "Id", "Nombre", tema2.IdPrioridad);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Nombre", tema2.IdUsuario);
            ViewBag.Descripcion = tema2.Descripcion;

            return View(tema2);
        }


        // GET: Temas/Edit/5
        [Authorize]
        public ActionResult Edit(int? id, string Fecha)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tema tema = db.Temas.Find(id);
            if (tema == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdContacto = new SelectList(db.Contactos, "Id", "Nombres",tema.IdContacto);
            ViewBag.IdPrioridad = new SelectList(db.Prioridads, "Id", "Nombre", tema.IdPrioridad);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Nombre", tema.IdUsuario);
            ViewBag.FechaHora = tema.FechaHora;
            ViewBag.VerificaFechaHora = tema.VerificaFechaHora;
            ViewBag.Ingreso = tema.Ingreso;
            ViewBag.Presupuesto = tema.Presupuesto;
            ViewBag.Compras = tema.Compras;
            ViewBag.Pagos = tema.Pagos;
            return View(tema);
        }

        // POST: Temas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,IdPrioridad,Descripcion,FechaHora,Verificado,IdUsuario,VerificaFechaHora,Notas,Ingreso,Presupuesto,Compras,Pagos,IdContacto")] Tema tema3, string Fecha,string viene)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    string currentUserId = User.Identity.GetUserId();
                    tema3.UserId = currentUserId;
                }
                ViewBag.FechaHora = tema3.FechaHora;
                ViewBag.VerificaFechaHora = tema3.VerificaFechaHora;
                ViewBag.Ingreso = tema3.Ingreso;
                ViewBag.Presupuesto = tema3.Presupuesto;
                ViewBag.Compras = tema3.Compras;
                ViewBag.Pagos = tema3.Pagos;
                ViewBag.viene = viene;
                //tema3.Total = (from s in db.Temas where (s.FechaHora == null) && (s.Descripcion.Contains("=>")) && (s.UserId == tema3.UserId) select s).Count();
                var idcontacto = tema3.IdContacto;
                //if (tema3.IdUsuario.ToString() != tema3.IdContacto.ToString())
                //{
                //    //envia correo de verificacion al usuario de contacto.
                //    var Email = "";
                //    if (!String.IsNullOrEmpty(idcontacto.ToString()))
                //    {
                //        Email = (from s in db.Contactos where (s.Id.ToString() == idcontacto.ToString()) select new { Correo = s.CorreoElectronico }).SingleOrDefault().Correo.ToString();
                //        //// ****
                //        MailMessage mailMsg = new MailMessage();
                //        //// To
                //        mailMsg.To.Add(new MailAddress(Email.ToString(), Email.ToString()));
                //        ////// From
                //        mailMsg.From = new MailAddress("info@xcursor.com", "Sistema Automatico de Correo CALENDARIO XcursoR");
                //        ////// Subject and multipart/alternative Body
                //        mailMsg.Subject = "Alerta Actualizacón de Calendario XcursoR - > " + tema3.Descripcion;
                //        //////string text = "text body";
                //        string html = "<BR><BR>Se hace Actualizó el tema " + tema3.Descripcion + " en fecha y hora " + tema3.FechaHora + "<br><br>Notas:" + tema3.Notas.ToString() + "<br><br>Gracias por su atención cal.xcursor.com";
                //        //////mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                //        mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
                //        ////// Init SmtpClient and send
                //        SmtpClient smtpClient = new SmtpClient("mail.xcursor.com", Convert.ToInt32(587));
                //        System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("info@xcursor.com", "124mateoH!!");
                //        smtpClient.Credentials = credentials;
                //        smtpClient.Send(mailMsg);
                //    }
                //    else
                //    {
                //        Email = "No hay idcontacto" + idcontacto.ToString();
                //    }
                //    tema3.Notas = tema3.Notas.ToString() + " Correo de contacto enviado " + tema3.FechaHora + " " + Email.ToString();
                //}
                db.Entry(tema3).State = EntityState.Modified;
                db.SaveChanges();
                if (!String.IsNullOrEmpty(Fecha) && !String.IsNullOrEmpty(viene))
                {
                    return RedirectToAction(viene, new { Fecha, viene });
                }
                else
                {
                    if (!String.IsNullOrEmpty(Fecha))
                    {
                        return RedirectToAction("Index", "temas", new { Fecha, IdPrioridad = tema3.IdPrioridad.ToString(), viene });
                    }
                    else
                    {
                        return RedirectToAction("Index","temas", new { IdPrioridad = tema3.IdPrioridad.ToString(), viene });
                    }
                }
            }
            ViewBag.IdContacto = new SelectList(db.Contactos, "Id", "Nombres", "Seleccione...");
            ViewBag.IdPrioridad = new SelectList(db.Prioridads, "Id", "Nombre", tema3.IdPrioridad);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Nombre", "Seleccione...");
            if (String.IsNullOrEmpty(viene))
            {
                return View(tema3);
            }
            else
            {
                return RedirectToAction(viene); 
            }
        }

        // Get: Temas/NextDay/5
        [HttpGet]
        [Authorize]
        public ActionResult NextDay(int? id,string fecha)
        {
            if (String.IsNullOrEmpty(fecha))
            {
                fecha = DateTime.Now.ToString();
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tema tema = db.Temas.Find(id);
            tema.FechaHora = DateTime.Parse(fecha).AddDays(1);
            db.Entry(tema).State = EntityState.Modified;
            db.SaveChanges();
            if (tema == null)
            {
                return HttpNotFound();
            }
            if (!String.IsNullOrEmpty(fecha))
            {
                return RedirectToAction("Hoy", new { Fecha = fecha});
            }
            else
            {
                return RedirectToAction("Hoy");
            }
        }



        // GET: Temas/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tema tema = db.Temas.Find(id);
            if (tema == null)
            {
                return HttpNotFound();
            }
            return View(tema);
        }

        // POST: Temas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Tema tema = db.Temas.Find(id);
            db.Temas.Remove(tema);
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
