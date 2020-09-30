using BooksApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace MongoDB.Services
{
    public class TileService
    {
        private readonly IMongoCollection<Tile> _tiles;
        public TileService(IConfiguration settings)
        {
            var client = new MongoClient("mongodb://localhost:27017");

            var database = client.GetDatabase("png");
            _tiles = database.GetCollection<Tile>("layer");
        }
        public async Task<Tile> Create(Tile tile)
        {
            await _tiles.InsertOneAsync(tile);
            return tile;
        }
        public async Task<List<Tile>> Get()
        {
            var result = await _tiles.FindAsync(tile => true);
            return result.ToList();
        }
       
        public async Task<IActionResult> Get(string x,string y,string z)
        {
            var tile = await _tiles.FindAsync(tile => tile.x == x && tile.y == y && tile.z == z);
            List<Tile> listTile = tile.ToList();
            if(listTile.Count==0)
            {
                return null;
            }
            byte[] img1 = listTile[0].img;
            return new FileContentResult(img1, "image/png");
        }
        public async Task Update(string x, string y, string z, Tile tile)
        {
            await _tiles.ReplaceOneAsync(tile => tile.x == x && tile.y == y && tile.z == z,tile);
        }
        public async Task Remove(Tile tile)
        {
            await _tiles.DeleteOneAsync(tile => tile.img == tile.img);
        }
        public async Task Remove(string x, string y,string z)
        {
            await _tiles.DeleteOneAsync(tile => tile.x == x&&tile.y==y&&tile.z==z);
        }
    }
}
