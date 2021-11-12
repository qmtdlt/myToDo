<template>
  <div>
    <el-row>
        <el-col :span="10">
            <el-input v-model="todoText"></el-input>
        </el-col>        
        <el-button :span="10" @click="do_post">post</el-button>
        <el-button :span="10" @click="send">发送消息</el-button>
    </el-row>
    <el-row>
      <el-col :span="11">
        <el-card>
          <el-row v-for="item in todoList" v-bind:key="item">
              <el-col :span="16">
                <p>{{item.number}} -- {{item.text}}</p>
              </el-col>
              <el-col :span="3">
                <el-button @click="deltodo(item.text)">删除</el-button>
              </el-col>
              <el-col :span="3">
                <el-button @click="gotop(item.text)">置顶</el-button>
              </el-col>
          </el-row>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script>
import axios from 'axios'

import * as signalR from "@aspnet/signalr";

export default {
  name: 'ToDoForm',
  props: {
    msg: String
  },
  data()
  {
      return {
        todoText:"",
        url : "Todo/",
        todoList:[],
        connection:""
      }
  },
  methods:{
    send()
    {
      this.$data.connection.invoke("guang_bo", "asdfasdf");
    },
      do_post(){ 
          axios.post(`http://localhost:5189/api/Todo/AddToDo?todoText=` + this.$data.todoText)
          .then(()=>{
            axios.get(`http://localhost:5189/api/Todo/GetList`)
            .then((res)=>{
                console.log("刷新todo");
                this.$data.todoList = res.data;
                console.log(this.$data.todoList);
            });              
          });
      },
      deltodo(t_text)
      {
        axios.post(`http://localhost:5189/api/Todo/DelTodo?todoText=` + t_text)
        .then((res)=>{
          console.log("刷新todo");
          this.$data.todoList = res.data;
          console.log(this.$data.todoList);              
        });
      },
      gotop(t_text)
      {
        axios.post(`http://localhost:5189/api/Todo/GoTop?todoText=` + t_text)
          .then((res)=>{
            console.log("刷新todo");
            this.$data.todoList = res.data;
            console.log(this.$data.todoList);              
          });
      }
  },
  mounted(){
    axios.get(`http://localhost:5189/api/Todo/GetList`)
    .then((res)=>{
        this.$data.todoList = res.data;
    });

     this.$data.connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5189/ChatHub", {
          skipNegotiation: true,  
          transport: signalR.HttpTransportType.WebSockets
        })     //debug
    .configureLogging(signalR.LogLevel.Error).build();
    
     this.$data.connection.start().then(()=>{
       
    });

    this.$data.connection.on("guang_bo", data => {
        console.log(data);
      });
  }
}
</script>
<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
p{
  text-align: left;
}
</style>
