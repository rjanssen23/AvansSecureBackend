namespace ProjectMapGroepsproject.WebApi.Models
{
    public class Progressie1
    {
        public Guid Id { get; set; }

        public int NumberCompleet { get; set; }
        public bool vakje1 { get; set; }
        public bool vakje2 { get; set; }
        public bool vakje3 { get; set; }
        public bool vakje4 { get; set; }
        public bool vakje5{ get; set; }
        public bool vakje6 { get; set; }
        public Guid ProfielKeuzeId { get; set; } 
    }
}
