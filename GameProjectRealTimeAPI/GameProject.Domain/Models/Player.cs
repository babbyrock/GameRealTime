namespace GameProject.Domain.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public TimeSpan AccumulatedTime { get; set; } = TimeSpan.Zero;
    }
}
