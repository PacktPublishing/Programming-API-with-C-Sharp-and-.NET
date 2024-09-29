using System.Net.Mime;
using AutoMapper;
using Cars.Data.DTOs;
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
        private readonly IMapper _mapper;


        public CarController(ILogger<CarController> logger,
            ICarRepository carRepository,
            ICarService carService,
            IMapper mapper)
        {
           _logger = logger;
           _carRepository = carRepository;
           _carService = carService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all the cars in the Database 
        /// </summary>
        /// <param name="returnDeletedRecords">If true, the method will return all the records</param> 
        /// <param name="pageOffset">which page to display</param>
        /// <param name="pageSize">how many records to display per page</param>
        /// <response code="200">Cars returned</response>
        /// <response code="404">Specified Car not found</response>
        /// <response code="500">An Internal Server Error prevented the request from being executed.</response>
        [HttpGet]
        public async Task<IEnumerable<Car>> Get([FromRoute] bool showDeleted, int pageNumber, int pageSize )
        {
            return await _carRepository.Get(showDeleted, pageNumber, pageSize);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> Get(int id)
        {
            var car = await _carService.Get(id);
            if (car == null)
            {
                return NotFound();
            }
            return car;
        }

        [HttpPost]
        public async Task<ActionResult<Car>> Insert([FromBody] CarDto carAsDto)
        {
            try
            {
                if (carAsDto == null)
                {
                    return BadRequest("No car was provided");
                }

                var carToInsert = _mapper.Map<Car>(carAsDto);
                var insertedCar = await _carService.Insert(carToInsert);
                var insertedCarDto = _mapper.Map<CarDto>(insertedCar);
                var location = $"https://localhost:5001/car/{insertedCarDto.Id}";
                return Created(location, insertedCarDto);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
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