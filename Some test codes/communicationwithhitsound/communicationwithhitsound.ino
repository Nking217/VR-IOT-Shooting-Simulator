#include "SoundData.h"
#include "XT_DAC_Audio.h"

#define TRIG_PORT 14

XT_Wav_Class Sound(sample);
XT_DAC_Audio_Class DacAudio(25,0);

const char FIRECOMMAND[] = "FIRE\n";//ESP sends command to Unity to fire.
const char HITCONFIRM[] = "HIT"; //Unity sends a message to the ESP to confirm that the bullet hit the target.

bool val;
bool lastVal;

String sBuffer;

void setup() {
  Serial.begin(9600);
  pinMode(TRIG_PORT, INPUT);
}

void loop() {
  val = digitalRead(TRIG_PORT);
  if(val == HIGH && lastVal == LOW){
    Serial.print(FIRECOMMAND);
    lastVal = HIGH;
  }
  else if(val == LOW && lastVal == HIGH){
    lastVal = LOW;
  }
  
  if(Serial.available() > 0){
    sBuffer = Serial.readStringUntil('\n');
    Serial.println(sBuffer);
    if(sBuffer == HITCONFIRM){
      //Hit confirmed.
      //Play the sound.
      DacAudio.FillBuffer();
      if(Sound.Playing==false)
        DacAudio.Play(&Sound);
    }
  }
  

}
