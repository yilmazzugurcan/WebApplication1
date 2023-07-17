using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/devices")]
    public class DeviceController : ControllerBase
    {
        private static List<Device> devices = new List<Device>();

        // POST /api/devices/{deviceId}/keys
        [HttpPost("{deviceId}/keys")]
        public IActionResult AddKeys(string deviceId, [FromBody] List<string> keys)
        {
            if (keys == null || !keys.Any())
            {
                return BadRequest("Invalid keys data");
            }

            var device = devices.FirstOrDefault(d => d.Id == deviceId);

            if (device == null)
            {
                return NotFound("Device not found");
            }

            foreach (var key in keys)
            {
                if (string.IsNullOrEmpty(key) || key.Length < 4 || key.Length > 8)
                {
                    return BadRequest("Invalid key format");
                }

                device.Keys.Add(key);
                device.Address.Keys.Add(key);
            }

            return Ok("Keys added successfully");
        }

        // GET /api/keys?filter={filter}&page={page}&limit={limit}
        [HttpGet("keys")]
        public IActionResult GetKeys(string filter, int page = 1, int limit = 10)
        {
            var filteredKeys = devices.SelectMany(d => d.Keys);

            if (!string.IsNullOrEmpty(filter))
            {
                filteredKeys = filteredKeys.Where(k => k.Contains(filter));
            }

            var paginatedKeys = filteredKeys
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList();

            return Ok(paginatedKeys);
        }

        // PUT /api/keys/{keyId}/invalidate
        [HttpPut("keys/{keyId}/invalidate")]
        public IActionResult InvalidateKey(string keyId)
        {
            try
            {
                // Make a request to another service to mark the key as invalid
                // Example: using HttpClient

                var client = new HttpClient();
                var url = $"https://another.service.com/keys/{keyId}";
                var requestBody = new { state = "invalid" };
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Add necessary headers
                content.Headers.Add("X-API-Key", "foo");

                var response = client.PutAsync(url, content).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    return Ok("Key invalidated successfully");
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Failed to invalidate key");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Failed to invalidate key");
            }
        }
    }
}
