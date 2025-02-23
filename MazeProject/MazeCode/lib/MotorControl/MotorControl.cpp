// MotorControl.cpp
#include "MotorControl.h"
#include "Arduino.h"
#include "../communs.h"


MotorControl::MotorControl(){

}

void MotorControl::Init(){

  // Set all the motor control inputs to OUTPUT
  pinMode(MOT_A1_PIN, OUTPUT);
  pinMode(MOT_A2_PIN, OUTPUT);
  pinMode(MOT_B1_PIN, OUTPUT);
  pinMode(MOT_B2_PIN, OUTPUT);

  // Turn off motors - Initial state
  digitalWrite(MOT_A1_PIN, LOW);
  digitalWrite(MOT_A2_PIN, LOW);
  digitalWrite(MOT_B1_PIN, LOW);
  digitalWrite(MOT_B2_PIN, LOW);

}

void MotorControl::GoForward(){

    digitalWrite(MOT_A1_PIN, LOW);
    analogWrite(MOT_A2_PIN, speed);
    digitalWrite(MOT_B1_PIN, LOW);
    analogWrite(MOT_B2_PIN, speed);
}
void MotorControl::GoBack(){
    digitalWrite(MOT_A1_PIN, speed);
    analogWrite(MOT_A2_PIN, LOW);
    digitalWrite(MOT_B1_PIN, speed);
    analogWrite(MOT_B2_PIN, LOW);
}

void MotorControl::TurnLeft(){
    digitalWrite(MOT_A1_PIN, LOW);
    analogWrite(MOT_A2_PIN, speed);
    digitalWrite(MOT_B1_PIN, LOW);
    analogWrite(MOT_B2_PIN, speed/2);
}
void MotorControl::TurnRight(){
    digitalWrite(MOT_A1_PIN, LOW);
    analogWrite(MOT_A2_PIN, speed/2);
    digitalWrite(MOT_B1_PIN, LOW);
    analogWrite(MOT_B2_PIN, speed);
}
void MotorControl::TurnFullLeft(){
    digitalWrite(MOT_A1_PIN, speed);
    analogWrite(MOT_A2_PIN, speed);
    digitalWrite(MOT_B1_PIN, LOW);
    analogWrite(MOT_B2_PIN, LOW);
}
void MotorControl::TurnFullRight(){
    digitalWrite(MOT_A1_PIN, LOW);
    analogWrite(MOT_A2_PIN, LOW);
    digitalWrite(MOT_B1_PIN, speed);
    analogWrite(MOT_B2_PIN, speed);
}
void MotorControl::TurnRightRight(){
    digitalWrite(MOT_A1_PIN, LOW);
    analogWrite(MOT_A2_PIN, LOW);
    digitalWrite(MOT_B1_PIN, 80);
    analogWrite(MOT_B2_PIN, 80);

}
void MotorControl::TurnLeftLeft(){
    digitalWrite(MOT_A1_PIN, 80);
    analogWrite(MOT_A2_PIN, 80);
    digitalWrite(MOT_B1_PIN, LOW);
    analogWrite(MOT_B2_PIN, LOW);

}


void MotorControl::Stop(){
  digitalWrite(MOT_A1_PIN, LOW);
  analogWrite(MOT_A2_PIN, LOW);
  digitalWrite(MOT_B1_PIN, LOW);
  analogWrite(MOT_B2_PIN, LOW);
}