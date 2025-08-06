namespace GentleBlossom_BE.Data.Constants
{
    public class LoginType
    {
        public const string Manual = "manual";
        public const string Google = "google";

        public static readonly List<string> All = new()
        {
            Manual, Google
        };
    }
}
