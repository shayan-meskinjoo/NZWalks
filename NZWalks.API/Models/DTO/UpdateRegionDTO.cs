using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
	public class UpdateRegionDTO
	{
		[Required]
		[MaxLength(3, ErrorMessage = "maximun characters for code is 3.")]
		public string Code { get; set; }
		[Required]
		[MaxLength(30, ErrorMessage = "maximun characters for name is 30.")]
		public string Name { get; set; }
		public string? RegionImageUrl { get; set; }
	}
}
