using System;

namespace Groove.Domain.Attributes
{
	/// <summary>
	/// Facilitates indicating which property(s) describe the unique signature of an 
	/// entity.  See DomainEntity.GetTypeSpecificSignatureProperties() for when this is leveraged.
	/// </summary>
	[Serializable]
	public class DomainSignatureAttribute : Attribute
	{
	}
}
