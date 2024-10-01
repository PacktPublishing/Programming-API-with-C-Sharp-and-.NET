using Cars.Data.Entities;
using Cars.Data.Interfaces;
using Cars.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarController : ControllerBase
    {
        private readonly ILogger<CarController> _logger;
        private readonly ICarRepository _carRepository;
        private readonly ICarService _carService;

        public CarController(ILogger<CarController> logger,
            ICarRepository carRepository,
            ICarService carService)
        {
           _logger = logger;
           _carRepository = carRepository;
           _carService = carService;
        }

        [HttpGet]
        public async Task<IEnumerable<Car>> GetAll(bool returnDeletedRecords = false)
        {
            return await _carRepository.GetAll(returnDeletedRecords);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> Get(int id)
        {
            var car = await _carRepository.Get(id);
            if (car == null)
            {
                return NotFound();
            }
            return car;
        }

        [HttpPost]
        public async Task<ActionResult<Car>> Post([FromBody] Car car)
        {
            try
            {
                car = await _carService.Insert(car);
            }
            catch (Exception e)
            {
                return BadRequest(e);             
            }

            return CreatedAtAction(nameof(Get), new { id = car.Id }, car);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Car car)
        {
            try
            {
                await _carService.Update(car);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return NoContent();
        }
        
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _carService.Delete(id);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            return NoContent();
        }
    }
}