using Microsoft.AspNetCore.Mvc;
using PopularMovieCatalogBackend.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PopularMovieCatalogBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ILogger<GenresController> logger;

        public GenresController(ILogger<GenresController> logger)
        {
            this.logger = logger;
        }



        // GET: api/<GenresController>
        [HttpGet]
        public IEnumerable<Genre> Get()
        {
            logger.LogInformation("Getting all genres");
          return new List<Genre>() { new Genre() { Id = 1, Name = "Comedy" } };
        }

        // GET api/<GenresController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            throw new NotImplementedException();
        }

        // POST api/<GenresController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            throw new NotImplementedException();
        }

        // PUT api/<GenresController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/<GenresController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
