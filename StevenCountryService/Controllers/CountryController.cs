using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StevenCountryService.Data;
using StevenCountryService.Models;

namespace StevenCountryService.Controllers
{
    [Route("api/countries")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly SqliteDbContext _context;
        private readonly ILogger<CountryController> _logger;

        public CountryController(SqliteDbContext context, ILogger<CountryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET api/countries
        [HttpGet(Name = "GetCountries")]
        public async Task<ActionResult<Country>> GetCountries(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var countries = await _context.Countries.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                return new OkObjectResult(countries);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { message = ex.Message });
            }
        }

        // GET api/countries/{country_id}
        [HttpGet("{country_id}", Name = "GetCountryById")]
        public async Task<ActionResult<Country>> GetCountryById(string country_id)
        {
            try
            {
                var country = await _context.Countries.FirstOrDefaultAsync(p => p.CountryId.ToLower() == country_id.ToLower());

                if (country == null) return new NotFoundObjectResult(new { message = $"Country with ID [{country_id}] not found." });

                return new OkObjectResult(country);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { message = ex.Message });
            }
        }

        // POST api/countries
        [HttpPost(Name = "InsertCountry")]
        public async Task<ActionResult<Country>> InsertCountry([FromBody] Country newCountry)
        {
            try
            {
                // Validation when country is empty is already implemented inside the Models.

                var country = await _context.Countries.FirstOrDefaultAsync(p => p.CountryId.ToLower() == newCountry.CountryId.ToLower());

                if (country != null) return new NotFoundObjectResult(new { message = $"Country with ID [{newCountry.CountryId}] already exists." });

                newCountry.CountryId = newCountry.CountryId.ToUpper();
                _context.Countries.Add(newCountry);
                await _context.SaveChangesAsync();

                return new OkObjectResult(new { message = $"Success insert country [{newCountry.CountryId}].", data = newCountry });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { message = ex.Message });
            }
        }

        // POST api/countries
        [HttpPost("list", Name = "InsertCountries")]
        public async Task<ActionResult<Country>> InsertCountries([FromBody] List<Country> listCountry)
        {
            try
            {
                var existingCountryIds = await _context.Countries.Select(p => p.CountryId).ToListAsync();

                // Filter out country that already exists
                var newCountries = listCountry.Where(c => !existingCountryIds.Contains(c.CountryId)).ToList();

                foreach (var newCountry in newCountries)
                {
                    _context.Countries.Add(newCountry);
                }
                await _context.SaveChangesAsync();

                return new OkObjectResult(new { message = $"Success insert countries.", data = newCountries });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { message = ex.Message });
            }
        }

        // PUT api/countries/{country_id}
        [HttpPatch("{country_id}", Name = "UpdateCountry")]
        public async Task<ActionResult<Country>> UpdateCountry(string country_id, [FromBody] Country updateCountry)
        {
            try
            {
                var country = await _context.Countries.FirstOrDefaultAsync(p => p.CountryId.ToLower() == country_id.ToLower());

                if (country == null) return new NotFoundObjectResult(new { message = $"Country with ID [{country_id}] not found." });

                country.Name = updateCountry.Name;
                country.CallingCode = updateCountry.CallingCode;
                await _context.SaveChangesAsync();

                return new OkObjectResult(new { message = $"Success update country [{country.CountryId}].", data = country });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { message = ex.Message });
            }
        }

        // DELETE api/countries/{country_id}
        [HttpDelete("{country_id}", Name = "DeleteCountry")]
        public async Task<ActionResult<Country>> DeleteCountry(string country_id)
        {
            try
            {
                var country = await _context.Countries.FirstOrDefaultAsync(p => p.CountryId.ToLower() == country_id.ToLower());

                if (country == null) return new NotFoundObjectResult(new { message = $"Country with ID [{country_id}] not found." });

                _context.Countries.Remove(country);
                await _context.SaveChangesAsync();

                return new OkObjectResult(new { message = $"Success delete country [{country.CountryId}].", data = country });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { message = ex.Message });
            }
        }
    }
}
