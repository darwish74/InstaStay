using DataAccess.Data;
using Models;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    internal class PaymentRepository:BaseRepository<Payment>, IPaymentRepository
    {
        private readonly ApplicationDbContext dbContext;

        public PaymentRepository(ApplicationDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
