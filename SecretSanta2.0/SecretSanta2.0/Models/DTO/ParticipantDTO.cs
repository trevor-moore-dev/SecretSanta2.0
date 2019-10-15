using SecretSanta2._0.Enums;
using SecretSanta2._0.Models.DB;
using SecretSanta2._0.Models.DTO.Interfaces;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SecretSanta2._0.Models.DTO
{
	[DataContract]
	public class ParticipantDTO : IDTO<Participant>
	{
		public ParticipantDTO(eResponse ResponseCode, List<Participant> Data)
		{
			this.ResponseCode = ResponseCode;
			this.Data = Data;
		}
		[DataMember]
		public eResponse ResponseCode { get; set; }
		[DataMember]
		public List<Participant> Data { get; set; }
	}
}
