/* USER CODE BEGIN Header */
/**
  ******************************************************************************
  * @file    spi.h
  * @brief   This file contains all the function prototypes for
  *          the spi.c file
  ******************************************************************************
  * @attention
  *
  * Copyright (c) 2025 STMicroelectronics.
  * All rights reserved.
  *
  * This software is licensed under terms that can be found in the LICENSE file
  * in the root directory of this software component.
  * If no LICENSE file comes with this software, it is provided AS-IS.
  *
  ******************************************************************************
  */
/* USER CODE END Header */
/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __SPI_H__
#define __SPI_H__

#ifdef __cplusplus
extern "C" {
#endif

/* Includes ------------------------------------------------------------------*/
#include "main.h"

/* USER CODE BEGIN Includes */
#include "lcd_fonts.h"	
#include	"lcd_image.h"
#include "stm32h7xx_hal.h"
/* USER CODE END Includes */

extern SPI_HandleTypeDef hspi2;

extern SPI_HandleTypeDef hspi4;

/* USER CODE BEGIN Private defines */
/*----------------------------------------------- LCD ���� -------------------------------------------*/
#define LCD_Width     240		// LCD ���
#define LCD_Height    280		// LCD �߶�

#define RADIAN(angle)  ((angle==0)?0:(3.14159*angle/180))   // �Ƕ�ת����
#define MAX(x,y)  		((x)>(y)? (x):(y))
#define MIN(x,y)  		((x)<(y)? (x):(y))
#define SWAP(x, y) \
  (y) = (x) + (y); \
  (x) = (y) - (x); \
  (y) = (y) - (x);
#define ABS(X)  ((X) > 0 ? (X) : -(X))   // ����ֵ

// 2D ����ṹ�壬���� x �� y ����
typedef struct COORDINATE 
{
  int x;  // x ����
  int y;  // y ����
} TypeXY;

#define point TypeXY 

// ��ת�ṹ�壬������ת���ġ��ǶȺͷ���
typedef struct ROATE
{
  TypeXY center;   // ��ת����
  float angle;     // ��ת�Ƕ�
  int direct;      // ��ת����
} TypeRoate;

// ��ʾ������
// ʹ�� LCD_DisplayDirection(Direction_H) ������ʾ����
#define	Direction_H				0					// ˮƽ��ʾ
#define	Direction_H_Flip	   1					// ˮƽ��ʾ����ת
#define	Direction_V				2					// ��ֱ��ʾ 
#define	Direction_V_Flip	   3					// ��ֱ��ʾ����ת 

// ������ʾ���ֵ����ģʽ
// ������ LCD_DisplayNumber() �� LCD_DisplayDecimals()
// ʹ�� LCD_ShowNumMode(Fill_Zero) ������䣬���� 123 ����ʾΪ 000123
#define  Fill_Zero  0		// �����
#define  Fill_Space 1		// ���ո�


/*---------------------------------------- ��ɫ���� ------------------------------------------------------

 1. ��Щ��ɫ��Ϊ�˷����û�ʹ�ã�����Ϊ 24 λ RGB888 ��ɫ��Ȼ��ͨ�������Զ�ת��Ϊ 16 λ RGB565 ��ɫ
 2. �� 24 λ��ɫ�У��Ӹ�λ����λ�ֱ��Ӧ R��G��B ������ɫͨ��
 3. �û��ڴ�����ʹ����ɫʱ���Ȼ�ȡ 24 λ RGB ��ɫ��Ȼ��ͨ�� LCD_SetColor() �� LCD_SetBackColor() ������ʾ��Ӧ��ɫ 
 */                                                  						
#define 	LCD_WHITE       0xFFFFFF	 // ��ɫ
#define 	LCD_BLACK       0x000000    // ��ɫ
                        
#define 	LCD_BLUE        0x0000FF	 //	��ɫ
#define 	LCD_GREEN       0x00FF00    //	��ɫ
#define 	LCD_RED         0xFF0000    //	��ɫ
#define 	LCD_CYAN        0x00FFFF    //	��ɫ
#define 	LCD_MAGENTA     0xFF00FF    //	���ɫ
#define 	LCD_YELLOW      0xFFFF00    //	��ɫ
#define 	LCD_GREY        0x2C2C2C    //	��ɫ
                        
#define 	LIGHT_BLUE      0x8080FF    //	ǳ��ɫ
#define 	LIGHT_GREEN     0x80FF80    //	ǳ��ɫ
#define 	LIGHT_RED       0xFF8080    //	ǳ��ɫ
#define 	LIGHT_CYAN      0x80FFFF    //	ǳ��ɫ
#define 	LIGHT_MAGENTA   0xFF80FF    //	ǳ���ɫ
#define 	LIGHT_YELLOW    0xFFFF80    //	ǳ��ɫ
#define 	LIGHT_GREY      0xA3A3A3    //	ǳ��ɫ
                        
#define 	DARK_BLUE       0x000080    //	����ɫ
#define 	DARK_GREEN      0x008000    //	����ɫ
#define 	DARK_RED        0x800000    //	���ɫ
#define 	DARK_CYAN       0x008080    //	����ɫ
#define 	DARK_MAGENTA    0x800080    //	�����ɫ
#define 	DARK_YELLOW     0x808000    //	���ɫ
#define 	DARK_GREY       0x404040    //	���ɫ
/* USER CODE END Private defines */

void MX_SPI2_Init(void);
void MX_SPI4_Init(void);

/* USER CODE BEGIN Prototypes */
void SPI_LCD_Init(void);      // ��ʼ��LCD��SPI
void LCD_Clear(void);         // ����
void LCD_ClearRect(uint16_t x, uint16_t y, uint16_t width, uint16_t height); // �ֲ�����

void LCD_SetAddress(uint16_t x1, uint16_t y1, uint16_t x2, uint16_t y2); // ���õ�ַ
void LCD_SetColor(uint32_t Color); // ����ǰ��ɫ
void LCD_SetBackColor(uint32_t Color); // ���ñ���ɫ
void LCD_SetDirection(uint8_t direction); // ������ʾ����

// ��ʾASCII�ַ�
void LCD_SetAsciiFont(pFONT *fonts); // ����ASCII����
void LCD_DisplayChar(uint16_t x, uint16_t y, uint8_t c); // ��ʾ����ASCII�ַ�
void LCD_DisplayString(uint16_t x, uint16_t y, char *p); // ��ʾASCII�ַ���

// ��ʾ�����ַ��ͻ��ASCII�ַ�
void LCD_SetTextFont(pFONT *fonts); // �����ı����壬�������ĺ�ASCII
void LCD_DisplayChinese(uint16_t x, uint16_t y, char *pText); // ��ʾ�����ַ�
void LCD_DisplayText(uint16_t x, uint16_t y, char *pText); // ��ʾ�ı����������ĺ�ASCII�ַ�

// ��ʾ���ֺ�С��
void LCD_ShowNumMode(uint8_t mode); // ����������ʾģʽ�����ո�����0
void LCD_DisplayNumber(uint16_t x, uint16_t y, int32_t number, uint8_t len); // ��ʾ����
void LCD_DisplayDecimals(uint16_t x, uint16_t y, double number, uint8_t len, uint8_t decs); // ��ʾС��

// 2Dͼ�λ���
void LCD_DrawPoint(uint16_t x, uint16_t y, uint32_t color); // ����

void LCD_DrawLine_V(uint16_t x, uint16_t y, uint16_t height); // ����ֱ��
void LCD_DrawLine_H(uint16_t x, uint16_t y, uint16_t width); // ��ˮƽ��
void LCD_DrawLine(uint16_t x1, uint16_t y1, uint16_t x2, uint16_t y2); // ����������֮�����

void LCD_DrawRect(uint16_t x, uint16_t y, uint16_t width, uint16_t height); // ������
void LCD_DrawCircle(uint16_t x, uint16_t y, uint16_t r); // ��Բ
void LCD_DrawEllipse(int x, int y, int r1, int r2); // ����Բ

// ���ͼ��
void LCD_FillRect(uint16_t x, uint16_t y, uint16_t width, uint16_t height); // ������
void LCD_FillCircle(uint16_t x, uint16_t y, uint16_t r); // ���Բ
void LCD_DrawImage(uint16_t x, uint16_t y, uint16_t width, uint16_t height, const uint8_t *pImage); // ��ͼ��
void DrawRoundRect(int x, int y, unsigned char w, unsigned char h, unsigned char r); // ��Բ�Ǿ���
void DrawfillRoundRect(int x, int y, unsigned char w, unsigned char h, unsigned char r); // ���Բ�Ǿ���
void DrawCircleHelper(int x0, int y0, unsigned char r, unsigned char cornername); // ��Բ�Ǹ�������
void DrawFillCircleHelper(int x0, int y0, unsigned char r, unsigned char cornername, int delta); // ���Բ�Ǹ�������
void DrawFillEllipse(int x0, int y0, int rx, int ry); // �����Բ

void DrawTriangle(unsigned char x0, unsigned char y0, unsigned char x1, unsigned char y1, unsigned char x2, unsigned char y2); // ��������
void DrawFillTriangle(int x0, int y0, int x1, int y1, int x2, int y2); // ���������
void DrawArc(int x, int y, unsigned char r, int angle_start, int angle_end); // ������
TypeXY GetXY(void); // ��ȡ��ǰ����
void SetRotateCenter(int x0, int y0); // ������ת����
void SetAngleDir(int direction); // ������ת����
void SetAngle(float angle); // ������ת�Ƕ�
static void Rotate(int x0, int y0, int *x, int *y, double angle, int direction); // ��ת����
float mySqrt(float x); // ����ƽ����
TypeXY GetRotateXY(int x, int y); // ��ȡ��ת�������
void MoveTo(int x, int y); // �ƶ���ָ������
void LineTo(int x, int y); // ���ߵ�ָ������
void SetRotateValue(int x, int y, float angle, int direct); // ������תֵ

// ����ɫͼ��
void LCD_DrawImage(uint16_t x, uint16_t y, uint16_t width, uint16_t height, const uint8_t *pImage); // ����ɫͼ��

// ���ƻ��������ݵ���Ļ
void LCD_CopyBuffer(uint16_t x, uint16_t y, uint16_t width, uint16_t height, uint16_t *DataBuff); // ���ƻ��������ݵ���Ļ
/*--------------------------------------------- LCD???????? -----------------------------------------------*/

#define  LCD_Backlight_PIN								GPIO_PIN_1			         // ????  ????				
#define	LCD_Backlight_PORT							GPIOJ									// ???? GPIO???
#define 	GPIO_LDC_Backlight_CLK_ENABLE        	__HAL_RCC_GPIOJ_CLK_ENABLE()	// ???? GPIO??? 	

#define	LCD_Backlight_OFF		HAL_GPIO_WritePin(LCD_Backlight_PORT, LCD_Backlight_PIN, GPIO_PIN_RESET);	// ????????????
#define 	LCD_Backlight_ON		HAL_GPIO_WritePin(LCD_Backlight_PORT, LCD_Backlight_PIN, GPIO_PIN_SET);		// ??????????????
 
#define  LCD_DC_PIN						GPIO_PIN_0				         // ??????????  ????				
#define	LCD_DC_PORT						GPIOJ									// ??????????  GPIO???
#define 	GPIO_LDC_DC_CLK_ENABLE     __HAL_RCC_GPIOJ_CLK_ENABLE()	// ??????????  GPIO??? 	

#define	LCD_DC_Command		   HAL_GPIO_WritePin(LCD_DC_PORT, LCD_DC_PIN, GPIO_PIN_RESET);	   // ??????????? 
#define 	LCD_DC_Data		      HAL_GPIO_WritePin(LCD_DC_PORT, LCD_DC_PIN, GPIO_PIN_SET);		// ?????????????

/* USER CODE END Prototypes */

#ifdef __cplusplus
}
#endif

#endif /* __SPI_H__ */

