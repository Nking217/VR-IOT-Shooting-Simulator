#include "SoundData.h"
#include "XT_DAC_Audio.h"
#include <ezButton.h>

#define VIBRATION_PORT 19

XT_Wav_Class Sound(sample);
XT_DAC_Audio_Class DacAudio(25,0);
String FIRECOMMAND = "FIRE"; //ESP sends command to Unity to fire. *String testing
String HITCONFIRM = "HIT"; //Unity sends a message to the ESP to confirm that the bullet hit the target. *String testing

ezButton limitSwitch(12);
//const char FIRECOMMAND[] = "FIRE";//ESP sends command to Unity to fire.
//const char HITCONFIRM[] = "HIT"; //Unity sends a message to the ESP to confirm that the bullet hit the target.

//bool val;
//bool lastVal;
int laststate;
String sBuffer;
bool bulletShot;
unsigned long lastTime = 0;

void setup() {
  Serial.begin(115200);
  //pinMode(TRIG_PORT, INPUT);
  //delay(5000);
  //Serial.println();
  pinMode(VIBRATION_PORT, OUTPUT);
  Serial.println("START");
  limitSwitch.setDebounceTime(50);

}

void loop() {
  DacAudio.FillBuffer(); //Load the sound buffer every time even if we are not playing any sound.
  limitSwitch.loop();
  int state = limitSwitch.getState();
  if(state == HIGH && laststate == LOW){
    digitalWrite(VIBRATION_PORT, LOW);
    laststate = HIGH;
  }
  else if(state == LOW && laststate == HIGH){
    Serial.println(FIRECOMMAND);
    bulletShot = HIGH;
    laststate = LOW;
  }
  if (bulletShot)
  {
    //Serial.println("bulletShot");
    digitalWrite(VIBRATION_PORT, HIGH);
    unsigned long startTime = millis();
    while (millis() - startTime < 150)
    {
      // Vibration for 5 milliseconds
    }
    //Serial.println("Delay finished");
    digitalWrite(VIBRATION_PORT, LOW);
    bulletShot = LOW;
  }
  
  if(Serial.available() > 0){
    sBuffer = Serial.readStringUntil('\n');
    //Serial.println(sBuffer);
    if(sBuffer == HITCONFIRM){
      //Hit confirmed.
      //Play the sound.
      //Serial.println("Hit confirmed");
      if(Sound.Playing==false)
        DacAudio.Play(&Sound);
    }
  }
}

/*
 * else if(state == LOW && laststate == HIGH){
    Serial.println(FIRECOMMAND);
    if(millis() - time1 >= vibrationPeriod){
      vibrationState = !vibrationState;
      time1 = millis();
    }
    laststate = LOW;
  }
  
  digitalWrite(VIBRATION_PORT, vibrationState);
 */
