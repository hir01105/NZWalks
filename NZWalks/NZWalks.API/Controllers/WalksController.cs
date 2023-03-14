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

		private readonly IRegionRepository regionRepository;

		private readonly IWalkDifficultyRepository walkDifficultyRepository;

		private readonly IMapper mapper;

		public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
		{
			this.walkRepository = walkRepository;
			this.regionRepository = regionRepository;
			this.walkDifficultyRepository = walkDifficultyRepository;
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
			// Validate the incoming request
			if (!(await ValidateAddWalkAsync(addWalkRequest))) {
				return BadRequest(ModelState);
			}

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
            // Validate the incoming request
            if (!(await ValidateUpdateWalkAsync(updateWalkRequest)))
            {
				return BadRequest(ModelState);
            }

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


		#region Private methods

		private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
		{
			//if (addWalkRequest == null)
			//{
			//	ModelState.AddModelError(nameof(addWalkRequest), $"{nameof(addWalkRequest)} cannot be empty.");
			//	return false;
			//}

			//if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
			//{
			//	ModelState.AddModelError(nameof(addWalkRequest.Name), $"{nameof(addWalkRequest.Name)} is required.");
			//}

			//if (addWalkRequest.Length <= 0)
			//{
			//  ModelState.AddModelError(nameof(addWalkRequest.Length), $"{nameof(addWalkRequest.Length)} should be greater than zero.");
			//}


			var region = await regionRepository.GetAsync(addWalkRequest.RegionId);

			if (region == null)
			{
				ModelState.AddModelError(nameof(addWalkRequest.RegionId),
					$"{nameof(addWalkRequest.RegionId)} is invalid.");
			}


			var walkDifficulty = await walkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);

			if (walkDifficulty == null)
			{
				ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId),
					$"{nameof(addWalkRequest.WalkDifficultyId)} is invalid.");
			}

			if (ModelState.ErrorCount > 0)
			{
				return false;
			}

			return true;

        }

		private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
		{
            //if (updateWalkRequest == null)
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest), $"{nameof(updateWalkRequest)} cannot be empty.");
            //    return false;
            //}

            //if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest.Name), $"{nameof(updateWalkRequest.Name)} is required.");
            //}

            //if (updateWalkRequest.Length <= 0)
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest.Length), $"{nameof(updateWalkRequest.Length)} should be greater than zero.");
            //}


            var region = await regionRepository.GetAsync(updateWalkRequest.RegionId);

            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId),
                    $"{nameof(updateWalkRequest.RegionId)} is invalid.");
            }


            var walkDifficulty = await walkDifficultyRepository.GetAsync(updateWalkRequest.WalkDifficultyId);

            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId),
                    $"{nameof(updateWalkRequest.WalkDifficultyId)} is invalid.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }


		#endregion

	}
}

