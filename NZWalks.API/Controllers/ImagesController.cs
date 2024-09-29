using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ImagesController : ControllerBase
	{
		private readonly IImageRepository _imageRepository;

		public ImagesController(IImageRepository imageRepository)
        {
			this._imageRepository = imageRepository;
		}

        [HttpPost]
		[Route("Upload")]
		public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDTO request)
		{
			ValidateFileUpload(request);

			if (ModelState.IsValid)
			{
				var imageDomainModel = new Image
				{
					File = request.File,
					FileName = request.FileName,
					FileExtension = Path.GetExtension(request.File.FileName),
					FileSizeInBytes = request.File.Length,
					FileDescription = request.FileDescription
				};

				imageDomainModel = await _imageRepository.Upload(imageDomainModel);

				return Ok(imageDomainModel);
			}

			return BadRequest(ModelState);

		}

		private void ValidateFileUpload(ImageUploadRequestDTO request)
		{
			var allowedExtensions = new string[] { ".jpg", ".jpeg", "png" };
			if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
			{
				ModelState.AddModelError("file", "Unsupported file format.");
			}

			if(request.File.Length > 10485760)
			{
				ModelState.AddModelError("file", "file size is too big.");
			}
		}
	}
}
