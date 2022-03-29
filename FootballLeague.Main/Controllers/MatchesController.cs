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
    public class MatchesController : FootballLeagueController
    {
        public MatchesController()
        {
            logger.LogError("Test Debug logger");
        }

        // GET: api/matches
        [HttpGet]
        [Route("api/matches")]
        public async Task<ActionResult<IEnumerable<Match>>> GetMatches()
        {
            try
            {
                var matches = _context.Matches.ToList();
                BindTeams(matches);
                return await _context.Matches.ToListAsync();
            }
            catch(Exception ex)
            {
                logger.LogError($"Method: GetMatches: {ex.Message}");
                return StatusCode(500);
            }
        }

        

        // GET: api/matches/{matchId}
        [HttpGet]
        [Route("api/matches/{id}")]
        public async Task<ActionResult<Match>> GetMatch(int id)
        {
            try
            {
                var matches = _context.Matches.ToList();
                BindTeams(matches);
                var match = await _context.Matches.FindAsync(id);

                if (match == null)
                {
                    return NotFound();
                }

                return match;
            }
            catch (Exception ex)
            {
                logger.LogError($"Method: GetMatch: {ex.Message}");
                return StatusCode(500);
            }
        }

        // PUT: api/matches/{matchId}
        [HttpPut]
        [Route("api/matches/{id}")]
        public async Task<IActionResult> UpdateMatch(int id, Match match)
        {
            try
            {
                if (id != match.Id)
                {
                    return BadRequest();
                }

                _context.Entry(match).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError($"Method: UpdateMatch: {ex.Message}");
                return StatusCode(500);
            }
        }

        // POST: api/matches/
        [HttpPost]
        [Route("api/matches/")]
        public async Task<ActionResult<Match>> PostMatch(Match match)
        {
            try
            {
                //assuming Name is the only know parameter
                //if select is used, then id would be more appropriate
                var homeTeam = _context.Teams.FirstOrDefault(t => t.Name == match.HomeTeam.Name);
                if (homeTeam == null)
                {
                    return NotFound();
                }

                var awayTeam = _context.Teams.FirstOrDefault(t => t.Name == match.AwayTeam.Name);
                if (awayTeam == null)
                {
                    return NotFound();
                }
                //assuming teams have exist before adding a match

                match.HomeTeam = homeTeam;
                match.AwayTeam = awayTeam;
                _context.Matches.Add(match);


                if (match.HomeTeamGoals > match.AwayTeamGoals)
                {
                    homeTeam.Score += 3;
                    _context.Entry(homeTeam).State = EntityState.Modified;
                }
                else if (match.HomeTeamGoals < match.AwayTeamGoals)
                {
                    awayTeam.Score += 3;
                    _context.Entry(awayTeam).State = EntityState.Modified;
                }
                else
                {
                    homeTeam.Score++;
                    _context.Entry(homeTeam).State = EntityState.Modified;
                    awayTeam.Score++;
                    _context.Entry(awayTeam).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetMatch), new { id = match.Id }, match);
            }
            catch (Exception ex)
            {
                logger.LogError($"Method: PostMatch: {ex.Message}");
                return StatusCode(500);
            }
        }

        // DELETE: api/matches/{matchId}
        [HttpDelete]
        [Route("api/matches/{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            try
            {
                var matchToDelete = await _context.Matches.FindAsync(id);
                if (matchToDelete == null)
                {
                    return NotFound();
                }

                _context.Matches.Remove(matchToDelete);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError($"Method: DeleteTodoItem: {ex.Message}");
                return StatusCode(500);
            }
        }

        private bool MatchExists(int id)
        {
            return _context.Matches.Any(e => e.Id == id);
        }

        private void BindTeams(IEnumerable<Match> matches)
        {
            foreach(var match in matches)
            {
                var homeTeam = _context.Teams.Find(match.HomeTeamId);
                var awayTeam = _context.Teams.Find(match.AwayTeamId);

                match.HomeTeam.Id = homeTeam.Id;
                match.HomeTeam.Name = homeTeam.Name;
                match.HomeTeam.Score = homeTeam.Score;
                
                match.AwayTeam.Id = awayTeam.Id;
                match.AwayTeam.Name = awayTeam.Name;
                match.AwayTeam.Score = awayTeam.Score;
            }
        }
    }
}
