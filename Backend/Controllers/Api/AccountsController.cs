using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Annotations;
using Backend.Data;
using Backend.Models;
using Backend.Models.Entities;
using Backend.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace Backend.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/accounts")]
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _map;
        private readonly UserManager<ApplicationUser> _um;

        public AccountsController(ApplicationDbContext db, IMapper map, UserManager<ApplicationUser> um)
        {
            _um = um;
            _db = db;
            _map = map;
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            var user = _map.Map<ApplicationUser>(model);
            var result = await _um.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new MessageModel { Message = "Registration error" });
            }
            await _um.AddToRoleAsync(user, RoleModel.User);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetRole()
        {
            if (User.IsInRole(RoleModel.Admin)) return Json(RoleModel.Admin);
            if (User.IsInRole(RoleModel.User)) return Json(RoleModel.User);
            return Json("ANONYMOUS");
        }
    }
}
