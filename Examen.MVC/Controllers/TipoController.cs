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
    public class TipoController : Controller
    {
        public INotyfService _notifyService { get; }

        private string urlApi;
        public TipoController(IConfiguration configuration, INotyfService notyfService)
        {
            urlApi = configuration.GetValue("ApiUrlBase", "").ToString() + "/Tipo";
            _notifyService = notyfService;
        }
        // GET: TipoController
        public ActionResult Index()
        {
            try
            {
                var data = Consume.API.Crud<Tipo>.Read(urlApi);
                _notifyService.Information("Añade un tipo a un anime");
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

        // GET: TipoController/Details/5
        public ActionResult Details(int id)
        {
            _notifyService.Success("Bienbenido a Details");
            try
                {
                var data = Crud<Tipo>.Read_ById(urlApi, id);
                return View(data);
            }
            catch (Exception ex)
            {
                _notifyService.Error("Error al cargar los datos");
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: TipoController/Create
        public ActionResult Create()
        {
            string urlApi = "https://examenapi20240515125420.azurewebsites.net/api/Anime";
            var AnimeID = Crud<Anime>.Read(urlApi);
            ViewBag.AnimeID = new SelectList(AnimeID, "AnimeID", "NombreAnime");
            return View();
        }

        // POST: TipoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tipo data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newData = Crud<Tipo>.Created(urlApi, data);
                    _notifyService.Success("Tipo creado con éxito!");
                    return RedirectToAction("Index", "Anime");
                }
                else
                {
                    _notifyService.Error("Error al crear el Tipo");
                    return PartialView("Index", data);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("Index", data);
            }
        }

        // GET: TipoController/Edit/5
        public ActionResult Edit(int id)
        {
            var data = Crud<Tipo>.Read_ById(urlApi, id);
            string urlApi2 = "https://examenapi20240515125420.azurewebsites.net/api/Anime";
            var AnimeID = Crud<Anime>.Read(urlApi2);
            ViewBag.AnimeID = new SelectList(AnimeID, "AnimeID", "NombreAnime");
            return View(data);
        }

        // POST: TipoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Tipo data)
        {
            try
            {
                Crud<Tipo>.Update(urlApi, id, data);
                _notifyService.Success("Tipo actualizado con éxito!");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _notifyService.Error("Error al actualizar el Tipo");
                ModelState.AddModelError("", ex.Message);
                return View(data);
            }
        }

        // GET: TipoController/Delete/5
        public ActionResult Delete(int id)
        {
            var data = Crud<Tipo>.Read_ById(urlApi, id);
            _notifyService.Warning("Esta apundo de borrar este Tipo");
            return View(data);
        }

        // POST: TipoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Tipo data)
        {
            try
            {
                Crud<Tipo>.Delete(urlApi, id);
                _notifyService.Information("Tipo eliminado con éxito!");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _notifyService.Error("Error al eliminar el Tipo");
                ModelState.AddModelError("", ex.Message);
                return View(data);
            }
        }
    }
}
