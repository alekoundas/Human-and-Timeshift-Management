using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models.Entity;
using Bussiness;
using DataAccess.ViewModels.Leaves;

namespace WebApplication.Api
{
    [Route("api/leaves")]
    [ApiController]
    public class LeavesApiController : ControllerBase
    {
        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;
        public LeavesApiController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(SecurityDbContext);
        }

        // GET: api/leaves
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Leave>>> GetLeaves()
        {
            return await _context.Leaves.ToListAsync();
        }

        // GET: api/leaves/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Leave>> GetLeave(int id)
        {
            var leave = await _context.Leaves.FindAsync(id);

            if (leave == null)
                return NotFound();

            return leave;
        }

        // PUT: api/leaves/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeave(int id, Leave leave)
        {
            if (id != leave.Id)
                return BadRequest();

            _context.Entry(leave).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaveExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/leaves/postleave
        [HttpPost("postleave")]
        public async Task<ActionResult<Leave>> PostLeave(ApiLeavesAdd apiLeave)
        {
            if (apiLeave.EmployeeIds.Count() > 0)
                apiLeave.EmployeeIds.ForEach(id => _baseDataWork.Leaves.Add(
                    new Leave
                    {
                        StartOn = apiLeave.StartOn,
                        EndOn = apiLeave.EndOn,
                        Description = apiLeave.Description,
                        EmployeeId = id,
                        CreatedOn = DateTime.Now
                    }));
            await _context.SaveChangesAsync();

            return Ok("Success my dudes");
        }

        // DELETE: api/leaves/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Leave>> DeleteLeave(int id)
        {
            var leave = await _context.Leaves.FindAsync(id);
            if (leave == null)
                return NotFound();

            _context.Leaves.Remove(leave);
            await _context.SaveChangesAsync();

            return leave;
        }


        //TODO: Fix async in repo
        // POST: api/leaves/hasoverlap
        [HttpPost("hasoverlap")]
        public async Task<ActionResult<Leave>> HasOverlap(ApiLeavesHasOverlap apiLeave)
        {
            var dataToReturn = new List<ApiLeavesHasOverlapResponse>();

            if (apiLeave.EmployeeIds.Count() > 0)
                foreach (var id in apiLeave.EmployeeIds)
                {
                    dataToReturn.AddRange(await _baseDataWork.DateHasOverlap(
                        apiLeave.StartOn, apiLeave.EndOn, id) ?? 
                        new List<ApiLeavesHasOverlapResponse>());
                }

            return Ok(dataToReturn);
        }


        private bool LeaveExists(int id)
        {
            return _context.Leaves.Any(e => e.Id == id);
        }
    }
}
