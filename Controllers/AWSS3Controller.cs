using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using exmaple_identity.Auth;
using exmaple_identity.Models;
using exmaple_identity.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;

namespace exmaple_identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AWSS3Controller : ControllerBase
    {
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly IConfiguration _configuration;
        public string BucketName = "test-connection-s3";

        public AWSS3Controller(UserManager<IdentityUser<int>> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Post(IFormFile formFile, string token)
        {
            string userNameT = User.Identity.Name;

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Access the claims from the JWT
            string userName = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

            var user = await _userManager.FindByNameAsync(userName);

            long maxFileSize = 10 * 1024 * 1024;

            if(formFile.Length > maxFileSize) 
            {
                return BadRequest("File exceed the limit");
            }

            var client = new AmazonS3Client();
            //var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(client, BucketName);
            //if (!bucketExists)
            //{

            //    var bucketRequest = new PutBucketRequest()
            //    {
            //        BucketName = BucketName,
            //        UseClientRegion = true
            //    };

            //    await client.PutBucketAsync(bucketRequest);
            //}

            var objectRequest = new PutObjectRequest()
            {
                BucketName = BucketName,
                Key = $"{user.Email}/{formFile.FileName}",
                //Key = $"{user.Id}/{formFile.FileName}-{DateTime.UtcNow}"
                InputStream = formFile.OpenReadStream(),
            };
            var response = await client.PutObjectAsync(objectRequest);

            return Ok("Succefull");

        }

        [HttpGet("getFiles")]
        public async Task<IActionResult> GetFiles(string prefix)
        {
            var client = new AmazonS3Client();
            var request = new ListObjectsV2Request()
            {
                BucketName = BucketName,
                Prefix = prefix
            };

            var response = await client.ListObjectsV2Async(request);

            var objectList = new List<S3ObjectInfo>();

            var obj = response.S3Objects.Select(x => new S3ObjectInfo
            {
                Key = x.Key,
                Size = x.Size
            });

            //foreach (var s3Object in response.S3Objects)
            //{
            //    objectList.Add(new S3ObjectInfo
            //    {
            //        Key = s3Object.Key,
            //        Size = s3Object.Size
            //    });
            //}

            return Ok(obj);
        }

        [HttpDelete("deleteFile")]
        public async Task<IActionResult> DeleteFile(string key)
        {
            var client = new AmazonS3Client();
            var request = new DeleteObjectRequest
            {
                BucketName = BucketName,
                Key = key
            };

            var response = await client.DeleteObjectAsync(request);

            return Ok(response);
        }

    }
}

public class S3ObjectInfo
{
    public string Key { get; set; }
    public long Size { get; set; }
    
}
