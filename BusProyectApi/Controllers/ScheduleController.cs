using BusProyectApi.Data;
using BusProyectApi.Models;
using BusProyectApi.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusProyectApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ScheduleController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET ALL SCHEDULES
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusSchedule>>> GetSchedule()
        {
            try
            {
                return await _context.schedules.ToListAsync();
            }
            catch (Exception ex)
            {
                return Content("ERROR:\n" + ex);
            }
            
        }

        // GET BUS BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<BusSchedule>> GetBus(int id)
        {
            try
            {
                var schedule = await _context.schedules.FindAsync(id);
                if (schedule == null)
                {
                    return NotFound();
                }
                return schedule;
            }
            catch(Exception ex)
            {
                return Content("ERROR:" + ex);
            }
        }

        // CREATE SCHEDULE
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<BusSchedule>> CreateSchedule(BusSchedule schedule)
        {
            try
            {
                if (_context.schedules.Any(s => s.Id == schedule.Id))
                {
                    return Conflict("Schedule already exists");
                }

                // If there are no duplicates, create the schedule.
                _context.schedules.Add(schedule);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetSchedule), new { id = schedule.Id }, schedule);
            }
            catch (Exception ex)
            {
                return Content("ERROR:\n" + ex);
            }
            
        }

        //
        // UPDATE SCHEDULE
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<BusSchedule>> UpdateSchedule(int id, BusSchedule schedule)
        {
            try
            {
                var existingSchedule = await _context.schedules.FindAsync(id);
                if (existingSchedule == null)
                {
                    return Content("Record Not Found!");
                }

                existingSchedule.ArrivingTime = schedule.ArrivingTime;
                existingSchedule.DepartingTime = schedule.DepartingTime;
                existingSchedule.BusId = schedule.BusId;
                existingSchedule.RouteId = schedule.RouteId;

                await _context.SaveChangesAsync();
                return existingSchedule;
            }
            catch (Exception ex)
            {
                return Content("ERROR:\n" + ex);
            }
        }

        // DELETE SCHEDULE
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var schedule = await _context.schedules.FindAsync(id);

            if (schedule == null)
            {
                return NotFound(new { message = $"Error! Schedule with ID {id} not found!" });
            }
            _context.schedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return Content("Success!");
        }
    }
}
