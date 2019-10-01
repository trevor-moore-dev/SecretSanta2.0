using SecretSanta2._0.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SecretSanta2._0.Services.Data
{
	public class SantaDAO : ISantaDAO
	{
		private readonly string _conn;

		public SantaDAO(string conn)
		{
			this._conn = conn;
		}

		public async Task<ParticipantsModel> GetParticipants()
		{
			var model = new ParticipantsModel();
			var users = new List<string>();

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
								users.Add(await reader.GetFieldValueAsync<string>(0));
							}
							reader.Close();
						}
					}
					connection.Close();
				}
				model.Participants = users;
			}
			catch (Exception e)
			{
				throw e;
			}

			return model;
		}

		public async void AddParticipant(InputModel user)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_conn))
				{
					await connection.OpenAsync();

					using (SqlCommand command = new SqlCommand(@"dbo.[AddParticipant]", connection))
					{
						command.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = user.Name;
						command.Parameters.Add("@Taken", SqlDbType.Int).Value = 0;
						command.Parameters.Add("@Havedrawn", SqlDbType.Int).Value = 0;
						command.Parameters.Add("@Wishlist", SqlDbType.VarChar, -1).Value = user.Wishlist;

						command.CommandType = CommandType.StoredProcedure;

						SqlDataReader reader = await command.ExecuteReaderAsync();

						reader.Close();
					}
					connection.Close();
				}
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<int> DoesParticipantExist(string participantName)
		{
			var userExists = 0;

			try
			{
				using (SqlConnection connection = new SqlConnection(_conn))
				{
					await connection.OpenAsync();

					using (SqlCommand command = new SqlCommand(@"dbo.[DoesParticipantExist]", connection))
					{
						command.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = participantName;

						command.CommandType = CommandType.StoredProcedure;

						userExists = (int) await command.ExecuteScalarAsync();

					}
					connection.Close();
				}
			}
			catch (Exception e)
			{
				throw e;
			}

			return userExists;
		}

		public async Task<int> GetNumberOfParticipants()
		{
			var numRows = 0;

			try
			{
				using (SqlConnection connection = new SqlConnection(_conn))
				{
					await connection.OpenAsync();

					using (SqlCommand command = new SqlCommand(@"dbo.[GetNumberOfParticipants]", connection))
					{
						command.CommandType = CommandType.StoredProcedure;

						numRows = (int)await command.ExecuteScalarAsync();

					}
					connection.Close();
				}
			}
			catch (Exception e)
			{
				throw e;
			}

			return numRows;
		}

		public async Task<int> HasParticipantDrawn(string participantName)
		{
			var hasDrawn = 1;

			try
			{
				using (SqlConnection connection = new SqlConnection(_conn))
				{
					await connection.OpenAsync();

					using (SqlCommand command = new SqlCommand(@"dbo.[HasParticipantDrawn]", connection))
					{
						command.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = participantName;

						command.CommandType = CommandType.StoredProcedure;

						hasDrawn = (int) await command.ExecuteScalarAsync();
					}
					connection.Close();
				}
			}
			catch (Exception e)
			{
				throw e;
			}

			return hasDrawn;
		}

		public async Task<PresentModel> GetRandomParticipant(string participantName)
		{
			var secret = new PresentModel();

			try
			{
				using (SqlConnection connection = new SqlConnection(_conn))
				{
					await connection.OpenAsync();

					using (SqlCommand command = new SqlCommand(@"dbo.[GetRandomParticipant]", connection))
					{
						command.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = participantName;

						command.CommandType = CommandType.StoredProcedure;

						using (SqlDataReader reader = await command.ExecuteReaderAsync())
						{
							while (await reader.ReadAsync())
							{
								secret.Name = await reader.GetFieldValueAsync<string>(0);
								secret.WishList = await reader.GetFieldValueAsync<string>(1);
							}
							reader.Close();
						}
					}
					connection.Close();
				}
			}
			catch (Exception e)
			{
				throw e;
			}

			return secret;
		}

		public async void SetTakenParticipant(string takenParticipantName)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_conn))
				{
					await connection.OpenAsync();

					using (SqlCommand command = new SqlCommand(@"dbo.[SetTakenParticipant]", connection))
					{
						command.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = takenParticipantName;

						command.CommandType = CommandType.StoredProcedure;

						SqlDataReader reader = await command.ExecuteReaderAsync();

						reader.Close();
					}
					connection.Close();
				}
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async void SetParticipantDrawFlag(string participantName)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_conn))
				{
					await connection.OpenAsync();

					using (SqlCommand command = new SqlCommand(@"dbo.[SetParticipantDrawFlag]", connection))
					{
						command.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = participantName;

						command.CommandType = CommandType.StoredProcedure;

						SqlDataReader reader = await command.ExecuteReaderAsync();

						reader.Close();
					}
					connection.Close();
				}
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async void SaveDrawnParticipant(string takenParticipantName, string participantName)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_conn))
				{
					await connection.OpenAsync();

					using (SqlCommand command = new SqlCommand(@"dbo.[SaveDrawnParticipant]", connection))
					{
						command.Parameters.Add("@Secret", SqlDbType.VarChar, 50).Value = takenParticipantName;
						command.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = participantName;

						command.CommandType = CommandType.StoredProcedure;

						SqlDataReader reader = await command.ExecuteReaderAsync();

						reader.Close();
					}
					connection.Close();
				}
			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}