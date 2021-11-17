using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using todo.Api.Domain.Data;
using todo.Api.Domain.DTO;
using todo.Api.Domain.Entitys;

namespace todo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        ILogger<TodoController> _logger;
        IFreeSql _freeSql;
        public TodoController(ILogger<TodoController> logger, IFreeSql freeSql)
        {
            _logger = logger;
            _freeSql = freeSql;
        }

        [HttpPost("AddToDo")]
        public async void AddToDo(string todoText)
        {
            todolist old = await _freeSql.Select<todolist>().Where(t=>t.todo_text == todoText).FirstAsync();

            if (old == null)
            {
                try
                {
                    int max = _freeSql.Select<todolist>().Max(t => t.sort);

                    _freeSql.Insert<todolist>().AppendData(new todolist
                    {
                        id = Guid.NewGuid().ToString(),
                        todo_text = todoText,
                        sort = max+1
                    }).ExecuteIdentity();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
        [HttpGet("GetList")]
        public async Task<List<ToDoDTO>> GetList()
        {
            var list = await _freeSql.Select<todolist>().ToListAsync();

            List < ToDoDTO > res = new List<ToDoDTO>();
            foreach (var item in list.OrderBy(t=>t.sort))
            {
                res.Add(new ToDoDTO() { number = item.sort,text = item.todo_text});
            }
            return res;
        }

        /// <summary>
        /// 删除并返回现有的todo
        /// </summary>
        /// <param name="todoText"></param>
        /// <returns></returns>
        [HttpPost("DelTodo")]
        public async Task<List<ToDoDTO>> DelTodo(string todoText)
        {
            _freeSql.Delete<todolist>().Where(t=>t.todo_text == todoText).ExecuteAffrows();
            
            var list = _freeSql.Select<todolist>().ToList();

            List<ToDoDTO> res = new List<ToDoDTO>();
            foreach (var item in list.OrderBy(t => t.sort))
            {
                res.Add(new ToDoDTO() { number = item.sort, text = item.todo_text });
            }
            return res;
        }
        /// <summary>
        /// 消息置顶
        /// </summary>
        /// <param name="todoText"></param>
        /// <returns></returns>
        [HttpPost("GoTop")]
        public async Task<List<ToDoDTO>> GoTop(string todoText)
        {
            var oldList = _freeSql.Select<todolist>().OrderBy(t=>t.sort).ToList();

            int pos = 0;
            for (int i = 0; i < oldList.Count; i++)
            {
                if (oldList[i].todo_text == todoText)
                {
                    pos = i;
                    break;
                }
            }

            if (pos > 0)
            {
                var first = oldList[0].sort;
                oldList[0].sort = oldList[pos].sort;
                oldList[pos].sort = first;
                oldList = oldList.OrderBy(t => t.sort).ToList();

                _freeSql.Update<todolist>(oldList).ExecuteAffrows();
            }


            List<ToDoDTO> res = new List<ToDoDTO>();
            foreach (var item in oldList.OrderBy(t => t.sort))
            {
                res.Add(new ToDoDTO() { number = item.sort, text = item.todo_text });
            }
            return res;
        }
    }
}
