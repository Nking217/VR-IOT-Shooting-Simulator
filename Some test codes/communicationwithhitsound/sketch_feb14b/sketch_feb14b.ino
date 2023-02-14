#define LIMIT_SWITCH_PIN 14
unsigned long lmswPressTime = 0;
int delayPeriod = 500, cnt = 0;
bool debounceDone = false;
void setup(){
  Serial.begin(115200);
  pinMode(LIMIT_SWITCH_PIN, INPUT);
  
}


void loop(){
  if(millis() - lmswPressTime > lmswPressTime)
    debounceDone = true;
  if(digitalRead(LIMIT_SWITCH_PIN) == HIGH && debounceDone == true){
    lmswPressTime = millis();
    debounceDone = false;
    cnt++;
    Serial.println(cnt);
    Serial.println("FIRE");
  }
}
