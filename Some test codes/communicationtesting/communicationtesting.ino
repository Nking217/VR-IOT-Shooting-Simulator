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
#define BUTTON_PIN 4
void setup(){
  Serial.begin(9600);
  pinMode(BUTTON_PIN, INPUT);
  Serial2.begin(9600,SERIAL_8N1,RXD2,TXD2); //Serial port for debuggin on a seprate Arduino microcontroller
  _State = STATE_DISCONNECTED;
  waitForConnection();
}


void loop(){
  if(Serial.available() > 0)
    sBuffer = Serial.readStringUntil('\n');

  if(_ConnectionStatus == CONNECTION_STATUS_DISCONNECTED){
    waitForConnection(); //Not running the rest of the code until ESP and Unity are connected.
  }
  
  switch(sBuffer){
    case startsWith(ISREADY):
      setState(STATE_READY);
      Serial2.println("Unity and ESP are connected and ready");
      break;
    
    case startsWith(GAMEMODE):
      Serial2.println("Game has started");
      setState(STATE_GAME);
      gameOn();
      break;
    
    case startsWith(STOPGAME):
      Serial2.println("Error: GAMESTOP Command was read on the main loop function check your code...");
      setState(STATE_READY);
      break;
  }
  
  
}

void waitForConnection(){ //Making sure that there is a connection btween Unity and the ESP
  bool Wait = true;
  Serial2.print("Waiting for connection");
  while(wait){
    if(Serial.available() > 0){ //if there is more than 0 bit in the serial buffer;
      sBuffer = Serial.readStringUntil('\n');
      if(sBuffer == ISAVL){
        setState(STATE_CONNECTED);
        Serial.write(ACK);
        Serial2.print("Connected");
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
      else{
        /*
        if(button == 1){
          Serial.write(FIRECOMMAND);
        }
        */
        //Fire test code:
        Serial.println(FIRECOMMAND);
        //sending command to unity to fire.
        //making a haptic feedback for the Rifle.
        //making a sound feedback for the Rifle.
      }
    }
  }
}
