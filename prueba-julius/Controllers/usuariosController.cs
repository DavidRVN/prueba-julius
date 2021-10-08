using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using prueba_julius.Context;
using prueba_julius.Models;


namespace prueba_julius.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuariosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public usuariosController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _configuration = config;
        }

        // GET: api/usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<usuarios>>> Getusuarios()
        {
            return await _context.usuarios.ToListAsync();
        }

        // GET: api/usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<usuarios>> Getusuarios(int id)
        {
            var usuarios = await _context.usuarios.FindAsync(id);

            if (usuarios == null)
            {
                return NotFound();
            }

            return usuarios;
        }

        // PUT: api/usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putusuarios(int id, usuarios usuarios)
        {
            if (id != usuarios.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuarios).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!usuariosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<usuarios>> Postusuarios(usuarios usuarios)
        {
            _context.usuarios.Add(usuarios);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getusuarios", new { id = usuarios.Id }, usuarios);
        }

        // DELETE: api/usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteusuarios(int id)
        {
            var usuarios = await _context.usuarios.FindAsync(id);
            if (usuarios == null)
            {
                return NotFound();
            }

            _context.usuarios.Remove(usuarios);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool usuariosExists(int id)
        {
            return _context.usuarios.Any(e => e.Id == id);
        }


        // crear el token de autenticación

        [HttpGet("{username}/{password}")]
        public ActionResult<List<usuarios>> GetLogin(string username, string password)
        {
            try
            {
                var users = _context.usuarios.Where(user_bd => user_bd.usuario.Equals(username) && user_bd.contrasena.Equals(password)).ToList();
                if (users == null)
                {
                    return NotFound();
                }
                // Console.WriteLine(  GenerateToken(username));
              return users;
              // return Ok(GenerateToken(username));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private string GenerateToken(string username)
        {

       
            //Header
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            //Claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,username)
             
            };

            //Payload
            var payload = new JwtPayload
            (
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claims,
                DateTime.Now,
                DateTime.UtcNow.AddMinutes(10)
            );

            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
