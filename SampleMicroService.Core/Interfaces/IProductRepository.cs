using SampleMicroService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMicroService.Core.Interfaces;

public interface IProductRepository :
IRepositoryReader<ProductSpecifications,IEnumerable<ProductSpecifications>>,
    IRepositoryModifier<string, ProductSpecifications>;
