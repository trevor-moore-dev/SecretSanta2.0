﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SecretSanta2._0.Models.DB
{
	public class Participant
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		public string Name { get; set; }
		public decimal Taken { get; set; }
		public string HaveDrawn { get; set; }
		public string WishList { get; set; }
		public string WhoTheyDrew { get; set; }
	}
}
