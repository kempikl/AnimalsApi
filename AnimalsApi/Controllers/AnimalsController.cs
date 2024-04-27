using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using AnimalsApi.Models;

namespace AnimalsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AnimalsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetAnimals([FromQuery] string orderBy = "Name")
        {
            List<dynamic> animals = new List<dynamic>();
            string query = $"SELECT * FROM Animal ORDER BY {orderBy}";
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(query, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    animals.Add(new
                    {
                        IdAnimal = reader["IdAnimal"],
                        Name = reader["Name"],
                        Description = reader["Description"],
                        Category = reader["CATEGORY"],
                        Area = reader["AREA"]
                    });
                }
            }

            return Ok(animals);
        }

        [HttpPost]
        public IActionResult AddAnimal([FromBody] Animal animal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string query = "INSERT INTO Animal (Name, Description, CATEGORY, AREA) VALUES (@Name, @Description, @Category, @Area)";
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", animal.Name);
                command.Parameters.AddWithValue("@Description", animal.Description);
                command.Parameters.AddWithValue("@Category", animal.Category);
                command.Parameters.AddWithValue("@Area", animal.Area);

                connection.Open();
                int result = command.ExecuteNonQuery();
                if (result < 0)
                    return BadRequest();
            }

            return StatusCode(201);
        }


        [HttpPut("{idAnimal}")]
        public IActionResult UpdateAnimal(int idAnimal, [FromBody] Animal animal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string query = "UPDATE Animal SET Name = @Name, Description = @Description, CATEGORY = @Category, AREA = @Area WHERE IdAnimal = @IdAnimal";
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdAnimal", idAnimal);
                command.Parameters.AddWithValue("@Name", animal.Name);
                command.Parameters.AddWithValue("@Description", animal.Description);
                command.Parameters.AddWithValue("@Category", animal.Category);
                command.Parameters.AddWithValue("@Area", animal.Area);

                connection.Open();
                int result = command.ExecuteNonQuery();
                if (result <= 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }


        [HttpDelete("{idAnimal}")]
        public IActionResult DeleteAnimal(int idAnimal)
        {
            string query = "DELETE FROM Animal WHERE IdAnimal = @IdAnimal";
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdAnimal", idAnimal);
                
                connection.Open();
                int result = command.ExecuteNonQuery();
                if (result < 0)
                    return BadRequest();

                return NoContent();
            }
        }
    }
}
