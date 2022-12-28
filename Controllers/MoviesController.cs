using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PopularMovieCatalogBackend.DTOs.Actor;
using PopularMovieCatalogBackend.DTOs.Genre;
using PopularMovieCatalogBackend.DTOs.Movie;
using PopularMovieCatalogBackend.DTOs.MovieTheater;
using PopularMovieCatalogBackend.Helpers.ImageInAzureStorage;
using PopularMovieCatalogBackend.Model.Movies;

namespace PopularMovieCatalogBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorageServices fileStorageService;
        private string containerName = "movies";

        public MoviesController(ApplicationDbContext context, IMapper mapper, IFileStorageServices fileStorageService)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStorageService = fileStorageService;
        }
        // Get Movies by Id

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDTO>>Get(int Id)
        {
            var movie = await context.Movies
                .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MovieTheatersMovies).ThenInclude(x=> x.MovieTheater)
                .Include(x => x.MoviesActors).ThenInclude(x => x.Actor)
                .FirstOrDefaultAsync(x => x.Id == Id);  

            if(movie == null)
            {
                return Ok("No movies Found");
            }
            var dto = mapper.Map<MovieDTO>(movie);
            dto.Actors.OrderBy(x => x.Order).ToList();
            return dto;
            



        }

        //Get all the Theaters and Genres in Create Movie frontend
        [HttpGet("PostGet")]
        public async Task<ActionResult<MoviePostGetDTO>> PostGet()
        {
            var movieTheaters = await context.MoviesTheaters.ToListAsync();
            var genres = await context.Genres.ToListAsync();

            var movieTheaterDTO = mapper.Map<List<MovieTheaterDTO>>(movieTheaters);
            var genresDTO = mapper.Map<List<GenreDTO>>(genres);
            return new MoviePostGetDTO() { Genres = genresDTO, MovieTheaters = movieTheaterDTO };
        }


        [HttpPost]
        public async Task<ActionResult<int>> Post([FromForm] MovieCreationDTO movieCreationDTO)
        {
            var movie = mapper.Map<Movie>(movieCreationDTO);          
            if (movieCreationDTO.Poster != null)
            {
                movie.Poster = await fileStorageService.SaveFiles(containerName, movieCreationDTO.Poster);
            }
            AnnotateActorOrder(movie);
            context.Add(movie);
            await context.SaveChangesAsync();
            return movie.Id;
        }

        private void AnnotateActorOrder(Movie movie)
        {
            if (movie.MoviesActors != null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i;
                }
            }
        }
    }
}