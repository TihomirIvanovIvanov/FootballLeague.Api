using FootballLeague.Business.Matches.Commands;
using FootballLeague.Business.Matches.Dtos;
using FootballLeague.Business.Matches.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballLeague.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly IMediator mediator;

        public MatchesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<List<MatchDto>> GetAll()
        {
            return await this.mediator.Send(new GetAllMatchesQuery());
        }

        [HttpGet("{id}")]
        public async Task<MatchDto> GetById(int id)
        {
            return await this.mediator.Send(new GetMatchByIdQuery(id));
        }

        [HttpPost]
        public async Task<int> Create([FromBody] CreateMatchCommand cmd)
        {
            return await this.mediator.Send(cmd);
        }

        [HttpPut("{id}")]
        public async Task<Unit> Update(int id, [FromBody] UpdateMatchCommand cmd)
        {
            return await this.mediator.Send(new UpdateMatchCommand(id, cmd.HomeScore, cmd.AwayScore));
        }

        [HttpDelete("{id}")]
        public async Task<Unit> Delete(int id)
        {
            return await this.mediator.Send(new DeleteMatchCommand(id));
        }
    }
}
