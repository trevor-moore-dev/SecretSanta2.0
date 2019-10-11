using SecretSanta2._0.Models.DB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretSanta2._0.Services.Data.Interfaces
{
	public interface ISantaDAO2
	{
		Task<List<Participant>> GetAllParticipants();
		Task<Participant> GetParticipantByName(string name);
		Task<Participant> AddParticipant(Participant participant);
		void UpdateParticipantByName(string name, Participant updatedParticipant);
		void DeleteParticipantByName(string name);
	}
}
