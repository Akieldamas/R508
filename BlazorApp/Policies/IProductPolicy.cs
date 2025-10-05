using BlazorApp.Models;

namespace BlazorApp.Policies
{
    public abstract class IProductPolicy
    {
        public abstract ProductDisponibility CalculateDisponibility(Product product);
    }
}
