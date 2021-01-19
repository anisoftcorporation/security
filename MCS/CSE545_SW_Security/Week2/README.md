# HTTP Webserver Backdoor

The purpose of this project is to create a HTTP Webserver Backdoor using bare minimal C libraries.
The main file is WebServer.c
### To make, simply execute the below command
```
make
```
### To Execute

```
./WebServer <port number>
```
After it starts executing, a HTTP Server will be running on the host and will be listening on the PORT passed during execution as argument.

To invoke the backdoor, the syntax is

```
http://hostname:<PORT>/exec/<command to execute on target>
```
If the /exec/ is missing , the response will be a valid HTTP 404 response
Other wise , it will execute the command being passed and will return HTTP 200 OK aloong with the output of the command.
