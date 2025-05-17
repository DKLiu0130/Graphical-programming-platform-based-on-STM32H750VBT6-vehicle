#ifndef EXPT
#define EXPT
#include<cstdio>
#include<vector>
#include<map>
#include<set>
#include<cstring>
#include<string>
#include<iostream>
using namespace std; 
struct expt_input
{
};
struct expt_logic
{
	int id;
	expt_logic(int x)
	{
		id=x;
	}
};
struct expt_outbreak
{
	int id;
	expt_outbreak(int x)
	{
		id=x;
	}
};
struct expt_invalidcode
{
	int id;
	expt_invalidcode(int x)
	{
		id=x;
	}
};
struct expt_invalidexpr 
{
	int id;
	expt_invalidexpr(int x)
	{
		id=x;
	}
};
struct expt_exconst
{
	int id;
	expt_exconst(int x)
	{
		id=x;
	}
};
struct expt_udfvar
{
	int id;
	expt_udfvar(int x)
	{
		id=x;
	}
};
struct expt_invalidleftval
{
	int id;
	expt_invalidleftval(int x)
	{
		id=x;
	}
};
struct expt_keywordsinexpr
{
	int id;
	expt_keywordsinexpr(int x)
	{
		id=x;
	}
};
#endif
