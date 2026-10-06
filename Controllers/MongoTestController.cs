using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ChallengeAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MongoTestController : ControllerBase
{
    private readonly IMongoDatabase _database;

    public MongoTestController(IMongoDatabase database)
    {
        _database = database;
    }

    [HttpGet]
    public async Task<IActionResult> TestConnection()
    {
        await _database.ListCollectionNamesAsync();

        return Ok(new
        {
            message = "Conexão com MongoDB Atlas realizada com sucesso!"
        });
    }
}