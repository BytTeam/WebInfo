using App.Core.Utilities;
using Microsoft.AspNetCore.Http;

namespace App.WebInfo.MVCUI.Helpers
{
    public static class SessionExtension
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            string objectString = Utility.Instance.JsonSerializer(value);
            session.SetString(key, objectString);
        }

        public static T GetObject<T>(this ISession session, string key) where T : class
        {
            string objectString = session.GetString(key);
            if (string.IsNullOrEmpty(objectString))
            {
                return null;
            }
            T value = Utility.Instance.JsonDeserialize<T>(objectString);
            return value;
        }
    }
}
