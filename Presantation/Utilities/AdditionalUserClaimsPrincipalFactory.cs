/*using Core.Models.Application;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Presantation.Utilities
{
	public class AdditionalUserClaimsPrincipalFactory
		: UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
	{
		public AdditionalUserClaimsPrincipalFactory(
			UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager,
			IOptions<IdentityOptions> optionsAccessor)
			: base(userManager, roleManager, optionsAccessor)
		{ }

		public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
		{
			var principal = await base.CreateAsync(user);
			var identity = (ClaimsIdentity)principal.Identity;

			var claims = new List<Claim>();
				claims.Add(new Claim("TenantName", user.TenantId));

			identity.AddClaims(claims);
			return principal;
		}
	}
}
*/