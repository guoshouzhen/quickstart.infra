namespace QuickStart.Infra.RabbitMq
{
    public interface IRabbitMqPwdDescryptor
    {
        string DecrptyPwd(string originPwd);
    }
}
