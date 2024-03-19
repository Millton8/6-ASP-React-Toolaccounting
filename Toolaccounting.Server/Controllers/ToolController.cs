using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Toolaccounting.Server.Data;
using Toolaccounting.Server.Models;

namespace Toolaccounting.Server.Controllers
{
    [Route("[controller]/{id?}")]
    public class ToolController : Controller
    {
        ApplicationContext db;
        public ToolController(ApplicationContext context)
        {
            db = context;
        }
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var tools = await db.Tools.ToListAsync();
            return Json(tools);
        }
        /// <summary>
        /// Если Есть похожий по имени инструмент добавляем количество
        /// </summary>
        /// <param name="tool">Принимаем Имя и Количество</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddTool([FromBody] Tool tool)
        {
            if (tool == null || string.IsNullOrWhiteSpace(tool.Name))
            {
                return BadRequest();
            }
            Console.WriteLine(tool.Name+" "+tool.Count);
            var toolCheck=await db.Tools.Where(item => item.Name == tool.Name).FirstOrDefaultAsync();
            Console.WriteLine("tc"+ toolCheck??"Null");
            if (toolCheck == null)
                await db.Tools.AddAsync(tool);
            else 
                toolCheck.Count += tool.Count;

            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteTool(uint id)
        {
            if (id == null || id == 0)
            {
                return BadRequest();
            }
            var tool = await db.Tools.Where(tool => tool.Id == id).FirstOrDefaultAsync();
            if (tool == null)
            {
                return NotFound("Tool not found");
            }
            db.Tools.Remove(tool);
            await db.SaveChangesAsync();

           


            return Ok();
        }
    }
}
