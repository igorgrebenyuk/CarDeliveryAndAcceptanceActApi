using CarDeliveryAndAcceptance.Services.Contracts.Models.AcceptanceAct;

namespace CarDeliveryAndAcceptance.Services.Contracts;

/// <summary>
/// Сервис генерации Excel-файлов актов приёма-передачи
/// </summary>
public interface IAcceptanceActExcelService
{
    /// <summary>
    /// Генерация Excel-файла по акту
    /// </summary>
    Task<Stream> GenerateAcceptanceActExcelAsync(AcceptanceActModel act, CancellationToken cancellationToken);
}