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
        //Get upcomming and intheaters movies

        [HttpGet]
        public async Task<ActionResult<LandingPageDTO>> Get()
        {
            var top = 10;
            var today = DateTime.Today;

            var upCommingRelease = await context.Movies
                .Where(x => x.ReleaseDate > today)
                .OrderBy(x => x.ReleaseDate)
                .Take(top)
                .ToListAsync();

            var inTheaters = await context.Movies
                .Where(_ => _.InTheaters)
                .OrderBy(_ => _.InTheaters)
                .Take(top)
                .ToListAsync();

            var landingPageDTO = new LandingPageDTO();
            landingPageDTO.UpcommingReleases = mapper.Map<List<MovieDTO>>(upCommingRelease);
            landingPageDTO.InTheaters = mapper.Map<List<MovieDTO>>(inTheaters);
            return landingPageDTO;

        }
        // Get Movies by Id

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDTO>> Get(int id)
        {
            var movie = await context.Movies
                .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MovieTheatersMovies).ThenInclude(x => x.MovieTheater)
                .Include(x => x.MoviesActors).ThenInclude(x => x.Actor)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (movie == null)
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

        // Create a get by id end poinr for getting necessary information to edit the movie
        [HttpGet("PutGet/{id:int}")]
        public async Task<ActionResult<MoviePutGetDTO>> PutGet(int id)
        {
            var movieById = await Get(id);

            if (movieById.Result is NotFoundResult)
            {
                return NotFound();
            }
            var movie = movieById.Value;

            var genreSelectedIds = movie.Genres.Select(x => x.Id).ToList();
            var nonSelectedGenres = await context.Genres.Where(x => !genreSelectedIds.Contains(x.Id)).ToListAsync();

            var movieTheatersIds = movie.MovieTheaters.Select(x => x.Id).ToList();
            var nonSelectedmovieTheaters = await context.MoviesTheaters.Where(x => !movieTheatersIds.Contains(x.Id)).ToListAsync();

            var nonSelectedGenresDTO = mapper.Map<List<GenreDTO>>(nonSelectedGenres);
            var nonSelectedMoviesTheatersDTO = mapper.Map<List<MovieTheaterDTO>>(nonSelectedmovieTheaters);

            var response = new MoviePutGetDTO();
            response.Movie = movie;
            response.SelectedGenres = movie.Genres;
            response.NonSelectedGenres = nonSelectedGenresDTO;
            response.SelectedMovieTheater = movie.MovieTheaters;
            response.NonSelectedMovieTheater = nonSelectedMoviesTheatersDTO;
            response.Actors = movie.Actors;
            return response;

        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id ,[FromForm] MovieCreationDTO movieCreationDTO )
        {
            var movie = await context.Movies
                .Include(x => x.MoviesActors)
                .Include(x => x.MoviesGenres)
                .Include(x => x.MovieTheatersMovies)
                .FirstOrDefaultAsync(x => x.Id == id);


            if(movie == null) {
                return NotFound();
                    
             }
            movie = mapper.Map(movieCreationDTO, movie);

            if(movieCreationDTO.Poster != null)
            {
                movie.Poster = await fileStorageService.EditFile(containerName, movieCreationDTO.Poster, movie.Poster);

            }
            AnnotateActorOrder(movie);
            await context.SaveChangesAsync();
            //return Ok($"The id {id} is Updated !!");
            return NoContent(); 

        }
       
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
            {
            var movie = await context.Movies.FirstOrDefaultAsync(x => x.Id == id);

            if(movie == null) {
                return Ok("There is no movie exist");
            }

            context.Remove(movie);
            await context.SaveChangesAsync();
            await fileStorageService.DeleteFile(containerName, movie.Poster);   
            return NoContent();


            }


        }
}