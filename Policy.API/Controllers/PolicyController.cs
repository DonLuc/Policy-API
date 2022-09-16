using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Policy.API.Data;
using Policy.API.Models;
using Policy.API.Utils;

namespace Policy.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class PolicyController : Controller
    {
        private readonly PolicyDBContext policyDBContext;
        private string encryptKey;

        private readonly IConfiguration _configuration;

        public PolicyController(PolicyDBContext policyDBContext, IConfiguration configuration)
        {
            this.policyDBContext = policyDBContext;
            this._configuration = configuration;
            this.encryptKey = this._configuration.GetSection("EncryptionKey").GetSection("key").Value;
        }


        //Get all policies
        [HttpGet]
        public async Task<IActionResult> GetAllPolicies()
        {
            var policies = await policyDBContext.Policies.ToListAsync();

            policies.ForEach(policy =>
            {
                policy.PolicyNumber = EncryptionUtil.DecryptString(this.encryptKey, policy.PolicyNumber);
            });

            return Ok(policies);
        }

        //Get single policy
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetPolicy")]
        public async Task<IActionResult> GetPolicy([FromRoute] Guid id)
        {
            var policy = await policyDBContext.Policies.FirstOrDefaultAsync(x => x.id == id);

            if (policy != null)
            {
                policy.PolicyNumber = EncryptionUtil.DecryptString(encryptKey, policy.PolicyNumber);
                return Ok(policy);
            }
            else
            {
                return NotFound("The Policy does not exist!");
            }
        }


        ////Create Policy
        [HttpPost]
        public async Task<IActionResult> AddPolicy(AvbobPolicy policy) {

            policy.PolicyNumber = EncryptionUtil.EncryptString(this.encryptKey, policy.PolicyNumber);

            policy.id = Guid.NewGuid();
            await policyDBContext.Policies.AddAsync(policy);
            await policyDBContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPolicy), new { id = policy.id }, policy);

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeletePolicy([FromRoute] Guid id)
        {

            var existingPolicy = await policyDBContext.Policies.FirstOrDefaultAsync(x => x.id == id);

            if (existingPolicy != null)
            {
                policyDBContext.Remove(existingPolicy);
                await policyDBContext.SaveChangesAsync();
                return Ok(existingPolicy);
            }
            return NotFound("The Policy with the provided ID is not found");
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdatePolicy([FromRoute] Guid id, [FromBody] AvbobPolicy policy)
        {
            policy.PolicyNumber = EncryptionUtil.EncryptString(this.encryptKey, policy.PolicyNumber);
            var existingPolicy = await policyDBContext.Policies.FirstOrDefaultAsync(x => x.id == id);

            if (existingPolicy != null)
            {

                existingPolicy.PolicyNumber = policy.PolicyNumber;
                existingPolicy.CommencementDate = policy.CommencementDate;
                existingPolicy.PolicyType = policy.PolicyType;
                existingPolicy.Installment = policy.Installment;

                await policyDBContext.SaveChangesAsync();
                return Ok(existingPolicy);
            }
            return NotFound("The Policy Holder with the provided ID is not found");

        }







    }
}
