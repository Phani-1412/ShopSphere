using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopSphere.Data;
using ShopSphere.DTO;
using ShopSphere.Services;
using System.Security.Claims;

namespace ShopSphere.Controllers

{

    [Route("api/[controller]")]

    [ApiController]

    public class SellerStoreController : ControllerBase
    {


        private readonly ISellerStoreService _sellerStoreService;
        private readonly ApplicationDbContext _context;

        public SellerStoreController(ApplicationDbContext context, ISellerStoreService sellerStoreService)

        {
            _context = context;
            _sellerStoreService = sellerStoreService;

        }


        // POST: api/SellerStore/create    
        [Authorize(Roles ="Seller")]

        [HttpPost("create")]

        public async Task<IActionResult> CreateSellerStore(CreateSellerStoreAsyncDTO dto)

        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var seller = await _context.Sellers
                .FirstOrDefaultAsync(s => s.UserID == userId);

            if (seller == null)
                return BadRequest("Seller not found");

            var result = await _sellerStoreService.CreateSellerStoreAsync(seller.SellerID, dto);

            return Ok(new { message = result });

        }

        [Authorize(Roles = "Seller")]
        // GET: api/SellerStore/my-stores    
        [HttpGet("my-stores")]
        public async Task<IActionResult> GetAllMyStores()

        {

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);


            var stores = await _sellerStoreService.GetAllSellersStoresAsync(userId);

            return Ok(stores);

        }


        [Authorize(Roles ="Seller")]// PUT: api/SellerStore/update-status/{storeId}    
        [HttpPut("update-status/{storeId}")]

        public async Task<IActionResult> UpdateStatus(int storeId, [FromBody] UpdateStoreStatusAsyncDTO dto)

        {

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);


            var result = await _sellerStoreService.UpdateStoreStatusAsync(userId, storeId, dto);

            return Ok(new { message = result });

        }

        [Authorize(Roles="Seller")]
        // DELETE: api/SellerStore/delete/{storeId}    
        [HttpDelete("delete/{storeId}")]

        public async Task<IActionResult> DeleteStore(int storeId)

        {

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);


            var result = await _sellerStoreService.DeleteSellerStoreAsync(userId, storeId);

            return Ok(new { message = result });

        }


    }

}