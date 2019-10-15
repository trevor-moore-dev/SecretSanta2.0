using SecretSanta2._0.Enums;
using SecretSanta2._0.Models.DB;
using SecretSanta2._0.Models.DTO;
using SecretSanta2._0.Services.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SecretSanta2._0.Services.Data
{
	public class SantaDAO : IDAO<Participant, ParticipantDTO>
	{
		private readonly string _conn;

		public SantaDAO(string conn)
		{
			this._conn = conn;
		}

		public async Task<ParticipantDTO> GetAll()
		{
			var participants = new List<Participant>();

			try
			{
				using (SqlConnection connection = new SqlConnection(_conn))
				{
					await connection.OpenAsync();

					using (SqlCommand command = new SqlCommand(@"dbo.[GetParticipants]", connection))
					{
						command.CommandType = CommandType.StoredProcedure;

						using (SqlDataReader reader = await command.ExecuteReaderAsync())
						{
							while (await reader.ReadAsync())
							{
								var participant = new Participant();
								participant.Id = await reader.GetFieldValueAsync<int>(0);
								participant.Name = await reader.GetFieldValueAsync<string>(1);
								participant.Taken = await reader.GetFieldValueAsync<int>(2);
								participant.HaveDrawn = await reader.GetFieldValueAsync<int>(3);
								participant.WishList = await reader.GetFieldValueAsync<string>(4);
								participant.WhoTheyDrew = await reader.GetFieldValueAsync<string>(5);
								participants.Add(participant);
							}
							await reader.CloseAsync();
						}
					}
					await connection.CloseAsync();
				}
			}
			catch (Exception e)
			{
				throw e;
			}

			return new ParticipantDTO
			(
				eResponse.Success,
				participants
			);
		}

		public async Task<ParticipantDTO> Get(string name)
		{
			var participant = new Participant();

			try
			{
				using (SqlConnection connection = new SqlConnection(_conn))
				{
					await connection.OpenAsync();

					using (SqlCommand command = new SqlCommand(@"dbo.[GetParticipant]", connection))
					{
						command.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = name;

						command.CommandType = CommandType.StoredProcedure;

						using (SqlDataReader reader = await command.ExecuteReaderAsync())
						{
							while (await reader.ReadAsync())
							{
								participant.Id = await reader.GetFieldValueAsync<int>(0);
								participant.Name = await reader.GetFieldValueAsync<string>(1);
								participant.Taken = await reader.GetFieldValueAsync<int>(2);
								participant.HaveDrawn = await reader.GetFieldValueAsync<int>(3);
								participant.WishList = await reader.GetFieldValueAsync<string>(4);
								participant.WhoTheyDrew = await reader.GetFieldValueAsync<string>(5);
							}
							await reader.CloseAsync();
						}
					}
					await connection.CloseAsync();
				}
			}
			catch (Exception e)
			{
				throw e;
			}

			return new ParticipantDTO
			(
				eResponse.Success,
				new List<Participant>() 
				{
					participant
				}
			);
		}

		public async Task<ParticipantDTO> Add(Participant participant)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_conn))
				{
					await connection.OpenAsync();

					using (SqlCommand command = new SqlCommand(@"dbo.[AddParticipant]", connection))
					{
						command.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = participant.Name;
						command.Parameters.Add("@Taken", SqlDbType.Int).Value = 0;
						command.Parameters.Add("@Havedrawn", SqlDbType.Int).Value = 0;
						command.Parameters.Add("@Wishlist", SqlDbType.VarChar, -1).Value = participant.WishList;
						command.Parameters.Add("@Secret", SqlDbType.VarChar, 50).Value = "";

						command.CommandType = CommandType.StoredProcedure;

						SqlDataReader reader = await command.ExecuteReaderAsync();

						await reader.CloseAsync();
					}
					await connection.CloseAsync();
				}
			}
			catch (Exception e)
			{
				throw e;
			}

			return new ParticipantDTO
			(
				eResponse.Success,
				new List<Participant>()
				{
					participant
				}
			);
		}

		public async Task<ParticipantDTO> Update(string name, Participant participant)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_conn))
				{
					await connection.OpenAsync();

					using (SqlCommand command = new SqlCommand(@"dbo.[UpdateParticipant]", connection))
					{
						command.Parameters.Add("@Secret", SqlDbType.VarChar, 50).Value = participant.WhoTheyDrew;
						command.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = name;
						command.Parameters.Add("@Taken", SqlDbType.Int).Value = participant.Taken;
						command.Parameters.Add("@Havedrawn", SqlDbType.Int).Value = participant.HaveDrawn;

						command.CommandType = CommandType.StoredProcedure;

						SqlDataReader reader = await command.ExecuteReaderAsync();

						await reader.CloseAsync();
					}
					await connection.CloseAsync();	
				}
			}
			catch (Exception e)
			{
				throw e;
			}

			return new ParticipantDTO
			(
				eResponse.Success,
				new List<Participant>()
				{
					participant
				}
			);
		}

		public async Task<ParticipantDTO> Delete(string name)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_conn))
				{
					await connection.OpenAsync();

					using (SqlCommand command = new SqlCommand(@"dbo.[DeleteParticipant]", connection))
					{
						command.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = name;

						command.CommandType = CommandType.StoredProcedure;

						SqlDataReader reader = await command.ExecuteReaderAsync();

						await reader.CloseAsync();
					}
					await connection.CloseAsync();
				}
			}
			catch (Exception e)
			{
				throw e;
			}

			return new ParticipantDTO
			(
				eResponse.Success,
				new List<Participant>()
			);
		}
	}
}