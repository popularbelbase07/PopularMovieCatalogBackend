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
        public async Task<ActionResult>Post([FromForm]ActorCreationDTO actorCreationDTO)
        {
            var actor = mapper.Map<Actor>(actorCreationDTO);    
            if(actorCreationDTO != null)
            {
                actor.Picture = await fileStorageService.SaveFiles(containerName, actorCreationDTO.Picture);
            }
            context.Add(actor);
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPut]
        public async Task<ActionResult>Put(int id , [FromForm] ActorCreationDTO actorCreationDTO)
        {
            throw new NotImplementedException();
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
            return Ok($"The ID : {id} of the Actor is Deleted !!!");

        }

    }
}
