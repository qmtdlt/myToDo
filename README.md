# myToDo

```
搭建网站步骤
0. 发开一个网站
1. 租服务器
2. 发布网站
3. 服务器控制台开端口，通过ip+端口可访问网站
4. 购买域名
5. 备案域名，关联服务器地址
6. 备案网站，提交备案材料，等待云服务商、工信部审核通过
7. 通过域名访问网站成功
8. 网站填写备案号
```

### 一个todo应用
### 支持signalr的消息发送

### nginx config
```nginx
server { 
		listen          80;                   
		server_name     qmtdlt.cn;    

		location / { 
			root   html/dist;                
			index  index.html index.htm; 
			proxy_http_version 1.1; 
			proxy_set_header Upgrade $http_upgrade;                
			proxy_set_header Connection "upgrade";
		}
		
		location /api { 
			proxy_pass  http://www.qmtdlt.cn:8088/api/;  
		} 
	}
```