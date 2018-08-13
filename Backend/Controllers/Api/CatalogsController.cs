using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Models;
using Backend.Models.Entities;
using Backend.Services.Interfaces;
using Backend.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Api
{
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CatalogsController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly IMapper _mapper;

        public CatalogsController(ICatalogService catalogService, IMapper mapper)
        {
            _catalogService = catalogService;
            _mapper = mapper;
        }

        [Authorize(Roles = RoleModel.Admin + ", " + RoleModel.User)]
        [HttpGet("GetMessages/{id}")]
        public ActionResult GetMessages(string id)
        {
            var catalog = _catalogService.GetCatalogWithMessages(id);

            return Ok(_mapper.Map<CatalogViewModel>(catalog));
        }
        [Authorize(Roles = RoleModel.User)]
        [HttpGet("GetRoots")]
        //[ProducesResponseType(201, Type = typeof(List<Catalog>))]
        //[ProducesResponseType(400)]
        public ActionResult GetRoots()
        {
            //var catalogs = _catalogService.GetRoots();

            return Ok(_catalogService.GetRoots());
        }

        [Authorize(Roles = RoleModel.Admin + ", " + RoleModel.User)]
        [HttpGet("GetCatalog/{id}")]
        public ActionResult GetCatalog(string id)
        {
            var catalog = _catalogService.GetCatalog(id);

            return Ok(_mapper.Map<CatalogViewModel>(catalog));
        }

        [Authorize(Roles = RoleModel.Admin)]
        [HttpPost("")]
        public async Task<IActionResult> CreateCatalog([FromBody, Bind("Title", "ParentCatalogId")]CatalogViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var catalog = _mapper.Map<Catalog>(model);

            if(!string.IsNullOrWhiteSpace(model.ParentCatalogId))
            {
                var parentCatalog = _catalogService.FindCatalogId(model.ParentCatalogId);

                catalog.ParentCatalog = parentCatalog;
            }

            await _catalogService.CreateCatalogAsync(catalog);

            return Created("", _mapper.Map<CatalogViewModel>(catalog));
        }

        [Authorize(Roles = RoleModel.Admin)]
        [HttpPut("{catalogId}")]
        public async Task<IActionResult> EditCatalog(string catalogId, [FromBody, Bind("Title","ParentCatalogId")]CatalogViewModel model)
        {                               
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var catalog = _mapper.Map<Catalog>(model);

            await _catalogService.EditCatalogAsync(catalogId, catalog);

            return Ok(_mapper.Map<CatalogViewModel>(catalog));
        }

        [Authorize(Roles = RoleModel.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCatalog(string id)
        {
            await _catalogService.DeleteCatalog(id);

            return Ok();
        }
        
    }
}