using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
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
		public async Task<IActionResult> AddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
		{
			if (!ValidateAddWalkDifficultyAsync(addWalkDifficultyRequest))
			{
				return BadRequest(ModelState);
			}

			var walkDifficultyDomain = mapper.Map<Models.Domain.WalkDifficulty>(addWalkDifficultyRequest);


			walkDifficultyDomain = await walkDifficultyRepository.AddAsync(walkDifficultyDomain);

			var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

			return CreatedAtAction(nameof(GetWalkDifficultyAsync), new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
		}

		[HttpDelete]
		[Route("{id:guid}")]
		public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
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
		public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody]  Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
		{
			if (!ValidateUpdateWalkDifficultyAsync(updateWalkDifficultyRequest))
			{
				return BadRequest(ModelState);
			}

			var walkDifficultyDomain = mapper.Map<Models.Domain.WalkDifficulty>(updateWalkDifficultyRequest);

			walkDifficultyDomain = await walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomain);

			if (walkDifficultyDomain == null)
			{
				return NotFound();
			}

			var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);


			return Ok(walkDifficultyDTO);

		}

		#region Private methods
		private bool ValidateAddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
		{
			if (addWalkDifficultyRequest == null)
			{
				ModelState.AddModelError(nameof(addWalkDifficultyRequest),
					$"{addWalkDifficultyRequest} is required.");
				return false;
			}

			if (!string.IsNullOrWhiteSpace(addWalkDifficultyRequest.Code))
			{
				ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code),
					$"{addWalkDifficultyRequest.Code} is invalid.");

				return false;
			}

			return true;
		}

		private bool ValidateUpdateWalkDifficultyAsync(Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
		{
            if (updateWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest),
                    $"{updateWalkDifficultyRequest} is required.");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(updateWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code),
                    $"{updateWalkDifficultyRequest.Code} is invalid.");

                return false;
            }

            return true;
        }
		#endregion
	}
}

