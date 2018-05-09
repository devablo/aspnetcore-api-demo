using Devable.Demo.AspNetCore.API.Models.Domain;
using Devable.Demo.AspNetCore.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Devable.Demo.AspNetCore.API.Controllers
{
    [Route("api/[controller]")]
    public class StationsController : Controller
    {
        private IStationService _stationService;

        public StationsController(IStationService stationService)
        {
            _stationService = stationService;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var models = _stationService.GetAll();

            return Ok(models);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id < 1)
                return NotFound();

            var model = _stationService.Get(id);

            return Ok(model);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Station model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var station = _stationService.Add(model);

            return CreatedAtAction("Get", new { id = station.Id }, station);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]Station model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _stationService.Update(id, model);

            return NoContent();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _stationService.Delete(id);
            return NoContent();
        }
    }
}
