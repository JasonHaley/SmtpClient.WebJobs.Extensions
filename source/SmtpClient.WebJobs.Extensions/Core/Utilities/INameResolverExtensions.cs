using Microsoft.Azure.WebJobs;

namespace Smtp.WebJobs.Extensions.Utilities
{
    public static class INameResolverExtensions
    {
        public static int ResolveAsInt(this INameResolver resolver, string name, int defaultValue)
        {
            var value = resolver.Resolve(name);
            int val = 0;
            if (int.TryParse(value, out val))
            {
                return val;
            }
            else
            {
                return defaultValue;
            }
        }

        public static bool ResolveAsBool(this INameResolver resolver, string name, bool defaultValue)
        {
            var value = resolver.Resolve(name);
            bool val = false;
            if (bool.TryParse(value, out val))
            {
                return val;
            }
            else
            {
                return defaultValue;
            }
        }
    }
}
