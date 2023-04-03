namespace QuickStart.Infra.Redis
{
    public interface IRedisPwdDecryptor
    {
        string Decrypt(string orignRedisPwd);
    }
}
