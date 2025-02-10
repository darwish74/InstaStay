using Microsoft.AspNetCore.Http;
using Models.Models;
using Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.IRepositories
{
    public interface IProblemReports : IBaseRepositories<ProblemReport>
    {
        void CreateWithImage(ProblemReport problemReport, IFormFile imgFile, string v1, string v2);
    }
}
