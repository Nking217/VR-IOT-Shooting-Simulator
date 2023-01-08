/*
  Rui Santos
  Complete project details at our blog.
    - ESP32: https://RandomNerdTutorials.com/esp32-firebase-realtime-database/
    - ESP8266: https://RandomNerdTutorials.com/esp8266-nodemcu-firebase-realtime-database/
  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files.
  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
  Based in the RTDB Basic Example by Firebase-ESP-Client library by mobizt
  https://github.com/mobizt/Firebase-ESP-Client/blob/main/examples/RTDB/Basic/Basic.ino
*/

#include <Arduino.h>
#if defined(ESP32)
  #include <WiFi.h>
#elif defined(ESP8266)
  #include <ESP8266WiFi.h>
#endif
#include <Firebase_ESP_Client.h>

//Provide the token generation process info.
#include "addons/TokenHelper.h"
//Provide the RTDB payload printing info and other helper functions.
#include "addons/RTDBHelper.h"


#include "SoundData.h"
#include "XT_DAC_Audio.h"

#define TRIG_PORT 14
XT_Wav_Class Sound(sample);
XT_DAC_Audio_Class DacAudio(25,0);
String FIRECOMMAND = "FIRE"; //ESP sends command to Unity to fire. *String testing
String HITCONFIRM = "HIT"; //Unity sends a message to the ESP to confirm that the bullet hit the target. *String testing


// Insert your network credentials
#define WIFI_SSID "Kinneret College"
#define WIFI_PASSWORD ""

// Insert Firebase project API Key
#define API_KEY "AIzaSyBJx_urLrpl3-snrVJVi965m-vJ_4E8DN0"

// Insert RTDB URLefine the RTDB URL */
#define DATABASE_URL "https://esp-test-f6c1a-default-rtdb.europe-west1.firebasedatabase.app/" 

//Define Firebase Data object
FirebaseData fbdo;

FirebaseAuth auth;
FirebaseConfig config;

unsigned long sendDataPrevMillis = 0;
int intValue;
float floatValue;
bool signupOK = false;

bool val;
bool lastVal;

int wason;

String sBuffer;


void setup() {
  Serial.begin(115200);
  WiFi.begin(WIFI_SSID, WIFI_PASSWORD);
  Serial.print("Connecting to Wi-Fi");
  while (WiFi.status() != WL_CONNECTED) {
    Serial.print(".");
    delay(300);
  }
  Serial.println();
  Serial.print("Connected with IP: ");
  Serial.println(WiFi.localIP());
  Serial.println();
  pinMode(TRIG_PORT, INPUT);
  


  /* Assign the api key (required) */
  config.api_key = API_KEY;

  /* Assign the RTDB URL (required) */
  config.database_url = DATABASE_URL;

  /* Sign up */
  if (Firebase.signUp(&config, &auth, "", "")) {
    Serial.println("ok");
    signupOK = true;
  }
  else {
    Serial.printf("%s\n", config.signer.signupError.message.c_str());
  }

  /* Assign the callback function for the long running token generation task */
  config.token_status_callback = tokenStatusCallback; //see addons/TokenHelper.h

  Firebase.begin(&config, &auth);
  Firebase.reconnectWiFi(true);
  Serial.println();
}

void loop() {
  DacAudio.FillBuffer(); 
  if (Firebase.ready() && signupOK && (millis() - sendDataPrevMillis > 1000 || sendDataPrevMillis == 0)) {
    sendDataPrevMillis = millis();
    if (Firebase.RTDB.getInt(&fbdo, "/test/int")) {
      if (fbdo.dataType() == "int") {
        intValue = fbdo.intData();
        //Serial.println(intValue);
      }
    }
    else {
      Serial.println(fbdo.errorReason());
    }
    
    if (Firebase.RTDB.getFloat(&fbdo, "/test/float")) {
      if (fbdo.dataType() == "float") {
        floatValue = fbdo.floatData();
        //Serial.println(floatValue);
      }
    }
    else {
      Serial.println(fbdo.errorReason());
    }

  }
  if(intValue == 1){
    if(wason == 0){
      Serial.println("START");
      wason = 1;
    }
    
    val = digitalRead(TRIG_PORT);
    if(val == HIGH && lastVal == LOW){
      Serial.println(FIRECOMMAND);
      lastVal = HIGH;
      //delay(50);
    }
    else if(val == LOW && lastVal == HIGH){
      lastVal = LOW;
      //delay(50);
    }

    if(Serial.available() > 0){
      sBuffer = Serial.readStringUntil('\n');
      if(sBuffer == HITCONFIRM){
        if(Sound.Playing==false)
          DacAudio.Play(&Sound);
      }
    }

  }
  else if(intValue == 0){
    if(wason == 1){
      Serial.println("STOP");
      wason = 0;
    }
  }
}
