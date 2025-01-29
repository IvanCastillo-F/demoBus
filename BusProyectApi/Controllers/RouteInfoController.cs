using BusProyectApi.Data.Interfaces;
using BusProyectApi.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BusProyectApi.Controllers
{
    [Route("api/route")]
    [ApiController]
    public class RouteInfoController : ControllerBase
    {
        private readonly IRouteRepository _routeRepository;
        private readonly ILogger<RouteInfoController> _logger;

        public RouteInfoController(IRouteRepository routeRepository, ILogger<RouteInfoController> logger)
        {
            _routeRepository = routeRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoutes()
        {
            try
            {
                var routes = await _routeRepository.GetAllRoutesAsyc();
                bool isNullOrEmpty = routes?.Any() != true;
                if (isNullOrEmpty) { 
                    return NotFound( new
                    {
                        StatusCode= 404,
                        message="No users registered yet."
                    });
                }
                return Ok(routes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = 500,
                    message = "Records Not Found"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoutes(int id)
        {
            try
            {
                var existingRoute = await _routeRepository.GetRouteByIdAsyc(id);
                if (existingRoute == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Record Not Found"
                    });
                }
                return Ok(existingRoute);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = 500,
                    message = "Record Not Found"
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            try
            {
                var existingRoute = await _routeRepository.GetRouteByIdAsyc(id);
                if (existingRoute == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Record Not Found"
                    });
                }
                await _routeRepository.DeleteRouteAsync(existingRoute);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = 500,
                    message = "Record Not Found"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddRoute(RouteInfo route)
        {
            try
            {
                // Check if the route already exists
                var isDuplicate = await _routeRepository.IsRouteDuplicateAsync(route.DeparturePlace, route.ArrivingPlace);
                if (isDuplicate)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        message = "A route with the same origin and destination already exists."
                    });
                }
                route.Id = 0;
                // Create the route if no duplicate is found
                var createdRoute = await _routeRepository.CreateRouteAsync(route);
                return CreatedAtAction(nameof(AddRoute), createdRoute);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRoute(RouteInfo routeToUpdate)
        {
            try
            {
                // Check if the route exists
                var existingRoute = await _routeRepository.GetRouteByIdAsyc(routeToUpdate.Id);
                if (existingRoute == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Record Not Found"
                    });
                }

                // Check if the new route details conflict with an existing route
                var isDuplicate = await _routeRepository.IsRouteDuplicateForOtherRouteAsync(
                    routeToUpdate.DeparturePlace, routeToUpdate.ArrivingPlace, routeToUpdate.Id);
                if (isDuplicate)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        message = "A route with the same origin and destination already exists."
                    });
                }

                // Update the route details
                existingRoute.DeparturePlace = routeToUpdate.DeparturePlace;
                existingRoute.ArrivingPlace = routeToUpdate.ArrivingPlace;
                existingRoute.RouteName = routeToUpdate.RouteName;
                existingRoute.Distance = routeToUpdate.Distance;
                existingRoute.NumberOfStops = routeToUpdate.NumberOfStops;
                await _routeRepository.UpdateRouteAsync(existingRoute);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = 500,
                    message = "An unexpected error occurred."
                });
            }
        }


        [HttpGet("fastest-route")]
        public async Task<IActionResult> GetFastestRoute([FromQuery] string start, [FromQuery] string end)
        {
            try
            {
                if (string.IsNullOrEmpty(start) || string.IsNullOrEmpty(end))
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "Start and end points are required."
                    });
                }

                var result = await _routeRepository.GetFastestRouteAsync(start.ToUpper(), end.ToUpper());

                if (result.Distance == double.MaxValue)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = $"No path found between {start} and {end}."
                    });
                }

                return Ok(new
                {
                    Distance = result.Distance,
                    Path = string.Join(" -> ", result.Path)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating the fastest route.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while processing your request."
                });
            }
        }

    }
}
