using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tareas.Models;
using System.IO;


namespace Tareas.Controllers
{
    public class CategoriaController : Controller
    {
        Models.Tarea1Entities contexto = new Models.Tarea1Entities();

        // GET: Categoria
        public ActionResult Index()
        {
            if (Session["Usuario"] != null)
            {
                var cat = contexto.CategoriaProductos.ToList();
                return View(cat);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Guardar(HttpPostedFileBase imagen)
        {
            string mensaje = "";
            var id = Request["idCategoria"];
            var nombre = Request["nombre"];
            string nombreImagen = "";

            if (imagen != null)
            {
                nombreImagen = id + imagen.FileName.Substring(imagen.FileName.IndexOf("."));

                string ruta = Path.Combine(Server.MapPath("~/Content/img/cat"), Path.GetFileName(nombreImagen));
                imagen.SaveAs(ruta);
            }


            CategoriaProducto c = new CategoriaProducto();
            c.Id = int.Parse(id);
            c.Nombre = nombre;
            c.imagen = nombreImagen;


            try
            {
                contexto.CategoriaProductos.Add(c);
                contexto.SaveChanges();
                mensaje = "Guardado con exito";
            }
            catch (Exception ex)
            {
                mensaje = "Error al guardar el dato";
            }
            ViewBag.mensaje = mensaje;

            return RedirectToAction("Index");
        }
        public ActionResult InfoEditar(int id)
        {
            var cat = (from c in contexto.CategoriaProductos where c.Id == id select c).FirstOrDefault();
            return View(cat);

        }
        public ActionResult Editar(int id, HttpPostedFileBase imagen)
        {
            int Id = int.Parse(Request["idCategoria"]);
            var nombre = Request["nombre"];
            string nombreImagen = "";

            if (imagen != null)
            {
                nombreImagen = id + imagen.FileName.Substring(imagen.FileName.IndexOf("."));

                string ruta = Path.Combine(Server.MapPath("~/Content/img/cat"), Path.GetFileName(nombreImagen));
                imagen.SaveAs(ruta);
            }


            CategoriaProducto c = (from cate in contexto.CategoriaProductos where cate.Id == Id select cate).FirstOrDefault();
            if (c != null)
            {
                c.Nombre = nombre;
                c.imagen = nombreImagen;

                contexto.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        public ActionResult Borrar(int id)
        {
            var mensaje = "";
            if (Session["Usuario"] != null)
            {
                var cat = (from p in contexto.CategoriaProductos where p.Id == id select p).FirstOrDefault();

                string nombreImagen = cat.imagen;

                contexto.CategoriaProductos.Remove(cat);

                try
                {
                    contexto.SaveChanges();

                    string fullPath = Request.MapPath("~/Content/img/cat/" + nombreImagen);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                catch (Exception)
                {
                    mensaje = "Error al borrar el Categoria";
                }
                ViewBag.mensaje = mensaje;
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("General", "Home");
            }
        }

    }
}