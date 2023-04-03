using QuickStart.Infra.Redis;

namespace QuickStartDemo
{
    public class MyRedisPwdDecryptor : IRedisPwdDecryptor
    {
        public string Decrypt(string orignRedisPwd)
        {
            return orignRedisPwd;
        }
    }
}
