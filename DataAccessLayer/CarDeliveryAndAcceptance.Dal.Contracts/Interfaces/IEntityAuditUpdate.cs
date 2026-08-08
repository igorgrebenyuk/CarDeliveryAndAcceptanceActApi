namespace FinalExercise.Dal.Contracts.Interfaces;

/// <summary>
/// Аудит редактирования сущностей
/// </summary>
public interface IEntityAuditUpdate
{
    /// <summary>
    /// Дата изменения
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// Кто изменил
    /// </summary>
    public string UpdatedBy { get; set; }
}