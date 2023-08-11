using Cbms.Dependency;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Integration
{
    public interface IRewardAppManager : ISingletonDependency
    {
        Task<object> FetchSpoon(string spoonCode);
       
        Task<bool> SyncQrCode(string shopCode, DateTime scanDate, DateTime beginDate, DateTime endDate, List<string> qrCodes);
    }
}
