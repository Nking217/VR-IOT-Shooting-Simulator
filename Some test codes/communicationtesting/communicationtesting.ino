/// - Command prefix - /// (Not sure)
// c - Innisiate the connection btween the ESP and Unity.
// g - Game, game mode - Unity is waiting for incoming bytes from the ESP, until Unity sends s (stop command).
// s - Stop - stopping the game mode on the ESP (stop sending bytes every time the button is pressed).
// e - End - ending the connection between the ESP and Unity.
//////////////////////////
#define STATE_CONNECTED 0
#define STATE_READY 1
#define STATE_GAME 2
#define STATE_DISCONNECTED 3

const char SOK[] = "OK\n";
const char ISAVL[] = "ISAVAILABLE"; //Unity sends message to the ESP, to make sure they both connected.
const char ISREADY[] = "READY"; //
const char GAMEMODE[] = "GAME";
const char STOPGAME[] = "GAME_STOP";

String sBuffer;
int _State;

void setup(){
  Serial.begin(9600);
  waitForConnection();
}


void loop(){
  if(Serial.available() > 0){
  sBuffer = Serial.readStringUntil('\n');

  switch(sBuffer){
    case startsWith(ISREADY){
            
    }
  }
  
  
}

void waitForConnection(){ //Making sure that there is a connection btween Unity and the ESP
  bool Wait = true;
  while(wait){
    if(Serial.available() > 0){ //if there is more than 0 bit in the serial buffer;
      sBuffer = Serial.readStringUntil('\n');
      if(sBuffer == ISAVL){
        Serial.write(ACK);
        Wait = false;
      }
    }
  }
}

void setState(int state){
  _State = state;
  
}
