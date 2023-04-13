namespace QuickStart.Infra.RabbitMq.Serevices
{
    public interface IRabbitMqPwdDescryptor
    {
        string DecrptyPwd(string originPwd);
    }
}
