using System;

namespace Gimnastika.Data
{
	/// <summary>
	/// Summary description for IDataAccessProviderFactory.
	/// </summary>
	public interface IDataProviderFactory
	{
        IDataContext GetDataContext();
	}
}
