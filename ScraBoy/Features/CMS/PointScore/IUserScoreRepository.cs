using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.CMS.PointScore
{
    public interface IUserScoreRepository
    {
        Task Create(UserScore model);
        Task<IEnumerable<UserScore>> TrackScoreByUser(string userId);
        Task<UserScore> FindOne(int id);
        Task Delete(UserScore model);
        Task Edit(UserScore model);

    }
}