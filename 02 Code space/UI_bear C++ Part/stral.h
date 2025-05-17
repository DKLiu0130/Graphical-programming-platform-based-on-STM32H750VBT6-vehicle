#ifndef STRAL
#define STRAL
#include<cstdio>
#include<vector>
#include<map>
#include<set>
#include<cstring>
#include<string>
#include<iostream>
using namespace std; 

set<string>var;
set<string>keywords; 
bool need_getvar=0; 
string cutstring(const string&s,int l,int r)
{
	string ans;
	for(int i=l;i<=r;i++)ans+=s[i];
	return ans;
}
int get_opos(const string&s,int l)
{
	int len=s.length();
	if(l==len-1)return -1;
	if(s[l]=='+')
	{
		if(s[l+1]=='+')return -1;
		return l+1;
	}
	if(s[l]=='!')
	{
		if(s[l+1]=='=')return (l+2<len)?l+2:-1;
		return -1;
	}
	if(s[l]=='-')
	{
		if(s[l+1]=='-')return -1;
		return l+1;
	}
	if(s[l]=='*')return l+1;
	if(s[l]=='/')return l+1;
	if(s[l]=='%')return l+1;
	if(s[l]=='<')
	{
		if(s[l+1]=='<'||s[l+1]=='=')return (l+2<len)?l+2:-1;
		return l+1;
	}
	if(s[l]=='>')
	{
		if(s[l+1]=='>'||s[l+1]=='=')return (l+2<len)?l+2:-1;
		return l+1;
	}
	if(s[l]=='=')
	{
		if(s[l+1]=='=')return (l+2<len)?l+2:-1;
		return 0;
	}
	if(s[l]=='|')
	{
		if(s[l+1]=='|')return (l+2<len)?l+2:-1;
		return l+1;
	}
	if(s[l]=='&')
	{
		if(s[l+1]=='&')return (l+2<len)?l+2:-1;
		return l+1;
	}
	if(s[l]=='^')return l+1;
	return -1;
}
inline bool isvarc(char c)
{
	return c>='a'&&c<='z'||c>='A'&&c<='Z'||c=='_';
}
int get_dpos(const string&s,int l)
{
	long long num=0;
	int len=s.length();
	while(l<len&&isdigit(s[l]))
	{
		num*=10,num+=s[l]&15,l++;
		if(num>2147482647ll||num<-2147483648ll)return -2;
	}
	return l-1;
}
int get_vpos(const string&s,int l)
{
	string text="";
	if(isvarc(s[l]))text+=s[l],l++;
	int len=s.length();
	while(l<len&&(isvarc(s[l])||isdigit(s[l])))text+=s[l],l++;
	return (!need_getvar&&var.find(text)==var.end()&&text!="func")?-2:l-1;
}
void strins(string& s,int l)
{
	string ans="";
	int len=s.length();
	for(int i=0;i<l;i++)ans+=s[i];
	ans+="var_";
	for(int i=l;i<len;i++)ans+=s[i];
	s=ans;
}
int check_expr(string& s)//1:normal	0:invalid	-1:too large	-2:unknown var
{
	set<int>st;
	if(s=="")return 0;
	int len=s.length();
	int l=0;
	int cnt=0;
	bool needvar=0;
	while(l<len)
	{
		if(s[l]=='+')
		{
			if(l==len-1)return 0;
			if(s[l+1]=='+')return 0;
			++l;
		}
		else if(s[l]=='-')
		{
			if(l==len-1)return 0;
			if(s[l+1]=='-')return 0;
			++l;
		}
		else if(s[l]=='(')
		{
			++cnt;
			++l;
			needvar=1;
		}
		else if(s[l]==')')
		{
			if(needvar)return 0;
			--cnt;
			if(cnt<0)return 0;
			++l;
			if(l>=len)break;
			if(s[l]==')')continue;
			l=get_opos(s,l);
			if(l<=0)return 0;
			if(l>=len)return 0;
		}
		else if(s[l]=='!')
		{
			++l;
		}
		else if(s[l]=='~')
		{
			++l;
		}
		else if(s[l]==' ')
		{
			++l;
		}
		else if(isdigit(s[l]))
		{
			needvar=0;
			l=get_dpos(s,l)+1;
			if(l==-1)return -1;
			if(l>=len)break;
			if(s[l]==')')continue;
			l=get_opos(s,l);
			if(l<=0)return 0;
			if(l>=len)return 0;
		}
		else if(isvarc(s[l]))
		{
			needvar=0;
			int t=get_vpos(s,l);
			if(t==-2)return -2;
			string evar=cutstring(s,l,t);
			if(keywords.count(evar))return -3;
			if(need_getvar&&evar!="func")
			{
				var.insert(evar);
			}
			if(evar!="func")st.insert(l);
			l=t+1;
			if(l>=len)break;
			if(s[l]==')')continue;
			l=get_opos(s,l);
			if(l<=0)
			{
				return 0;
			}
			if(l>=len)
			{
				return 0;
			}
		}
		else return 0;
	}
	for(set<int>::iterator it=st.begin();it!=st.end();it++)
	{
		strins(s,*it);
	}
	len=s.length();
	return s[len-1]!='+'&&s[len-1]!='-'&&s[len-1]!='!'&&s[len-1]!='~'&&cnt==0;
}
#endif
