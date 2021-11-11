<template>
  <div class="hello">
    <el-row>
        <el-col :span="10">
            <el-input v-model="todoText"></el-input>
        </el-col>        
        <el-button @click="do_post">post</el-button>
    </el-row>
    
    <div class="container">
        <div v-for="item in todoList" v-bind:key="item" class="itemContainer">
            <p class="todoli">
                {{item.number}} -- {{item.text}}
            </p>
            <el-button class="todoDel" @click="deltodo(item.text)">删除</el-button>
        </div>
    </div>
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
.container{
    /* width: 500px; */
}
.todoDel
{
    margin: 0px 0px 0px 400px;
}
.itemContainer{
    float: left;
    margin: 10px;
    display: block;
}
.todoli{
    display: inline;
}
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
