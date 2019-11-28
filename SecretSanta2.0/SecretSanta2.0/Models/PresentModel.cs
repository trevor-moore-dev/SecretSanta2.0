using SecretSanta2._0.Enums;

namespace SecretSanta2._0.Models
{
	public class PresentModel
	{
		public string Name { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;
		public string PageDescription { get; set; } = string.Empty;
		public string Header { get; set; } = string.Empty;
		public eResponse Response { get; set; } = eResponse.Nothing;
	}
}