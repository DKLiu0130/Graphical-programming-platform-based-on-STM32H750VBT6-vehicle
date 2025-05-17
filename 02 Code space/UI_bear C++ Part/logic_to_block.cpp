#include<cstdio>
#include<vector>
#include<map>
#include<set>
#include<cstring>
#include<string>
#include<iostream>
#include"expt.h"
#include"stral.h" 
using namespace std; 
string output;
int forcnt,whilecnt;
struct code
{
	int id;
	string type;
	vector<int> c_id;
	vector<string> expr;
	int nxt;
	code()
	{
		id=nxt=0;
	}
};
vector<code>vct;
extern set<string>var;
extern set<string>keywords;
extern bool need_getvar;
void pre()
{
	need_getvar=1;
	keywords.insert("if");
	keywords.insert("else");
	keywords.insert("while");
	keywords.insert("signed");
	keywords.insert("throw");
	keywords.insert("union");
	keywords.insert("this");
	keywords.insert("int");
	keywords.insert("char");
	keywords.insert("double");
	keywords.insert("unsigned");
	keywords.insert("const");
	keywords.insert("goto");
	keywords.insert("virtual");
	keywords.insert("for");
	keywords.insert("float");
	keywords.insert("break");
	keywords.insert("auto");
	keywords.insert("class");
	keywords.insert("operator");
	keywords.insert("case");
	keywords.insert("do");
	keywords.insert("long");
	keywords.insert("typedef");
	keywords.insert("static");
	keywords.insert("firend");
	keywords.insert("template");
	keywords.insert("default");
	keywords.insert("new");
	keywords.insert("void");
	keywords.insert("register");
	keywords.insert("extern");
	keywords.insert("return");
	keywords.insert("enun");
	keywords.insert("inline");
	keywords.insert("try");
	keywords.insert("short");
	keywords.insert("continue");
	keywords.insert("sizeof");
	keywords.insert("switch");
	keywords.insert("private");
	keywords.insert("protected");
	keywords.insert("asm");
	keywords.insert("while");
	keywords.insert("catch");
	keywords.insert("delete");
	keywords.insert("public");
	keywords.insert("volatile");
	keywords.insert("struct");
}
//if 1	while 2	for 3	else 4
bool tran(int f,bool b,int ctr)
{ 
	string ipt;
	string e1,e2;
	if(!(cin>>ipt))
	{
		if(ctr)throw expt_logic(0);
		return 0;
	}
	code t;
	if(ipt=="goline")
	{
		cin>>e1;
		if(check_expr(e1)!=1)throw expt_invalidexpr(0);
		t.id=vct.size()+1;
		if(f&&b)vct[f-1].c_id.push_back(t.id);
		if(f&&!b)vct[f-1].nxt=t.id;
		t.type="goline";
		t.expr.push_back(e1);
		vct.push_back(t);
		return tran(t.id,0,ctr);
	}
	else if(ipt=="turn")
	{
		cin>>e1;
		if(check_expr(e1)!=1)throw expt_invalidexpr(0);
		t.id=vct.size()+1;
		if(f&&b)vct[f-1].c_id.push_back(t.id);
		if(f&&!b)vct[f-1].nxt=t.id;
		t.type="turn";
		t.expr.push_back(e1);
		vct.push_back(t);
		return tran(t.id,0,ctr);
	}
	else if(ipt=="circle")
	{
		cin>>e1>>e2;
		if(check_expr(e1)!=1)throw expt_invalidexpr(0);
		if(check_expr(e2)!=1)throw expt_invalidexpr(0);
		t.id=vct.size()+1;
		if(f&&b)vct[f-1].c_id.push_back(t.id);
		if(f&&!b)vct[f-1].nxt=t.id;
		t.type="circle";
		t.expr.push_back(e1);
		t.expr.push_back(e2);
		vct.push_back(t);
		return tran(t.id,0,ctr);
	}
	else if(ipt=="goline_till")
	{
		cin>>e1;
		if(check_expr(e1)!=1)throw expt_invalidexpr(0);
//		t.id=vct.size()+1;
//		if(f&&b)vct[f-1].c_id.push_back(t.id);
//		if(f&&!b)vct[f-1].nxt=t.id;
//		t.type="gotill";
//		t.expr.push_back(e1);
//		vct.push_back(t);
		
		
		t.id=vct.size()+1;
		if(f&&b)vct[f-1].c_id.push_back(t.id);
		if(f&&!b)vct[f-1].nxt=t.id;
		t.type="undefcode";
		vct.push_back(t);
		return tran(t.id,0,ctr);
	}
	else if(ipt=="for")
	{
		cin>>e1;
		if(check_expr(e1)!=1)throw expt_invalidexpr(0);
		t.id=vct.size()+1;
		if(f&&b)vct[f-1].c_id.push_back(t.id);
		if(f&&!b)vct[f-1].nxt=t.id;
		t.type="for";
		t.expr.push_back(e1);
		vct.push_back(t);
		forcnt++;
		tran(t.id,1,3);
		return tran(t.id,0,ctr);
	}
	else if(ipt=="while")
	{
		cin>>e1;
		if(check_expr(e1)!=1)throw expt_invalidexpr(0);
		t.id=vct.size()+1;
		if(f&&b)vct[f-1].c_id.push_back(t.id);
		if(f&&!b)vct[f-1].nxt=t.id;
		t.type="while";
		t.expr.push_back(e1);
		vct.push_back(t);
		whilecnt++;
		tran(t.id,1,2);
		return tran(t.id,0,ctr);
	}
	else if(ipt=="if")
	{
		cin>>e1;
		if(check_expr(e1)!=1)throw expt_invalidexpr(check_expr(e1));
		t.id=vct.size()+1;
		if(f&&b)vct[f-1].c_id.push_back(t.id);
		if(f&&!b)vct[f-1].nxt=t.id;
		t.type="if";
		t.expr.push_back(e1);
		vct.push_back(t);
		if(tran(t.id,1,1))
		{
			tran(t.id,1,4);
		}
		return tran(t.id,0,ctr);
	}
	else if(ipt=="break")
	{
		t.id=vct.size()+1;
		if(f&&b)vct[f-1].c_id.push_back(t.id);
		if(f&&!b)vct[f-1].nxt=t.id;
		t.type="break";
		vct.push_back(t);
		if(whilecnt+forcnt==0)throw expt_logic(0);
		return tran(t.id,0,ctr);
	}
	else if(ipt=="undefcode")
	{
		t.id=vct.size()+1;
		if(f&&b)vct[f-1].c_id.push_back(t.id);
		if(f&&!b)vct[f-1].nxt=t.id;
		t.type="undefcode";
		vct.push_back(t);
		return tran(t.id,0,ctr);
	}
	else if(ipt=="mov")
	{
		cin>>e1>>e2;
		if(check_expr(e1)!=1)throw expt_invalidexpr(0);
		if(check_expr(e2)!=1)throw expt_invalidexpr(0);
		t.id=vct.size()+1;
		if(f&&b)vct[f-1].c_id.push_back(t.id);
		if(f&&!b)vct[f-1].nxt=t.id;
		t.type="mov";
		t.expr.push_back(e1);
		t.expr.push_back(e2);
		vct.push_back(t);
		return tran(t.id,0,ctr);
	}
	else if(ipt=="endif")
	{
		if(ctr!=1&&ctr!=4)throw expt_logic(0);
		return 0;
	}
	else if(ipt=="endfor")
	{
		if(ctr!=3)throw expt_logic(0);
		forcnt--;
		return 0;
	}
	else if(ipt=="endwhile")
	{
		if(ctr!=2)throw expt_logic(0);
		whilecnt--;
		return 0;
	}
	else if(ipt=="else")
	{
		if(ctr!=1)throw expt_logic(0);
		return 1;
	}
	else
	{
		throw expt_invalidcode(0); 
	}
}
void opt()
{
	int len=vct.size();
	cout<<len<<endl;
	for(int i=0;i<len;i++)
	{
		code t=vct[i];
		cout<<t.id<<'\n'<<t.type<<'\n';
		int len=t.expr.size();
		cout<<len<<'\n';
		for(int j=0;j<len;j++)cout<<t.expr[j]<<'\n';
		len=t.c_id.size();
		cout<<len<<'\n';
		for(int j=0;j<len;j++)cout<<t.c_id[j]<<'\n';
		cout<<t.nxt<<endl;
	}
	cout<<var.size()<<endl;
	for(set<string>::iterator it=var.begin();it!=var.end();it++)cout<<*it<<'\n';cout<<endl;
}
int main()
{
	freopen("in1.txt","r",stdin);
	freopen("logic_to_block.out","w",stdout);
	try
	{
		pre();
		tran(0,0,0);
		opt();
	}
	catch(expt_input e)
	{
		cout<<"!unexpected format"<<endl;
		return -1;
	}
	catch(expt_invalidcode e)
	{
		cout<<"!invalid code"<<endl;
		opt();
		return -2;
	}
	catch(expt_logic e)
	{
		cout<<"!be impossible"<<endl;
	}
	catch(expt_invalidexpr e)
	{
		cout<<"!invalid expression with code "<<e.id<<endl;
	}
	return 0;
}
