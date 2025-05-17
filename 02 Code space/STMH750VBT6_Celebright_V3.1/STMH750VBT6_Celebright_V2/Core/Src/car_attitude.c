/**
 * @file car_control.c
 * @author cbr (ran13245@outlook.com)
 * @brief 小车基本姿态代码
 * @version 0.1
 * @date 2023-06-25
 * 
 * @copyright Copyright (c) 2023
 * 
 */

#include <math.h>
#include "car_attitude.h"
#include "car_control.h"
#include "config.h"
#include "motor.h"
#include "IMU.h"
_car_attitude car_attitude={0};//car_attitude内变量全部初始化为0


/*!
 * @brief 初始化小车姿态
 * @param  
 */
void init_Car_Attitude(void){
    //限幅常量，后续调整
    Set_PID_Limit(&car_attitude.pid_v_angle,LIMIT_INC_V_ANGLE,LIMIT_POS_V_ANGLE,LIMIT_ITGR_V_ANGLE);
   //PID参数，后续调整
    Set_PID(&car_attitude.pid_v_angle,P_V_ANGLE,I_V_ANGLE,D_V_ANGLE);
   
}

/*!
 * @brief 更新小车当前姿态
 * @param  
 */
void Car_Attitude_Update_Input(void){
    car_attitude.updated = 1; // 标记小车姿态已更新
    float left, right;
    left = Wheel_Get_V_Real(LEFT); // 获取左轮的实际速度
    right = Wheel_Get_V_Real(RIGHT); // 获取右轮的实际速度
    car_attitude.current_v_line = 0.5F * (left + right); // 计算小车的当前直线速度，取左右轮速度的平均值
    #if V_DEGREE_FROM_IMU
        car_attitude.current_v_angle =imu_g_z; // 从IMU获取角速度
				//printf("%f\n",imu_g_z);
    #else
        car_attitude.current_v_angle = 0.5F * (right - left) * FRAME_W_HALF_REC * RAD_TO_DEGREE; // 用编码器计算小车角速度
    #endif
}

/*!
 * @brief 更新小车姿态输出,增量式PID实现角速度环
 * @param  
 */
void Car_Attitude_Update_Output(void){
		float v_left=0;
		float v_right=0;
		float v_z=0;
		if(car_attitude.flag_stop){
				v_left=0;
				v_right=0;
				v_z=0;
		}
		else {
#if 		V_ANGLE_PID
v_z=PID_Cal_Pos(&car_attitude.pid_v_angle,car_attitude.current_v_angle,car_attitude.target_v_angle);
#else
				v_z=car_attitude.target_v_angle*FRAME_W_HALF*DEGREE_TO_RAD;
#endif
				v_left=car_attitude.target_v_line-v_z;
				v_right=car_attitude.target_v_line+v_z;
		}
		Wheel_Set_V_Real(v_left,v_right);
}

/**
 * @brief 设置小车姿态
 * @param v_line_target 目标直线速度,mm/s
 * @param v_angle_target 目标角速度,degree/s
 */
void Set_Car_Attitude(float v_line_target,float v_angle_target){
    // 限制目标线速度在最大速度范围内
    v_line_target = v_line_target > MAX_V_REAL ? MAX_V_REAL : v_line_target;
    v_line_target = v_line_target < -MAX_V_REAL ? -MAX_V_REAL : v_line_target;
    
    // 限制目标角速度在最大角速度范围内
    v_angle_target = v_angle_target > MAX_V_ANGLE ? MAX_V_ANGLE : v_angle_target;
    v_angle_target = v_angle_target < -MAX_V_ANGLE ? -MAX_V_ANGLE : v_angle_target;

    // 设置小车的目标直线速度和目标角速度
    car_attitude.target_v_line = v_line_target;
    car_attitude.target_v_angle = v_angle_target;

    // 如果目标速度和角速度均为0，则设置小车停止，否则启动小车
    if (car_attitude.target_v_line == 0 && car_attitude.target_v_angle == 0) {
        Set_Car_Stop();
    } else {
        Set_Car_Start();
    }

    // 如果不使用小车控制，则清除PID控制器的状态
    #if (!USE_CAR_CONTROL)
        PID_Clear(&car_attitude.pid_v_angle);
    #endif
}


/*!
 * @brief 设置小车停止
 * @param  
 */
void Set_Car_Stop(void){
    car_attitude.flag_stop=1;
}

/*!
 * @brief 设置小车启动
 * @param  
 */
void Set_Car_Start(void){
    car_attitude.flag_stop=0;
}


/**
 * @brief 更新汽车的偏航角度
 * 
 * 该函数根据给定的角速度和时间更新汽车的偏航角度，并处理角度的循环。
 * 如果偏航角度小于0，则将其加上360度，并减少圈数。
 * 如果偏航角度大于等于360度，则将其减去360度，并增加圈数。
 * 
 * @param v_angle 角速度，单位为度/秒
 * @param time 时间，单位为秒
 */
void Car_Attitude_Yaw_Update(float v_angle,float time){
	  //printf("%.2f\n",v_angle);
    car_attitude.yaw+=v_angle*time;
    
    if(car_attitude.yaw < 0){
        car_attitude.yaw+=360.0F;
        car_control.spin_parameter.circles-=1;
    }
    if(car_attitude.yaw >= 360.0F){
        car_attitude.yaw-=360.0F;
        car_control.spin_parameter.circles+=1;
    }
}
