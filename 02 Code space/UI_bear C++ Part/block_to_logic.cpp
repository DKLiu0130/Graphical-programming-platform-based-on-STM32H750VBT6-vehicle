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
extern set<string>var;
extern set<string>keywords; 
typedef pair<int,int>pr;
map<string,pr>check_c;
int sub; 
struct code
{
	int id;
	string type;
	vector<int> c_id;
	vector<string> expr;
	int nxt=0;
	bool check()
	{
		map<string,pr>::iterator it=check_c.find(type);
		return it!=check_c.end()&&(it->second==pr(expr.size(),c_id.size())||type=="if"&&expr.size()==1&&c_id.size()==2);
	}
};








map<int,code>cl;
set<int>c_tag;
int n,m;
string output;
int len;
int root; 
void pre()
{
	check_c["goline"]=pr(1,0);
	check_c["turn"]=pr(1,0);
	check_c["circle"]=pr(2,0);
	check_c["gotill"]=pr(1,0);
	
	check_c["for"]=pr(1,1);
	check_c["if"]=pr(1,1);
//	check_c["ifelse"]=pr(1,2);
	check_c["while"]=pr(1,1);
	check_c["break"]=pr(0,0);
	
	check_c["mov"]=pr(2,0); 
	
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

void init()
{
	cin>>sub;
	cin>>n;
	for(int i=1;i<=n;i++)
	{
		int tp;
		cin>>cl[i].id>>cl[i].type;
		cin>>len;
		for(int j=1;j<=len;j++)
		{
			string s;
			cin>>s;
			cl[i].expr.push_back(s);
		}
		cin>>len;
		for(int j=1;j<=len;j++)
		{
			cin>>tp;
			cl[i].c_id.push_back(tp);
		}
		cin>>cl[i].nxt;
	}
	cin>>m;
	string ipt;
	for(int i=1;i<=m;i++)
	{
		cin>>ipt;
//		if(keywords.find(ipt)!=keywords.end())throw expt_keywordsinexpr(0);
		if(var.find(ipt)==var.end())var.insert(ipt);
	}
}
void getroot()
{
	c_tag.clear();
	for(map<int,code>::iterator it=cl.begin();it!=cl.end();it++)
	{
		int nxt=it->second.nxt;
		if(nxt&&c_tag.find(nxt)!=c_tag.end())
		{
			throw expt_logic(it->first);
		}
		if(nxt)c_tag.insert(nxt);
		const vector<int>&vct=it->second.c_id;
		int len=vct.size();
		for(int i=0;i<len;i++)
		{
			if(c_tag.find(vct[i])!=c_tag.end())
			{
				throw expt_logic(it->first);
			}
			if(vct[i])c_tag.insert(vct[i]);
		}
	}
	if(c_tag.size()!=cl.size()-1)throw expt_logic(0);
	for(map<int,code>::iterator it=cl.begin();it!=cl.end();it++)
	{
		if(c_tag.find(it->first)==c_tag.end())
		{
			root=it->first;
			return;
		}
	}
	throw expt_logic(0);
} 
int cnt;
void c_code(int x)
{
	if(!x)return;
	if(c_tag.find(x)!=c_tag.end())throw expt_logic(x);
	c_tag.insert(x);
	map<int,code>::iterator it=cl.find(x);
	if(!cnt&&it->second.type=="break")throw expt_outbreak(x);
	if(!it->second.check())throw expt_invalidcode(x);
	int len=it->second.c_id.size();
	for(int i=0;i<len;i++)it->second.type!="if"?cnt++:0,c_code(it->second.c_id[i]),it->second.type!="if"?cnt--:0;
	if(it->second.nxt)c_code(it->second.nxt);
	len=it->second.expr.size();
	for(int i=0;i<len;i++)
	{
		int ans=check_expr(it->second.expr[i]);
		if(ans==0)throw expt_invalidexpr(x);
		if(ans==-1)throw expt_exconst(x);
		if(ans==-2)throw expt_udfvar(x);
		if(ans==-3)throw expt_keywordsinexpr(x);
	}
}
void check()
{
	c_tag.clear();
	c_code(root);
}
void trans(int x)
{
	code t=cl[x];
	if(t.type=="goline")
	{
//		if(get_dpos(t.expr[0],0)!=t.expr[0].length()-1)throw expt_invalidexpr(x);
		output+="goline "+t.expr[0]+'\n';
	}
	else if(t.type=="turn")
	{
//		if(get_dpos(t.expr[0],0)!=t.expr[0].length()-1)throw expt_invalidexpr(x);
		output+="turn "+t.expr[0]+'\n';
	}
	else if(t.type=="circle")
	{
//		if(get_dpos(t.expr[0],0)!=t.expr[0].length()-1)throw expt_invalidexpr(x);
//		if(get_dpos(t.expr[1],0)!=t.expr[1].length()-1)throw expt_invalidexpr(x);
		output+="circle "+t.expr[0]+' '+t.expr[1]+'\n';
	}
//	else if(t.type=="gotill")
//	{
//		output+="goline_till "+t.expr[0]+'\n';
//	}
	else if(t.type=="for")
	{
		if(get_dpos(t.expr[0],0)!=t.expr[0].length()-1)throw expt_invalidexpr(x);
		output+="for "+t.expr[0]+'\n';
		trans(t.c_id[0]);
		output+="endfor\n";
	}
	else if(t.type=="if")
	{
		if(t.c_id.size()==1)
		{
			output+="if "+t.expr[0]+'\n';
			trans(t.c_id[0]);
			output+="endif\n";
		}
		else
		{
			output+="if "+t.expr[0]+'\n';
			trans(t.c_id[0]);
			output+="else\n";
			trans(t.c_id[1]);
			output+="endif\n";
		}
	}
	else if(t.type=="while")
	{
		output+="while "+t.expr[0]+'\n';
		trans(t.c_id[0]);
		output+="endwhile\n";
	}
	else if(t.type=="break")
	{
		output+="break\n";
	}
	else if(t.type=="mov")
	{
		if(t.expr[0].length()<=4||get_vpos(t.expr[0],4)!=t.expr[0].length()-1)throw expt_invalidleftval(x);
		output+="mov "+t.expr[0]+' '+t.expr[1]+'\n';
	}
	if(t.nxt)trans(t.nxt);
}
int main()
{
	freopen("block_to_logic.in","r",stdin);
	freopen("block_to_logic.out","w",stdout);
	try
	{
		pre();
		init();
		getroot();
		check();
		trans(root);
		cout<<sub<<endl;
		cout<<m<<endl;
		for(set<string>::iterator it=var.begin();it!=var.end();it++)cout<<"var_"<<*it<<' ';cout<<endl; 
		cout<<output<<endl; 
	}
	catch(expt_input e)
	{
		cout<<" unexpected format";
		return -1;
	}
	catch(expt_logic e)
	{
		if(e.id==0)cout<<" no code";
		else cout<<" be impossible at line "<<e.id;
		return -2;
	}
	catch(expt_invalidcode e)
	{
		cout<<" invalid codeblock at line "<<e.id;
		return -3;
	}
	catch(expt_invalidexpr e)
	{
		cout<<" invalid expression at line "<<e.id;
		return -4;
	}
	catch(expt_exconst e)
	{
		cout<<" exceeded val at line "<<e.id;
		return -5;
	}
	catch(expt_udfvar e)
	{
		cout<<" undefined variable at line "<<e.id;
		return -6;
	}
	catch(expt_invalidleftval e)
	{
		cout<<" invalid left value at line "<<e.id;
		return -6;
	}
	catch(expt_keywordsinexpr e)
	{
		cout<<" find keywords at line "<<e.id;
		return -7;
	}
	catch(expt_outbreak e)
	{
		cout<<" invalid break at line "<<e.id;
		return -8;
	}
	return 0;
}
