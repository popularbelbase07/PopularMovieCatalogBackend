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
        private readonly ApplicationDbContext context;

        public GenresController(ILogger<GenresController> logger , ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }



        // GET: api/<GenresController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> Get()
        {
            logger.LogInformation("Getting all genres");
            return await context.Genres.getListAsync();
        }

        // GET api/<GenresController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            throw new NotImplementedException();
        }

        // POST api/<GenresController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Genre genre)
        {
           context.Genres.Add(genre);
            await context.SaveChangesAsync(); 
            return Ok();
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
