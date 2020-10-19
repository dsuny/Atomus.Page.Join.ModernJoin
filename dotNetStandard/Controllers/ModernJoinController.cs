using Atomus.Database;
using Atomus.Service;
using System;
using System.Threading.Tasks;

namespace Atomus.Page.Join.Controllers
{
    internal static class ModernJoinController
    {
        internal static async Task<IResponse> SaveAsync(this ICore core, string EMAIL, string ACCESS_NUMBER, string NICKNAME, decimal REFERRAL_USER_ID)
        {
            IServiceDataSet serviceDataSet;

            serviceDataSet = new ServiceDataSet { ServiceName = core.GetAttribute("ServiceName") };
            serviceDataSet["JOIN"].ConnectionName = core.GetAttribute("DatabaseName");
            serviceDataSet["JOIN"].CommandText = core.GetAttribute("ProcedureJoin");
            serviceDataSet["JOIN"].AddParameter("@EMAIL", DbType.NVarChar, 100);
            serviceDataSet["JOIN"].AddParameter("@ACCESS_NUMBER", DbType.NVarChar, 4000);
            serviceDataSet["JOIN"].AddParameter("@NICKNAME", DbType.NVarChar, 50);
            serviceDataSet["JOIN"].AddParameter("@REFERRAL_USER_ID", DbType.Decimal, 18);
            serviceDataSet["JOIN"].AddParameter("@USER_ID", DbType.Decimal, 18);

            serviceDataSet["JOIN"].NewRow();
            serviceDataSet["JOIN"].SetValue("@EMAIL", EMAIL.EmptyToDBNullValue());
            serviceDataSet["JOIN"].SetValue("@ACCESS_NUMBER", ACCESS_NUMBER.EmptyToDBNullValue());
            serviceDataSet["JOIN"].SetValue("@NICKNAME", NICKNAME.EmptyToDBNullValue());
            if (REFERRAL_USER_ID <= 0)
                serviceDataSet["JOIN"].SetValue("@REFERRAL_USER_ID", DBNull.Value);
            else
                serviceDataSet["JOIN"].SetValue("@REFERRAL_USER_ID", REFERRAL_USER_ID);

            return await core.ServiceRequestAsync(serviceDataSet);
        }
    }
}
