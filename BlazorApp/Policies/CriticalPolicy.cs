using BlazorApp.Models;

namespace BlazorApp.Policies
{
    public class CriticalPolicy: IProductPolicy
    {
        public override ProductDisponibility CalculateDisponibility(Product product)
        {
            if (product.StockReal.HasValue)
            {
                if (product.StockReal < product.StockMin)
                {
                    return ProductDisponibility.Indisponible;
                }
                else if (product.StockReal > product.StockMax)
                {
                    return ProductDisponibility.Blocked;
                }
            }
            return ProductDisponibility.Indisponible;
        }
    }
}
