﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using todo.Api.Domain.Data;
using todo.Api.Domain.DTO;

namespace todo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        ILogger<TodoController> _logger;
        public TodoController(ILogger<TodoController> logger)
        {
            _logger = logger;
        }

        [HttpPost("AddToDo")]
        public void AddToDo(string todoText)
        {
            var oldList = RedisHelper.Get<List<ToDoDTO>>(RedisKeys.todoList);

            if (oldList == null || oldList.Count <= 0)
            {
                oldList = new List<ToDoDTO>();
                oldList.Add(new ToDoDTO
                {
                    number = 1,text = todoText
                });
            }
            else
            {
                oldList.Add(new ToDoDTO { number = oldList.Max(t=>t.number) + 1,text = todoText });
            }
            RedisHelper.Set(RedisKeys.todoList,oldList);
        }      
        [HttpGet("GetList")]
        public async Task<List<ToDoDTO>> GetList()
        {
            return await RedisHelper.GetAsync<List<ToDoDTO>>(RedisKeys.todoList);
        }

        /// <summary>
        /// 删除并返回现有的todo
        /// </summary>
        /// <param name="todoText"></param>
        /// <returns></returns>
        [HttpPost("DelTodo")]
        public async Task<List<ToDoDTO>> DelTodo(string todoText)
        {
            var oldList = await RedisHelper.GetAsync<List<ToDoDTO>>(RedisKeys.todoList);

            var findTodo = oldList.Where(t => t.text == todoText).FirstOrDefault();

            if (findTodo != null)
            {
                oldList.Remove(findTodo);
                RedisHelper.Set(RedisKeys.todoList, oldList);
            }

            return oldList;
        }
        /// <summary>
        /// 消息置顶
        /// </summary>
        /// <param name="todoText"></param>
        /// <returns></returns>
        [HttpPost("GoTop")]
        public async Task<List<ToDoDTO>> GoTop(string todoText)
        {
            var oldList = await RedisHelper.GetAsync<List<ToDoDTO>>(RedisKeys.todoList);

            int pos = 0;
            for (int i = 0; i < oldList.Count; i++)
            {
                if (oldList[i].text == todoText)
                {
                    pos = i;
                    break;
                }
            }

            if (pos > 0)
            {
                var first = oldList[0].number;
                oldList[0].number = oldList[pos].number;
                oldList[pos].number = first;
                oldList = oldList.OrderBy(t => t.number).ToList();

                RedisHelper.Set(RedisKeys.todoList, oldList);
            }

            return oldList;
        }
    }
}