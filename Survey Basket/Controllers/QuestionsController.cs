namespace Survey_Basket.Controllers;

[Route("api/polls/{PollId}/[controller]")]
[ApiController]
[Authorize]
public class QuestionsController(IQuestionService questionService) : ControllerBase
{
    private readonly IQuestionService _questionService = questionService;


    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromRoute] int PollId, [FromQuery] RequestFilters filters, CancellationToken cancellationToken)
    {
        var result = await _questionService.GetAllAsync(PollId, filters, cancellationToken);
        if (result.IsSuccess)
            return Ok(result.Value);

        return result.ToProblem();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _questionService.GetAsync(pollId, id, cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);
        return result.ToProblem();
    }

    [HttpPost("")]
    public async Task<IActionResult> Add([FromRoute] int PollId, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
    {
        var result = await _questionService.AddAsync(PollId, request);

        if (result.IsSuccess)
            return CreatedAtAction(nameof(Get), new { PollId, id = result.Value.Id }, result.Value);

        return result.ToProblem();
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int pollId, [FromRoute] int id, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
    {
        var result = await _questionService.UpdateAsync(pollId, id, request, cancellationToken);

        if (result.IsSuccess)
            return NoContent();

        return result.ToProblem();
    }


    [HttpPut("{id}/toggleStatus")]
    public async Task<IActionResult> ToggleStatus([FromRoute] int PollId, [FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _questionService.ToggleStatusAsync(PollId, id, cancellationToken);
        if (result.IsSuccess)
            return NoContent();
        return result.ToProblem();
    }
}
