using OSharp.Data.Entity;
using Solution.ProductManagement.Models;
using System;

namespace Solution.ProductManagement.ModelConfigurations
{
    public class ProductInfoConfigration : EntityConfigurationBase<ProductInfo, Guid>
    {
        public ProductInfoConfigration()
        {
            HasRequired(m => m.ProductType).WithMany();
        }
    }
}
