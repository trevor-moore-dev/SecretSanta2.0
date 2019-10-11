using MongoDB.Driver;
using SecretSanta2._0.Models.DB;
using SecretSanta2._0.Services.Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretSanta2._0.Services.Data
{
	public class SantaDAO2 : ISantaDAO2
	{
		private readonly IMongoCollection<Participant> _participants;

		public SantaDAO2(string connectionString, string databaseName, string collectionName)
		{
			var client = new MongoClient(connectionString);
			var database = client.GetDatabase(databaseName);
			_participants = database.GetCollection<Participant>(collectionName);
		}

		public async Task<List<Participant>> GetAllParticipants()
		{
			var participants = await _participants.FindAsync(participant => true);
			return await participants.ToListAsync();
		}

		public async Task<Participant> GetParticipantByName(string name)
		{
			var participants = await _participants.FindAsync(participant => participant.Name == name);
			return await participants.FirstOrDefaultAsync();
		}

		public async Task<Participant> AddParticipant(Participant participant)
		{
			await _participants.InsertOneAsync(participant);
			return participant;
		}

		public async void UpdateParticipantByName(string name, Participant updatedParticipant)
		{
			await _participants.ReplaceOneAsync(participant => participant.Name == name, updatedParticipant);
		}

		public async void DeleteParticipantByName(string name)
		{
			await _participants.DeleteOneAsync(participant => participant.Name == name);
		}
	}
}
