using Calendario2.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Calendario2.Controllers
{
    public class HomeController : Controller
    {
        private Model1 db = new Model1();

        private xcursorc_calEntities db2 = new xcursorc_calEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult MyChart()
        {
            decimal Ene = 0;
            decimal Feb = 2;
            decimal Mar = 4;
            decimal Abr = 10;
            decimal May = 1;
            decimal Jun = 0;
            decimal Jul = 6;
            decimal Ago = 12;
            decimal Sep = 11;
            decimal Oct = 10;
            decimal Nov = 15;
            decimal Dic = 20;
            new Chart(width: 400, height: 150)
                .AddSeries(
                chartType: "area",
                xValue: new[] { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" },
                yValues: new[] { Ene, Feb, Mar, Abr, May, Jun, Jul, Ago, Sep, Oct, Nov, Dic })
                .Write("png");
            return null;
        }

        public ActionResult MyChart2()
        {
            // Si no se esta en sistema se muestran datos ejemplo
            var IdUser = "";
            decimal Prioridades = 0;
            decimal Temas = 0;
            decimal Citas = 0;
            if (User.Identity.IsAuthenticated)
            {
                string currentUserId = User.Identity.GetUserId();
                IdUser = currentUserId;
                Prioridades = (from s in db.Prioridads where s.UserId == IdUser select s.Id).Count();
                Temas = (from s in db.Temas where (s.UserId == IdUser) && (s.FechaHora == null) select s.Id).Count();
                Citas = (from s in db.Temas where (s.UserId == IdUser) && (s.FechaHora != null) select s.Id).Count();
            }
            ViewBag.Temas = Temas;
            ViewBag.Prioridades = Prioridades;
            ViewBag.Citas = Citas;
            TempData["Temas"] = Temas;
            TempData["Prioridades"] = Prioridades;
            TempData["Citas"] = Citas;
            new Chart(width: 400, height: 150)
                .AddSeries(
                chartType:"column",
                xValue: new[] { "Prioridades", "Temas", "Citas" },
                yValues: new[] { Prioridades, Temas, Citas })
                .Write("png");
            return null;
        }



        public ActionResult MyArea()
        {
            //var IdUser = "";
            //string currentUserId = User.Identity.GetUserId();
            //IdUser = currentUserId;
            //var data = from s in db2.pa_sel_tema_diario(IdUser)
            //           select s;
            //var idcount = data.Count();
            //var inti = 0;
            //do while (idcount >= inti)
            //    {
            //        inti += 1;
            //        decimal day[] = s => s.Fecha.ToString();
            //        decimal data.
            //    }


            //using (db2.Temas dbContext = new Tema())
            //{
            //    IQueryable<Tema> temas = from p in dbContext.Products
            //                                   orderby p.ProductName descending
            //                                   select p;
            //    Product[] productsArray = products.ToArray();
            //    foreach (Product product in productsArray)
            //    {
            //        Console.WriteLine(product.ProductName);
            //    }
            //}



            decimal Ene = 0;
            decimal Feb = 0;
            decimal Mar = 4;
            decimal Abr = 0;
            decimal May = 0;
            decimal Jun = 0;
            decimal Jul = 6;
            decimal Ago = 0;
            decimal Sep = 0;
            decimal Oct = 10;
            decimal Nov = 0;
            decimal Dic = 0;
            new Chart(width: 400, height: 350)
                .AddSeries(
                chartType: "area",
                xValue: new[] { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" },
                yValues: new[] { Ene, Feb, Mar, Abr, May, Jun, Jul, Ago, Sep, Oct, Nov, Dic })
                .Write("png");
            return null;
        }










        public ActionResult BillChart()
        {
            decimal Dia1 = 60;
            decimal Dia2 = 0;
            decimal Dia3 = 0;
            decimal Dia4 = 5;
            decimal Dia5 = 17;
            decimal Dia6 = 45;
            decimal Dia7 = 99;
            string myTheme =
                @"<Chart BackColor=""Transparent"" >
                    <ChartAreas>
                        <ChartArea Name=""Default"" BackColor=""Transparent""></ChartArea>
                    </ChartAreas>
                </Chart>";
            new Chart(width: 150, height: 150, theme: myTheme)
                .AddSeries(
                chartType: "pie",
                xValue: new[] { "Dia1", "Dia2", "Dia3", "Dia4", "Dia5", "Dia6", "Dia7" },
                yValues: new[] { Dia1, Dia2, Dia3, Dia4, Dia5, Dia6, Dia7 })
                .Write("png");
            return null;

    

        }
        public ActionResult AreaChart()
        {

            // Create chart series and add data points into it.

            //ChartSeries series = new ChartSeries("Series Name", ChartSeriesType.Bar);

            //series.Points.Add(0, 1);

            //series.Points.Add(1, 3);

            //series.Points.Add(2, 4);

            //// Add the series to the chart series collection.

            //this.chartControl1.Series.Add(series);




            return null;

        }

    }
}