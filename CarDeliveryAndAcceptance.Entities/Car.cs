using CarDeliveryAndAcceptance.Dal.Contracts;

namespace CarDeliveryAndAcceptance.Entities
{
    public class Car : BaseAuditEntity
    {
       

        /// <summary>
        /// Марка, модель
        /// </summary>
        public string MakeAndModel { get; set; } = string.Empty;

        /// <summary>
        /// Регистрационный знак 
        /// </summary>
        public string LicensePlate { get; set; } = string.Empty;

        /// <summary>
        /// Идентификационный номер
        /// </summary>
        public string VinCode { get; set; } = string.Empty;

        /// <summary>
        /// Год выпуска
        /// </summary>
        public int ManufactureYear { get; set; }

        /// <summary>
        /// Двигатель №
        /// </summary>
        public string EngineNumber { get; set; } = string.Empty;

        /// <summary>
        /// Шасси (рама)
        /// </summary>
        public string ChassisNumber { get; set; } = string.Empty;

        /// <summary>
        /// Кузов (коляска)
        /// </summary>
        public string BodyNumber { get; set; } = string.Empty;

        /// <summary>
        /// Цвет
        /// </summary>
        public string Color { get; set; } = string.Empty;

        
    }
}
