using System.Text.Json.Serialization;

namespace ChallengeAPI.Models;

/// <summary>
/// Representa uma vacina aplicada em um pet.
/// </summary>
public class Vacina
{
    /// <summary>
    /// Identificador único da vacina.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nome da vacina.
    /// </summary>
    public string NomeVacina { get; set; }

    /// <summary>
    /// Data da aplicação da vacina.
    /// </summary>
    public string DataAplicacao { get; set; }

    /// <summary>
    /// Data prevista para a próxima dose.
    /// </summary>
    public string ProximaDose { get; set; }

    /// <summary>
    /// ID do pet relacionado à vacina.
    /// </summary>
    public int PetId { get; set; }

    /// <summary>
    /// Pet relacionado à vacina.
    /// </summary>
    [JsonIgnore]//isso faz com que o json fique limpo e somente com os requisitos da entidade, sem a referência circular que poderia causar problemas de serialização.
    public Pet? Pet { get; set; } //coloquei como algo não obrigatório para evitar problemas de referência circular, na hora de fazer os endpoints isso não atrapalho o json, e assim é possível manter a relação entre as entidades sem causar erros de serialização.
}