using System.Collections.Generic;

namespace SecretSanta2._0.Models
{
	public class ParticipantsModel
	{
		public IEnumerable<string> Participants { get; set; } = new List<string>();
	}
}