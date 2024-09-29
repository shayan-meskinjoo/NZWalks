﻿using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
	public class SQLWalkRepository : IWalkRepository
	{
		private readonly NZWalksDbContext _dbContext;

		public SQLWalkRepository(NZWalksDbContext dbContext)
        {
			this._dbContext = dbContext;
		}

        public async Task<Walk> CreateWalkAsync(Walk walk)
		{
			await _dbContext.Walks.AddAsync(walk);
			await _dbContext.SaveChangesAsync();
			return walk;
		}

		public async Task<Walk?> DeletWalkAsync(Guid id)
		{
			var existingDomainModel = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

			if (existingDomainModel == null)
			{
				return null;
			}

			_dbContext.Walks.Remove(existingDomainModel);
			await _dbContext.SaveChangesAsync();
			return existingDomainModel;
		}

		public async Task<List<Walk>> GetAllWalksAsync(string? filterOn = null, string? filterQuery = null,
			string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 5)
		{
			var walks = _dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

			if(string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
			{
				if(filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
				{
					walks = walks.Where(x => x.Name.Contains(filterQuery));
				}
				else if (filterOn.Equals("Length", StringComparison.OrdinalIgnoreCase))
				{
					walks = walks.Where(x => x.LengthInKm.ToString() == filterQuery);
				}
			}

			if(string.IsNullOrWhiteSpace(sortBy) == false)
			{
				if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
				{
					walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
				}
				else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
				{
					walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
				}
			}

			var skipResult = (pageNumber - 1) * pageSize;
			return await walks.Skip(skipResult).Take(pageSize).ToListAsync();
			//return await _dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
		}

		public async Task<Walk?> GetWalkByIdAsync(Guid id)
		{
			var existingDomainModel = await _dbContext.Walks
				.Include("Difficulty").Include("Region")
				.FirstOrDefaultAsync(x => x.Id == id);

			if(existingDomainModel == null)
			{
				return null;
			}

			return existingDomainModel;
		}

		public async Task<Walk?> UpdateWalkAsync(Guid id, Walk walk)
		{
			var existingDomainModel = await _dbContext.Walks
				.Include("Difficulty").Include("Region")
				.FirstOrDefaultAsync(x => x.Id == id);

			if( existingDomainModel == null)
			{
				return null;
			}

			existingDomainModel.Name = walk.Name;
			existingDomainModel.Description = walk.Description;
			existingDomainModel.LengthInKm = walk.LengthInKm;
			existingDomainModel.WalkImageUrl = walk.WalkImageUrl;
			existingDomainModel.DifficultyId = walk.DifficultyId;
			existingDomainModel.RegionId = walk.RegionId;

			await _dbContext.SaveChangesAsync();

			return existingDomainModel;
		}
	}
}
