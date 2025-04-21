namespace Api_Laia_T1.PR2.APIrest.Models.DTOs
{
    public class GameDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Desenv { get; set; }
        public string Img { get; set; }
        public List<string> Users { get; set; } = new List<string>();
    }
}
