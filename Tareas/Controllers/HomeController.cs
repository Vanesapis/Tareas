using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tareas.Models;

namespace Tareas.Controllers
{
    public class HomeController : Controller
    {
        Models.Tarea1Entities contexto = new Models.Tarea1Entities();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult General()
        {
            var productos = contexto.Productoes.ToList();
            var categorias = contexto.CategoriaProductos.ToList();
            ViewBag.Categorias = categorias;
            return View(productos);
        }

        public ActionResult Detalle(int id)
        {

            var prod = (from p in contexto.Productoes where p.Id == id select p).FirstOrDefault();

            if (prod != null)
                return View(prod);
            else
                return RedirectToAction("Index");
        }

        public ActionResult Login()
        {
            var email = Request["email"];
            var password = Request["password"];

            var user = (from u in contexto.Usuarios where u.email == email && u.pass == password select u).FirstOrDefault();

            if (user != null)
            {
                Session["Usuario"]=user;
                return RedirectToAction("General", "Home");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult SignUp()
        {
            var mensaje = "";
            Usuario u = new Usuario();
            u.nombre = Request["nombre"];
            u.email = Request["email"];
            u.pass = Request["password"];


            try
            {
                contexto.Usuarios.Add(u);
                contexto.SaveChanges();
                mensaje = "Guardado con exito";
            }
            catch (Exception)
            {
                mensaje = "Error al guardar el dato";
            }
            ViewBag.mensaje = mensaje;

            return RedirectToAction("Index");

        }
    }
}