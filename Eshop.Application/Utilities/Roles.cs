namespace Eshop.Application.Utilities
{
    public static class Roles
    {
        public const string Administrator = "1";
        public const string UserSystem = "2";
        public const string ContentUploader = "3";
        public const string AdminAssistant = "4";
        public const string BikeDelivery = "5";

        public static string GetRoleBy(long id)
        {
            return id switch
            {
                1 => "مدیر سیستم",
                2 => "کاربر سیستم",
                3 => "محتوا گذار",
                4 => "دستیار مدیر",
                5 => "پیک موتوری",
                _ => ""
            };
        }
    }
}
