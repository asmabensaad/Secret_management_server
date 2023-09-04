using Core.Security;
using Core.Security.Cryptographie;
using Microsoft.AspNetCore.Mvc;

namespace Services.Kms.Controllers;
[ApiController]
[Route("/api/v1.0/encryption/[controller]/[action]")]
public class CryptoController: ControllerBase
{
    private readonly ICrypto _crypto;
    private readonly IKmsVaultClient _client;
    
        public CryptoController(ICrypto crypto, IKmsVaultClient client)
        {
            _crypto = crypto;
            _client = client;
        }

        [HttpGet("generate-key")]
         public IActionResult GenerateEncryptionKey()
         {
             var generatedKey = _crypto.GenerateEncryptionKey();
             return Ok(generatedKey);
         }
    

        [HttpPost]
        public IActionResult EncryptionData([FromBody] byte[] data,[FromQuery] byte[] key, [FromQuery] Crypto.Algorithm algorithm)
        {
                var encryptedData = _crypto.Encrypt(data, algorithm, key);
                var response = new
                {
                    Algorithm = algorithm,
                    Key = key,
                    EncryptedData = encryptedData
                };
                return Ok(response);
          
              //  return BadRequest("Encryption failed: " + e.Message);
            
         
        }


        [HttpPost]

        public IActionResult DecryptionData([FromBody] byte[] encryptedData, [FromQuery] byte[] key,
            [FromQuery] Crypto.Algorithm algorithm)
        {
            try
            {
                var decryptedData = _crypto.Decrypt(encryptedData, algorithm, key);
                return Ok(decryptedData);
            }
            catch (Exception ex)
            {
                return BadRequest("Decryption failed: " + ex.Message);
            }
            
        }
    
    
    
    
}