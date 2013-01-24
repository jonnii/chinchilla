namespace Chinchilla.Api
{
    public class BindingOptions
    {
        public static BindingOptions Default
        {
            get
            {
                return new BindingOptions { RoutingKey = "#" };
            }
        }

        public string RoutingKey { get; set; }
    }
}
