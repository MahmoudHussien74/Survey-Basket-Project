using Microsoft.AspNetCore.Authorization;
using Survey_Basket.Abstractions;
using Survey_Basket.Contracts.Votes;
using Survey_Basket.Extentions;

namespace Survey_Basket.Controllers
{
    [Route("api/polls/{pollId}/vote")]
    [ApiController]
    [Authorize]
    public class VotesController(IQuestionService questionService,IVoteService voteService) : ControllerBase
    {
        private readonly IQuestionService _questionService = questionService;
        private readonly IVoteService _voteService = voteService;

        [HttpGet("")]
        public async Task<IActionResult> Start([FromRoute] int pollId,CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            var result = await _questionService.GetAvilableAsync(pollId,userId! ,cancellationToken);

            if(result.IsSuccess) 
                return Ok(result.Value);

            return result.ToProblem();

        }
        [HttpPost("")]
        public async Task<IActionResult>Vote([FromRoute] int pollId, [FromBody] VoteRequest request, CancellationToken cancellationToken)
        {
            var result =await _voteService.AddAsync(pollId,User.GetUserId()!,request,cancellationToken);

            if (result.IsSuccess)
                return Created();
            return result.ToProblem();
        }
    }
}
