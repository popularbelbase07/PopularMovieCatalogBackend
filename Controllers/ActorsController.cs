using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PopularMovieCatalogBackend.DTOs.Actor;
using PopularMovieCatalogBackend.DTOs.Genre;
using PopularMovieCatalogBackend.Helpers.ImageInAzureStorage;
using PopularMovieCatalogBackend.Helpers.Pagination;
using PopularMovieCatalogBackend.Model;

namespace PopularMovieCatalogBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorageServices fileStorageService;
        private readonly string containerName = "actors";

        public ActorsController(ApplicationDbContext context, IMapper mapper, IFileStorageServices fileStorageService)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStorageService = fileStorageService;
        }

        [HttpGet]
        //Implementating pagination with actor
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginationDTO pagination)
        {
            var queryable = context.Actors.AsQueryable();
            await HttpContext.InsertParametsrPaginationInHeader(queryable);
            var actors = await queryable.OrderBy(x => x.Name).Paginate(pagination).ToListAsync();
            // var actors = await context.Actors.ToListAsync();
            return mapper.Map<List<ActorDTO>>(actors);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            var actor = await context.Actors.SingleOrDefaultAsync(x => x.Id == id);
            if (actor == null)
            {
                return NotFound();
            }
            return mapper.Map<ActorDTO>(actor);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreationDTO actorCreationDTO)
        {
            var actor = mapper.Map<Actor>(actorCreationDTO);
            if (actorCreationDTO != null)
            {
                actor.Picture = await fileStorageService.SaveFiles(containerName, actorCreationDTO.Picture);
            }
            context.Add(actor);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreationDTO actorCreationDTO)
        {
            var actor = await context.Actors.FirstOrDefaultAsync(x => x.Id == id);
            if (actor == null)
            {
                return NotFound();
            }
            actor = mapper.Map(actorCreationDTO, actor);

            if (actorCreationDTO.Picture != null)
            {
                actor.Picture = await fileStorageService.EditFile(containerName,
                    actorCreationDTO.Picture, actor.Picture);
            }
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var actor = await context.Actors.SingleOrDefaultAsync(x => x.Id == id);
            if (actor == null)
            {
                return NotFound();
            }

            context.Remove(actor);
            await context.SaveChangesAsync();
            await fileStorageService.DeleteFile(actor.Picture, containerName);
            return Ok($"The ID : {id} of the Actor is Deleted !!!");
        }

        //Search by name for Actors
        [HttpGet ("searchByName/{query}")]

        public async Task<ActionResult<List<ActorsMovieDTO>>> SearchByName(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new List<ActorsMovieDTO>();  
            }
            return await context.Actors.Where(x => x.Name.Contains(query))
                .OrderBy(x => x.Name).Select(x => new ActorsMovieDTO { Id = x.Id, Name = x.Name, Picture = x.Picture })
                .Take(5).ToListAsync();  

        }

    }
}
