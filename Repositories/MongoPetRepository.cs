using ChallengeAPI.Models;
using MongoDB.Driver;

namespace ChallengeAPI.Repositories;

public class MongoPetRepository
{
    private readonly IMongoCollection<MongoPet> _pets;

    public MongoPetRepository(IMongoDatabase database)
    {
        _pets = database.GetCollection<MongoPet>("Pets");
    }

    public async Task<List<MongoPet>> GetAllAsync() =>
        await _pets.Find(_ => true).ToListAsync();

    public async Task<MongoPet?> GetByIdAsync(int id) =>
        await _pets.Find(p => p.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(MongoPet pet)
    {
        var lastPet = await _pets
            .Find(_ => true)
            .SortByDescending(p => p.Id)
            .FirstOrDefaultAsync();

        pet.Id = lastPet is null ? 1 : lastPet.Id + 1;

        await _pets.InsertOneAsync(pet);
    }

    public async Task UpdateAsync(int id, MongoPet pet) =>
        await _pets.ReplaceOneAsync(p => p.Id == id, pet);

    public async Task DeleteAsync(int id) =>
        await _pets.DeleteOneAsync(p => p.Id == id);
}