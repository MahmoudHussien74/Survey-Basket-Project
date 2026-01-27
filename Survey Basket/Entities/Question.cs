namespace Survey_Basket.Entities
{
    public class Question:AuditableEntity
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int PollId { get; set; }
        public bool IsActive { get; set; } = true;
        public Poll Poll { get; set; }

        public ICollection<Answer> Answers { get; set; } = [];
    }
}
