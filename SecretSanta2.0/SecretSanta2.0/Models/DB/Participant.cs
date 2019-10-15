namespace SecretSanta2._0.Models.DB
{
	public class Participant
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Taken { get; set; }
		public int HaveDrawn { get; set; }
		public string WishList { get; set; }
		public string WhoTheyDrew { get; set; }
	}
}
