using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models.Application;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Presantation.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ITenant _tenant;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            ITenant tenant,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _tenant = tenant;

        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Required]
            [Display(Name = "Tenant")]
            public string Tenant { get; set; }
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
        
        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                if (Input.Tenant == null)
                {
                    ModelState.AddModelError(String.Empty, "Plese Write You'r Tenant Name");
                }
               var tenant=await _tenant.GetTenantByName(Input.Tenant);
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };
               PasswordHasher<ApplicationUser> hasher = new();
                user.PasswordHash=hasher.HashPassword(user, Input.Password);
                if (tenant != null)
                {
                    var options = new DbContextOptions<ApplicationDbContext>();
                    var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>(options);
                    dbContextOptionsBuilder.UseSqlServer(tenant.ConnectionString);
                    using (ApplicationDbContext db = new ApplicationDbContext(dbContextOptionsBuilder.Options)) 
                    {
                        db.ApplicationUsers.Add(user);
                        await db.SaveChangesAsync();
                    }
                }
               
               // var result = await _userManager.CreateAsync(user, Input.Password);

                if (true)
                {
                    _logger.LogInformation("User created a new account with password.");

                   // var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                   /* //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);*/

                 /*   await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
*/
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
               /* foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }*/
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
