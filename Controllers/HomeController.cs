using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using LoginReg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace LoginReg.Controllers
{
    public class HomeController : Controller
    {
        private static UserContext _context;
        public HomeController(UserContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost("/register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.Any(user => user.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use.");
                    return View("Index");
                }
            }

            if(ModelState.IsValid == false)
            {
                return View("Index");
            }
            
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            newUser.Password = hasher.HashPassword(newUser, newUser.Password);

            _context.Add(newUser);
            _context.SaveChanges();

            HttpContext.Session.SetInt32("UserId", newUser.UserID);
            HttpContext.Session.SetString("FirstName", newUser.FirstName);

            return RedirectToAction("Success");
        }

        [HttpGet("/login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost("login/user")]
        public IActionResult LoginUser(LoginUser logUser)
        {
            if(ModelState.IsValid == false)
            {
                return View("Login");
            }

            User dbUser = _context.Users.FirstOrDefault(user => user.Email == logUser.LogEmail);
            if(dbUser == null)
            {
                ModelState.AddModelError("LogEmail", "Email not found.");
                return View("Login");
            }

            PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
            PasswordVerificationResult pwCompare = hasher.VerifyHashedPassword(logUser, dbUser.Password, logUser.LogPassword);

            if(pwCompare == 0)
            {
                ModelState.AddModelError("LogEmail", "Invalid Email/Password");
                return View("Login");
            }


            HttpContext.Session.SetInt32("UserId", dbUser.UserID);
            HttpContext.Session.SetString("FirstName", dbUser.FirstName);

            return RedirectToAction("Success");
        }

        [HttpGet("Success")]
        public IActionResult Success()
        {
            return View("Success");
        }
        
        [HttpGet("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

    }
}