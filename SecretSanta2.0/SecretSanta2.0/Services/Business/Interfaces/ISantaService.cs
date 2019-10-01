using SecretSanta2._0.Enums;
using SecretSanta2._0.Models;
using System.Threading.Tasks;

namespace SecretSanta2._0.Services.Business
{
	public interface ISantaService
	{
		Task<PresentModel> GetSecretSanta(string name);
		Task<ParticipantsModel> GetParticipants();
		Task<eResponse> JoinTheFun(InputModel user);
	}
}
