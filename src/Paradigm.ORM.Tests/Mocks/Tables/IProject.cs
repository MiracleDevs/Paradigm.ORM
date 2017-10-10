using System;

namespace Paradigm.ORM.Tests.Mocks.Tables
{
	public interface IProject
    {
        #region Properties
	
		int Id { get; }
	
		string Name { get; }

		DateTime RegistrationDate { get; }
	
		int ClientId { get; }

		bool Active { get; }

		string Notes { get; }
	
		int CreationUserId { get; }
	
		DateTime CreationDate { get; }
	
		int ModificationUserId { get; }

		DateTime ModificationDate { get; }

		#endregion
    }
}