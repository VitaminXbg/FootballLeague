using FootballLeague.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Main.Controllers
{
    public abstract class FootballLeagueController : ControllerBase
    {
        protected readonly FootballLeagueContext _context = new();
        protected readonly ILogger logger;

        public FootballLeagueController()
        {
            var loggerFactory = new LoggerFactory();
            logger = loggerFactory.CreateLogger<FootballLeagueController>();
        }
    }
}
