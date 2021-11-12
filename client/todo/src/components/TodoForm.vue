<template>
  <div>
    <el-row>
        <el-col :span="10">
            <el-input v-model="todoText"></el-input>
        </el-col>        
        <el-button :span="10" @click="do_post">post</el-button>
        <el-col :span="10">
            <el-input v-model="msgText"></el-input>
        </el-col>   
        <el-button :span="10" @click="send">发送消息</el-button>
        <el-input v-model="receiveMsg"></el-input>
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
        debug_base_url:"http://1.15.248.70:8089",//http://localhost:5189
        prod_base_url:"http://1.15.248.70:8089",
        todoText:"",
        url : "Todo/",
        todoList:[],
        connection:"",
        msgText:"",
        receiveMsg:''
      }
  },
  methods:{
    send()
    {
      this.$data.connection.invoke("guang_bo", this.$data.msgText);
    },
      do_post(){ 
          axios.post(`${this.$data.debug_base_url}/api/Todo/AddToDo?todoText=` + this.$data.todoText)
          .then(()=>{
            axios.get(`${this.$data.debug_base_url}/api/Todo/GetList`)
            .then((res)=>{
                this.$data.todoList = res.data;
            });              
          });
      },
      deltodo(t_text)
      {
        axios.post(`${this.$data.debug_base_url}/api/Todo/DelTodo?todoText=` + t_text)
        .then((res)=>{
          this.$data.todoList = res.data;           
        });
      },
      gotop(t_text)
      {
        axios.post(`${this.$data.debug_base_url}/api/Todo/GoTop?todoText=` + t_text)
          .then((res)=>{
            this.$data.todoList = res.data;            
          });
      }
  },
  mounted(){
    axios.get(`${this.$data.debug_base_url}/api/Todo/GetList`)
    .then((res)=>{
        this.$data.todoList = res.data;
    });

     this.$data.connection = new signalR.HubConnectionBuilder()
    .withUrl(`${this.$data.debug_base_url}/ChatHub`
      , 
      {
            skipNegotiation: true,  
            transport: signalR.HttpTransportType.WebSockets
      }
    )     //debug
    .configureLogging(signalR.LogLevel.Error).build();
    
     this.$data.connection.start();

    this.$data.connection.on("guang_bo", data => {
        console.log(data);
        this.$data.receiveMsg += data;
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
