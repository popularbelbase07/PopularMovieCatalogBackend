using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PopularMovieCatalogBackend.DTOs;
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
        private readonly IMapper mapper;


        public GenresController(ILogger<GenresController> logger , ApplicationDbContext context, IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
           this.mapper = mapper;

        }



        // GET: api/<GenresController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDTO>>> Get()
        {
            logger.LogInformation("Getting all genres");
            //  return await context.Genres.getListAsync();
            /*
           
              var genresDTOs = new List<GenreDTO>();  
              foreach (var genre in genres)
              {
                  genresDTOs.Add(new Genre()
                  {
                      Id = genre.Id,
                      Name = genre.Name
                  });
              }
              return genresDTOs;
            */
            var genres = await context.Genres.ToListAsync();
            return mapper.Map<List<GenreDTO>>(genres);


        }

        // GET api/<GenresController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            throw new NotImplementedException();
        }

        // POST api/<GenresController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GenreCreationDTO genreCreation)
        {
            var genre = mapper.Map<Genre>(genreCreation);
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
