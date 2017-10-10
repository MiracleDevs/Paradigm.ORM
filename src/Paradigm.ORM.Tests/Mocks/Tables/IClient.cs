using System;
using System.Collections.Generic;

namespace Paradigm.ORM.Tests.Mocks.Tables
{
	public interface IClient
    {
        #region Properties
	
		int Id { get; }
	
		string Name { get; }

		string ContactName { get; }
	
		string ContactEmail { get; }
	
		string ContactPhone { get; }
	
		int? AddressId { get; }
	
		DateTime? RegistrationDate { get; }
	
		int? LogoFileId { get; }

		string Notes { get; }
	
		decimal HourlyRate { get; }
	
		int CreationUserId { get; }
	
		DateTime CreationDate { get; }

		int ModificationUserId { get; }
	
		DateTime ModificationDate { get; }
	
		IAddress Address { get; }
	
		IReadOnlyCollection<IProject> Projects { get; }

		#endregion
    }
}