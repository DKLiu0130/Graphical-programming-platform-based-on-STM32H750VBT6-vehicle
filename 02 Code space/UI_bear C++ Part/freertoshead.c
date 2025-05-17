/* USER CODE BEGIN Header */
/**
  ******************************************************************************
  * File Name          : freertos.c
  * Description        : Code for freertos applications
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

/* Includes ------------------------------------------------------------------*/
#include "FreeRTOS.h"
#include "task.h"
#include "main.h"
#include "cmsis_os.h"

/* Private includes ----------------------------------------------------------*/
/* USER CODE BEGIN Includes */
#include "IMU.h"
#include "motor.h"
#include "tim.h"
#include "spi.h"
#include "keyboard.h"
#include "config.h"
#include "car_attitude.h"
#include "car_control.h"
#include "uart_fifo.h"

/* USER CODE END Includes */

/* Private typedef -----------------------------------------------------------*/
typedef StaticTask_t osStaticThreadDef_t;
/* USER CODE BEGIN PTD */

/* USER CODE END PTD */

/* Private define ------------------------------------------------------------*/
/* USER CODE BEGIN PD */

/* USER CODE END PD */

/* Private macro -------------------------------------------------------------*/
/* USER CODE BEGIN PM */

/* USER CODE END PM */

/* Private variables ---------------------------------------------------------*/
/* USER CODE BEGIN Variables */
_Bool Is_Speed_Enc_Update=0;
extern float motion6[7];
extern float ypr[3];          // yaw pitch roll
extern int math_pl;
/* USER CODE END Variables */
/* Definitions for LEDBlink */
osThreadId_t LEDBlinkHandle;
uint32_t LEDBlinkBuffer[ 128 ];
osStaticThreadDef_t LEDBlinkControlBlock;
const osThreadAttr_t LEDBlink_attributes = {
  .name = "LEDBlink",
  .cb_mem = &LEDBlinkControlBlock,
  .cb_size = sizeof(LEDBlinkControlBlock),
  .stack_mem = &LEDBlinkBuffer[0],
  .stack_size = sizeof(LEDBlinkBuffer),
  .priority = (osPriority_t) osPriorityAboveNormal,
};
/* Definitions for AppTaskCreate */
osThreadId_t AppTaskCreateHandle;
uint32_t AppTaskCreateBuffer[ 128 ];
osStaticThreadDef_t AppTaskCreateControlBlock;
const osThreadAttr_t AppTaskCreate_attributes = {
  .name = "AppTaskCreate",
  .cb_mem = &AppTaskCreateControlBlock,
  .cb_size = sizeof(AppTaskCreateControlBlock),
  .stack_mem = &AppTaskCreateBuffer[0],
  .stack_size = sizeof(AppTaskCreateBuffer),
  .priority = (osPriority_t) osPriorityNormal1,
};
/* Definitions for UartHandle */
osThreadId_t UartHandleHandle;
uint32_t UartHandleBuffer[ 128 ];
osStaticThreadDef_t UartHandleControlBlock;
const osThreadAttr_t UartHandle_attributes = {
  .name = "UartHandle",
  .cb_mem = &UartHandleControlBlock,
  .cb_size = sizeof(UartHandleControlBlock),
  .stack_mem = &UartHandleBuffer[0],
  .stack_size = sizeof(UartHandleBuffer),
  .priority = (osPriority_t) osPriorityNormal7,
};
/* Definitions for _9axisService */
osThreadId_t _9axisServiceHandle;
uint32_t _9axisServiceBuffer[ 128 ];
osStaticThreadDef_t _9axisServiceControlBlock;
const osThreadAttr_t _9axisService_attributes = {
  .name = "_9axisService",
  .cb_mem = &_9axisServiceControlBlock,
  .cb_size = sizeof(_9axisServiceControlBlock),
  .stack_mem = &_9axisServiceBuffer[0],
  .stack_size = sizeof(_9axisServiceBuffer),
  .priority = (osPriority_t) osPriorityNormal3,
};
/* Definitions for CarAttitude */
osThreadId_t CarAttitudeHandle;
uint32_t CarAttitudeBuffer[ 128 ];
osStaticThreadDef_t CarAttitudeControlBlock;
const osThreadAttr_t CarAttitude_attributes = {
  .name = "CarAttitude",
  .cb_mem = &CarAttitudeControlBlock,
  .cb_size = sizeof(CarAttitudeControlBlock),
  .stack_mem = &CarAttitudeBuffer[0],
  .stack_size = sizeof(CarAttitudeBuffer),
  .priority = (osPriority_t) osPriorityNormal5,
};
/* Definitions for Key */
osThreadId_t KeyHandle;
uint32_t KeyBuffer[ 128 ];
osStaticThreadDef_t KeyControlBlock;
const osThreadAttr_t Key_attributes = {
  .name = "Key",
  .cb_mem = &KeyControlBlock,
  .cb_size = sizeof(KeyControlBlock),
  .stack_mem = &KeyBuffer[0],
  .stack_size = sizeof(KeyBuffer),
  .priority = (osPriority_t) osPriorityNormal6,
};
/* Definitions for Buzzer */
osThreadId_t BuzzerHandle;
uint32_t BuzzerBuffer[ 128 ];
osStaticThreadDef_t BuzzerControlBlock;
const osThreadAttr_t Buzzer_attributes = {
  .name = "Buzzer",
  .cb_mem = &BuzzerControlBlock,
  .cb_size = sizeof(BuzzerControlBlock),
  .stack_mem = &BuzzerBuffer[0],
  .stack_size = sizeof(BuzzerBuffer),
  .priority = (osPriority_t) osPriorityNormal1,
};
/* Definitions for Screen_LCD */
osThreadId_t Screen_LCDHandle;
uint32_t Screen_LCDBuffer[ 128 ];
osStaticThreadDef_t Screen_LCDControlBlock;
const osThreadAttr_t Screen_LCD_attributes = {
  .name = "Screen_LCD",
  .cb_mem = &Screen_LCDControlBlock,
  .cb_size = sizeof(Screen_LCDControlBlock),
  .stack_mem = &Screen_LCDBuffer[0],
  .stack_size = sizeof(Screen_LCDBuffer),
  .priority = (osPriority_t) osPriorityNormal1,
};
/* Definitions for queue_key */
osMessageQueueId_t queue_keyHandle;
const osMessageQueueAttr_t queue_key_attributes = {
  .name = "queue_key"
};
/* Definitions for semphr_buzzer_trigger */
osSemaphoreId_t semphr_buzzer_triggerHandle;
const osSemaphoreAttr_t semphr_buzzer_trigger_attributes = {
  .name = "semphr_buzzer_trigger"
};
/* Definitions for semphr_uart_receive */
osSemaphoreId_t semphr_uart_receiveHandle;
const osSemaphoreAttr_t semphr_uart_receive_attributes = {
  .name = "semphr_uart_receive"
};

