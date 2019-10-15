using SecretSanta2._0.Enums;
using System.Collections.Generic;

namespace SecretSanta2._0.Models.DTO.Interfaces
{
	public interface IDTO<T>
	{
		eResponse ResponseCode { get; set; }
		List<T> Data { get; set; }
	}
}
