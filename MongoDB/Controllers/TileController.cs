using Microsoft.AspNetCore.Mvc;
using MongoDB.Models;
using MongoDB.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
namespace MongoDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TileController:ControllerBase
    {
        private readonly TileService _tileService;
        public TileController(TileService tileService)
        {
            _tileService = tileService;
        }
        [HttpGet]
        public async Task<List<Tile>> Get()
        {
            return await _tileService.Get();
        }
        [HttpGet]
        [Route("{x}/{y}/{z}")]
        public async Task<ActionResult<Tile>> Get(string x, string y, string z)
        {
            var tile=await _tileService.Get(x, y, z);
            if(tile==null)
            {
                return NotFound();
            }
            return Ok(tile);
        }
        public async Task<Tile> Create(Tile tile)
        {
            return await _tileService.Create(tile);
        }
       
    }
}