/* Private function prototypes -----------------------------------------------*/
/* USER CODE BEGIN FunctionPrototypes */
void Task_APP();
void Task_exam_24H(void *argument);
void Task_AtoB(void *argument);
void Task_ABCD(void *argument);
void Task_Round(void *argument);
void Task_Round_4(void *argument);
/* USER CODE END FunctionPrototypes */

void Task_LEDBlink(void *argument);
void Task_AppTaskCreate(void *argument);
void Task_UartHandle(void *argument);
void Task_9axisService(void *argument);
void Task_CarAttitude(void *argument);
void Task_Key(void *argument);
void Task_Buzzer(void *argument);
void Task_Screen(void *argument);

void MX_FREERTOS_Init(void); /* (MISRA C 2004 rule 8.1) */

/**
  * @brief  FreeRTOS initialization
  * @param  None
  * @retval None
  */
void MX_FREERTOS_Init(void) {
  /* USER CODE BEGIN Init */

  /* USER CODE END Init */

  /* USER CODE BEGIN RTOS_MUTEX */
  /* add mutexes, ... */
  /* USER CODE END RTOS_MUTEX */

  /* Create the semaphores(s) */
  /* creation of semphr_buzzer_trigger */
  semphr_buzzer_triggerHandle = osSemaphoreNew(1, 1, &semphr_buzzer_trigger_attributes);

  /* creation of semphr_uart_receive */
  semphr_uart_receiveHandle = osSemaphoreNew(16, 0, &semphr_uart_receive_attributes);

  /* USER CODE BEGIN RTOS_SEMAPHORES */
  /* add semaphores, ... */
  /* USER CODE END RTOS_SEMAPHORES */

  /* USER CODE BEGIN RTOS_TIMERS */
  /* start timers, add new ones, ... */
  /* USER CODE END RTOS_TIMERS */

  /* Create the queue(s) */
  /* creation of queue_key */
  queue_keyHandle = osMessageQueueNew (1, sizeof(char), &queue_key_attributes);

  /* USER CODE BEGIN RTOS_QUEUES */
  /* add queues, ... */
  /* USER CODE END RTOS_QUEUES */

  /* Create the thread(s) */
  /* creation of LEDBlink */
  LEDBlinkHandle = osThreadNew(Task_LEDBlink, NULL, &LEDBlink_attributes);

  /* creation of AppTaskCreate */
  AppTaskCreateHandle = osThreadNew(Task_AppTaskCreate, NULL, &AppTaskCreate_attributes);

  /* creation of UartHandle */
  UartHandleHandle = osThreadNew(Task_UartHandle, NULL, &UartHandle_attributes);

  /* creation of _9axisService */
  _9axisServiceHandle = osThreadNew(Task_9axisService, NULL, &_9axisService_attributes);

  /* creation of CarAttitude */
  CarAttitudeHandle = osThreadNew(Task_CarAttitude, NULL, &CarAttitude_attributes);

  /* creation of Key */
  KeyHandle = osThreadNew(Task_Key, NULL, &Key_attributes);

  /* creation of Buzzer */
  BuzzerHandle = osThreadNew(Task_Buzzer, NULL, &Buzzer_attributes);

  /* creation of Screen_LCD */
  Screen_LCDHandle = osThreadNew(Task_Screen, NULL, &Screen_LCD_attributes);

  /* USER CODE BEGIN RTOS_THREADS */
	//Key_StartScanTask();
	xTaskCreate(Task_APP,"Test",512,NULL,(osPriority_t) osPriorityNormal5,NULL);
	//xTaskCreate(Task_exam_24H,"24_H",512,NULL,(osPriority_t) osPriorityNormal5,NULL);
  /* add threads, ... */
  /* USER CODE END RTOS_THREADS */

  /* USER CODE BEGIN RTOS_EVENTS */
  /* add events, ... */
  /* USER CODE END RTOS_EVENTS */

}

