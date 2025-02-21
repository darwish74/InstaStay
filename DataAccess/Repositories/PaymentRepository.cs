using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
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
    public class PaymentRepository:BaseRepository<Payment>, IPaymentRepository
    {
        private readonly ApplicationDbContext dbContext;

        public PaymentRepository(ApplicationDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> GetPendingPaymentsCountAsync()
        {
            return await dbContext.payments.CountAsync(p => p.PaymentStatus != "Paid");
        }
    }
}
