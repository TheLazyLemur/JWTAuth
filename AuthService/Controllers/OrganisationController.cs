using System;
using System.Collections.Generic;
using System.Linq;
using AuthService.Model;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganisationController : ControllerBase
    {
        public static List<OrganisationModel> OrganisationModels { get; set; } = new()
        {
            new OrganisationModel
            {
                Id = "1", 
                Name = "Eden",
                DateJoined = new DateTime(2020,1,1),
                DateUpdated = new DateTime(2020,1,1),
            }
        };
        
        [HttpGet]
        public IActionResult GetAllOrganisations()
        {
            return Ok(OrganisationModels);
        }
        
        [HttpGet("{id}")]
        public IActionResult GetOrganisationById(string id)
        {
            var results = OrganisationModels.Where(organisationModel => organisationModel.Id == id);

            var organisationModels = results.ToList();
            
            if (organisationModels.Any())
                return Ok(organisationModels.First());

            return NoContent();
        }
    }
}