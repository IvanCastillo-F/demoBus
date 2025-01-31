using BusProyectApi.Data;
using BusProyectApi.Models;
using BusProyectApi.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BusProyectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public BusController(ApplicationDBContext context)
        {
            _context = context;
            
        }

        // GET ALL BUSES
        [HttpGet]

        public async Task<ActionResult<IEnumerable<BusInfo>>> GetBuses()
        {
            try
            {
                return await _context.buses.ToListAsync();
            }
            catch (Exception ex)
            {
                return Content("ERROR:\n" + ex);
            } 
        }

        //Poner string en tipo de parametro para que truene y ver error 
        // GET BUS BY ID
        [HttpGet("{BusPlate}")]
        public async Task<ActionResult<BusInfo>> GetBus(string BusPlate)
        {
            try
            {
                var bus = await _context.buses.FindAsync(BusPlate);
                if (bus == null)
                {
                    return Content("Record Not Found!");
                }
                return bus;
            }
            catch (Exception ex) 
            {
                return Content("ERROR:\n" + ex);
            }
            
        }

        // CREATE BUS
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<BusInfo>> CreateBus(BusInfo bus)
        {
            try
            {

                if (_context.buses.Any(b => b.BusPlate == bus.BusPlate))
                {
                    return Conflict("Bus already exists");
                }

                // If there are no duplicates, create the bus.
                _context.buses.Add(bus);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetBuses), new { BusPlate = bus.BusPlate }, bus);
            }
            catch (Exception ex)
            {
                return Content("ERROR:" + ex.Message);
            }
        }

        // UPDATE BUS
        [Authorize(Roles = "Admin")]
        [HttpPut("{BusPlate}")]
        public async Task<ActionResult<BusInfo>> UpdateBus(string BusPlate, BusInfo bus)
        {
            try
            {
                var existingBus = await _context.buses.FindAsync(BusPlate);
                if (existingBus == null)
                {
                    return Content("Record Not Found!");
                }

                existingBus.Capacity = bus.Capacity;
                existingBus.IsAvailable = bus.IsAvailable;
                existingBus.Category = bus.Category;
                existingBus.Status = bus.Status;

                await _context.SaveChangesAsync();
                return existingBus;
            }
            catch (Exception ex)
            {
                return Content("ERROR:\n" + ex);
            }
        }

        // DELETE BUS
        [Authorize(Roles = "Admin")]
        [HttpDelete("{BusPlate}")]
        public async Task<IActionResult> DeleteBus(string BusPlate)
        {
            try
            {
                var bus = await _context.buses.FindAsync(BusPlate);

                if (bus == null)
                {
                    return NotFound(new { message = $"Error! Bus with ID {BusPlate} not found!" });
                }
                _context.buses.Remove(bus);
                await _context.SaveChangesAsync();
                return Content("Record deleted successfuly!");
            }
            catch(Exception ex)
            {
                return Content("ERROR\n" + ex);
            }
            
        }
    }
}
