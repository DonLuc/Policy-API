using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Policy.API.Data;
using Policy.API.Models;
using Policy.API.Utils;

namespace Policy.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class PolicyHolderController : Controller
    {
        private string encryptKey;
        private readonly PolicyDBContext policyDBContext;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        public PolicyHolderController(PolicyDBContext policyDBContext,/* IEmailSender emailSender,*/ IConfiguration configuration )
        {
            this.policyDBContext = policyDBContext;
            //this._emailSender = emailSender;
            this._configuration = configuration;
            this.encryptKey = this._configuration.GetSection("EncryptionKey").GetSection("key").Value;

        }

        
        //Get all policy holders
        [HttpGet]
        public async Task<IActionResult> GetAllPolicyHolders()
        {
            var message = new Message(new string[] { "lucas.nkoana@yahoo.com" }, "Testing", "Message here");
            _emailSender.sendEmail(message);
            var policyHolders = await policyDBContext.PolicyHolders.ToListAsync();
            policyHolders.ForEach(policyHolder =>
            {
                policyHolder.IdNumber = EncryptionUtil.DecryptString(this.encryptKey, policyHolder.IdNumber);
            });
            return Ok(policyHolders);
        }

        //Get single policy holder
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetPolicyHolder")]
        public async Task<IActionResult> GetPolicyHolder([FromRoute] Guid id)
        {
            var policyHolder = await policyDBContext.PolicyHolders.FirstOrDefaultAsync(x => x.id == id);

            if (policyHolder != null)
            {
                policyHolder.IdNumber = EncryptionUtil.DecryptString(this.encryptKey, policyHolder.IdNumber);
                return Ok(policyHolder);
            }
            else { 
                return NotFound("The Policy Holder does not exist!");
            }
        }

        //Add Policy Holder
        [HttpPost]
        public async Task<IActionResult> CreatePolicyHolder([FromBody] PolicyHolder policyHolder)
        {
            policyHolder.id = Guid.NewGuid();
            policyHolder.IdNumber = EncryptionUtil.EncryptString(this.encryptKey, policyHolder.IdNumber);
            await policyDBContext.PolicyHolders.AddAsync(policyHolder);
            await policyDBContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPolicyHolder), new { id = policyHolder.id } , policyHolder);
        }


        // Update Policy Holder
        [HttpPut]
        [Route("{id:guid}") ]
        public async Task<IActionResult> UpdatePolicyHolder([FromRoute] Guid id, [FromBody] PolicyHolder policyHolder) {
            policyHolder.IdNumber = EncryptionUtil.EncryptString(this.encryptKey, policyHolder.IdNumber);
            var existingPolicyHolder = await policyDBContext.PolicyHolders.FirstOrDefaultAsync(x => x.id == id);

            if (existingPolicyHolder != null) { 
                
                existingPolicyHolder.dob = policyHolder.dob;
                existingPolicyHolder.IdNumber = policyHolder.IdNumber;
                existingPolicyHolder.Surname = policyHolder.Surname;
                existingPolicyHolder.Inititals = policyHolder.Inititals;
                existingPolicyHolder.gender = policyHolder.gender;


                await policyDBContext.SaveChangesAsync();
                return Ok(existingPolicyHolder);
            }
            return NotFound("The Policy Holder with the provided ID is not found");
        
        }

        // Delete Policy Holder
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeletePolicyHolder([FromRoute] Guid id)
        {

            var existingPolicyHolder = await policyDBContext.PolicyHolders.FirstOrDefaultAsync(x => x.id == id);

            if (existingPolicyHolder != null)
            {
                policyDBContext.Remove(existingPolicyHolder);
                await policyDBContext.SaveChangesAsync();
                return Ok(existingPolicyHolder);
            }
            return NotFound("The Policy Holder with the provided ID is not found");

        }



    }
}
