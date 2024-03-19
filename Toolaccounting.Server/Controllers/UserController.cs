using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Toolaccounting.Server.Data;
using Toolaccounting.Server.Models;

namespace Toolaccounting.Server.Controllers
{
    [Route("[controller]/{id?}")]
    public class UserController : Controller
    {
        ApplicationContext db;
        public UserController(ApplicationContext context)
        {
            db = context;
        }
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var users = await db.Users.ToListAsync();
            return Json(users);
        }
        [HttpPost]
        public async Task<ActionResult> AddUser([FromBody]User user)
        {
            if (user == null||string.IsNullOrWhiteSpace(user.FullName))
            {
                return BadRequest();
            }
            db.Users.Add(user);
            db.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Проверяем инструменты у пользователя
        /// Возращаем их на склад 
        /// Удаляем запись
        /// </summary>

        [HttpDelete]
        public async Task<ActionResult> DeleteUser(uint id)
        {
            if (id == null||id==0 )
            {
                return BadRequest();
            }
            var user=await db.Users.Where(user=>user.Id==id).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound("User not found");
            }
            
            var toolsUsed = await db.ToolsUsed.Where(user => user.UserId == id).ToListAsync();
            if (toolsUsed == null)
            {
                db.Users.Remove(user);
                await db.SaveChangesAsync();
                return Ok();
            }
            var tools = await db.Tools.ToListAsync();
            for (int i = 0; i < toolsUsed.Count; i++)
            {
                var tool = tools.Where(tool => tool.Id == toolsUsed[i].ToolId).FirstOrDefault();
                if (tool == null)
                    return NotFound("Error when delete tools from user");
                tool.Count += toolsUsed[i].ToolCount;

            }
            db.Users.Remove(user);
            await db.SaveChangesAsync();


            return Ok();
        }
    }
}
