using SecretSanta2._0.Models;
using System.Threading.Tasks;

namespace SecretSanta2._0.Services.Data.Interfaces
{
	public interface ISantaDAO
	{
		Task<ParticipantsModel> GetParticipants();
		void AddParticipant(InputModel user);
		Task<int> DoesParticipantExist(string participantName);
		Task<int> GetNumberOfParticipants();
		Task<int> HasParticipantDrawn(string participantName);
		Task<PresentModel> GetRandomParticipant(string participantName);
		void SetTakenParticipant(string takenParticipantName);
		void SetParticipantDrawFlag(string participantName);
		void SaveDrawnParticipant(string takenParticipantName, string participantName);
	}
}
