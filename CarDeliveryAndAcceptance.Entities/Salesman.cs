using CarDeliveryAndAcceptance.Dal.Contracts;

namespace CarDeliveryAndAcceptance.Entities
{
    /// <summary>
    /// Сущность  представляющая продавца
    /// </summary>
    public class Salesman : BaseAuditEntity
    {
        /// <summary>
        /// Уникальный идентификатор продавца
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название продавца
        /// </summary>
        public string SalesmanName { get; set; } = string.Empty;

        /// <summary>
        /// Имя продавца
        /// </summary>
        public string SalesmanFirstName { get; set; } = string.Empty;

        /// <summary>
        /// Фамилия продавца
        /// </summary>
        public string SalesmanSurname { get; set; } = string.Empty;
    }
}
