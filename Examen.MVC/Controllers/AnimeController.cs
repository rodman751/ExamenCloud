using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Examen.Entidades;
using AspNetCoreHero.ToastNotification.Abstractions;
using Consume.API;

namespace Examen.MVC.Controllers
{
    public class AnimeController : Controller
    {
        public INotyfService _notifyService { get; }

        private string urlApi;
        public AnimeController(IConfiguration configuration, INotyfService notyfService)
        {
            urlApi = configuration.GetValue("ApiUrlBase", "").ToString() + "/Anime";
            _notifyService = notyfService;
        }
        // GET: AnimeController
        public ActionResult Index()
        {
            try
            {
                var data = Consume.API.Crud<Anime>.Read(urlApi);
                return View(data);
            }
            catch (Exception ex)
            {
                if (ex.Message == "La API está cargando o ha ocurrido un error")
                {
                    _notifyService.Error("Error al cargar los datos, La API está cargando o ha ocurrido un error, vuelva a intentarlo");
                    ModelState.AddModelError("", ex.Message);
                    return RedirectToAction("Index", "ErrorAPI");
                }
                throw;
            }

        }

        // GET: AnimeController/Details/5
        public ActionResult Details(int id)
        {
            _notifyService.Success("Bienbenido a Details");
            try
            {
                var data = Crud<Anime>.Read_ById(urlApi, id);
                return View(data);
            }
            catch (Exception ex)
            {
                _notifyService.Error("Error al cargar los datos");
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: AnimeController/Create
        public ActionResult Create()
        {
            string urlApi = "https://examenapi20240515125420.azurewebsites.net/api/Tipo";
            var Tipo = Crud<Tipo>.Read(urlApi);
            ViewBag.Tipo = new SelectList(Tipo, "TipoID", "NombreTipo");
            return View();
        }

        // POST: AnimeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Anime data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newData = Crud<Anime>.Created(urlApi, data);
                    _notifyService.Success("Anime creado con éxito!");
                    _notifyService.Information("Ahora debes añadir un Tipo");
                    return RedirectToAction("Index");
                }
                else
                {
                    _notifyService.Error("Error al crear el Anime");
                    return PartialView("Index", data);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("Index", data);
            }
        }

        // GET: AnimeController/Edit/5
        public ActionResult Edit(int id)
        {
            var data = Crud<Anime>.Read_ById(urlApi, id);
            return View(data);
        }

        // POST: AnimeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Anime data)
        {
            try
            {
                Crud<Anime>.Update(urlApi, id, data);
                _notifyService.Success("Anime actualizado con éxito!");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _notifyService.Error("Error al actualizar el Anime");
                ModelState.AddModelError("", ex.Message);
                return View(data);
            }
        }

        // GET: AnimeController/Delete/5
        public ActionResult Delete(int id)
        {
            var data = Crud<Anime>.Read_ById(urlApi, id);
            _notifyService.Warning("Esta apundo de borrar este Anime");
            return View(data);
        }

        // POST: AnimeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Anime data)
        {
            try
            {
                Crud<Anime>.Delete(urlApi, id);
                _notifyService.Information("Anime eliminado con éxito!");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _notifyService.Error("Error al eliminar el Anime");
                ModelState.AddModelError("", ex.Message);
                return View(data);
            }
        }

        public ActionResult AgregarTipo()
        {
            return RedirectToAction("Index", "Tipo");
        }
    }
}
