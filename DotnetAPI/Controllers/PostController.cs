using System.Data;
using System.Security.Claims;
using Dapper;
using DotnetAPI.Data;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        public PostController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet("posts")]
        public IEnumerable<Post> GetPosts(int? postId, int? userId, string? query) 
        {
            string sqlGetPosts = $@"EXEC TutorialAppSchema.spPosts_Get @UserId, @SearchValue, @PostId";

            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserId", userId, DbType.Int32);
            sqlParameters.Add("@SearchValue", query, DbType.String);
            sqlParameters.Add("@PostId", postId, DbType.Int32);

            return _dapper.LoadDataWithParams<Post>(sqlGetPosts, sqlParameters);
        }

        [HttpGet("myposts")]
        public IEnumerable<Post> GetAuthenticatedUserPosts()
        {
            string sqlGetPost = $@"EXEC TutorialAppSchema.spPosts_Get @UserId";
            return _dapper.LoadDataWithParams<Post>(sqlGetPost, new {UserId = User.FindFirstValue("userId")});
        }

        [HttpPost]
        public IActionResult AddPost(Post postToAdd)
        {
            string sqlInsertPost = $@"EXEC TutorialAppSchema.spPosts_Upsert 
                    @UserId,
                    @PostTitle,
                    @PostContent
                    ";

            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserId", User.FindFirstValue("userId"), DbType.Int32);
            sqlParameters.Add("@PostTitle", postToAdd.PostTitle, DbType.String);
            sqlParameters.Add("@PostContent", postToAdd.PostContent, DbType.String);
            
            if (_dapper.ExecuteSqlWithParameters(sqlInsertPost, sqlParameters))
            {
                return Ok();
            }
            throw new Exception("Failed to create post!");
        }

        [HttpPut]
        public IActionResult EditPost(Post postToEdit)
        {
            string sqlUpdatePost = $@"EXEC TutorialAppSchema.spPosts_Upsert 
                    @UserId,
                    @PostTitle,
                    @PostContent,
                    @PostId
                    ";
            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserId", User.FindFirstValue("userId"), DbType.Int32);
            sqlParameters.Add("@PostTitle", postToEdit.PostTitle, DbType.String);
            sqlParameters.Add("@PostContent", postToEdit.PostContent, DbType.String);
            sqlParameters.Add("@PostId", postToEdit.PostId, DbType.Int32);

            if (_dapper.ExecuteSqlWithParameters(sqlUpdatePost, sqlParameters))
            {
                return Ok();
            }
            throw new Exception("Failed to edit post!");
        }

        [HttpDelete("{postId}")]
        public IActionResult DeletePost(int postId, int userId)
        {
            string sqlDeletePost = $@"EXEC TutorialAppSchema.SpPosts_Delete
                @PostId, 
                @UserId";

            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserId", userId, DbType.Int32);
            sqlParameters.Add("@PostId", postId, DbType.Int32);
            if (_dapper.ExecuteSqlWithParameters(sqlDeletePost, sqlParameters))
            {
                return Ok();
            }
            throw new Exception("Failed to delete post!");
        }
    }
}