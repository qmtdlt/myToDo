namespace todo.Api.Domain.DTO
{
    public record ToDoDTO
    {
        public int number { get; set; }
        public string text { get; set; }
    }
}
