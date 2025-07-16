using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tweet.Models;

namespace Tweet.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private ILogger<AccountController> _logger;
    private readonly IWebHostEnvironment _env;
    
    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AccountController> logger, IWebHostEnvironment env)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _env = env;
    }

    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        /*
        if (ModelState.IsValid)
        {

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var maxFileSizeInBytes = 2 * 1024 * 1024;
            var extension = Path.GetExtension(model.Avatar.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("Avatar", "only .jpg, .jpeg, .png, .gif");
                return View(model);
            }

            if (model.Avatar == null)
            {
                ModelState.AddModelError("Avatar", "Please select a valid avatar.");
                return View(model);
            }
            if (model.Avatar.Length > maxFileSizeInBytes)
            {
                ModelState.AddModelError("Avatar", "max file size is " + maxFileSizeInBytes);
                return View(model);
            }
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Avatar.FileName);
            var uploadsPath = Path.Combine(_env.WebRootPath, "Uploads");
            var filePath = Path.Combine(uploadsPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Avatar.CopyToAsync(stream);
            }

            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                ActiveAccount = true,
                GenderId = model.GenderId,
                Bio = "Check",
                AvatarPath = "/Uploads/" + fileName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation("User {Email} Registred in.", model.Email);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        _logger.LogWarning("Model state isnt valid(or other error");
        return View(model);
        */
        if (ModelState.IsValid)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var maxFileSizeInBytes = 2 * 1024 * 1024; //2 MB
            var extention = Path.GetExtension(model.Avatar.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extention))
            {
                ModelState.AddModelError("Avatar", "only .jpg / .jpeg / .png / .gif files are allowed");
                return View(model);
            }
            if (model.Avatar.Length > maxFileSizeInBytes)
            {
                ModelState.AddModelError("Avatar", "file size must be less then 2MB");
                return View(model);
            }
            var uploadsPath = Path.Combine(_env.WebRootPath, "Uploads");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Avatar.FileName);
            var filePath = Path.Combine(uploadsPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Avatar.CopyToAsync(stream);
            }
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                ActiveAccount = true,
                GenderId = model.GenderId,
                Bio = "check",
                AvatarPath = "/Uploads/" + fileName
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation("User {Email} registered succesfully.", model.Email);
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

        }
        _logger.LogWarning("Model state isn't valid(or other error)");
        return View(model);
}

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt");
        }
        
        return View(model);
    }
    // [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}