using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMicroService.Core.Interfaces;

public interface IRepositoryReader<in TIn, TOut>
{
    Task<TOut> SearchAsync(TIn searchCriteria, CancellationToken cancellationToken);

    Task<TOut> AllAsync(CancellationToken cancellationToken);
}
