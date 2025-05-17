#include<cstdio>
#include<vector>
#include<map>
#include<set>
#include<cstring>
#include<string>
#include<iostream>
#include"expt.h"
using namespace std;
string ipt;
map<string,int>qt;
int sub;
inline isvar(char c)
{
	return isdigit(c)||c>='a'&&c<='z'||c>='A'&&c<='Z'||c=='_';
}
bool f=0;
bool read(string& s)
{
	s="";
	bool f=0;
	char cs[6]={0};
	char c=0;
	f=0;
	while(c=getchar(),c!=EOF&&c!=' '&&c!='\n'&&c!='\r')
	{
		cs[0]=cs[1];cs[1]=cs[2];cs[2]=cs[3];
		cs[3]=cs[4];cs[4]=cs[5];cs[5]=c;
		if(!(isvar(cs[0])||isvar(cs[5]))&&cs[1]=='f'&&cs[2]=='u'&&cs[3]=='n'&&cs[4]=='c')
		{
			s=s.substr(0,s.length()-4);
			if(sub==1)s+="detected_number";
			else if(sub==2)s+="traffic_light";
			f=1;
		}
		s+=c;
	}
	cs[0]=cs[1];cs[1]=cs[2];cs[2]=cs[3];
	cs[3]=cs[4];cs[4]=cs[5];cs[5]=c;
	if(!(isvar(cs[0])||isvar(cs[5]))&&cs[1]=='f'&&cs[2]=='u'&&cs[3]=='n'&&cs[4]=='c')
	{
		s=s.substr(0,s.length()-4);
		if(sub==1)s+="detected_number";
		else if(sub==2)s+="traffic_light";
		f=1;
	}
	return c!=EOF;
}
int cnt; 
const string GDThead="\
void Get_Data_in_Task(uint8_t* received_datap,uint8_t* detected_numberp,uint8_t* traffic_lightp)\n\
{\n\
    Submission=";
const string GDTtail="; \n\
    if(uart_received_frame_length > 0) {\n\
        *received_datap = 0;\n\
        for(int i = 0; i < 8; i++) {\n\
            *received_datap |= (uart_received_frame[i+8] & 0x01) << i;\n\
        }\n\
\n\
        if(Submission == MODE_NUMBER) {\n\
            *detected_numberp = (*received_datap & 0x07) + 1;\n\
            LCD_DisplayNumber(150,50,*detected_numberp,2);\n\
        }\n\
        else if(Submission == MODE_TRAFFIC) {\n\
            *traffic_lightp = (*received_datap >> 3) & 0x03;\n\
            LCD_DisplayNumber(150,70,*traffic_lightp,2);\n\
        }\n\
    }\n\
}\n\
";
const string data_upd="Get_Data_in_Task(&received_data,&detected_number,&traffic_light);\n";
const string delay="while(!car_control.oprate_done)osDelay(10);\n";
const string head_code="\
void Task_APP()\n\
{\n\
	/* USER CODE BEGIN Task_Submission */\n\
	uint8_t received_data = 0;    \n\
	uint8_t detected_number = 0;  \n\
	uint8_t traffic_light = 0;    \n\
";

