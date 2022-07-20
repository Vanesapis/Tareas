using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tareas.Models;
using System.IO;


namespace Tareas.Controllers
{
    public class ProductoController : Controller
    {
        Models.Tarea1Entities contexto = new Models.Tarea1Entities();
        // Aquí recibo la lista de productos
        public ActionResult Index()
        {
            if (Session["Usuario"] != null)
            {
                var prod = contexto.Productoes.ToList();
                var cat = contexto.CategoriaProductos.ToList();
                ViewBag.Categorias = cat;
                return View(prod);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Guardar(HttpPostedFileBase imagen)
        {
            string mensaje = "";
            var id = Request["idProducto"];
            var nombre = Request["nombre"];
            var descripcion = Request["descripcion"];
            var precio = Request["precio"];
            var stock = Request["stock"];
            var idCategoria = Request["idCategoria"];
            string nombreImagen = "";

            if (imagen != null)
            {
                nombreImagen = id + imagen.FileName.Substring(imagen.FileName.IndexOf("."));

                string ruta = Path.Combine(Server.MapPath("~/Content/img/prod"), Path.GetFileName(nombreImagen));
                imagen.SaveAs(ruta);
            }


            Producto p = new Producto();
            p.Id = int.Parse(id);
            p.Nombre = nombre;
            p.Descripcion = descripcion;
            p.Precio = int.Parse(precio);
            p.Stock = int.Parse(stock);
            p.IdCategoria = int.Parse(idCategoria);
            p.imagen = nombreImagen;


            try
            {
                contexto.Productoes.Add(p);
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
            var prod = (from p in contexto.Productoes where p.Id == id select p).FirstOrDefault();


            if (prod != null)
            {
                var cat = contexto.CategoriaProductos.ToList();
                ViewBag.Categorias = cat;
                return View(prod);
            }
            else
                return RedirectToAction("Index");
        }
        public ActionResult Editar(int id, HttpPostedFileBase imagen)
        {
            int Id = int.Parse(Request["idProducto"]);
            var nombre = Request["nombre"];
            var descripcion = Request["descripcion"];
            var precio = Request["precio"];
            var stock = Request["stock"];
            var idCategoria = Request["idCategoria"];
            string nombreImagen = "";

            if (imagen != null)
            {
                nombreImagen = id + imagen.FileName.Substring(imagen.FileName.IndexOf("."));

                string ruta = Path.Combine(Server.MapPath("~/Content/img/prod"), Path.GetFileName(nombreImagen));
                imagen.SaveAs(ruta);
            }


            Producto p = (from pro in contexto.Productoes where pro.Id == Id select pro).FirstOrDefault();
            if (p != null)
            {
                p.Nombre = nombre;
                p.Descripcion = descripcion;
                p.Precio = int.Parse(precio);
                p.Stock = int.Parse(stock);
                p.IdCategoria = int.Parse(idCategoria);
                p.imagen = nombreImagen;

                contexto.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        public ActionResult Borrar(int id)
        {
            if (Session["Usuario"] != null)
            {
                var prod = (from p in contexto.Productoes where p.Id == id select p).FirstOrDefault();

                string nombreImagen = prod.imagen;

                contexto.Productoes.Remove(prod);

                contexto.SaveChanges();

                string fullPath = Request.MapPath("~/Content/img/prod/" + nombreImagen);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }


                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("General","Home");
            }
        }

    }
}