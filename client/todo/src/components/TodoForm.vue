<template>
  <div>
    <el-row>
        <el-col :span="10">
            <el-input v-model="todoText"></el-input>
        </el-col>        
        <el-button :span="10" @click="do_post">post</el-button>
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
        todoList:[]
      }
  },
  methods:{
      do_post(){ 
          axios.post(`http://1.15.248.70:8089/api/Todo/AddToDo?todoText=` + this.$data.todoText)
          .then(()=>{
            axios.get(`http://1.15.248.70:8089/api/Todo/GetList`)
            .then((res)=>{
                console.log("刷新todo");
                this.$data.todoList = res.data;
                console.log(this.$data.todoList);
            });              
          });
      },
      deltodo(t_text)
      {
        axios.post(`http://1.15.248.70:8089/api/Todo/DelTodo?todoText=` + t_text)
        .then((res)=>{
          console.log("刷新todo");
          this.$data.todoList = res.data;
          console.log(this.$data.todoList);              
        });
      },
      gotop(t_text)
      {
        axios.post(`http://1.15.248.70:8089/api/Todo/GoTop?todoText=` + t_text)
          .then((res)=>{
            console.log("刷新todo");
            this.$data.todoList = res.data;
            console.log(this.$data.todoList);              
          });
      }
  },
  mounted(){
    axios.get(`http://1.15.248.70:8089/api/Todo/GetList`)
    .then((res)=>{
        this.$data.todoList = res.data;
        console.log('初始化完成');
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
