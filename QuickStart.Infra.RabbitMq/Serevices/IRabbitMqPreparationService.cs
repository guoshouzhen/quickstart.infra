namespace QuickStart.Infra.RabbitMq.Serevices
{
    public interface IRabbitMqPreparationService
    {
        /// <summary>
        /// Preparation for RabbitMq.
        /// </summary>
        void PrepareRabbitMq();
    }
}
