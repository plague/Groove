using System;
using System.Collections.Generic;

namespace Groove.Domain.Entities
{
	/// <summary>
	/// Facilitates comparing identity values with null and default for type.
	/// </summary>
	public static class DomainEntityIdentityComparer
	{
		#region Public Static Methods
		/// <summary>
		/// Checks whether the value is equal to the default for that type.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="value">The value to check</param>
		/// <returns>true if the value is equal to the default for it's type; otherwise, false.</returns>
		public static bool IsNullOrDefault<T>(this T value)
		{
			if (typeof(T) == typeof(String))
			{
				var stringValue = value as String;
				return String.IsNullOrEmpty(stringValue);
			}

			return EqualityComparer<T>.Default.Equals(value, default(T));
		}
		#endregion
	}
}
