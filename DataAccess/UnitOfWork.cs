using DataAccess.Data;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Models.IRepositories;
using System;

namespace DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IBookingRepository _bookingRepository;
        private IHotelRepository _hotelRepository;
        private ILocationRepository _locationRepository;
        private IPaymentRepository _paymentRepository;
        private IRoomRepository _roomRepository;
        private IReviewRepository _reviewRepository;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public IBookingRepository BookingRepository => _bookingRepository ??= new BookingRepository(_context);
        public IHotelRepository hotelRepository => _hotelRepository ??= new HotelRepository(_context);
        public ILocationRepository locationRepository => _locationRepository ??= new LocationRepository(_context);
        public IPaymentRepository paymentRepository => _paymentRepository ??= new PaymentRepository(_context);
        public IRoomRepository roomRepository => _roomRepository ??= new RoomRepository(_context);
        public IReviewRepository reviewRepository => _reviewRepository ??= new ReviewRepository(_context);
        public void Commit()
        {
            _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
