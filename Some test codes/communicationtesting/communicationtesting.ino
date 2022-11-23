/// - Command prefix - /// (Not sure)
// c - Innisiate the connection btween the ESP and Unity.
// g - Game, game mode - Unity is waiting for incoming bytes from the ESP, until Unity sends s (stop command).
// s - Stop - stopping the game mode on the ESP (stop sending bytes every time the button is pressed).
// e - End - ending the connection between the ESP and Unity.
//////////////////////////
#define CONNECTION_STATUS_DISCONNECTED 0
#define CONNECTION_STATUS_CONNECTED 1

#define STATE_READY 1
#define STATE_GAME 2
#define STATE_DISCONNECTED 3

const char SOK[] = "OK\n"; //ESP sends message to Unity, Approves the command has recived.
const char ISAVL[] = "ISAVAILABLE"; //Unity sends message to the ESP, to make sure they both connected.
const char ISREADY[] = "READY"; //Unity sends message to the ESP (unity is on map select scene), 
const char GAMEMODE[] = "GAME"; //Unity sends message to the ESP, Starting the game.
const char STOPGAME[] = "GAME_STOP"; //Unity sends message to the ESP, Stopping the game.

const char FIRECOMMAND[] = "FIRE\n"; //ESP sends a fire command to Unity, syncronyzing the firing btween the ESP and unity

String sBuffer;
byte _State = STATE_DISCONNECTED; //Not connected to Unity until the waitforconnection function runs.
byte _ConnectionStatus = CONNECTION_STATUS_DISCONNECTED;

#define RXD2 16 //RX port for the second serial port.
#define TXD2 17 //TX port for the second serial port.

void setup(){
  Serial.begin(9600);
  //Serial2.begin(115200,RXD2,TXD2); //Serial port for debuggin on a seprate Arduino microcontroller
  _State = STATE_DISCONNECTED;
  waitForConnection();
}


void loop(){
  if(Serial.available() > 0)
    sBuffer = Serial.readStringUntil('\n');

  if(_ConnectionStatus == CONNECTION_STATUS_DISCONNECTED){
    waitForConnection();
  }
  
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
        setState(STATE_CONNECTED);
        Serial.write(ACK);
        Wait = false;
      }
    }
  }
}

void setState(int state){
  _State = state; 
}

void gameOn(){
  setState(STATE_GAME);
  String gBuffer;
  bool isGame = true;
  while(isGame){
    if(Serial.available() > 0){
      gBuffer = Serial.readStringUntil('\n');
      if(gBuffer == STOPGAME){
        isGame = false; //Ending the game loop.
        setState(STATE_READY); //Updating the state of the system.
      }
      else if(gBuffer == FIRE_COMMAND){
        //making a haptic feedback for the Rifle.
        //making a sound feedback for the Rifle.
      }
    }
  }
  
}
