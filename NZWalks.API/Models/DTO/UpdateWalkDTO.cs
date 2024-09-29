using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
	public class UpdateWalkDTO
	{
		[Required]
		[MaxLength(30, ErrorMessage = "maximun characters for name is 30.")]
		public string Name { get; set; }
		[Required]
		[MaxLength(1000, ErrorMessage = "maximun characters for des is 1000.")]
		public string Description { get; set; }
		[Required]
		public double LengthInKm { get; set; }
		public string? WalkImageUrl { get; set; }
		[Required]
		public Guid RegionId { get; set; }
		[Required]
		public Guid DifficultyId { get; set; }
	}
}
