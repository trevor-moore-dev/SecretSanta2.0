using SecretSanta2._0.Enums;
using SecretSanta2._0.Models.DB;
using SecretSanta2._0.Models.DTO.Interfaces;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SecretSanta2._0.Models.DTO
{
	[DataContract]
	public class ParticipantDTO2 : IDTO<Participant2>
	{
		public ParticipantDTO2(eResponse ResponseCode, List<Participant2> Data)
		{
			this.ResponseCode = ResponseCode;
			this.Data = Data;
		}
		[DataMember]
		public eResponse ResponseCode { get; set; }
		[DataMember]
		public List<Participant2> Data { get; set; }
	}
}
