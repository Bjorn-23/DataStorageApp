﻿using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Interfaces;

public interface IPriceListRepository : IBaseRepository<PriceListEntity, ProductCatalog>
{
}