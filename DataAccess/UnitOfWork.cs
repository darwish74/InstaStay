using DataAccess.Data;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Models.IRepositories;
using Models.Models;
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
        private IProblemReports _problemReportRepository; 
        private IHotelManagerRequestsRepository _HotelManagerRequests;
        private INewHotelRequestsRepository _NewHotelRequestsRepository;
        private IHotelManager _HotelManagerRepository;
        private IHotelImagesRepository _HotelImagesRepository;
        private IAmentitiesRepository _AmentitesRepository;
        private IRoomImagesRepository _RoomImagesRepository;
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
        public IProblemReports ProblemReportRepository =>_problemReportRepository ??= new ProblemReportRepository(_context);  
        public IHotelManagerRequestsRepository HotelManagerRequestsRepository => _HotelManagerRequests ??= new HotelManagerRequestsRepository(_context);
        public INewHotelRequestsRepository NewHotelRequestsRepository => _NewHotelRequestsRepository ??= new NewHotelRequestsRepository(_context);
        public IHotelManager HotelManagerRepository => _HotelManagerRepository ??= new HotelManagerRepository(_context);
        public IHotelImagesRepository HotelImagesRepository => _HotelImagesRepository ??= new HotelImageRepository(_context);
        public IAmentitiesRepository AmentitiesRepository => _AmentitesRepository ??= new AmentitiesRepository(_context);
        public IRoomImagesRepository RoomImagesRepository => _RoomImagesRepository ??=new RoomImagesRepository(_context);   
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
