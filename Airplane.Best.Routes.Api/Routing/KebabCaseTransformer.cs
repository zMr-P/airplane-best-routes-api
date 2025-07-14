using System.Text.RegularExpressions;

namespace airplane_best_routes_api.Routing
{
    public class KebabCaseTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value)
        {
            if (value == null) return null;

            return Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }
}
