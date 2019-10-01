using SecretSanta2._0.Enums;
using SecretSanta2._0.Models;
using SecretSanta2._0.Services.Data;
using System;
using System.Threading.Tasks;

namespace SecretSanta2._0.Services.Business
{
	public class SantaService : ISantaService
	{
		private readonly ISantaDAO _santaDAO;

		public SantaService(ISantaDAO santaDAO)
		{
			this._santaDAO = santaDAO;
		}

		public async Task<PresentModel> GetSecretSanta(string participantName)
		{
			if (participantName.Equals("Please Select Your Name"))
			{
				return new PresentModel()
				{
					Title = "Oops!",
					PageDescription = "Looks like you forgot to select your name. Please return to the previous page and select your name from the dropdown before continuing.",
					HeaderOne = "",
					Name = "",
					HeaderTwo = "",
					WishList = "",
					Response = eResponse.Failure
				};
			}

			try
			{
				if (await _santaDAO.GetNumberOfParticipants() <= 1)
				{
					return new PresentModel()
					{
						Title = "Oops!",
						PageDescription = "Looks like you're the only one in the drawing pool. Make sure to come back soon to check and see if anyone else has joined the fun!",
						HeaderOne = "",
						Name = "",
						HeaderTwo = "",
						WishList = "",
						Response = eResponse.Failure
					};
				}

				if (await _santaDAO.DoesParticipantExist(participantName) == 0)
				{
					return new PresentModel()
					{
						Title = "Oops!",
						PageDescription = "Looks like your name doesn't exist. Please return to the previous page and select a name from the dropdown.",
						HeaderOne = "",
						Name = "",
						HeaderTwo = "",
						WishList = "",
						Response = eResponse.Failure
					};
				}

				if (await _santaDAO.HasParticipantDrawn(participantName) == 1)
				{
					return new PresentModel()
					{
						Title = "Sorry, you have already drawn someone.",
						PageDescription = "If you can't remember who you drew, please text the system admin at (602) 810-3667.",
						HeaderOne = "",
						Name = "",
						HeaderTwo = "",
						WishList = "",
						Response = eResponse.Failure
					};
				}

				var secret = await _santaDAO.GetRandomParticipant(participantName);

				if (string.IsNullOrEmpty(secret.Name))
				{
					return new PresentModel()
					{
						Title = "Sorry, all names have already been drawn.",
						PageDescription = "If this a mistake and you didn't get to draw someone, please text (602) 810-3667.",
						HeaderOne = "",
						Name = "",
						HeaderTwo = "",
						WishList = "",
						Response = eResponse.Failure
					};
				}

				_santaDAO.SetTakenParticipant(secret.Name);
				_santaDAO.SetParticipantDrawFlag(participantName);
				_santaDAO.SaveDrawnParticipant(secret.Name, participantName);
				secret.Response = eResponse.Success;
				return secret;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<ParticipantsModel> GetParticipants()
		{
			try
			{ 
				return await _santaDAO.GetParticipants();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<eResponse> JoinTheFun(InputModel user)
		{
			try
			{
				if (await _santaDAO.DoesParticipantExist(user.Name) == 0)
				{
					_santaDAO.AddParticipant(user);
					return eResponse.Success;
				}
				else
				{
					return eResponse.Failure;
				}
			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}