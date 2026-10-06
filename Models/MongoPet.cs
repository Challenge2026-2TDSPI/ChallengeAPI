using MongoDB.Bson.Serialization.Attributes;

namespace ChallengeAPI.Models;

public class MongoPet
{
    [BsonId]
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;
    public string Especie { get; set; } = string.Empty;
    public string Raca { get; set; } = string.Empty;
}