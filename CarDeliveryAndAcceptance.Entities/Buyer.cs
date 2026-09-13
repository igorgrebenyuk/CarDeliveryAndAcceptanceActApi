using CarDeliveryAndAcceptance.Dal.Contracts;

namespace CarDeliveryAndAcceptance.Entities;

    /// <summary>
    /// Представляет сущность покупателя
    /// </summary>
    public class Buyer : BaseAuditEntity
    {
        /// <summary>
        /// Название покупателя
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Имя покупателя
        /// </summary>
        public string FirstName { get; set; } = string.Empty;
        
        /// <summary>
        /// Фамилия покупателя
        /// </summary>
        public string Surname { get; set; } = string.Empty;


    }

