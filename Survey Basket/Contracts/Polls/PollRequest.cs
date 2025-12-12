namespace Survey_Basket.Contracts
{
    public record PollRequest(
        string Title,
        string Summary,
        DateOnly StartsAt,
        DateOnly EndsAt
    );
}
