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
    [Route("ProfielKeuze")]
    public class ProfielKeuzeController : ControllerBase
    {
        private readonly IProfielKeuzeRepository _profielKeuzeRepository;
        private readonly ILogger<ProfielKeuzeController> _logger;
        private readonly IAuthenticationService _authenticationService;

        public ProfielKeuzeController(IProfielKeuzeRepository profielKeuzeRepository, ILogger<ProfielKeuzeController> logger, IAuthenticationService authenticationService)
        {
            _profielKeuzeRepository = profielKeuzeRepository;
            _logger = logger;
            _authenticationService = authenticationService;
        }

        [HttpGet(Name = "ReadProfielKeuzes")]
        public async Task<ActionResult<IEnumerable<ProfielKeuze>>> Get()
        {
            try
            {
                var userId = _authenticationService.GetCurrentAuthenticatedUserId();
                if (userId == null)
                {
                    return Unauthorized();
                }

                var profielKeuzes = await _profielKeuzeRepository.GetProfielKeuzesByUserIdAsync(Guid.Parse(userId));
                return Ok(profielKeuzes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting profiel keuzes.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{profielKeuzeId}", Name = "ReadProfielKeuze")]
        public async Task<ActionResult<ProfielKeuze>> Get(Guid profielKeuzeId)
        {
            try
            {
                var profielKeuze = await _profielKeuzeRepository.ReadAsync(profielKeuzeId);
                if (profielKeuze == null)
                    return NotFound();

                return Ok(profielKeuze);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting profiel keuze.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost(Name = "CreateProfielKeuze")]
        public async Task<ActionResult> Add([FromBody] ProfielKeuze profielKeuze)
        {
            try
            {
                var userId = _authenticationService.GetCurrentAuthenticatedUserId();
                if (userId == null)
                {
                    return Unauthorized();
                }

                var existingProfielen = await _profielKeuzeRepository.GetProfielKeuzesByUserIdAsync(Guid.Parse(userId));
                if (existingProfielen.Count() >= 6)
                {
                    return BadRequest("Er kunnen maximaal 3 profielen aangemaakt worden.");
                }

                profielKeuze.Id = Guid.NewGuid();
                profielKeuze.UserId = Guid.Parse(userId); // UserId wordt hier ingesteld
                var createdProfielKeuze = await _profielKeuzeRepository.InsertAsync(profielKeuze);
                return CreatedAtRoute("ReadProfielKeuze", new { profielKeuzeId = createdProfielKeuze.Id }, createdProfielKeuze);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating profiel keuze.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{profielKeuzeId}", Name = "UpdateProfielKeuze")]
        public async Task<ActionResult> Update(Guid profielKeuzeId, ProfielKeuze newProfielKeuze)
        {
            try
            {
                var existingProfielKeuze = await _profielKeuzeRepository.ReadAsync(profielKeuzeId);

                if (existingProfielKeuze == null)
                    return NotFound();

                await _profielKeuzeRepository.UpdateAsync(newProfielKeuze);

                return Ok(newProfielKeuze);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating profiel keuze.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{profielKeuzeId}", Name = "DeleteProfielKeuze")]
        public async Task<IActionResult> Delete(Guid profielKeuzeId)
        {
            try
            {
                var existingProfielKeuze = await _profielKeuzeRepository.ReadAsync(profielKeuzeId);

                if (existingProfielKeuze == null)
                    return NotFound();

                await _profielKeuzeRepository.DeleteAsync(profielKeuzeId);

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
