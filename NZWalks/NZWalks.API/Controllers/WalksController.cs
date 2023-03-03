using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WalksController : Controller
	{
		private readonly IWalkRepository walkRepository;

		private readonly IMapper mapper;

		public WalksController(IWalkRepository walkRepository, IMapper mapper)
		{
			this.walkRepository = walkRepository;
			this.mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllWalksAsync()
		{
			// Fetch data from database - domain walks
			var walks = await walkRepository.GetAllAsync();

			// Convert domain walks to DTO Walks
			var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walks);

			return Ok(walksDTO);
		}

		[HttpGet]
		[Route("{id:guid}")]
		[ActionName("GetWalkAsync")]
		public async Task<IActionResult> GetWalkAsync([FromRoute] Guid id)
		{
			var walk = await walkRepository.GetAsync(id);

			if (walk == null)
			{
				return NotFound();
			}

			var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

			return Ok(walkDTO);
		}

		[HttpPost]
		public async Task<IActionResult> AddWalkAsync([FromBody] AddWalkRequest addWalkRequest)
		{
			// Convert GTO to Domain Object
			var walkDomain = mapper.Map<Models.Domain.Walk>(addWalkRequest);

			// Pass domain object to Repository to persist this
			walkDomain = await walkRepository.AddAsync(walkDomain);

			// Convert the Domain object back to DTO
			var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

			// Send DTO response back to Client
			return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);

		}

		[HttpPut]
		[Route("{id:guid}")]
		public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] UpdateWalkRequest updateWalkRequest)
		{
			// Convert DTO to Domain object
			var walkDomain = mapper.Map<Models.Domain.Walk>(updateWalkRequest);

			// Pass details to Repository - Get Domain object in response (or null)
			walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

			// Handle Null (not found)
			if (walkDomain == null)
			{
				return NotFound();
			}

			// Convert back Domain to DTO
			var walkDTO = mapper.Map<Walk>(walkDomain);

			// Return Response
			return Ok(walkDTO);

        }

		[HttpDelete]
		[Route("{id:guid}")]
		public async Task<IActionResult> DeleteWalkAsync(Guid id)
		{
			// call repository to delete walk
			var walkDomain = await walkRepository.DeleteAsync(id);

			if (walkDomain == null)
			{
				return NotFound();
			}

			var walkDTO = mapper.Map<Walk>(walkDomain);
			return Ok(walkDTO);
		}

    }
}

