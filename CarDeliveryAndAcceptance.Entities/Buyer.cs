using CarDeliveryAndAcceptance.Dal.Contracts;

namespace CarDeliveryAndAcceptance.Entities
{
    /// <summary>
    /// Представляет сущность покупателя
    /// </summary>
    public class Buyer : BaseAuditEntity
    {
        /// <summary>
        /// Название покупателя
        /// </summary>
        public string BuyerName { get; set; } = string.Empty;

        /// <summary>
        /// Имя покупателя
        /// </summary>
        public string BuyerFirstName { get; set; } = string.Empty;
        
        /// <summary>
        /// Фамилия покупателя
        /// </summary>
        public string BuyerSurname { get; set; } = string.Empty;


    }
}
