using BlazorApp.Models;

namespace BlazorApp.Models.Policies
{
    public abstract class IProductPolicy
    {
        public abstract ProductDisponibility CalculateDisponibility(Product product);
    }
}
     