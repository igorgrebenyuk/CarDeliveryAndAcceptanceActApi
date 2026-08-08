using FinalExercise.Dal.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinalExercise.Context.EntityFrameworkCore;

/// <summary>
/// Методы расширения для <see cref="EntityTypeBuilder"/>
/// </summary>
public static class EntityTypeBuilderExtensions
{
    /// <summary>
    /// Задаёт конфигурацию ключа для идентификатора <see cref="IEntityWithId.Id"/>
    /// </summary>
    public static void HasIdAsKey<T>(this EntityTypeBuilder<T> builder)
        where T : class, IEntityWithId
        => builder.HasKey(x => x.Id);
    
    /// <summary>
    /// Задаёт конфигурацию свойств аудита добавления для сущности <inheritdoc cref="BaseAuditEntity"/>
    /// </summary>
    public static void CreateAuditConfiguration<T>(this EntityTypeBuilder<T> builder)
        where T : class, IEntityAuditCreated
    {
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(200);
    }

    /// <summary>
    /// Задаёт конфигурацию свойств полного аудита для сущности <inheritdoc cref="BaseAuditEntity"/>
    /// </summary>
    public static void UpdateAuditConfiguration<T>(this EntityTypeBuilder<T> builder)
        where T : class, IEntityAuditUpdate
    {
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.Property(x => x.UpdatedBy).IsRequired().HasMaxLength(200);
    }   
}