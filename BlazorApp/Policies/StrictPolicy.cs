using BlazorApp.Models;

namespace BlazorApp.Policies
{
    public class StrictPolicy: IProductPolicy
    {
        public override ProductDisponibility CalculateDisponibility(Product product)
        {
            if (product.StockReal.HasValue)
            {
                if (product.StockMin < product.StockReal && product.StockReal < product.StockMax)
                {
                    return ProductDisponibility.Disponible;
                }
                else
                {
                    return ProductDisponibility.Indisponible;
                }
            }
            return ProductDisponibility.Indisponible;
        }
    }
}
