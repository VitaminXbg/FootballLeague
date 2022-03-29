using FootballLeague.Data;
using FootballLeague.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Main.Controllers
{
    [Route("")]
    [ApiController]
    public class TeamsController : FootballLeagueController
    {
        // GET: api/teams
        [HttpGet]
        [Route("api/teams")]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
        {
            try
            {
                return await _context.Teams.ToListAsync();
            }
            catch(Exception ex)
            {
                logger.LogError($"Method: GetTeams: {ex.Message}");
                return StatusCode(500);
            }
        }
            

        // GET: api/teams/byranking
        [HttpGet]
        [Route("api/teams/byranking")]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeamsByRanking()
        {
            try
            {
                return await _context.Teams.OrderByDescending(t => t.Score).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError($"Method: GetTeamsByRanking: {ex.Message}");
                return StatusCode(500);
            }
        }

        // GET: api/teams/{teamId}
        [HttpGet]
        [Route("api/teams/{id}")]
        public async Task<ActionResult<Team>> GetTeam(int id)
        {
            try
            {
                var team = await _context.Teams.FindAsync(id);

                if (team == null)
                {
                    return NotFound();
                }

                return team;
            }
            catch(Exception ex)
            {
                logger.LogError($"Method: GetTeam: {ex.Message}");
                return StatusCode(500);
            }
        }

        // PUT: api/teams/{teamId}
        [HttpPut]
        [Route("api/teams/{id}")]
        public async Task<IActionResult> UpdateTeam(int id, Team team)
        {
            try
            {
                if (id != team.Id)
                {
                    return BadRequest();
                }

                _context.Entry(team).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                
                if (ex.GetType() == typeof(DbUpdateConcurrencyException) && !TeamExists(id))
                {
                    return NotFound();
                }
                else
                {
                    logger.LogError($"Method: UpdateTeam: {ex.Message}");
                    return StatusCode(500);
                }
            }

            return NoContent();
        }

        // POST: api/teams/
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("api/teams/")]
        public async Task<ActionResult<Team>> PostTeam(Team team)
        {
            try
            {
                _context.Teams.Add(team);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTeam), new { id = team.Id }, team);
            }
            catch(Exception ex)
            {
                logger.LogError($"Method: PostTeam: {ex.Message}");
                return StatusCode(500);
            }
            
        }

        // DELETE: api/teams/{teamId}
        [HttpDelete]
        [Route("api/teams/{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            try
            {
                var teamToDelete = await _context.Teams.FindAsync(id);
                if (teamToDelete == null)
                {
                    return NotFound();
                }

                _context.Teams.Remove(teamToDelete);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                logger.LogError($"Method: DeleteTodoItem: {ex.Message}");
                return StatusCode(500);
            }
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }

        private IEnumerable<Team> GetRankedTeams()
        {
            return _context.Teams.AsEnumerable().OrderByDescending(t => t.Score);
        }
    }
}
