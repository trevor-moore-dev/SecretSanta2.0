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
	public class SantaDAO2 : IDAO<Participant2, ParticipantDTO2>
	{
		private readonly IMongoCollection<Participant2> _participants;

		public SantaDAO2(string connectionString, string databaseName, string collectionName)
		{
			try
			{
				var client = new MongoClient(connectionString);
				var database = client.GetDatabase(databaseName);
				_participants = database.GetCollection<Participant2>(collectionName);
			}
			catch (Exception e)
			{
				LoggerHelper.Log(e);
				throw e;
			}
		}

		public async Task<ParticipantDTO2> GetAll()
		{
			try
			{ 
				var participants = await _participants.FindAsync(x => true);
				var participantsList = await participants.ToListAsync();
				return new ParticipantDTO2
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

		public async Task<ParticipantDTO2> Get(string index)
		{
			try
			{
				var participants = await _participants.FindAsync(x => x.Id.Equals(index));
				var participant = await participants.FirstOrDefaultAsync();
				return new ParticipantDTO2
				(
					eResponse.Success,
					new List<Participant2>()
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

		public async Task<ParticipantDTO2> Add(Participant2 participant)
		{
			try
			{ 
				await _participants.InsertOneAsync(participant);
				return new ParticipantDTO2
				(
					eResponse.Success,
					new List<Participant2>()
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

		public async Task<ParticipantDTO2> Update(string index, Participant2 participant)
		{
			try
			{
				await _participants.ReplaceOneAsync(x => x.Id.Equals(index), participant);
				return new ParticipantDTO2
				(
					eResponse.Success,
					new List<Participant2>()
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

		public async Task<ParticipantDTO2> Delete(string index)
		{
			try
			{
				await _participants.DeleteOneAsync(x => x.Id.Equals(index));
				return new ParticipantDTO2
				(
					eResponse.Success,
					new List<Participant2>()
				);
			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}