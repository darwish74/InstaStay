using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IBookingRepository BookingRepository {  get; }  
        IHotelRepository hotelRepository { get; }   
        ILocationRepository locationRepository { get; } 
        IPaymentRepository paymentRepository { get; }
        IRoomRepository roomRepository { get; }
        IReviewRepository reviewRepository { get; } 
        public void Commit();
    }
}
