<template>
  <div>
    <!-- 输入提交 -->
    <el-row>
        <el-col :span="15">
            <el-input v-model="todoText"></el-input>
        </el-col>        
        <el-button :span="2" @click="do_post" size="mini">post</el-button>        
    </el-row>

    <el-row>
      <el-card v-for="item in todoList" v-bind:key="item" style="width:170px;margin:2px;">
          <el-row>
              <el-col :span="24">
                <p>{{item.number}} -- {{item.text}}</p>
              </el-col>
          </el-row>
          <el-row>
            <el-col :span="12">
                <el-button @click="deltodo(item.text)" size="mini">删除</el-button>
              </el-col>
              <el-col :span="12">
                <el-button @click="gotop(item.text)" size="mini">置顶</el-button>
              </el-col>
          </el-row>
        </el-card>
    </el-row>

    <el-row>
      <el-col :span="20">
            <el-input v-model="msgText"></el-input>
        </el-col>   
        <el-button :span="2" @click="send">发送消息</el-button>
    </el-row>

    <el-row>
      <el-input :span="20" v-model="receiveMsg" type="textarea" autosize></el-input>
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
      let url = "";
      let isdebug = true;
      if(isdebug)
      {
        url = "http://localhost:5189";
      }
      else
      {
        url = "http://www.qmtdlt.cn:8088";
      }
      
        
      return {
        base_url:url,
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
          axios.post(`${this.$data.base_url}/api/Todo/AddToDo?todoText=` + this.$data.todoText)
          .then(()=>{
            axios.get(`${this.$data.base_url}/api/Todo/GetList`)
            .then((res)=>{
                this.$data.todoList = res.data;
            });              
          });
      },
      deltodo(t_text)
      {
        axios.post(`${this.$data.base_url}/api/Todo/DelTodo?todoText=` + t_text)
        .then((res)=>{
          this.$data.todoList = res.data;           
        });
      },
      gotop(t_text)
      {
        axios.post(`${this.$data.base_url}/api/Todo/GoTop?todoText=` + t_text)
          .then((res)=>{
            this.$data.todoList = res.data;            
          });
      }
  },
  mounted(){
    axios.get(`${this.$data.base_url}/api/Todo/GetList`)
    .then((res)=>{
        this.$data.todoList = res.data;
    });

     this.$data.connection = new signalR.HubConnectionBuilder()
    .withUrl(`${this.$data.base_url}/ChatHub`
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
