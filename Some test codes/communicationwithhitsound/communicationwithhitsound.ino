#include "SoundData.h"
#include "XT_DAC_Audio.h"

#define TRIG_PORT 14

XT_Wav_Class Sound(sample);
XT_DAC_Audio_Class DacAudio(25,0);
String FIRECOMMAND = "FIRE"; //ESP sends command to Unity to fire. *String testing
String HITCONFIRM = "HIT"; //Unity sends a message to the ESP to confirm that the bullet hit the target. *String testing

//const char FIRECOMMAND[] = "FIRE";//ESP sends command to Unity to fire.
//const char HITCONFIRM[] = "HIT"; //Unity sends a message to the ESP to confirm that the bullet hit the target.

bool val;
bool lastVal;

String sBuffer;

void setup() {
  Serial.begin(115200);
  pinMode(TRIG_PORT, INPUT);
  delay(5000);
  Serial.println();
  Serial.println("START");
}

void loop() {
  DacAudio.FillBuffer(); //Load the sound buffer every time even if we are not playing any sound.
  val = digitalRead(TRIG_PORT);
  if(val == HIGH && lastVal == LOW){
    Serial.println(FIRECOMMAND);
    lastVal = HIGH;
  }
  else if(val == LOW && lastVal == HIGH){
    lastVal = LOW;
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
