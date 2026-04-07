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
                return "Only users with Seller role can create seller profile.";

            if (await _context.Sellers.AnyAsync(s => s.UserID == userId))
                return "Seller profile already exists.";

            var seller = new Seller
            {
                UserID = userId,
                StoreName = dto.StoreName
            };

            _context.Sellers.Add(seller);
            await _context.SaveChangesAsync();

            return "Seller profile created. Awaiting admin approval.";
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
            var seller = await _context.Sellers.FindAsync(sellerId);

            if (seller == null)
                return false;

            seller.ComplianceStatus = "Approved";
            seller.RejectionReason = null;
            seller.ReviewedByAdminId = adminUserId;

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
