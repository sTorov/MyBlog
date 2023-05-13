using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace MyBlog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentApiController : ControllerBase
    {
        [HttpGet]
        [Route("{id}")]
        public void Get([FromRoute] int id)
        {

        }

        [HttpGet]
        public void GetAll()
        {
        }

        [HttpPost]
        public void Create(/*[FromBody] model*/)
        {

        }

        [HttpPut]
        [Route("{id}")]
        public void Update([FromRoute] int id /*[FromBody] model*/)
        {

        }

        [HttpDelete]
        [Route("{id}")]
        public void Delete([FromRoute] int id)
        {

        }
    }
}