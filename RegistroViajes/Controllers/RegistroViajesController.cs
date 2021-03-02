using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RegistroViajes.Models;

namespace RegistroViajes.Controllers
{
    public class RegistroViajesController : Controller
    {
        private DatosviajeEntities db = new DatosviajeEntities();

        // GET: RegistroViajes
        public ActionResult Index()
        {
            var registroViajes = db.RegistroViajes.Include(r => r.pai).Include(r => r.pai1);
            return View(registroViajes.ToList());
        }

        // GET: RegistroViajes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistroViaje registroViaje = db.RegistroViajes.Find(id);
            if (registroViaje == null)
            {
                return HttpNotFound();
            }
            return View(registroViaje);
        }

        // GET: RegistroViajes/Create
        public ActionResult Create()
        {



            //candidad de viajes
            int[] CantidadPersonas = { 1, 2, 3, 4, 5 };
            List<int> listcatidadViajes = new List<int>();
            foreach (int arrItem in CantidadPersonas)
            {
                listcatidadViajes.Add(arrItem);
            }
            ViewBag.PaisOrigen = new SelectList(db.pais, "codigo", "descripcion");
            ViewBag.PaisDestino = new SelectList(db.pais, "codigo", "descripcion");

            var listacantidad = new List<cantidadDTO>();

            listacantidad.Add(new cantidadDTO
            {
                codigo = "0",
                descripcion = "0"

            });
            listacantidad.Add(new cantidadDTO
            {
                codigo = "1",
                descripcion = "1"

            });
            listacantidad.Add(new cantidadDTO
            {
                codigo = "2",
                descripcion = "2"

            });
            listacantidad.Add(new cantidadDTO
            {
                codigo = "3",
                descripcion = "3"

            });
            listacantidad.Add(new cantidadDTO
            {
                codigo = "4",
                descripcion = "4"

            }); listacantidad.Add(new cantidadDTO
            {
                codigo = "5",
                descripcion = "5"

            });
            ViewBag.Cantidad_OP1 = new SelectList(listacantidad, "codigo", "descripcion");
            ViewBag.Cantidad_OP2 = new SelectList(listacantidad, "codigo", "descripcion");
            ViewBag.Cantidad_OP3 = new SelectList(listacantidad, "codigo", "descripcion");




            //cantidad de viajes
            //ViewBag.CantidadViajes = new SelectList(db.pais, "codigo", "descripcion")};
            return View();
        }
        public class cantidadDTO
        {
            public string codigo { get; set; }
            public string descripcion { get; set; }
        }
        // POST: RegistroViajes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PaisOrigen,PaisDestino,Cantidad_OP1,Cantidad_OP2,Cantidad_OP3")] RegistroViaje registroViaje)
        {
            var listacantidad = new List<cantidadDTO>();
            
            if (registroViaje.Cantidad_OP1 + registroViaje.Cantidad_OP2 + registroViaje.Cantidad_OP3 <= 5)
            {
                if (ModelState.IsValid)
                {
                    db.RegistroViajes.Add(registroViaje);
                    db.SaveChanges();

                }
                int pasj0_5 = registroViaje.Cantidad_OP1;
                int pasj6_18 = registroViaje.Cantidad_OP2;
                int pasj19_65 = registroViaje.Cantidad_OP3;
                int costo0_5 = registroViaje.Cantidad_OP1 * 2864;
                int costo6_18 = registroViaje.Cantidad_OP1 * 4690;
                int costo19_65 = registroViaje.Cantidad_OP1 * 4800;

                int costoTotal = costo0_5 + costo6_18 + costo19_65;

                db.Pasajeros.Add(new Pasajero { Edad_0_5 = pasj0_5, Edad_6_18 = pasj6_18, Edad_19_65 = pasj19_65, Costo = costoTotal, IdRegistroViaje = registroViaje.ID }
                   );
                db.SaveChanges();
                return RedirectToAction("Details/");

            }
            ViewBag.PaisOrigen = new SelectList(db.pais, "codigo", "descripcion", registroViaje.PaisOrigen);
            ViewBag.PaisDestino = new SelectList(db.pais, "codigo", "descripcion", registroViaje.PaisDestino);
            ViewBag.Cantidad_OP1 = new SelectList(listacantidad, "codigo", "descripcion");
            ViewBag.Cantidad_OP2 = new SelectList(listacantidad, "codigo", "descripcion");
            ViewBag.Cantidad_OP3 = new SelectList(listacantidad, "codigo", "descripcion");
            return RedirectToAction("Index");
        }

        // GET: RegistroViajes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistroViaje registroViaje = db.RegistroViajes.Find(id);
            if (registroViaje == null)
            {
                return HttpNotFound();
            }
            ViewBag.PaisOrigen = new SelectList(db.pais, "codigo", "descripcion", registroViaje.PaisOrigen);
            ViewBag.PaisDestino = new SelectList(db.pais, "codigo", "descripcion", registroViaje.PaisDestino);
            return View(registroViaje);
        }

        // POST: RegistroViajes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PaisOrigen,PaisDestino")] RegistroViaje registroViaje)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registroViaje).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PaisOrigen = new SelectList(db.pais, "codigo", "descripcion", registroViaje.PaisOrigen);
            ViewBag.PaisDestino = new SelectList(db.pais, "codigo", "descripcion", registroViaje.PaisDestino);
            return View(registroViaje);
        }

        // GET: RegistroViajes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistroViaje registroViaje = db.RegistroViajes.Find(id);
            if (registroViaje == null)
            {
                return HttpNotFound();
            }
            return View(registroViaje);
        }

        // POST: RegistroViajes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RegistroViaje registroViaje = db.RegistroViajes.Find(id);
            var registroPasj = db.Pasajeros.Where(x => x.IdRegistroViaje == id);
            db.Pasajeros.RemoveRange(registroPasj);
            db.RegistroViajes.Remove(registroViaje);
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
