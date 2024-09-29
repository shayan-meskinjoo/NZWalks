using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WalksController : ControllerBase
	{
		private readonly IMapper mapper;
		private readonly IWalkRepository _walkRepository;

		public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
			this.mapper = mapper;
			this._walkRepository = walkRepository;
		}

		[HttpPost]
		[ValidateModel]
		public async Task<IActionResult> CreateWalk([FromBody] AddWalkDTO addWalkDTO )
		{
			var walkDomainModel = mapper.Map<Walk>(addWalkDTO);

			walkDomainModel = await _walkRepository.CreateWalkAsync(walkDomainModel);

			var walkDTO = mapper.Map<WalkDTO>(walkDomainModel);
			return Ok(walkDTO);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllWalks([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
			[FromQuery] string? sortBy, [FromQuery] bool? isAscending, int pageNumber = 1, int pageSize = 5)
		{
			var walksDomainModel = await _walkRepository.GetAllWalksAsync(filterOn, filterQuery,
				sortBy, isAscending ?? true, pageNumber, pageSize);

			var walksDTO = mapper.Map<List<WalkDTO>>(walksDomainModel);

			return Ok(walksDTO);
		}

		[HttpGet]
		[Route("{id}")]
		public async Task<IActionResult> GetWalkById([FromRoute] Guid id)
		{
			var walkDomainModel = await _walkRepository.GetWalkByIdAsync(id);

			if(walkDomainModel == null)
			{
				return NotFound();
			}

			var walkDTO = mapper.Map<WalkDTO>(walkDomainModel);
			return Ok(walkDTO);
		}

		[HttpPut]
		[Route("{id}")]
		[ValidateModel]
		public async Task<IActionResult> UpdateWalk([FromRoute] Guid id, [FromBody] UpdateWalkDTO updateWalkDTO)
		{
			var walkDomainModel = mapper.Map<Walk>(updateWalkDTO);

			walkDomainModel = await _walkRepository.UpdateWalkAsync(id, walkDomainModel);

			if (walkDomainModel == null)
			{
				return NotFound();
			}

			var walkDTO = mapper.Map<WalkDTO>(walkDomainModel);
			return Ok(walkDTO);
		}

		[HttpDelete]
		[Route("{id}")]
		public async Task<IActionResult> DeleteWalk([FromRoute] Guid id)
		{
			var walkDomainModel = await _walkRepository.DeletWalkAsync(id);
			 if (walkDomainModel == null)
			 {
				return NotFound();
			 }

			return Ok();
		}
    }
}
