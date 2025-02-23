// MVTController.cpp
#include "MVTController.h"
#include <RGBController.h>
#include "../communs.h"
#include <MotorControl.h>

void MVTController::Init(){
    curentExcecStep = 0;
    curentSaveStep = 0;
    WorkingMode = Idle;
    rgb.Init();
    rgb.SetStaticColor(rgb.strip.Color(0,0,0));
    rgb.strip.setBrightness(100);  
    motors.Init();
}
MVTController::MVTController() : rgb(),sound(),motors(){

}
unsigned long previousMillis = 0; // Store the last time the timer was updated
const unsigned long interval = 1000; // 10 seconds (10000 milliseconds)

void MVTController::Execute(){
    if(WorkingMode == Runing){
        unsigned long currentMillis = millis();
        if (currentMillis - previousMillis >= interval) {
            previousMillis = currentMillis; 
            if(curentExcecStep<curentSaveStep)
            {
                Serial.println(mvtTable[curentExcecStep]) ;   
                switch(mvtTable[curentExcecStep]){
                    case 1:
                        motors.GoForward();
                        break;
                    case 2:
                        motors.GoBack();
                        break;
                    case 3:
                        motors.TurnFullLeft();
                        break;
                    case 4:
                        motors.TurnFullRight();
                        break;
                    default:
                        motors.Stop();
                        break;
                }
                curentExcecStep++;   
            }
            else
            {
                WorkingMode = Done;
                rgb.SetStaticColor(rgb.strip.Color(255,0,0));  
                motors.Stop();
            }
        }

    }
}



void MVTController::ClearTable(){
    curentExcecStep = 0;
    curentSaveStep = 0;
    WorkingMode = Idle;
    for(int i=0;i<MaxMouvment;i++){
        mvtTable[i] = Empty;
    }
}


void MVTController::EnterProgrammingMode()
{
    if(WorkingMode == Learning)return;
    ClearTable();
    rgb.SetStaticColor(rgb.strip.Color(255,255,255));  
    WorkingMode = Learning;
    sound.PlayCanChangeMode();
}

void  MVTController::AddMvt(mvt mvtStep){
    if(WorkingMode == Learning && curentSaveStep < MaxMouvment){
        mvtTable[curentSaveStep] = mvtStep;
        curentSaveStep++;
        sound.PlayChangeMode();
    }
}

void MVTController::StartExecuting()
{
    if(WorkingMode == Learning || WorkingMode == Done){
        WorkingMode = Runing;
        curentExcecStep =0;
        sound.PlayCantChangeMode();
        rgb.SetStaticColor(rgb.strip.Color(0,0,255));  
    }
}