using SecretSanta2._0.Enums;
using SecretSanta2._0.Models;
using System.Threading.Tasks;

namespace SecretSanta2._0.Services.Business.Interfaces
{
	public interface ISantaService2
	{
		Task<PresentModel> GetSecretSanta(string name);
		Task<ParticipantsModel> GetParticipants();
		Task<eResponse> JoinTheFun(InputModel user);
	}
}
