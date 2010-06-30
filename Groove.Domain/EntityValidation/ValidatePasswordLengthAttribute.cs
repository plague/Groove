using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Groove.Domain.EntityValidation
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
	{
		#region Data Members
		private const string _defaultErrorMessage = "'{0}' must be at least {1} characters long.";
		#endregion

		#region Constructors
		public ValidatePasswordLengthAttribute(int minCharacters)
			: base(_defaultErrorMessage)
		{
			MinCharacters = minCharacters;
		}
		#endregion

		#region Public Properties
		public int MinCharacters { get; private set; }
		#endregion

		#region ValidationAttribute Overrides
		/// <summary>
		/// Applies formatting to an error message, based on the data field where the
		/// error occurred.
		/// </summary>
		/// <param name="name">The name to include in the formatted message.</param>
		/// <returns>An instance of the formatted error message.</returns>
		public override string FormatErrorMessage(string name)
		{
			return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
				name, MinCharacters);
		}

		/// <summary>
		/// Determines whether the specified value of the object is valid.
		/// </summary>
		/// <param name="value">The value of the object to validate.</param>
		/// <returns>true if the specified value is valid; otherwise, false.</returns>
		public override bool IsValid(object value)
		{
			string valueAsString = value as string;
			return (valueAsString != null && valueAsString.Length >= MinCharacters);
		}
		#endregion
	}
}
