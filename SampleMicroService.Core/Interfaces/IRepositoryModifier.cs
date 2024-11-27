using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMicroService.Core.Interfaces;

public interface IRepositoryModifier<TIdType, in TDocumentType>
{
    Task<TIdType> Save(TDocumentType item, CancellationToken cancellationToken) => Save(default, item, cancellationToken);

    Task<TIdType> Save(TIdType id, TDocumentType item, CancellationToken cancellationToken);

    Task<bool> RemoveById(TIdType id, CancellationToken cancellationToken);
}
