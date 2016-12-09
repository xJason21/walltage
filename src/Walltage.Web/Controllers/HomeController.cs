﻿using log4net;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Walltage.Service;
using Walltage.Service.Models;
using Walltage.Service.Services;
using Walltage.Web.Infrastructures;

namespace Walltage.Web.Controllers
{
    public class HomeController : Controller
    {
        // ToDo : Check AntiValidate attribute

        private readonly ILog _logger;
        private readonly IWallpaperService _wallpaperService;

        public HomeController(ILog logger,
            IWallpaperService wallpaperService)
        {
            _logger = logger;
            _wallpaperService = wallpaperService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetHomePageWallpapers()
        {
            var result = _wallpaperService.GetHomePageUploads(20);
            if (result == null)
            {
                ModelState.AddModelError("", "Doesnt find any wallpaper for display to homepage !");
                _logger.Warn("Doesnt find any wallpaper for display to homepage !");
                return Json(null);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Search(string q)
        {
            var result = _wallpaperService.GetSearchResult(q);
            if (result == null)
            {
                ModelState.AddModelError("", "Not found, try again!");
                return View();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ViewResult Faq()
        {
            return View();
        }

        public ActionResult Rules()
        {
            return View();
        }

        public ActionResult Stats()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(object s)
        {
            return View(s);
        }

        public ActionResult Category(int id)
        {
            return View();
        }

        public ActionResult Wallpaper(int id)
        {

            return View();
        }

        [UserAuthorize]
        public ActionResult Upload()
        {
            var categories = _wallpaperService.GetCategoryList();
            var resolutions = _wallpaperService.GetResolutionList();

            var model = new WallpaperViewModel
            {
                CategoryList = categories,
                ResolutionList = resolutions
            };
            return View(model);  
        }

        [HttpPost, ActionName("Upload")]
        [ValidateAntiForgeryToken]
        public ActionResult UploadWallpaper(WallpaperViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _wallpaperService.WallpaperInsert(model);
                if (result.Success)
                {
                    ViewBag.Message = "Wallpaper uploaded successfuly !";
                    return RedirectToAction("Upload");
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);
            }
            var categories = _wallpaperService.GetCategoryList();
            var resolutions = _wallpaperService.GetResolutionList();

            model = new WallpaperViewModel
            {
                CategoryList = categories,
                ResolutionList = resolutions
            };
            return View(model);
        }
    }
}