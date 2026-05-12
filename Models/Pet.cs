using System.Text.Json.Serialization;

namespace ChallengeAPI.Models;

/// <summary>
/// Representa um pet cadastrado no sistema.
/// </summary>
public class Pet
{
    /// <summary>
    /// Identificador único do pet.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nome do pet.
    /// </summary>
    public string Nome { get; set; }

    /// <summary>
    /// Espécie do pet.
    /// </summary>
    public string Especie { get; set; }

    /// <summary>
    /// Raça do pet.
    /// </summary>
    public string Raca { get; set; }

    /// <summary>
    /// Idade do pet.
    /// </summary>
    public int Idade { get; set; }

    /// <summary>
    /// ID do tutor responsável pelo pet.
    /// </summary>
    public int TutorId { get; set; }

    /// <summary>
    /// ID da clínica relacionada ao pet.
    /// </summary>
    public int ClinicaId { get; set; }

    /// <summary>
    /// Lista de consultas do pet.
    /// </summary>
    [JsonIgnore]
    public ICollection<Consulta>? Consultas { get; set; }

    /// <summary>
    /// Tutor responsável pelo pet.
    /// </summary>
    [JsonIgnore]//isso faz com que o json fique limpo e somente com os requisitos da entidade, sem a referência circular que poderia causar problemas de serialização.
    public Tutor? Tutor { get; set; }

    /// <summary>
    /// Lista de vacinas do pet.
    /// </summary>
    [JsonIgnore] //isso faz com que o json fique limpo e somente com os requisitos da entidade, sem a referência circular que poderia causar problemas de serialização.
    public ICollection<Vacina>? Vacinas { get; set; }//coloquei como algo não obrigatório para evitar problemas de referência circular, na hora de fazer os endpoints isso não atrapalho o json, e assim é possível manter a relação entre as entidades sem causar erros de serialização.
}