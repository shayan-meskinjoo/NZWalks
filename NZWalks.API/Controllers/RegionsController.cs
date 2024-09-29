using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
	public class RegionsController : ControllerBase
	{
		private readonly IRegionRepository _regionRepository;
		private readonly IMapper mapper;

		public RegionsController(IRegionRepository regionRepository, IMapper mapper)
		{
			this._regionRepository = regionRepository;
			this.mapper = mapper;
		}


		[HttpGet]
		[Authorize(Roles = "Reader,Writer")]
		public async Task<IActionResult> GetAll()
		{
			var regions = await _regionRepository.GetAllAsync();
			var regionsDTOs = mapper.Map<List<RegionDTO>>(regions);

			return Ok(regionsDTOs);
		}

		[HttpGet]
		[Route("{id}")]
		[Authorize(Roles = "Reader,Writer")]
		public async Task<IActionResult> GetById([FromRoute] Guid id)
		{
			var region = await _regionRepository.GetByIdAsync(id);

			if (region == null)
			{
				return NotFound();
			}
			var regionDTO = mapper.Map<RegionDTO>(region);

			return Ok(regionDTO);
		}

		[HttpPost]
		[ValidateModel]
		[Authorize(Roles = "Writer")]
		public async Task<IActionResult> CreateRegion([FromBody] AddRegionDTO addRegionDTO)
		{
			var regionDomainModel = mapper.Map<Region>(addRegionDTO);

			regionDomainModel = await _regionRepository.CreateRegionAsync(regionDomainModel);

			var regionDTO = mapper.Map<RegionDTO>(regionDomainModel);

			return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDTO);
		}

		[HttpPut]
		[Route("{id}")]
		[ValidateModel]
		[Authorize(Roles = "Writer")]
		public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, UpdateRegionDTO updateRegionDTO)
		{
			var regionDomainModel = mapper.Map<Region>(updateRegionDTO);

			regionDomainModel = await _regionRepository.UpdateRegionAsync(id, regionDomainModel);

			if (regionDomainModel == null)
			{
				return NotFound();
			}
			var regionDTO = mapper.Map<RegionDTO>(regionDomainModel);

			return Ok(regionDTO);
		}

		[HttpDelete]
		[Route("{id}")]
		[Authorize(Roles = "Writer")]
		public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
		{
			var regionDomainModel = await _regionRepository.DeleteRegionAsync(id);

			if ( regionDomainModel == null)
			{
				return NotFound();
			}

			return Ok();
		}
	}
}
