#ifndef _DELAY_H
#define _DELAY_H
#include "main.h"
//////////////////////////////////////////////////////////////////////////////////  
//������ֻ��ѧϰʹ�ã�δ��������ɣ��������������κ���;
//ALIENTEK STM32F407������
//ʹ��SysTick����ͨ����ģʽ���ӳٽ��й���(֧��ucosii)
//����delay_us,delay_ms
//����ԭ��@ALIENTEK
//������̳:www.openedv.com
//��������:2017/4/6
//�汾��V1.0
//��Ȩ���У�����ؾ���
//Copyright(C) ������������ӿƼ����޹�˾ 2014-2024
//All rights reserved
//********************************************************************************
//�޸�˵��
////////////////////////////////////////////////////////////////////////////////// 
#define u16 unsigned short
#define u32 unsigned int
void delay_init(u16 SYSCLK);
void delay_ms(u16 nms);
void delay_us(u32 nus);
#endif

