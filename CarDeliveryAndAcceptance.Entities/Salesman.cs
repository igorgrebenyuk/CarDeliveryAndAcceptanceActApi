using CarDeliveryAndAcceptance.Dal.Contracts;

namespace CarDeliveryAndAcceptance.Entities;

    /// <summary>
    /// Сущность  представляющая продавца
    /// </summary>
    public class Salesman : BaseAuditEntity
    {
        
        /// <summary>
        /// Название продавца
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Имя продавца
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Фамилия продавца
        /// </summary>
        public string Surname { get; set; } = string.Empty;
    }