/* USER CODE BEGIN Header_Task_LEDBlink */
/**
  * @brief  Function implementing the LEDBlink thread.
  * @param  argument: Not used
  * @retval None
  */
/* USER CODE END Header_Task_LEDBlink */
void Task_LEDBlink(void *argument)
{
  /* USER CODE BEGIN Task_LEDBlink */
  /* Infinite loop */
  for(;;)
  { 
		HAL_GPIO_TogglePin(LED2_GPIO_Port, LED2_Pin);
    osDelay(1000);
  }
  /* USER CODE END Task_LEDBlink */
}

/* USER CODE BEGIN Header_Task_AppTaskCreate */
/**
* @brief Function implementing the AppTaskCreate thread.
* @param argument: Not used
* @retval None
*/
/* USER CODE END Header_Task_AppTaskCreate */
void Task_AppTaskCreate(void *argument)
{
  /* USER CODE BEGIN Task_AppTaskCreate */
  /* Infinite loop */
  for(;;)
  {
    osDelay(1);
  }
  /* USER CODE END Task_AppTaskCreate */
}

/* USER CODE BEGIN Header_Task_UartHandle */
/**
* @brief Function implementing the UartHandle thread.
* @param argument: Not used
* @retval None
*/
/* USER CODE END Header_Task_UartHandle */
void Task_UartHandle(void *argument)
{
  /* USER CODE BEGIN Task_UartHandle */
  /* Infinite loop */
  for(;;)
  {
		
    osDelay(1);
  }
  /* USER CODE END Task_UartHandle */
}

/* USER CODE BEGIN Header_Task_9axisService */
/**
* @brief Function implementing the _9axisService thread.
* @param argument: Not used
* @retval None
*/
/* USER CODE END Header_Task_9axisService */
void Task_9axisService(void *argument)
{
  /* USER CODE BEGIN Task_9axisService */
	TickType_t xLastWakeTime;
	const TickType_t xFrequency = TASK_ITV_IMU;
	xLastWakeTime = xTaskGetTickCount();
  /* Infinite loop */
  for(;;)
  {
		vTaskDelayUntil( &xLastWakeTime, xFrequency );
	  uint8_t data[8] = {1,2,3,4,5,6,7,8};
	  IMU_getYawPitchRoll(ypr);
		IMU_TT_getgyro(motion6);
		//printf("acc(mg):%.2f\t%.2f\t%.2f\tgyro(dps):%.2f\t%.2f\t%.2f\n", motion6[0], motion6[1], motion6[2], motion6[3], motion6[4], motion6[5]);
		math_pl++;	
		Car_Attitude_Yaw_Update(car_attitude.current_v_angle,TASK_ITV_IMU*0.001);
  }
  /* USER CODE END Task_9axisService */
}

