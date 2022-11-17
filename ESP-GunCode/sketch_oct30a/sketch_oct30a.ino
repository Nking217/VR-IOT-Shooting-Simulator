////////////////////////////////////////////////////
////// -- ESP32 VR Shooting Simulator IOT  -- //////
////////////////////////////////////////////////////
#define TRIGGER_PIN 4
#define VIBRATE_PIN
#define 




bool val;
bool lastVal;
void setup() {
  Serial.begin(115200);
  pinMode(4, INPUT);
}

void loop() {
  val = digitalRead(4);
  if(val = HIGH && lastVal = LOW){
    Serial.println("1");
    lastVal = HIGH;
  }
  else if(val = LOW && lastVal = HIGH){
    lastVal = LOW;
  }
}
