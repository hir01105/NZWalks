using System;
using AutoMapper;

namespace NZWalks.API.Profiles
{
	public class WalkDifficultyProfile : Profile
	{
		public WalkDifficultyProfile()
		{
			CreateMap<Models.Domain.WalkDifficulty, Models.DTO.WalkDifficulty>()
				.ReverseMap();

			CreateMap<Models.DTO.AddWalkDifficultyRequest, Models.Domain.WalkDifficulty>();

			CreateMap<Models.DTO.UpdateWalkDifficultyRequest, Models.Domain.WalkDifficulty>();
		}
	}
}

