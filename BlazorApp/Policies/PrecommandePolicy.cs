using BlazorApp.Models;

namespace BlazorApp.Policies
{
    public class PrecommandePolicy: IProductPolicy
    {
        public override ProductDisponibility CalculateDisponibility(Product product)
        {
            if (product.StockReal.HasValue)
            {
                if (product.StockReal < product.StockMin)
                {
                    return ProductDisponibility.Precommande;
                }
                return ProductDisponibility.Disponible;
            }

            return ProductDisponibility.Indisponible;
        }
    }
}
