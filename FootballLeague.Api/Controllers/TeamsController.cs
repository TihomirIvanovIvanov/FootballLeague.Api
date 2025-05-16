using FootballLeague.Business.Matches.Dtos;
using FootballLeague.Business.Matches.Queries;
using FootballLeague.Business.Teams.Commands;
using FootballLeague.Business.Teams.Dtos;
using FootballLeague.Business.Teams.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballLeague.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly IMediator mediator;

        public TeamsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<List<TeamDto>> GetAll()
        {
            return await this.mediator.Send(new GetAllTeamsQuery());
        }

        [HttpGet("{id}")]
        public async Task<TeamDto> GetById(int id)
        {
            return await this.mediator.Send(new GetTeamByIdQuery(id));
        }

        [HttpPost]
        public async Task<int> Create([FromBody] CreateTeamCommand cmd)
        {
            return await this.mediator.Send(cmd);
        }

        [HttpPut("{id}")]
        public async Task<Unit> Update(int id, [FromBody] UpdateTeamCommand cmd)
        {
            return await this.mediator.Send(new UpdateTeamCommand(id, cmd.Name));
        }

        [HttpDelete("{id}")]
        public async Task<Unit> Delete(int id)
        {
            return await this.mediator.Send(new DeleteTeamCommand(id));
        }

        [HttpGet("ranking")]
        public async Task<List<TeamRankingDto>> GetRanking()
        {
            return await this.mediator.Send(new GetTeamRankingQuery());
        }

        [HttpGet("{teamId}/matches")]
        public async Task<List<MatchDto>> GetMatches(int teamId)
        {
            return await this.mediator.Send(new GetMatchesByTeamQuery(teamId));
        }
    }
}
