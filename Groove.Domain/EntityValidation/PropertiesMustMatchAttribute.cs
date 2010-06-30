using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Groove.Domain.EntityValidation
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public sealed class PropertiesMustMatchAttribute : ValidationAttribute
	{
		#region Data Members
		private const string _defaultErrorMessage = "'{0}' and '{1}' do not match.";
		private readonly object _typeId = new object();
		#endregion

		#region Constructors
		public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
			: base(_defaultErrorMessage)
		{
			OriginalProperty = originalProperty;
			ConfirmProperty = confirmProperty;
		}
		#endregion

		#region Public Properties
		public string ConfirmProperty { get; private set; }
		public string OriginalProperty { get; private set; }
		#endregion

		#region ValidationAttribute Overrides
		/// <summary>
		/// Gets a unique identifier for this System.Attribute.
		/// </summary>
		public override object TypeId
		{
			get
			{
				return _typeId;
			}
		}

		/// <summary>
		/// Applies formatting to an error message, based on the data field where the
		/// error occurred.
		/// </summary>
		/// <param name="name">The name to include in the formatted message.</param>
		/// <returns>An instance of the formatted error message.</returns>
		public override string FormatErrorMessage(string name)
		{
			return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString, OriginalProperty, ConfirmProperty);
		}

		/// <summary>
		/// Determines whether the specified value of the object is valid.
		/// </summary>
		/// <param name="value">The value of the object to validate.</param>
		/// <returns>true if the specified value is valid; otherwise, false.</returns>
		public override bool IsValid(object value)
		{
			var properties = TypeDescriptor.GetProperties(value);
			object originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
			object confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
			return Object.Equals(originalValue, confirmValue);
		}
		#endregion
	}
}
