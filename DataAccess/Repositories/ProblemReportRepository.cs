using DataAccess.Data;
using Models.IRepositories;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ProblemReportRepository : BaseRepository<ProblemReport>,IProblemReports
    {
        private readonly ApplicationDbContext _context;
        public ProblemReportRepository(ApplicationDbContext _context) : base(_context)
        {
            this._context = _context;
        }
    }
}
