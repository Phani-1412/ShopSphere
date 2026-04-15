using Microsoft.EntityFrameworkCore;
using ShopSphere.Data;
using ShopSphere.DTO;
using ShopSphere.Models;

namespace ShopSphere.Services
{
    public class SellerService : ISellerService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuditService _auditService;

        public SellerService(ApplicationDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        public async Task<string> CreateSellerAsync(int userId, CreateSellerDto dto)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null || user.Role != "Seller")
                return "Only users with Seller role can create a seller profile.";

            if (await _context.Sellers.AnyAsync(s => s.UserID == userId))
                return "Seller profile already exists.";
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var seller = new Seller
                {
                    UserID = userId,
                    StoreName = dto.StoreName,
                    ComplianceStatus = "Pending"
                };

                _context.Sellers.Add(seller);
                await _context.SaveChangesAsync();
                var initialStore = new SellerStore
                {
                    SellerID = seller.SellerID,
                    CategoryFocus = "General",
                    Rating = 0,
                    Status = "Pending" 
                };

                _context.SellerStores.Add(initialStore);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return "Seller profile and initial store created. Awaiting admin approval.";
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return "An error occurred during registration. Please try again.";
            }
        }

        public async Task<IEnumerable<SellerResponseDto>> GetAllSellersAsync()
        {
            return await _context.Sellers
                .Select(s => new SellerResponseDto
                {
                    SellerId = s.SellerID,
                    StoreName = s.StoreName,
                    ComplianceStatus = s.ComplianceStatus
                }).ToListAsync();
        }

        public async Task<bool> ApproveSellerAsync(int sellerId, int adminUserId)
        {
            var seller = await _context.Sellers
                .Include(s => s.SellerStores)
                .FirstOrDefaultAsync(s => s.SellerID == sellerId);

            if (seller == null)
                return false;
            seller.ComplianceStatus = "Approved";
            seller.RejectionReason = null;
            seller.ReviewedByAdminId = adminUserId;
            if (seller.SellerStores != null)
            {
                foreach (var store in seller.SellerStores)
                {
                    if (store.Status == "Pending")
                    {
                        store.Status = "Active";
                    }
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<SellerResponseDto> GetSellerByUserIdAsync(int userId)
        {
            var seller = await _context.Sellers
                .Where(s => s.UserID == userId)
                .Select(s => new SellerResponseDto
                {
                    SellerId= s.SellerID,
                    StoreName = s.StoreName,
                    ComplianceStatus = s.ComplianceStatus
                }).FirstOrDefaultAsync();

            return seller;
        }

        public async Task<bool> RejectSellerAsync(int sellerId, int adminUserID, string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                return false;

            var seller = await _context.Sellers.FindAsync(sellerId);

            if (seller == null)
                return false;

            seller.ComplianceStatus = "Rejected";
            seller.RejectionReason = reason;

            seller.ReviewedByAdminId = adminUserID;

            await _context.SaveChangesAsync();
            return true;
        }


    }
}
