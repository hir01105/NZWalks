using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WalkDifficultyController : Controller
	{
		private readonly IWalkDifficultyRepository walkDifficultyRepository;

		private readonly IMapper mapper;

		public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
		{
			this.walkDifficultyRepository = walkDifficultyRepository;
			this.mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllWalkDifficulties()
		{

			var walkDifficulties = await walkDifficultyRepository.GetAllAsync();

			var walkDifficultiesDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficulties);

			return Ok(walkDifficultiesDTO);
		}

		[HttpGet]
		[Route("{id:guid}")]
		[ActionName("GetWalkDifficultyAsync")]
		public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
		{
			var walkDifficulty = await walkDifficultyRepository.GetAsync(id);

			if (walkDifficulty == null)
			{
				return NotFound();
			}

			var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

			return Ok(walkDifficultyDTO);
		}

		[HttpPost]
		public async Task<IActionResult> AddWalkDifficulty(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
		{
			var walkDifficultyDomain = mapper.Map<Models.Domain.WalkDifficulty>(addWalkDifficultyRequest);


			walkDifficultyDomain = await walkDifficultyRepository.AddAsync(walkDifficultyDomain);

			var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

			return CreatedAtAction(nameof(GetWalkDifficultyAsync), new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
		}

		[HttpDelete]
		[Route("{id:guid}")]
		public async Task<IActionResult> DeleteWalkDifficulty(Guid id)
		{
			var walkDifficulty = await walkDifficultyRepository.DeleteAsync(id);

			if (walkDifficulty == null)
			{
				return NotFound();
			}

			var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

			return Ok(walkDifficultyDTO);
		}

		[HttpPut]
		[Route("{id:guid}")]
		public async Task<IActionResult> UpdateWalkDifficuly([FromRoute] Guid id, [FromBody]  Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
		{
			var walkDifficultyDomain = mapper.Map<Models.Domain.WalkDifficulty>(updateWalkDifficultyRequest);

			walkDifficultyDomain = await walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomain);

			if (walkDifficultyDomain == null)
			{
				return NotFound();
			}

			var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);


			return Ok(walkDifficultyDTO);

		}
	}
}

