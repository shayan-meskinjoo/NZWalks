using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
	public class SQLRegionRepository : IRegionRepository
	{
		private readonly NZWalksDbContext _dbContext;

		public SQLRegionRepository(NZWalksDbContext dbContext)
        {
			this._dbContext = dbContext;
		}

		public async Task<Region> CreateRegionAsync(Region region)
		{
			await _dbContext.AddAsync(region);
			await _dbContext.SaveChangesAsync();
			return region;
		}

		public async Task<Region?> DeleteRegionAsync(Guid id)
		{
			var regionDomainModel = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
			if (regionDomainModel == null) 
			{
				return null;
			}

			_dbContext.Regions.Remove(regionDomainModel);
			await _dbContext.SaveChangesAsync();

			return regionDomainModel;
		}

		public async Task<List<Region>> GetAllAsync()
		{
			return await _dbContext.Regions.ToListAsync();
		}

		public async Task<Region?> GetByIdAsync(Guid id)
		{
			return await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<Region?> UpdateRegionAsync(Guid id, Region updatedRegion)
		{
			var regionDomainModel = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

			if(regionDomainModel == null) 
			{
				return null;
			}

			regionDomainModel.Code = updatedRegion.Code;
			regionDomainModel.Name = updatedRegion.Name;
			regionDomainModel.RegionImageUrl = updatedRegion.RegionImageUrl;

			await _dbContext.SaveChangesAsync();

			return regionDomainModel;
		}
	}
}
