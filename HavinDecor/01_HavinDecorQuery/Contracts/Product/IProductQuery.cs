﻿using System.Collections.Generic;

namespace _01_HavinDecorQuery.Contracts.Product
{
    public interface IProductQuery
    {
        List<ProductQueryModel> GetLatestArrivals();
    }
}
