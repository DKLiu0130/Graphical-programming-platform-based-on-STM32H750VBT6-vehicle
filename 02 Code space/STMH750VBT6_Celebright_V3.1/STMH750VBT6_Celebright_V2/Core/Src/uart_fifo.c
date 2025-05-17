#include "uart_fifo.h"
#include <stdio.h>

extern UART_HandleTypeDef huart1;
uint8_t uart_rx_byte; 


uint8_t uart_received_frame[MAX_FRAME_SIZE * 8]; 
uint16_t uart_received_frame_length = 0; 
uint8_t uart_tx_buffer[MAX_FRAME_SIZE + 2];  // ?????
uint16_t uart_tx_length = 0; 

void UART_Init(void) {
    HAL_UART_Receive_IT(&huart1, &uart_rx_byte, 1);
}


// This function is used to deconstruct the received data frame, 
// removing the frame header and footer, and dividing the middle data into bits.
void HAL_UART_RxCpltCallback(UART_HandleTypeDef *huart) {
	//HAL_GPIO_TogglePin(GPIOC, GPIO_PIN_13);
    if (huart->Instance == USART1) {
        static uint8_t frame_data[MAX_FRAME_SIZE];  // ??????
        static uint16_t frame_index = 0;
        
        if (uart_rx_byte == FRAME_HEADER) {
            frame_index = 0;  // ??????????
        }

        if (frame_index < sizeof(frame_data)) {
            frame_data[frame_index++] = uart_rx_byte;
        }

        if (uart_rx_byte == FRAME_TAIL) {
            uart_received_frame_length = frame_index * 8;  // ?? bit ??

            for (uint16_t i = 0; i < frame_index; i++) {
                uint8_t byte = frame_data[i];
                for (uint8_t bit = 0; bit < 8; bit++) {
                    uart_received_frame[i * 8 + bit] = (byte >> (7 - bit)) & 0x01;
                }
            }

        }

        HAL_UART_Receive_IT(&huart1, &uart_rx_byte, 1);
    }
}

void UART_SendFrame(uint8_t *bit_data, uint16_t bit_length) {
    if (bit_length % 8 != 0) {
        printf("Error: Bit length must be a multiple of 8!\n");
        return;
    }

    uint16_t byte_length = bit_length / 8;  

    uart_tx_buffer[0] = FRAME_HEADER;

    for (uint16_t i = 0; i < byte_length; i++) {
        uart_tx_buffer[i + 1] = 0;
        for (uint8_t k = 0; k < 8; k++) {
            uart_tx_buffer[i + 1] |= (bit_data[i * 8 + k] & 0x01) << (7 - k);
        }
    }

    uart_tx_buffer[byte_length + 1] = FRAME_TAIL;

    uart_tx_length = byte_length + 2;

    HAL_UART_Transmit_IT(&huart1, uart_tx_buffer, uart_tx_length);
}

void HAL_UART_TxCpltCallback(UART_HandleTypeDef *huart) {
    if (huart->Instance == USART1) {
        printf("UART Transmission Complete!\n");
    }
}
