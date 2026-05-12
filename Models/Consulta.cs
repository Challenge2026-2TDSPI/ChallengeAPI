using System.Text.Json.Serialization;

namespace ChallengeAPI.Models;

/// <summary>
/// Representa uma consulta veterinária.
/// </summary>
public class Consulta
{
    /// <summary>
    /// Identificador único da consulta.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Data da consulta.
    /// </summary>
    public string DataConsulta { get; set; }

    /// <summary>
    /// Descrição da consulta.
    /// </summary>
    public string Descricao { get; set; }

    /// <summary>
    /// Nome do veterinário responsável.
    /// </summary>
    public string Veterinario { get; set; }

    /// <summary>
    /// ID do pet relacionado.
    /// </summary>
    public int PetId { get; set; }

    /// <summary>
    /// Pet relacionado à consulta.
    /// </summary>
    [JsonIgnore]
    public Pet? Pet { get; set; }
}