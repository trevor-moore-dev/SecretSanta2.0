using MongoDB.Driver;
using SecretSanta2._0.Enums;
using SecretSanta2._0.Helpers;
using SecretSanta2._0.Models.DB;
using SecretSanta2._0.Models.DTO;
using SecretSanta2._0.Services.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretSanta2._0.Services.Data
{
	public class SantaDAO : IDAO<Participant, ParticipantDTO>
	{
		private readonly IMongoCollection<Participant> _participants;

		public SantaDAO(string connectionString, string databaseName, string collectionName)
		{
			try
			{
				var client = new MongoClient(connectionString);
				var database = client.GetDatabase(databaseName);
				_participants = database.GetCollection<Participant>(collectionName);
			}
			catch (Exception e)
			{
				LoggerHelper.Log(e);
				throw e;
			}
		}

		public async Task<ParticipantDTO> GetAll()
		{
			try
			{ 
				var participants = await _participants.FindAsync(x => true);
				var participantsList = await participants.ToListAsync();
				return new ParticipantDTO
				(
					eResponse.Success,
					participantsList
				); 
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<ParticipantDTO> Get(string index)
		{
			try
			{
				var participants = await _participants.FindAsync(x => x.Id.Equals(index));
				var participant = await participants.FirstOrDefaultAsync();
				return new ParticipantDTO
				(
					eResponse.Success,
					new List<Participant>()
					{
						participant
					}
				);
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<ParticipantDTO> Add(Participant participant)
		{
			try
			{ 
				await _participants.InsertOneAsync(participant);
				return new ParticipantDTO
				(
					eResponse.Success,
					new List<Participant>()
					{
						participant
					}
				);
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<ParticipantDTO> Update(string index, Participant participant)
		{
			try
			{
				await _participants.ReplaceOneAsync(x => x.Id.Equals(index), participant);
				return new ParticipantDTO
				(
					eResponse.Success,
					new List<Participant>()
					{
						participant
					}
				);
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<ParticipantDTO> Delete(string index)
		{
			try
			{
				await _participants.DeleteOneAsync(x => x.Id.Equals(index));
				return new ParticipantDTO
				(
					eResponse.Success,
					new List<Participant>()
				);
			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}