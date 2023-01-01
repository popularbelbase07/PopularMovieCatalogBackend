using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PopularMovieCatalogBackend.DTOs.Genre;
using PopularMovieCatalogBackend.DTOs.MovieTheater;
using PopularMovieCatalogBackend.Helpers.Pagination;
using PopularMovieCatalogBackend.Model;

namespace PopularMovieCatalogBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public class MovieTheatersController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public MovieTheatersController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;

        }
        [HttpGet]
        public async Task<ActionResult<List<MovieTheaterDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = context.MoviesTheaters.AsQueryable();
            await HttpContext.InsertParametsrPaginationInHeader(queryable);
            var entities = await queryable.OrderBy(_ => _.Name).Paginate(paginationDTO).ToListAsync() ;
            //var entities = await context.MoviesTheaters.ToListAsync();
            return mapper.Map<List<MovieTheaterDTO>>(entities);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieTheaterDTO>> Get(int id)
        {
            var movieTheater = await context.MoviesTheaters.FirstOrDefaultAsync(x => x.Id == id);

            if (movieTheater == null)
            {
                return NoContent();
            }
            return mapper.Map<MovieTheaterDTO>(movieTheater);
        }

        [HttpPost]
        public async Task<ActionResult> Post(MovieTheaterCreationDTO movieCreationDTO)
        {
            var CreateMovieTheater = mapper.Map<MovieTheater>(movieCreationDTO);
            context.Add(CreateMovieTheater);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, MovieTheaterCreationDTO movieCreationDTO)
        {
            var movieTheater = await context.MoviesTheaters.FirstOrDefaultAsync(x => x.Id == id);

            if (movieTheater == null)
            {
                return NoContent();
            }
            movieTheater = mapper.Map(movieCreationDTO, movieTheater);
            await context.SaveChangesAsync();
            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existMovieTheater = await context.MoviesTheaters.AnyAsync(x => x.Id == id);

            if (!existMovieTheater)
            {
                return NotFound();

            }
            context.Remove(new MovieTheater { Id = id });
            await context.SaveChangesAsync();
            return Ok($"The ID : {id} of the genre is Deleted !!!");


        }
    }
}
