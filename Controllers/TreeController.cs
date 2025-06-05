using familytree_api.Dtos.Family;
using familytree_api.Dtos.Partner;
using familytree_api.Services;
using familytree_api.Services.Family;
using familytree_api.Services.Files;
using familytree_api.Services.Partner;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace familytree_api.Controllers
{
    [Route("api/tree")]
    [ApiController]
    public class TreeController(
        IFamilyService _familyService,
        IPartnerService _partnerService,
        IFileService _fileService
        ) : Controller
    {
        [Authorize]
        [HttpPost("members")]

        public async Task<IActionResult> CreateMember(FamilyMemberInputDto body)
        {
            try
            {
                await _familyService.CreateFamilyMember(body);

                return Ok(new ApiResponse<string>(data: "", message: "Family member created successfully",
                    statusCode: (int)HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(errors: [], message: ex.Message));
            }
        }

        [Authorize]
        [HttpPut("members")]

        public async Task<IActionResult> UpdateMember(FamilyMemberUpdateDto body)
        {
            try
            {
                await _familyService.UpdateFamilyMember(body);

                return Ok(new ApiResponse<string>(data: "", message: "Family member updated successfully",
                    statusCode: (int)HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(errors: [], message: ex.Message));
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> FetchTree()
        {
            try
            {
                var tree = await _familyService.Tree();

                return Ok(new ApiResponse<TreeOutputDto?>(data: tree, message: "Ok",
                    statusCode: (int)HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(errors: [], message: ex.Message));
            }
        }

        [Authorize]
        [HttpPost("partner")]
        public async Task<IActionResult> CreatePartner(PartnerInputDto body)
        {
            try
            {
                 await _partnerService.AddPartner(body);

                return Ok(new ApiResponse<string>(data: "", message: "Partner created successfully",
                    statusCode: (int)HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(errors: [], message: ex.Message));
            }
        }


        [Authorize]
        [HttpPut("partner")]
        public async Task<IActionResult> UpdatePartnership(PartnerUpdateDto body)
        {
            try
            {
                await _partnerService.UpdatePartnership(body);

                return Ok(new ApiResponse<string>(data: "", message: "Partner updated successfully",
                    statusCode: (int)HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(errors: [], message: ex.Message));
            }
        }

        [Authorize]
        [HttpPost("photos")]
        public async Task<IActionResult> AddPhotos([FromForm] FileInputDto body)
        {
            try
            {
                await _fileService.UploadFile(body);

                return Ok(new ApiResponse<string>(data: "", message: "Image successfully uploaded",
                    statusCode: (int)HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(errors: [], message: ex.Message));
            }
        }

        [Authorize]
        [HttpDelete("photos")]
        public async Task<IActionResult> DeletePhotos([FromQuery] int id)
        {
            try
            {
                await _fileService.RemoveFile(id);

                return Ok(new ApiResponse<string>(data: "", message: "Image successfully removed",
                    statusCode: (int)HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(errors: [], message: ex.Message));
            }
        }

        [Authorize]
        [HttpDelete("members")]
        public async Task<IActionResult> RemoveMember([FromQuery] int id)
        {
            try
            {
                await _familyService.DeleteFamilyMember(id);

                return Ok(new ApiResponse<string>(data: "", message: "Member removed",
                    statusCode: (int)HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(errors: [], message: ex.Message));
            }
        }
    }
}
