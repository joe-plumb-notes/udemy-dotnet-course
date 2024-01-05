using System.Security.Claims;
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
        public IEnumerable<Post> GetPosts() 
        {
            string sqlGetPosts = $@"SELECT [PostId],
                    [UserId],
                    [PostTitle],
                    [PostContent],
                    [PostCreated],
                    [PostUpdated] 
                FROM TutorialAppSchema.Posts";
            return _dapper.LoadData<Post>(sqlGetPosts);
        }

        [HttpGet("{postId}")]
        public Post GetPost(int postId)
        {
            string sqlGetPost = $@"SELECT [PostId],
                    [UserId],
                    [PostTitle],
                    [PostContent],
                    [PostCreated],
                    [PostUpdated] 
                FROM TutorialAppSchema.Posts
                WHERE PostId = @PostId";
            return _dapper.LoadDataSingleWithParams<Post>(sqlGetPost, new {PostId = postId});
        }

        [HttpGet("posts/{userId}")]
        public IEnumerable<Post> GetUserPosts(int userId)
        {
            string sqlGetPost = $@"SELECT [PostId],
                    [UserId],
                    [PostTitle],
                    [PostContent],
                    [PostCreated],
                    [PostUpdated] 
                FROM TutorialAppSchema.Posts
                WHERE UserId = @UserId";
            return _dapper.LoadDataWithParams<Post>(sqlGetPost, new {UserId = userId});
        }

        [HttpGet("myposts")]
        public IEnumerable<Post> GetAuthenticatedUserPosts()
        {
            string sqlGetPost = $@"SELECT [PostId],
                    [UserId],
                    [PostTitle],
                    [PostContent],
                    [PostCreated],
                    [PostUpdated] 
                FROM TutorialAppSchema.Posts
                WHERE UserId = @UserId";
            return _dapper.LoadDataWithParams<Post>(sqlGetPost, new {UserId = User.FindFirstValue("userId")});
        }

        [HttpPost]
        public IActionResult AddPost(PostToAddDto postToAdd)
        {
            string sqlInsertPost = $@"INSERT INTO TutorialAppSchema.Posts (
                    [UserId],
                    [PostTitle],
                    [PostContent],
                    [PostCreated],
                    [PostUpdated]) 
                VALUES
                    ({ User.FindFirstValue("userId") },
                    '{postToAdd.PostTitle}',
                    '{postToAdd.PostContent}',
                    GETDATE(),
                    GETDATE());";
            if (_dapper.ExecuteSql(sqlInsertPost))
            {
                return Ok();
            }
            throw new Exception("Failed to create post!");
        }

        [HttpPut]
        public IActionResult EditPost(PostToEditDto postToEdit)
        {
            string sqlInsertPost = $@"UPDATE TutorialAppSchema.Posts SET 
                    [PostTitle] = '{postToEdit.PostTitle}',
                    [PostContent] = '{postToEdit.PostContent}',
                    [PostUpdated] = GETDATE() 
                WHERE [PostId] = {postToEdit.PostId} 
                AND [UserId] = { User.FindFirstValue("userId") };";
            if (_dapper.ExecuteSql(sqlInsertPost))
            {
                return Ok();
            }
            throw new Exception("Failed to edit post!");
        }

        [HttpDelete("{postId}")]
        public IActionResult DeletePost(int postId)
        {
            string sqlDeletePost = $@"DELETE FROM TutorialAppSchema.Posts
                WHERE [PostId] = {postId}
                AND [UserId] = { User.FindFirstValue("userId") }";
            if (_dapper.ExecuteSql(sqlDeletePost))
            {
                return Ok();
            }
            throw new Exception("Failed to delete post!");
        }

        [HttpGet("search/{query}")]
        public IEnumerable<Post> GetPostsSearch(string query)
        {
            string GetPostsSearch = $@"SELECT [PostId],
                    [UserId],
                    [PostTitle],
                    [PostContent],
                    [PostCreated],
                    [PostUpdated] 
                FROM TutorialAppSchema.Posts
                WHERE [PostTitle] LIKE @Query
                OR [PostContent] LIKE @Query;";
            return _dapper.LoadDataWithParams<Post>(GetPostsSearch, new {Query = '%' + query + '%'});
        }
    }
}