const string tail_code="\
	}\n\
	/* USER CODE END Task_Submission */\n\
	go_line(0);\n\
//	HAL_GPIO_WritePin(GPIOJ, GPIO_PIN_3, GPIO_PIN_RESET);\n\
//	LCD_DisplayNumber(150, 150, 9999, 4);\n\
	vTaskDelete(NULL);\n\
}\n\
";
string htab(int x)
{
	string ans="";
	for(int i=1;i<=x;i++)ans+="    ";
	return ans;
}
bool tran(int x)
{
	if(!(read(ipt)))return 0;
	if(qt[ipt])return ipt=="else";
	if(ipt=="goline")
	{
//		cout<<htab(x)<<data_upd;
		cout<<htab(x)<<"go_line(20*(";
		read(ipt);cout<<ipt<<"));\n";
		cout<<htab(x)<<delay;
		cout<<endl;
	}
	else if(ipt=="turn")
	{
		read(ipt);
//		cout<<htab(x)<<data_upd;
		cout<<htab(x)<<"turn";
		cout<<"(("<<ipt<<")>0?1:0,("<<ipt<<")>0?(("<<ipt<<")*2-10):-2*("<<ipt<<")+10";
		cout<<");\n";
		cout<<htab(x)<<delay;
		cout<<endl;
	}
	else if(ipt=="circle")
	{
//		cout<<htab(x)<<data_upd;
		cout<<htab(x)<<"circle";
		read(ipt);cout<<"(("<<ipt<<")>0?1:0,("<<ipt<<")>0?("<<ipt<<"):-("<<ipt<<")";cout<<',';
		read(ipt);cout<<ipt<<");\n";
		cout<<htab(x)<<delay;
		cout<<endl;
	}
	else if(ipt=="for")
	{
//		cout<<htab(x)<<data_upd;
		cout<<htab(x)<<"for(int counter=1;counter<=";
		read(ipt);cout<<'('<<ipt<<')'<<";counter++)\n";
		cout<<htab(x)<<"{\n";
		tran(x+1);
		cout<<htab(x)<<"}\n";
		cout<<endl;
	}
	else if(ipt=="while")
	{
		cout<<htab(x)<<data_upd;
		cout<<htab(x)<<"while(";
		read(ipt);cout<<ipt<<")\n";
		cout<<htab(x)<<"{\n";
		tran(x+1);
		cout<<htab(x)<<data_upd;
		cout<<htab(x)<<"}\n";
		cout<<endl;
	}
	else if(ipt=="if")
	{
		cout<<htab(x)<<data_upd;
		cout<<htab(x)<<"if(";
		read(ipt);cout<<ipt<<")\n";
		cout<<htab(x)<<"{\n";
		if(tran(x+1))
		{
			cout<<htab(x)<<"}\n";
			cout<<htab(x)<<"else\n";
			cout<<htab(x)<<"{\n";
			tran(x+1);
			cout<<htab(x)<<"}\n";
		}
		else cout<<htab(x)<<"}\n";
		cout<<endl;
	}
//	else if(ipt=="goline_till")
//	{
//		cout<<"while(!(";
//		read(ipt);
//		cout<<ipt<<")){goline(1);}\n";
//	}
	else if(ipt=="mov")
	{
		cout<<htab(x)<<data_upd;
		read(ipt);cout<<htab(x)<<ipt<<"=(";
		read(ipt);
		cout<<ipt<<");\n";
		cout<<endl;
	}
	else if(ipt=="break")
	{
		cout<<htab(x)<<"break;\n";
		cout<<endl;
	}
	return tran(x);
}
void pre()
{
	qt["endif"]=1;
	qt["else"]=1;
	qt["endwhile"]=1;
	qt["endfor"]=1;
}
void opthead()
{
	freopen("freertoshead.c","r",stdin);
	char ipt;
	while((ipt=getchar())!=EOF)putchar(ipt);
}
int varcount;
string varopt;
int main()
{
//	freopen("D:\\c++working\\car_iptsolved\\Tutorial_C#\\Tutorial_STM32H750XBH6_Startup\\Core\\Src\\freertos.c","w",stdout);
	freopen("freertos.c","w",stdout);
	opthead();
	freopen("block_to_logic.out","r",stdin);
	cin>>sub;
	cin>>varcount;
	string vaript;
	for(int i=1;i<=varcount;i++)
	{
		cin>>vaript;
		if(i==1)varopt="    int ",varopt+=vaript+"=0";
		else varopt+',',varopt+=vaript;
	}
	varopt+=';';
	pre();
	cout<<GDThead<<sub<<GDTtail;
	cout<<head_code<<varopt;
	cout<<
	"\n    osDelay(1000);\n\
	{\n";
	tran(2);
	cout<<tail_code;
	return 0;
}
