namespace FinalExercise.Dal.Contracts.Interfaces;

/// <summary>
/// Аудит удаления сущностей
/// </summary>
public interface IEntityAuditDeletedAt
{
    /// <summary>
    /// Дата удаления
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }
}