using System.Net.Mime;
using AutoMapper;
using Cars.Data.DTOs;
using Cars.Data.Entities;
using Cars.Data.Interfaces;
using Cars.Data.Repositories;
using Cars.Validators;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using Cars.Validators;
using FluentValidation;
using FluentValidation.Results;
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
        /// <param name="returnDeletedRecords">If true, the method will return all the records, including the ones that have been deleted</param>
        /// <response code="200">Cars returned</response>
        /// <response code="404">Specified Car not found</response>
        /// <response code="500">An Internal Server Error prevented the request from being executed.</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<Car>> Get(bool returnDeletedRecords = false)
        {
            return await _carRepository.Get(returnDeletedRecords);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<CarDto>> Get(int id)
        {
            var car = await _carService.Get(id);
            if (car == null)
            {
                return NotFound();
            }
            var carDto = _mapper.Map<CarDto>(car);

            return carDto;
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

                CarDtoValidator validator = new CarDtoValidator();

                validator.ValidateAndThrow(carAsDto);

                var carToInsert = _mapper.Map<Car>(carAsDto);
                var insertedCar = await _carService.Insert(carToInsert);
                var insertedCarDto = _mapper.Map<CarDto>(insertedCar);
                var location = $"https://localhost:5001/car/{insertedCarDto.Id}";
                return Created(location, insertedCarDto);
            }
            catch(ValidationException e)
            {
                IEnumerable<ValidationFailure> errors = e.Errors;
                return BadRequest(errors);
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