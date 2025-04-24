using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectMap.WebApi.Models;
using ProjectMap.WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectMap.WebApi.Controllers
{
    [ApiController]
    [Route("Environment")]
    public class EnvironmentController : ControllerBase
    {
        private readonly IEnvironmentRepository _environmentRepository;
        private readonly ILogger<EnvironmentController> _logger;
        private readonly IAuthenticationService _authenticationService;

        public EnvironmentController(IEnvironmentRepository profielKeuzeRepository, ILogger<EnvironmentController> logger, IAuthenticationService authenticationService)
        {
            _environmentRepository = profielKeuzeRepository;
            _logger = logger;
            _authenticationService = authenticationService;
        }



        [HttpGet("{environmentId}", Name = "ReadEnvironments")]
        public async Task<ActionResult<Models.UserEnvironment>> Get(Guid EnvironmentId)
        {
            try
            {
                var environment = await _environmentRepository.ReadAsync(EnvironmentId);
                if (environment == null)
                    return NotFound();

                return Ok(environment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting environment.");
                return StatusCode(500, "Internal server error");
            }
        }

        //To Do:Error code here
        [HttpPost(Name = "CreateEnvironment")]
        public async Task<ActionResult> Add(UserEnvironment environment)
        {
            try
            {
                var userId = _authenticationService.GetCurrentAuthenticatedUserId();
                if (userId == null)
                {
                    return Unauthorized();
                }


                var existingProfielen = await _environmentRepository.GetEnvironmentsByUserIdAsync(Guid.Parse(userId));
                if (existingProfielen.Count() >= 3)
                {
                    return BadRequest("Er kunnen maximaal 3 environments aangemaakt worden.");
                }

                environment.Id = Guid.NewGuid();
                environment.UserId = Guid.Parse(userId); // UserId wordt hier ingesteld
                var createdEnvironment = await _environmentRepository.InsertAsync(environment);
                return CreatedAtRoute("ReadEnvironments", new { environmentId = createdEnvironment.Id }, createdEnvironment);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating profiel keuze.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{EnvironmentId}", Name = "UpdateEnvironment")]
        public async Task<ActionResult> Update(Guid EnvironmentId, Models.UserEnvironment newEnvironment)
        {
            try
            {
                var existingEnvironment = await _environmentRepository.ReadAsync(EnvironmentId);

                if (existingEnvironment == null)
                    return NotFound();

                await _environmentRepository.UpdateAsync(newEnvironment);

                return Ok(newEnvironment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating environment.");
                return StatusCode(500, "Internal server error");
            }
        }

 

        [HttpDelete("{environmentId}", Name = "DeleteEnvironment")]
        public async Task<IActionResult> Delete(Guid environmentId)
        {
            try
            {
                var existingEnvironment = await _environmentRepository.ReadAsync(environmentId);

                if (existingEnvironment == null)
                    return NotFound();

                await _environmentRepository.DeleteAsync(environmentId);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting profiel keuze.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