/* USER CODE BEGIN Header_Task_CarAttitude */
/**
* @brief Function implementing the CarAttitude thread.
* @param argument: Not used
* @retval None
*/
/* USER CODE END Header_Task_CarAttitude */
void Task_CarAttitude(void *argument)
{
  /* USER CODE BEGIN Task_CarAttitude */
	TickType_t xLastWakeTime;
	const TickType_t xFrequency = TASK_ITV_CAR;
	xLastWakeTime = xTaskGetTickCount();
  /* Infinite loop */
  for(;;)
  {
		vTaskDelayUntil( &xLastWakeTime, xFrequency );
		Tim_Update_Enc_Speed();//
    Is_Speed_Enc_Update=1;//
 	  Motor_Update_Input_All();
		Car_Attitude_Update_Input();
		Car_Control_Update_Input();
		Car_Control_Update_Output();
		Car_Attitude_Update_Output();
 		Motor_Update_Output_All();				
  }
  /* USER CODE END Task_CarAttitude */
}

/* USER CODE BEGIN Header_Task_Key */
/**
* @brief Function implementing the Key thread.
* @param argument: Not used
* @retval None
*/
/* USER CODE END Header_Task_Key */
void Task_Key(void *argument)
{
  /* USER CODE BEGIN Task_Key */
	char key;
  /* Infinite loop */
  for(;;)
  {
		if(xQueueReceive(queue_keyHandle, &key, portMAX_DELAY)) {
            //
            //printf("Pressed: %c\n", key);     
				//LCD_DisplayChar(10,200,key);
			  if(key == '1')
				{
					key_callback1();
				}
        }
    osDelay(1);
  }
  /* USER CODE END Task_Key */
}

/* USER CODE BEGIN Header_Task_Buzzer */
/**
* @brief Function implementing the Buzzer thread.
* @param argument: Not used
* @retval None
*/
/* USER CODE END Header_Task_Buzzer */
void Task_Buzzer(void *argument)
{
  /* USER CODE BEGIN Task_Buzzer */
  /* Infinite loop */
  for(;;)
  {
    osDelay(1);
  }
  /* USER CODE END Task_Buzzer */
}

/* USER CODE BEGIN Header_Task_Screen */
/**
* @brief Function implementing the Screen thread.
* @param argument: Not used
* @retval None
*/
/* USER CODE END Header_Task_Screen */
void Task_Screen(void *argument)
{
  /* USER CODE BEGIN Task_Screen */
  /* Infinite loop */
  for(;;)
  {
	/*	if (Is_Speed_Enc_Update==1) 
		 {   
		  LCD_DisplayNumber(10, 10, motor_LeftFront.EncSource, 3);
		  LCD_DisplayNumber(10, 40, motor_LeftFront.duty, 3);//PWM
		  LCD_DisplayNumber(10, 60, motor_LeftFront.v_real, 3); //mm/s
			  //
		  LCD_DisplayNumber(10, 80, motor_LeftRear.EncSource, 3);  
		  LCD_DisplayNumber(10, 100, motor_LeftRear.duty, 3);
		  LCD_DisplayNumber(10, 120, motor_LeftRear.v_real, 3); 
        //��ǰ��
		  LCD_DisplayNumber(100, 10, motor_RightFront.EncSource, 3);  
		  LCD_DisplayNumber(100, 40, motor_RightFront.duty, 3);
		  LCD_DisplayNumber(100, 60, motor_RightFront.v_real, 3); 
			  
		  LCD_DisplayNumber(100, 80, motor_RightRear.EncSource, 3);  
		  LCD_DisplayNumber(100, 100, motor_RightRear.duty, 3);
		  LCD_DisplayNumber(100, 120, motor_RightRear.v_real, 3); 
			   	  
		  Is_Speed_Enc_Update= 0;  //
        }
		*/	 
    osDelay(100);
  }
  /* USER CODE END Task_Screen */
}

/* Private application code --------------------------------------------------*/
/* USER CODE BEGIN Application */
uint8_t Submission;
#define MODE_DEFAULT 0 
#define MODE_NUMBER 1 
#define MODE_TRAFFIC 2
