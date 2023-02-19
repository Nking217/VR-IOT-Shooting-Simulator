#include "SoundData.h"
#include "XT_DAC_Audio.h"
#include <ezButton.h>

//#define TRIG_PORT 14

XT_Wav_Class Sound(sample);
XT_DAC_Audio_Class DacAudio(25,0);
String FIRECOMMAND = "FIRE"; //ESP sends command to Unity to fire. *String testing
String HITCONFIRM = "HIT"; //Unity sends a message to the ESP to confirm that the bullet hit the target. *String testing

ezButton limitSwitch(14);
//const char FIRECOMMAND[] = "FIRE";//ESP sends command to Unity to fire.
//const char HITCONFIRM[] = "HIT"; //Unity sends a message to the ESP to confirm that the bullet hit the target.

//bool val;
//bool lastVal;
int laststate;
String sBuffer;

void setup() {
  Serial.begin(115200);
  //pinMode(TRIG_PORT, INPUT);
  //delay(5000);
  //Serial.println();
  Serial.println("START");
  limitSwitch.setDebounceTime(50);
}

void loop() {
  DacAudio.FillBuffer(); //Load the sound buffer every time even if we are not playing any sound.
  limitSwitch.loop();
  int state = limitSwitch.getState();
  //val = digitalRead(TRIG_PORT);
  if(state == HIGH && laststate == LOW){
    //Serial.println(FIRECOMMAND);
    //Serial.println("The limit switch: UNTOUCHED");
    laststate = HIGH;
  }
  else if(state == LOW && laststate == HIGH){
    //lastVal = LOW;
    Serial.println(FIRECOMMAND);
    //Serial.println("The limit switch: TOUCHED");
    laststate = LOW;
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
