using Microsoft.AspNetCore.Authorization;
using Survey_Basket.Abstractions;

namespace Survey_Basket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PollsController(IPollService pollService) : ControllerBase
    {
        private readonly IPollService _pollService = pollService;

        [HttpGet("")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var polls = await _pollService.GetAllAsync(cancellationToken);

            return Ok(polls);
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentAll(CancellationToken cancellationToken)
        {
            var polls = await _pollService.GetCurrentAsync(cancellationToken);

            return Ok(polls);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            var poll = await _pollService.GetAsync(id, cancellationToken);
            if (poll.IsFailure)
                return poll.ToProblem();
            var response = poll.Value.Adapt<PollResponse>();

            return Ok(response);
        }
        [HttpPost("")]
        public async Task<IActionResult> Add([FromBody] PollRequest poll, CancellationToken cancellationToken)
        {
            var newpoll = await _pollService.AddAsync(poll, cancellationToken);


            if (newpoll.IsFailure)
                return newpoll.ToProblem();

            return CreatedAtAction(nameof(Get), new { id = newpoll.Value.Id }, newpoll.Value);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request, CancellationToken cancellationToken)
        {

            var Isupdated = await _pollService.UpdateAsync(id, request, cancellationToken);

            if (Isupdated.IsFailure)
                return Isupdated.ToProblem();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id, CancellationToken cancellationToken)
        {

            var IsDeleted = await _pollService.Delete(id, cancellationToken);

            if (IsDeleted.IsFailure)
                return IsDeleted.ToProblem();

            return NoContent();
        }
        [HttpPut("{id}/togglePublish")]
        public async Task<IActionResult> ToggleStatus([FromRoute] int id, CancellationToken cancellationToken)
        {

            var toggle = await _pollService.ToggleStatusAsync(id, cancellationToken);

            if (toggle.IsFailure)
                return toggle.ToProblem();

            return NoContent();
        }

    }
}
