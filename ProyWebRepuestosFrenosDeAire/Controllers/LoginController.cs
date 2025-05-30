﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Capa_Bussiness;
using Capa_Entidad;
using ProyWebRepuestosFrenosDeAire.Permisos;

namespace ProyWebRepuestosFrenosDeAire.Controllers
{
    public class LoginController : Controller
    {
        LoginBussiness log = new LoginBussiness();
        DistritoBussiness dist = new DistritoBussiness();

        // GET: Login
        public ActionResult Login()
        {

            return View(new Usuario());
        }
        [HttpPost]
        public ActionResult Login(Usuario obj)
        {
            if (ModelState.IsValid)
            {
                bool registrado = log.ValidarUsuario(obj.cor_usu, obj.pass_usu);
                if (registrado)
                {
                    
                    Console.WriteLine("Email: " + obj.cor_usu);
                    Console.WriteLine("Password: " + obj.pass_usu);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Mensaje = "Usuario o contraseña incorrectos.";
                    return View();
                }
            }
            return View(obj);
        }

        public ActionResult Logout()
        {
            //FormsAuthentication.SignOut();
            Session.Clear();
            //Session["Carrito"]= new List<Carrito>();
            return RedirectToAction("Login");
        }

        public ActionResult Registrar()
        {
            ViewBag.listDist = new SelectList(dist.listaDistrito(), "cod_dist", "nom_dist");
            return View(new Usuario());
        }

        [HttpPost]
        public ActionResult Registrar(Usuario obj) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["mensaje"] = log.RegistrarUsuario(obj);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                ViewBag.mensaje = e.Message;
            }
            ViewBag.listDist = new SelectList(dist.listaDistrito(), "cod_dist", "nom_dist");
            return View(obj);
        }

        //GET
        public ActionResult ActualizarUsuario(string id)
        {
            var buscar = log.ListatUsuarios("").Find(x => x.cod_usu == id);
            ViewBag.listDist = new SelectList(dist.listaDistrito(), "cod_dist", "nom_dist");
            return View(buscar);
        }
        [HttpPost]
        public ActionResult ActualizarUsuario(string id,Usuario obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["mensaje"] = log.ActualizarUsuario(obj);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                ViewBag.mensaje = e.Message;
            }
            ViewBag.listDist = new SelectList(dist.listaDistrito(), "cod_dist", "nom_dist");
            return View(obj);
        }

        [ValidarSession]
        public ActionResult IndexUsuario(string apellido = "")
        {
            return View(log.ListatUsuarios(apellido));
        }

        public ActionResult DetailsUsuario(string id,string rol)
        {
            
            var buscar = new Usuario();
            if (rol == "R01")
            {
                buscar = log.ListatAdmin("").Find(x => x.cod_usu == id);
            }
            else
            {
                buscar = log.ListatUsuarios("").Find(x => x.cod_usu == id);
            }
            return View(buscar);
        }
    }
}