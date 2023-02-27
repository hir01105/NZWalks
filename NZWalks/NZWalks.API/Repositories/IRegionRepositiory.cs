using System;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
	public interface IRegionRepositiory
	{
		Task<IEnumerable<Region>> GetAllAsync();
	}
}

