namespace Paradigm.ORM.Tests.Mocks.Tables
{
	public interface IAddress
    {
        #region Properties
	
		int Id { get; }
	
		string Address1 { get; }
	
		string Address2 { get; }
	
		int AddressTypeId { get; }
	
		string City { get; }

		string State { get; }
	
		string Country { get; }

		string Email { get; }
	
		string PhoneNumber { get; }
	
		string FaxNumber { get; }
	
		string Attention { get; }
	
		string Contact { get; }
	
		string Notes { get; }

		#endregion
    }
}