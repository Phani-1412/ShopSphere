using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.Models;
using ShopSphere.Services;

namespace ShopSphere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyService _service;

        public PolicyController(IPolicyService service)
        {
            _service = service;
        }

        // POST: api/policy
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Policy policy)
        {
            if (policy == null || string.IsNullOrWhiteSpace(policy.Title) || string.IsNullOrWhiteSpace(policy.Content))
                return BadRequest("Invalid policy data.");

            var result = await _service.CreatePolicyAsync(policy);
            return Ok(new { message = result });
        }

        // GET: api/policy
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var policies = await _service.GetAllPoliciesAsync();
            return Ok(policies);
        }

        // GET: api/policy/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var policy = await _service.GetPolicyByIdAsync(id);

            if (policy == null)
                return NotFound("Policy not found.");

            return Ok(policy);
        }

        // PUT: api/policy/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Policy policy)
        {
            if (policy == null)
                return BadRequest("Invalid data.");

            var result = await _service.UpdatePolicyAsync(id, policy);

            if (result == "Policy not found.")
                return NotFound(result);

            return Ok(new { message = result });
        }

        // DELETE: api/policy/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeletePolicyAsync(id);

            if (result == "Policy not found.")
                return NotFound(result);

            return Ok(new { message = result });
        }
    }
}
