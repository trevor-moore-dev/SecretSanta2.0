using Microsoft.AspNetCore.Http;
using SecretSanta2._0.Models;
using System;
using System.Linq;

namespace SecretSanta2._0.Helpers
{
	public class CookieHelper
	{
		public static void AddCookie(HttpResponse response, string key, string value, int? expireTime = null)
		{
			response.Cookies.Append
			(
				key,
				value,
				new CookieOptions()
				{
					Expires = expireTime.HasValue ? DateTime.UtcNow.AddMinutes(expireTime.Value) : DateTime.UtcNow.AddMonths(1),
					IsEssential = true,
				}
			);
		}
	}
}
