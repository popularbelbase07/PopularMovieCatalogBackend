using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PopularMovieCatalogBackend.DTOs.Genre;
using PopularMovieCatalogBackend.Helpers.Pagination;
using PopularMovieCatalogBackend.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PopularMovieCatalogBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public class GenresController : ControllerBase
    {
        private readonly ILogger<GenresController> logger;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;


        public GenresController(ILogger<GenresController> logger, ApplicationDbContext context, IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;

        }



        // GET: api/<GenresController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            // logger.LogInformation("Getting all genres");
            // Implementating pagination
            var queryable = context.Genres.AsQueryable();
            await HttpContext.InsertParametsrPaginationInHeader(queryable);
            var genres = await queryable.OrderBy(x => x.Name).Paginate(paginationDTO).ToListAsync();


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
            // var genres = await context.Genres.ToListAsync();
            return mapper.Map<List<GenreDTO>>(genres);


        }

        // Filter the movies and filter by Genres
        // GET: api/<GenresController>
        [HttpGet("allGenres")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<GenreDTO>>> Get()
        { var genres = await context.Genres.OrderBy(x => x.Name).ToListAsync();
            return mapper.Map<List<GenreDTO>>(genres);
        }


        // GET api/<GenresController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GenreDTO>> Get(int id)
        {
            var getById = await context.Genres.FirstOrDefaultAsync(x => x.Id == id); 

            if(getById == null) { 
            return NoContent(); 
            }
            return mapper.Map<GenreDTO>(getById);              
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
        public async Task<ActionResult> Put(int id, [FromBody] GenreCreationDTO genreCreateDTO)
        {
            var updateGenre = await context.Genres.FirstOrDefaultAsync(_ => _.Id == id);

            if (updateGenre == null)
            {
                return NotFound();
            }

            updateGenre= mapper.Map(genreCreateDTO, updateGenre);
            await context.SaveChangesAsync();
            return Ok($"{id} of the Genre is updated !! ");

        }

        // DELETE api/<GenresController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existData = await context.Genres.AnyAsync(x => x.Id == id);

            if (!existData)
            {
                return NotFound();

            }
            context.Remove(new Genre() { Id = id });
            await context.SaveChangesAsync();
            return Ok($"The ID : {id} of the genre is Deleted !!!");
        }
    }
}
