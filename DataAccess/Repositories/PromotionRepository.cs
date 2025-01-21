using DataAccess.Data;
using Models;
using Models.IRepositories;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class PromotionRepository:BaseRepository<Promotion>,IPromotionRepository
    {
        private readonly ApplicationDbContext dbContext;

        public PromotionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
