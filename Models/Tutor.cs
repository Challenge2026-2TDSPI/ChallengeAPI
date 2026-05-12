namespace ChallengeAPI.Models;
using System.Text.Json.Serialization;


/// <summary>
/// Representa um tutor responsável pelos pets.
/// </summary>
public class Tutor
{
    /// <summary>
    /// Identificador único do tutor.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nome completo do tutor.
    /// </summary>
    public string Nome { get; set; }

    /// <summary>
    /// Telefone do tutor.
    /// </summary>
    public string Telefone { get; set; }

    /// <summary>
    /// E-mail do tutor.
    /// </summary>
    public string Email { get; set; }


    /// <summary>
    /// Lista de pets do tutor.
    /// </summary>
    [JsonIgnore]//isso faz com que o json fique limpo e somente com os requisitos da entidade, sem a referência circular que poderia causar problemas de serialização.
    public ICollection<Pet>? Pets { get; set; }//coloquei como algo não obrigatório para evitar problemas de referência circular, na hora de fazer os endpoints isso não atrapalho o json, e assim é possível manter a relação entre as entidades sem causar erros de serialização.
}