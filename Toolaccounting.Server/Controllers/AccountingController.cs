using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Toolaccounting.Server.Data;
using Toolaccounting.Server.Models;

namespace Toolaccounting.Server.Controllers
{

    [Route("[controller]/[action]/{id?}/{countToDel?}")]
    public class AccountingController : Controller
    {
        ApplicationContext db;
        public AccountingController(ApplicationContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var toolsUsed = await db.ToolsUsed.Include(x=>x.Tool).Include(x=>x.User).ToListAsync();
            var users = await db.Users.ToListAsync();
            var resp = new {toolsUsed, users};

            return Json(resp);
        }

        [HttpGet]
        public async Task<ActionResult> GetTools()
        {
            var tools = await db.Tools.Where(item=>item.Count>0).ToListAsync();
            return Json(tools);
        }

        [HttpPost]
        public async Task<ActionResult> addNewRowUsedTools([FromBody] ToolUsed toolUsed)
        {
            if (toolUsed==null)
                return BadRequest("Object is Null");
            var tool=await db.Tools.Where(x=>x.Id==toolUsed.ToolId).FirstOrDefaultAsync();
            if(tool==null)
                return NotFound("Tool not founded");
            if (tool.Count < toolUsed.ToolCount)
                return BadRequest("tool.Count<toolUsed.ToolCount");

            tool.Count-=toolUsed.ToolCount;

            await db.ToolsUsed.AddAsync(toolUsed);
            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(uint id,uint countToDel)
        {
            var row=await db.ToolsUsed.Where(item=>item.Id==id).FirstOrDefaultAsync();
            
            if (row == null)
                return NotFound("Row not found");
            var tools = await db.Tools.Where(item => item.Id == row.ToolId).FirstOrDefaultAsync();
            if (tools == null)
                return NotFound("Tool not found");

            row.ToolCount -= countToDel;
            tools.Count += countToDel;

            if (row.ToolCount == 0)
            db.ToolsUsed.Remove(row);

            await db.SaveChangesAsync();
            return Ok();
        }


    }
}
